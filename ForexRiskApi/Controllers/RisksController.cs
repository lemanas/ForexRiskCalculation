using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using ForexRisk.DataAccess;
using ForexRiskApi.Params;
using ForexRiskApi.ReturnObjects;
using ForexRiskTraining.TemporaryObjects;

namespace ForexRiskApi.Controllers
{
    public class RisksController : ApiController
    {
        [HttpPost]
        public List<Prediction> GetPredictions([FromBody]Period period)
        {
            var model = Globals.Model;
            var inputs = GetInputData(period.Start, period.End);
            int[] answers = model.Decide(inputs);
            int currentYear = period.Start;
            return answers.Select(t => new Prediction
                {
                    Year = currentYear,
                    PredictionNumber = t
                })
                .ToList();
        }

        [HttpPost]
        public List<Probability> GetProbabilities([FromBody] Period period)
        {
            List<Probability> probabilityList = new List<Probability>();
            var model = Globals.Model;
            var inputs = GetInputData(period.Start, period.End);
            double[][] probabilities = model.Probabilities(inputs);
            int currentYear = period.Start;
            foreach (var t in probabilities)
            {
                Probability probability = new Probability {Year = currentYear};
                for (int j = 0; j < 5; j++)
                {
                    probability.ScenarioProbabilities[j] = t[j];
                }
                probabilityList.Add(probability);
                currentYear++;
            }
            return probabilityList;
        }

        private static double[][] GetInputData(int startYear, int endYear)
        {
            using (ForexDataEntities context = new ForexDataEntities())
            {
                var data = context.AnalyticRecords.Where(r => r.Year >= startYear && r.Year <= endYear).Select(r => new InputRecord
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
}
