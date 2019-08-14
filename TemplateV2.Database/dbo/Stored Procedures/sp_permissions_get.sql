CREATE PROCEDURE [dbo].[sp_permissions_get]
AS
BEGIN
   SELECT
		[C].[Id],
		[C].[Key],
		[C].[Group_Name],
		[C].[Name],
		[C].[Description],
		[C].[Created_By],
		[C].[Created_Date],
		[C].[Updated_By],
		[C].[Updated_Date],
		[C].Is_Deleted
   FROM   [Permission] [C](NOLOCK)
   WHERE [C].Is_Deleted = 0
END