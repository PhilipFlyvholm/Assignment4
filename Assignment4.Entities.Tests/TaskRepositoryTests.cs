using System.IO;
using Assignment4.Core;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace Assignment4.Entities.Tests
{
    public class TaskRepositoryTests
    {
        [Fact]
        public void Does_TaskRepository_Work()
        {
            //Given
            var config = LoadConfiguration();
            using var repository = new TaskRepository(new SqlConnection(config.GetConnectionString("Kanban")));
            var taskId = repository.Create(new TaskDTO
            {
                Title = "Test title",
                Description = "Description",
                State = State.NEW,
                AssignedToId = 1
            });

            var taskDetails = repository.FindById(taskId);
            Assert.Equal("Peter Nielsen", taskDetails.AssignedToName);
        }
        static IConfiguration LoadConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .AddUserSecrets<TaskRepositoryTests>();

            return builder.Build();
        }
    }
}
