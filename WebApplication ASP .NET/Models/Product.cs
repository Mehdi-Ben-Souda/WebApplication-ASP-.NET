namespace WebApplication_ASP_.NET.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Category { get; set; }
        public string? ImagePath { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
    }
}
