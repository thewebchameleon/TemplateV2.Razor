CREATE PROCEDURE [dbo].[sp_session_event_update]
	@Id					INT,
	@Description		VARCHAR(256),
	@Updated_By			INT
AS
BEGIN
	UPDATE [Session_Event]
	SET
        [Description] = @Description,
        [Updated_By] = @Updated_By,
        [Updated_Date] = GETDATE()
	WHERE [Id] = @Id

	SELECT
		@Id
END