using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Models;

namespace AndreVeiculos.Address.Data
{
    public class AndreVeiculosAddressContext : DbContext
    {
        public AndreVeiculosAddressContext (DbContextOptions<AndreVeiculosAddressContext> options)
            : base(options)
        {
        }

        public DbSet<Models.Address> Address { get; set; } = default!;
    }
}
