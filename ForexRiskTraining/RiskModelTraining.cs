﻿using System.Linq;
using Accord.Statistics.Models.Regression;
using Accord.Statistics.Models.Regression.Fitting;
using ForexRisk.DataAccess;
using ForexRiskTraining.TemporaryObjects;

namespace ForexRiskTraining
{
    public class RiskModelTraining
    {
        public static MultinomialLogisticRegression TrainModel(int startYear, int endYear)
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

                int[] outputs = context.AnalyticRecords.Select(p => p.Outcome).ToArray();
                
                LowerBoundNewtonRaphson lbnr = new LowerBoundNewtonRaphson
                {
                    MaxIterations = 50000,
                    Tolerance = 0.001
                };

                MultinomialLogisticRegression mlr = lbnr.Learn(inputs, outputs);
                return mlr;
            }
        }
    }
}
