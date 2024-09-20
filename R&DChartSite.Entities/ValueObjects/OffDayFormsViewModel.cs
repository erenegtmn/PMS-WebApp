using RDChartSite.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace RDChartSite.Entities.ValueObjects
{
    public class OffDayFormsViewModel
    {
        [DisplayName("Gidiş Tarihi")]
        public DateTime StartDate { get; set; }

        [DisplayName("Bitiş Tarihi")]
        public DateTime ReturnDate { get; set; }

        [Required(ErrorMessage = "Lütfen İzin Ücretlendirmesini Seçin."),DisplayName("İzin Ücretlendirmesi")]
        public string PType { get; set; }

        [DisplayName("Gidiş Saati")]
        public TimeSpan StartTime { get; set; }

        [DisplayName("Geliş Tarihi")]
        public TimeSpan ReturnTime { get; set; }

        [DisplayName("Proje Kodu")]
        public string ProjectCode { get; set; }

        [StringLength(150),DisplayName("İzin Açıklaması")]
        public string Description { get; set; }

        [DisplayName("İzin Sebebi")]
        public string Reason { get; set; }

        [DisplayName("Tarih")]
        public DateTime PostTime { get; set; }

        public int UserId { get; set; }

        [DisplayName("Kullanıcı Adı")]
        public string Name { get; set; }

        [DisplayName("Kullanıcı Soyadı")]
        public string Surname { get; set; }

        [DisplayName("Kullanılacak izin günü sayısı")]
        public int UsingPermission { get; set; }

        [DisplayName("Kullanıcı Ekibi")]
        public string UserTeam { get; set; }


        public int MailTo { get; set; }
    }
}
