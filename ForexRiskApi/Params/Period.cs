using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ForexRiskApi.Params
{
    public class Period
    {
        public bool IsYearly { get; set; }
        public int Start { get; set; }
        public int End { get; set; }
    }
}