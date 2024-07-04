using System.ComponentModel.DataAnnotations;

namespace MVC_Template_NET6.Models
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Kullanıcı adı zorunlu")]
        [StringLength(10, ErrorMessage = "En fazla 10 karakter girimeli")]
        public string Fullname { get; set; }
        [Required(ErrorMessage = "Kullanıcı adı zorunlu")]
        [StringLength(10, ErrorMessage = "En fazla 10 karakter girimeli")]
        public string Username { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Şifre girmek zorunlu adı zorunlu")]
        [MinLength(4, ErrorMessage = "Şifreniz en az 4 karakter girilmeli")]
        [MaxLength(10, ErrorMessage = "Şifreniz en fazla 10 karakter girilmeli")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Şifrenizi tekrar giriniz")]
        [MinLength(4, ErrorMessage = "Şifreniz en az 4 karakter girilmeli")]
        [MaxLength(10, ErrorMessage = "Şifreniz en fazla 10 karakter girilmeli")]
        [Compare(nameof(Password))]
        public string RePassword { get; set; }

    }
}
