using System.ComponentModel.DataAnnotations;

namespace stepik_asp.Models
{
    public enum OrderStatus
    {
        [Display(Name = "Создан")]
        Generated,

        [Display(Name = "Обработан")]
        Processed,

        [Display(Name = "В пути")]
        InTransit,

        [Display(Name = "Доставлен")]
        Delivered,

        [Display(Name = "Отменен")]
        Cancelled
    }
}
