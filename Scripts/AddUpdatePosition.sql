USE [Trade]
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author: Naga Swathi Sariputi
-- Create date: 13/12/2025
-- Description: To store calculated Position info
-- =============================================
CREATE PROCEDURE [dbo].[sp_add_or_update_position]
	(@Account [nvarchar](50), 
	@Asset [nvarchar](50),
	@NetQuantity [int],
	@AveragePrice [decimal](18,4),
	@RealizedPnl [decimal](18,4),
	@NotionalValue [decimal](18,4),
	@PositionStatus [nvarchar](10)
	)
AS
BEGIN
	UPDATE [dbo].[Position] 
	SET [NetQuantity] = @NetQuantity, 
		[AveragePrice] = @AveragePrice, 
		[RealizedPnl] = @RealizedPnl, 
		[NotionalValue] = @NotionalValue, 
		[PositionStatus] = @PositionStatus, 
		[LastUpdated] = GETDATE()
	WHERE [Account]=@Account AND [Asset]=@Asset;
	IF @@ROWCOUNT=0
		INSERT INTO [dbo].[Position] 
		([Account], 
		[Asset],
		[NetQuantity], 
		[AveragePrice], 
		[RealizedPnl], 
		[NotionalValue], 
		[PositionStatus], 
		[LastUpdated]) 
		VALUES(
			@Account,
			@Asset,
			@NetQuantity,
			@AveragePrice,
			@RealizedPnl,
			@NotionalValue,
			@PositionStatus,
			GETDATE());
END
