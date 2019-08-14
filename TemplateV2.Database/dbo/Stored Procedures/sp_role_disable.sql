CREATE PROCEDURE [dbo].[sp_role_disable]
	@Id        INT,
	@Updated_By INT
AS
BEGIN
   UPDATE [Role]
   SET
		[Is_Enabled] = 0,
		[Updated_By] = @Updated_By,
		[Updated_Date] = GETDATE()
   WHERE
		[Id] = @Id
   SELECT
		@Id
END