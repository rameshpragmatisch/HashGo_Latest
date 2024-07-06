using HashGo.Core.Db;
using HashGo.Infrastructure.Setting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace HashGo.Domain.DataContext
{
    public class HashGoCacheContext : DbContext
    {
        public DbSet<TenantConnect> ConnectItems { get; set; }
        public DbSet<ProductDetail> ProductItems { get; set; }
        public DbSet<QueueSettings> QueueSettings { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Filename="+LocalSetting.DbPath+"\\HashGoCache.db", options =>
            {
                options.MigrationsAssembly(Assembly.GetExecutingAssembly().FullName);
            });
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TenantConnect>().ToTable(nameof(this.ConnectItems), "HashGo");
            modelBuilder.Entity<ProductDetail>().ToTable(nameof(this.ProductItems), "HashGo");
            modelBuilder.Entity<QueueSettings>().ToTable(nameof(this.QueueSettings), "HashGo");

            base.OnModelCreating(modelBuilder);
        }
    }
}
