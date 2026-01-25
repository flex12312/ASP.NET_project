using stepik_asp.Models;
using System.ComponentModel.DataAnnotations;

namespace stepik.Db.Models
{
    public class Order
    {
        public Guid Id { get; set; }
        public string UserId { get; set; }
        public List<CartItem> Items { get; set; }
        public DeliveryUser DeliveryUser { get; set; }
        public DateTime CreationDateTime { get; set; } = DateTime.UtcNow;
        public decimal? TotalCost => Items?.Sum(item => item.Product?.Cost * item.Quantity);
        public int? ItemsQuantity => Items?.Sum(item => item.Quantity);

        public OrderStatus Status { get; set; }
    }
}