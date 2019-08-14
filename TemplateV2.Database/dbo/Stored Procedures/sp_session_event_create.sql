CREATE PROCEDURE [dbo].[sp_session_event_create]
	@Key				VARCHAR(256),
	@Description		VARCHAR(256),
	@Created_By			INT
AS
BEGIN
   INSERT INTO [Session_Event]
	    (
		[Key],
        [Description],
        [Created_By],
        [Created_Date],
        [Updated_By],
        [Updated_Date],
        Is_Deleted
	    )
   VALUES
	    (
		@Key,
		@Description,
		@Created_By,
		GETDATE(),
		@Created_By,
		GETDATE(),
		0
	    )
   SELECT
		SCOPE_IDENTITY() AS [Id]
END