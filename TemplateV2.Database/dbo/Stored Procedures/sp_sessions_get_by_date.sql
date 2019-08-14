CREATE PROCEDURE [dbo].[sp_sessions_get_by_date]
	@Date DATE
AS
BEGIN
	SELECT
		[S].[Id],
		[S].[User_Id],
		[S].[Created_By],
		[S].[Created_Date],
		[S].[Updated_By],
		[S].[Updated_Date],
		[S].Is_Deleted,

		[U].[Username],
		MAX(SL.Updated_Date) AS Last_Session_Log_Date,
		MAX(SLE.Updated_Date) AS Last_Session_Event_Date
	FROM   [Session] [S] (NOLOCK)
	LEFT JOIN [User] [U] (NOLOCK)
		ON [S].[User_Id] = [U].[Id]
	LEFT JOIN [Session_Log] SL (NOLOCK)
		ON S.Id = SL.Session_Id
	LEFT JOIN [Session_Log_Event] SLE (NOLOCK)
		ON SL.Id = SLE.Session_Log_Id
	WHERE [S].[Is_Deleted] = 0
	AND CONVERT(DATE, S.Created_Date) = @Date
	GROUP BY
		[S].[Id],
		[S].[User_Id],
		[S].[Created_By],
		[S].[Created_Date],
		[S].[Updated_By],
		[S].[Updated_Date],
		[S].Is_Deleted,

		[U].[Username]
	ORDER BY [S].[Created_Date] DESC
END