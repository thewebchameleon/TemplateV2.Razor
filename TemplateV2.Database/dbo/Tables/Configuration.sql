CREATE TABLE [dbo].[Configuration] (
    [Id]				INT             IDENTITY (1, 1) NOT NULL,
    [Key]				VARCHAR (256)   NOT NULL,
    [Description]		VARCHAR (256)   NOT NULL,
    [Boolean_Value]		BIT             NULL,
    [DateTime_Value]	DATETIME        NULL,
	[Date_Value]		DATE			NULL,
	[Time_Value]		TIME			NULL,
    [Decimal_Value]		DECIMAL (18, 2) NULL,
    [Int_Value]			INT             NULL,
    [Money_Value]		DECIMAL (18, 2) NULL,
    [String_Value]		VARCHAR (MAX)   NULL,
    [Created_By]		INT             NOT NULL,
    [Created_Date]		DATETIME        NOT NULL,
    [Updated_By]		INT             NOT NULL,
    [Updated_Date]		DATETIME        NOT NULL,
    [Is_Deleted]		BIT             CONSTRAINT [DF_Configuration_Is_Deleted] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_Configuration] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Configuration__User_Created_By] FOREIGN KEY ([Created_By]) REFERENCES [dbo].[User] ([Id]),
    CONSTRAINT [FK_Configuration__User_Updated_By] FOREIGN KEY ([Updated_By]) REFERENCES [dbo].[User] ([Id])
);
GO

CREATE UNIQUE NONCLUSTERED INDEX [IX_Configuration_Key]
    ON [dbo].[Configuration]([Key] ASC);
GO

CREATE NONCLUSTERED INDEX [IX_Configuration_Is_Deleted]
    ON [dbo].[Configuration]([Is_Deleted] ASC);
GO