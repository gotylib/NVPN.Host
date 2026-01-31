using DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Context
{
    public class VlessLinkConfiguration : IEntityTypeConfiguration<VlessLink>
    {
        public void Configure(EntityTypeBuilder<VlessLink> builder)
        {
        
            builder.HasKey(v => v.Id)
                .HasName("PK_VlessLinks");

         
            builder.Property(v => v.Link)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(v => v.CreatedDate)
                .IsRequired()
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.Property(v => v.ExpiryDate);

            builder.Property(v => v.IsActive)
                .IsRequired()
                .HasDefaultValue(true);

            // Связь с пользователем
            builder.HasOne(v => v.User)
                .WithMany(u => u.VlessLinks)
                .HasForeignKey(v => v.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Связь с сервером
            builder.HasOne(v => v.Server)
                .WithMany(s => s.VlessLinks)
                .HasForeignKey(v => v.ServerId)
                .OnDelete(DeleteBehavior.Cascade);

            // Индексы
            builder.HasIndex(v => v.UserId)
                .HasDatabaseName("IX_VlessLinks_UserId");

            builder.HasIndex(v => v.ServerId)
                .HasDatabaseName("IX_VlessLinks_ServerId");

            builder.HasIndex(v => v.IsActive)
                .HasDatabaseName("IX_VlessLinks_IsActive");

            builder.HasIndex(v => v.ExpiryDate)
                .HasDatabaseName("IX_VlessLinks_ExpiryDate");

            // Составной индекс для частых запросов
            builder.HasIndex(v => new { v.UserId, v.IsActive })
                .HasDatabaseName("IX_VlessLinks_UserId_IsActive");

            builder.HasIndex(v => new { v.ServerId, v.IsActive })
                .HasDatabaseName("IX_VlessLinks_ServerId_IsActive");

        }
    }
}