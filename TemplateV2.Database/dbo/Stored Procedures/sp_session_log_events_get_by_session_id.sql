CREATE PROCEDURE [dbo].[sp_session_log_events_get_by_session_id]
	@Session_Id INT
AS
BEGIN
	SELECT
		[S].[Id],
		[S].[Session_Log_Id],
		[S].[Event_Id],
		[S].InfoDictionary_JSON,
		[S].[Created_By],
		[S].[Created_Date],
		[S].[Updated_By],
		[S].[Updated_Date],
		[S].[Is_Deleted]
	FROM   [Session_Log_Event] [S](NOLOCK)
	INNER JOIN [Session_Log] [SL] (NOLOCK)
		ON [S].Session_Log_Id = [SL].[Id]
	WHERE  [SL].[Session_Id] = @Session_Id
	AND S.[Is_Deleted] = 0
END