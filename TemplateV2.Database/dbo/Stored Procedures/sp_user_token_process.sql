CREATE PROCEDURE [dbo].[sp_user_token_process]
	@Guid        UNIQUEIDENTIFIER,
	@Updated_By  INT
AS
BEGIN
	UPDATE [User_Token]
	SET 
		[Processed] = 1,
		[Updated_By] = @Updated_By,
		[Updated_Date] = GETDATE()
	WHERE [Guid] = @Guid
END;