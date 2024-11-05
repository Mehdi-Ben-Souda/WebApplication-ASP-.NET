using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WebApplication_ASP_.NET.Models;
using System.Text.Json;

namespace WebApplication_ASP_.NET.Pages.User
{
    public class ProductDetailsModel : PageModel
    {

        private readonly WebApplication_ASP_.NET.Data.WebApplication_ASP_NETContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private const string CartCookieName = "Cart";

        public ProductDetailsModel(WebApplication_ASP_.NET.Data.WebApplication_ASP_NETContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }
        public Product Product { get; set; } = default!;

        public  IActionResult OnGet(int? id)
        {
            if (id == null || _context.Product == null)
            {
                return NotFound();
            }

            //var product = await _context.Product.FirstOrDefaultAsync(m => m.Id == id);
            var product = _context.Product.Find(id);
            if (product == null)
            {
                return NotFound();
            }
            else
            {
                Product = product;
            }
            return Page();
        }
        public IActionResult OnPost(int id, int quantity)
        {

            var product = _context.Product.Find(id);
            if (product == null)
            {
                return NotFound();
            }

            var cartJson = _httpContextAccessor.HttpContext.Request.Cookies[CartCookieName];
            var cartItems = string.IsNullOrEmpty(cartJson)
                ? new List<CartLine>()
                : JsonSerializer.Deserialize<List<CartLine>>(cartJson);

            var existingItem = cartItems.FirstOrDefault(i => i.ProductID == id);
            if (existingItem != null)
            {
                existingItem.Quantity += quantity;
            }
            else
            {
                cartItems.Add(new CartLine
                {
                    ProductID = product.Id,
                    Quantity = quantity
                });
            }

            var updatedCartJson = JsonSerializer.Serialize(cartItems);
            _httpContextAccessor.HttpContext.Response.Cookies.Append(CartCookieName, updatedCartJson, new CookieOptions
            {
                Expires = DateTime.Now.AddDays(7),
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Lax
            });

            return RedirectToPage("/User/Cart");
        }
    }
}
