CREATE PROCEDURE [dbo].[sp_role_permissions_get]
AS
BEGIN
   SELECT
		[RC].[Id],
		[RC].[Permission_Id],
		[RC].[Role_Id],
		[RC].[Created_By],
		[RC].[Created_Date],
		[RC].[Updated_By],
		[RC].[Updated_Date],
		[RC].Is_Deleted
   FROM [Role_Permission] [RC](NOLOCK)
   WHERE RC.Is_Deleted = 0
END