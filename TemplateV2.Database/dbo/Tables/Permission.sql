CREATE TABLE [dbo].[Permission] (
    [Id]           INT           IDENTITY (1, 1) NOT NULL,
    [Key]          VARCHAR (256) NOT NULL,
    [Group_Name]   VARCHAR (256) NOT NULL,
    [Name]         VARCHAR (256) NOT NULL,
    [Description]  VARCHAR (256) NOT NULL,
    [Created_By]   INT           NOT NULL,
    [Created_Date] DATETIME      NOT NULL,
    [Updated_By]   INT           NOT NULL,
    [Updated_Date] DATETIME      NOT NULL,
    [Is_Deleted]   BIT           CONSTRAINT [DF_Permission_Is_Deleted] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_Permission] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Permission__User_Created_By] FOREIGN KEY ([Created_By]) REFERENCES [dbo].[User] ([Id]),
    CONSTRAINT [FK_Permission__User_Updated_By] FOREIGN KEY ([Updated_By]) REFERENCES [dbo].[User] ([Id])
);


GO

CREATE UNIQUE NONCLUSTERED INDEX [IX_Permission_Key]
    ON [dbo].[Permission]([Key] ASC);
GO

CREATE NONCLUSTERED INDEX [IX_Permission_Is_Deleted] 
	ON [dbo].[Permission] ([Is_Deleted])
GO