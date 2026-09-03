CREATE TABLE [dbo].[ScriptVersion] (
    [ClientName]    NVARCHAR (255) NOT NULL,
    [description]   NVARCHAR (MAX) NULL,
    [versionnumber] NVARCHAR (100) NULL,
    [versionDate]   DATETIME       NULL,
    [SourceSystem]  DECIMAL (8, 2) NULL,
    CONSTRAINT [PK_ScriptVersion] PRIMARY KEY CLUSTERED ([ClientName] ASC)
);

