/*
	This is a deployment script for the TemplateV2.MVC application.
	It contains lookups and the creation of an admin user which you can configure below

	The [Password_Hash] in the [User table] is generated with BCrypt which is used in this template
*/

--add system user
INSERT INTO [User]
([Username], [Email_Address], [Registration_Confirmed], [First_Name], [Password_Hash], [Is_Enabled], [Created_By], [Created_Date], [Updated_By], [Updated_Date])
VALUES ('system', 'system@example.com', 1, 'System', 'this-password-will-never-work', 1, NULL, GETDATE(), NULL, GETDATE())

--add session events
INSERT INTO [Session_Event]
([Key], [Description], [Created_By], [Created_Date], [Updated_By], [Updated_Date])
VALUES ('USER_LOGGED_IN', 'Signed in', 1, GETDATE(), 1, GETDATE())

INSERT INTO [Session_Event]
([Key], [Description], [Created_By], [Created_Date], [Updated_By], [Updated_Date])
VALUES ('USER_UPDATED_PROFILE', 'Updated profile', 1, GETDATE(), 1, GETDATE())

INSERT INTO [Session_Event]
([Key], [Description], [Created_By], [Created_Date], [Updated_By], [Updated_Date])
VALUES ('ERROR', 'An exception occurred', 1, GETDATE(), 1, GETDATE())

INSERT INTO [Session_Event]
([Key], [Description], [Created_By], [Created_Date], [Updated_By], [Updated_Date])
VALUES ('USER_REGISTERED', 'Registered', 1, GETDATE(), 1, GETDATE())

INSERT INTO [Session_Event]
([Key], [Description], [Created_By], [Created_Date], [Updated_By], [Updated_Date])
VALUES ('PERMISSION_CREATED', 'Created permission', 1, GETDATE(), 1, GETDATE())

INSERT INTO [Session_Event]
([Key], [Description], [Created_By], [Created_Date], [Updated_By], [Updated_Date])
VALUES ('SESSION_EVENT_CREATED', 'Created session event', 1, GETDATE(), 1, GETDATE())

INSERT INTO [Session_Event]
([Key], [Description], [Created_By], [Created_Date], [Updated_By], [Updated_Date])
VALUES ('PERMISSION_UPDATED', 'Updated permission', 1, GETDATE(), 1, GETDATE())

INSERT INTO [Session_Event]
([Key], [Description], [Created_By], [Created_Date], [Updated_By], [Updated_Date])
VALUES ('CONFIGURATION_CREATED', 'Created configuration item', 1, GETDATE(), 1, GETDATE())

INSERT INTO [Session_Event]
([Key], [Description], [Created_By], [Created_Date], [Updated_By], [Updated_Date])
VALUES ('CONFIGURATION_UPDATED', 'Updated configuration item', 1, GETDATE(), 1, GETDATE())

INSERT INTO [Session_Event]
([Key], [Description], [Created_By], [Created_Date], [Updated_By], [Updated_Date])
VALUES ('ROLES_PERMISSIONS_UPDATED', 'Updated role permissions', 1, GETDATE(), 1, GETDATE())

INSERT INTO [Session_Event]
([Key], [Description], [Created_By], [Created_Date], [Updated_By], [Updated_Date])
VALUES ('ROLE_CREATED', 'Created role', 1, GETDATE(), 1, GETDATE())

INSERT INTO [Session_Event]
([Key], [Description], [Created_By], [Created_Date], [Updated_By], [Updated_Date])
VALUES ('ROLE_UPDATED', 'Updated role', 1, GETDATE(), 1, GETDATE())

INSERT INTO [Session_Event]
([Key], [Description], [Created_By], [Created_Date], [Updated_By], [Updated_Date])
VALUES ('ROLE_DISABLED', 'Disabled role', 1, GETDATE(), 1, GETDATE())

INSERT INTO [Session_Event]
([Key], [Description], [Created_By], [Created_Date], [Updated_By], [Updated_Date])
VALUES ('ROLE_ENABLED', 'Enabled role', 1, GETDATE(), 1, GETDATE())

INSERT INTO [Session_Event]
([Key], [Description], [Created_By], [Created_Date], [Updated_By], [Updated_Date])
VALUES ('USER_CREATED', 'Created user', 1, GETDATE(), 1, GETDATE())

