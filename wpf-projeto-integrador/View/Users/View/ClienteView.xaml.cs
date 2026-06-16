using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using wpf_projeto_integrador.Data;
using wpf_projeto_integrador.View.Users.ControleDeUsuarios;


namespace wpf_projeto_integrador.View.Users.View
{
    /// <summary>
    /// Interação lógica para ClienteView.xam
    /// </summary>
    public partial class ClienteView : UserControl
    {
        private List<ClienteViewModel> clientes = new();
        public ClienteView()
        {
            InitializeComponent();
            CarregarClientes();
        }

        private void CarregarClientes()
        {
            using var db = new MusicStationContext();

            txtTotalClientes.Text = db.Clientes.Count().ToString();

            txtTotalAtivos.Text = db.Clientes
                .Count(c => c.Ativo)
                .ToString();

            txtTotalInativos.Text = db.Clientes
                .Count(c => !c.Ativo)
                .ToString();

            var dataLimite = System.DateTime.Now.AddDays(-30);

            txtTotalNovos.Text = db.Clientes
                .Count(c => c.DataCriacao >= dataLimite)
                .ToString();

            clientes = db.Clientes
                .Select(c => new ClienteViewModel
                {
                    Id = c.Id,
                    Nome = c.Nome,
                    Email = c.Email,
                    NomeUsuario = c.NomeUsuario,

                    Telefone = c.Telefone ?? "Não informado",
                    Rua = c.Rua ?? "Não informado",
                    Numero = c.Numero ?? "Não informado",
                    Cidade = c.Cidade ?? "Não informado",

                    Status = c.Ativo
                        ? "Ativo"
                        : "Inativo",

                    DataCadastro = c.DataCriacao.ToString("dd/MM/yyyy")
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

            foreach (var cliente in clientes)
            {
                cliente.Iniciais = string.Join("",
                    (cliente.Nome ?? "")
                        .Split(' ')
                        .Where(p => !string.IsNullOrWhiteSpace(p))
                        .Take(2)
                        .Select(p => p[0]))
                    .ToUpper();

                cliente.CorPerfil = cores[index % cores.Length];

                index++;
            }

            dgClientes.ItemsSource = clientes;
        }

        private void AplicarFiltros()
        {
            var lista = clientes.AsEnumerable();

            string busca = txtBusca.Text.ToLower();

            if (!string.IsNullOrWhiteSpace(busca))
            {
                lista = lista.Where(c =>
                    c.Nome.ToLower().Contains(busca) ||
                    c.Email.ToLower().Contains(busca) ||
                    c.NomeUsuario.ToLower().Contains(busca) ||
                    c.Telefone.ToLower().Contains(busca) ||
                    c.Cidade.ToLower().Contains(busca));
            }

            string status =
                ((ComboBoxItem)cmbStatus.SelectedItem)
                .Content
                .ToString();

            if (status != "Todos")
            {
                lista = lista.Where(c =>
                    c.Status == status);
            }

            string cidade =
                ((ComboBoxItem)cmbCidade.SelectedItem)
                .Content
                .ToString();

            if (cidade != "Todas")
            {
                lista = lista.Where(c =>
                    c.Cidade == cidade);
            }

            dgClientes.ItemsSource = lista.ToList();
        }

        private void txtBusca_TextChanged(object sender, TextChangedEventArgs e)
        {
            AplicarFiltros();
        }

        private void Filtro_Changed(object sender, SelectionChangedEventArgs e)
        {
            if (dgClientes != null)
            {
                AplicarFiltros();
            }
        }

        private void BtnNovoCliente_Click(object sender, RoutedEventArgs e)
        {
            var telaPrincipal = (FormMenu)Window.GetWindow(this);

            telaPrincipal.AbrirTela(new ClienteFormView());
        }

        private void BtnVisualizar_Click(object sender, RoutedEventArgs e)
        {
            var cliente = (sender as Button)?.DataContext as ClienteViewModel;

            if (cliente == null)
            {
                MessageBox.Show("Cliente não encontrado.");
                return;
            }

            MessageBox.Show(
                $"Nome: {cliente.Nome}\n" +
                $"Usuário: {cliente.NomeUsuario}\n" +
                $"Email: {cliente.Email}\n" +
                $"Telefone: {cliente.Telefone}\n" +
                $"Status: {cliente.Status}\n" +
                $"Rua: {cliente.Rua}\n" +
                $"Número: {cliente.Numero}\n" +
                $"Cidade: {cliente.Cidade}\n" +
                $"Cadastro: {cliente.DataCadastro}",
                "Detalhes do cliente");
        }

        private void BtnEditar_Click(object sender, RoutedEventArgs e)
        {
            var cliente = (sender as Button)?.DataContext as ClienteViewModel;

            if (cliente == null)
            {
                MessageBox.Show("Cliente não encontrado.");
                return;
            }

            var telaPrincipal = (FormMenu)Window.GetWindow(this);

            telaPrincipal.AbrirTela(new ClienteFormView(cliente));
        }

        private void BtnDesativar_Click(object sender, RoutedEventArgs e)
        {
            var clienteSelecionado =
                (sender as Button)?.DataContext as ClienteViewModel;

            if (clienteSelecionado == null)
            {
                MessageBox.Show("Cliente não encontrado.");
                return;
            }

            using var db = new MusicStationContext();

            var cliente = db.Clientes
                .FirstOrDefault(c => c.Id == clienteSelecionado.Id);

            if (cliente == null)
            {
                MessageBox.Show("Cliente não encontrado no banco.");
                return;
            }

            string acao = cliente.Ativo ? "desativar" : "ativar";

            var resultado = MessageBox.Show(
                $"Deseja realmente {acao} o cliente {cliente.Nome}?",
                "Confirmação",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (resultado != MessageBoxResult.Yes)
                return;

            cliente.Ativo = !cliente.Ativo;

            db.SaveChanges();

            MessageBox.Show(
                cliente.Ativo
                    ? "Cliente ativado com sucesso."
                    : "Cliente desativado com sucesso.");

            CarregarClientes();
        }
    }

    public class ClienteViewModel
    {
        public int Id { get; set; }

        public string Nome { get; set; } = "";

        public string Email { get; set; } = "";

        public string NomeUsuario { get; set; } = "";

        public string Telefone { get; set; } = "";

        public string Rua { get; set; } = "";

        public string Numero { get; set; } = "";

        public string Cidade { get; set; } = "";

        public string Status { get; set; } = "";

        public string DataCadastro { get; set; } = "";

        public string Iniciais { get; set; } = "";

        public string CorPerfil { get; set; } = "";
    }




}
