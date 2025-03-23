﻿// <auto-generated />
using System;
using ALL.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace URLshortner.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("MappedUrlUser", b =>
                {
                    b.Property<string>("MappedUrlsshortUrl")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("UsersId")
                        .HasColumnType("int");

                    b.HasKey("MappedUrlsshortUrl", "UsersId");

                    b.HasIndex("UsersId");

                    b.ToTable("UserMappedUrls", (string)null);
                });

            modelBuilder.Entity("URLshortner.Models.MappedUrl", b =>
                {
                    b.Property<string>("shortUrl")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("longUrl")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("shortUrl");

                    b.ToTable("MappedUrls");
                });

            modelBuilder.Entity("URLshortner.Models.RefreshToken", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<DateTime>("Expires")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsRevoked")
                        .HasColumnType("bit");

                    b.Property<string>("Token")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId");

                    b.ToTable("RefreshToken");
                });

            modelBuilder.Entity("URLshortner.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Role")
                        .HasColumnType("int");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("URLshortner.Models.UserFriend", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<int>("FriendId")
                        .HasColumnType("int");

                    b.HasKey("UserId", "FriendId");

                    b.HasIndex("FriendId");

                    b.ToTable("UserFriends");
                });

            modelBuilder.Entity("MappedUrlUser", b =>
                {
                    b.HasOne("URLshortner.Models.MappedUrl", null)
                        .WithMany()
                        .HasForeignKey("MappedUrlsshortUrl")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("URLshortner.Models.User", null)
                        .WithMany()
                        .HasForeignKey("UsersId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("URLshortner.Models.RefreshToken", b =>
                {
                    b.HasOne("URLshortner.Models.User", "User")
                        .WithOne("RefreshToken")
                        .HasForeignKey("URLshortner.Models.RefreshToken", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("URLshortner.Models.UserFriend", b =>
                {
                    b.HasOne("URLshortner.Models.User", "Friend")
                        .WithMany()
                        .HasForeignKey("FriendId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("URLshortner.Models.User", "User")
                        .WithMany("Friends")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Friend");

                    b.Navigation("User");
                });

            modelBuilder.Entity("URLshortner.Models.User", b =>
                {
                    b.Navigation("Friends");

                    b.Navigation("RefreshToken")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