INSERT INTO [Session_Event]
([Key], [Description], [Created_By], [Created_Date], [Updated_By], [Updated_Date])
VALUES ('USER_UPDATED', 'Updated user', 1, GETDATE(), 1, GETDATE())

INSERT INTO [Session_Event]
([Key], [Description], [Created_By], [Created_Date], [Updated_By], [Updated_Date])
VALUES ('USER_DISABLED', 'Disabled user', 1, GETDATE(), 1, GETDATE())

INSERT INTO [Session_Event]
([Key], [Description], [Created_By], [Created_Date], [Updated_By], [Updated_Date])
VALUES ('USER_ENABLED', 'Enabled user', 1, GETDATE(), 1, GETDATE())

INSERT INTO [Session_Event]
([Key], [Description], [Created_By], [Created_Date], [Updated_By], [Updated_Date])
VALUES ('USER_ROLES_UPDATED', 'Updated user roles', 1, GETDATE(), 1, GETDATE())

INSERT INTO [Session_Event]
([Key], [Description], [Created_By], [Created_Date], [Updated_By], [Updated_Date])
VALUES ('SESSION_EVENT_UPDATED', 'Updated session event', 1, GETDATE(), 1, GETDATE())

INSERT INTO [Session_Event]
([Key], [Description], [Created_By], [Created_Date], [Updated_By], [Updated_Date])
VALUES ('USER_LOCKED', 'Locked user', 1, GETDATE(), 1, GETDATE())

INSERT INTO [Session_Event]
([Key], [Description], [Created_By], [Created_Date], [Updated_By], [Updated_Date])
VALUES ('USER_UNLOCKED', 'Unlocked user', 1, GETDATE(), 1, GETDATE())

INSERT INTO [Session_Event]
([Key], [Description], [Created_By], [Created_Date], [Updated_By], [Updated_Date])
VALUES ('USER_ACTIVATED', 'Activated user', 1, GETDATE(), 1, GETDATE())

INSERT INTO [Session_Event]
([Key], [Description], [Created_By], [Created_Date], [Updated_By], [Updated_Date])
VALUES ('PASSWORD_UPDATED', 'Password updated', 1, GETDATE(), 1, GETDATE())

INSERT INTO [Session_Event]
([Key], [Description], [Created_By], [Created_Date], [Updated_By], [Updated_Date])
VALUES ('PASSWORD_RESET_URL_GENERATED', 'Reset password URL generated', 1, GETDATE(), 1, GETDATE())

INSERT INTO [Session_Event]
([Key], [Description], [Created_By], [Created_Date], [Updated_By], [Updated_Date])
VALUES ('PASSWORD_RESET_EMAIL_SENT', 'Reset password email sent', 1, GETDATE(), 1, GETDATE())


--add configuration
INSERT INTO [Configuration]
([Key], [Description], [Is_Client_Side], [Boolean_Value], [DateTime_Value], [Decimal_Value], [Int_Value], [Money_Value], [String_Value], [Created_By], [Created_Date], [Updated_By], [Updated_Date])
VALUES ('SESSION_LOGGING_IS_ENABLED', 'Feature switch for session and event tracking', 0, 1, NULL, NULL, NULL, NULL, NULL, 1, GETDATE(), 1, GETDATE())

INSERT INTO [Configuration]
([Key], [Description], [Is_Client_Side], [Boolean_Value], [DateTime_Value], [Decimal_Value], [Int_Value], [Money_Value], [String_Value], [Created_By], [Created_Date], [Updated_By], [Updated_Date])
VALUES ('HOME_PROMO_BANNER_IS_ENABLED', 'Feature switch for a promotional banner on the home page', 0, 1, NULL, NULL, NULL, NULL, NULL, 1, GETDATE(), 1, GETDATE())

INSERT INTO [Configuration]
([Key], [Description], [Is_Client_Side], [Boolean_Value], [DateTime_Value], [Decimal_Value], [Int_Value], [Money_Value], [String_Value], [Created_By], [Created_Date], [Updated_By], [Updated_Date])
VALUES ('ACCOUNT_LOCKOUT_EXPIRY_MINUTES', 'The amount of time before a locked out user can login again', 0, NULL, NULL, NULL, 10, NULL, NULL, 1, GETDATE(), 1, GETDATE())

