using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace stepik_asp.Areas.Admin.Models
{
    public class ChangePassword
    {
        [Display(Name = "Логин", Prompt = "Логин пользователя")]
        [Required(ErrorMessage = "Не указан логин")]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "Логин должен быть от {2} до {1} символов")]
        public string Login { get; set; } = string.Empty;  


        [Display(Name = "Пароль", Prompt = "Ваш пароль")]
        [Required(ErrorMessage = "Не указан пароль")]
        [DataType(DataType.Password)]
        [StringLength(50, MinimumLength = 6, ErrorMessage = "Пароль должен быть от {2} до {1} символов")]
        [AllowNull]
        public string Password { get; set; }


        [Display(Name = "Подтвердите пароль", Prompt = "Подтвердите пароль")]
        [Required(ErrorMessage = "Не указан повторный пароль")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        [AllowNull]
        public string ConfirmPassword { get; set; }
    }
}