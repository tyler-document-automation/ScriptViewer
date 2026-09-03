/*
 Deletes a client's data from the system

 EXEC dbo.DeleteData @ClientName = N'test';
*/

CREATE   PROCEDURE [dbo].[DeleteData]
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

    BEGIN TRY
        BEGIN TRANSACTION;

		DELETE dbo.ScriptLib
        WHERE ClientName = @ClientName;

		DELETE dbo.ScriptVersion
        WHERE ClientName = @ClientName;

        DELETE dbo.ScriptViewer2_Parameters
        WHERE ClientName = @ClientName;

        DELETE dbo.ScriptViewer2_Scripts
        WHERE ClientName = @ClientName;

        DELETE dbo.ScriptViewer2_Libs
        WHERE ClientName = @ClientName;


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
