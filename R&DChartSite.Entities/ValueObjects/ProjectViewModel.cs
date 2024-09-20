using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDChartSite.Entities.ValueObjects
{
    public class ProjectViewModel
    {
        [DisplayName("Proje Adı"), Required(ErrorMessage = "Lütfen proje adı girin.")]
        public string ProjectName { get; set; }

        [Required(ErrorMessage = "Lütfen proje kodunu girin."), DisplayName("Proje Kodu")]
        public string ProjectCode { get; set; }

        [DisplayName("Proje Durumu")]
        public bool IsProjectActive { get; set; }
    }
}
