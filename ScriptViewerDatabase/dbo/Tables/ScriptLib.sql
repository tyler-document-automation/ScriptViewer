CREATE TABLE [dbo].[ScriptLib] (
    [ClientName]   NVARCHAR (255)  NOT NULL,
    [ScriptLibID]  INT             NOT NULL,
    [Name]         VARCHAR (100)   NOT NULL,
    [Description]  VARCHAR (255)   NULL,
    [Script]       VARCHAR (MAX)   NULL,
    [CreateDate]   DATETIME        NOT NULL,
    [ModifiedDate] DATETIME        NOT NULL,
    [Version]      DECIMAL (18, 1) NOT NULL,
    [ModifiedBy]   VARCHAR (128)   NULL,
    [LibType]      SMALLINT        NOT NULL,
    [LibAccess]    SMALLINT        NOT NULL,
    [Release]      VARCHAR (20)    NULL
);

