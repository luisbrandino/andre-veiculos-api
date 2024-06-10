using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Models;

namespace AndreVeiculos.Purchase.Data
{
    public class AndreVeiculosPurchaseContext : DbContext
    {
        public AndreVeiculosPurchaseContext (DbContextOptions<AndreVeiculosPurchaseContext> options)
            : base(options)
        {
        }

        public DbSet<Models.Purchase> Purchase { get; set; } = default!;

        public DbSet<Models.Car> Car { get; set; } = default!;
    }
}
