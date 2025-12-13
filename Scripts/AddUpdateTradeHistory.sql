USE [Trade]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author: Naga Swathi Sariputi
-- Create date: 13/12/2025
-- Description: To add/update Trade info
-- =============================================
CREATE PROCEDURE [dbo].[sp_add_update_trade_history]
	(@TradeId [int],
	@Account [nvarchar](50), 
	@Asset [nvarchar](50),
	@Price [decimal](18,4),
	@TradeType [nvarchar](5),
	@Quantity [int]
	)
AS
BEGIN
	DECLARE @Result TABLE (TradeId INT)
	UPDATE [dbo].[TradeHistory] 
	SET [Account] = @Account, 
		[Asset] = @Asset, 
		[Price] = @Price, 
		[TradeType] = @TradeType, 
		[Quantity] = @Quantity, 
		[TradeTimeStamp] = GETDATE()
	OUTPUT Inserted.TradeId INTO @Result
	WHERE [TradeId]=@TradeId;
	IF @@ROWCOUNT=0
		INSERT INTO [dbo].[TradeHistory] ([Account], [Asset], [Price], [TradeType], [Quantity], [TradeTimeStamp]) 
		OUTPUT Inserted.TradeId INTO @Result
		VALUES(
		@Account,
		@Asset,
		@Price,
		@TradeType,
		@Quantity,
		GETDATE());
	SELECT TradeId FROM @Result;
END
