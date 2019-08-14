CREATE PROCEDURE [dbo].[sp_user_role_delete]
	@User_Id    INT,
	@Role_Id    INT,
	@Updated_By INT
AS
BEGIN
   DECLARE
		@UserRoleId INT;
   UPDATE [User_Role]
   SET
		@UserRoleId = [Id],
		Is_Deleted = 1,
		[Updated_By] = @Updated_By,
		[Updated_Date] = GETDATE()
   WHERE
		[User_Id] = @User_Id
		AND [Role_Id] = @Role_Id
   SELECT
		@UserRoleId AS [Id]
END