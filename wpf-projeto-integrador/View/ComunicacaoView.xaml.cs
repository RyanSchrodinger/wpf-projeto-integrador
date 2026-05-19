using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using wpf_projeto_integrador.Data;
using wpf_projeto_integrador.Models;

namespace wpf_projeto_integrador.Views
{
    public partial class ComunicacaoView : UserControl
    {
        private readonly int _usuarioLogadoId;
        private Chat _chatAtual;
        private Administrador _adminSelecionado;

        public ComunicacaoView(int usuarioLogadoId)
        {
            InitializeComponent();

            _usuarioLogadoId = usuarioLogadoId;

            CarregarAdministradores();
        }

        private void CarregarAdministradores(string busca = "")
        {
            using var db = new MusicStationContext();

            var administradores = db.Administradores
                .Where(a =>
                    string.IsNullOrEmpty(busca) ||
                    a.Nome.Contains(busca) ||
                    a.NomeUsuario.Contains(busca) ||
                    a.Email.Contains(busca))
                .Select(a => new
                {
                    a.Id,
                    a.Nome,
                    a.Email,
                    a.NomeUsuario,
                    Iniciais = a.Nome.Substring(0, 1).ToUpper()
                })
                .ToList();

            ListaAdministradores.ItemsSource = administradores;
        }

        private void TxtBuscarAdm_TextChanged(object sender, TextChangedEventArgs e)
        {
            CarregarAdministradores(TxtBuscarAdm.Text.Trim());
        }

        private void ListaAdministradores_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ListaAdministradores.SelectedItem == null)
                return;

            dynamic admin = ListaAdministradores.SelectedItem;

            int adminId = admin.Id;

            using var db = new MusicStationContext();

            _adminSelecionado = db.Administradores
                .FirstOrDefault(a => a.Id == adminId);

            if (_adminSelecionado == null)
                return;

            TxtNomeContato.Text = _adminSelecionado.Nome;
            TxtIniciaisTopo.Text = _adminSelecionado.Nome.Substring(0, 1).ToUpper();

            _chatAtual = db.Chats
                .Include(c => c.Mensagens)
                .FirstOrDefault(c =>
                    (c.Usuario1Id == _usuarioLogadoId && c.Usuario2Id == _adminSelecionado.Id) ||
                    (c.Usuario1Id == _adminSelecionado.Id && c.Usuario2Id == _usuarioLogadoId));

            if (_chatAtual == null)
            {
                _chatAtual = new Chat
                {
                    Usuario1Id = _usuarioLogadoId,
                    Usuario2Id = _adminSelecionado.Id,
                    DataCriacao = DateTime.Now
                };

                db.Chats.Add(_chatAtual);
                db.SaveChanges();
            }

            CarregarMensagens();
        }

        private void CarregarMensagens()
        {
            if (_chatAtual == null)
                return;

            using var db = new MusicStationContext();

            var mensagens = db.Mensagens
                .Where(m => m.ChatId == _chatAtual.Id)
                .OrderBy(m => m.DataEnvio)
                .Select(m => new
                {
                    m.Texto,
                    Hora = m.DataEnvio.ToString("HH:mm"),
                    MinhaMensagem = m.RemetenteId == _usuarioLogadoId
                })
                .ToList();

            ListaMensagens.ItemsSource = mensagens;

            ScrollMensagens.ScrollToEnd();
        }

        private void BtnEnviar_Click(object sender, RoutedEventArgs e)
        {
            if (_chatAtual == null)
            {
                MessageBox.Show("Selecione um administrador primeiro.");
                return;
            }

            if (string.IsNullOrWhiteSpace(TxtMensagem.Text))
                return;

            using var db = new MusicStationContext();

            var mensagem = new Mensagem
            {
                ChatId = _chatAtual.Id,
                RemetenteId = _usuarioLogadoId,
                Texto = TxtMensagem.Text.Trim(),
                DataEnvio = DateTime.Now,
                Visualizada = false
            };

            db.Mensagens.Add(mensagem);
            db.SaveChanges();

            TxtMensagem.Clear();

            CarregarMensagens();
        }
    }
}