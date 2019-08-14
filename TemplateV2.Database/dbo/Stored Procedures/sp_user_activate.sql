CREATE PROCEDURE [dbo].[sp_user_activate]
	@Id					INT,
	@Updated_By					INT
AS
BEGIN
   UPDATE [User]
   SET
		[Registration_Confirmed] = 1,
		[Updated_By] = @Updated_By,
		[Updated_Date] = GETDATE()
   WHERE
		[Id] = @Id
   SELECT
		@Id
END