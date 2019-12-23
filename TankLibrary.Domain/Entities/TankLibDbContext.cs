namespace TankLibrary.Domain.Entities
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class TankLibDbContext : DbContext
    {
        public TankLibDbContext()
            : base("name=TankLibDbContext")
        {
        }

        public virtual DbSet<Tank> Tanks { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Tank>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<Tank>()
                .Property(e => e.Type)
                .IsUnicode(false);

            modelBuilder.Entity<Tank>()
                .Property(e => e.PlaceOfOrigin)
                .IsUnicode(false);

            modelBuilder.Entity<Tank>()
                .Property(e => e.Manufacturer)
                .IsUnicode(false);

            modelBuilder.Entity<Tank>()
                .Property(e => e.Armor)
                .IsUnicode(false);

            modelBuilder.Entity<Tank>()
                .Property(e => e.MainArmament)
                .IsUnicode(false);

            modelBuilder.Entity<Tank>()
                .Property(e => e.SecondaryArmament)
                .IsUnicode(false);

            modelBuilder.Entity<Tank>()
                .Property(e => e.Engine)
                .IsUnicode(false);

            modelBuilder.Entity<Tank>()
                .Property(e => e.Transmission)
                .IsUnicode(false);

            modelBuilder.Entity<Tank>()
                .Property(e => e.Suspension)
                .IsUnicode(false);

            modelBuilder.Entity<Tank>()
                .Property(e => e.FuelCapacity)
                .HasPrecision(18, 0);

            modelBuilder.Entity<Tank>()
                .Property(e => e.Description)
                .IsUnicode(false);

            modelBuilder.Entity<Tank>()
                .Property(e => e.ImagePath)
                .IsUnicode(false);

            modelBuilder.Entity<Tank>()
                .Property(e => e.ImageFile)
                .IsUnicode(false);
        }
    }
}
