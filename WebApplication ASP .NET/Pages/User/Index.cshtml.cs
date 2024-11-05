using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication_ASP_.NET.Models;
using System.Text.Json;

namespace WebApplication_ASP_.NET.Pages.User
{

    // Mod�le de la page d'index qui affiche la liste des produits
    public class IndexModel : PageModel
    {
        // Injection de d�pendance du contexte de base de donn�es
        private readonly WebApplication_ASP_.NET.Data.WebApplication_ASP_NETContext _context;

        public IndexModel(WebApplication_ASP_.NET.Data.WebApplication_ASP_NETContext context)
        {
            _context = context;
        }
        public IList<Product> Product { get; set; } = default!;
        public SelectList Category { get; set; }

        // Propri�t� li�e � l'URL pour le filtrage par cat�gorie
        // SupportsGet=true permet la liaison des donn�es depuis l'URL
        [BindProperty(SupportsGet = true)]
        public string? ProductCategory { get; set; }

        // M�thode asynchrone appel�e lors du chargement de la page
        public async Task OnGetAsync()
        {
            // Cr�ation d'une requ�te LINQ pour r�cup�rer les produits
            var products = from m in _context.Product
                           select m;
            // Si une cat�gorie est s�lectionn�e, filtre les produits par cette cat�gorie
            if (!string.IsNullOrEmpty(ProductCategory))
            {
                products = products.Where(x => x.Category == ProductCategory);
            }

            Product = await products.ToListAsync();
        }


    }
}
