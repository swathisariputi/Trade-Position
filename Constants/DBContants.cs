namespace Trade_Position.Constants
{
    /// <summary>
    /// Contains all the database constants 
    /// </summary>
    public class DBContants
    {
        public const string action_add = "add";
        public const string action_update = "update";
        public const string SP_add_or_update_trade = "sp_add_update_trade_history";
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
        public const string SP_get_postion_of_asset_in_account = "sp_get_position_of_asset_in_account";
        public const string SP_get_trade_by_tradeId = "sp_get_trade_by_tradeId";
        public const string SP_get_trades_by_account_asset = "sp_get_trades_by_account_asset";
    }
}
