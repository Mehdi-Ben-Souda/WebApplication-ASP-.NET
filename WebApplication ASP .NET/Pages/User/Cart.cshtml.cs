using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApplication_ASP_.NET.Models;

namespace WebApplication_ASP_.NET.Pages.User
{
    // Classe DTO (Data Transfer Object) pour repr�senter une ligne du panier
    public class CartLineDTO
    {
        public int ProductID { get; set; }
        public int Quantity { get; set; }
        public string ProductName { get; set; }
        public double Price { get; set; }
        public string image { get; set; }

    }
    // Mod�le de page pour g�rer le panier d'achats
    [Authorize]
    public class CartModel : PageModel
    {
        // Injection des d�pendances pour acc�der au contexte HTTP et � la base de donn�es
        private readonly IHttpContextAccessor _httpContextAccessor;
        private const string CartCookieName = "Cart";
        private readonly WebApplication_ASP_.NET.Data.WebApplication_ASP_NETContext _context;


        public CartModel(IHttpContextAccessor httpContextAccessor, WebApplication_ASP_.NET.Data.WebApplication_ASP_NETContext context)
        {
            _httpContextAccessor = httpContextAccessor;
            _context = context;
        }

        public List<CartLine> CartItems { get; set; } = new List<CartLine>();
        public List<CartLineDTO> cartLineDTOs { get; set; } = new List<CartLineDTO>();
        
        // M�thode appel�e lors du chargement de la page
        public void OnGet()
        {
            // R�cup�ration du panier depuis les cookies
            var cartJson = _httpContextAccessor.HttpContext.Request.Cookies[CartCookieName];
            if (!string.IsNullOrEmpty(cartJson))
            {
                // D�s�rialisation du JSON en liste d'articles
                CartItems = System.Text.Json.JsonSerializer.Deserialize<List<CartLine>>(cartJson);
                foreach(CartLine c in CartItems)
                {
                    var product = _context.Product.Find(c.ProductID);
                    // Cr�ation d'un DTO avec les informations compl�tes
                    if (product != null)
                    {
                        cartLineDTOs.Add(new CartLineDTO
                        {
                            ProductID = c.ProductID,
                            Quantity = c.Quantity,
                            ProductName = product.Name,
                            Price = product.Price,
                            image = product.ImagePath
                        });
                    }
                }
            }
        }
        // M�thode pour supprimer un article du panier
        public IActionResult OnPostRemoveFromCart(int productId)
        {
            // R�cup�ration du panier depuis les cookies
            var cartJson = _httpContextAccessor.HttpContext.Request.Cookies[CartCookieName];
            if (!string.IsNullOrEmpty(cartJson))
            {
                var cartItems = System.Text.Json.JsonSerializer.Deserialize<List<CartLine>>(cartJson);
                cartItems.RemoveAll(i => i.ProductID == productId);

                var updatedCartJson = System.Text.Json.JsonSerializer.Serialize(cartItems);
                _httpContextAccessor.HttpContext.Response.Cookies.Append(CartCookieName, updatedCartJson);
            }
            // Redirection vers la m�me page
            return RedirectToPage();
        }

    }
}
