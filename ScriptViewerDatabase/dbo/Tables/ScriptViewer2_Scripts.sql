CREATE TABLE [dbo].[ScriptViewer2_Scripts] (
    [ClientName]         NVARCHAR (255) NULL,
    [release_script_id]  INT            IDENTITY (1, 1) NOT NULL,
    [release_library_id] INT            NOT NULL,
    [script_index]       INT            NOT NULL,
    [name]               NVARCHAR (255) NULL,
    [description]        NVARCHAR (MAX) NULL,
    [workflow_task_id]   NVARCHAR (100) NULL,
    [has_errors]         BIT            NULL,
    [js_body]            NVARCHAR (MAX) NULL,
    [created_at_utc]     DATETIME2 (7)  NULL,
    CONSTRAINT [PK_ScriptViewer2_Scripts] PRIMARY KEY CLUSTERED ([release_script_id] ASC)
);

