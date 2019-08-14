CREATE PROCEDURE [dbo].[sp_role_permission_delete]
	@Role_Id     INT,
	@Permission_Id	VARCHAR(256),
	@Updated_By  INT
AS
BEGIN
   DECLARE
		@Id INT;
   UPDATE [Role_Permission]
   SET
		@Id = [Id],
		Is_Deleted = 1,
		[Updated_By] = @Updated_By,
		[Updated_Date] = GETDATE()
   WHERE
		[Role_Id] = @Role_Id
		AND [Permission_Id] = @Permission_Id
   SELECT
		@Id AS [Id]
END