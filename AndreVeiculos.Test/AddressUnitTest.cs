using AndreVeiculos.Address.Controllers;
using AndreVeiculos.Address.Data;
using AndreVeiculos.Address.Database;
using AndreVeiculos.Address.DTO;
using AndreVeiculos.Address.MongoServices;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Mongo2Go;
using Repositories;

namespace AndreVeiculos.Test
{
    public class AddressUnitTest
    {
        private DbContextOptions<AndreVeiculosAddressContext> _options;

        private void InitDatabase()
        {
            _options = new DbContextOptionsBuilder<AndreVeiculosAddressContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;

            using (var context = new AndreVeiculosAddressContext(_options))
            {
                for (int i = 0; i < 5; i++)
                    context.Address.Add(new Models.Address()
                    {
                        PostalCode = $"{new Random().Next(100000, 999999)}",
                        State = "SP",
                        Street = "Rua dos Testes",
                        StreetType = "Rua",
                        City = "Matão",
                        Complement = "Casa B",
                        Neighborhood = "Pereira",
                        Number = 4
                    });

                context.SaveChanges();
            }
        }

        [Fact]
        public void TestFindAll()
        {
            InitDatabase();

            using (var context = new AndreVeiculosAddressContext(_options))
            {
                AddressesController controller = new(context);

                IEnumerable<Models.Address> addresses = controller.GetAddress("ado").Result.Value;

                Assert.Equal(5, addresses.Count());
            }
        }

        [Fact]
        public void TestFind()
        {
            InitDatabase();

            using (var context = new AndreVeiculosAddressContext(_options))
            {
                AddressesController controller = new(context);

                var address = controller.GetAddress("ado", 1).Result.Value;

                Assert.Equal(1, address.Id);
            }
        }

        [Fact]
        public void TestInsert()
        {
            InitDatabase();

            var runner = MongoDbRunner.Start();

            var services = new ServiceCollection();

            services.AddSingleton<IMongoDatabaseSettings>(_ => new MongoDatabaseSettings()
            {
                ConnectionString = runner.ConnectionString, // in memory
                AddressCollectionName = "Address",
                DatabaseName = "AndreVeiculosAPI"
            });

            services.AddSingleton<AddressService>();

            var serviceProvider = services.BuildServiceProvider();

            using (var context = new AndreVeiculosAddressContext(_options))
            {
                var httpContext = new DefaultHttpContext();
                httpContext.RequestServices = serviceProvider;

                AddressesController controller = new(context)
                {
                    ControllerContext = new Microsoft.AspNetCore.Mvc.ControllerContext
                    {
                        HttpContext = httpContext
                    }
                };

                var address = new AddressDTO()
                {
                    PostalCode = "15990-840",
                    Complement = "Casa D",
                    Number = 2
                };

                var addressReturnedFromController = controller.PostAddress("ado", address).Result.Value;

                Assert.Equal(addressReturnedFromController.PostalCode, address.PostalCode);
            }

        }
    }
}