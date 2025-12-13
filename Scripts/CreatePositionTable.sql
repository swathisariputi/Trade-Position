-- =============================================
-- Author: Naga Swathi Sariputi
-- Create date: 13/12/2025
-- Description: To store calculated Position info
-- =============================================

CREATE TABLE Position(
 PositionId BIGINT IDENTITY(1,1) PRIMARY KEY,
 Account NVARCHAR(50) NOT NULL,
 Asset NVARCHAR(50) NOT NULL,
 NetQuantity INT NOT NULL,
 AveragePrice DECIMAL(18,4) NOT NULL DEFAULT 0,
 RealizedPnl DECIMAL(18,4) NOT NULL DEFAULT 0,
 NotionalValue DECIMAL(18,4) NOT NULL DEFAULT 0,
 PositionStatus NVARCHAR(10) NOT NULL,
 LastUpdated DATETIME2 NOT NULL
		DEFAULT SYSDATETIME()
 );