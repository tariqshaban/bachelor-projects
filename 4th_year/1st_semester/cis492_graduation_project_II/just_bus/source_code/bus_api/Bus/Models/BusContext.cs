using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

#nullable disable

namespace Bus.Models
{
    public partial class BusContext : DbContext
    {
        private readonly IConfiguration _configuration;

        public BusContext(IConfiguration Configuration)
        {
            _configuration = Configuration;
        }

        public BusContext(DbContextOptions<BusContext> options)
            : base(options)
        {
        }

        public virtual DbSet<ApiAccess> ApiAccesses { get; set; }
        public virtual DbSet<Configuration> Configurations { get; set; }
        public virtual DbSet<Driver> Drivers { get; set; }
        public virtual DbSet<Feedback> Feedbacks { get; set; }
        public virtual DbSet<Image> Images { get; set; }
        public virtual DbSet<Path> Paths { get; set; }
        public virtual DbSet<Person> People { get; set; }
        public virtual DbSet<RefreshToken> RefreshTokens { get; set; }
        public virtual DbSet<Route> Routes { get; set; }
        public virtual DbSet<Stop> Stops { get; set; }
        public virtual DbSet<Vehicle> Vehicles { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(_configuration.GetConnectionString("BusConnectionString"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<ApiAccess>(entity =>
            {
                entity.ToTable("ApiAccess");

                entity.Property(e => e.Action)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.DateAccessed).HasColumnType("datetime");

                entity.Property(e => e.Path)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<Configuration>(entity =>
            {
                entity.ToTable("Configuration");

                entity.Property(e => e.AppVersion)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.DriverLocationSetterInterval).HasColumnName("driverLocationSetterInterval");

                entity.Property(e => e.ImageDrawerDirectory)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.ModificationDate).HasColumnType("datetime");

                entity.Property(e => e.SpeedContributionFactor).HasDefaultValueSql("((1))");

                entity.HasOne(d => d.ModifierNavigation)
                    .WithMany(p => p.Configurations)
                    .HasForeignKey(d => d.Modifier);
            });

            modelBuilder.Entity<Driver>(entity =>
            {
                entity.HasKey(e => e.PersonId);

                entity.ToTable("Driver");

                entity.Property(e => e.PersonId).ValueGeneratedNever();

                entity.Property(e => e.CreationDate).HasColumnType("datetime");

                entity.Property(e => e.LastLatitude).HasColumnType("decimal(8, 6)");

                entity.Property(e => e.LastLocationUpdate).HasColumnType("datetime");

                entity.Property(e => e.LastLongitude).HasColumnType("decimal(9, 6)");

                entity.Property(e => e.ModificationDate).HasColumnType("datetime");

                entity.HasOne(d => d.CreatorNavigation)
                    .WithMany(p => p.DriverCreatorNavigations)
                    .HasForeignKey(d => d.Creator);

                entity.HasOne(d => d.Image)
                    .WithMany(p => p.Drivers)
                    .HasForeignKey(d => d.ImageId)
                    .HasConstraintName("FK_Driver_Image");

                entity.HasOne(d => d.ModifierNavigation)
                    .WithMany(p => p.DriverModifierNavigations)
                    .HasForeignKey(d => d.Modifier);

                entity.HasOne(d => d.Person)
                    .WithOne(p => p.DriverPerson)
                    .HasForeignKey<Driver>(d => d.PersonId)
                    .HasConstraintName("FK_Driver_Person");

                entity.HasOne(d => d.Vehicle)
                    .WithMany(p => p.Drivers)
                    .HasForeignKey(d => d.VehicleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Driver_Vehicle");
            });

            modelBuilder.Entity<Feedback>(entity =>
            {
                entity.ToTable("Feedback");

                entity.Property(e => e.DateCreated)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.Message)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.Model).HasMaxLength(150);

                entity.Property(e => e.Os)
                    .HasMaxLength(150)
                    .HasColumnName("OS");

                entity.Property(e => e.Phone).HasMaxLength(15);

                entity.HasOne(d => d.Person)
                    .WithMany(p => p.Feedbacks)
                    .HasForeignKey(d => d.PersonId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_Feedback_Person");
            });

            modelBuilder.Entity<Image>(entity =>
            {
                entity.ToTable("Image");

                entity.Property(e => e.CreationDate).HasColumnType("datetime");

                entity.Property(e => e.Directory)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasColumnName("directory");

                entity.Property(e => e.Is360).HasColumnName("is360");

                entity.Property(e => e.ModificationDate).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("name");

                entity.HasOne(d => d.CreatorNavigation)
                    .WithMany(p => p.ImageCreatorNavigations)
                    .HasForeignKey(d => d.Creator);

                entity.HasOne(d => d.ModifierNavigation)
                    .WithMany(p => p.ImageModifierNavigations)
                    .HasForeignKey(d => d.Modifier);
            });

            modelBuilder.Entity<Path>(entity =>
            {
                entity.HasKey(e => new { e.RouteId, e.Type });

                entity.ToTable("Path");

                entity.Property(e => e.AverageSpeed).HasColumnType("decimal(6, 3)");

                entity.Property(e => e.CreationDate).HasColumnType("datetime");

                entity.Property(e => e.EndName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.EndNameAr)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.ModificationDate).HasColumnType("datetime");

                entity.Property(e => e.Path1)
                    .IsRequired()
                    .HasColumnName("Path");

                entity.Property(e => e.StartName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.StartNameAr)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.HasOne(d => d.CreatorNavigation)
                    .WithMany(p => p.PathCreatorNavigations)
                    .HasForeignKey(d => d.Creator);

                entity.HasOne(d => d.Image)
                    .WithMany(p => p.Paths)
                    .HasForeignKey(d => d.ImageId)
                    .HasConstraintName("FK_Path_Image");

                entity.HasOne(d => d.ModifierNavigation)
                    .WithMany(p => p.PathModifierNavigations)
                    .HasForeignKey(d => d.Modifier);

                entity.HasOne(d => d.Route)
                    .WithMany(p => p.Paths)
                    .HasForeignKey(d => d.RouteId)
                    .HasConstraintName("FK_Path_Route");
            });

            modelBuilder.Entity<Person>(entity =>
            {
                entity.ToTable("Person");

                entity.HasIndex(e => e.Number, "AK_Person")
                    .IsUnique();

                entity.Property(e => e.CreationDate).HasColumnType("datetime");

                entity.Property(e => e.ModificationDate).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.NameAr)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.Number)
                    .IsRequired()
                    .HasMaxLength(5);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(300);

                entity.Property(e => e.Salt)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.CreatorNavigation)
                    .WithMany(p => p.InverseCreatorNavigation)
                    .HasForeignKey(d => d.Creator);

