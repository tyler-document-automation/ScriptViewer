
CREATE     procedure [dbo].[GetScripts]
	@querytype int,  --controls which section of logic below runs
	@database nvarchar(200) = null,  --determines what client data to pull
	@eventorcustom nvarchar(200) = null,  --event or custom library
	@library nvarchar(200) = null,  --library name
	@eventfunctionname nvarchar(200) = null,  --function name
	@searchstring varchar(20) = null  --used to filter results to only scripts containing this string
as

/*****************************
values for @querytype:
0 - builds list of clients/databases used in dropdown list
1 - used when retrieving list of event or custom libraries (based on value of @eventorcustom param)
2 - used to retrieve the functions inside the library clicked in the event/custom library list
3 - shows the code withing the function selected
4 - called when the dropdown selected value is changed to get the version of the client script that is stored
5 - used to generate version by client report on screen

******************************/

begin

declare @sql nvarchar(max)

if @searchstring is null
	set @searchstring = '%'
else 
	set @searchstring = '%' + @searchstring + '%'

--build client list for dropdown
if @querytype = 0
	begin
		
	--if search string exists, pull filtered client list
	IF @searchstring LIKE '\%%\%' ESCAPE '\'
		select case
				when SourceSystem = 5 then sv.ClientName
				else sv.ClientName + ' (v2)'
			end ClientName
			from scriptlib sl
				join ScriptVersion sv
					on sl.ClientName = sv.ClientName
			WHERE SCRIPT LIKE @searchstring
		UNION
		select case
				when SourceSystem = 5 then sv.ClientName
				else sv.ClientName + ' (v2)'
			end ClientName
			from ScriptViewer2_SCRIPTS sl
				join ScriptVersion sv
					on sl.ClientName = sv.ClientName
			WHERE JS_BODY LIKE @searchstring
			ORDER BY 1
	else
		select 
			case
				when SourceSystem = 5 then ClientName
				else ClientName + ' (v2)'
			end ClientName
			from scriptversion
			order by 1
			
			return
	end

