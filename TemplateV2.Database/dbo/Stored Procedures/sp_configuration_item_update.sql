CREATE PROCEDURE [dbo].[sp_configuration_item_update]
	@Id					INT,
	@Description		VARCHAR(256),
	@Boolean_Value		BIT,
	@DateTime_Value		DATETIME,
	@Date_Value			DATE,
	@Time_Value			TIME,
	@Decimal_Value		DECIMAL(18, 2),
	@Int_Value			INT,
	@Money_Value		DECIMAL(18, 2),
	@String_Value		VARCHAR(MAX),
	@Updated_By			INT
AS
BEGIN
	UPDATE [Configuration]
	SET
        [Description] = @Description,
        [Boolean_Value] = @Boolean_Value,
        [DateTime_Value] = @DateTime_Value,
		[Date_Value] = @Date_Value,
		[Time_Value] = @Time_Value,
        [Decimal_Value] = @Decimal_Value,
        [Int_Value] = @Int_Value,
        [Money_Value] = @Money_Value,
        [String_Value] = @String_Value,
        [Updated_By] = @Updated_By,
        [Updated_Date] = GETDATE()
	WHERE [Id] = @Id

	SELECT
		@Id
END