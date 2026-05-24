using System.Linq.Expressions;
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
            try
            {

                using (var db = new MusicStationContext())
                {
                    Administrador adm = db.Administradores
                        .FirstOrDefault(a =>
                            a.NomeUsuario == nomeUsuario);
                            


                    if (adm == null)
                    {
                        MessageBox.Show("Nome de usuário ou senha incorretos.", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }


                    if (!adm.Ativo)
                    {
                        MessageBox.Show("Esta conta está desativada.", "Acesso negado", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }

                    string senhaCorreta = adm.SenhaHash;
                    if (senhaCorreta != adm.SenhaHash)
                    {
                        MessageBox.Show("Nome de usuário ou senha incorretos.", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    //bool senhaCorreta = BCrypt.Net.BCrypt.Verify(senha, adm.SenhaHash);
                    

                    //if (!senhaCorreta)
                    //{
                    //    MessageBox.Show("Nome de usuário ou senha incorretos.", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                    //    return;
                    //}


                    var login = db.TiposAcao.FirstOrDefault(t => t.Nome == "Login");

                    if (login == null)
                    {
                        MessageBox.Show("Vish, vey. Deu probelma ai", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                    }

                    var log = new LogSistema
                    {
                        UsuarioId = adm.Id,
                        TipoAcaoId = login.Id,
                        Entidade = "Administrador",
                        EntidadeId = adm.Id,
                        Descricao = $"Administrador {adm.NomeUsuario} fez login.",
                        DataHora = DateTime.Now,
                        NomeComputador = Environment.MachineName
                    };

                    db.LogsSistema.Add(log);
                    db.SaveChanges();



                    FormMenu tela = new FormMenu(adm);
                    tela.Show();

                    this.Close();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ocorreu um erro ao tentar fazer login: {ex.Message}", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }



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
                //}

        }
    }
}