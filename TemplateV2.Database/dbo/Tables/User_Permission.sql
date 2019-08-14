CREATE TABLE [dbo].[User_Permission] (
    [Id]           INT      IDENTITY (1, 1) NOT NULL,
	[User_Id]		INT      NOT NULL,
    [Permission_Id]     INT      NOT NULL,
    [Created_By]   INT      NOT NULL,
    [Created_Date] DATETIME NOT NULL,
    [Updated_By]   INT      NOT NULL,
    [Updated_Date] DATETIME NOT NULL,
    [Is_Deleted]   BIT      CONSTRAINT [DF_User_Permission_Is_Deleted] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_User_Permission] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_User_Permission__Permission_Permission_Id] FOREIGN KEY ([Permission_Id]) REFERENCES [dbo].[Permission] ([Id]),
    CONSTRAINT [FK_User_Permission__User_User_Id] FOREIGN KEY ([User_Id]) REFERENCES [dbo].[User] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_User_Permission__User_Created_By] FOREIGN KEY ([Created_By]) REFERENCES [dbo].[User] ([Id]),
    CONSTRAINT [FK_User_Permission__User_Updated_By] FOREIGN KEY ([Updated_By]) REFERENCES [dbo].[User] ([Id]),
    CONSTRAINT [UC_User_Permission__Permission_Id_User_Id] UNIQUE NONCLUSTERED ([Permission_Id] ASC, [User_Id] ASC)
);


GO

CREATE NONCLUSTERED INDEX [IX_User_Permission_Is_Deleted]
	ON [dbo].[User_Permission]([Is_Deleted] ASC)
GO


