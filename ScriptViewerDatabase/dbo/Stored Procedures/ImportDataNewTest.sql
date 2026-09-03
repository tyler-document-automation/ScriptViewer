
/*************************************
This procedure imports scripting supplied by the ScriptViewer application.

The application passes the contents of the JSON file through @JsonData.
*************************************/

/*
DECLARE @JsonData nvarchar(max);

SELECT @JsonData = BulkColumn
FROM OPENROWSET
(
    BULK N'C:\Temp\release.json',
    SINGLE_CLOB,
    CODEPAGE = '65001'
) AS JsonFile;

SELECT
    DATALENGTH(@JsonData) AS JsonBytes,
    ISJSON(@JsonData) AS IsValidJson;

EXEC dbo.ImportDataNewTest
    @JsonData = @JsonData,
    @ClientName = N'TEST';

*/

CREATE PROCEDURE [dbo].[ImportDataNewTest]
    @JsonData nvarchar(max),
    @ClientName nvarchar(255)
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    SET @ClientName = NULLIF(LTRIM(RTRIM(UPPER(@ClientName))), N'');

    IF @ClientName IS NULL
    BEGIN
        ;THROW 50002, 'ClientName is required.', 1;
    END;

    IF @JsonData IS NULL OR DATALENGTH(@JsonData) = 0
    BEGIN
        ;THROW 50003, 'JSON data is required.', 1;
    END;

    IF ISJSON(@JsonData) <> 1
    BEGIN
        ;THROW 50004, 'The supplied data is not valid JSON.', 1;
    END;

    --EXEC dbo.DeleteData @ClientName;

    BEGIN TRY
        BEGIN TRANSACTION;

		--Temporary test error. Remove after testing.
    ;THROW 50099, 'This is a test error from ImportDataNewTest.', 1;

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
            JSON_VALUE(@JsonData, '$.description'),
            JSON_VALUE(@JsonData, '$.version'),
            TRY_CONVERT(datetimeoffset(7), JSON_VALUE(@JsonData, '$.exportedTime'), 127),
            '6.0'
        );

        INSERT dbo.ScriptViewer2_Libs
        (
            ClientName,
            library_index,
            name,
            description,
            library_type
        )
        SELECT
            @ClientName,
            CONVERT(int, lib.[key]) AS library_index,
            library.name,
            library.description,
            library.library_type
        FROM OPENJSON(@JsonData, '$.scriptLibraries') AS lib
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
        FROM dbo.ScriptViewer2_Libs;

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
        FROM OPENJSON(@JsonData, '$.scriptLibraries') AS lib
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
        FROM OPENJSON(@JsonData, '$.scriptLibraries') AS lib
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
		PRINT 'Import completed successfully for client: ' + @ClientName;
    END TRY
    BEGIN CATCH
         IF @@TRANCOUNT > 0
        BEGIN
            ROLLBACK TRANSACTION;
        END;

        PRINT 'Import failed for client: ' + COALESCE(@ClientName, 'Unknown');
        PRINT 'Error number: ' + CONVERT(varchar(20), ERROR_NUMBER());
        PRINT 'Error message: ' + ERROR_MESSAGE();
        PRINT 'Error procedure: ' + COALESCE(ERROR_PROCEDURE(), 'Unknown');
        PRINT 'Error line: ' + CONVERT(varchar(20), ERROR_LINE());

        THROW;
    END CATCH;
END;
