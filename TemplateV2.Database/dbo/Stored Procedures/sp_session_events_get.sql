CREATE PROCEDURE [dbo].[sp_session_events_get]
AS
     BEGIN
         SELECT [Id],
                [Key],
                [Description],
                [Created_By],
                [Created_Date],
                [Updated_By],
                [Updated_Date],
                Is_Deleted
         FROM [Session_Event](NOLOCK)
         WHERE Is_Deleted = 0;
     END;