﻿namespace Instagraph.Data
{
    using Instagraph.Models;
    using Microsoft.EntityFrameworkCore;

    public class InstagraphContext : DbContext
    {
        public InstagraphContext() { }

        public InstagraphContext(DbContextOptions options)
            :base(options) { }

        public DbSet<Picture> Pictures { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<Post> Posts { get; set; }

        public DbSet<Comment> Comments { get; set; }

        public DbSet<UserFollower> UsersFollowers { get; set; }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(Configuration.ConnectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Picture>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Path)
                    .IsRequired();

                entity.Property(e => e.Size)
                    .IsRequired();
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.HasAlternateKey(e => e.Username);

                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasMaxLength(30);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.HasOne(e => e.ProfilePicture)
                    .WithMany(pp => pp.Users)
                    .HasForeignKey(e => e.ProfilePictureId);
            });

            modelBuilder.Entity<Post>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Caption)
                    .IsRequired();

                entity.HasOne(e => e.User)
                    .WithMany(u => u.Posts)
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.Picture)
                    .WithMany(p => p.Posts)
                    .HasForeignKey(e => e.PictureId);
            });

            modelBuilder.Entity<Comment>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Content)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.HasOne(e => e.User)
                    .WithMany(u => u.Comments)
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.Post)
                    .WithMany(p => p.Comments)
                    .HasForeignKey(e => e.PostId);
            });

            modelBuilder.Entity<UserFollower>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.FollowerId });

                entity.HasOne(e => e.User)
                    .WithMany(u => u.Followers)
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.Follower)
                    .WithMany(f => f.UsersFollowing)
                    .HasForeignKey(e => e.FollowerId);
            });
        }
    }
}
