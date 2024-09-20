using RDChartSite.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDChartSite.Entities.ValueObjects
{
    public class MonthlyDataModel
    {
     public int currentMonth { get; set; }
        public MonthlyDataModel()
        {
            currentMonth = DateTime.Now.Month;
        }
    }
}

