using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication_ASP_.NET.Models;
using System.Text.Json;

namespace WebApplication_ASP_.NET.Pages.User
{

    // Modèle de la page d'index qui affiche la liste des produits
    public class IndexModel : PageModel
    {
        // Injection de dépendance du contexte de base de données
        private readonly WebApplication_ASP_.NET.Data.WebApplication_ASP_NETContext _context;

        public IndexModel(WebApplication_ASP_.NET.Data.WebApplication_ASP_NETContext context)
        {
            _context = context;
        }
        public IList<Product> Product { get; set; } = default!;
        public SelectList Category { get; set; }

        // Propriété liée à l'URL pour le filtrage par catégorie
        // SupportsGet=true permet la liaison des données depuis l'URL
        [BindProperty(SupportsGet = true)]
        public string? ProductCategory { get; set; }

        // Méthode asynchrone appelée lors du chargement de la page
        public async Task OnGetAsync()
        {
            // Création d'une requête LINQ pour récupérer les produits
            var products = from m in _context.Product
                           select m;
            // Si une catégorie est sélectionnée, filtre les produits par cette catégorie
            if (!string.IsNullOrEmpty(ProductCategory))
            {
                products = products.Where(x => x.Category == ProductCategory);
            }

            Product = await products.ToListAsync();
        }


    }
}
