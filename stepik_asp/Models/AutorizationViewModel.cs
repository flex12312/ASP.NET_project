using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace stepik_asp.Models
{
    public class AutorizationViewModel
    {
        [Required(ErrorMessage = "Не указано имя")]
        [EmailAddress(ErrorMessage ="Введите валидную email") ]
        [DataType(DataType.EmailAddress)]
        [StringLength(30, MinimumLength = 5, ErrorMessage = "Логин должен быть от 5 до 30 символов")]
        [Display(Name = "Логин",Prompt ="qwerty@mail.ru")]
        public string Login { get; set; }

        [Required(ErrorMessage = "Не указан пароль")]
        [DataType(DataType.Password)]
        [StringLength(50, MinimumLength = 6, ErrorMessage = "Пароль должен быть от 6 до 50 символов")]
        [Display(Name ="Пароль")]
        public string Password { get; set; }

        [Display(Name = "Запомнить меня")]
        [Required]
        public bool IsRememberMe { get; set; }

        public string ReturnUrl { get; set; }
    }
}