using hub.domain.model.directory;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace hub.dbMigration.FluentApi.Directory
{
    public class DepartmentConfiguration : IEntityTypeConfiguration<Department>
    {
        public void Configure(EntityTypeBuilder<Department> builder)
        {
            builder
                .HasKey(d => d.DepartmentId);
            builder
                .Property(d => d.DepartmentName)
                .IsRequired(true)
                .HasColumnType("varchar(50)");
        }
    }
}
