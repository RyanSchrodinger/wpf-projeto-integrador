using MahApps.Metro.IconPacks;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using wpf_projeto_integrador.Data;
using wpf_projeto_integrador.Helpers;
using wpf_projeto_integrador.Models;
using wpf_projeto_integrador.View.Users;
using wpf_projeto_integrador.View.Users.View;

namespace wpf_projeto_integrador.View.Users.ControleDeUsuarios
{
    public partial class ClienteFormView : UserControl
    {
        private ClienteViewModel cliente;

        public ClienteFormView()
        {
            InitializeComponent();

            cmbStatus.Visibility = Visibility.Collapsed;
            txtDataCadastro.Visibility = Visibility.Collapsed;
            btnDesativar.Visibility = Visibility.Collapsed;

            btnSalvar.Text = "Cadastrar Cliente";
            txtModo.Text = "Cadastro";
        }

        public ClienteFormView(ClienteViewModel cliente)
        {
            InitializeComponent();

            this.cliente = cliente;

            CarregarCliente(cliente);

            txtModo.Text = "Edição";
            cmbStatus.Visibility = Visibility.Collapsed;

            if (cliente.Status == "Inativo")
            {
                txtBotaoDesativar.Text = "Ativar Usuário";
                iconBotaoDesativar.Kind = PackIconMaterialKind.AccountCheck;
                iconBotaoDesativar.Foreground =
                    new SolidColorBrush(Color.FromRgb(34, 197, 94));

                btnDesativar.Style = (Style)FindResource("BotaoAtivar");
            }
        }

        private void BtnSalvar_Click(object sender, RoutedEventArgs e)
        {
            if (btnSalvar.Text == "Cadastrar Cliente")
                CadastrarCliente();
            else
                AtualizarCliente();
        }

        private void BtnDesativar_Click(object sender, RoutedEventArgs e)
        {
            if (txtBotaoDesativar.Text == "Ativar Usuário")
                AtivarConta();
            else
                DesativarConta();
        }

        private void BtnCancelar_Click(object sender, RoutedEventArgs e)
        {
            VoltarParaLista();
        }

        private void BtnVoltar_Click(object sender, RoutedEventArgs e)
        {
            VoltarParaLista();
        }

        private void VoltarParaLista()
        {
            var telaPrincipal = (FormMenu)Window.GetWindow(this);
            telaPrincipal.AbrirTela(new ClienteView());
        }

        private void CarregarCliente(ClienteViewModel cliente)
        {
            using var db = new MusicStationContext();

            var clienteBanco = db.Clientes
                .FirstOrDefault(c => c.Id == cliente.Id);

            if (clienteBanco == null)
            {
                MessageBox.Show("Cliente não encontrado.");
                return;
            }

            txtNome.Text = clienteBanco.Nome;
            txtEmail.Text = clienteBanco.Email;
            txtNomeUsuario.Text = clienteBanco.NomeUsuario;
            txtTelefone.Text = clienteBanco.Telefone;
            txtRua.Text = clienteBanco.Rua;
            txtNumero.Text = clienteBanco.Numero;
            txtCidade.Text = clienteBanco.Cidade;
            txtDataCadastro.Text = clienteBanco.DataCriacao.ToString("dd/MM/yyyy");
            cmbStatus.Text = clienteBanco.Ativo ? "Ativo" : "Inativo";
        }

        private void LimparCampos()
        {
            txtNome.Clear();
            txtEmail.Clear();
            txtNomeUsuario.Clear();
            txtSenha.Clear();
            txtTelefone.Clear();
            txtRua.Clear();
            txtNumero.Clear();
            txtCidade.Clear();
            txtDataCadastro.Clear();

            cmbStatus.SelectedIndex = -1;
        }

        private bool CamposObrigatoriosPreenchidos()
        {
            if (string.IsNullOrWhiteSpace(txtNome.Text) ||
                string.IsNullOrWhiteSpace(txtEmail.Text) ||
                string.IsNullOrWhiteSpace(txtNomeUsuario.Text))
            {
                MessageBox.Show(
                    "Preencha os campos obrigatórios.",
                    "Atenção",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);

                return false;
            }

            return true;
        }

