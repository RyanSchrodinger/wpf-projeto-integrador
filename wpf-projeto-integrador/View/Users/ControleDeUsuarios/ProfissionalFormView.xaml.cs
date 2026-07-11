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

namespace wpf_projeto_integrador.View.Users.ControleDeUsuarios
{
    public partial class ProfissionalFormView : UserControl
    {
        private ProfissionalViewModel profissional;

        public ProfissionalFormView()
        {
            InitializeComponent();

            CarregarEmpresas();

            cmbStatus.Visibility = Visibility.Collapsed;
            txtDataCadastro.Visibility = Visibility.Collapsed;
            btnDesativar.Visibility = Visibility.Collapsed;

            btnSalvar.Text = "Cadastrar Profissional";
            txtModo.Text = "Cadastro";
        }

        public ProfissionalFormView(ProfissionalViewModel profissional)
        {
            InitializeComponent();

            this.profissional = profissional;

            CarregarEmpresas();
            CarregarProfissional(profissional);

            txtModo.Text = "Edição";
            cmbStatus.Visibility = Visibility.Collapsed;

            if (profissional.Status == "Inativo")
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
            if (btnSalvar.Text == "Cadastrar Profissional")
            {
                CadastrarProfissional();
            }
            else
            {
                AtualizarProfissional();
            }
        }

