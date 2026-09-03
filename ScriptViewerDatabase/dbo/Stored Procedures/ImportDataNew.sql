/*************************************
This script is used to import new scripting so it is viewable via the ScriptViewer app.

exec ImportDataNew '\\dbksvcsifsvai\Development\BrianV\ScriptViewer\ImportFiles\20260828_092258_708\releases_scripting_1.1.9 (1).json', 'test'
*************************************/

CREATE   PROCEDURE [dbo].[ImportDataNew]
    @FilePath nvarchar(255),  --full path including file name
	@ClientName nvarchar(255)
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

	 SET @ClientName = NULLIF(LTRIM(RTRIM(upper(@ClientName))), N'');
    SET @FilePath = NULLIF(LTRIM(RTRIM(@FilePath)), N'');

    --IF @ClientName IS NULL
    --BEGIN
    --    THROW 50002, 'ClientName is required.', 1;
    --END;

    --IF @FilePath IS NULL
    --BEGIN
    --    THROW 50003, 'JsonFilePath is required.', 1;
    --END;

	--exec DeleteData @ClientName

    DECLARE @json nvarchar(max);
    DECLARE @ReleaseExportId int;
    DECLARE @BulkSql nvarchar(max);

    SET @BulkSql = N'
SELECT @jsonOut = BulkColumn
FROM OPENROWSET(
    BULK N''' + REPLACE(@FilePath, '''', '''''') + N''',
    SINGLE_CLOB,
    CODEPAGE = ''65001''
) AS src;';

    EXEC sys.sp_executesql
        @BulkSql,
        N'@jsonOut nvarchar(max) OUTPUT',
        @jsonOut = @json OUTPUT;

    --IF COALESCE(ISJSON(@json), 0) <> 1
    --BEGIN
    --    THROW 50000, 'The file contents are not valid JSON.', 1;
    --END;

    BEGIN TRY
        BEGIN TRANSACTION;

        INSERT dbo.ScriptVersion
        (
            ClientName,
            description,
			versionnumber,
            versiondate,
            SourceSystem
        )
        VALUES
        (
            @ClientName,
            JSON_VALUE(@json, '$.description'),
			JSON_VALUE(@json, '$.version'),
            TRY_CONVERT(datetimeoffset(7), JSON_VALUE(@json, '$.exportedTime'), 127),
			'6.0'
        );

        --SET @ReleaseExportId = CONVERT(int, SCOPE_IDENTITY());

		--print @ReleaseExportId

        INSERT dbo.ScriptViewer2_Libs
        (
          --  release_export_id,
            ClientName,
            library_index,
            name,
            description,
            library_type
        )
        SELECT
           -- @ReleaseExportId,
            @ClientName,
            CONVERT(int, lib.[key]) AS library_index,
            library.name,
            library.description,
            library.library_type
        FROM OPENJSON(@json, '$.scriptLibraries') AS lib
        CROSS APPLY OPENJSON(lib.value)
        WITH
        (
            name nvarchar(255) '$.name',
            description nvarchar(max) '$.description',
            library_type nvarchar(100) '$.type'
        ) AS library;

        CREATE TABLE #LibraryMap
        (
            library_index int NOT NULL,
            release_library_id int NOT NULL
        );

        INSERT #LibraryMap
        (
            library_index,
            release_library_id
        )
        SELECT
            library_index,
            release_library_id
        FROM dbo.ScriptViewer2_Libs
       -- WHERE release_export_id = @ReleaseExportId;

        INSERT dbo.ScriptViewer2_Scripts
        (
            release_library_id,
            ClientName,
            script_index,
            name,
            description,
            workflow_task_id,
            has_errors,
            js_body
        )
        SELECT
            library_map.release_library_id,
            @ClientName,
            CONVERT(int, script_item.[key]) AS script_index,
            script.name,
            script.description,
            script.workflowTaskId,
            script.hasErrors,
            script.jsBody
        FROM OPENJSON(@json, '$.scriptLibraries') AS lib
        CROSS APPLY OPENJSON(lib.value)
        WITH
        (
            scripts nvarchar(max) '$.scripts' AS JSON
        ) AS library
        INNER JOIN #LibraryMap AS library_map
            ON library_map.library_index = CONVERT(int, lib.[key])
        CROSS APPLY OPENJSON(library.scripts) AS script_item
        CROSS APPLY OPENJSON(script_item.value)
        WITH
        (
            name nvarchar(255) '$.name',
            description nvarchar(max) '$.description',
            workflowTaskId nvarchar(100) '$.workflowTaskId',
            jsBody nvarchar(max) '$.jsBody',
            hasErrors bit '$.hasErrors'
        ) AS script;

        CREATE TABLE #ScriptMap
        (
            library_index int NOT NULL,
            script_index int NOT NULL,
            release_script_id int NOT NULL
        );

        INSERT #ScriptMap
        (
            library_index,
            script_index,
            release_script_id
        )
        SELECT
            library_map.library_index,
            script.script_index,
            script.release_script_id
        FROM dbo.ScriptViewer2_Scripts AS script
        INNER JOIN #LibraryMap AS library_map
            ON library_map.release_library_id = script.release_library_id;

        INSERT dbo.ScriptViewer2_Parameters
        (
            release_script_id,
            ClientName,
            parameter_index,
            name,
            description,
            parameter_type,
            parameter_sequence
        )
        SELECT
            script_map.release_script_id,
            @ClientName,
            CONVERT(int, parameter_item.[key]) AS parameter_index,
            parameter.name,
            parameter.description,
            parameter.parameter_type,
            parameter.parameter_sequence
        FROM OPENJSON(@json, '$.scriptLibraries') AS lib
        CROSS APPLY OPENJSON(lib.value)
        WITH
        (
            scripts nvarchar(max) '$.scripts' AS JSON
        ) AS library
        CROSS APPLY OPENJSON(library.scripts) AS script_item
        INNER JOIN #ScriptMap AS script_map
            ON script_map.library_index = CONVERT(int, lib.[key])
           AND script_map.script_index = CONVERT(int, script_item.[key])
        CROSS APPLY OPENJSON(script_item.value)
        WITH
        (
            parameters nvarchar(max) '$.parameters' AS JSON
        ) AS script
        CROSS APPLY OPENJSON(script.parameters) AS parameter_item
        CROSS APPLY OPENJSON(parameter_item.value)
        WITH
        (
            name nvarchar(255) '$.name',
            description nvarchar(max) '$.description',
            parameter_type int '$.type',
            parameter_sequence int '$.sequence'
        ) AS parameter;

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
        BEGIN
            ROLLBACK TRANSACTION;
        END;

        THROW;
    END CATCH;

END;