        private void CadastrarCliente()
        {
            try
            {
                if (!CamposObrigatoriosPreenchidos())
                    return;

                if (string.IsNullOrWhiteSpace(txtSenha.Password))
                {
                    MessageBox.Show(
                        "Informe uma senha para cadastrar o cliente.",
                        "Atenção",
                        MessageBoxButton.OK,
                        MessageBoxImage.Warning);

                    return;
                }

                using var db = new MusicStationContext();

                var novoCliente = new Cliente
                {
                    Nome = txtNome.Text,
                    Email = txtEmail.Text,
                    NomeUsuario = txtNomeUsuario.Text,
                    SenhaHash = BCrypt.Net.BCrypt.HashPassword(txtSenha.Password),
                    Telefone = txtTelefone.Text,
                    Rua = txtRua.Text,
                    Numero = txtNumero.Text,
                    Cidade = txtCidade.Text,
                    DataCriacao = DateTime.Now,
                    Ativo = true
                };

                db.Clientes.Add(novoCliente);

                GerenciadorLogs.FazerRegistro(
                    db,
                    SessaoUsuario.usuarioLogado?.Id,
                    TipoAcaoLog.Cadastro,
                    $"Administrador {SessaoUsuario.usuarioLogado?.NomeUsuario} cadastrou o cliente {novoCliente.NomeUsuario}.",
                    "Cliente",
                    novoCliente.Id,
                    true,
                    null,
                    "Cadastro de Cliente");

                db.SaveChanges();

                MessageBox.Show(
                    "Cliente cadastrado com sucesso!",
                    "Sucesso",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);

                LimparCampos();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.InnerException?.Message ?? ex.Message,
                    "Erro ao cadastrar cliente",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private void AtualizarCliente()
        {
            try
            {
                if (!CamposObrigatoriosPreenchidos())
                    return;

                using var db = new MusicStationContext();

                var clienteBanco = db.Clientes
                    .FirstOrDefault(c => c.Id == cliente.Id);

                if (clienteBanco == null)
                {
                    MessageBox.Show("Cliente não encontrado.");
                    return;
                }

                clienteBanco.Nome = txtNome.Text;
                clienteBanco.Email = txtEmail.Text;
                clienteBanco.NomeUsuario = txtNomeUsuario.Text;
                clienteBanco.Telefone = txtTelefone.Text;
                clienteBanco.Rua = txtRua.Text;
                clienteBanco.Numero = txtNumero.Text;
                clienteBanco.Cidade = txtCidade.Text;

                if (!string.IsNullOrWhiteSpace(txtSenha.Password))
                {
                    clienteBanco.SenhaHash =
                        BCrypt.Net.BCrypt.HashPassword(txtSenha.Password);
                }

                GerenciadorLogs.FazerRegistro(
                    db,
                    SessaoUsuario.usuarioLogado?.Id,
                    TipoAcaoLog.Atualizacao,
                    $"Administrador {SessaoUsuario.usuarioLogado?.NomeUsuario} atualizou o cliente {clienteBanco.NomeUsuario}.",
                    "Cliente",
                    clienteBanco.Id,
                    true,
                    null,
                    "Edição de Cliente");

                db.SaveChanges();

                MessageBox.Show(
                    "Cliente atualizado com sucesso!",
                    "Sucesso",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);

                VoltarParaLista();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.InnerException?.Message ?? ex.Message,
                    "Erro ao atualizar cliente",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        public void DesativarConta()
        {
            try
            {
                if (cliente == null)
                {
                    MessageBox.Show("Cliente não encontrado.");
                    return;
                }

                var resultado = MessageBox.Show(
                    "Deseja realmente desativar esta conta?",
                    "Confirmação",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);

                if (resultado != MessageBoxResult.Yes)
                    return;

                using var db = new MusicStationContext();

                var clienteBanco = db.Clientes
                    .FirstOrDefault(c => c.Id == cliente.Id);

                if (clienteBanco == null)
                {
                    MessageBox.Show("Cliente não encontrado.");
                    return;
                }

                clienteBanco.Ativo = false;

                GerenciadorLogs.FazerRegistro(
                    db,
                    SessaoUsuario.usuarioLogado?.Id,
                    TipoAcaoLog.Desativacao,
                    $"Administrador {SessaoUsuario.usuarioLogado?.NomeUsuario} desativou o cliente {clienteBanco.NomeUsuario}.",
                    "Cliente",
                    clienteBanco.Id,
                    true,
                    null,
                    "Edição de Cliente");

                db.SaveChanges();

                MessageBox.Show(
                    "Conta desativada com sucesso.",
                    "Sucesso",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);

                VoltarParaLista();
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

        private void AtivarConta()
        {
            try
            {
                using var db = new MusicStationContext();

                var clienteBanco = db.Clientes
                    .FirstOrDefault(c => c.Id == cliente.Id);

                if (clienteBanco == null)
                {
                    MessageBox.Show("Cliente não encontrado.");
                    return;
                }

                var resultado = MessageBox.Show(
                    "Deseja realmente ativar esta conta?",
                    "Confirmação",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);

                if (resultado != MessageBoxResult.Yes)
                    return;

                clienteBanco.Ativo = true;

                GerenciadorLogs.FazerRegistro(
                    db,
                    SessaoUsuario.usuarioLogado?.Id,
                    TipoAcaoLog.Reativacao,
                    $"Administrador {SessaoUsuario.usuarioLogado?.NomeUsuario} ativou o cliente {clienteBanco.NomeUsuario}.",
                    "Cliente",
                    clienteBanco.Id,
                    true,
                    null,
                    "Edição de Cliente");

                db.SaveChanges();

                MessageBox.Show(
                    "Conta reativada com sucesso.",
                    "Sucesso",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);

                VoltarParaLista();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.InnerException?.Message ?? ex.Message,
                    "Erro ao ativar conta",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }
    }
}