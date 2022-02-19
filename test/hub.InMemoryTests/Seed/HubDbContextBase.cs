using hub.dbMigration.dbContext;
using Microsoft.EntityFrameworkCore;
using System;


namespace hub.InMemoryTests.Seed
{
    public class HubDbContextBase : IDisposable
    {
        protected readonly HubDbContext _context;
        public HubDbContextBase()
        {
            var dbName = "TheHub" + DateTime.Now.ToFileTimeUtc();

            //insert seed data into database using one instance of the context
            var options = new DbContextOptionsBuilder<HubDbContext>()
               .UseInMemoryDatabase(databaseName: dbName)
               .EnableSensitiveDataLogging()
               .Options;

            _context = new HubDbContext(options);

            _context.Database.EnsureCreated();

            HubDbInitializer.Initialize(_context);

        }
        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
    }
}
