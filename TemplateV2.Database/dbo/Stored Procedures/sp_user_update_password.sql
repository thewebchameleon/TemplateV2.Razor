CREATE PROCEDURE [dbo].[sp_user_update_password]
	@User_Id					INT,
	@Password_Hash				VARCHAR(MAX),
	@Updated_By					INT
AS
BEGIN
   UPDATE [User]
   SET
		[Password_Hash] = ISNULL(@Password_Hash, [Password_Hash]),
		[Updated_By] = @Updated_By,
		[Updated_Date] = GETDATE()
   WHERE
		[Id] = @User_Id
   SELECT
		@User_Id
END