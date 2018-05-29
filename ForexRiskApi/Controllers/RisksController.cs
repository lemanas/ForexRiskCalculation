using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Http;
using Accord.Statistics.Models.Regression;
using ForexRisk.DataAccess;
using ForexRiskApi.Params;
using ForexRiskApi.ReturnObjects;
using ForexRiskTraining;
using ForexRiskTraining.TemporaryObjects;

namespace ForexRiskApi.Controllers
{
    public class RisksController : ApiController
    {
        private static readonly ForexDataEntities Context = new ForexDataEntities();

        [HttpPost]
        public List<Prediction> GetPredictions([FromBody]Period period)
        {
            List<Prediction> predictions = new List<Prediction>();
            MultinomialLogisticRegression model;
            double[][] inputs;

            if (period.PeriodRange == "Yearly")
            {
                model = Globals.Model ?? RiskModelTraining.TrainModel(Context.AnalyticRecords, 2000, 2017);
                inputs = GetInputData(Context.AnalyticRecords, period.Start, period.End);
            }
            else
            {
                model = Globals.Model ?? RiskModelTraining.TrainModel(Context.AnalyticQuarterlyRecords, 2000, 2017);
                inputs = GetInputData(Context.AnalyticQuarterlyRecords, period.Start, period.End);
            }
            
            int[] answers = model.Decide(inputs);
            int currentYear = period.Start;
            int counter = 0;
            for (int i = 0; i < answers.Length; i++)
            {
                if (period.PeriodRange == "Yearly")
                {
                    Prediction prediction = new Prediction
                    {
                        Year = currentYear,
                        PredictionNumber = answers[i]
                    };
                    predictions.Add(prediction);
                    currentYear++;
                }
                else if (period.PeriodRange == "Quarterly")
                {
                    counter++;
                    Prediction prediction = new Prediction
                    {
                        Year = currentYear,
                        PredictionNumber = answers[i]
                    };
                    predictions.Add(prediction);
                    if (counter > 3)
                    {
                        currentYear++;
                        counter = 0;
                    }
                }
            }
            return predictions;
        }

        [HttpPost]
        public List<Probability> GetProbabilities([FromBody] Period period)
        {
            List<Probability> probabilityList = new List<Probability>();
            MultinomialLogisticRegression model;
            double[][] inputs;
            if (period.PeriodRange == "Yearly")
            {
                model = Globals.Model ?? RiskModelTraining.TrainModel(Context.AnalyticRecords, 2000, 2017);
                inputs = GetInputData(Context.AnalyticRecords, period.Start, period.End);
            }
            else
            {
                model = Globals.Model ?? RiskModelTraining.TrainModel(Context.AnalyticQuarterlyRecords, 2000, 2017);
                inputs = GetInputData(Context.AnalyticQuarterlyRecords, period.Start, period.End);
            }
            double[][] probabilities = model.Probabilities(inputs);
            int currentYear = period.Start;
            int counter = 0;
            foreach (var t in probabilities)
            {
                if (period.PeriodRange == "Yearly")
                {
                    Probability probability = new Probability {Year = currentYear};
                    for (int j = 0; j < 5; j++)
                    {
                        probability.ScenarioProbabilities[j] = t[j];
                    }
                    probabilityList.Add(probability);
                    currentYear++;
                }
                else
                {
                    counter++;
                    Probability probability = new Probability { Year = currentYear };
                    for (int j = 0; j < 5; j++)
                    {
                        probability.ScenarioProbabilities[j] = t[j];
                    }
                    probabilityList.Add(probability);
                    if (counter > 3)
                    {
                        currentYear++;
                        counter = 0;
                    }
                }
            }
            return probabilityList;
        }

        private static double[][] GetInputData<T>(DbSet<T> analyticTable, int startYear, int endYear) where T : class, IAnalyticRecord
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

            return inputs;
        }

        private static double[][] GetDailyInputData<T>(DbSet<T> analyticTable, int startYear, int endYear) where T : class, IAnalyticDailyRecord
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

            return inputs;
        }
    }
}
