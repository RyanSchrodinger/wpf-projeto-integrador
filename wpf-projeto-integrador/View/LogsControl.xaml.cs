using MahApps.Metro.IconPacks;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Media;
using wpf_projeto_integrador.Data;
using wpf_projeto_integrador.Models;

namespace wpf_projeto_integrador.View
{
    public partial class LogsControl : UserControl
    {
        private List<LogItemViewModel> _todosLogs = new();

        public LogsControl()
        {
            InitializeComponent();
            CarregarLogs();
        }

        public void CarregarLogs()
        {
            using var db = new MusicStationContext();

            var logs = db.LogsSistema
                .Include(l => l.Usuario)
                .OrderByDescending(l => l.DataHora)
                .ToList();

            _todosLogs = logs.Select(l => new LogItemViewModel
            {
                Id = l.Id,
                TipoAcao = l.TipoAcao.ToString(),
                Descricao = l.Descricao,
                EntidadeAfetada = l.EntidadeAfetada ?? "Sistema",
                EntidadeId = l.EntidadeId,
                UsuarioNome = l.Usuario?.NomeUsuario ?? "Sistema",
                NomeComputador = l.NomeComputador ?? "-",
                DataHora = l.DataHora,
                Sucesso = l.Sucesso,
                Erro = l.Erro,
                Icone = ObterIcone(l.TipoAcao),
                CorTexto = ObterCorTexto(l.TipoAcao, l.Sucesso),
                CorFundo = ObterCorFundo(l.TipoAcao, l.Sucesso)
            }).ToList();

            cmbAcao.ItemsSource = new[] { "Todas ações" }
                .Concat(Enum.GetNames(typeof(TipoAcaoLog)))
                .ToList();

            cmbAcao.SelectedIndex = 0;

            cmbEntidade.ItemsSource = new[] { "Todas entidades" }
                .Concat(_todosLogs.Select(l => l.EntidadeAfetada).Distinct())
                .ToList();

            cmbEntidade.SelectedIndex = 0;

            AtualizarTela(_todosLogs);
        }

        private void BtnFiltrar_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            AplicarFiltros();
        }

