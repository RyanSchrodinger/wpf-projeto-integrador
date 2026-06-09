using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using wpf_projeto_integrador.Models;

namespace wpf_projeto_integrador.Data
{
    public class MusicStationContext : DbContext 
    {
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Administrador> Administradores { get; set; }
        public DbSet<LogSistema> LogsSistema { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Empresa> Empresas { get; set; }
        public DbSet<Profissional> Profissionais { get; set; }
        public DbSet<Chat> Chats { get; set; }
        public DbSet<Mensagem> Mensagens { get; set; }


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
                    "NivelAcesso IN ('AdministradorGeral','Atendente','Suporte','Financeiro','Moderador')"
                 ));

            // Cliente
            modelBuilder.Entity<Cliente>(entity =>
            {

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

            //Log
            modelBuilder.Entity<LogSistema>(entity =>
            {
                entity.HasKey(l => l.Id);

                entity.Property(l => l.Descricao)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(l => l.EntidadeAfetada)
                    .HasMaxLength(100);

                entity.Property(l => l.Tela)
                    .HasMaxLength(100);

                entity.Property(l => l.NomeComputador)
                    .HasMaxLength(100);

                entity.Property(l => l.Erro)
                    .HasMaxLength(1000);

                entity.Property(l => l.DataHora)
                    .HasDefaultValueSql("GETDATE()");

                // Relacionamento com Usuario
                entity.HasOne(l => l.Usuario)
                    .WithMany(u => u.Logs)
                    .HasForeignKey(l => l.UsuarioId)
                    .OnDelete(DeleteBehavior.SetNull);

                // Salvar enum como int
                entity.Property(l => l.TipoAcao)
                    .HasConversion<int>();
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
