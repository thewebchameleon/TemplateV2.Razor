CREATE PROCEDURE [dbo].[sp_session_log_create]
	@Session_Id			INT,
	@Method				VARCHAR(10),
	@Page				VARCHAR(256),
	@Handler_Name		VARCHAR(256),
	@Controller			VARCHAR(256),
	@Action				VARCHAR(256),
	@IsAJAX				BIT,
	@Action_Data_JSON	VARCHAR(MAX),
	@Url				VARCHAR(256),
	@Created_By			INT
AS
BEGIN
   INSERT INTO [Session_Log]
	    (
		[Session_Id],
		[Method],
		[Page],
		[Handler_Name],
		[Controller],
        [Action],
		[IsAJAX],
		[Action_Data_JSON],
		[Url],
        [Created_By],
        [Created_Date],
        [Updated_By],
        [Updated_Date],
        Is_Deleted
	    )
   VALUES
	    (
		@Session_Id,
		@Method,
		@Page,
		@Handler_Name,
		@Controller,
		@Action,
		@IsAJAX,
		@Action_Data_JSON,
		@Url,
		@Created_By,
		GETDATE(),
		@Created_By,
		GETDATE(),
		0
	    )
   SELECT
		SCOPE_IDENTITY() AS [Id]
END