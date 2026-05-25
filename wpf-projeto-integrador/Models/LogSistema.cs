using System;
using System.ComponentModel.DataAnnotations;

namespace wpf_projeto_integrador.Models
{
    public class LogSistema
    {
        public int Id { get; set; }

        // Usuário que realizou a ação
        public int? UsuarioId { get; set; }
        public Usuario? Usuario { get; set; }

        // Tipo da ação realizada
        [Required]
        public TipoAcaoLog TipoAcao { get; set; }

        // Descrição detalhada do log
        [Required]
        [MaxLength(500)]
        public string Descricao { get; set; } = string.Empty;

        // Nome da entidade afetada
        [MaxLength(100)]
        public string? EntidadeAfetada { get; set; }

        // Id da entidade afetada
        public int? EntidadeId { get; set; }

        // Tela onde aconteceu
        [MaxLength(100)]
        public string? Tela { get; set; }

        // Nome do computador
        [MaxLength(100)]
        public string? NomeComputador { get; set; }

        // Se a ação deu certo
        public bool Sucesso { get; set; } = true;

        // Mensagem de erro
        [MaxLength(1000)]
        public string? Erro { get; set; }

        // Data e hora do acontecimento
        public DateTime DataHora { get; set; } = DateTime.Now;
    }

    public enum TipoAcaoLog
    {
        LoginSucesso,
        LoginFalha,
        Logout,

        Cadastro,
        Atualizacao,
        Exclusao,
        Desativacao,
        Reativacao,

        AlteracaoSenha,
        RedefinicaoSenha,

        Visualizacao,
        Busca,
        Exportacao,

        EnvioMensagem,
        LeituraMensagem,

        LocacaoCriada,
        LocacaoCancelada,
        LocacaoFinalizada,

        PagamentoRegistrado,

        ErroSistema,
        AcessoNegado
    }
}