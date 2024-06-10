using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Models;

namespace AndreVeiculos.Sale.Data
{
    public class AndreVeiculosSaleContext : DbContext
    {
        public AndreVeiculosSaleContext (DbContextOptions<AndreVeiculosSaleContext> options)
            : base(options)
        {
        }

        public DbSet<Models.Sale> Sale { get; set; } = default!;
        public DbSet<Models.Client> Client { get; set; } = default!;
        public DbSet<Models.Employee> Employee { get; set; } = default!;
        public DbSet<Models.Payment> Payment { get; set; } = default!;
        public DbSet<Models.Car> Car { get; set; } = default!;
    }
}
