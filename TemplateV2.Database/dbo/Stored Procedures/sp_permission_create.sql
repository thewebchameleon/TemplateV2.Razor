CREATE PROCEDURE [dbo].[sp_permission_create]
	@Key				VARCHAR(256),
	@Name				VARCHAR(256),
	@Group_Name			VARCHAR(256),
	@Description		VARCHAR(256),
	@Created_By			INT
AS
BEGIN
   INSERT INTO [Permission]
	    (
		[Key],
		[Name],
		[Group_Name],
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
		@Name,
		@Group_Name,
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