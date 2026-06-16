using MahApps.Metro.IconPacks;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using wpf_projeto_integrador.Data;
using wpf_projeto_integrador.Helpers;
using wpf_projeto_integrador.Models;
using wpf_projeto_integrador.View.Users.View;

namespace wpf_projeto_integrador.View.Users.ControleDeUsuarios
{
    public partial class EmpresaFormView : UserControl
    {
        private EmpresaViewModel empresa;

        public EmpresaFormView()
        {
            InitializeComponent();

            btnDesativar.Visibility = Visibility.Collapsed;
            txtDataCadastro.Visibility = Visibility.Collapsed;

            btnSalvar.Text = "Cadastrar Empresa";
            txtModo.Text = "Cadastro";
        }

        public EmpresaFormView(EmpresaViewModel empresa)
        {
            InitializeComponent();

            this.empresa = empresa;

            CarregarEmpresa(empresa);

            txtModo.Text = "Edição";

            if (empresa.Status == "Inativa")
            {
                txtBotaoDesativar.Text = "Ativar empresa";
                iconBotaoDesativar.Kind = PackIconMaterialKind.OfficeBuilding;
                iconBotaoDesativar.Foreground =
                    new SolidColorBrush(Color.FromRgb(34, 197, 94));

                btnDesativar.Style = (Style)FindResource("BotaoAtivar");
            }
        }

        private void BtnSalvar_Click(object sender, RoutedEventArgs e)
        {
            if (btnSalvar.Text == "Cadastrar Empresa")
            {
                CadastrarEmpresa();
            }
            else
            {
                AtualizarEmpresa();
            }
        }

        private void BtnDesativar_Click(object sender, RoutedEventArgs e)
        {
            if (txtBotaoDesativar.Text == "Ativar empresa")
            {
                AtivarEmpresa();
            }
            else
            {
                DesativarEmpresa();
            }
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
            telaPrincipal.AbrirTela(new EmpresaView());
        }

        private void CarregarEmpresa(EmpresaViewModel empresa)
        {
            using var db = new MusicStationContext();

            var empresaBanco = db.Empresas
                .FirstOrDefault(e => e.Id == empresa.Id);

            if (empresaBanco == null)
            {
                MessageBox.Show("Empresa não encontrada.");
                return;
            }

            txtNome.Text = empresaBanco.Nome;
            txtNomeFantasia.Text = empresaBanco.NomeFantasia;
            txtCnpj.Text = empresaBanco.Cnpj;
            txtResponsavel.Text = empresaBanco.Responsavel;
            txtEmail.Text = empresaBanco.Email;
            txtNomeUsuario.Text = empresaBanco.NomeUsuario;
            txtTelefone.Text = empresaBanco.Telefone;
            txtEndereco.Text = empresaBanco.Endereco;
            txtDataCadastro.Text = empresaBanco.DataCriacao.ToString("dd/MM/yyyy");
        }

        private void LimparCampos()
        {
            txtNome.Clear();
            txtNomeFantasia.Clear();
            txtCnpj.Clear();
            txtResponsavel.Clear();
            txtEmail.Clear();
            txtNomeUsuario.Clear();
            txtSenha.Clear();
            txtTelefone.Clear();
            txtEndereco.Clear();
            txtDataCadastro.Clear();
        }

