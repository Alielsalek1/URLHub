using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using URLshortner.Models;

namespace ALL.Database
{
    public class AppDbContext : DbContext
    {   
        public AppDbContext(DbContextOptions options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Friend>().ToTable("Friends").HasKey(u => new {u.ID1, u.ID2});
            modelBuilder.Entity<URL>().ToTable("URLS").HasKey(u => new {u.ID, u.Url});
            modelBuilder.Entity<User>().ToTable("Users").HasKey("ID");
        }
    }
}
