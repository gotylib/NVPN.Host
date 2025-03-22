using DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace DAL.Context;

public class VpnDbContext : DbContext
{
    public VpnDbContext(DbContextOptions<VpnDbContext> options) : base(options){}
    
    public DbSet<User> Users { get; set; }
    
    public DbSet<Server> Servers { get; set; }
}