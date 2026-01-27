using stepik.Db.Models;

namespace stepik.Db.Models
{
    public class Comparison
    {
        public Guid Id { get; set; }
        public string? UserId { get; set; }
        public string? GuestId { get; set; }
        public List<Product> Items { get; set; }
    }
}