        private void BtnDesativar_Click(object sender, RoutedEventArgs e)
        {
            if (txtBotaoDesativar.Text == "Ativar Usuário")
            {
                AtivarConta();
            }
            else
            {
                DesativarConta();
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

            telaPrincipal.AbrirTela(new ProfissionalView());
        }

        private void CarregarEmpresas()
        {
            using var db = new MusicStationContext();

            cmbEmpresa.Items.Clear();

            cmbEmpresa.Items.Add(new ComboBoxItem
            {
                Content = "Autônomo",
                Tag = null,
                IsSelected = true
            });

            var empresas = db.Empresas
                .Where(e => e.Ativo)
                .OrderBy(e => e.NomeFantasia)
                .ToList();

            foreach (var empresa in empresas)
            {
                cmbEmpresa.Items.Add(new ComboBoxItem
                {
                    Content = empresa.NomeFantasia,
                    Tag = empresa.Id
                });
            }
        }

        private int? ObterEmpresaSelecionadaId()
        {
            if (cmbEmpresa.SelectedItem is ComboBoxItem item &&
                item.Tag != null)
            {
                return Convert.ToInt32(item.Tag);   
            }

            return null;
        }

        private void SelecionarEmpresa(int? empresaId)
        {
            foreach (ComboBoxItem item in cmbEmpresa.Items)
            {
                if (empresaId == null && item.Tag == null)
                {
                    cmbEmpresa.SelectedItem = item;
                    return;
                }

                if (item.Tag != null &&
                    Convert.ToInt32(item.Tag) == empresaId)
                {
                    cmbEmpresa.SelectedItem = item;
                    return;
                }
            }

            cmbEmpresa.SelectedIndex = 0;
        }

        private void CarregarProfissional(ProfissionalViewModel profissional)
        {
            using var db = new MusicStationContext();

            var profBanco = db.Profissionais
                .FirstOrDefault(p => p.Id == profissional.Id);

            if (profBanco == null)
            {
                MessageBox.Show("Profissional não encontrado.");
                return;
            }

            txtNome.Text = profBanco.Nome;
            txtEmail.Text = profBanco.Email;
            txtNomeUsuario.Text = profBanco.NomeUsuario;
            txtTelefone.Text = profBanco.Telefone;
            txtEspecialidade.Text = profBanco.Especialidade;
            txtEndereco.Text = profBanco.Endereco;
            txtDescricao.Text = profBanco.Descricao;
            txtDataCadastro.Text = profBanco.DataCriacao.ToString("dd/MM/yyyy");
            cmbStatus.Text = profBanco.Ativo ? "Ativo" : "Inativo";

            SelecionarEmpresa(profBanco.EmpresaId);
        }

        private void LimparCampos()
        {
            txtNome.Clear();
            txtEmail.Clear();
            txtNomeUsuario.Clear();
            txtSenha.Clear();
            txtTelefone.Clear();
            txtEspecialidade.Clear();
            txtEndereco.Clear();
            txtDescricao.Clear();
            txtDataCadastro.Clear();

            cmbEmpresa.SelectedIndex = 0;
            cmbStatus.SelectedIndex = -1;
        }

        private bool CamposObrigatoriosPreenchidos()
        {
            if (string.IsNullOrWhiteSpace(txtNome.Text) ||
                string.IsNullOrWhiteSpace(txtEmail.Text) ||
                string.IsNullOrWhiteSpace(txtNomeUsuario.Text) ||
                string.IsNullOrWhiteSpace(txtDescricao.Text))
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

        private void CadastrarProfissional()
        {
            try
            {
                if (!CamposObrigatoriosPreenchidos())
                    return;

                if (string.IsNullOrWhiteSpace(txtSenha.Password))
                {
                    MessageBox.Show(
                        "Informe uma senha para cadastrar o profissional.",
                        "Atenção",
                        MessageBoxButton.OK,
                        MessageBoxImage.Warning);

                    return;
                }

                using var db = new MusicStationContext();

                var novoProfissional = new Profissional
                {
                    Nome = txtNome.Text,
                    Email = txtEmail.Text,
                    NomeUsuario = txtNomeUsuario.Text,
                    SenhaHash = BCrypt.Net.BCrypt.HashPassword(txtSenha.Password),
                    Telefone = txtTelefone.Text,
                    Especialidade = txtEspecialidade.Text,
                    Endereco = txtEndereco.Text,
                    Descricao = txtDescricao.Text,
                    EmpresaId = ObterEmpresaSelecionadaId(),
                    DataCriacao = DateTime.Now,
                    Ativo = true
                };

                db.Profissionais.Add(novoProfissional);

                GerenciadorLogs.FazerRegistro(
                    db,
                    SessaoUsuario.usuarioLogado?.Id,
                    TipoAcaoLog.Cadastro,
                    $"Administrador {SessaoUsuario.usuarioLogado?.NomeUsuario} cadastrou o profissional {novoProfissional.NomeUsuario}.",
                    "Profissional",
                    novoProfissional.Id,
                    true,
                    null,
                    "Cadastro de Profissional");

                db.SaveChanges();

                MessageBox.Show(
                    "Profissional cadastrado com sucesso!",
                    "Sucesso",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);

                LimparCampos();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.InnerException?.Message ?? ex.Message,
                    "Erro ao cadastrar profissional",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private void AtualizarProfissional()
        {
            try
            {
                if (!CamposObrigatoriosPreenchidos())
                    return;

                using var db = new MusicStationContext();

                var profBanco = db.Profissionais
                    .FirstOrDefault(p => p.Id == profissional.Id);

                if (profBanco == null)
                {
                    MessageBox.Show("Profissional não encontrado.");
                    return;
                }

                profBanco.Nome = txtNome.Text;
                profBanco.Email = txtEmail.Text;
                profBanco.NomeUsuario = txtNomeUsuario.Text;
                profBanco.Telefone = txtTelefone.Text;
                profBanco.Especialidade = txtEspecialidade.Text;
                profBanco.Endereco = txtEndereco.Text;
                profBanco.Descricao = txtDescricao.Text;
                profBanco.EmpresaId = ObterEmpresaSelecionadaId();

                if (!string.IsNullOrWhiteSpace(txtSenha.Password))
                {
                    profBanco.SenhaHash =
                        BCrypt.Net.BCrypt.HashPassword(txtSenha.Password);
                }

                GerenciadorLogs.FazerRegistro(
                    db,
                    SessaoUsuario.usuarioLogado?.Id,
                    TipoAcaoLog.Atualizacao,
                    $"Administrador {SessaoUsuario.usuarioLogado?.NomeUsuario} atualizou o profissional {profBanco.NomeUsuario}.",
                    "Profissional",
                    profBanco.Id,
                    true,
                    null,
                    "Edição de Profissional");

                db.SaveChanges();

                MessageBox.Show(
                    "Profissional atualizado com sucesso!",
                    "Sucesso",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);

                VoltarParaLista();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.InnerException?.Message ?? ex.Message,
                    "Erro ao atualizar profissional",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        public void DesativarConta()
        {
            try
            {
                if (profissional == null)
                {
                    MessageBox.Show("Profissional não encontrado.");
                    return;
                }

                var resultado = MessageBox.Show(
                    "Deseja realmente desativar esta conta?",
                    "Confirmação",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);

                if (resultado != MessageBoxResult.Yes)
                    return;

                using (var db = new MusicStationContext())
                {
                    var profBanco = db.Profissionais
                        .FirstOrDefault(p => p.Id == profissional.Id);

                    if (profBanco == null)
                    {
                        MessageBox.Show("Profissional não encontrado.");
                        return;
                    }

                    profBanco.Ativo = false;

                    GerenciadorLogs.FazerRegistro(
                        db,
                        SessaoUsuario.usuarioLogado?.Id,
                        TipoAcaoLog.Desativacao,
                        $"Administrador {SessaoUsuario.usuarioLogado?.NomeUsuario} desativou o profissional {profBanco.NomeUsuario}.",
                        "Profissional",
                        profBanco.Id,
                        true,
                        null,
                        "Edição de Profissional");

                    db.SaveChanges();

                    MessageBox.Show(
                        "Conta desativada com sucesso.",
                        "Sucesso",
                        MessageBoxButton.OK,
                        MessageBoxImage.Information);

                    var formMenu = (FormMenu)Window.GetWindow(this);

                    formMenu.AbrirTela(new ProfissionalView());
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

        private void AtivarConta()
        {
            try
            {
                using var db = new MusicStationContext();

                var profBanco = db.Profissionais
                    .FirstOrDefault(p => p.Id == profissional.Id);

                if (profBanco == null)
                {
                    MessageBox.Show("Profissional não encontrado.");
                    return;
                }

                var resultado = MessageBox.Show(
                    "Deseja realmente ativar esta conta?",
                    "Confirmação",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);

                if (resultado != MessageBoxResult.Yes)
                    return;

                profBanco.Ativo = true;

                GerenciadorLogs.FazerRegistro(
                    db,
                    SessaoUsuario.usuarioLogado?.Id,
                    TipoAcaoLog.Reativacao,
                    $"Administrador {SessaoUsuario.usuarioLogado?.NomeUsuario} ativou o profissional {profBanco.NomeUsuario}.",
                    "Profissional",
                    profBanco.Id,
                    true,
                    null,
                    "Edição de Profissional");

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