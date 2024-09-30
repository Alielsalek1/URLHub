using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using URLshortner.Models;

namespace ALL.Database;

public class AppDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<Friend> Friends { get; set; }
    public DbSet<URL> URLs { get; set; }
    public DbSet<User> Users { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Friend>().HasKey(u => new {u.ID1, u.ID2});
        modelBuilder.Entity<URL>().HasKey(u => new {u.ID, u.Url});
        modelBuilder.Entity<User>().HasKey("ID");
    }
}
