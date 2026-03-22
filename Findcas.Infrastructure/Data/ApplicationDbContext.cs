using Findcas.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Findcas.Infrastructure.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // Aquí registras la tabla de Fincas
        public DbSet<Property> Properties { get; set; }
        public DbSet<PropertyImage> PropertyImages { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<Amenity> Amenities { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<State> States { get; set; }
        public DbSet<City> Cities { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Property>()
                .Property(p => p.PricePerNight)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Property>()
                .Property(p => p.Name)
                .HasMaxLength(150)
                .IsRequired();


            modelBuilder.Entity<Amenity>().HasData(
                new Amenity { Id = 1, Name = "Wifi", Icon = Icons.Material.Filled.Wifi },
                new Amenity { Id = 2, Name = "Piscina", Icon = Icons.Material.Filled.Pool },
                new Amenity { Id = 3, Name = "Zona de trabajo", Icon = Icons.Material.Filled.WorkOutline },
                new Amenity { Id = 4, Name = "Aire acondicionado", Icon = Icons.Material.Filled.AcUnit },
                new Amenity { Id = 5, Name = "Cocina equipada", Icon = Icons.Material.Filled.Kitchen },
                new Amenity { Id = 6, Name = "Estacionamiento gratuito", Icon = Icons.Material.Filled.LocalParking },
                new Amenity { Id = 7, Name = "Televisión", Icon = Icons.Material.Filled.Tv },
                new Amenity { Id = 8, Name = "Se permiten mascotas", Icon = Icons.Material.Filled.Pets },
                new Amenity { Id = 9, Name = "Lavadora", Icon = Icons.Material.Filled.LocalLaundryService },
                new Amenity { Id = 10, Name = "Zona de BBQ", Icon = Icons.Material.Filled.OutdoorGrill }
            );
        }
    }
}