INSERT INTO [Configuration]
([Key], [Description], [Is_Client_Side], [Boolean_Value], [DateTime_Value], [Decimal_Value], [Int_Value], [Money_Value], [String_Value], [Created_By], [Created_Date], [Updated_By], [Updated_Date])
VALUES ('MAX_LOGIN_ATTEMPTS', 'The amount of invalid password login attempts that a user may perform', 0, NULL, NULL, NULL, 1, NULL, NULL, 1, GETDATE(), 1, GETDATE())

INSERT INTO [Configuration]
([Key], [Description], [Is_Client_Side], [Boolean_Value], [DateTime_Value], [Decimal_Value], [Int_Value], [Money_Value], [String_Value], [Created_By], [Created_Date], [Updated_By], [Updated_Date])
VALUES ('SYSTEM_FROM_EMAIL_ADDRESS', 'The email address used for emails from the system', 0, NULL, NULL, NULL, NULL, NULL, 'TemplateV2.mvc@example.com', 1, GETDATE(), 1, GETDATE())

INSERT INTO [Configuration]
([Key], [Description], [Is_Client_Side], [Boolean_Value], [DateTime_Value], [Decimal_Value], [Int_Value], [Money_Value], [String_Value], [Created_By], [Created_Date], [Updated_By], [Updated_Date])
VALUES ('CONTACT_EMAIL_ADDRESS', 'The email address used for receiving contact messages', 0, NULL, NULL, NULL, NULL, NULL, 'contact.TemplateV2.mvc@example.com', 1, GETDATE(), 1, GETDATE())

-- client-side
INSERT INTO [Configuration]
([Key], [Description], [Is_Client_Side], [Boolean_Value], [DateTime_Value], [Decimal_Value], [Int_Value], [Money_Value], [String_Value], [Created_By], [Created_Date], [Updated_By], [Updated_Date])
VALUES ('TOAST_DELAY_SECONDS', 'The amount of seconds for toast notifications to display', 1, NULL, NULL, NULL, 6, NULL, NULL, 1, GETDATE(), 1, GETDATE())

INSERT INTO [Configuration]
([Key], [Description], [Is_Client_Side], [Boolean_Value], [DateTime_Value], [Decimal_Value], [Int_Value], [Money_Value], [String_Value], [Created_By], [Created_Date], [Updated_By], [Updated_Date])
VALUES ('AUTO_LOGOUT_IS_ENABLED', 'Feature switch to detect if the user is idle and automatically log them out', 1, 1, NULL, NULL, NULL, NULL, NULL, 1, GETDATE(), 1, GETDATE())


--add permissions
INSERT INTO [Permission]
([Key], [Group_Name], [Name], [Description], [Created_By], [Created_Date], [Updated_By], [Updated_Date])
VALUES ('SESSIONS_VIEW', 'Admin', 'View sessions', 'View user''s sessions', 1, GETDATE(), 1, GETDATE())

INSERT INTO [Permission]
([Key], [Group_Name], [Name], [Description], [Created_By], [Created_Date], [Updated_By], [Updated_Date])
VALUES ('USERS_MANAGE', 'Admin', 'Manage users', 'Create, edit and view users', 1, GETDATE(), 1, GETDATE())

INSERT INTO [Permission]
([Key], [Group_Name], [Name], [Description], [Created_By], [Created_Date], [Updated_By], [Updated_Date])
VALUES ('ROLES_MANAGE', 'Admin', 'Manage roles', 'Create, edit and moderate roles', 1, GETDATE(), 1, GETDATE())

INSERT INTO [Permission]
([Key], [Group_Name], [Name], [Description], [Created_By], [Created_Date], [Updated_By], [Updated_Date])
VALUES ('CONFIGURATION_MANAGE', 'Admin', 'Manage configuration', 'Create and edit configuration items', 1, GETDATE(), 1, GETDATE())


--add admin role
INSERT INTO [Role]
([Name], [Description], [Is_Enabled], [Created_By], [Created_Date], [Updated_By], [Updated_Date])
VALUES ('Admin', 'Administrator account', 1, 1, GETDATE(), 1, GETDATE())

--add permissions to admin role
INSERT INTO [Role_Permission]
([Role_Id], [Permission_Id], Created_By, [Created_Date], [Updated_By], [Updated_Date])
SELECT 1, [Id] AS [Permission_Id], 1, GETDATE(), 1, GETDATE()
FROM [Permission]
