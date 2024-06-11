using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Models;

namespace AndreVeiculos.Bank.Data
{
    public class AndreVeiculosBankContext : DbContext
    {
        public AndreVeiculosBankContext (DbContextOptions<AndreVeiculosBankContext> options)
            : base(options)
        {
        }

        public DbSet<Models.Bank> Bank { get; set; } = default!;
    }
}
