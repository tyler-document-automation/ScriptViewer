/*************************************
Creates all of the tables used in by the ScriptViewer application

exec droptables
exec CreateTables

select * from scriptlib
select * from scriptversion order by 1
select * from  ScriptViewer2_Libs
select * from  ScriptViewer2_Scripts
select * from  ScriptViewer2_Parameters

*************************************/



CREATE   procedure [dbo].[CreateTables] as
begin

IF OBJECT_ID(N'dbo.ScriptLib', N'U') IS NULL
	begin
		CREATE TABLE [dbo].[ScriptLib](
		ClientName nvarchar(255) not NULL,
		[ScriptLibID] [int] NOT NULL,
		[Name] [varchar](100) NOT NULL,
		[Description] [varchar](255) NULL,
		[Script] [varchar](max) NULL,
		[CreateDate] [datetime] NOT NULL,
		[ModifiedDate] [datetime] NOT NULL,
		[Version] [decimal](18, 1) NOT NULL,
		[ModifiedBy] [varchar](128) NULL,
		[LibType] [smallint] NOT NULL,
		[LibAccess] [smallint] NOT NULL,
		[Release] [varchar](20) NULL)
	end

IF OBJECT_ID(N'dbo.ScriptVersion', N'U') IS NULL
	begin
		create table dbo.ScriptVersion (
		[ClientName] [nvarchar](255) not NULL,
		[description] [nvarchar](max) NULL,
		[versionnumber] [nvarchar](100) NULL,
		versionDate datetime null,
		[SourceSystem] [decimal](8, 2) NULL)
	end

IF OBJECT_ID(N'dbo.ScriptViewer2_Libs', N'U') IS NULL
    BEGIN
         CREATE TABLE dbo.ScriptViewer2_Libs
        (
            ClientName nvarchar(255) NULL,
            release_library_id int IDENTITY(1, 1) NOT NULL
                CONSTRAINT PK_ScriptViewer2_Libs PRIMARY KEY,
           -- release_export_id int NULL,
            library_index int NOT NULL,
            name nvarchar(255) NULL,
            description nvarchar(max) NULL,
            library_type nvarchar(100) NULL,
            created_at_utc datetime2(7) NULL
        );
    END;

IF OBJECT_ID(N'dbo.ScriptViewer2_Scripts', N'U') IS NULL
    BEGIN
        CREATE TABLE dbo.ScriptViewer2_Scripts
        (
            ClientName nvarchar(255) NULL,
            release_script_id int IDENTITY(1, 1) NOT NULL
                CONSTRAINT PK_ScriptViewer2_Scripts PRIMARY KEY,
            release_library_id int NOT NULL,
            script_index int NOT NULL,
            name nvarchar(255) NULL,
            description nvarchar(max) NULL,
            workflow_task_id nvarchar(100) NULL,
            has_errors bit NULL,
            js_body nvarchar(max) NULL,
            created_at_utc datetime2(7)  NULL
        );
    END;

IF OBJECT_ID(N'dbo.ScriptViewer2_Parameters', N'U') IS NULL
    BEGIN
        CREATE TABLE dbo.ScriptViewer2_Parameters
        (
            ClientName nvarchar(255) NULL,
            release_script_parameter_id int IDENTITY(1, 1) NOT NULL
                CONSTRAINT PK_ScriptViewer2_Parameters PRIMARY KEY,
            release_script_id int NOT NULL,
            parameter_index int NOT NULL,
            name nvarchar(255) NULL,
            description nvarchar(max) NULL,
            parameter_type int NULL,
            parameter_sequence int NULL,
            created_at_utc datetime2(7) NULL
        );
    END;



end