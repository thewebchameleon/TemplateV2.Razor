CREATE PROCEDURE [dbo].[sp_session_log_events_get_by_user_id]
	@User_Id INT
AS
BEGIN
	SELECT
		[SLE].[Id],
		[SLE].[Session_Log_Id],
		[SLE].[Event_Id],
		[SLE].InfoDictionary_JSON,
		[SLE].[Created_By],
		[SLE].[Created_Date],
		[SLE].[Updated_By],
		[SLE].[Updated_Date],
		[SLE].[Is_Deleted]
	FROM   [Session_Log_Event] [SLE](NOLOCK)
	INNER JOIN [Session_Log] [SL] (NOLOCK)
		ON [SLE].Session_Log_Id = [SL].[Id]
	INNER JOIN [Session] S
		ON [SL].Session_Id = S.Id
	WHERE  [S].[User_Id] = @User_Id
	AND S.[Is_Deleted] = 0
END