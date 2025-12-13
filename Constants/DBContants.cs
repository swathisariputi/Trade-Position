namespace Trade_Position.Constants
{
    /// <summary>
    /// Contains all the database constants 
    /// </summary>
    public class DBContants
    {
        public const string SP_add_trade_history = "sp_add_trade_history";
        public const string Account = "Account";
        public const string Asset = "Asset";
        public const string Price = "Price";
        public const string TradeType = "TradeType";
        public const string Quantity = "Quantity";
        public const string SP_add_update_position = "sp_add_or_update_position";
        public const string NetQuantity = "NetQuantity";
        public const string AveragePrice = "AveragePrice";
        public const string RealizedPnl = "RealizedPnl";
        public const string NotionalValue = "NotionalValue";
        public const string PositionStatus = "PositionStatus";
        public const string SP_get_all_trades = "sp_get_trades";
        public const string TradeId = "TradeId";
        public const string TradeTimeStamp = "TradeTimeStamp";
        public const string SP_get_all_positions = "sp_get_positions";
        public const string PositionId = "PositionId";
        public const string LastUpdated = "LastUpdated";
        public const string SP_get_postion_by_asset = "sp_get_position_by_asset";
    }
}
