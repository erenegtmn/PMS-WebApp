using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDChartSite.Entities.ValueObjects
{
    public class HourlyActivityViewModel
    {
        [Required]
        public int UserId { get; set; }

        [Required, DataType(DataType.Time)]
        public TimeSpan Time { get; set; }

        [Required(ErrorMessage = "Lütfen Açıklamayı girin."),DisplayName("Açıklama")]
        public string Description { get; set; }

        [Required,DisplayName("Proje Kodu")]
        public string ProjectCode { get; set; }
    }
}
