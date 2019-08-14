CREATE PROCEDURE [dbo].[sp_user_roles_get]
AS
BEGIN
   SELECT
		[UR].[Id],
		[UR].[Role_Id],
		[UR].[User_Id],
		[UR].[Created_By],
		[UR].[Created_Date],
		[UR].[Updated_By],
		[UR].[Updated_Date],
		[UR].Is_Deleted
   FROM   [User_Role] [UR](NOLOCK)
   WHERE UR.Is_Deleted = 0
END