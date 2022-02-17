using hub.domain.model.directory;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace hub.dbMigration.FluentApi.Directory
{
    public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            //primary and alternate key
            builder.HasKey(e => e.EmployeeId);
            builder.HasAlternateKey(e => e.Username);
            builder.HasAlternateKey(e => e.Email);

            //not null values
            builder
                .Property(e => e.LastName)
                .IsRequired(true)
                .HasColumnType("varchar(50)");
            builder
                .Property(e => e.FirstName)
                .IsRequired(true)
                .HasColumnType("varchar(50)");
            builder
                .Property(e => e.AccountCreated)
                .IsRequired(true)
                .HasColumnType("DateTime2");

            //nullable columns
            builder
                .Property(e => e.FullNumber)
                .HasColumnType("varchar(50)")
                .IsRequired(false);
            builder
                .Property(e => e.Extension)
                .HasColumnType("varchar(50)")
                .IsRequired(false);
            builder
                .Property(e => e.HireDate)
                .HasColumnType("DateTime2")
                .IsRequired(false);
            builder.
                Property(e => e.NickName)
                .HasColumnType("varchar(50)")
                .IsRequired(false);
            builder.
                Property(e => e.EmployeeNumber)
                .HasColumnType("varchar(50)")
                .IsRequired(false);
            builder.
                Property(e => e.PhotoPath)
                .HasColumnType("varchar(50)")
                .IsRequired(false);

            //foreign keys
            builder
                .HasOne(e => e.JobTitle)
                .WithMany(j => j.Employees)
                .HasForeignKey(e => e.JobTitleId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Restrict);
            builder
                .HasOne(e => e.Department)
                .WithMany(e => e.Employees)
                .HasForeignKey(e => e.DepartmentId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Restrict);
            builder
                .HasOne(e => e.Location)
                .WithMany(e => e.Employees)
                .HasForeignKey(e => e.LocationId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Restrict);


            //self reference keys
            builder
                .HasOne(e => e.PrimaryManager)
                .WithMany(m => m.PrimaryStaff)
                .HasForeignKey(e => e.PrimaryManagerId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .Property(e => e.PrimaryManagerId)
                .IsRequired(false);

        }
    }
}
