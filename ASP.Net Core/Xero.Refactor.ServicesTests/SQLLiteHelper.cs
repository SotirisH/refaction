using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xero.AspNet.Core;
using Xero.Refactor.Data;

namespace Xero.Refactor.ServicesTests
{
    internal static class DbMemoryHelper
    {
        private static RefactorDb db;
        public static RefactorDb Get()
        {
            if (db == null)
            {
                db = CreateMemoryDb();
            }
            return db;
        }

        public static void Dispose()
        {
            if (db == null)
            {
                db.Dispose();
                db = null;
            }
        }




        /// <summary>
        /// Creates a unit of work using a memory database
        /// </summary>
        /// <returns></returns>
        private static RefactorDb CreateMemoryDb()
        {
            var mockUser = new Mock<ICurrentUserService>();
            mockUser.Setup(m => m.GetCurrentUser()).Returns("TestUser");

            var connectionStringBuilder =
                    new SqliteConnectionStringBuilder { DataSource = ":memory:" };
            var connectionString = connectionStringBuilder.ToString();
            var connection = new SqliteConnection(connectionString);

            DbContextOptions<RefactorDb> options;
            var builder = new DbContextOptionsBuilder<RefactorDb>();
            builder.UseSqlite(connection);
            options = builder.Options;

            var context = new RefactorDb(options, mockUser.Object);

            context.Database.OpenConnection();
            context.Database.EnsureCreated();
            return context;

        }

    }
}
