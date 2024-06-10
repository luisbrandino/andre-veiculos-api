using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Models;

namespace AndreVeiculos.Car.Data
{
    public class AndreVeiculosCarContext : DbContext
    {
        public AndreVeiculosCarContext (DbContextOptions<AndreVeiculosCarContext> options)
            : base(options)
        {
        }

        public DbSet<Models.Car> Car { get; set; } = default!;
    }
}
