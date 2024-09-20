using RDChartSite.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDChartSite.Entities
{
    public class HourlyActivity
    {
        [Key]
        public int Id { get; set; }

        
        public int DailyActivityId { get; set; } 
        public DailyActivity DailyActivity { get; set; }


        public TimeSpan Time { get; set; }

        [DisplayName("Açıklama")]
        public string Description { get; set; }

        [DisplayName("Proje Kodu")]
        public string ProjectCode { get; set; } 
    }
}
