using System.Data.Common;
using ExamScheduler.Contexts;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace ExamSchedulerTests.TestSetup
{
    public class TestsBase : IDisposable
    {
        private DbConnection? connection;
        private DbContextOptions<ApplicationContext>? contextOptions;

        public ApplicationContext GetSqliteDbContext()
        {
            connection = new SqliteConnection("Filename=:memory:");
            connection.Open();

            contextOptions = new DbContextOptionsBuilder<ApplicationContext>()
                .UseSqlite(connection)
                .Options;

            var context = new ApplicationContext(contextOptions);
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
            return context;
        }

        public void Dispose()
        {
            connection?.Dispose();
        }
    }
}
