using hub.domain.model.directory;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace hub.dbMigration.FluentApi.Directory
{
    public class JobTitleConfiguration : IEntityTypeConfiguration<JobTitle>
    {
        public void Configure(EntityTypeBuilder<JobTitle> builder)
        {
            builder.HasKey(j => j.JobTitleId);
            builder
                .Property(j => j.JobTitleName)
                .IsRequired(true)
                .HasColumnType("varchar(50)");
        }
    }
}
