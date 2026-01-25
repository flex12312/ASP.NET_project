namespace stepik_asp.Models
{
    public class CartViewModel
    {
        public Guid Id { get; set; }
        public string UserId { get; set; }
        public List<CartItemViewModel> Items { get; set; } = new List<CartItemViewModel>();
        public decimal? TotalCost => Items?.Sum(item => item.Cost);
        public int Quantity => Items.Sum(item => item.Quantity);

    }
}