        private bool CamposObrigatoriosPreenchidos()
        {
            if (string.IsNullOrWhiteSpace(txtNome.Text) ||
                string.IsNullOrWhiteSpace(txtNomeFantasia.Text) ||
                string.IsNullOrWhiteSpace(txtCnpj.Text) ||
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

        private void CadastrarEmpresa()
        {
            try
            {
                if (!CamposObrigatoriosPreenchidos())
                    return;

                if (string.IsNullOrWhiteSpace(txtSenha.Password))
                {
                    MessageBox.Show(
                        "Informe uma senha para cadastrar a empresa.",
                        "Atenção",
                        MessageBoxButton.OK,
                        MessageBoxImage.Warning);

                    return;
                }

                using var db = new MusicStationContext();

                bool cnpjExiste = db.Empresas.Any(e => e.Cnpj == txtCnpj.Text);

                if (cnpjExiste)
                {
                    MessageBox.Show(
                        "Já existe uma empresa cadastrada com este CNPJ.",
                        "Atenção",
                        MessageBoxButton.OK,
                        MessageBoxImage.Warning);

                    return;
                }

                bool usuarioExiste = db.Usuarios.Any(u => u.NomeUsuario == txtNomeUsuario.Text);

                if (usuarioExiste)
                {
                    MessageBox.Show(
                        "Já existe um usuário com este nome de usuário.",
                        "Atenção",
                        MessageBoxButton.OK,
                        MessageBoxImage.Warning);

                    return;
                }

                var novaEmpresa = new Empresa
                {
                    Nome = txtNome.Text,
                    NomeFantasia = txtNomeFantasia.Text,
                    Cnpj = txtCnpj.Text,
                    Responsavel = txtResponsavel.Text,
                    Email = txtEmail.Text,
                    NomeUsuario = txtNomeUsuario.Text,
                    SenhaHash = BCrypt.Net.BCrypt.HashPassword(txtSenha.Password),
                    Telefone = txtTelefone.Text,
                    Endereco = txtEndereco.Text,
                    DataCriacao = DateTime.Now,
                    Ativo = true
                };

                db.Empresas.Add(novaEmpresa);

                db.SaveChanges();

                GerenciadorLogs.FazerRegistro(
                    db,
                    SessaoUsuario.usuarioLogado?.Id,
                    TipoAcaoLog.Cadastro,
                    $"Administrador {SessaoUsuario.usuarioLogado?.NomeUsuario} cadastrou a empresa {novaEmpresa.NomeUsuario}.",
                    "Empresa",
                    novaEmpresa.Id,
                    true,
                    null,
                    "Cadastro de Empresa");

                db.SaveChanges();

                MessageBox.Show(
                    "Empresa cadastrada com sucesso!",
                    "Sucesso",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);

                LimparCampos();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.InnerException?.Message ?? ex.Message,
                    "Erro ao cadastrar empresa",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private void AtualizarEmpresa()
        {
            try
            {
                if (!CamposObrigatoriosPreenchidos())
                    return;

                using var db = new MusicStationContext();

                var empresaBanco = db.Empresas
                    .FirstOrDefault(e => e.Id == empresa.Id);

                if (empresaBanco == null)
                {
                    MessageBox.Show("Empresa não encontrada.");
                    return;
                }

                bool cnpjExiste = db.Empresas
                    .Any(e => e.Cnpj == txtCnpj.Text && e.Id != empresaBanco.Id);

                if (cnpjExiste)
                {
                    MessageBox.Show(
                        "Já existe outra empresa cadastrada com este CNPJ.",
                        "Atenção",
                        MessageBoxButton.OK,
                        MessageBoxImage.Warning);

                    return;
                }

                bool usuarioExiste = db.Usuarios
                    .Any(u => u.NomeUsuario == txtNomeUsuario.Text && u.Id != empresaBanco.Id);

                if (usuarioExiste)
                {
                    MessageBox.Show(
                        "Já existe outro usuário com este nome de usuário.",
                        "Atenção",
                        MessageBoxButton.OK,
                        MessageBoxImage.Warning);

                    return;
                }

                empresaBanco.Nome = txtNome.Text;
                empresaBanco.NomeFantasia = txtNomeFantasia.Text;
                empresaBanco.Cnpj = txtCnpj.Text;
                empresaBanco.Responsavel = txtResponsavel.Text;
                empresaBanco.Email = txtEmail.Text;
                empresaBanco.NomeUsuario = txtNomeUsuario.Text;
                empresaBanco.Telefone = txtTelefone.Text;
                empresaBanco.Endereco = txtEndereco.Text;

                if (!string.IsNullOrWhiteSpace(txtSenha.Password))
                {
                    empresaBanco.SenhaHash =
                        BCrypt.Net.BCrypt.HashPassword(txtSenha.Password);
                }

                GerenciadorLogs.FazerRegistro(
                    db,
                    SessaoUsuario.usuarioLogado?.Id,
                    TipoAcaoLog.Atualizacao,
                    $"Administrador {SessaoUsuario.usuarioLogado?.NomeUsuario} atualizou a empresa {empresaBanco.NomeUsuario}.",
                    "Empresa",
                    empresaBanco.Id,
                    true,
                    null,
                    "Edição de Empresa");

                db.SaveChanges();

                MessageBox.Show(
                    "Empresa atualizada com sucesso!",
                    "Sucesso",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);

                VoltarParaLista();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.InnerException?.Message ?? ex.Message,
                    "Erro ao atualizar empresa",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private void DesativarEmpresa()
        {
            try
            {
                if (empresa == null)
                {
                    MessageBox.Show("Empresa não encontrada.");
                    return;
                }

                var resultado = MessageBox.Show(
                    "Deseja realmente desativar esta empresa?",
                    "Confirmação",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);

                if (resultado != MessageBoxResult.Yes)
                    return;

                using var db = new MusicStationContext();

                var empresaBanco = db.Empresas
                    .FirstOrDefault(e => e.Id == empresa.Id);

                if (empresaBanco == null)
                {
                    MessageBox.Show("Empresa não encontrada.");
                    return;
                }

                empresaBanco.Ativo = false;

                GerenciadorLogs.FazerRegistro(
                    db,
                    SessaoUsuario.usuarioLogado?.Id,
                    TipoAcaoLog.Desativacao,
                    $"Administrador {SessaoUsuario.usuarioLogado?.NomeUsuario} desativou a empresa {empresaBanco.NomeUsuario}.",
                    "Empresa",
                    empresaBanco.Id,
                    true,
                    null,
                    "Edição de Empresa");

                db.SaveChanges();

                MessageBox.Show(
                    "Empresa desativada com sucesso.",
                    "Sucesso",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);

                VoltarParaLista();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.InnerException?.Message ?? ex.Message,
                    "Erro ao desativar empresa",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private void AtivarEmpresa()
        {
            try
            {
                if (empresa == null)
                {
                    MessageBox.Show("Empresa não encontrada.");
                    return;
                }

                var resultado = MessageBox.Show(
                    "Deseja realmente ativar esta empresa?",
                    "Confirmação",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);

                if (resultado != MessageBoxResult.Yes)
                    return;

                using var db = new MusicStationContext();

                var empresaBanco = db.Empresas
                    .FirstOrDefault(e => e.Id == empresa.Id);

                if (empresaBanco == null)
                {
                    MessageBox.Show("Empresa não encontrada.");
                    return;
                }

                empresaBanco.Ativo = true;

                GerenciadorLogs.FazerRegistro(
                    db,
                    SessaoUsuario.usuarioLogado?.Id,
                    TipoAcaoLog.Reativacao,
                    $"Administrador {SessaoUsuario.usuarioLogado?.NomeUsuario} ativou a empresa {empresaBanco.NomeUsuario}.",
                    "Empresa",
                    empresaBanco.Id,
                    true,
                    null,
                    "Edição de Empresa");

                db.SaveChanges();

                MessageBox.Show(
                    "Empresa ativada com sucesso.",
                    "Sucesso",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);

                VoltarParaLista();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.InnerException?.Message ?? ex.Message,
                    "Erro ao ativar empresa",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }
    }
}