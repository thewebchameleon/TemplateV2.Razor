CREATE TABLE [dbo].[Role_Permission] (
    [Id]           INT      IDENTITY (1, 1) NOT NULL,
    [Role_Id]      INT      NOT NULL,
    [Permission_Id]     INT      NOT NULL,
    [Created_By]   INT      NOT NULL,
    [Created_Date] DATETIME NOT NULL,
    [Updated_By]   INT      NOT NULL,
    [Updated_Date] DATETIME NOT NULL,
    [Is_Deleted]   BIT      CONSTRAINT [DF_Role_Permission_Is_Deleted] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_Role_Permission] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Role_Permission__Permission_Permission_Id] FOREIGN KEY ([Permission_Id]) REFERENCES [dbo].[Permission] ([Id]),
    CONSTRAINT [FK_Role_Permission__Role_Role_Id] FOREIGN KEY ([Role_Id]) REFERENCES [dbo].[Role] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_Role_Permission__User_Created_By] FOREIGN KEY ([Created_By]) REFERENCES [dbo].[User] ([Id]),
    CONSTRAINT [FK_Role_Permission__User_Updated_By] FOREIGN KEY ([Updated_By]) REFERENCES [dbo].[User] ([Id]),
    CONSTRAINT [UC_Role_Permission__Permission_Id_Role_Id] UNIQUE NONCLUSTERED ([Permission_Id] ASC, [Role_Id] ASC)
);


GO

CREATE NONCLUSTERED INDEX [IX_Role_Permission_Is_Deleted]
	ON [dbo].[Role_Permission]([Is_Deleted] ASC)
GO


