CREATE PROCEDURE [dbo].[sp_user_add_invalid_login_attempt]
	@User_Id    INT,
	@Lockout_End DATETIME NULL,
	@Updated_By INT
AS
BEGIN
   UPDATE [User]
   SET
		[Invalid_Login_Attempts] = [Invalid_Login_Attempts] + 1,
		[Lockout_End] = @Lockout_End,
		[Updated_By] = @Updated_By,
		[Updated_Date] = GETDATE()
   WHERE
		[Id] = @User_Id
END