using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using wpf_projeto_integrador.Data;
using wpf_projeto_integrador.View.Users.ControleDeUsuarios;

namespace wpf_projeto_integrador.View.Users
{
    public partial class ProfissionalView : UserControl
    {
        private List<ProfissionalViewModel> profissionais = new();

        public ProfissionalView()
        {
            InitializeComponent();
            CarregarProfissionais();
        }

        private void CarregarProfissionais()
        {
            using var db = new MusicStationContext();

            txtTotalProfissionais.Text = db.Profissionais.Count().ToString();

            txtTotalAtivos.Text = db.Profissionais
                .Count(p => p.Ativo)
                .ToString();

            txtTotalAutonomos.Text = db.Profissionais
                .Count(p => p.EmpresaId == null)
                .ToString();

            txtTotalComEmpresa.Text = db.Profissionais
                .Count(p => p.EmpresaId != null)
                .ToString();

            profissionais = db.Profissionais
                .Include(p => p.Empresa)
                .Select(p => new ProfissionalViewModel
                {
                    Id = p.Id,
                    Nome = p.Nome,
                    Email = p.Email,
                    NomeUsuario = p.NomeUsuario,

                    Telefone = p.Telefone ?? "Não informado",
                    Especialidade = p.Especialidade ?? "Não informado",

                    NomeEmpresa = p.Empresa != null
                        ? p.Empresa.NomeFantasia
                        : "Autônomo",

                    TipoProfissional = p.EmpresaId == null
                        ? "Autônomo"
                        : "Com Empresa",

                    Status = p.Ativo
                        ? "Ativo"
                        : "Inativo",

                    DataCadastro = p.DataCriacao.ToString("dd/MM/yyyy"),

                    Endereco = p.Endereco ?? "Não informado"
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

            dgProfissionais.ItemsSource = profissionais;
        }

        private void AplicarFiltros()
        {
            var lista = profissionais.AsEnumerable();

            string busca = txtBusca.Text.ToLower();

            if (!string.IsNullOrWhiteSpace(busca))
            {
                lista = lista.Where(p =>
                    p.Nome.ToLower().Contains(busca) ||
                    p.Email.ToLower().Contains(busca) ||
                    p.NomeUsuario.ToLower().Contains(busca) ||
                    p.Especialidade.ToLower().Contains(busca) ||
                    p.NomeEmpresa.ToLower().Contains(busca));
            }

            string tipo =
                ((ComboBoxItem)cmbTipo.SelectedItem)
                .Content
                .ToString();

            if (tipo != "Todos")
            {
                lista = lista.Where(p =>
                    p.TipoProfissional == tipo);
            }

            string status =
                ((ComboBoxItem)cmbStatus.SelectedItem)
                .Content
                .ToString();

            if (status != "Todos")
            {
                lista = lista.Where(p =>
                    p.Status == status);
            }

            dgProfissionais.ItemsSource = lista.ToList();
        }

        private void txtBusca_TextChanged(object sender, TextChangedEventArgs e)
        {
            AplicarFiltros();
        }

        private void Filtro_Changed(object sender, SelectionChangedEventArgs e)
        {
            if (dgProfissionais != null)
            {
                AplicarFiltros();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var telaPrincipal = (FormMenu)Window.GetWindow(this);

            telaPrincipal.AbrirTela(new ProfissionalFormView());
        }

        private void BtnVisualizar_Click(object sender, RoutedEventArgs e)
        {
            var profissional = (sender as Button)?.DataContext as ProfissionalViewModel;

            if (profissional == null)
            {
                MessageBox.Show("Profissional não encontrado.");
                return;
            }

            MessageBox.Show(
                $"Nome: {profissional.Nome}\n" +
                $"Usuário: {profissional.NomeUsuario}\n" +
                $"Email: {profissional.Email}\n" +
                $"Telefone: {profissional.Telefone}\n" +
                $"Especialidade: {profissional.Especialidade}\n" +
                $"Empresa: {profissional.NomeEmpresa}\n" +
                $"Tipo: {profissional.TipoProfissional}\n" +
                $"Status: {profissional.Status}\n" +
                $"Endereço: {profissional.Endereco}",
                "Detalhes do profissional");
        }

        private void BtnEditar_Click(object sender, RoutedEventArgs e)
        {
            var profissional = (sender as Button)?.DataContext as ProfissionalViewModel;

            if (profissional == null)
            {
                MessageBox.Show("Profissional não encontrado.");
                return;
            }

            var telaPrincipal = (FormMenu)Window.GetWindow(this);

            telaPrincipal.AbrirTela(new ProfissionalFormView(profissional));
        }

        private void BtnDesativar_Click(object sender, RoutedEventArgs e)
        {
            var profissionalSelecionado =
                (sender as Button)?.DataContext as ProfissionalViewModel;

            if (profissionalSelecionado == null)
            {
                MessageBox.Show("Profissional não encontrado.");
                return;
            }

            using var db = new MusicStationContext();

            var profissional = db.Profissionais
                .FirstOrDefault(p => p.Id == profissionalSelecionado.Id);

            if (profissional == null)
            {
                MessageBox.Show("Profissional não encontrado no banco.");
                return;
            }

            string acao = profissional.Ativo ? "desativar" : "ativar";

            var resultado = MessageBox.Show(
                $"Deseja realmente {acao} o profissional {profissional.Nome}?",
                "Confirmação",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (resultado != MessageBoxResult.Yes)
                return;

            profissional.Ativo = !profissional.Ativo;

            db.SaveChanges();

            MessageBox.Show(
                profissional.Ativo
                    ? "Profissional ativado com sucesso."
                    : "Profissional desativado com sucesso.");

            CarregarProfissionais();
        }
    }

    public class ProfissionalViewModel
    {
        public int Id { get; set; }

        public string Nome { get; set; } = "";

        public string Email { get; set; } = "";

        public string NomeUsuario { get; set; } = "";

        public string Telefone { get; set; } = "";

        public string Especialidade { get; set; } = "";

        public string NomeEmpresa { get; set; } = "";

        public string TipoProfissional { get; set; } = "";

        public string Status { get; set; } = "";

        public string DataCadastro { get; set; } = "";

        public string Endereco { get; set; } = "";

        public string Iniciais { get; set; } = "";
        

        public string CorPerfil { get; set; } = "";
    }
}