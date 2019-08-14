CREATE TABLE [dbo].[User_Token] (
    [Id]           INT					IDENTITY (1, 1) NOT NULL,
    [User_Id]      INT					NOT NULL,
    [Guid]         UNIQUEIDENTIFIER		NOT NULL,
    [Type_Id]      INT					NOT NULL,
	[Processed]	   BIT					NOT NULL,
    [Created_By]   INT					NOT NULL,
    [Created_Date] DATETIME				NOT NULL,
    [Updated_By]   INT					NOT NULL,
    [Updated_Date] DATETIME				NOT NULL,
    [Is_Deleted]   BIT					CONSTRAINT [DF_User_Token_Is_Deleted] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_User_Token] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_User_Token__User_Created_By] FOREIGN KEY ([Created_By]) REFERENCES [dbo].[User] ([Id]),
    CONSTRAINT [FK_User_Token__User_Updated_By] FOREIGN KEY ([Updated_By]) REFERENCES [dbo].[User] ([Id])
);
GO

CREATE NONCLUSTERED INDEX [IX_User_Token_Is_Deleted]
	ON [dbo].[User_Token]([Is_Deleted] ASC)
GO

CREATE UNIQUE NONCLUSTERED INDEX [IX_User_Token_Guid]
	ON [dbo].[User_Token]([Guid] ASC)
GO