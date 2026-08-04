using MahApps.Metro.IconPacks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using wpf_pi.Views;
using wpf_projeto_integrador.Data;
using wpf_projeto_integrador.Helpers;
using wpf_projeto_integrador.Models;
using wpf_projeto_integrador.View;
using wpf_projeto_integrador.View.Demais;
using wpf_projeto_integrador.View.Financeiro;
using wpf_projeto_integrador.View.GestaoServicos;
using wpf_projeto_integrador.View.Users;
using wpf_projeto_integrador.View.Users.View;
using wpf_projeto_integrador.Views;
using static wpf_projeto_integrador.Models.Administrador;

namespace wpf_projeto_integrador
{
    public partial class FormMenu : Window
    {
        private DispatcherTimer _timerVerificacao;
        private Administrador _administrador;

        public int IdUsuarioLogado;
        private bool telaCheia = false;

        public FormMenu(Administrador administrador)    
        {
            InitializeComponent();

            _administrador = administrador;
            IdUsuarioLogado = _administrador.Id;

            VerificarPermissoes();
            IniciarVerificacaoUsuario();
            CarregarContaLogada();

            WindowStyle = WindowStyle.None;
            WindowState = WindowState.Maximized;

            //MainContent.Content = new DashBoardView();
            //SelecionarMenu(BtnDashboard);
            AbrirFinanceiro();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F11)
                AlternarTelaCheia();
        }

        private void AlternarTelaCheia()
        {
            if (!telaCheia)
            {
                WindowStyle = WindowStyle.None;
                WindowState = WindowState.Maximized;
                telaCheia = true;
            }
            else
            {
                WindowStyle = WindowStyle.SingleBorderWindow;
                WindowState = WindowState.Normal;
                telaCheia = false;
            }
        }

        public void AbrirFinanceiro()
        {
            if(_administrador.NivelAcesso == NivelAcessoEnum.Financeiro)
            {
                MainContent.Content = new FinanceiroView();

            }
        }
        public void AbrirTela(UserControl tela)
        {
            MainContent.Content = tela;
        }

        private void LimparSelecaoMenu()
        {
            //BtnDashboard.Tag = null;

            BtnGestaoPessoas.Tag = null;
            BtnGestaoFinanceira.Tag = null;
            BtnGestaoServicos.Tag = null;
            BtnGestaoLocacoes.Tag = null;
            BtnComunicacao.Tag = null;
            //BtnConfiguracao.Tag = null;
            BtnLogs.Tag = null;
            BtnUsuarios.Tag = null;
            BtnAdministradores.Tag = null;
            BtnProfissionais.Tag = null;
            BtnClientes.Tag = null;
            BtnEmpresas.Tag = null;

            // Submenus do financeiro
            BtnVisaoGeralFinanceiro.Tag = null;
            BtnDashboardFinanceiro.Tag = null;

            

            
        }

        private void SelecionarMenu(Button menuPrincipal, Button submenu = null)
        {
            LimparSelecaoMenu();

            if (menuPrincipal != null)
                menuPrincipal.Tag = "Ativo";

            if (submenu != null)
                submenu.Tag = "Ativo";
        }

        private void AbrirSubMenu(StackPanel submenu, PackIconMaterial seta)
        {
            bool aberto = submenu.Visibility == Visibility.Visible;

            submenu.Visibility = aberto ? Visibility.Collapsed : Visibility.Visible;

            seta.Kind = aberto
                ? PackIconMaterialKind.ChevronDown
                : PackIconMaterialKind.ChevronUp;
        }

