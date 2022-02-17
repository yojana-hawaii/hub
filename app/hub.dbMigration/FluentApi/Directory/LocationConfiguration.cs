using hub.domain.model.directory;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace hub.dbMigration.FluentApi.Directory
{
    public class LocationConfiguration : IEntityTypeConfiguration<Location>
    {
        public void Configure(EntityTypeBuilder<Location> builder)
        {
            builder.HasKey(l => l.LocationId);
            builder
                .Property(l => l.LocationName)
                .IsRequired(true)
                .HasColumnType("varchar(50)");
        }
    }
}
