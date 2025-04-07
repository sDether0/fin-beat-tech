using FinBeatTech.Dto;
using FinBeatTech.Models;

using Microsoft.EntityFrameworkCore;

namespace FinBeatTech.Database
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            var dataItem = modelBuilder.Entity<DataItem>();
            dataItem.ToTable("dataitems");
            dataItem.Property(x => x.Code).HasColumnName("code");
            dataItem.Property(x => x.Id).HasColumnName("id");
            dataItem.Property(x => x.Value).HasColumnName("value");
        }

        public DbSet<DataItem> DataItems { get; set; }
    }
}
