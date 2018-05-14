namespace ForexRisk.DataAccess
{
    public interface IAnalyticRecord
    {
        int Year { get; set; }
        double CpiDifference { get; set; }
        double CpiTendency { get; set; }
        double InterestRateDifference { get; set; }
        double InterestRateTendency { get; set; }
        double TradeBalanceByUk { get; set; }
        double TradeBalanceByUs { get; set; }
        double DebtGrowthUk { get; set; }
        double DebtGrowthUs { get; set; }
        double ForexTendency { get; set; }
        int Outcome { get; set; }
    }
}
