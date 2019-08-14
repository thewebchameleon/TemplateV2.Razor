CREATE PROCEDURE [dbo].[sp_user_enable]
	@Id    INT,
	@Updated_By INT
AS
BEGIN
   UPDATE [User]
   SET
		[Is_Enabled] = 1,
		[Updated_By] = @Updated_By,
		[Updated_Date] = GETDATE()
   WHERE
		[Id] = @Id
   SELECT
		@Id AS [Id]
END