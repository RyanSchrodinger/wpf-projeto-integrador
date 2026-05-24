using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using wpf_projeto_integrador.Data;
using wpf_projeto_integrador.Models;

namespace wpf_projeto_integrador.View.Users.ControleDeUsuarios
{
    /// <summary>
    /// Interação lógica para AdministradorFormView.xam
    /// </summary>
    public partial class AdministradorFormView : UserControl
    {
        public AdministradorViewModel adm;
        public AdministradorFormView()
        {
            InitializeComponent();
            cmbStatus.Visibility = Visibility.Collapsed;
            txtDataCadastro.Visibility = Visibility.Collapsed;
            btnSalvar.Text = "Cadastrar Administrador";
            btnDesativar.Visibility = Visibility.Collapsed;

        }

        public AdministradorFormView(AdministradorViewModel adm)
        {
            this.adm = adm;
            InitializeComponent();
            CarregarAdministrador(adm);
            cmbStatus.Visibility = Visibility.Collapsed;
            
        }

        private void BtnSalvar_Click(object sender, RoutedEventArgs e)
        {
            if (btnSalvar.Text == "Cadastrar Administrador")
            {
                CadastrarUsuario();
            }
            else
            {
                
            }
            
        }

        private void BtnDesativar_Click(object sender, RoutedEventArgs e)
        {
            DesativarConta();

        }

        private void BtnCancelar_Click(object sender, RoutedEventArgs e)
        {

        }


        private void BtnVoltar_Click(  object sender,RoutedEventArgs e)
        {
            var telaPrincipal = (FormMenu)Window.GetWindow(this);

            telaPrincipal.AbrirTela( new AdministradorView());
        }


        #region Metodos

        public void CarregarAdministrador(AdministradorViewModel adm)
        {
            txtEmail.Text = adm.Email;
            txtDataCadastro.Text = adm.DataCadastro;
            txtNome.Text = adm.Nome;
            txtNomeUsuario.Text = adm.NomeUsuario;
            txtTelefone.Text = adm.Telefone;
            txtObservacao.Text = adm.Observacao;
            txtNome.Text = adm.Nome.ToString();
            cmbNivel.Text = adm.NivelAcesso.ToString();
            cmbStatus.Text = adm.Status.ToString();

        }

        public void LimparCampos()
        {
            txtDataCadastro.Clear();
            txtEmail.Clear(); 
            cmbNivel.SelectedIndex = -1;
            txtNome.Clear();
            txtEmail.Clear();
            txtNomeUsuario.Clear();
            cmbStatus.SelectedIndex = -1;
            txtSenha.Clear();
            txtTelefone.Clear();
        }

        public void DesativarConta()
        {
            try
            {
                if (adm.Id == null)
                {
                    MessageBox.Show("Administrador não encontrado.");
                    return;
                }
               
                var resultado = MessageBox.Show(
                    "Deseja realmente desativar esta conta?",
                    "Confirmação",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);
                if (resultado != MessageBoxResult.Yes)
                    return;

                using(var db = new MusicStationContext())
                {
                    var admm = db.Administradores
                        .FirstOrDefault(a => a.Id ==adm.Id);

                    if (admm == null)
                    {
                        MessageBox.Show("Administrador não encontrado.");
                        return;
                    }

                    admm.Ativo = false;

                    db.SaveChanges();

                    MessageBox.Show(
                        "Conta desativada com sucesso.",
                        "Sucesso",
                        MessageBoxButton.OK,
                        MessageBoxImage.Information);

                    var formMenu = (FormMenu)Window.GetWindow(this);

                    formMenu.AbrirTela(new AdministradorView());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Erro ao desativar conta: {ex.Message}",
                    "Erro",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }




        }

        public void CadastrarUsuario()
        {
            try
            {
                using (var db = new MusicStationContext())
                {

                    if (string.IsNullOrWhiteSpace(cmbNivel.Text))
                    {
                        MessageBox.Show("Selecione um nível de acesso.");
                        return;
                    }

                    string nome = txtNome.Text;
                    string nomeUser = txtNomeUsuario.Text;
                    string observacao = txtObservacao.Text;
                    string senhaHash = BCrypt.Net.BCrypt.HashPassword(txtSenha.Password);
                    string email = txtEmail.Text;
                    string telefone = txtTelefone.Text;
                    Administrador.NivelAcessoEnum nivelAcesso = Enum.Parse<Administrador.NivelAcessoEnum>(cmbNivel.Text);
                    if (string.IsNullOrWhiteSpace(nome) || string.IsNullOrWhiteSpace(nomeUser) || string.IsNullOrWhiteSpace(email) ||
                        string.IsNullOrWhiteSpace(txtSenha.Password))
                    {
                        MessageBox.Show("Preencha todos os campos.");
                        return;
                    }

                    var novoAdm = new Administrador
                    {
                        Nome = nome,
                        NomeUsuario = nomeUser,
                        SenhaHash = senhaHash,
                        Email = email,
                        NivelAcesso = nivelAcesso,
                        DataCriacao = DateTime.Now,
                        Ativo = true
                    };

                    db.Administradores.Add(novoAdm);

                    db.SaveChanges();

                    MessageBox.Show(
                        "Administrador cadastrado com sucesso!",
                        "Cadastro realizado",
                        MessageBoxButton.OK,
                        MessageBoxImage.Information
                        );
                }
            } 
            catch(Exception ex)
            {
                MessageBox.Show($"Ocorreu um erro ao tentar fazer login: {ex.Message}", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            LimparCampos();
        }
       
        
          
        
    }
    
        #endregion
    
}



