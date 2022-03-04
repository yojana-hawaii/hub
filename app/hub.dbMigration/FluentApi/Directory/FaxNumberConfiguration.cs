using hub.domain.model.directory;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace hub.dbMigration.FluentApi.Directory
{
    public class FaxNumberConfiguration : IEntityTypeConfiguration<FaxNumber>
    {
        public void Configure(EntityTypeBuilder<FaxNumber> builder)
        {
            builder.HasKey(f => f.FaxId);
            builder
                .Property(f => f.FaxName)
                .IsRequired(false)
                .HasColumnType("varchar(50)");
            builder
                .Property(f => f.Number)
                .IsRequired(true)
                .HasColumnType("varchar(50)");

            builder
                .HasOne(e => e.Department)
                .WithMany(e => e.FaxNumbers)
                .HasForeignKey(e => e.DepartmentId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Restrict);
            builder
                .HasOne(e => e.Location)
                .WithMany(e => e.FaxNumbers)
                .HasForeignKey(e => e.LocationId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
