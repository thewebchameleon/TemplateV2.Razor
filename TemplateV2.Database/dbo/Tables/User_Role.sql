CREATE TABLE [dbo].[User_Role] (
    [Id]           INT      IDENTITY (1, 1) NOT NULL,
    [User_Id]      INT      NOT NULL,
    [Role_Id]      INT      NOT NULL,
    [Created_By]   INT      NOT NULL,
    [Created_Date] DATETIME NOT NULL,
    [Updated_By]   INT      NOT NULL,
    [Updated_Date] DATETIME NOT NULL,
    [Is_Deleted]   BIT      CONSTRAINT [DF_User_Role_Is_Deleted] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_User_Role] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_User_Role__Role] FOREIGN KEY ([Role_Id]) REFERENCES [dbo].[Role] ([Id]),
    CONSTRAINT [FK_User_Role__User_Created_By] FOREIGN KEY ([Created_By]) REFERENCES [dbo].[User] ([Id]),
    CONSTRAINT [FK_User_Role__User_Updated_By] FOREIGN KEY ([Updated_By]) REFERENCES [dbo].[User] ([Id])
);


GO

CREATE NONCLUSTERED INDEX [IX_User_Role_Is_Deleted]
	ON [dbo].[User_Role]([Is_Deleted] ASC)
GO

CREATE UNIQUE NONCLUSTERED INDEX [IX_User_Role__User_Id_Role_Id]
    ON [dbo].[User_Role]([User_Id] ASC, [Role_Id] ASC);
GO