        public void VerificarPermissoes()
        {
            //BtnDashboard.Visibility = Visibility.Collapsed;

            BtnGestaoPessoas.Visibility = Visibility.Collapsed;
            BtnUsuarios.Visibility = Visibility.Collapsed;
            BtnAdministradores.Visibility = Visibility.Collapsed;
            BtnProfissionais.Visibility = Visibility.Collapsed;
            BtnClientes.Visibility = Visibility.Collapsed;
            BtnEmpresas.Visibility = Visibility.Collapsed;

            BtnGestaoFinanceira.Visibility = Visibility.Collapsed;
            BtnVisaoGeralFinanceiro.Visibility = Visibility.Collapsed;
            BtnDashboardFinanceiro.Visibility = Visibility.Collapsed;
            BtnGestaoLocacoes.Visibility = Visibility.Collapsed;
            

            BtnComunicacao.Visibility = Visibility.Collapsed;
            //BtnConfiguracao.Visibility = Visibility.Collapsed;
            BtnLogs.Visibility = Visibility.Collapsed;

            switch (_administrador.NivelAcesso)
            {
                case NivelAcessoEnum.AdministradorGeral:
                    //BtnDashboard.Visibility = Visibility.Visible;

                    BtnGestaoPessoas.Visibility = Visibility.Visible;
                    BtnUsuarios.Visibility = Visibility.Visible;
                    BtnAdministradores.Visibility = Visibility.Visible;
                    BtnProfissionais.Visibility = Visibility.Visible;
                    BtnClientes.Visibility = Visibility.Visible;
                    BtnEmpresas.Visibility = Visibility.Visible;

                    BtnGestaoFinanceira.Visibility = Visibility.Visible;
                    BtnVisaoGeralFinanceiro.Visibility = Visibility.Visible;
                    BtnDashboardFinanceiro.Visibility = Visibility.Visible;

                    BtnGestaoServicos.Visibility = Visibility.Visible;
                   

                    BtnGestaoLocacoes.Visibility = Visibility.Visible;
                    

                    BtnComunicacao.Visibility = Visibility.Visible;
                    //BtnConfiguracao.Visibility = Visibility.Visible;
                    BtnLogs.Visibility = Visibility.Visible;
                    break;

                case NivelAcessoEnum.Atendente:
                    //BtnDashboard.Visibility = Visibility.Visible;

                    BtnGestaoPessoas.Visibility = Visibility.Visible;
                    BtnClientes.Visibility = Visibility.Visible;

                    BtnGestaoLocacoes.Visibility = Visibility.Visible;
                    

                    BtnComunicacao.Visibility = Visibility.Visible;
                    break;

                case NivelAcessoEnum.Financeiro:
                    //BtnDashboard.Visibility = Visibility.Visible;

                    BtnGestaoFinanceira.Visibility = Visibility.Visible;
                    BtnVisaoGeralFinanceiro.Visibility = Visibility.Visible;
                    BtnDashboardFinanceiro.Visibility = Visibility.Visible;
                    BtnComunicacao.Visibility = Visibility.Visible;
                    break;

                case NivelAcessoEnum.Suporte:
                    //BtnDashboard.Visibility = Visibility.Visible;

                    BtnGestaoServicos.Visibility = Visibility.Visible;
                    

                    BtnComunicacao.Visibility = Visibility.Visible;
                    break;

                case NivelAcessoEnum.Moderador:
                    //BtnDashboard.Visibility = Visibility.Visible;

                    BtnGestaoPessoas.Visibility = Visibility.Visible;
                    BtnUsuarios.Visibility = Visibility.Visible;
                    BtnClientes.Visibility = Visibility.Visible;

                    BtnLogs.Visibility = Visibility.Visible;
                    BtnComunicacao.Visibility = Visibility.Visible;
                    break;
            }
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

                Close();
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

        //private void BtnDashboard_Click(object sender, RoutedEventArgs e)
        //{
        //    MainContent.Content = new DashBoardView();
        //    SelecionarMenu(BtnDashboard);
        //}

        private void BtnGestaoPessoas_Click(object sender, RoutedEventArgs e)
        {
            AbrirSubMenu(SubMenuPessoas, SetaPessoas);
            SelecionarMenu(BtnGestaoPessoas);
        }

        private void BtnUsuarios_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new UsuariosView();
            SelecionarMenu(BtnGestaoPessoas, BtnUsuarios);
        }

        private void BtnAdministradores_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new AdministradorView();
            SelecionarMenu(BtnGestaoPessoas, BtnAdministradores);
        }

        private void BtnProfissional_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new ProfissionalView();
            SelecionarMenu(BtnGestaoPessoas, BtnProfissionais);
        }

        private void BtnClientes_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new ClienteView();
            SelecionarMenu(BtnGestaoPessoas, BtnClientes);
        }

        private void BtnEmpresas_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new EmpresaView();
            SelecionarMenu(BtnGestaoPessoas, BtnEmpresas);
        }

        private void BtnGestaoFinanceira_Click(object sender, RoutedEventArgs e)
        {
            AbrirSubMenu(SubMenuFinanceiro, SetaFinanceiro);
            SelecionarMenu(BtnGestaoFinanceira);
        }
        private void BtnVisaoGeralFinanceiro_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new FinanceiroView();

            SelecionarMenu(
                BtnGestaoFinanceira,
                BtnVisaoGeralFinanceiro);
        }
        private void BtnDashboardFinanceiro_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new DashboardFinanceiroView();

            SelecionarMenu(
                BtnGestaoFinanceira,
                BtnDashboardFinanceiro);
        }

        private void BtnGestaoServicos_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new GestaoServicosView();

            SelecionarMenu(BtnGestaoServicos);
        }

        

        private void BtnGestaoLocacoes_Click(object sender, RoutedEventArgs e)
        {
            
            SelecionarMenu(BtnGestaoLocacoes);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new ComunicacaoView(IdUsuarioLogado);
            SelecionarMenu(BtnComunicacao);
        }

        private void btnLogs_Click(object sender, RoutedEventArgs e)
        {
            if (_administrador.NivelAcesso != NivelAcessoEnum.AdministradorGeral)
            {
                MessageBox.Show("Você não tem permissão.");
                return;
            }

            MainContent.Content = new LogsControl();
            SelecionarMenu(BtnLogs);
        }
    }
}