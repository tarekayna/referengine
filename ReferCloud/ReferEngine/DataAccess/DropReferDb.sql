USE [referengine_db]
GO

DECLARE	@return_value Int

EXEC	@return_value = [dbo].[DropReferDb]

SELECT	@return_value as 'Return Value'

GO
