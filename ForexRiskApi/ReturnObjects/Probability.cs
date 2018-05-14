namespace ForexRiskApi.ReturnObjects
{
    public class Probability
    {
        public int Year { get; set; }
        public double[] ScenarioProbabilities { get; set; } = new double[5];
    }
}