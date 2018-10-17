using System.ComponentModel.DataAnnotations; 

namespace Shop_v2.Models
{
    public class User
    { 
        public int Id { get; set; }

        [Required]
        [Display(Name = "Логин")] 
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Длина логина должна быть от 3 до 50 символов")]
        public string UserName { get; set; } 

        [Display(Name = "Имя")]
        [Required(ErrorMessage = "Введите ваше имя")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Длина имени должна быть от 3 до 50 символов")]
        public string FirstName { get; set; }

        [Display(Name = "Фамилия")]
        [Required(ErrorMessage = "Введите вашу фамилию")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Длина фамилии должна быть от 3 до 50 символов")]
        public string LastName { get; set; } 

        [Required(ErrorMessage = "Введите ваш номер телефона")]
        [Display(Name = "Номер телефона")]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Не корректный номер телефона")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Введите город")]
        [Display(Name = "Город")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Длина города должна быть от 3 до 50 символов")]
        public string City { get; set; }

        [Required(ErrorMessage = "Введите улицу")]
        [Display(Name = "Улица")]
        [StringLength(60, MinimumLength = 3, ErrorMessage = "Длина улицы должна быть от 3 до 60 символов")]
        public string Street { get; set; }

        [Required(ErrorMessage = "Введите номер дома")]
        [Display(Name = "Номер дома")]
        public int House { get; set; }

        [Display(Name = "Корпус")]
        public string Housing { get; set; }
         
        [Display(Name = "Квартира")]
        public string Flat { get; set; }

        [Required(ErrorMessage = "Введите пароль")]
        [Display(Name = "Пароль")]
        [StringLength(10000, MinimumLength = 4, ErrorMessage = "Минимальная длина пароля - 4 символа")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Повторите пароль")]
        [Display(Name = "Повторите пароль")]
        [DataType(DataType.Password)]
        [System.Web.Mvc.Compare("Password", ErrorMessage = "Пароли не совпадают")]
        public string RePassword { get; set; }

        [Display(Name = "Email")]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "Некорректный Email")]
        public string Email { get; set; } 
    }
}