using DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Context
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            
            builder.HasKey(u => u.Id);


            builder.Property(u => u.Username)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(u => u.Email)
                .HasMaxLength(100);

            builder.Property(u => u.HashPassword)
                .IsRequired()
                .HasMaxLength(256); // Длина SHA256 хэша

            builder.Property(u => u.Salt)
                .IsRequired()
                .HasMaxLength(32);

            // Индексы

            // Индекс по Email
            builder.HasIndex(u => u.Email)
                .IsUnique()
                .HasFilter(@"""Email"" IS NOT NULL")
                .HasDatabaseName("IX_Users_Email");

            // Индекс по Username
            builder.HasIndex(u => u.Username)
                .HasDatabaseName("IX_Users_Username");

            // Связь с VlessLinks (один ко многим)
            builder.HasMany(u => u.VlessLinks)
                .WithOne(v => v.User)
                .HasForeignKey(v => v.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_VlessLinks_Users");

            // Связь с Servers (многие ко многим)
            builder.HasMany(u => u.Servers)
                .WithMany(s => s.Users)
                .UsingEntity<UserServer>(
                    j => j
                        .HasOne(us => us.Server)
                        .WithMany()
                        .HasForeignKey(us => us.ServerId)
                        .OnDelete(DeleteBehavior.Cascade),
                    j => j
                        .HasOne(us => us.User)
                        .WithMany()
                        .HasForeignKey(us => us.UserId)
                        .OnDelete(DeleteBehavior.Cascade),
                    j =>
                    {
                        j.HasKey(us => new { us.UserId, us.ServerId });
                        j.ToTable("UserServers");
                    });
        }
    }
}