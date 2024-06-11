using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Models;

namespace AndreVeiculos.Financing.Data
{
    public class AndreVeiculosFinancingContext : DbContext
    {
        public AndreVeiculosFinancingContext (DbContextOptions<AndreVeiculosFinancingContext> options)
            : base(options)
        {
        }

        public DbSet<Models.Financing> Financing { get; set; } = default!;
    }
}
