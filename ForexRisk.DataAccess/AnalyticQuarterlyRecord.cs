//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ForexRisk.DataAccess
{
    using System;
    using System.Collections.Generic;
    
    public partial class AnalyticQuarterlyRecord : IAnalyticRecord
    {
        public int Id { get; set; }
        public int Year { get; set; }
        public int Quarter { get; set; }
        public double CpiDifference { get; set; }
        public double CpiTendency { get; set; }
        public double InterestRateDifference { get; set; }
        public double InterestRateTendency { get; set; }
        public double TradeBalanceByUk { get; set; }
        public double TradeBalanceByUs { get; set; }
        public double DebtGrowthUk { get; set; }
        public double DebtGrowthUs { get; set; }
        public double ForexTendency { get; set; }
        public int Outcome { get; set; }
    }
}
