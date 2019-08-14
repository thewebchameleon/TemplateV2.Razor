CREATE PROCEDURE [dbo].[sp_role_create]
	@Name           VARCHAR(256),
	@Description    VARCHAR(256),
	@Created_By      INT
AS
BEGIN
   INSERT INTO [Role]
	    (
		[Name],
		[Description],
		[Created_By],
		[Created_Date],
		[Updated_By],
		[Updated_Date],
		Is_Enabled,
		Is_Deleted
	    )
   VALUES
	    (
		@Name,
		@Description,
		@Created_By,
		GETDATE(),
		@Created_By,
		GETDATE(),
		1,
		0
	    )
   SELECT
		SCOPE_IDENTITY() AS [Id]
END