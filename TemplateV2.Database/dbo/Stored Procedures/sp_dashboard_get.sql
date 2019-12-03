CREATE PROCEDURE [dbo].[sp_dashboard_get]
AS
BEGIN

	DECLARE @TotalSessions INT,
			@TotalUsers INT,
			@TotalRoles INT,
			@TotalConfigItems INT;

	SELECT @TotalSessions = COUNT(1)
	FROM [Session] (NOLOCK)

	SELECT @TotalUsers = COUNT(1)
	FROM [User] (NOLOCK)

	SELECT @TotalRoles = COUNT(1)
	FROM [Role] (NOLOCK)

	SELECT @TotalConfigItems = COUNT(1)
	FROM [Configuration] (NOLOCK)

	SELECT 
		@TotalSessions AS TotalSessions, 
		@TotalUsers AS TotalUsers, 
		@TotalRoles AS TotalRoles, 
		@TotalConfigItems AS TotalConfigItems 

END