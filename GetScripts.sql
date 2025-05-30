USE [ScriptViewer]
GO
/****** Object:  StoredProcedure [dbo].[GetScripts]    Script Date: 3/17/2025 09:48:36 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER   procedure [dbo].[GetScripts]
	@querytype int,
	@database nvarchar(200) = null,
	@eventorcustom nvarchar(200) = null,
	@library nvarchar(200) = null,
	@eventfunctionname nvarchar(200) = null
as

begin

declare @sql nvarchar(max)

if @querytype = 0
	begin
	select * from (
	--select '--Select client--' ClientName, null DBName
	--union
		select upper(replace(
		case
			when name like 'QA%' then substring(name, 3, 100)
			when name like 'DEV%' then substring(name, 4, 100)
			else name
		end, 
	'_intellidact', '')) ClientName, name DBName 
	from sys.databases 
	where name like '%_intellidact%') x
	order by x.clientname
		
		return
	end

if @querytype in (1,2,3)
	begin

		create table #ScriptResults (
			LibraryType varchar(max),
			LibraryName varchar(max),
			LibraryDescr varchar(max),
			EventFunctionName varchar(max),
			EventDescr varchar(max),
			Params varchar(max),
			EventCode nvarchar(max))

		set @sql = 'insert into #ScriptResults 
			select a.LibraryType, a.LibraryName, a.LibraryDescr, a.EventFunctionName, a.Descr as EventDescr, b.Params, a.eventCode
			from 
			--event library code
			(SELECT ''Event'' as LibraryType,
				scriptlib.name as LibraryName,
				scriptlib.Description LibraryDescr,
				eventData.eventName as EventFunctionName,
				eventdata.eventdescription as Descr,
				eventData.eventCode as eventCode
			FROM
				 ' + @database + '..scriptlib
			CROSS APPLY 
				OPENJSON(script, ''$.events'') 
				WITH (
					eventName NVARCHAR(MAX) ''$.eventName'',
					eventDescription nvarchar(max) ''$.eventDescription'',
					eventCode NVARCHAR(MAX) ''$.eventCode''
				) AS eventData
				) a
			inner join 
			--event library params
			(select LibraryType, LibraryName, EventFunctionName, Descr, concat(''Parameters: '', string_agg(ParamName, '', ''))  as Params
				from (
				select ''Event ''LibraryType,
				sl.Name LibraryName,
				json_value(e.value, ''$.eventName'') as EventFunctionName,
				json_value(e.value, ''$.eventDescription'') as Descr,
				json_value(p.value, ''$.paramName'') as ParamName--,
				FROM ' + @database + '..ScriptLib sl             
					cross apply openjson(sl.script, ''$.events'') as e
					cross apply openjson(e.value, ''$.eventParams'') as p
			  where libtype in (1, 2)) as x
			  group by LibraryType, LibraryName, EventFunctionName, Descr
			  ) b
			  on a.LibraryType = b.LibraryType
				and a.LibraryName = b.LibraryName
				and a.EventFunctionName = b.EventFunctionName
		union
		select a.LibraryType, a.LibraryName, a.LibraryDescr, a.EventFunctionName, a.Descr as EventDescr, b.Params, a.eventcode
			from 
			--custom libary code
			(SELECT ''Custom'' as LibraryType,
				scriptlib.name as LibraryName,
				scriptlib.Description as LibraryDescr,
				eventData.eventName as EventFunctionName,
				eventdata.eventdescription as Descr,
				eventData.eventCode as eventCode
			FROM
				' + @database + '..scriptlib
			CROSS APPLY 
				OPENJSON(script, ''$.methods'') 
				WITH (
					eventName NVARCHAR(MAX) ''$.eventName'',
					eventDescription nvarchar(max) ''$.eventDescription'',
					eventCode NVARCHAR(MAX) ''$.eventCode''
				) AS eventData
				) a
			inner join
			--custom library params
			(select LibraryType, LibraryName, EventFunctionName, Descr, concat(''Parameters: '', string_agg(Params, '', ''))  as Params
				from (
				select ''Custom'' as LibraryType,
				sl.Name LibraryName, 
				json_value(m.value, ''$.eventName'') as EventFunctionName,
				json_value(m.value, ''$.eventDescription'') as Descr,
				concat(json_value(p.value, ''$.paramName'') , '' ('', json_value(p.value, ''$.paramType''), '')'' ) Params
				FROM ' + @database + '..ScriptLib sl             
					cross apply openjson(sl.script, ''$.methods'') as m
					cross apply openjson(m.value, ''$.eventParams'') as p
			  where sl.libtype = 0) x
			  group by LibraryType, LibraryName, EventFunctionName, Descr
			  ) b
			  on a.LibraryType = b.LibraryType
				and a.LibraryName = b.LibraryName
				and a.EventFunctionName = b.EventFunctionName'
		


			exec sp_executesql @sql

			--select * from #ScriptResults

			if @querytype = 1
				begin
					select distinct LibraryName, LibraryDescr 
						from #ScriptResults 
						where LibraryType = @eventorcustom order by 1
				end

			if @querytype = 2
				begin
					select distinct EventFunctionName, EventDescr 
						from #ScriptResults 
						where LibraryType = @eventorcustom 
							and LibraryName = @library order by 1
				end

			if @querytype = 3
				begin
					select '===============================================================================================================' + 
						char(10) + char(13) + params + char(10) + char(13) + '==============================================================================================================='
						+ char(10) + char(13) + eventCode as EventCode
						from #ScriptResults 
							where LibraryType = @eventorcustom 
								and LibraryName = @library 
								and EventFunctionName = @eventfunctionname order by 1
				end
		end  
end

if @querytype = 4
begin
	set @sql = 'select ''Version: '' + value_data as Revision from ' + @database + '..SysConfigValue where value_name = ''ScriptVersion'''
	--print @sql
	exec sp_executesql @sql
end

if @querytype = 5
begin
	CREATE TABLE #CLIENTS (
			ClientName varchar(100),
			Revision varchar(100))

		declare getdata cursor for
			select name from sys.databases	
			--where name like '%_intellidact%' order by 1
		
		open getdata

		fetch next from getdata into @database

		while @@FETCH_STATUS = 0
			begin
			print @database
			set @sql = 'IF EXISTS (SELECT 1 FROM ' + @DATABASE + '.INFORMATION_SCHEMA.TABLES
							WHERE TABLE_NAME = ''SYSCONFIGVALUE'')
					
						BEGIN
							INSERT INTO #CLIENTS
								select upper(replace(''' + @database + ''', ''_intellidact'', '''')) ClientName, value_data as Revision from ' + @database + '..SysConfigValue where value_name = ''ScriptVersion''
						END
						'
			--PRINT @SQL
			--set @sql = 'select upper(replace(''' + @database + ''', ''_intellidact'', '''')) ClientName, value_data as Revision from ' + @database + '..SysConfigValue where value_name = ''ScriptVersion'''
			--print @sql
			exec sp_executesql @sql
			fetch next from getdata into @database
			end

		close getdata
		deallocate getdata

		select ClientName, 
		case 
			when revision like '%(%)%' then
				LEFT(revision, CHARINDEX('(', revision) - 1) 
			else null
		end AS 'Version',
		case
			when revision like '%(%)%' then
			cast(SUBSTRING(revision, CHARINDEX('(', revision) + 1, CHARINDEX(')', revision) - CHARINDEX('(', revision) - 1) as datetime)
			else null
		end AS 'Date'
		from #CLIENTS order by 1

		--select clientname, version, cast(date as datetime) date from (
		--select 'test' ClientName, '1.1' as 'Version', '10/22/2025 03:01:44 PM' as 'Date'
		--union
		--select 'test' ClientName, '1.1' as 'Version', '07/22/2027 03:01:44 PM' as 'Date'
		--union
		--select 'test' ClientName, '1.1' as 'Version', '11/22/2026 03:01:44 PM' as 'Date') a
end

