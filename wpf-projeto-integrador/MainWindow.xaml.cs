using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using wpf_projeto_integrador.Data;
using wpf_projeto_integrador.Models;

namespace wpf_projeto_integrador
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }



        private void btnEntrar_Click_1(object sender, RoutedEventArgs e)
        {

            string? nomeUsuario = txtUsuario.Text;
            string? senha = txtSenha.Password;
            if (string.IsNullOrWhiteSpace(nomeUsuario) || string.IsNullOrWhiteSpace(senha))
            {
                MessageBox.Show("Por favor, preencha todos os campos.", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            using (var db = new MusicStationContext())
            {
                Administrador adm = db.Administradores
                    .FirstOrDefault(a =>
                        a.NomeUsuario == nomeUsuario &&
                        a.SenhaHash == senha);


                if (adm == null)
                {
                    MessageBox.Show("Nome de usuário ou senha incorretos.", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }


                MessageBox.Show($"Bem-vindo, {adm.NomeUsuario}!", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);
                FormMenu tela = new FormMenu(adm);
                tela.Show();

                this.Close();





                // Aqui você pode adicionar a lógica de autenticação, como verificar o nome de usuário e senha no banco de dados.
                // Por exemplo:
                // if (AutenticarUsuario(nomeUsuario, senha))
                // {
                //     MessageBox.Show("Login bem-sucedido!", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);
                //     // Redirecionar para a próxima janela ou funcionalidade do aplicativo
                // }
                // else
                // {
                //     MessageBox.Show("Nome de usuário ou senha incorretos.", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                // }
            }

        }
    }
}