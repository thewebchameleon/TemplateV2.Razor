CREATE PROCEDURE [dbo].[sp_user_permissions_get_by_user_id]
	@User_Id INT
AS
BEGIN
   SELECT
		[UP].[Id],
		[UP].[User_Id],
		[UP].[Permission_Id],
		[UP].[Created_By],
		[UP].[Created_Date],
		[UP].[Updated_By],
		[UP].[Updated_Date],
		[UP].Is_Deleted
   FROM   [User_Permission] [UP](NOLOCK)
   WHERE  [UP].[User_Id] = @User_Id
   AND [UP].Is_Deleted = 0
END