using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StandardEng.Data.CustomModel
{
    public class LoginModel
    {
        [Display(Name = "Username")]
        public string UserName { get; set; }

        [Display(Name = "Password")]
        public string Password { get; set; }

        public bool RememberMe { get; set; }

        public int? CompanyId { get; set; }

        public string Name { get; set; }
    }
}
