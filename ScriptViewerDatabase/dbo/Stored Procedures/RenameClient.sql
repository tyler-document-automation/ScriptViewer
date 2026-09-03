/*************************************
This script is used to rename clients

exec RenameClient 'charlottecrt', 'charlotte'
*************************************/

CREATE   PROCEDURE [dbo].[RenameClient]
    @OldName nvarchar(255),
	@NewName nvarchar(255)
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;
	
	set @OldName = upper(@OldName)
	set @NewName = upper(@NewName)

    BEGIN TRY
        BEGIN TRANSACTION;

        print 'updating tables....'

		update clientapi set Client = @NewName where Client = @OldName
		update ScriptLib set ClientName = @NewName where ClientName = @OldName
		update ScriptVersion set ClientName = @NewName where ClientName = @OldName
		update ScriptViewer2_Libs set ClientName = @NewName where ClientName = @OldName
		update ScriptViewer2_Parameters set ClientName = @NewName where ClientName = @OldName
		update ScriptViewer2_Scripts set ClientName = @NewName where ClientName = @OldName

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
