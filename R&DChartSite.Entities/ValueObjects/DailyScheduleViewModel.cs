using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDChartSite.Entities.ValueObjects
{
    public class DailyScheduleViewModel
    {
        public IEnumerable<RDChartSite.Entities.Projects> Projects { get; set; }
        public DailyActivity DailyActivity { get; set; }

        public HourlyActivity HourlyActivity { get; set; }
    }
}
