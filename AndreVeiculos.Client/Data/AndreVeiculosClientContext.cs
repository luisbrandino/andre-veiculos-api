using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Models;

namespace AndreVeiculos.Client.Data
{
    public class AndreVeiculosClientContext : DbContext
    {
        public AndreVeiculosClientContext (DbContextOptions<AndreVeiculosClientContext> options)
            : base(options)
        {
        }

        public DbSet<Models.Client> Client { get; set; } = default!;

        public DbSet<Models.Address> Address { get; set; } = default!;
    }
}
