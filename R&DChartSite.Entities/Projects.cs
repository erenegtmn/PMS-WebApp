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
    public class Projects
    {
        
        [Key, Required]
        public int Id { get; set; }

        [Required(ErrorMessage = "Lütfen proje ismini girin."), StringLength(60)]
        [Display(Name = "Proje İsmi")]
        public string ProjectName { get; set; }

        [Required(ErrorMessage = "Lütfen proje kodunu girin."), StringLength(20)]
        [Display(Name = "Proje Kodu")]
        public string ProjectCode { get; set; }

        [Display(Name = "Proje Aktif mi?")]
        public bool IsProjectActive { get; set; }

        [StringLength(150), DisplayName("Proje Açıklaması")]
        public string ProjectDescription { get; set; }

        [DisplayName("Tahmini Maliyet")]
        public int EstimatedExpense { get; set; }

        [DisplayName("Başlangıç Tarihi")]
        public DateTime StartingDate { get; set; }

        [DisplayName("Hedeflenen Çalışma Saati")]
        public int? EstimatedWorkingHours { get; set; }

        [DisplayName("Tahmini Bitiş Tarihi")]
        public DateTime EndingDate { get; set; }



        public List<Users> Users { get; set; }
        public Projects()
        {
            Users = new List<Users>();
        }
       
    }
}
