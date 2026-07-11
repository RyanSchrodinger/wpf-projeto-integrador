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
        public DbSet<Pagamento> Pagamentos { get; set; }
        public DbSet<FormaPagamento> FormasPagamento { get; set; }
        public DbSet<CategoriaPagamento> CategoriasPagamento { get; set; }
        public DbSet<StatusPagamento> StatusPagamentos { get; set; }
        public DbSet<Pedido> Pedidos { get; set; }
        public DbSet<Servico> Servicos { get; set; }
        public DbSet<ServicoPedido> ServicosPedidos { get; set; }
        public DbSet<Avaliacao> Avaliacoes { get; set; }
        public DbSet<Equipamento> Equipamentos { get; set; }
        public DbSet<Locacao> Locacoes { get; set; }
        public DbSet<ItemLocacao> ItensLocacao { get; set; }

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
                    .HasMaxLength(20);

                entity.Property(c => c.Cidade)
                    .HasMaxLength(100);

                entity.Property(c => c.Bairro)
                    .HasMaxLength(100);

                entity.Property(c => c.Cep)
                    .HasMaxLength(9);

                entity.Property(c => c.Estado)
                    .HasMaxLength(2);
            });

            // Profissional
            modelBuilder.Entity<Profissional>(entity =>
            {
                entity.Property(p => p.Descricao)
                    .IsRequired()
                    .HasMaxLength(300);

                entity.Property(p => p.Especialidade)
                    .HasMaxLength(100);

                entity.Property(p => p.Endereco)
                    .HasMaxLength(200);

                entity.HasOne(p => p.Empresa)
                    .WithMany(e => e.Profissionais)
                    .HasForeignKey(p => p.EmpresaId)
                    .OnDelete(DeleteBehavior.NoAction);
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

                entity.HasIndex(e => e.Cnpj)
                    .IsUnique();

                entity.Property(e => e.NomeFantasia)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Responsavel)
                    .HasMaxLength(100);

                entity.Property(e => e.Endereco)
                    .HasMaxLength(200);
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


            modelBuilder.Entity<FormaPagamento>(entity =>
            {
                entity.Property(f => f.Nome)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasIndex(f => f.Nome).IsUnique();

                entity.HasData(
                    new FormaPagamento { Id = 1, Nome = "Pix" },
                    new FormaPagamento { Id = 2, Nome = "Cartão de Crédito" },
                    new FormaPagamento { Id = 3, Nome = "Cartão de Débito" },
                    new FormaPagamento { Id = 4, Nome = "Dinheiro" },
                    new FormaPagamento { Id = 5, Nome = "Boleto" },
                    new FormaPagamento { Id = 6, Nome = "Transferência Bancária" }
                );
            });

            modelBuilder.Entity<StatusPagamento>(entity =>
            {
                entity.Property(s => s.Nome)
                    .IsRequired()
                    .HasMaxLength(30);

                entity.HasIndex(s => s.Nome).IsUnique();

                entity.HasData(
                    new StatusPagamento { Id = 1, Nome = "Pendente" },
                    new StatusPagamento { Id = 2, Nome = "Pago" },
                    new StatusPagamento { Id = 3, Nome = "Vencido" },
                    new StatusPagamento { Id = 4, Nome = "Cancelado" }
                );
            });

            modelBuilder.Entity<CategoriaPagamento>(entity =>
            {
                entity.Property(c => c.Nome)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasIndex(c => c.Nome).IsUnique();

                entity.HasData(
                    new CategoriaPagamento { Id = 1, Nome = "Serviço" },
                    new CategoriaPagamento { Id = 2, Nome = "Locação" },
                    new CategoriaPagamento { Id = 3, Nome = "Venda" },
                    new CategoriaPagamento { Id = 4, Nome = "Multa" },
                    new CategoriaPagamento { Id = 5, Nome = "Outro" }
                );
            });

            modelBuilder.Entity<Pagamento>(entity =>
            {
                entity.Property(p => p.Valor)
                    .HasColumnType("decimal(18,2)");

                entity.Property(p => p.Observacoes)
                    .HasMaxLength(500);

                entity.HasOne(p => p.FormaPagamento)
                    .WithMany(f => f.Pagamentos)
                    .HasForeignKey(p => p.FormaPagamentoId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(p => p.StatusPagamento)
                    .WithMany(s => s.Pagamentos)
                    .HasForeignKey(p => p.StatusPagamentoId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(p => p.CategoriaPagamento)
                    .WithMany(c => c.Pagamentos)
                    .HasForeignKey(p => p.CategoriaPagamentoId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // =========================================================
            // PEDIDO
            // =========================================================

            modelBuilder.Entity<Pedido>(entity =>
            {
                entity.ToTable("Pedidos");

                entity.HasKey(p => p.IdPedido);

                entity.Property(p => p.DataPedido)
                    .HasDefaultValueSql("GETDATE()")
                    .IsRequired();

                entity.Property(p => p.Total)
                    .HasColumnType("decimal(18,2)")
                    .IsRequired();

                entity.Property(p => p.Status)
                    .HasConversion<string>()
                    .HasMaxLength(30)
                    .HasDefaultValue(StatusPedido.Pendente)
                    .IsRequired();

                entity.HasOne(p => p.Cliente)
                    .WithMany(c => c.Pedidos)
                    .HasForeignKey(p => p.ClienteId)
                    .OnDelete(DeleteBehavior.Restrict);
            });


            // =========================================================
            // SERVIÇO
            // =========================================================

            modelBuilder.Entity<Servico>(entity =>
            {
                entity.ToTable("Servicos", tabela =>
                {
                    tabela.HasCheckConstraint(
                        "CK_Servico_Prestador",
                        @"(
                [EmpresaId] IS NOT NULL
                AND [ProfissionalId] IS NULL
              )
              OR
              (
                [EmpresaId] IS NULL
                AND [ProfissionalId] IS NOT NULL
              )"
                    );
                });

                entity.HasKey(s => s.IdServico);

                entity.Property(s => s.Nome)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(s => s.Descricao)
                    .IsRequired()
                    .HasMaxLength(300);

                entity.Property(s => s.Preco)
                    .HasColumnType("decimal(18,2)")
                    .IsRequired();

                entity.Property(s => s.Ativo)
                    .HasDefaultValue(true);

                entity.HasOne(s => s.Empresa)
                    .WithMany(e => e.Servicos)
                    .HasForeignKey(s => s.EmpresaId)
                    .OnDelete(DeleteBehavior.NoAction);

                entity.HasOne(s => s.Profissional)
                    .WithMany(p => p.Servicos)
                    .HasForeignKey(s => s.ProfissionalId)
                    .OnDelete(DeleteBehavior.NoAction);
            });


            // =========================================================
            // SERVIÇO DO PEDIDO
            // =========================================================

            modelBuilder.Entity<ServicoPedido>(entity =>
            {
                entity.ToTable("ServicosPedidos");

                entity.HasKey(sp => sp.IdItem);

                entity.Property(sp => sp.ValorServico)
                    .HasColumnType("decimal(18,2)")
                    .IsRequired();

                entity.Property(sp => sp.Observacao)
                    .HasMaxLength(200)
                    .HasDefaultValue("-");

                entity.Property(sp => sp.Status)
                    .HasConversion<string>()
                    .HasMaxLength(30)
                    .HasDefaultValue(StatusServicoPedido.Pendente)
                    .IsRequired();

                entity.HasOne(sp => sp.Pedido)
                    .WithMany(p => p.ServicosPedidos)
                    .HasForeignKey(sp => sp.PedidoId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(sp => sp.Servico)
                    .WithMany(s => s.ServicosPedidos)
                    .HasForeignKey(sp => sp.ServicoId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(sp => sp.Profissional)
                    .WithMany(p => p.ServicosPedidos)
                    .HasForeignKey(sp => sp.ProfissionalId)
                    .OnDelete(DeleteBehavior.Restrict);
            });


            // =========================================================
            // AVALIAÇÃO
            // =========================================================

            modelBuilder.Entity<Avaliacao>(entity =>
            {
                entity.ToTable("Avaliacoes");

                entity.HasKey(a => a.Id);

                entity.Property(a => a.Nota)
                    .IsRequired();

                entity.Property(a => a.Comentario)
                    .HasMaxLength(300);

                entity.Property(a => a.DataAvaliacao)
                    .HasDefaultValueSql("GETDATE()");

                entity.ToTable("Avaliacoes", tabela =>
                {
                    tabela.HasCheckConstraint(
                        "CK_Avaliacao_Nota",
                        "[Nota] BETWEEN 1 AND 5"
                    );
                });

                entity.HasOne(a => a.Cliente)
                    .WithMany(c => c.Avaliacoes)
                    .HasForeignKey(a => a.ClienteId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(a => a.ServicoPedido)
                    .WithOne(sp => sp.Avaliacao)
                    .HasForeignKey<Avaliacao>(a => a.ServicoPedidoId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Equipamento>(entity =>
            {
                entity.HasKey(e => e.IdEquipamento);

                entity.Property(e => e.Nome)
                    .HasMaxLength(100)
                    .IsRequired();

                entity.Property(e => e.Descricao)
                    .HasMaxLength(500);

                entity.Property(e => e.Valor)
                    .HasColumnType("decimal(10,2)")
                    .IsRequired();

                entity.Property(e => e.QuantidadeTotal)
                    .IsRequired();

                entity.Property(e => e.QuantidadeDisponivel)
                    .IsRequired();

                entity.Property(e => e.Ativo)
                    .HasDefaultValue(true)
                    .IsRequired();

                entity.HasOne(e => e.Empresa)
                    .WithMany(empresa => empresa.Equipamentos)
                    .HasForeignKey(e => e.EmpresaId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Locacao>(entity =>
            {
                entity.HasKey(l => l.IdLocacao);

                entity.Property(l => l.DataLocacao)
                    .IsRequired();

                entity.Property(l => l.Status)
                    .IsRequired();

                entity.Property(l => l.ValorTotal)
                    .HasColumnType("decimal(10,2)")
                    .IsRequired();

                entity.Property(l => l.Observacao)
                    .HasMaxLength(500);

                entity.HasOne(l => l.Cliente)
                    .WithMany(cliente => cliente.Locacoes)
                    .HasForeignKey(l => l.ClienteId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<ItemLocacao>(entity =>
            {
                entity.HasKey(i => i.IdItemLocacao);

                entity.Property(i => i.Quantidade)
                    .IsRequired();

                entity.Property(i => i.Valor)
                    .HasColumnType("decimal(10,2)")
                    .IsRequired();

                entity.Property(i => i.ValorTotal)
                    .HasColumnType("decimal(10,2)")
                    .IsRequired();

                entity.HasOne(i => i.Locacao)
                    .WithMany(locacao => locacao.ItensLocacao)
                    .HasForeignKey(i => i.LocacaoId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(i => i.Equipamento)
                    .WithMany(equipamento => equipamento.ItensLocacao)
                    .HasForeignKey(i => i.EquipamentoId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }


    }


    
}
