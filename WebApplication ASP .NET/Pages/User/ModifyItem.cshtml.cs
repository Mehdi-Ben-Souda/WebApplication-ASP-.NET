using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.CodeAnalysis;
using System.ComponentModel.DataAnnotations;
using WebApplication_ASP_.NET.Models;

namespace WebApplication_ASP_.NET.Pages.User
{
    public class ModifyItemModel : PageModel
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private const string CartCookieName = "Cart";
        private readonly WebApplication_ASP_.NET.Data.WebApplication_ASP_NETContext _context;

        public ModifyItemModel(IHttpContextAccessor httpContextAccessor, WebApplication_ASP_.NET.Data.WebApplication_ASP_NETContext context)
        {
            _httpContextAccessor=httpContextAccessor;
            _context=context;
        }

        [BindProperty]
        public CartLineDTO CartItem { get; set; }

        [BindProperty]
        [Range(1, 100, ErrorMessage = "Quantity must be between 1 and 100.")]
        public int NewQuantity { get; set; }

        public IActionResult OnGet(int id)
        {
            
            List<CartLine> CartItems;
            CartLineDTO cartItem=null;
            var cartJson = _httpContextAccessor.HttpContext.Request.Cookies[CartCookieName];
            if (!string.IsNullOrEmpty(cartJson))
            {
                CartItems = System.Text.Json.JsonSerializer.Deserialize<List<CartLine>>(cartJson);

                foreach (var item in CartItems)
                {
                    
                    if (item.ProductID == id)
                    {
                        var prod = _context.Product.Find(item.ProductID);
                        CartItem = new CartLineDTO {
                            ProductID = item.ProductID,
                            image = prod.ImagePath,
                            Quantity = item.Quantity,
                            Price = prod.Price,
                            ProductName= prod.Name
                        };
                        break;
                    }               
                }
            }
                if (CartItem == null)
            {
                return NotFound();
            }

            NewQuantity = CartItem.Quantity;
            return Page();
        }

        public IActionResult OnPost()
        {


            var cartJson = _httpContextAccessor.HttpContext.Request.Cookies[CartCookieName];
            if (!string.IsNullOrEmpty(cartJson))
            {
                var cartItems = System.Text.Json.JsonSerializer.Deserialize<List<CartLine>>(cartJson);
                foreach(var item in cartItems)
                    if(item.ProductID == CartItem.ProductID)
                    {
                        var prod = _context.Product.Find(item.ProductID);
                        if (prod.Quantity < NewQuantity)
                        {
                            ModelState.AddModelError(string.Empty, "Not enough stock. Please try again.");
                            return Page();
                        }
                        item.Quantity = NewQuantity;
                    }
                        
                var updatedCartJson = System.Text.Json.JsonSerializer.Serialize(cartItems);
                _httpContextAccessor.HttpContext.Response.Cookies.Append(CartCookieName, updatedCartJson);
            }

            /*if (!result)
            {
                ModelState.AddModelError(string.Empty, "Failed to update quantity. Please try again.");
                return Page();
            }*/

            return RedirectToPage("/User/Cart");
        }
    }
}
