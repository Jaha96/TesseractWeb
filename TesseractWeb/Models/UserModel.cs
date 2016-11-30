using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TesseractWeb.Models
{
    public class UserModel
    {
        

        [StringLength(50,MinimumLength = 5,ErrorMessage ="Хэрэглэгчийн нэр багадаа 5-үсэг байх ёстой")]
        public string UserName{ get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]

        [Compare("Password", ErrorMessage = "Нууц үгүүд зөрж байна!")]
        public string ConfirmPassword { get; set; }
        
        [EmailAddress(ErrorMessage = "Мэйл хаяг буруу байна!")]
        public string Email { get; set; }
        public bool RememberMe { get; set; }
        public int Count { get; set; }
        public int UserId { get; set; }
    }
}