                entity.HasOne(d => d.ModifierNavigation)
                    .WithMany(p => p.InverseModifierNavigation)
                    .HasForeignKey(d => d.Modifier);
            });

            modelBuilder.Entity<RefreshToken>(entity =>
            {
                entity.HasKey(e => new { e.Id, e.Token });

                entity.ToTable("RefreshToken");

                entity.Property(e => e.Token).HasMaxLength(100);

                entity.Property(e => e.Created).HasColumnType("datetime");

                entity.Property(e => e.CreatedByIp)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Expires).HasColumnType("datetime");

                entity.Property(e => e.ReasonRevoked).HasMaxLength(100);

                entity.Property(e => e.ReplacedByToken).HasMaxLength(100);

                entity.Property(e => e.Revoked).HasColumnType("datetime");

                entity.Property(e => e.RevokedByIp).HasMaxLength(100);

                entity.HasOne(d => d.IdNavigation)
                    .WithMany(p => p.RefreshTokens)
                    .HasForeignKey(d => d.Id)
                    .HasConstraintName("FK_RefreshToken_Person");
            });

            modelBuilder.Entity<Route>(entity =>
            {
                entity.ToTable("Route");

                entity.HasIndex(e => e.Id, "AK_Route_Name")
                    .IsUnique();

                entity.HasIndex(e => e.Id, "AK_Route_NameAr")
                    .IsUnique();

                entity.Property(e => e.CreationDate).HasColumnType("datetime");

                entity.Property(e => e.ModificationDate).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.NameAr)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.HasOne(d => d.CreatorNavigation)
                    .WithMany(p => p.RouteCreatorNavigations)
                    .HasForeignKey(d => d.Creator)
                    .HasConstraintName("FK_Route_Perons_Creator");

                entity.HasOne(d => d.ModifierNavigation)
                    .WithMany(p => p.RouteModifierNavigations)
                    .HasForeignKey(d => d.Modifier)
                    .HasConstraintName("FK_Route_Perons_Modifier");
            });

            modelBuilder.Entity<Stop>(entity =>
            {
                entity.HasKey(e => new { e.RouteId, e.PathType, e.Sequence })
                    .HasName("PK_Stop_1");

                entity.ToTable("Stop");

                entity.Property(e => e.CreationDate).HasColumnType("datetime");

                entity.Property(e => e.Latitude).HasColumnType("decimal(8, 6)");

                entity.Property(e => e.Longitude).HasColumnType("decimal(8, 6)");

                entity.Property(e => e.ModificationDate).HasColumnType("datetime");

                entity.Property(e => e.Name).HasMaxLength(100);

                entity.Property(e => e.NameAr).HasMaxLength(100);

                entity.HasOne(d => d.CreatorNavigation)
                    .WithMany(p => p.StopCreatorNavigations)
                    .HasForeignKey(d => d.Creator)
                    .HasConstraintName("FK_Stop_Perons_Creator");

                entity.HasOne(d => d.Image)
                    .WithMany(p => p.Stops)
                    .HasForeignKey(d => d.ImageId)
                    .HasConstraintName("FK_Stop_Image");

                entity.HasOne(d => d.ModifierNavigation)
                    .WithMany(p => p.StopModifierNavigations)
                    .HasForeignKey(d => d.Modifier)
                    .HasConstraintName("FK_Stop_Perons_Modifier");

                entity.HasOne(d => d.Path)
                    .WithMany(p => p.Stops)
                    .HasForeignKey(d => new { d.RouteId, d.PathType })
                    .HasConstraintName("FK_Stop_Path");
            });

            modelBuilder.Entity<Vehicle>(entity =>
            {
                entity.ToTable("Vehicle");

                entity.Property(e => e.Color)
                    .IsRequired()
                    .HasMaxLength(6)
                    .IsFixedLength(true);

                entity.Property(e => e.CreationDate).HasColumnType("datetime");

                entity.Property(e => e.Manufacturer)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.ManufacturerAr)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Model)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.ModelAr)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.ModificationDate).HasColumnType("datetime");

                entity.Property(e => e.PlateNumber).HasMaxLength(10);

                entity.Property(e => e.SecondaryColor)
                    .HasMaxLength(6)
                    .IsFixedLength(true);

                entity.HasOne(d => d.CreatorNavigation)
                    .WithMany(p => p.VehicleCreatorNavigations)
                    .HasForeignKey(d => d.Creator);

                entity.HasOne(d => d.Image)
                    .WithMany(p => p.Vehicles)
                    .HasForeignKey(d => d.ImageId)
                    .HasConstraintName("FK_Vehicle_Image");

                entity.HasOne(d => d.ModifierNavigation)
                    .WithMany(p => p.VehicleModifierNavigations)
                    .HasForeignKey(d => d.Modifier);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
