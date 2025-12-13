USE [Trade]
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author: Naga Swathi Sariputi
-- Create date: 13/12/2025
-- Description: To get all the positions
-- =============================================
CREATE PROCEDURE [dbo].[sp_get_position_by_asset]
(@Asset nvarchar(50))
AS
BEGIN
	SELECT * FROM [dbo].[Position] WHERE [Asset]=@Asset;
END
