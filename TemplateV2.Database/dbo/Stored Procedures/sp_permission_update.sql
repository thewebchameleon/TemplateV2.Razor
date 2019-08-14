CREATE PROCEDURE [dbo].[sp_permission_update]
	@Id					INT,
	@Name				VARCHAR(256),
	@Group_Name			VARCHAR(256),
	@Description		VARCHAR(256),
	@Updated_By			INT
AS
BEGIN
	UPDATE [Permission]
	SET
        [Description] = @Description,
		[Name] = @Name,
		[Group_Name] = @Group_Name,
        [Updated_By] = @Updated_By,
        [Updated_Date] = GETDATE()
	WHERE [Id] = @Id

	SELECT
		@Id
END