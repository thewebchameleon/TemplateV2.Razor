CREATE PROCEDURE [dbo].[sp_user_lockout]
	@Id    INT,
	@Lockout_End DATETIME,
	@Updated_By INT
AS
BEGIN
   UPDATE [User]
   SET
		[Is_Locked_Out] = 1,
		[Lockout_End] = @Lockout_End,
		[Updated_By] = @Updated_By,
		[Updated_Date] = GETDATE()
   WHERE
		[Id] = @Id
   SELECT
		@Id AS [Id]
END