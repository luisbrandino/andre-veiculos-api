using AndreVeiculos.Address.Controllers;
using AndreVeiculos.Address.Data;
using AndreVeiculos.Job.Controllers;
using AndreVeiculos.Job.Data;
using Microsoft.EntityFrameworkCore;

namespace AndreVeiculos.Test
{
    public class JobUnitTest
    {
        private DbContextOptions<AndreVeiculosJobContext> _options;

        private void InitDatabase()
        {
            _options = new DbContextOptionsBuilder<AndreVeiculosJobContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;

            using (var context = new AndreVeiculosJobContext(_options))
            {
                for (int i = 0; i < 5; i++)
                    context.Job.Add(new Models.Job()
                    {
                        Description = $"{new Random().Next(10000, 99999)}"
                    });

                context.SaveChanges();
            }
        }

        [Fact]
        public void TestFindAll()
        {
            InitDatabase();

            using (var context = new AndreVeiculosJobContext(_options))
            {
                JobsController controller = new(context);

                var jobs = controller.GetJob("ado").Result.Value;

                Assert.Equal(5, jobs.Count());
            }
        }

        [Fact]
        public void TestFind()
        {
            InitDatabase();

            using (var context = new AndreVeiculosJobContext(_options))
            {
                JobsController controller = new(context);

                var job = controller.GetJob("ado", 1).Result.Value;

                Assert.Equal(1, job.Id);
            }
        }

        [Fact]
        public void TestInsert()
        {
            InitDatabase();

            using (var context = new AndreVeiculosJobContext(_options))
            {
                JobsController controller = new(context);

                var job = new Models.Job()
                {
                    Description = "Teste"
                };

                var jobReturnedFromController = controller.PostJob("ado", job).Result.Value;

                Assert.Equal(jobReturnedFromController.Description, job.Description);
            }
        }
    }
}
