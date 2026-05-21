using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using wpf_projeto_integrador.Data;

namespace wpf_pi.Views
{
    public partial class UsuariosView : UserControl
    {
        private List<UsuarioListaViewModel> usuarios = new();

        public UsuariosView()
        {
            InitializeComponent();
            CarregarDados();
        }

        private void CarregarDados()
        {
            using var db = new MusicStationContext();

            txtTotalUsuarios.Text = db.Usuarios.Count().ToString();
            txtTotalAdministradores.Text = db.Administradores.Count().ToString();
            txtTotalClientes.Text = db.Clientes.Count().ToString();
            txtTotalProfissionais.Text = db.Profissionais.Count().ToString();
            txtTotalEmpresas.Text = db.Empresas.Count().ToString();

            usuarios = db.Usuarios
                .Select(u => new UsuarioListaViewModel
                {
                    Id = u.Id,
                    Nome = u.Nome,
                    Email = u.Email,
                    NomeUsuario = u.NomeUsuario,
                    Status = u.Ativo ? "Ativo" : "Inativo",
                    DataCadastro = u.DataCriacao.ToString("dd/MM/yyyy"),
                })
                .ToList();


            string[] cores =
            {
                "#7C3AED", // roxo
                "#FBBF24", // amarelo
                "#EC4899", // rosa
                "#3B82F6", // azul
                "#0F8B8D", // verde/azulado
            };

            int index = 0;


            foreach (var usuario in usuarios)
            {
                usuario.Iniciais = string.Join("",
                    (usuario.Nome ?? "")
                        .Split(' ')
                        .Where(p => !string.IsNullOrWhiteSpace(p))
                        .Take(2)
                        .Select(p => p[0]))
                    .ToUpper();
                usuario.CorPerfil = cores[index % cores.Length];

                index++;

            }

            foreach (var usuario in usuarios)
            {
                if (db.Administradores.Any(a => a.Id == usuario.Id))
                    usuario.Tipo = "Administrador";
                else if (db.Clientes.Any(c => c.Id == usuario.Id))
                    usuario.Tipo = "Cliente";
                else if (db.Profissionais.Any(p => p.Id == usuario.Id))
                    usuario.Tipo = "Profissional";
                else if (db.Empresas.Any(e => e.Id == usuario.Id))
                    usuario.Tipo = "Empresa";
                else
                    usuario.Tipo = "Usuário";
            }

            dgUsuarios.ItemsSource = usuarios;
        }

        private void AplicarFiltros()
        {
            var lista = usuarios.AsEnumerable();

            string busca = txtBusca.Text.ToLower();

            if (!string.IsNullOrWhiteSpace(busca))
            {
                lista = lista.Where(u =>
                    u.Nome.ToLower().Contains(busca) ||
                    u.Email.ToLower().Contains(busca) ||
                    u.NomeUsuario.ToLower().Contains(busca));
            }

            string tipo = ((ComboBoxItem)cmbTipo.SelectedItem).Content.ToString();

            if (tipo != "Todos")
                lista = lista.Where(u => u.Tipo == tipo);

            string status = ((ComboBoxItem)cmbStatus.SelectedItem).Content.ToString();

            if (status != "Todos")
                lista = lista.Where(u => u.Status == status);

            dgUsuarios.ItemsSource = lista.ToList();
        }

        private void txtBusca_TextChanged(object sender, TextChangedEventArgs e)
        {
            AplicarFiltros();
        }

        private void Filtro_Changed(object sender, SelectionChangedEventArgs e)
        {
            if (dgUsuarios != null)
                AplicarFiltros();
        }

        private void BtnLimparFiltros_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            txtBusca.Text = "";
            cmbTipo.SelectedIndex = 0;
            cmbStatus.SelectedIndex = 0;
            dgUsuarios.ItemsSource = usuarios;
        }
    }

    public class UsuarioListaViewModel
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string NomeUsuario { get; set; }
        public string Tipo { get; set; }
        public string Status { get; set; }
        public string DataCadastro { get; set; }

        public string Iniciais { get; set; } = " ";

        public string CorPerfil { get; set; } = "#7C3AED";

    }
}