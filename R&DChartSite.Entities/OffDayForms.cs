using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDChartSite.Entities;
using System.ComponentModel;

namespace RDChartSite.Entities
{
    public class OffDayForms
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public Guid FormGuid { get; set; }

        [DisplayName("Başlangıç Tarihi")]
        public DateTime StartDate { get; set; }

        [DisplayName("Dönüş Tarihi")]
        public DateTime ReturnDate { get; set; }

        [DisplayName("İzin Türü")]
        public string PType { get; set; }

        [DisplayName("Başlangıç Saati")]
        public TimeSpan StartTime { get; set; }

        [DisplayName("Bitiş Saati")]
        public TimeSpan ReturnTime { get; set; }

        [Display(Name = "Proje Kodu")]
        public string ProjectCode { get; set; }

        [StringLength(150),DisplayName("Açıklama")]
        public string Description { get; set; }

        [DisplayName("Sebep")]
        public string Reason { get; set; }

        [DisplayName("Gönderim Tarihi")]
        public DateTime PostTime { get; set; }

        public int UserId { get; set; } 
        public Users User { get; set; }

        [DisplayName("Onay Durumu")]
        public bool IsApproved { get; set; }

    }
}
