using Endpoints;
using Humanizer;
using Models;

namespace AndreVeiculos.Test
{
    public class HttpClientEndpointUnitTest
    {

        [Fact]
        public async Task CarFindAllEndpointTest()
        {
            var settings = new EndpointSettings { Url = "https://localhost:7046/api/Cars/ado" };
            var endpoint = new HttpClientEndpoint<Models.Car>(settings);

            List<Models.Car> cars = (List<Models.Car>) await endpoint.Find();

            Assert.True(cars.Any());
        }

        [Fact]
        public async Task CarFindEndpointTest()
        {
            var settings = new EndpointSettings { Url = "https://localhost:7046/api/Cars/ado" };
            var endpoint = new HttpClientEndpoint<Models.Car>(settings);

            Models.Car car = await endpoint.Find("ABC1234");

            Assert.Equal(car.LicensePlate, "ABC1234");
        }

        [Fact]
        public async Task CarInsertEndpointTest()
        {
            var settings = new EndpointSettings { Url = "https://localhost:7046/api/Cars/ado" };
            var endpoint = new HttpClientEndpoint<Models.Car>(settings);

            var licensePlate = new Random().Next(10000, 99999).ToString();

            Models.Car car = new Models.Car
            {
                LicensePlate = licensePlate,
                Name = $"test_{Guid.NewGuid()}",
                ModelYear = 2000,
                ManufactureYear = 2000,
                Sold = false
            };

            Models.Car insertedCar = await endpoint.Insert(car);

            Assert.Equal(car.LicensePlate, insertedCar.LicensePlate);
        }

        [Fact]
        public async Task CarJobInsertEndpointTest()
        {
            var settings = new EndpointSettings { Url = "https://localhost:7162/api/CarJob/ado" };
            var endpoint = new HttpClientEndpoint<CarJob>(settings);

            var carJob = new CarJob
            {
                Car = new Models.Car
                {
                    Name = "string",
                    LicensePlate = "ABC1234",
                    ModelYear = 1001,
                    ManufactureYear = 3000,
                    Sold = true
                },
                Job = new Models.Job
                {
                    Id = 2,
                    Description = "string"
                },
                Status = true
            };

            CarJob insertedCarJob = await endpoint.Insert(carJob);

            Assert.Equal(carJob.Id, insertedCarJob.Id);
            Assert.Equal(carJob.Car.LicensePlate, "ABC1234");
        }

        [Fact]
        public async Task CarJobFindEndpointTest()
        {
            var settings = new EndpointSettings { Url = "https://localhost:7162/api/CarJob/ado" };
            var endpoint = new HttpClientEndpoint<CarJob>(settings);

            CarJob carJobs = await endpoint.Find(1);

            Assert.Equal(carJobs.Id, 1);
            Assert.Equal(carJobs.Car.LicensePlate, "ABC1234");
        }

    }
}
