CREATE PROCEDURE [dbo].[sp_user_token_get_by_guid]
	@Guid UNIQUEIDENTIFIER
AS
BEGIN
   SELECT TOP 1
		[UT].[Id],
		[UT].[User_Id],
		[UT].[Guid],
		[UT].[Type_Id],
		[UT].[Processed],
		[UT].[Created_By],
		[UT].[Created_Date],
		[UT].[Updated_By],
		[UT].[Updated_Date],
		[UT].Is_Deleted
   FROM   [User_Token] [UT](NOLOCK)
   WHERE  [UT].[Guid] = @Guid
   AND UT.Is_Deleted = 0
END