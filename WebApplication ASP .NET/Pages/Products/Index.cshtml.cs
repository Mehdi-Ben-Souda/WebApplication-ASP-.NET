using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication_ASP_.NET.Data;
using WebApplication_ASP_.NET.Models;

namespace WebApplication_ASP_.NET.Pages.Products
{
    public class IndexModel : PageModel
    {
        private readonly WebApplication_ASP_.NET.Data.WebApplication_ASP_NETContext _context;

        public IndexModel(WebApplication_ASP_.NET.Data.WebApplication_ASP_NETContext context)
        {
            _context = context;
        }

        public IList<Product> Product { get;set; } = default!;

        public async Task OnGetAsync()
        {
            var products = from m in _context.Product
                         select m;
            if (!string.IsNullOrEmpty(SearchString))
            {
                products = products.Where(s => s.Category.Contains(SearchString));
            }

            Product = await products.ToListAsync();
        }

        [BindProperty(SupportsGet = true)]
        public string? SearchString { get; set; }

        public SelectList Category { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? ProductCategory { get; set; }

    }
}
