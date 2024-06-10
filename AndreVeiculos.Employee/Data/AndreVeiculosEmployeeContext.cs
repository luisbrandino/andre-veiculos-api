using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Models;

namespace AndreVeiculos.Employee.Data
{
    public class AndreVeiculosEmployeeContext : DbContext
    {
        public AndreVeiculosEmployeeContext (DbContextOptions<AndreVeiculosEmployeeContext> options)
            : base(options)
        {
        }

        public DbSet<Models.Employee> Employee { get; set; } = default!;

        public DbSet<Models.Address> Address { get; set; } = default!;

        public DbSet<Models.Role> Role { get; set; } = default!;
    }
}
