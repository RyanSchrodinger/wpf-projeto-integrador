using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
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
            SubMenuPessoas.Visibility =
               SubMenuPessoas.Visibility == Visibility.Visible
               ? Visibility.Collapsed
               : Visibility.Visible;
        }
    }
        
}