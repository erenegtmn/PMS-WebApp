using RDChartSite.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDChartSite.Entities
{
    public class Users
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }

        [StringLength(25), DisplayName("İsim")]
        public string Name { get; set; }

        [StringLength(25), DisplayName("Soyisim")]
        public string Surname { get; set; }

        [Required, StringLength(70), DisplayName("E-mail")]
        public string Email { get; set; }

        [Required, StringLength(100), DisplayName("Parola")]
        public string Password { get; set; }

        [DisplayName("Hesap Aktifliği")]
        public bool IsActive { get; set; }

        [DisplayName("Admin")]
        public bool IsAdmin { get; set; }

        [DisplayName("Yönetici")]
        public bool IsSuperAdmin { get; set; }

        [Required]
        public Guid ActivateGuid { get; set; }

        public List<Projects> Projects { get; set; }

        public List<DailyActivity> DailyActivities { get; set; }

        [DisplayName("Toplam İzin Günü")]
        public int? TotalPermission { get; set; }

        [DisplayName("Kalan İzin Günü")]
        public int? RemainingPermission { get; set; }

        public List<OffDayForms> OffDayForms { get; set; }

        [ForeignKey("Team")]
        public int? TeamId { get; set; }
        [InverseProperty("Users")]
        public Teams Team { get; set; }

        public bool IsTeamLeader {  get; set; }

        [DisplayName("Maaş")]
        public int? Salary { get; set; }

        public Users()
        {
            Projects = new List<Projects>();
            DailyActivities = new List<DailyActivity>();
        }
    }
}
