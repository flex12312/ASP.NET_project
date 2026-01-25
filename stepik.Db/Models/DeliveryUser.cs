using System.ComponentModel.DataAnnotations;

namespace stepik_asp.Models
{
    public class DeliveryUser
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public DateTime Date { get; set; } = DateTime.UtcNow;
        public string? Comment { get; set; }

    }
}