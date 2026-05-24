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
        public AdministradorFormView()
        {
            InitializeComponent();
        }

        private void BtnSalvar_Click(object sender, RoutedEventArgs e)
        {
            CadastrarUsuario();
        }

        private void BtnDesativar_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnCancelar_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnVoltar_Click(object sender, RoutedEventArgs e)
        {

        }


        #region Funções

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



