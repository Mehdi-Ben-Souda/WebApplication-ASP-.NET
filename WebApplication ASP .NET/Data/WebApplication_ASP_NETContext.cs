using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebApplication_ASP_.NET.Models;

namespace WebApplication_ASP_.NET.Data
{
    public class WebApplication_ASP_NETContext : DbContext
    {
        public WebApplication_ASP_NETContext (DbContextOptions<WebApplication_ASP_NETContext> options)
            : base(options)
        {
        }

        public DbSet<WebApplication_ASP_.NET.Models.Product> Product { get; set; } = default!;

        public DbSet<WebApplication_ASP_.NET.Models.Movie>? Movie { get; set; }
    }
}
