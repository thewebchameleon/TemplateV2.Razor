CREATE PROCEDURE [dbo].[sp_user_role_create]
	@User_Id    INT,
	@Role_Id    INT,
	@Created_By INT
AS
BEGIN

	 -- find existing record else create a new one
	IF EXISTS
	(
		SELECT
			1
		FROM   [User_Role] (NOLOCK)
		WHERE  [User_Id] = @User_Id
			 AND [Role_Id] = @Role_Id
	)
	BEGIN

		-- reactivate deleted record
		DECLARE @Id INT;

		UPDATE	[User_Role]
		SET 
				@Id = [Id],
				[Is_Deleted] = 0,
				Updated_By = Created_By,
				Updated_Date = GETDATE()
		WHERE   [User_Id] = @User_Id
		AND		[Role_Id] = @Role_Id

		SELECT @Id AS [Id]
	END 
	ELSE
	BEGIN
	    INSERT INTO [User_Role]
	    (
			 [User_Id],
			 [Role_Id],
			 [Created_By],
			 [Created_Date],
			 [Updated_By],
			 [Updated_Date],
			 Is_Deleted
	    )
	    VALUES
	    (
			 @User_Id,
			 @Role_Id,
			 @Created_By,
			 GETDATE(),
			 @Created_By,
			 GETDATE(),
			 0
	    )
	    SELECT
			 SCOPE_IDENTITY() AS [Id]
	 END
END