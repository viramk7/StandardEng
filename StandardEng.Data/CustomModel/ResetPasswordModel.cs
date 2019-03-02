using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StandardEng.Data.CustomModel
{
    public class ResetPasswordModel
    {
        public int UserId { get; set; }

        [DataType(DataType.Password)]
        //[Display(ResourceType = typeof(CommonMessage), Name = "Password")]

        public string Password { get; set; }
        [DataType(DataType.Password)]
        //[Display(ResourceType = typeof(CommonMessage), Name = "ConfirmPassword")]
        //[Required(ErrorMessageResourceName = "ConfirmPasswordRequired", ErrorMessageResourceType = typeof(CommonMessage))]
        public string ConfirmPassword { get; set; }
    }
}
