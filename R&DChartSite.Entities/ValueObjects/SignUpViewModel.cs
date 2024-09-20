using RDChartSite.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RDChartSite.Entities.ValueObjects
{
    public class SignUpViewModel
    {
        [Required(ErrorMessage = "Lütfen adınızı girin."), DataType(DataType.Text),DisplayName("İsim")]
        public string firstName { get; set; }

        [Required(ErrorMessage = "Lütfen soyadınızı girin."), DataType(DataType.Text), DisplayName("Soyisim")]
        public string lastName { get; set; }

        [DisplayName("E-mail"),Required(ErrorMessage = "Lütfen kullanmak istediğiniz mail adresini girin."), DataType(DataType.EmailAddress),/*RegularExpression(@"^[a-zA-Z0-9._%+-]+@ortem\.com\.tr$", ErrorMessage = "Email adresi Ortem Elektronik şirketine ait olmalıdır.")*/] 
        public string Email { get; set; }

        [Required(ErrorMessage = "Lütfen şifrenizi girin."), DataType(DataType.Password), DisplayName("Şifre")] 
        public string Password { get; set; }

        [Required(ErrorMessage ="Lütfen şifrenizi tekrar girin."), DataType(DataType.Password), Compare("Password",ErrorMessage ="Şifreler Aynı Değil")] 
        public string PasswordCheck { get; set; }

        [Required(ErrorMessage = "Lütfen çalıştığınız ekibi seçin."), DisplayName("Ekip Seçimi")]
        public int? SelectedTeamId { get; set; }
        public List<Teams> AvailableTeams { get; set; }

        public SignUpViewModel()
        {
            AvailableTeams = new List<Teams>();
        }
    }
}