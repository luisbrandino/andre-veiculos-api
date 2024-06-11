using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Models;

namespace AndreVeiculos.Insurance.Data
{
    public class AndreVeiculosInsuranceContext : DbContext
    {
        public AndreVeiculosInsuranceContext (DbContextOptions<AndreVeiculosInsuranceContext> options)
            : base(options)
        {
        }

        public DbSet<Models.Insurance> Insurance { get; set; } = default!;
    }
}
