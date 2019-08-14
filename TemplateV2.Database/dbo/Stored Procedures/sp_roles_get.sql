CREATE PROCEDURE [dbo].[sp_roles_get]
AS
BEGIN
   SELECT
		[R].[Id],
		[R].[Name],
		[R].[Description],
		[R].[Created_By],
		[R].[Created_Date],
		[R].[Updated_By],
		[R].[Updated_Date],
		[R].[Is_Enabled],
		[R].Is_Deleted
   FROM   [Role] [R](NOLOCK)
   WHERE R.Is_Deleted = 0
END