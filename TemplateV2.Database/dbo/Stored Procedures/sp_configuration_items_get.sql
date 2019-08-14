CREATE PROCEDURE [dbo].[sp_configuration_items_get]
AS
     BEGIN
         SELECT [Id],
                [Key],
                [Description],
                [Boolean_Value],
                [DateTime_Value],
				[Date_Value],
				[Time_Value],
                [Decimal_Value],
                [Int_Value],
                [Money_Value],
                [String_Value],
                [Created_By],
                [Created_Date],
                [Updated_By],
                [Updated_Date],
                Is_Deleted
         FROM [Configuration](NOLOCK)
         WHERE Is_Deleted = 0;
     END;