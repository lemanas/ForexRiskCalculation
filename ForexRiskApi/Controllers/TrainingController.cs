using System.Web.Http;
using System.Web.Http.Description;
using ForexRisk.DataAccess;
using ForexRiskApi.Params;
using ForexRiskTraining;

namespace ForexRiskApi.Controllers
{
    public class TrainingController : ApiController
    {
        private static readonly ForexDataEntities Context = new ForexDataEntities();

        [HttpPost]
        [ResponseType(typeof(void))]
        public IHttpActionResult TrainRiskModel([FromBody]Period period)
        {
            if (period.PeriodRange == "Yearly")
            {
                Globals.Model = RiskModelTraining.TrainModel(Context.AnalyticRecords, period.Start, period.End);
            }
            else if (period.PeriodRange == "Quarterly")
            {
                Globals.Model =
                    RiskModelTraining.TrainModel(Context.AnalyticQuarterlyRecords, period.Start, period.End);
            }
            else
            {
                Globals.Model = RiskModelTraining.TrainDailyModel(Context.AnalyticDailyRecords, period.Start, period.End);
            }
            return Ok();
        }
    }
}
