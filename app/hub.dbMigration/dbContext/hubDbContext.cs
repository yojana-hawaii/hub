using hub.domain.model.directory;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace hub.dbMigration.dbContext
{
    public class HubDbContext : DbContext
    {
        public HubDbContext(DbContextOptions<HubDbContext> options) : base(options) { }


        public DbSet<Employee> Employees { get; set; }
        public DbSet<JobTitle> JobTitles { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<FaxNumber> FaxNumbers { get; set; }
        //public DbSet<Specialist> Specialists { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //apply all fluent api configuration to entity using reflection
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
