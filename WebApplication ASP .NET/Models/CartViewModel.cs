namespace WebApplication_ASP_.NET.Models
{
    public class CartViewModel
    {
        public int Id { get; set; }
        public float Total { get; set; }
        public List<CartLineViewModel> CartLines { get; set; } = new List<CartLineViewModel>();
    }
    public class CartLineViewModel
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public float Price { get; set; }
    }
}
