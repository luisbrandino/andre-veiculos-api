using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Models;

namespace AndreVeiculos.FinancialPending.Data
{
    public class AndreVeiculosFinancialPendingContext : DbContext
    {
        public AndreVeiculosFinancialPendingContext (DbContextOptions<AndreVeiculosFinancialPendingContext> options)
            : base(options)
        {
        }

        public DbSet<Models.FinancialPending> FinancialPending { get; set; } = default!;
    }
}
