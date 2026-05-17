using MahApps.Metro.IconPacks;
using System.Windows;
using System.Windows.Media;
using wpf_pi.Views;
using wpf_projeto_integrador.Models;
using wpf_projeto_integrador.View;
using static wpf_projeto_integrador.Models.Administrador;

namespace wpf_projeto_integrador
{
    public partial class FormMenu : Window
    {
        private Administrador _administrador;

        public FormMenu(Administrador administrador)
        {
            InitializeComponent();
            _administrador = administrador;
            VerificarPermissoes();
        }


        public void VerificarPermissoes()
        {
            if (_administrador.NivelAcesso != Administrador.NivelAcessoEnum.Alto)
            {
                btnLogs.Visibility = Visibility.Collapsed;
            }
        }

        private void btnLogs_Click(object sender, RoutedEventArgs e)
        {
            if (_administrador.NivelAcesso != Administrador.NivelAcessoEnum.Alto)
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

        private void BtnUsuarios_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new UsuariosView();
        }
    }
        
}