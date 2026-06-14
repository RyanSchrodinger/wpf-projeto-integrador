using MahApps.Metro.IconPacks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;
using wpf_pi.Views;
using wpf_projeto_integrador.Data;
using wpf_projeto_integrador.Helpers;
using wpf_projeto_integrador.Models;
using wpf_projeto_integrador.View;
using wpf_projeto_integrador.View.Users;
using wpf_projeto_integrador.Views;
using static wpf_projeto_integrador.Models.Administrador;

namespace wpf_projeto_integrador
{
    public partial class FormMenu : Window
    {
        private DispatcherTimer _timerVerificacao;
        private Administrador _administrador;

        public int IdUsuarioLogado;
        public FormMenu(Administrador administrador)
        {
            InitializeComponent();
            _administrador = administrador;
            IdUsuarioLogado = _administrador.Id;
            VerificarPermissoes();
            IniciarVerificacaoUsuario();
            CarregarContaLogada();


        }

        public void AbrirTela(UserControl tela)
        {
            MainContent.Content = tela;
        }


        public void VerificarPermissoes()
        {
            // Tudo oculto inicialmente
            BtnDashboard.Visibility = Visibility.Collapsed;

            BtnGestaoPessoas.Visibility = Visibility.Collapsed;
            BtnUsuarios.Visibility = Visibility.Collapsed;
            BtnAdministradores.Visibility = Visibility.Collapsed;
            BtnProfissionais.Visibility = Visibility.Collapsed;
            BtnClientes.Visibility = Visibility.Collapsed;
            BtnEmpresas.Visibility = Visibility.Collapsed;
            BtnProfissionalCargo.Visibility = Visibility.Collapsed;

            BtnGestaoFinanceira.Visibility = Visibility.Collapsed;
            BtnPagamentos.Visibility = Visibility.Collapsed;
            BtnTransacoes.Visibility = Visibility.Collapsed;
            BtnFormaPagamento.Visibility = Visibility.Collapsed;

            BtnGestaoServicos.Visibility = Visibility.Collapsed;
            BtnServicos.Visibility = Visibility.Collapsed;
            BtnPedidos.Visibility = Visibility.Collapsed;
            BtnServicosPedidos.Visibility = Visibility.Collapsed;
            BtnAvaliacoes.Visibility = Visibility.Collapsed;

            BtnGestaoLocacoes.Visibility = Visibility.Collapsed;
            BtnLocacoes.Visibility = Visibility.Collapsed;
            BtnLocacoesItens.Visibility = Visibility.Collapsed;
            BtnInstrumentos.Visibility = Visibility.Collapsed;

            BtnComunicacao.Visibility = Visibility.Collapsed;
            BtnConfiguracao.Visibility = Visibility.Collapsed;
            BtnLogs.Visibility = Visibility.Collapsed;


            switch (_administrador.NivelAcesso)
            {
                case NivelAcessoEnum.AdministradorGeral:

                    BtnDashboard.Visibility = Visibility.Visible;

                    BtnGestaoPessoas.Visibility = Visibility.Visible;
                    BtnUsuarios.Visibility = Visibility.Visible;
                    BtnAdministradores.Visibility = Visibility.Visible;
                    BtnProfissionais.Visibility = Visibility.Visible;
                    BtnClientes.Visibility = Visibility.Visible;
                    BtnEmpresas.Visibility = Visibility.Visible;
                    BtnProfissionalCargo.Visibility = Visibility.Visible;

                    BtnGestaoFinanceira.Visibility = Visibility.Visible;
                    BtnPagamentos.Visibility = Visibility.Visible;
                    BtnTransacoes.Visibility = Visibility.Visible;
                    BtnFormaPagamento.Visibility = Visibility.Visible;

                    BtnGestaoServicos.Visibility = Visibility.Visible;
                    BtnServicos.Visibility = Visibility.Visible;
                    BtnPedidos.Visibility = Visibility.Visible;
                    BtnServicosPedidos.Visibility = Visibility.Visible;
                    BtnAvaliacoes.Visibility = Visibility.Visible;

                    BtnGestaoLocacoes.Visibility = Visibility.Visible;
                    BtnLocacoes.Visibility = Visibility.Visible;
                    BtnLocacoesItens.Visibility = Visibility.Visible;
                    BtnInstrumentos.Visibility = Visibility.Visible;

                    BtnComunicacao.Visibility = Visibility.Visible;
                    BtnConfiguracao.Visibility = Visibility.Visible;
                    BtnLogs.Visibility = Visibility.Visible;

                    break;


                case NivelAcessoEnum.Atendente:

                    BtnDashboard.Visibility = Visibility.Visible;

                    BtnGestaoPessoas.Visibility = Visibility.Visible;
                    BtnClientes.Visibility = Visibility.Visible;

                    BtnGestaoLocacoes.Visibility = Visibility.Visible;
                    BtnLocacoes.Visibility = Visibility.Visible;
                    BtnLocacoesItens.Visibility = Visibility.Visible;
                    BtnInstrumentos.Visibility = Visibility.Visible;

                    BtnComunicacao.Visibility = Visibility.Visible;

                    break;


                case NivelAcessoEnum.Financeiro:

                    BtnDashboard.Visibility = Visibility.Visible;

                    BtnGestaoFinanceira.Visibility = Visibility.Visible;
                    BtnPagamentos.Visibility = Visibility.Visible;
                    BtnTransacoes.Visibility = Visibility.Visible;
                    BtnFormaPagamento.Visibility = Visibility.Visible;

                    BtnComunicacao.Visibility = Visibility.Visible;

                    break;


                case NivelAcessoEnum.Suporte:

                    BtnDashboard.Visibility = Visibility.Visible;

                    BtnGestaoServicos.Visibility = Visibility.Visible;
                    BtnServicos.Visibility = Visibility.Visible;
                    BtnPedidos.Visibility = Visibility.Visible;
                    BtnServicosPedidos.Visibility = Visibility.Visible;

                    BtnComunicacao.Visibility = Visibility.Visible;

                    break;


                case NivelAcessoEnum.Moderador:

                    BtnDashboard.Visibility = Visibility.Visible;

                    BtnGestaoPessoas.Visibility = Visibility.Visible;
                    BtnUsuarios.Visibility = Visibility.Visible;
                    BtnClientes.Visibility = Visibility.Visible;

                    BtnLogs.Visibility = Visibility.Visible;

                    BtnComunicacao.Visibility = Visibility.Visible;

                    break;
            }
        }




