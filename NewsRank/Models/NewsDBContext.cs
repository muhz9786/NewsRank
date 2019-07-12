using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace NewsRank.Models
{
    public partial class NewsDBContext : DbContext
    {
        public NewsDBContext()
        {
        }

        public NewsDBContext(DbContextOptions<NewsDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<TblNews> TblNews { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TblNews>(entity =>
            {
                entity.ToTable("tbl_news", "newsdb");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasColumnType("int(11)")
                    .ValueGeneratedNever();

                entity.Property(e => e.NewsContent)
                    .HasColumnName("NEWS_CONTENT")
                    .IsUnicode(false);

                entity.Property(e => e.NewsRank)
                    .HasColumnName("news_rank")
                    .HasColumnType("int(11)");

                entity.Property(e => e.NewsTitle)
                    .IsRequired()
                    .HasColumnName("news_title")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.NewsType)
                    .HasColumnName("news_type")
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.NewsUrl)
                    .HasColumnName("news_url")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.SubmitTime).HasColumnName("submit_time");
            });
        }
    }
}
