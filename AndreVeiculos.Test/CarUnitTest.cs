using AndreVeiculos.Car.Controllers;
using AndreVeiculos.Car.Data;
using AndreVeiculos.Job.Controllers;
using AndreVeiculos.Job.Data;

using Microsoft.EntityFrameworkCore;

namespace AndreVeiculos.Test
{
    public class CarUnitTest
    {
        private DbContextOptions<AndreVeiculosCarContext> _options;

        private void InitDatabase()
        {
            _options = new DbContextOptionsBuilder<AndreVeiculosCarContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;

            using (var context = new AndreVeiculosCarContext(_options))
            {
                for (int i = 0; i < 5; i++)
                    context.Car.Add(new Models.Car()
                    {
                        LicensePlate = new Random().Next(1000000, 9999999).ToString(),
                        Name = "Voyage",
                        ModelYear = 1992,
                        ManufactureYear = 2001,
                        Sold = false
                    });

                context.SaveChanges();
            }
        }

        [Fact]
        public void TestFindAll()
        {
            InitDatabase();

            using (var context = new AndreVeiculosCarContext(_options))
            {
                CarsController controller = new(context);

                var cars = controller.GetCar("ado").Result.Value;

                Assert.Equal(5, cars.Count());
            }
        }

        [Fact]
        public void TestFind()
        {
            InitDatabase();

            using (var context = new AndreVeiculosCarContext(_options))
            {
                CarsController controller = new(context);

                var licensePlate = controller.GetCar("ado").Result.Value.First().LicensePlate;

                var car = controller.GetCar("ado", licensePlate).Result.Value;

                Assert.Equal(licensePlate, car.LicensePlate);
            }
        }

        [Fact]
        public void TestInsert()
        {
            InitDatabase();

            using (var context = new AndreVeiculosCarContext(_options))
            {
                CarsController controller = new(context);

                var car = new Models.Car()
                {
                    LicensePlate = "ABE2929",
                    Name = "Citroen",
                    ManufactureYear = 2001,
                    ModelYear = 2000,
                    Sold = true
                };

                var carReturnedFromController = controller.PostCar("ado", car).Result.Value;

                Assert.Equal(carReturnedFromController.Name, car.Name);
                Assert.True(carReturnedFromController.Sold);
            }
        }
    }
}
