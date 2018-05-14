using System.Web.Http;
using System.Web.Http.Description;
using ForexRiskApi.Params;
using ForexRiskTraining;

namespace ForexRiskApi.Controllers
{
    public class TrainingController : ApiController
    {
        [HttpPost]
        [ResponseType(typeof(void))]
        public IHttpActionResult TrainRiskModel([FromBody]Period period)
        {
            Globals.Model = RiskModelTraining.TrainModel(period.Start, period.End);
            return Ok();
        }
    }
}
