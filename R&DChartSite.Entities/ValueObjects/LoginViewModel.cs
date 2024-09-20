using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RDChartSite.Entities.ValueObjects
{
    public class LoginViewModel
    {
        [DisplayName("E-mail"), Required(ErrorMessage = "Lütfen mail adresini girin."), DataType(DataType.EmailAddress), /*RegularExpression(@"^[a-zA-Z0-9._%+-]+@ortem\.com\.tr$", ErrorMessage = "Email adresi Ortem Elektronik şirketine ait olmalıdır.")*/]
        public string Email { get; set; }

        [Required(ErrorMessage = "Lütfen şifrenizi girin."), DataType(DataType.Password), DisplayName("Şifre")]
        public string password { get; set; }
    }
}