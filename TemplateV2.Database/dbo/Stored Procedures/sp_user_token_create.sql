CREATE PROCEDURE [dbo].[sp_user_token_create]
	@User_Id     INT,
	@Token      VARCHAR(256),
	@Type_Id     INT,
	@Created_By  INT
AS
BEGIN
	INSERT INTO [dbo].[User_Token]
	(
		[User_Id],
		[Guid],
		[Type_Id],
		[Processed],
		[Created_By],
		[Created_Date],
		[Updated_By],
		[Updated_Date],
		Is_Deleted
	)
	VALUES
	(
		@User_Id,
		@Token,
		@Type_Id,
		0,
		@Created_By,
		GETDATE(),
		@Created_By,
		GETDATE(),
		0
	);
	SELECT SCOPE_IDENTITY() AS [Id];
END;