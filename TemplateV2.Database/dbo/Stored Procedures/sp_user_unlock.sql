CREATE PROCEDURE [dbo].[sp_user_unlock]
	@Id    INT,
	@Updated_By INT
AS
BEGIN
   UPDATE [User]
   SET
		[Is_Locked_Out] = 0,
		[Invalid_Login_Attempts] = 0,
		[Lockout_End] = NULL,
		[Updated_By] = @Updated_By,
		[Updated_Date] = GETDATE()
   WHERE
		[Id] = @Id
   SELECT
		@Id AS [Id]
END