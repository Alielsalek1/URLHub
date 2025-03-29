using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using URLshortner.Models;

namespace ALL.Database;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
    public DbSet<MappedUrl> MappedUrls { get; set; }
    public DbSet<UserFriend> UserFriends { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Configure User table
        modelBuilder.Entity<User>()
            .HasKey(u => u.Id);

        // one-to-one Relationship: User -> RefreshToken
        modelBuilder.Entity<User>()
            .HasOne(u => u.RefreshToken)
            .WithOne(rt => rt.User)
            .HasForeignKey<RefreshToken>(rt => rt.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        // one-to-many Relationship: User -> ActivationToken
        modelBuilder.Entity<User>()
            .HasMany(u => u.ActivationTokens)
            .WithOne(at => at.User)
            .HasForeignKey(at => at.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        // configure RefreshToken Join Table
        modelBuilder.Entity<RefreshToken>()
            .HasKey(rt => rt.UserId);

        // configure ActivationToken Join Table
        modelBuilder.Entity<ActivationToken>()
            .HasKey(at => at.Token);

        // Configure MappedUrl table
        modelBuilder.Entity<MappedUrl>()
            .HasKey(mu => mu.shortUrl);

        // Many-to-Many Relationship: User <-> MappedUrl
        modelBuilder.Entity<MappedUrl>()
            .HasMany(mu => mu.Users)
            .WithMany(u => u.MappedUrls)
            .UsingEntity(j => j.ToTable("UserMappedUrls"));

        // Many-to-Many Self-Referencing: User <-> User (Friends)
        modelBuilder.Entity<UserFriend>()
            .HasKey(uf => new { uf.UserId, uf.FriendId });

        modelBuilder.Entity<UserFriend>()
            .HasOne(uf => uf.User)
            .WithMany(u => u.Friends) // 'Friends' is a collection property on User
            .HasForeignKey(uf => uf.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<UserFriend>()
            .HasOne(uf => uf.Friend)
            .WithMany() // No inverse navigation property defined on User for the friend role
            .HasForeignKey(uf => uf.FriendId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}