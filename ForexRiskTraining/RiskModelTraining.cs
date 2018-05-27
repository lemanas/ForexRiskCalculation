using System.Data.Entity;
using System.Linq;
using Accord.Statistics.Models.Regression;
using Accord.Statistics.Models.Regression.Fitting;
using ForexRisk.DataAccess;
using ForexRiskTraining.TemporaryObjects;

namespace ForexRiskTraining
{
    public class RiskModelTraining
    {
        public static MultinomialLogisticRegression TrainModel<T>(DbSet<T> analyticTable, int startYear, int endYear) where T : class, IAnalyticRecord
        {
            var data = analyticTable.Where(r => r.Year >= startYear && r.Year <= endYear).Select(r => new InputRecord
            {
                CpiDifference = r.CpiDifference,
                CpiTendency = r.CpiTendency,
                InterestRateDifference = r.InterestRateDifference,
                InterestRateTendency = r.InterestRateTendency,
                TradeBalanceByUk = r.TradeBalanceByUk,
                TradeBalanceByUs = r.TradeBalanceByUs,
                DebtGrowthUk = r.DebtGrowthUk,
                DebtGrowthUs = r.DebtGrowthUs,
                ForexTendency = r.ForexTendency
            }).ToList();

            double[][] inputs = new double[data.Count][];

            for (int i = 0; i < data.Count; i++)
            {
                inputs[i] = new[]
                {
                        data[i].CpiDifference,
                        data[i].CpiTendency,
                        data[i].InterestRateDifference,
                        data[i].InterestRateTendency,
                        data[i].TradeBalanceByUk,
                        data[i].TradeBalanceByUs,
                        data[i].DebtGrowthUk,
                        data[i].DebtGrowthUs,
                        data[i].ForexTendency
                    };
            }

            int[] outputs = analyticTable.Select(p => p.Outcome).ToArray();

            LowerBoundNewtonRaphson lbnr = new LowerBoundNewtonRaphson
            {
                MaxIterations = 5000000,
                Tolerance = 0.000000001
            };

            MultinomialLogisticRegression mlr = lbnr.Learn(inputs, outputs);
            return mlr;
        }

        public static MultinomialLogisticRegression TrainDailyModel<T>(DbSet<T> analyticTable, int startYear, int endYear) where T : class, IAnalyticDailyRecord
        {
            var data = analyticTable.Where(r => r.Date.Year >= startYear && r.Date.Year <= endYear).Select(r => new InputRecord
            {
                CpiDifference = r.CpiDifference,
                CpiTendency = r.CpiTendency,
                InterestRateDifference = r.InterestRateDifference,
                InterestRateTendency = r.InterestRateTendency,
                TradeBalanceByUk = r.TradeBalanceByUk,
                TradeBalanceByUs = r.TradeBalanceByUs,
                DebtGrowthUk = r.DebtGrowthUk,
                DebtGrowthUs = r.DebtGrowthUs,
                ForexTendency = r.ForexTendency
            }).ToList();

            double[][] inputs = new double[data.Count][];

            for (int i = 0; i < data.Count; i++)
            {
                inputs[i] = new[]
                {
                    data[i].CpiDifference,
                    data[i].CpiTendency,
                    data[i].InterestRateDifference,
                    data[i].InterestRateTendency,
                    data[i].TradeBalanceByUk,
                    data[i].TradeBalanceByUs,
                    data[i].DebtGrowthUk,
                    data[i].DebtGrowthUs,
                    data[i].ForexTendency
                };
            }

            int[] outputs = analyticTable.Select(p => p.Outcome).ToArray();

            LowerBoundNewtonRaphson lbnr = new LowerBoundNewtonRaphson
            {
                MaxIterations = 5000000,
                Tolerance = 0.000000001
            };

            MultinomialLogisticRegression mlr = lbnr.Learn(inputs, outputs);
            return mlr;
        }
    }
}
