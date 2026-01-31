using DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Context
{
    public class ServerConfiguration : IEntityTypeConfiguration<Server>
    {
        public void Configure(EntityTypeBuilder<Server> builder)
        {
            builder.HasKey(e => e.Id);

            builder.Property(s => s.Host)
                .IsRequired()
                .HasMaxLength(100);


            builder.Property(s => s.Country)
                .IsRequired()
                .HasMaxLength(50);


            builder.Property(s => s.AdminLogin)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(s => s.AdminPassword)
                .IsRequired()
                .HasMaxLength(100);

            // Уникальный индекс на Host (не может быть двух серверов с одинаковым хостом)
            builder.HasIndex(s => s.Host)
                .IsUnique();

           

            // Связь с VlessLinks (один сервер - много Vless ссылок)
            builder.HasMany(s => s.VlessLinks)
                .WithOne(v => v.Server)
                .HasForeignKey(v => v.ServerId)
                .OnDelete(DeleteBehavior.Cascade); 

            
        }
    }
}