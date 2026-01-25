using System.ComponentModel.DataAnnotations;

namespace stepik_asp.Areas.AdminPanel.Models
{
    public class Role
    {
        public Guid Id { get; set; }


        [Required(ErrorMessage = "Не указано название роли")]
        [Display(Name = "Название роли",Prompt = "Название роли")]
        [DataType(DataType.Text)]
        [StringLength(50,MinimumLength = 2, ErrorMessage = "Название роли должно быть от {2} до {1} символов")]
        public required string Name { get; set; }

        public Role() { }
        public Role(string name)
        {
            Name = name;
        }
    }
}