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
using wpf_projeto_integrador.Helpers;
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


                        GerenciadorLogs.FazerRegistro(
                            db,
                            null,
                            TipoAcaoLog.LoginFalha,
                            $"Tentativa de login com usuário inexistente: {nomeUsuario}",
                            null,
                            null,
                            false,
                            null,
                            "Tela Login"


                            );


                        db.LogsSistema.Add(new LogSistema
                        {
                            TipoAcao = TipoAcaoLog.LoginFalha,
                            Descricao = $"Tentativa de login com usuário inexistente: {nomeUsuario}",
                            Tela = "Tela de Login",
                            NomeComputador = Environment.MachineName,
                            Sucesso = false
                        });

                        db.SaveChanges();

                        MessageBox.Show("Nome de usuário ou senha incorretos.", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }


                    if (!adm.Ativo)
                    {

                        GerenciadorLogs.FazerRegistro(
                            db,
                            adm.Id,
                            TipoAcaoLog.AcessoNegado,
                            $"Administrador {adm.NomeUsuario} tentou acessar com conta desativada.",
                            "Administrador",
                            adm.Id,
                            false,
                            null,
                            "Tela de Login"
                            );

                        db.SaveChanges();

                        MessageBox.Show("Essa conta está desativada.", "Acesso negado", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }

                    //string senhaCorreta = adm.SenhaHash;
                    //if (senhaCorreta != adm.SenhaHash)
                    //{
                    //    MessageBox.Show("Nome de usuário ou senha incorretos.", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                    //    return;
                    //}

                    bool senhaCorreta = BCrypt.Net.BCrypt.Verify(senha, adm.SenhaHash);

                    if (!senhaCorreta)
                    {

                        GerenciadorLogs.FazerRegistro(
                            db,
                            adm.Id,
                            TipoAcaoLog.LoginFalha,
                            $"Senha incorreta para o administrador {adm.NomeUsuario}.",
                            "Administrador",
                            adm.Id,
                            false,
                            null,
                            "Tela de Login"


                            );
                        db.SaveChanges();

                        MessageBox.Show("Nome de usuário ou senha incorretos.", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }



                    GerenciadorLogs.FazerRegistro(
                        db,
                        adm.Id,
                        TipoAcaoLog.LoginSucesso,
                        $"Administrador {adm.NomeUsuario} fez login.",
                        "Administrador",
                        adm.Id,
                        true,
                        null,
                        "Tela de Login"
                        );

                    db.SaveChanges();

                    SessaoUsuario.Iniciar(adm);


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