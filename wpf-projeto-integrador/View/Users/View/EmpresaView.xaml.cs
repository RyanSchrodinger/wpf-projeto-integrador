using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using wpf_projeto_integrador.Data;
using wpf_projeto_integrador.View.Users.ControleDeUsuarios;

namespace wpf_projeto_integrador.View.Users.View
{
    public partial class EmpresaView : UserControl
    {
        private List<EmpresaViewModel> empresas = new();
        public EmpresaView()
        {
            InitializeComponent();
            CarregarEmpresas();
        }

        private void CarregarEmpresas()
        {
            using var db = new MusicStationContext();

            txtTotalEmpresas.Text = db.Empresas.Count().ToString();

            txtTotalAtivas.Text = db.Empresas
                .Count(e => e.Ativo)
                .ToString();

            txtComProfissionais.Text = db.Empresas
                .Include(e => e.Profissionais)
                .Count(e => e.Profissionais.Any())
                .ToString();

            txtSemProfissionais.Text = db.Empresas
                .Include(e => e.Profissionais)
                .Count(e => !e.Profissionais.Any())
                .ToString();

            empresas = db.Empresas
                .Include(e => e.Profissionais)
                .Select(e => new EmpresaViewModel
                {
                    Id = e.Id,
                    Nome = e.Nome,
                    NomeFantasia = e.NomeFantasia,
                    Cnpj = e.Cnpj,
                    Responsavel = e.Responsavel ?? "Não informado",
                    Email = e.Email,
                    NomeUsuario = e.NomeUsuario,
                    Telefone = e.Telefone ?? "Não informado",
                    Endereco = e.Endereco ?? "Não informado",

                    TotalProfissionais = e.Profissionais.Count,

                    Status = e.Ativo
                        ? "Ativa"
                        : "Inativa",

                    DataCadastro = e.DataCriacao.ToString("dd/MM/yyyy")
                })
                .ToList();

            string[] cores =
            {
                "#7C3AED",
                "#EC4899",
                "#3B82F6",
                "#F59E0B",
                "#0F8B8D"
            };

            int index = 0;

            foreach (var empresa in empresas)
            {
                empresa.Iniciais = string.Join("",
                    (empresa.NomeFantasia ?? "")
                        .Split(' ')
                        .Where(p => !string.IsNullOrWhiteSpace(p))
                        .Take(2)
                        .Select(p => p[0]))
                    .ToUpper();

                empresa.CorPerfil = cores[index % cores.Length];

                index++;
            }

            dgEmpresas.ItemsSource = empresas;
        }

        private void AplicarFiltros()
        {
            var lista = empresas.AsEnumerable();

            string busca = txtBusca.Text.ToLower();

            if (!string.IsNullOrWhiteSpace(busca))
            {
                lista = lista.Where(e =>
                    e.Nome.ToLower().Contains(busca) ||
                    e.NomeFantasia.ToLower().Contains(busca) ||
                    e.Cnpj.ToLower().Contains(busca) ||
                    e.Email.ToLower().Contains(busca) ||
                    e.NomeUsuario.ToLower().Contains(busca) ||
                    e.Responsavel.ToLower().Contains(busca));
            }

            string filtroProfissionais =
                ((ComboBoxItem)cmbProfissionais.SelectedItem)
                .Content
                .ToString();

            if (filtroProfissionais == "Com Profissionais")
            {
                lista = lista.Where(e => e.TotalProfissionais > 0);
            }
            else if (filtroProfissionais == "Sem Profissionais")
            {
                lista = lista.Where(e => e.TotalProfissionais == 0);
            }

            string status =
                ((ComboBoxItem)cmbStatus.SelectedItem)
                .Content
                .ToString();

            if (status != "Todos")
            {
                lista = lista.Where(e => e.Status == status);
            }

            dgEmpresas.ItemsSource = lista.ToList();
        }

        private void txtBusca_TextChanged(object sender, TextChangedEventArgs e)
        {
            AplicarFiltros();
        }

        private void Filtro_Changed(object sender, SelectionChangedEventArgs e)
        {
            if (dgEmpresas != null)
            {
                AplicarFiltros();
            }
        }

        private void BtnNovaEmpresa_Click(object sender, RoutedEventArgs e)
        {
            var telaPrincipal = (FormMenu)Window.GetWindow(this);

            telaPrincipal.AbrirTela(new EmpresaFormView());
        }

