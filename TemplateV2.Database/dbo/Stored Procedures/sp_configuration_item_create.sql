CREATE PROCEDURE [dbo].[sp_configuration_item_create]
	@Key				VARCHAR(256),
	@Description		VARCHAR(256),
	@Boolean_Value		BIT,
	@DateTime_Value		DATETIME,
	@Date_Value			DATE,
	@Time_Value			TIME,
	@Decimal_Value		DECIMAL(18, 2),
	@Int_Value			INT,
	@Money_Value		DECIMAL(18, 2),
	@String_Value		VARCHAR(MAX),
	@Created_By			INT
AS
BEGIN
   INSERT INTO [Configuration]
	    (
		[Key],
        [Description],
        [Boolean_Value],
        [DateTime_Value],
		[Date_Value],
		[Time_Value],
        [Decimal_Value],
        [Int_Value],
        [Money_Value],
        [String_Value],
        [Created_By],
        [Created_Date],
        [Updated_By],
        [Updated_Date],
        Is_Deleted
	    )
   VALUES
	    (
		@Key,
		@Description,
		@Boolean_Value,
		@DateTime_Value,
		@Date_Value,
		@Time_Value,
		@Decimal_Value,
		@Int_Value,
		@Money_Value,
		@String_Value,
		@Created_By,
		GETDATE(),
		@Created_By,
		GETDATE(),
		0
	    )
   SELECT
		SCOPE_IDENTITY() AS [Id]
END