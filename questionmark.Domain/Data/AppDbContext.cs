using Microsoft.EntityFrameworkCore;
using questionmark.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace questionmark.Domain.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Session> Sessions { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Reaction> Reactions { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Session>(entity =>
            {
                entity.ToTable("session");
                entity.HasKey(e => e.ID);
                entity.Property(e => e.ID)
                    .HasColumnName("session_id")
                    .HasColumnType("bigint unsigned")
                    .IsRequired()
                    .ValueGeneratedOnAdd();
                entity.Property(e => e.hash)
                    .HasColumnName("session_hash")
                    .HasColumnType("binary(32)")
                    .IsRequired();
                entity.Property(e => e.nonce)
                    .HasColumnName("session_nonce")
                    .HasColumnType("binary(12)")
                    .IsRequired();
                entity.Property(e => e.tag)
                    .HasColumnName("session_tag")
                    .HasColumnType("binary(16)")
                    .IsRequired();
                entity.Property(e => e.end)
                    .HasColumnName("session_end")
                    .HasColumnType("timestamp")
                    .IsRequired()
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");
                entity.HasIndex(e => e.hash)
                    .IsUnique();
            });
            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("user");
                entity.HasKey(e => e.ID);
                entity.Property(e => e.ID)
                    .HasColumnName("user_id")
                    .HasColumnType("int unsigned")
                    .IsRequired()
                    .ValueGeneratedOnAdd();
                entity.Property(e => e.auth)
                    .HasColumnName("auth")
                    .HasColumnType("binary(64)")
                    .IsRequired();
                entity.HasIndex(e => e.auth)
                    .IsUnique();
            });
            modelBuilder.Entity<Post>(entity =>
            {
                entity.ToTable("post");
                entity.HasKey(e => e.ID);
                entity.Property(e => e.ID)
                    .HasColumnName("post_id")
                    .HasColumnType("int unsigned")
                    .IsRequired()
                    .ValueGeneratedOnAdd();
                entity.Property(e => e.text)
                    .HasColumnName("text")
                    .HasColumnType("varchar(512)")
                    .IsRequired();
                entity.Property(e => e.date)
                    .HasColumnName("date_of_post")
                    .HasColumnType("timestamp")
                    .IsRequired()
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");
                entity.Property(e => e.user_id)
                    .HasColumnName("user_id")
                    .HasColumnType("int unsigned")
                    .IsRequired();
                entity.HasOne(e => e.User)
                    .WithMany(p => p.Posts)
                    .HasForeignKey(e => e.user_id)
                    .OnDelete(DeleteBehavior.Restrict);
            });
            modelBuilder.Entity<Reaction>(entity =>
            {
                entity.ToTable("reaction");
                entity.HasKey(e => e.ID);
                entity.Property(e => e.ID)
                    .HasColumnName("reaction_id")
                    .HasColumnType("bigint unsigned")
                    .IsRequired()
                    .ValueGeneratedOnAdd();
                entity.Property(e => e.post_id)
                    .HasColumnName("post_id")
                    .HasColumnType("int unsigned")
                    .IsRequired();
                entity.Property(e => e.date)
                    .HasColumnName("date_of_reaction")
                    .HasColumnType("timestamp")
                    .IsRequired()
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");
                entity.Property(e => e.user_id)
                    .HasColumnName("user_id")
                    .HasColumnType("int unsigned")
                    .IsRequired();
                entity.Property(e => e.reaction)
                    .HasColumnName("reaction")
                    .HasColumnType("bit(1)")
                    .IsRequired();
                entity.HasIndex(e => new { e.post_id, e.user_id })
                    .IsUnique()
                    .HasDatabaseName("uq_post_id_user_id");
                entity.HasOne(e => e.Post)
                    .WithMany(p => p.Reactions)
                    .HasForeignKey(e => e.post_id)
                    .OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(e => e.User)
                    .WithMany(p => p.Reactions)
                    .HasForeignKey(e => e.user_id)
                    .OnDelete(DeleteBehavior.Restrict);
            });
            modelBuilder.Entity<Comment>(entity =>
            {
                entity.ToTable("comment");
                entity.HasKey(e => e.ID);
                entity.Property(e => e.ID)
                    .HasColumnName("comment_id")
                    .HasColumnType("bigint unsigned")
                    .IsRequired()
                    .ValueGeneratedOnAdd();
                entity.Property(e => e.user_id)
                    .HasColumnName("user_id")
                    .HasColumnType("int unsigned")
                    .IsRequired();
                entity.Property(e => e.post_id)
                    .HasColumnName("post_id")
                    .HasColumnType("int unsigned")
                    .IsRequired();
                entity.Property(e => e.date)
                    .HasColumnName("date_of_comment")
                    .HasColumnType("timestamp")
                    .IsRequired()
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");
                entity.Property(e => e.text)
                    .HasColumnName("text")
                    .HasColumnType("varchar(512)")
                    .IsRequired();
                entity.HasOne(e => e.Post)
                    .WithMany(p => p.Comments)
                    .HasForeignKey(e => e.post_id)
                    .OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(e => e.User)
                    .WithMany(p => p.Comments)
                    .HasForeignKey(e => e.user_id)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}
