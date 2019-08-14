CREATE TABLE [dbo].[Role] (
    [Id]           INT           IDENTITY (1, 1) NOT NULL,
    [Name]         VARCHAR (256) NOT NULL,
    [Description]  VARCHAR (256) NULL,
    [Is_Enabled]   BIT           NOT NULL,
    [Created_By]   INT           NOT NULL,
    [Created_Date] DATETIME      NOT NULL,
    [Updated_By]   INT           NOT NULL,
    [Updated_Date] DATETIME      NOT NULL,
    [Is_Deleted]   BIT           CONSTRAINT [DF_Role_Is_Deleted] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_Role] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Role__User_Created_By] FOREIGN KEY ([Created_By]) REFERENCES [dbo].[User] ([Id]),
    CONSTRAINT [FK_Role__User_Updated_By] FOREIGN KEY ([Updated_By]) REFERENCES [dbo].[User] ([Id]),
    CONSTRAINT [UC_Role__Name] UNIQUE NONCLUSTERED ([Name] ASC)
);


GO

CREATE NONCLUSTERED INDEX [IX_Role_Is_Deleted] 
	ON [dbo].[Role] ([Is_Deleted])
GO