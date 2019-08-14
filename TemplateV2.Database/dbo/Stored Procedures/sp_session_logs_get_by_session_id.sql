CREATE PROCEDURE [dbo].[sp_session_logs_get_by_session_id]
	@Session_Id INT
AS
BEGIN
   SELECT
		[S].[Id],
		[S].[Session_Id],
		[S].[Page],
		[S].Handler_Name,
		[S].[Action],
		[S].[Controller],
		[S].[IsAJAX],
		[S].[Method],
		[S].[Action_Data_JSON],
		[S].[Url],
		[S].[Created_By],
		[S].[Created_Date],
		[S].[Updated_By],
		[S].[Updated_Date],
		[S].[Is_Deleted]
   FROM   [Session_Log] [S] (NOLOCK)
   WHERE  [S].[Session_Id] = @Session_Id
   AND S.[Is_Deleted] = 0
END