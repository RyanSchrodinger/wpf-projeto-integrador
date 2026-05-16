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

        public DbSet<LogSistema> LogsSistema { get; set; }
        public DbSet<TipoAcao> TiposAcao { get; set; }


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


            modelBuilder.Entity<TipoAcao>()
               .HasIndex(t => t.Nome)
               .IsUnique();

            modelBuilder.Entity<TipoAcao>()
                .Property(t => t.Nome)
                .HasMaxLength(50)
                .IsRequired();

            modelBuilder.Entity<LogSistema>()
                .Property(l => l.NomeComputador)
                .HasMaxLength(100);

            modelBuilder.Entity<LogSistema>()
                .Property(l => l.Descricao)
                .HasMaxLength(500)
                .IsRequired();

            modelBuilder.Entity<LogSistema>()
                .Property(l => l.Entidade)
                .HasMaxLength(100);


            modelBuilder.Entity<LogSistema>()
                .Property(l => l.DataHora)
                .HasDefaultValueSql("GETDATE()");

            modelBuilder.Entity<LogSistema>()
                .HasOne(l => l.Usuario)
                .WithMany(u => u.Logs)
                .HasForeignKey(l => l.UsuarioId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<LogSistema>()
                .HasOne(l => l.TipoAcao)
                .WithMany(t => t.Logs)
                .HasForeignKey(l => l.TipoAcaoId)
                .OnDelete(DeleteBehavior.Restrict);
        }


    }
}