        private void btnLogs_Click(object sender, RoutedEventArgs e)
        {
            if (_administrador.NivelAcesso != Administrador.NivelAcessoEnum.AdministradorGeral)
            {
                MessageBox.Show("Você não tem permissão.");
                return;
            }
            MainContent.Content = new LogsControl();
        }

        private void BtnGestaoPessoas_Click(object sender, RoutedEventArgs e)
        {
            bool aberto = SubMenuPessoas.Visibility == Visibility.Visible;

            SubMenuPessoas.Visibility =
                aberto ? Visibility.Collapsed : Visibility.Visible;

            SetaPessoas.Kind =
                aberto
                ? PackIconMaterialKind.ChevronDown
                : PackIconMaterialKind.ChevronUp;
        }

        private void BtnGestaoFinanceira_Click(object sender, RoutedEventArgs e)
        {
            bool aberto = SubMenuFinancas.Visibility == Visibility.Visible;

            SubMenuFinancas.Visibility =
                aberto ? Visibility.Collapsed : Visibility.Visible;

            SetaFinancas.Kind =
                aberto
                ? PackIconMaterialKind.ChevronDown
                : PackIconMaterialKind.ChevronUp;
        }


        private void BtnGestaoServicos_Click(object sender, RoutedEventArgs e)
        {
            bool aberto = SubMenuServicos.Visibility == Visibility.Visible;

            SubMenuServicos.Visibility =
                aberto ? Visibility.Collapsed : Visibility.Visible;

            SetaServicos.Kind =
                aberto
                ? PackIconMaterialKind.ChevronDown
                : PackIconMaterialKind.ChevronUp;

        }

        private void BtnGestaoLocacoes_Click(object sender, RoutedEventArgs e)
        {
            bool aberto = SubMenuLocacoes.Visibility == Visibility.Visible;

            SubMenuLocacoes.Visibility =
                aberto ? Visibility.Collapsed : Visibility.Visible;

            SetaLocacoes.Kind =
                aberto
                ? PackIconMaterialKind.ChevronDown
                : PackIconMaterialKind.ChevronUp;

        }






        private void IniciarVerificacaoUsuario()
        {
            _timerVerificacao = new DispatcherTimer();
            _timerVerificacao.Interval = TimeSpan.FromSeconds(10);
            _timerVerificacao.Tick += TimerVerificacao_Tick;
            _timerVerificacao.Start();
        }



        private void TimerVerificacao_Tick(object sender, EventArgs e)
        {
            if (!UsuarioAindaEstaAtivo())
            {
                _timerVerificacao.Stop();

                MessageBox.Show(
                    "Sua conta foi desativada. Você será desconectado.",
                    "Acesso encerrado",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);

                MainWindow login = new MainWindow();
                login.Show();

                this.Close();
            }
        }

        private bool UsuarioAindaEstaAtivo()
        {
            using var db = new MusicStationContext();

            return db.Administradores.Any(a =>
                a.Id == IdUsuarioLogado &&
                a.Ativo);
        }


        private void CarregarContaLogada()
        {
            var usuario = SessaoUsuario.usuarioLogado ?? _administrador;

            if (usuario == null)
                return;

            txtNomeUsuarioLogado.Text = usuario.Nome;
            txtCargoUsuarioLogado.Text = usuario.NivelAcesso.ToString();
            txtIniciaisUsuario.Text = GerarIniciais(usuario.Nome);
        }

        private string GerarIniciais(string nome)
        {
            if (string.IsNullOrWhiteSpace(nome))
                return "?";

            var partes = nome.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);

            if (partes.Length == 1)
                return partes[0][0].ToString().ToUpper();

            return $"{partes[0][0]}{partes[^1][0]}".ToUpper();
        }







        private void BtnUsuarios_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new UsuariosView();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new ComunicacaoView(IdUsuarioLogado);
        }

        private void BtnAdministradores_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new AdministradorView();
        }


        private void BtnProfissional_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new ProfissionalView();
        }
    }
        
}