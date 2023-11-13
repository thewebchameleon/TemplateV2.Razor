# DEPRECATED
This project has been deprecated in favour of moving to a SPA architecture, starting with TemplateV3.Angular

# Template.Razor V2 [![Build Status](https://dev.azure.com/adrianbrink/TemplateV2.Razor/_apis/build/status/thewebchameleon.TemplateV2.Razor?branchName=master)](https://dev.azure.com/adrianbrink/TemplateV2.Razor/_build/latest?definitionId=21&branchName=master)
Intended for building **small self-contained business applications**, this template strives to be fast, secure and easy to understand.
#### [Demo](https://templatev2-razor.thewebchameleon.co.za/) (broken link, will fix soon)
#### Test User
**U**: admin
**P**: 123456

## Features
 - User profile
 - Activity Log
 - Admin section
 - User Journeys (Sessions)


## Architecture
 - Built with [Razor Pages](https://docs.microsoft.com/en-us/aspnet/core/razor-pages)
 - [N-tier application](https://docs.microsoft.com/en-us/visualstudio/data-tools/n-tier-data-applications-overview) with a focus on seperation of concerns
 - Uses MVC 6 with the latest version of Visual Studio 2019 and [ASP.NET 5](https://asp.net)
 - UI validation is shared with backend validation (client-side can only perform basic rules)
 - C# 8.0 Nullable References are enabled (.NET Standard 2.1)

### Database
- It is recommended to use SQL Server so that you can utilise the database project included
- Database project targets Microsoft SQL Server 2019 and uses the micro ORM [Dapper](https://github.com/StackExchange/Dapper)
- This project ONLY uses stored procedures to perform database querys
- Initial roll out script `V1.sql` is included and contains seed data
- Tables contain a soft-delete metadata column `Is_Deleted` to allow foreign key integrity
- Stored procedures are used to perform CRUD-like operations on the database
- Connecting to a MySQL database is supported

### Backend
- Business logic is contained within the `Services` layer
  - Services handle page request-response business logic
  - Managers handle independent logic (caching, authentication, session etc)

### UI
- [SB Admin 2](https://startbootstrap.com/themes/sb-admin-2/)
- [JQuery](https://jquery.com/)
- [JQuery DataTables](https://datatables.net/)
- [Bootstrap 4](https://getbootstrap.com/)
- [SASS](https://sass-lang.com/)
- Custom tag helpers
	- Authorization attribute (`AuthorizationTagHelper.cs`)
	- Back button (`BackButtonTagHelper.cs`)
	- MultiCheckbox (`MultiCheckboxTagHelper.cs`)
	- [MultiSelect](https://developer.snapappointments.com/bootstrap-select/) dropdown (`MultiSelectTagHelper.cs`)
	- Not authenticated attribute (`NotAuthenticatedTagHelper.cs`)

### Security
- Cookie authentication using authorization with permissions
	- Session / authentication cookies are **not** stored on the user's machine
- Passwords are hashed using [BCrypt](https://github.com/BcryptNet/bcrypt.net)
- Users are locked out after a configurable amount of invalid attempts
- Idle sessions are automatically logged out

### Sessions
- Custom session logging implementation which is recorded to the database
- Sessions can be viewed in detail on the `Sessions` admin page
- Session logs are recorded for each `GET` and `POST` request and includes form data
- Form data properties can be obfuscated to `******` if the data is sensitive (passwords)
- Session log events are high level actions that users may perform and may be useful for tracking / auditing user behavior

### Users, Roles, Permissions
- Users can register, login, update their profile and password.
- Users can perform forgot password requests and reset their password via an email containing an activation link
- Roles are a grouping of permissions assigned to users
- Permissions are access rights assigned to roles allowing access to otherwise restricted areas of the application

### Configuration
- Configuration items are used to control various aspects of the application ranging from features to core settings
- Stored as strong MSSQL types (`boolean`, `datetime`, `date`, `time`, `decimal` (18 before, 2 after), `int`, `money`, `string`)

### Admin
- Users can be created, updated, assigned roles, enabled and disabled
- Roles can be created, updated, assigned permissions, enabled and disabled
- Permissions can be created and updated
- Configuration items can be created and updated
- Session log events can be created and updated
