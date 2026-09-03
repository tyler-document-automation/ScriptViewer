CREATE TABLE [dbo].[ScriptViewer2_Parameters] (
    [ClientName]                  NVARCHAR (255) NULL,
    [release_script_parameter_id] INT            IDENTITY (1, 1) NOT NULL,
    [release_script_id]           INT            NOT NULL,
    [parameter_index]             INT            NOT NULL,
    [name]                        NVARCHAR (255) NULL,
    [description]                 NVARCHAR (MAX) NULL,
    [parameter_type]              INT            NULL,
    [parameter_sequence]          INT            NULL,
    [created_at_utc]              DATETIME2 (7)  NULL,
    CONSTRAINT [PK_ScriptViewer2_Parameters] PRIMARY KEY CLUSTERED ([release_script_parameter_id] ASC)
);

