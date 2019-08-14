CREATE PROCEDURE [dbo].[sp_user_roles_get_by_user_id]
	@User_Id INT
AS
BEGIN
   SELECT
		[UR].[Id],
		[UR].[User_Id],
		[UR].[Role_Id],
		[UR].[Created_By],
		[UR].[Created_Date],
		[UR].[Updated_By],
		[UR].[Updated_Date],
		[UR].Is_Deleted
   FROM   [User_Role] [UR](NOLOCK)
   WHERE  [UR].[User_Id] = @User_Id
   AND UR.Is_Deleted = 0
END