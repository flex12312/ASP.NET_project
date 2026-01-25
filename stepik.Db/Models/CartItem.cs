namespace stepik.Db.Models
{
    public class CartItem
    {
        public Guid Id { get; set; }
        public Product Product { get; set; }
        public int Quantity { get; set; }
        public Cart Cart { get; set; }
        public Guid? CartId { get; set; }
        public Order Order { get; set; }
        public Guid? OrderId { get; set; }
        public decimal? Cost => Product?.Cost * Quantity;
    }
}