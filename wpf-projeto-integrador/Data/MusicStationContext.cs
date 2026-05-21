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
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Empresa> Empresas { get; set; }
        public DbSet<Profissional> Profissionais { get; set; }
        public DbSet<Chat> Chats { get; set; }
        public DbSet<Mensagem> Mensagens { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(
                "Server=tcp:musicstation-db1.database.windows.net,1433;Initial Catalog=banco-central;Persist Security Info=False;User ID=ryan;Password=W@choswick01;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Usuario>().ToTable("Usuarios");
            modelBuilder.Entity<Administrador>().ToTable("Administradores");
            modelBuilder.Entity<Cliente>().ToTable("Clientes");
            modelBuilder.Entity<Profissional>().ToTable("Profissionais");
            modelBuilder.Entity<Empresa>().ToTable("Empresas");

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
            modelBuilder.Entity<Usuario>()
                .Property(u => u.Ativo)
                .HasDefaultValue(true);
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


            // Cliente
            modelBuilder.Entity<Cliente>(entity =>
            {
                entity.Property(c => c.Telefone)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(c => c.Rua)
                    .HasMaxLength(100);

                entity.Property(c => c.Numero)
                    .HasMaxLength(10);

                entity.Property(c => c.Cidade)
                    .HasMaxLength(100);
            });

            // Profissional
            modelBuilder.Entity<Profissional>(entity =>
            {
                entity.Property(p => p.Telefone)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(p => p.Descricao)
                    .IsRequired()
                    .HasMaxLength(300);

                entity.Property(p => p.Rua)
                    .HasMaxLength(100);

                entity.Property(p => p.Numero)
                    .HasMaxLength(10);

                entity.Property(p => p.Cidade)
                    .HasMaxLength(100);
            });


            // Empresa
            modelBuilder.Entity<Empresa>(entity =>
            {
                entity.Property(e => e.Cnpj)
                    .IsRequired()
                    .HasMaxLength(18);

                entity.Property(e => e.NomeFantasia)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.Telefone)
                    .HasMaxLength(20);

                entity.Property(e => e.Endereco)
                    .HasMaxLength(200);

                entity.HasIndex(e => e.Cnpj)
                    .IsUnique();
            });


            // CHAT
            modelBuilder.Entity<Chat>()
                .HasOne(c => c.Usuario1)
                .WithMany()
                .HasForeignKey(c => c.Usuario1Id)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Chat>()
                .HasOne(c => c.Usuario2)
                .WithMany()
                .HasForeignKey(c => c.Usuario2Id)
                .OnDelete(DeleteBehavior.Restrict);

            // MENSAGEM
            modelBuilder.Entity<Mensagem>()
                .HasOne(m => m.Chat)
                .WithMany(c => c.Mensagens)
                .HasForeignKey(m => m.ChatId);

            modelBuilder.Entity<Mensagem>()
                .HasOne(m => m.Remetente)
                .WithMany()
                .HasForeignKey(m => m.RemetenteId)
                .OnDelete(DeleteBehavior.Restrict);

            // TAMANHO TEXTO
            modelBuilder.Entity<Mensagem>()
                .Property(m => m.Texto)
                .HasMaxLength(1000);
        }
    }


    
}
