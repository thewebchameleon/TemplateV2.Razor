CREATE PROCEDURE [dbo].[sp_user_duplicate_check]
	@Email_Address		VARCHAR(256) NULL,
	@Mobile_Number		VARCHAR(30) NULL,
	@Username			VARCHAR(256) NULL,
	@User_Id			INT NULL
AS
BEGIN
   SELECT TOP 1
		[U].[Id],
		[U].[Username],
		[U].[Email_Address],
		[U].[Mobile_Number],
		[U].[First_Name],
		[U].[Last_Name],
		[U].[Registration_Confirmed],
		[U].[Password_Hash],
		[U].[Is_Locked_Out],
		[U].[Lockout_End],
		[U].[Invalid_Login_Attempts],
		[U].[Created_By],
		[U].[Created_Date],
		[U].[Updated_By],
		[U].[Updated_Date],
		[U].Is_Deleted,
		[U].Is_Enabled
   FROM   [User] [U](NOLOCK)
   WHERE  ([U].[Email_Address] = @Email_Address
		 OR [U].[Mobile_Number] = @Mobile_Number
		 OR [U].[Username] = @Username)
		AND [U].[Id] != ISNULL(@User_Id, 0)
END