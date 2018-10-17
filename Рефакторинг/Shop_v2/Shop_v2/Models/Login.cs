using System.ComponentModel.DataAnnotations; 

namespace Shop_v2.Models
{
    public class Login
    {
        [Display(Name = "Логин ")]
        [Required(ErrorMessage = "Введите логин", AllowEmptyStrings = false)]
        public string UserName { get; set; }

        [Display(Name = "Пароь ")]
        [Required(ErrorMessage = "Введите пароль", AllowEmptyStrings = false)]
        [DataType(System.ComponentModel.DataAnnotations.DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Запомнить ")]
        public bool RememberMe { get; set; }
    }
}