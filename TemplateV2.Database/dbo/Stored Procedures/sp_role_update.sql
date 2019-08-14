CREATE PROCEDURE [dbo].[sp_role_update]
	@Id             INT,
	@Name           VARCHAR(256),
	@Description    VARCHAR(256),
	@Updated_By      INT
AS
BEGIN
   UPDATE [Role]
   SET
		[Name] = @Name,
		[Description] = @Description,
		[Updated_By] = @Updated_By,
		[Updated_Date] = GETDATE()
   WHERE
		[Id] = @Id
   SELECT
		@Id
END