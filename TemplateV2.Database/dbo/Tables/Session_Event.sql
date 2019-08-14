CREATE TABLE [dbo].[Session_Event] (
    [Id]           INT           IDENTITY (1, 1) NOT NULL,
    [Key]          VARCHAR (256) NOT NULL,
    [Description]  VARCHAR (256) NOT NULL,
    [Created_By]   INT           NOT NULL,
    [Created_Date] DATETIME      NOT NULL,
    [Updated_By]   INT           NOT NULL,
    [Updated_Date] DATETIME      NOT NULL,
    [Is_Deleted]   BIT           CONSTRAINT [DF_Session_Event_Is_Deleted] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_Event] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Event__User_Created_By] FOREIGN KEY ([Created_By]) REFERENCES [dbo].[User] ([Id]),
    CONSTRAINT [FK_Event__User_Updated_By] FOREIGN KEY ([Updated_By]) REFERENCES [dbo].[User] ([Id])
);



GO
CREATE NONCLUSTERED INDEX [IX_Event_Is_Deleted]
    ON [dbo].[Session_Event]([Is_Deleted] ASC);
GO

CREATE NONCLUSTERED INDEX [IX_Event_Key]
    ON [dbo].[Session_Event]([Key] ASC);
GO