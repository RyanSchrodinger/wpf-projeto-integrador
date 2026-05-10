using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wpf_projeto_integrador.Models;

namespace wpf_projeto_integrador.Data
{
    public class MusicStationContext : DbContext 
    {
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Administrador> Administradores { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(
                "Server=Ryan\\SQLEXPRESS;Database=MusicStation;Trusted_Connection=True;TrustServerCertificate=True;"
            );
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Usuario>().ToTable("Usuarios");
            modelBuilder.Entity<Administrador>().ToTable("Administradores");

            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Usuario>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<Usuario>()
                .HasIndex(u => u.NomeUsuario)
                .IsUnique();
            modelBuilder.Entity<Usuario>()
                .Property(u => u.DataCriacao)
                .HasDefaultValueSql("GETDATE()");

            modelBuilder.Entity<Administrador>()
                .Property(a => a.NivelAcesso)
                .HasConversion<string>();

            modelBuilder.Entity<Administrador>()
                .ToTable(t => t.HasCheckConstraint(
                    "CK_Administrador_NivelAcesso",
                    "NivelAcesso IN ('Baixo','Medio','Alto')"
    ));

        }


    }
}
