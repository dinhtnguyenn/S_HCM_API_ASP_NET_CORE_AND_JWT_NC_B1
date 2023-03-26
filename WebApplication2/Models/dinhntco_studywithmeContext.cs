using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace WebApplication2.Models
{
    public partial class dinhntco_studywithmeContext : DbContext
    {

        public dinhntco_studywithmeContext(DbContextOptions<dinhntco_studywithmeContext> options)
            : base(options)
        {

        }

        public virtual DbSet<Account> Accounts { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<fpt_login> Fpt_Logins { get; set; }
        public virtual DbSet<fpt_datalog> Fpt_Datalogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("dinhntco")
                .HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Account>(entity =>
            {
                entity.HasKey(e => e.No);

                entity.ToTable("Account", "dbo");

                entity.Property(e => e.Email).IsRequired();

                entity.Property(e => e.FullName).IsRequired();

                entity.Property(e => e.MSSV)
                    .IsRequired()
                    .HasMaxLength(10)
                    .HasColumnName("MSSV");

                entity.Property(e => e.Phone).HasMaxLength(10);
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(e => e.No);

                entity.ToTable("Product");

                entity.Property(e => e.ProductName).IsRequired();
            });

            modelBuilder.Entity<fpt_login>(entity =>
            {
                entity.HasKey(e => e.username);

                entity.ToTable("fpt_login");

                entity.Property(e => e.password).IsRequired();
            });

            modelBuilder.Entity<fpt_datalog>(entity =>
            {
                entity.HasKey(e => e.id);

                entity.ToTable("fpt_datalog");

            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
