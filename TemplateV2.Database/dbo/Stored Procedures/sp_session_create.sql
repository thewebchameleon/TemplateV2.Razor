CREATE PROCEDURE [dbo].[sp_session_create]
	@User_Agent VARCHAR(256),
	@Created_By INT
AS
BEGIN
   INSERT INTO [Session]
	    (
		[User_Agent],
		[Created_By],
		[Created_Date],
		[Updated_By],
		[Updated_Date],
		Is_Deleted
	    )
   VALUES
	    (
		@User_Agent,
		@Created_By,
		GETDATE(),
		@Created_By,
		GETDATE(),
		0
	    )

	SELECT
		[S].[Id],
		[S].User_Agent,
		[S].[User_Id],
		[S].[Created_By],
		[S].[Created_Date],
		[S].[Updated_By],
		[S].[Updated_Date],
		[S].Is_Deleted
   FROM   [Session] [S](NOLOCK)
   WHERE  [S].[Id] = SCOPE_IDENTITY()
   AND S.Is_Deleted = 0
END