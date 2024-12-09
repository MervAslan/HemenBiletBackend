using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
namespace HemenBilet.Models
{
    public class User
    {
        public int UserId { get; set; }
        [Required(ErrorMessage ="Kullanıcı Adı alanı gereklidir")]
        public string UserName { get; set; }
        [Required(ErrorMessage ="Şifre alanı gereklidir")]
        public string Password { get; set; }
        [Required(ErrorMessage ="Email  alanı gereklidir")]

        public string Email { get; set; }
        public string? Phone { get; set; }
    }
    
}