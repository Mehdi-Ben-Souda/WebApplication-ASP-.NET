using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WebApplication_ASP_.NET.Data;
using WebApplication_ASP_.NET.Models;

namespace WebApplication_ASP_.NET.Pages.Products
{
    public class DetailsModel : PageModel
    {
        private readonly WebApplication_ASP_.NET.Data.WebApplication_ASP_NETContext _context;

        public DetailsModel(WebApplication_ASP_.NET.Data.WebApplication_ASP_NETContext context)
        {
            _context = context;
        }

      public Product Product { get; set; } = default!; 

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Product == null)
            {
                return NotFound();
            }

            var product = await _context.Product.FirstOrDefaultAsync(m => m.Id == id);
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
    }
}
