/*
  Creates or updates a stored procedure that imports the scripting release JSON
  export into the tables created by dbo.CreateTables in:

  01_create_release_json_tables.sql

  Notes:
  - Execute this script once to create dbo.ImportData.
  - Run the import with:

      EXEC dbo.ImportData @ClientName = N'Your Client Name';

  - The SQL Server service account must be able to read the JSON file path.
  - OPENROWSET(BULK...) requires Ad Hoc Distributed Queries to be enabled.
  - OPENROWSET(BULK...) uses a literal path in most SQL Server versions, so
    update both @JsonFile and the BULK path directly if you move the JSON file.
*/

CREATE   PROCEDURE dbo.ImportData
    @ClientName nvarchar(255)
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    SET @ClientName = NULLIF(LTRIM(RTRIM(@ClientName)), N'');

    IF @ClientName IS NULL
    BEGIN
        THROW 50002, 'ClientName is required.', 1;
    END;

    IF OBJECT_ID(N'dbo.ScriptViewer2_Version', N'U') IS NULL
    BEGIN
        THROW 50001, 'Run 01_create_release_json_tables.sql before this import procedure.', 1;
    END;

    DECLARE @JsonFile nvarchar(4000) =
        N'C:\Users\Brian.Varnell\Desktop\test2\releases_scripting_1.0.9 (1).json';

    DECLARE @json nvarchar(max);
    DECLARE @ReleaseExportId int;

    SELECT @json = BulkColumn
    FROM OPENROWSET(
        BULK N'C:\Users\Brian.Varnell\Desktop\test2\releases_scripting_1.0.9 (1).json',
        SINGLE_CLOB,
        CODEPAGE = '65001'
    ) AS src;

    IF COALESCE(ISJSON(@json), 0) <> 1
    BEGIN
        THROW 50000, 'The file contents are not valid JSON.', 1;
    END;

    BEGIN TRY
        BEGIN TRANSACTION;

        /* Delete existing rows for this client before importing the new export. */
        DELETE dbo.ScriptViewer2_Parameters
        WHERE ClientName = @ClientName;

        DELETE dbo.ScriptViewer2_BatchDefs
        WHERE ClientName = @ClientName;

        DELETE dbo.ScriptViewer2_GlobalVars
        WHERE ClientName = @ClientName;

        DELETE dbo.ScriptViewer2_Scripts
        WHERE ClientName = @ClientName;

        DELETE dbo.ScriptViewer2_Libs
        WHERE ClientName = @ClientName;

        DELETE dbo.ScriptViewer2_Version
        WHERE ClientName = @ClientName;

        INSERT dbo.ScriptViewer2_Version
        (
            ClientName,
            source_file,
            description,
            exported_from,
            exported_time_utc,
            version,
            exported_by_user,
            raw_json
        )
        VALUES
        (
            @ClientName,
            @JsonFile,
            JSON_VALUE(@json, '$.description'),
            JSON_VALUE(@json, '$.exportedFrom'),
            TRY_CONVERT(datetimeoffset(7), JSON_VALUE(@json, '$.exportedTime'), 127),
            JSON_VALUE(@json, '$.version'),
            JSON_VALUE(@json, '$.exportedByUser'),
            @json
        );

        SET @ReleaseExportId = CONVERT(int, SCOPE_IDENTITY());

        INSERT dbo.ScriptViewer2_Libs
        (
            release_export_id,
            ClientName,
            library_index,
            name,
            description,
            library_type
        )
        SELECT
            @ReleaseExportId,
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
        WHERE release_export_id = @ReleaseExportId;

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

        INSERT dbo.ScriptViewer2_BatchDefs
        (
            release_library_id,
            ClientName,
            linked_batch_definition_index,
            linked_batch_definition,
            linked_batch_definition_json
        )
        SELECT
            library_map.release_library_id,
            @ClientName,
            CONVERT(int, linked_item.[key]) AS linked_batch_definition_index,
            CASE
                WHEN linked_item.[type] IN (1, 2, 3)
                    THEN CONVERT(nvarchar(255), linked_item.value)
                ELSE NULL
            END AS linked_batch_definition,
            CASE
                WHEN linked_item.[type] IN (4, 5)
                    THEN CONVERT(nvarchar(max), linked_item.value)
                ELSE NULL
            END AS linked_batch_definition_json
        FROM OPENJSON(@json, '$.scriptLibraries') AS lib
        CROSS APPLY OPENJSON(lib.value)
        WITH
        (
            linkedBatchDefinitions nvarchar(max) '$.linkedBatchDefinitions' AS JSON
        ) AS library
        INNER JOIN #LibraryMap AS library_map
            ON library_map.library_index = CONVERT(int, lib.[key])
        CROSS APPLY OPENJSON(library.linkedBatchDefinitions) AS linked_item;

        INSERT dbo.ScriptViewer2_GlobalVars
        (
            release_export_id,
            ClientName,
            global_variable_index,
            name,
            description,
            global_variable_value,
            global_variable_type,
            json_value_type,
            global_variable_json
        )
        SELECT
            @ReleaseExportId,
            @ClientName,
            CONVERT(int, global_item.[key]) AS global_variable_index,
            CASE
                WHEN global_item.[type] = 5
                    THEN JSON_VALUE(global_item.value, '$.name')
                ELSE NULL
            END AS name,
            CASE
                WHEN global_item.[type] = 5
                    THEN JSON_VALUE(global_item.value, '$.description')
                ELSE NULL
            END AS description,
            CASE
                WHEN global_item.[type] = 5
                    THEN JSON_VALUE(global_item.value, '$.value')
                WHEN global_item.[type] IN (1, 2, 3)
                    THEN CONVERT(nvarchar(max), global_item.value)
                ELSE NULL
            END AS global_variable_value,
            CASE
                WHEN global_item.[type] = 5
                    THEN JSON_VALUE(global_item.value, '$.type')
                ELSE NULL
            END AS global_variable_type,
            global_item.[type] AS json_value_type,
            CASE
                WHEN global_item.[type] IN (4, 5)
                    THEN CONVERT(nvarchar(max), global_item.value)
                ELSE NULL
            END AS global_variable_json
        FROM OPENJSON(@json, '$.globalVariables') AS global_item;

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
