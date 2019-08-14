CREATE TABLE [dbo].[Session_Log_Event] (
    [Id]             INT           IDENTITY (1, 1) NOT NULL,
    [Session_Log_Id] INT           NOT NULL,
    [Event_Id]       INT           NOT NULL,
    [InfoDictionary_JSON]        VARCHAR (MAX) NULL,
    [Created_By]     INT           NOT NULL,
    [Created_Date]   DATETIME      NOT NULL,
    [Updated_By]     INT           NOT NULL,
    [Updated_Date]   DATETIME      NOT NULL,
    [Is_Deleted]     BIT           CONSTRAINT [DF_Session_Log_Event_Is_Deleted] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_Session_Log_Event] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Session_Log_Event__Event_Id] FOREIGN KEY ([Event_Id]) REFERENCES [dbo].[Session_Event] ([Id]),
    CONSTRAINT [FK_Session_Log_Event__Session_Session_Log_Id] FOREIGN KEY ([Session_Log_Id]) REFERENCES [dbo].[Session_Log] ([Id]),
    CONSTRAINT [FK_Session_Log_Event__User_Created_By] FOREIGN KEY ([Created_By]) REFERENCES [dbo].[User] ([Id]),
    CONSTRAINT [FK_Session_Log_Event__User_Updated_By] FOREIGN KEY ([Updated_By]) REFERENCES [dbo].[User] ([Id])
);



GO
CREATE NONCLUSTERED INDEX [IX_Session_Log_Event_Is_Deleted]
    ON [dbo].[Session_Log_Event]([Is_Deleted] ASC);
GO

CREATE NONCLUSTERED INDEX [IX_Session_Log_Event_Session_Log_Id]
    ON [dbo].[Session_Log_Event]([Session_Log_Id] ASC);
GO