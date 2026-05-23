using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using wpf_projeto_integrador.Data;
using wpf_projeto_integrador.Models;

namespace wpf_projeto_integrador.View.Users
{
    public partial class AdministradorView : UserControl
    {
        private List<AdministradorViewModel> administradores = new();

        public AdministradorView()
        {
            InitializeComponent();
            CarregarDados();
        }

        private void CarregarDados()
        {
            using var db = new MusicStationContext();

            // =========================
            // CARDS
            // =========================

            txtTotalAdministradores.Text =
                db.Administradores.Count().ToString();

            txtToalAlto.Text =
                db.Administradores.Count(a => a.NivelAcesso == Administrador.NivelAcessoEnum.Alto)
                .ToString();

            txtTotalMedio.Text =
                db.Administradores.Count(a => a.NivelAcesso == Administrador.NivelAcessoEnum.Medio)
                .ToString();

            txtTotalBaixo.Text =
                db.Administradores.Count(a => a.NivelAcesso == Administrador.NivelAcessoEnum.Baixo)
                .ToString();

            //txtotalAlto.Text =
            //    db.Administradores.Count(a => a.Ativo)
            //    .ToString();

            // =========================
            // LISTA
            // =========================

            administradores = db.Administradores
                .Select(a => new AdministradorViewModel
                {
                    Id = a.Id,
                    Nome = a.Nome,
                    Email = a.Email,
                    NomeUsuario = a.NomeUsuario,

                    NivelAcesso = a.NivelAcesso.ToString(),

                    Status = a.Ativo
                        ? "Ativo"
                        : "Inativo",

                    DataCadastro =
                        a.DataCriacao.ToString("dd/MM/yyyy")
                })
                .ToList();

            // =========================
            // INICIAIS + CORES
            // =========================

            string[] cores =
            {
                "#7C3AED",
                "#EC4899",
                "#3B82F6",
                "#F59E0B",
                "#0F8B8D"
            };

            int index = 0;

            foreach (var adm in administradores)
            {
                adm.Iniciais = string.Join("",
                    (adm.Nome ?? "")
                        .Split(' ')
                        .Where(p => !string.IsNullOrWhiteSpace(p))
                        .Take(2)
                        .Select(p => p[0]))
                    .ToUpper();

                adm.CorPerfil = cores[index % cores.Length];

                index++;
            }

            dgAdministradores.ItemsSource = administradores;
        }

        // =====================================
        // FILTROS
        // =====================================

        private void AplicarFiltros()
        {
            var lista = administradores.AsEnumerable();

            string busca = txtBusca.Text.ToLower();

            if (!string.IsNullOrWhiteSpace(busca))
            {
                lista = lista.Where(a =>
                    a.Nome.ToLower().Contains(busca) ||
                    a.Email.ToLower().Contains(busca) ||
                    a.NomeUsuario.ToLower().Contains(busca));
            }

            string nivel =  ((ComboBoxItem)cmbTipo.SelectedItem).Content.ToString();

            if (nivel != "Todos")
            {
                lista = lista.Where(a =>
                    a.NivelAcesso == nivel);
            }

            string status =
                ((ComboBoxItem)cmbStatus.SelectedItem)
                .Content
                .ToString();

            if (status != "Todos")
            {
                lista = lista.Where(a =>
                    a.Status == status);
            }

            dgAdministradores.ItemsSource =lista.ToList();
        }

        private void txtBusca_TextChanged( object sender,TextChangedEventArgs e)
        {
            AplicarFiltros();
        }

        private void Filtro_Changed(object sender,SelectionChangedEventArgs e)
        {
            if (dgAdministradores != null)
            {
                AplicarFiltros();
            }
        }

        // =====================================
        // BOTÕES
        // =====================================

        private void BtnNovoAdministrador_Click(
            object sender,
            RoutedEventArgs e)
        {
            MessageBox.Show(
                "Abrir tela de cadastro");
        }

        private void BtnLimparFiltros_Click(
            object sender,
            RoutedEventArgs e)
        {
            txtBusca.Text = "";

            cmbTipo.SelectedIndex = 0;

            cmbStatus.SelectedIndex = 0;

            dgAdministradores.ItemsSource =
                administradores;
        }

        private void txtBusca_TextChanged_1(object sender, TextChangedEventArgs e)
        {

        }
    }

    // =========================================
    // VIEWMODEL
    // =========================================

    public class AdministradorViewModel
    {
        public int Id { get; set; }

        public string Nome { get; set; } = "";

        public string Email { get; set; } = "";

        public string NomeUsuario { get; set; } = "";

        public string NivelAcesso { get; set; } = "";

        public string Status { get; set; } = "";

        public string DataCadastro { get; set; } = "";

        public string Iniciais { get; set; } = "";

        public string CorPerfil { get; set; } = "";
    }
}