
/*************************************
This script is used to import scripting so it is viewable via the ScriptViewer app.

Steps:
1) Geneate a new script release on the host system and copy the zip file to the sql server
2) Unzip the folders in the zip file into a new temp folder somewhere, make note of the folder path
4) Set the various input paraments and run

exec ImportDataOld 'C:\Users\brian.varnell\Desktop\test\', 'NYCDOF'
*************************************/

CREATE   procedure [dbo].[ImportDataOld] (@path varchar(200), @ClientName varchar(255))
as


begin

exec DeleteData @ClientName

declare @sql nvarchar(max)
declare @versionnumberfull nvarchar(50)
declare @versionnumbershort nvarchar(50)

set @ClientName = upper(@clientname)

set @sql = 
'SELECT distinct @version = concat(version, '' ('',ModifyDate, '')'')
	FROM OPENROWSET(BULK ''' + @path + 'release.json'', SINGLE_CLOB) AS raw
CROSS APPLY OPENJSON(BulkColumn, ''$.Scripts'')
WITH (
	Name NVARCHAR(100),
	Version NVARCHAR(20),
	Author NVARCHAR(100),
	ModifyDate NVARCHAR(30),
	Hash NVARCHAR(100)
) AS s;'

exec sp_executesql @sql, N'@version nvarchar(50) out', @versionnumberfull out

--print @versionnumber
--print substring(@versionnumber, 1, charindex(' ', @versionnumber)-1)

set @versionnumbershort = substring(@versionnumberfull, 1, charindex(' ', @versionnumberfull)-1)

--temp table to house scripts
CREATE TABLE #scripts(
	[ScriptLibID] [int] NOT NULL,
	[Name] [varchar](100) NOT NULL,
	[Description] [varchar](255) NULL,
	[Script] [varchar](max) NULL,
	[CreateDate] [datetime] NOT NULL,
	[ModifiedDate] [datetime] NOT NULL,
	[Version] [decimal](18, 1) NOT NULL,
	[ModifiedBy] [varchar](128) NULL,
	[LibType] [smallint] NOT NULL,
	[LibAccess] [smallint] NOT NULL,
	[Release] [varchar](20) NULL)
end

--event library info
set @sql = 'SELECT 
	JSON_VALUE(jsonRow.value, ''$.id'') AS id,
				JSON_VALUE(jsonRow.value, ''$.name'') AS name,
				JSON_VALUE(jsonRow.value, ''$.description'') AS description,
				jsonRow.value AS script,
				cast(JSON_VALUE(jsonRow.value, ''$.created'') as date) AS created,
				cast(JSON_VALUE(jsonRow.value, ''$.modified'') as date) AS modified,
				JSON_VALUE(jsonRow.value, ''$.version'') AS version,
				JSON_VALUE(jsonRow.value, ''$.modifiedBy'') AS modifiedby,
				JSON_VALUE(jsonRow.value, ''$.libraryType'') AS libraryType,
				JSON_VALUE(jsonRow.value, ''$.libraryAccess'') AS libraryAccess,
				null as Release
FROM OPENROWSET(BULK ''' + @path + 'event_libraries_' + @versionnumbershort + '.json'', SINGLE_CLOB) AS raw
CROSS APPLY OPENJSON(raw.BulkColumn) AS jsonRow'

insert into #scripts
exec (@sql)

--custom library info
set @sql = 'SELECT 
	JSON_VALUE(jsonRow.value, ''$.id'') AS id,
				JSON_VALUE(jsonRow.value, ''$.name'') AS name,
				JSON_VALUE(jsonRow.value, ''$.description'') AS description,
				jsonRow.value AS script,
				cast(JSON_VALUE(jsonRow.value, ''$.created'') as date) AS created,
				cast(JSON_VALUE(jsonRow.value, ''$.modified'') as date) AS modified,
				JSON_VALUE(jsonRow.value, ''$.version'') AS version,
				JSON_VALUE(jsonRow.value, ''$.modifiedBy'') AS modifiedby,
				JSON_VALUE(jsonRow.value, ''$.libraryType'') AS libraryType,
				JSON_VALUE(jsonRow.value, ''$.libraryAccess'') AS libraryAccess,
				null as Release
FROM OPENROWSET(BULK ''' + @path + 'custom_libraries_' + @versionnumbershort + '.json'', SINGLE_CLOB) AS raw
CROSS APPLY OPENJSON(raw.BulkColumn) AS jsonRow'

insert into #scripts
exec (@sql)

insert into ScriptVersion (ClientName, description, versionnumber, versiondate, SourceSystem)
	select @ClientName, null, @versionnumbershort,
	CASE WHEN @versionnumberfull IS NULL THEN NULL ELSE LTRIM(REPLACE(REPLACE(REPLACE(SUBSTRING(@versionnumberfull, CHARINDEX(' ', @versionnumberfull + ' ') + 1, LEN(@versionnumberfull)), ')M', ''), '(', ''), ')', '')) END, 
	'5.0'

insert into ScriptLib (ClientName, ScriptLibID, name, Description, Script, CreateDate, ModifiedDate, Version, ModifiedBy, LibType, LibAccess)
	select @clientname, ScriptLibID, name, Description, Script, CreateDate, ModifiedDate, Version, ModifiedBy, LibType, LibAccess
		from #scripts
