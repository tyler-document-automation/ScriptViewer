CREATE TABLE [dbo].[ScriptViewer2_Libs] (
    [ClientName]         NVARCHAR (255) NULL,
    [release_library_id] INT            IDENTITY (1, 1) NOT NULL,
    [library_index]      INT            NOT NULL,
    [name]               NVARCHAR (255) NULL,
    [description]        NVARCHAR (MAX) NULL,
    [library_type]       NVARCHAR (100) NULL,
    [created_at_utc]     DATETIME2 (7)  NULL,
    CONSTRAINT [PK_ScriptViewer2_Libs] PRIMARY KEY CLUSTERED ([release_library_id] ASC)
);

