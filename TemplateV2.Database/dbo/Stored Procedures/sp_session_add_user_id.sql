CREATE PROCEDURE [dbo].[sp_session_add_user_id]
	@Id			INT NULL,
	@User_Id    INT NULL,
	@Updated_By INT NULL
AS
BEGIN
   UPDATE [Session]
   SET
		[User_Id] = @User_Id,
		[Updated_By] = @Updated_By,
		[Updated_Date] = GETDATE()
   WHERE
		[Id] = @Id

   SELECT
		[S].[Id],
		[S].[User_Id],
		[S].[Created_By],
		[S].[Created_Date],
		[S].[Updated_By],
		[S].[Updated_Date],
		[S].Is_Deleted
   FROM   [Session] [S](NOLOCK)
   WHERE  [Id] = @Id
   AND S.Is_Deleted = 0
END