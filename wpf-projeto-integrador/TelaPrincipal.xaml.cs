using MahApps.Metro.IconPacks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;
using wpf_pi.Views;
using wpf_projeto_integrador.Data;
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


        }

        public void AbrirTela(UserControl tela)
        {
            MainContent.Content = tela;
        }


        public void VerificarPermissoes()
        {
            if (_administrador.NivelAcesso != Administrador.NivelAcessoEnum.AdministradorGeral)
            {
                btnLogs.Visibility = Visibility.Collapsed;
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
    }
        
}