        private void BtnVisualizar_Click(object sender, RoutedEventArgs e)
        {
            var empresa = (sender as Button)?.DataContext as EmpresaViewModel;

            if (empresa == null)
            {
                MessageBox.Show("Empresa não encontrada.");
                return;
            }

            MessageBox.Show(
                $"Razão social: {empresa.Nome}\n" +
                $"Nome fantasia: {empresa.NomeFantasia}\n" +
                $"CNPJ: {empresa.Cnpj}\n" +
                $"Responsável: {empresa.Responsavel}\n" +
                $"Usuário: {empresa.NomeUsuario}\n" +
                $"Email: {empresa.Email}\n" +
                $"Telefone: {empresa.Telefone}\n" +
                $"Endereço: {empresa.Endereco}\n" +
                $"Profissionais: {empresa.TotalProfissionais}\n" +
                $"Status: {empresa.Status}",
                "Detalhes da empresa");
        }

        private void BtnEditar_Click(object sender, RoutedEventArgs e)
        {
            var empresa = (sender as Button)?.DataContext as EmpresaViewModel;

            if (empresa == null)
            {
                MessageBox.Show("Empresa não encontrada.");
                return;
            }

            var telaPrincipal = (FormMenu)Window.GetWindow(this);

            telaPrincipal.AbrirTela(new EmpresaFormView(empresa));
        }

        private void BtnProfissionais_Click(object sender, RoutedEventArgs e)
        {
            var empresaSelecionada = (sender as Button)?.DataContext as EmpresaViewModel;

            if (empresaSelecionada == null)
            {
                MessageBox.Show("Empresa não encontrada.");
                return;
            }

            using var db = new MusicStationContext();

            var profissionais = db.Profissionais
                .Where(p => p.EmpresaId == empresaSelecionada.Id)
                .Select(p => new ProfissionalEmpresaViewModel
                {
                    Id = p.Id,
                    Nome = p.Nome,
                    Email = p.Email,
                    Especialidade = p.Especialidade ?? "Não informado",
                    Status = p.Ativo ? "Ativo" : "Inativo"
                })
                .ToList();

            string[] cores =
            {
                "#7C3AED",
                "#EC4899",
                "#3B82F6",
                "#F59E0B",
                "#0F8B8D"
            };

            int index = 0;

            foreach (var profissional in profissionais)
            {
                profissional.Iniciais = string.Join("",
                    (profissional.Nome ?? "")
                        .Split(' ')
                        .Where(p => !string.IsNullOrWhiteSpace(p))
                        .Take(2)
                        .Select(p => p[0]))
                    .ToUpper();

                profissional.CorPerfil = cores[index % cores.Length];

                index++;
            }

            txtNomeEmpresaSelecionada.Text = empresaSelecionada.NomeFantasia;
            txtTotalProfissionaisEmpresa.Text = profissionais.Count.ToString();
            lstProfissionaisEmpresa.ItemsSource = profissionais;

            PainelProfissionais.Visibility = Visibility.Visible;
        }

        private void BtnFecharProfissionais_Click(object sender, RoutedEventArgs e)
        {
            PainelProfissionais.Visibility = Visibility.Collapsed;
        }

        private void BtnDesativar_Click(object sender, RoutedEventArgs e)
        {
            var empresaSelecionada =
                (sender as Button)?.DataContext as EmpresaViewModel;

            if (empresaSelecionada == null)
            {
                MessageBox.Show("Empresa não encontrada.");
                return;
            }

            using var db = new MusicStationContext();

            var empresa = db.Empresas
                .FirstOrDefault(e => e.Id == empresaSelecionada.Id);

            if (empresa == null)
            {
                MessageBox.Show("Empresa não encontrada no banco.");
                return;
            }

            string acao = empresa.Ativo ? "desativar" : "ativar";

            var resultado = MessageBox.Show(
                $"Deseja realmente {acao} a empresa {empresa.NomeFantasia}?",
                "Confirmação",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (resultado != MessageBoxResult.Yes)
                return;

            empresa.Ativo = !empresa.Ativo;

            db.SaveChanges();

            MessageBox.Show(
                empresa.Ativo
                    ? "Empresa ativada com sucesso."
                    : "Empresa desativada com sucesso.");

            CarregarEmpresas();
        }
    }

    public class EmpresaViewModel
    {
        public int Id { get; set; }

        public string Nome { get; set; } = "";

        public string NomeFantasia { get; set; } = "";

        public string Cnpj { get; set; } = "";

        public string Responsavel { get; set; } = "";

        public string Email { get; set; } = "";

        public string NomeUsuario { get; set; } = "";

        public string Telefone { get; set; } = "";

        public string Endereco { get; set; } = "";

        public int TotalProfissionais { get; set; }

        public string Status { get; set; } = "";

        public string DataCadastro { get; set; } = "";

        public string Iniciais { get; set; } = "";

        public string CorPerfil { get; set; } = "";
    }

    public class ProfissionalEmpresaViewModel
    {
        public int Id { get; set; }

        public string Nome { get; set; } = "";

        public string Email { get; set; } = "";

        public string Especialidade { get; set; } = "";

        public string Status { get; set; } = "";

        public string Iniciais { get; set; } = "";

        public string CorPerfil { get; set; } = "";
    }


}

