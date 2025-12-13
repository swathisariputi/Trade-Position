USE [Trade]
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author: Naga Swathi Sariputi
-- Create date: 13/12/2025
-- Description: To add Trade info
-- =============================================
CREATE PROCEDURE [dbo].[sp_add_trade_history]
	(@Account [nvarchar](50), 
	@Asset [nvarchar](50),
	@Price [decimal](18,4),
	@TradeType [nvarchar](5),
	@Quantity [int]
	)
AS
BEGIN
	INSERT INTO [dbo].[TradeHistory] ([Account], [Asset], [Price], [TradeType], [Quantity], [TradeTimeStamp]) 
	OUTPUT Inserted.TradeId
	VALUES(
	@Account,
	@Asset,
	@Price,
	@TradeType,
	@Quantity,
	GETDATE());

END