if @querytype in (1,2,3)
	begin
	create table #ScriptResults (
				LibraryType varchar(max),
				LibraryName varchar(max),
				LibraryDescr varchar(max),
				EventFunctionName varchar(max),
				EventID varchar(max),
				EventDescr varchar(max),
				Params varchar(max),
				EventCode nvarchar(max))

	if @database not like '%(v2)%'
		begin
			set @sql = 'insert into #ScriptResults 
				select a.LibraryType, a.LibraryName, a.LibraryDescr, a.EventFunctionName, 0 as EventID, a.Descr as EventDescr, b.Params, a.eventCode
				from 
				--event library code
				(SELECT ''Event'' as LibraryType,
					scriptlib.name as LibraryName,
					scriptlib.Description LibraryDescr,
					eventData.eventName as EventFunctionName,
					eventdata.eventdescription as Descr,
					eventData.eventCode as eventCode
				FROM scriptlib
				CROSS APPLY 
					OPENJSON(script, ''$.events'') 
					WITH (
						eventName NVARCHAR(MAX) ''$.eventName'',
						eventDescription nvarchar(max) ''$.eventDescription'',
						eventCode NVARCHAR(MAX) ''$.eventCode''
					) AS eventData
					where clientname = ''' + @database + ''') a
				inner join 
				--event library params
				(select LibraryType, LibraryName, EventFunctionName, Descr, concat(''Parameters: '', string_agg(ParamName, '', ''))  as Params
					from (
					select ''Event ''LibraryType,
					sl.Name LibraryName,
					json_value(e.value, ''$.eventName'') as EventFunctionName,
					json_value(e.value, ''$.eventDescription'') as Descr,
					json_value(p.value, ''$.paramName'') as ParamName--,
					FROM ScriptLib sl      
						cross apply openjson(sl.script, ''$.events'') as e
						cross apply openjson(e.value, ''$.eventParams'') as p
				  where libtype in (1, 2)
					and clientname = ''' + @database + ''') as x
				  group by LibraryType, LibraryName, EventFunctionName, Descr
				  ) b
				  on a.LibraryType = b.LibraryType
					and a.LibraryName = b.LibraryName
					and a.EventFunctionName = b.EventFunctionName
					where 1 = 1
			union
			select a.LibraryType, a.LibraryName, a.LibraryDescr, a.EventFunctionName, a.EventID, a.Descr as EventDescr, b.Params, a.eventcode
				from 
				--custom libary code
				(SELECT ''Custom'' as LibraryType,
					scriptlib.name as LibraryName,
					scriptlib.Description as LibraryDescr,
					eventData.eventName as EventFunctionName,
					eventData.eventId as EventID,
					eventdata.eventdescription as Descr,
					eventData.eventCode as eventCode
				FROM scriptlib
				CROSS APPLY 
					OPENJSON(script, ''$.methods'') 
					WITH (
						eventName NVARCHAR(MAX) ''$.eventName'',
						eventDescription nvarchar(max) ''$.eventDescription'',
						eventId nvarchar(max) ''$.eventId'',
						eventCode NVARCHAR(MAX) ''$.eventCode''
					) AS eventData
					where clientname = ''' + @database + ''') a
				left join
				--inner join
				--custom library params
				(select LibraryType, LibraryName, EventFunctionName, EventID, Descr, concat(''Parameters: '', string_agg(Params, '', ''))  as Params
					from (
					select ''Custom'' as LibraryType,
					sl.Name LibraryName, 
					json_value(m.value, ''$.eventName'') as EventFunctionName,
					json_value(m.value, ''$.eventId'') as EventID,
					json_value(m.value, ''$.eventDescription'') as Descr,
					concat(json_value(p.value, ''$.paramName'') , '' ('', json_value(p.value, ''$.paramType''), '')'' ) Params
					FROM ScriptLib sl      
						cross apply openjson(sl.script, ''$.methods'') as m
						cross apply openjson(m.value, ''$.eventParams'') as p
				  where sl.libtype = 0
					and clientname = ''' + @database + '''  ) x
				  group by LibraryType, LibraryName, EventFunctionName, EventID, Descr
				  ) b
				  on a.LibraryType = b.LibraryType
					and a.LibraryName = b.LibraryName
					and a.EventFunctionName = b.EventFunctionName
					where 1 = 1'
			end
		else
			begin
				set @sql = 
				'insert into #scriptresults
				SELECT
					sl.library_type AS LibraryType,
					sl.name AS LibraryName,
					sl.description AS LibraryDescr,
					ss.name AS EventFunctionName,
					ss.script_index AS EventID,
					ss.description AS EventDescription,
					''Parameters: '' +
					STRING_AGG(
						sp.name + '' ('' +
						CAST(
							CASE sp.parameter_type
								WHEN 0 THEN ''Text''
								WHEN 3 THEN ''Object''
								ELSE ''??''
							END AS varchar(50)
						) + '')'',
						'', ''
					) AS Params,
					ss.js_body AS EventCode
				FROM ScriptViewer2_Libs sl
				INNER JOIN ScriptViewer2_Scripts ss
					ON sl.ClientName = ss.ClientName
					AND sl.release_library_id = ss.release_library_id
				INNER JOIN ScriptViewer2_Parameters sp
					ON ss.ClientName = sp.ClientName
					AND ss.release_script_id = sp.release_script_id
				where sl.library_type = ''' + @eventorcustom + '''
					and sl.ClientName = replace(''' + @database + ''', '' (v2)'', '''')
				GROUP BY
					sl.library_type,
					sl.name,
					sl.description,
					ss.name,
					ss.script_index,
					ss.description,
					ss.js_body;'
			
			end
		
			--print @sql

			--throw data into temp table
			exec sp_executesql @sql

			--based on input, determine what output to provide
			if @querytype = 1
				begin
					print @eventorcustom
					print @searchstring
					select distinct LibraryName, LibraryDescr 
						from #ScriptResults 
						where LibraryType = @eventorcustom 
						and eventcode like @searchstring 
						order by 1
				end

			if @querytype = 2
				begin
					select distinct EventFunctionName, EventDescr, EventID 
						from #ScriptResults 
						where LibraryType = @eventorcustom 
							and LibraryName = @library 
							and eventcode like @searchstring 
							order by eventID
				end

			if @querytype = 3
				begin
					select '===============================================================================================================' + 
						char(10) + char(13) + isnull(params, '') + char(10) + char(13) + '==============================================================================================================='
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
	set @database = replace(@database, ' (v2)', '')
	set @sql = 'select ''Version: '' + versionnumber + '' - '' + convert(varchar(10), versiondate, 101) as Revision from ScriptVersion where ClientName = ''' + @database + ''''
	--print @sql
	exec sp_executesql @sql
end

if @querytype = 5
begin
	select 
		case
			when SourceSystem = 6 then ClientName + ' (v2)'
			else ClientName
		end ClientName, 
		Versionnumber Version, versionDate Date from ScriptVersion order by 1
end



GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetScripts] TO [ScriptViewerUserRole]
    AS [dbo];

