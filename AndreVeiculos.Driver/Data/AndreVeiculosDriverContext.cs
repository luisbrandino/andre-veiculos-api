using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Models;

namespace AndreVeiculos.Driver.Data
{
    public class AndreVeiculosDriverContext : DbContext
    {
        public AndreVeiculosDriverContext (DbContextOptions<AndreVeiculosDriverContext> options)
            : base(options)
        {
        }

        public DbSet<Models.Driver> Driver { get; set; } = default!;

        public DbSet<Models.Category>? Category { get; set; }

        public DbSet<Models.DriverLicense>? DriverLicense { get; set; }
    }
}
