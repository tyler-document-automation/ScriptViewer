
/************************************
Loop through all cient dbs and import scripting data 
	into ScriptViewer db if not already present

exec ImportOldFromDBs
************************************/

CREATE   procedure [dbo].[ImportOldFromDBs] as
begin
DECLARE @DbName nvarchar(128);
declare @clientName nvarchar(255);
DECLARE @Sql nvarchar(max);

-- Cursor to loop through all user databases
DECLARE db_cursor CURSOR FOR
SELECT name, 
	upper(replace(
		case
			when name like 'QA%' then substring(name, 3, 100)
			when name like 'DEV%' then substring(name, 4, 100)
			else name
		end, 
	'_intellidact', '')) ClientName
FROM sys.databases
WHERE name like '%_intellidact%'
--and name = 'lakeilQA_intellidact'
ORDER BY name;

OPEN db_cursor;
FETCH NEXT FROM db_cursor INTO @DbName, @clientname;

WHILE @@FETCH_STATUS = 0
BEGIN
    -- Check if both required tables exist in the current database
    SET @Sql = N'USE [' + @DbName + N']; 
    IF OBJECT_ID(''dbo.scriptlib'') IS NOT NULL 
       AND OBJECT_ID(''dbo.sysconfigvalue'') IS NOT NULL 
    BEGIN
        PRINT N''Processing database: ' + @DbName + N' Client: ' + @clientname + N''';

		EXEC [ScriptViewer2].[dbo].[Deletedata] @clientname = N''' + @clientName + N''';

		INSERT INTO [ScriptViewer2].[dbo].[ScriptLib]
           ([ClientName]
           ,[ScriptLibID]
           ,[Name]
           ,[Description]
           ,[Script]
           ,[CreateDate]
           ,[ModifiedDate]
           ,[Version]
           ,[ModifiedBy]
           ,[LibType]
           ,[LibAccess]
           ,[Release])
		SELECT ''' + @clientName + ''', * FROM [' + @DbName + N'].dbo.scriptlib;

		INSERT INTO [ScriptViewer2].[dbo].[ScriptVersion]
           ([ClientName]
           ,[description]
           ,[versionnumber]
           ,[versionDate]
           ,[SourceSystem])
		SELECT ''' + @clientName + ''', 
		null, 
		CASE WHEN value_data IS NULL THEN NULL ELSE LEFT(value_data, CHARINDEX('' '', value_data + '' '') - 1) END, 
		CASE WHEN value_data IS NULL THEN NULL ELSE LTRIM(REPLACE(REPLACE(REPLACE(SUBSTRING(value_data, CHARINDEX('' '', value_data + '' '') + 1, LEN(value_data)), '')M'', ''''), ''('', ''''), '')'', '''')) END, 
		5.0 
		FROM [' + @DbName + N'].dbo.sysconfigvalue
        WHERE value_name = ''ScriptVersion'';
    END;';
    
	print @sql

	EXEC sys.sp_executesql @Sql;
    
    FETCH NEXT FROM db_cursor INTO @DbName, @clientname;
END;

CLOSE db_cursor;
DEALLOCATE db_cursor;

PRINT N'Database loop completed.';

end