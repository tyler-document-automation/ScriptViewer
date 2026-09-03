/*
  Creates or updates a stored procedure that drops the ScriptViewer2 import
  tables.

  Run the drop step with:

      EXEC dbo.DropTables;
*/

CREATE   PROCEDURE [dbo].[DropTables]
AS
BEGIN
    SET NOCOUNT ON;

    DROP TABLE IF EXISTS dbo.ScriptViewer2_Parameters;
    DROP TABLE IF EXISTS dbo.ScriptViewer2_Scripts;
    DROP TABLE IF EXISTS dbo.ScriptViewer2_Libs;
    DROP TABLE IF EXISTS dbo.ScriptVersion;
	DROP TABLE IF EXISTS dbo.ScriptLib;
END;
