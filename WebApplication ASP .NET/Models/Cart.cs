namespace WebApplication_ASP_.NET.Models
{
    public class Cart
    {
        public int Id { get; set; }
        public float Total { get; set; }
        public List<CartLine> CartLines { get; set; } = new List<CartLine>();   
    }
}
