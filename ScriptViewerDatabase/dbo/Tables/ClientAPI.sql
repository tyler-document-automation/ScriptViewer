CREATE TABLE [dbo].[ClientAPI] (
    [Client]    VARCHAR (255) NOT NULL,
    [URLPrefix] VARCHAR (255) NULL,
    [APIKey]    VARCHAR (255) NULL,
    CONSTRAINT [PK_ClientAPI] PRIMARY KEY CLUSTERED ([Client] ASC)
);

