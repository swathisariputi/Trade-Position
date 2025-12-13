USE [Trade]
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author: Naga Swathi Sariputi
-- Create date: 13/12/2025
-- Description: To get all the trades
-- =============================================
CREATE PROCEDURE [dbo].[sp_get_trades]
AS
BEGIN
	SELECT * FROM [dbo].[TradeHistory];
END
