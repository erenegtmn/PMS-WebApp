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
    public class DailyActivity
    {
        [Key]
        public int Id { get; set; }

        public int UserId { get; set; }
        public Users User { get; set; }

        [DisplayName("Tarih")]
        public DateTime Date { get; set; } 

        public List<HourlyActivity> HourlyActivity { get; set; }

    }
}