        private void BtnLimpar_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            txtPesquisa.Clear();
            cmbAcao.SelectedIndex = 0;
            cmbEntidade.SelectedIndex = 0;
            AtualizarTela(_todosLogs);
        }

        private void AplicarFiltros()
        {
            string pesquisa = txtPesquisa.Text.Trim();
            string acao = cmbAcao.SelectedItem?.ToString() ?? "Todas ações";
            string entidade = cmbEntidade.SelectedItem?.ToString() ?? "Todas entidades";

            var filtrados = _todosLogs.Where(l =>
                (string.IsNullOrWhiteSpace(pesquisa) ||
                 l.Descricao.Contains(pesquisa, StringComparison.OrdinalIgnoreCase) ||
                 l.UsuarioNome.Contains(pesquisa, StringComparison.OrdinalIgnoreCase) ||
                 l.TipoAcao.Contains(pesquisa, StringComparison.OrdinalIgnoreCase) ||
                 l.EntidadeAfetada.Contains(pesquisa, StringComparison.OrdinalIgnoreCase)) &&

                (acao == "Todas ações" || l.TipoAcao == acao) &&

                (entidade == "Todas entidades" || l.EntidadeAfetada == entidade)
            ).ToList();

            AtualizarTela(filtrados);
        }

        private void AtualizarTela(List<LogItemViewModel> logs)
        {
            DataContext = new
            {
                Logs = logs,
                TotalLogs = logs.Count,
                TotalSucesso = logs.Count(l => l.Sucesso),
                TotalFalha = logs.Count(l => !l.Sucesso)
            };
        }

        private PackIconMaterialKind ObterIcone(TipoAcaoLog tipo)
        {
            return tipo switch
            {
                TipoAcaoLog.LoginSucesso => PackIconMaterialKind.LoginVariant,
                TipoAcaoLog.LoginFalha => PackIconMaterialKind.AlertCircleOutline,
                TipoAcaoLog.Logout => PackIconMaterialKind.LogoutVariant,

                TipoAcaoLog.Cadastro => PackIconMaterialKind.PlusCircleOutline,
                TipoAcaoLog.Atualizacao => PackIconMaterialKind.PencilOutline,
                TipoAcaoLog.Exclusao => PackIconMaterialKind.DeleteOutline,

                TipoAcaoLog.Desativacao => PackIconMaterialKind.AccountOffOutline,
                TipoAcaoLog.Reativacao => PackIconMaterialKind.AccountCheckOutline,

                TipoAcaoLog.AlteracaoSenha => PackIconMaterialKind.LockReset,
                TipoAcaoLog.RedefinicaoSenha => PackIconMaterialKind.LockAlertOutline,

                TipoAcaoLog.Visualizacao => PackIconMaterialKind.EyeOutline,
                TipoAcaoLog.Busca => PackIconMaterialKind.Magnify,
                TipoAcaoLog.Exportacao => PackIconMaterialKind.FileExportOutline,

                TipoAcaoLog.EnvioMensagem => PackIconMaterialKind.SendOutline,
                TipoAcaoLog.LeituraMensagem => PackIconMaterialKind.EmailOpenOutline,

                TipoAcaoLog.LocacaoCriada => PackIconMaterialKind.MusicNotePlus,
                TipoAcaoLog.LocacaoCancelada => PackIconMaterialKind.Cancel,
                TipoAcaoLog.LocacaoFinalizada => PackIconMaterialKind.CheckCircleOutline,

                TipoAcaoLog.PagamentoRegistrado => PackIconMaterialKind.CashCheck,

                TipoAcaoLog.ErroSistema => PackIconMaterialKind.BugOutline,
                TipoAcaoLog.AcessoNegado => PackIconMaterialKind.ShieldAlertOutline,

                _ => PackIconMaterialKind.InformationOutline
            };
        }

        private Brush ObterCorTexto(TipoAcaoLog tipo, bool sucesso)
        {
            if (!sucesso) return new SolidColorBrush(Color.FromRgb(248, 113, 113));

            return tipo switch
            {
                TipoAcaoLog.LoginSucesso => new SolidColorBrush(Color.FromRgb(74, 222, 128)),
                TipoAcaoLog.Cadastro => new SolidColorBrush(Color.FromRgb(34, 211, 238)),
                TipoAcaoLog.Atualizacao => new SolidColorBrush(Color.FromRgb(250, 204, 21)),
                TipoAcaoLog.Exclusao => new SolidColorBrush(Color.FromRgb(248, 113, 113)),
                TipoAcaoLog.PagamentoRegistrado => new SolidColorBrush(Color.FromRgb(52, 211, 153)),
                TipoAcaoLog.ErroSistema => new SolidColorBrush(Color.FromRgb(251, 146, 60)),
                TipoAcaoLog.AcessoNegado => new SolidColorBrush(Color.FromRgb(248, 113, 113)),
                _ => new SolidColorBrush(Color.FromRgb(168, 85, 247))
            };
        }

        private Brush ObterCorFundo(TipoAcaoLog tipo, bool sucesso)
        {
            if (!sucesso) return new SolidColorBrush(Color.FromRgb(69, 26, 26));

            return tipo switch
            {
                TipoAcaoLog.LoginSucesso => new SolidColorBrush(Color.FromRgb(31, 59, 44)),
                TipoAcaoLog.Cadastro => new SolidColorBrush(Color.FromRgb(22, 52, 64)),
                TipoAcaoLog.Atualizacao => new SolidColorBrush(Color.FromRgb(66, 53, 24)),
                TipoAcaoLog.Exclusao => new SolidColorBrush(Color.FromRgb(69, 26, 26)),
                TipoAcaoLog.PagamentoRegistrado => new SolidColorBrush(Color.FromRgb(21, 61, 49)),
                TipoAcaoLog.ErroSistema => new SolidColorBrush(Color.FromRgb(67, 40, 24)),
                TipoAcaoLog.AcessoNegado => new SolidColorBrush(Color.FromRgb(69, 26, 26)),
                _ => new SolidColorBrush(Color.FromRgb(45, 31, 79))
            };
        }
    }

    public class LogItemViewModel
    {
        public int Id { get; set; }
        public string TipoAcao { get; set; } = "";
        public string Descricao { get; set; } = "";
        public string EntidadeAfetada { get; set; } = "";
        public int? EntidadeId { get; set; }
        public string UsuarioNome { get; set; } = "";
        public string NomeComputador { get; set; } = "";
        public DateTime DataHora { get; set; }
        public bool Sucesso { get; set; }
        public string? Erro { get; set; }
        public PackIconMaterialKind Icone { get; set; }
        public Brush CorTexto { get; set; } = Brushes.White;
        public Brush CorFundo { get; set; } = Brushes.Transparent;
    }
}