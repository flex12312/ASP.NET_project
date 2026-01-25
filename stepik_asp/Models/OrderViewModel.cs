using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace stepik_asp.Models
{
    public class OrderViewModel
    {
        public Guid Id { get; set; }

        [ValidateNever]
        public string UserId { get; set; }

        [ValidateNever]
        public List<CartItemViewModel> Items { get; set; }

        [Required]
        public DeliveryUserViewModel DeliveryUser { get; set; }

        [Required]
        public OrderStatus Status { get; set; }

        [ValidateNever]
        public DateTime CreationDateTime { get; set; }  = DateTime.UtcNow;

        public decimal? TotalCost { get; set; }

        public int? ItemsQuantity {  get; set; }
    }

}