using System.Web.Http;
using ForexRiskTraining;

namespace ForexRiskApi.Controllers
{
    public class TrainingController : ApiController
    {
        [HttpGet]
        public void TrainRiskModel()
        {
            RiskModelTraining.TrainModel();
        }
    }
}
