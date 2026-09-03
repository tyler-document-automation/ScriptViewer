
/*************************************
This script is used to backup data before importing new version info


exec VersionBackup 'C:\Users\brian.varnell\Desktop\test\', 'NYCDOF'
*************************************/

CREATE   procedure [dbo].[VersionBackup] (@path varchar(200), @ClientName varchar(255))
as

--SET NOCOUNT ON;
SET XACT_ABORT ON;

declare @version varchar(100)
declare @sourcesystem decimal(8,2)

begin try

	begin transaction

	select @version = versionnumber, @sourcesystem = SourceSystem
		from ScriptVersion
			where ClientName = @ClientName

	insert into ScriptVersion_bak
		select *
			from ScriptVersion
			where ClientName = @ClientName

	if @sourcesystem = 5
		begin
		insert into ScriptLib_bak
		select *, @version
			from ScriptLib
			where ClientName = @ClientName
		end

		/*
		truncate table ScriptVersion_bak
		truncate table ScriptLib_bak

		select count(*) from ScriptVersion_bak
		select count(*) from ScriptLib_bak

		*/

	commit transaction
end try

BEGIN CATCH

        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;

        THROW;

END CATCH;