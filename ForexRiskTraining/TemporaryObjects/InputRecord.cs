namespace ForexRiskTraining.TemporaryObjects
{
    public class InputRecord
    {
        public double CpiDifference { get; set; }
        public double CpiTendency { get; set; }
        public double InterestRateDifference { get; set; }
        public double InterestRateTendency { get; set; }
        public double TradeBalanceByUk { get; set; }
        public double TradeBalanceByUs { get; set; }
        public double DebtGrowthUk { get; set; }
        public double DebtGrowthUs { get; set; }
        public double ForexTendency { get; set; }
    }
}
