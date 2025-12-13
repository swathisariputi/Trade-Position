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
CREATE PROCEDURE [dbo].[sp_get_trade_by_tradeId]
(@TradeId BigInt)
AS
BEGIN
	SELECT * FROM [dbo].[TradeHistory] WHERE [TradeId]=@TradeId;
END
