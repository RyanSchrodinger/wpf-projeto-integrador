using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using wpf_projeto_integrador.Models;

namespace wpf_projeto_integrador
{
    public partial class FormMenu : Window
    {
        private bool menuAberto = true;

        public FormMenu()
        {
            InitializeComponent();
            MostrarDashboard();
        }

        private void BtnToggleMenu_Click(object sender, RoutedEventArgs e)
        {
            if (menuAberto)
            {
                MenuColumn.Width = new GridLength(90);

                TxtLogoMusic.Visibility = Visibility.Collapsed;
                TxtLogoStation.Visibility = Visibility.Collapsed;
                TxtPainelAdm.Visibility = Visibility.Collapsed;

                menuAberto = false;
            }
            else
            {
                MenuColumn.Width = new GridLength(250);

                TxtLogoMusic.Visibility = Visibility.Visible;
                TxtLogoStation.Visibility = Visibility.Visible;
                TxtPainelAdm.Visibility = Visibility.Visible;

                menuAberto = true;
            }

            AtualizarTextoMenu();
        }

        private void AtualizarTextoMenu()
        {
            if (menuAberto)
            {
                BtnMenuDashboard.Content = "🏠  Dashboard";
                BtnMenuPessoas.Content = "👥  Gestão de Pessoas";
                BtnMenuFinanceiro.Content = "💰  Gestão Financeira";
                BtnMenuServicos.Content = "🎵  Gestão de Serviços";
                BtnMenuLocacoes.Content = "📦  Gestão de Locações";
                BtnMenuComunicacao.Content = "💬  Comunicação";
                BtnMenuConfiguracoes.Content = "⚙️  Configurações";
                BtnMenuLogout.Content = "🚪  Fazer logout";
            }
            else
            {
                BtnMenuDashboard.Content = "🏠";
                BtnMenuPessoas.Content = "👥";
                BtnMenuFinanceiro.Content = "💰";
                BtnMenuServicos.Content = "🎵";
                BtnMenuLocacoes.Content = "📦";
                BtnMenuComunicacao.Content = "💬";
                BtnMenuConfiguracoes.Content = "⚙️";
                BtnMenuLogout.Content = "🚪";
            }
        }

        private void LimparConteudo(string titulo)
        {
            TxtTituloPagina.Text = titulo;
            AreaConteudo.Children.Clear();
        }

        private TextBlock CriarTitulo(string texto)
        {
            return new TextBlock
            {
                Text = texto,
                Foreground = Brushes.White,
                FontSize = 24,
                FontWeight = FontWeights.Bold,
                Margin = new Thickness(8, 0, 0, 18)
            };
        }

        private Button CriarCard(string texto)
        {
            return new Button
            {
                Content = texto,
                Height = 95,
                Margin = new Thickness(8),
                Background = new SolidColorBrush(Color.FromRgb(32, 32, 56)),
                Foreground = Brushes.White,
                BorderThickness = new Thickness(0),
                FontSize = 16,
                FontWeight = FontWeights.SemiBold,
                Cursor = System.Windows.Input.Cursors.Hand
            };
        }

        private Button CriarBotaoAcao(string texto)
        {
            return new Button
            {
                Content = texto,
                Height = 34,
                Padding = new Thickness(14, 0, 14, 0),
                Margin = new Thickness(4, 0, 0, 0),
                Background = new SolidColorBrush(Color.FromRgb(108, 61, 255)),
                Foreground = Brushes.White,
                BorderThickness = new Thickness(0),
                FontSize = 12,
                FontWeight = FontWeights.SemiBold,
                Cursor = System.Windows.Input.Cursors.Hand
            };
        }

        private void MostrarDashboard()
        {
            LimparConteudo("Dashboard");

            AreaConteudo.Children.Add(CriarTitulo("Resumo geral"));

            UniformGrid cardsResumo = new UniformGrid
            {
                Columns = 4
            };

            cardsResumo.Children.Add(CriarCardResumo("👥", "Usuários", "128"));
            cardsResumo.Children.Add(CriarCardResumo("🎸", "Instrumentos", "42"));
            cardsResumo.Children.Add(CriarCardResumo("📦", "Locações ativas", "12"));
            cardsResumo.Children.Add(CriarCardResumo("💰", "Pagamentos pendentes", "8"));

            AreaConteudo.Children.Add(cardsResumo);

            //AreaConteudo.Children.Add(new TextBlock
            //{
            //    Text = "Acesso rápido",
            //    Foreground = Brushes.White,
            //    FontSize = 22,
            //    FontWeight = FontWeights.Bold,
            //    Margin = new Thickness(8, 28, 0, 12)
            //});

            //UniformGrid acessoRapido = new UniformGrid
            //{
            //    Columns = 3
            //};

            //Button btnUsuarios = CriarCard("👤  Usuários");
            //btnUsuarios.Click += BtnUsuarios_Click;

            //Button btnClientes = CriarCard("👥  Clientes");
            //Button btnEmpresas = CriarCard("🏢  Empresas");
            //Button btnProfissionais = CriarCard("🧑‍🎤  Profissionais");
            //Button btnInstrumentos = CriarCard("🎸  Instrumentos");
            //Button btnLocacoes = CriarCard("📦  Locações");

            //acessoRapido.Children.Add(btnUsuarios);
            //acessoRapido.Children.Add(btnClientes);
            //acessoRapido.Children.Add(btnEmpresas);
            //acessoRapido.Children.Add(btnProfissionais);
            //acessoRapido.Children.Add(btnInstrumentos);
            //acessoRapido.Children.Add(btnLocacoes);

            //AreaConteudo.Children.Add(acessoRapido);
        }

        private Border CriarCardResumo(string iconeTexto, string titulo, string valor)
        {
            Border card = new Border
            {
                Background = new SolidColorBrush(Color.FromRgb(32, 32, 56)),
                CornerRadius = new CornerRadius(24),
                Padding = new Thickness(22),
                Margin = new Thickness(8)
            };

            StackPanel painel = new StackPanel();

            painel.Children.Add(new TextBlock
            {
                Text = iconeTexto,
                FontSize = 30
            });

            painel.Children.Add(new TextBlock
            {
                Text = titulo,
                Foreground = new SolidColorBrush(Color.FromRgb(184, 184, 209)),
                Margin = new Thickness(0, 10, 0, 0)
            });

            painel.Children.Add(new TextBlock
            {
                Text = valor,
                Foreground = Brushes.White,
                FontSize = 30,
                FontWeight = FontWeights.Bold
            });

            card.Child = painel;

            return card;
        }

        private void MostrarCards(string titulo, params string[] opcoes)
        {
            LimparConteudo(titulo);

            AreaConteudo.Children.Add(CriarTitulo(titulo));

            UniformGrid grid = new UniformGrid
            {
                Columns = 3
            };

            foreach (string opcao in opcoes)
            {
                Button card = CriarCard(opcao);

                if (opcao.Contains("Usuários"))
                {
                    card.Click += BtnUsuarios_Click;
                }

                grid.Children.Add(card);
            }

            AreaConteudo.Children.Add(grid);
        }

        private void BtnDashboard_Click(object sender, RoutedEventArgs e)
        {
            MostrarDashboard();
        }

        private void BtnPessoas_Click(object sender, RoutedEventArgs e)
        {
            MostrarCards("Gestão de Pessoas",
                "👤 Usuários",
                "🛡️ Administradores",
                "👥 Clientes",
                "🧑‍🎤 Profissionais",
                "🏢 Empresas");
        }

        private void BtnFinanceiro_Click(object sender, RoutedEventArgs e)
        {
            MostrarCards("Gestão Financeira",
                "💰 Pagamentos",
                "📊 Relatórios",
                "🧾 Pedidos",
                "💳 Formas de pagamento");
        }

        private void BtnServicos_Click(object sender, RoutedEventArgs e)
        {
            MostrarCards("Gestão de Serviços",
                "🎵 Serviços",
                "🎸 Instrumentos",
                "🧑‍🎤 Profissionais",
                "🏷️ Categorias");
        }

        private void BtnLocacoes_Click(object sender, RoutedEventArgs e)
        {
            MostrarCards("Gestão de Locações",
                "📦 Locações",
                "➕ Nova locação",
                "🔁 Devoluções",
                "⏳ Pendentes");
        }

        private void BtnComunicacao_Click(object sender, RoutedEventArgs e)
        {
            MostrarCards("Central de Comunicação",
                "💬 Chats",
                "📨 Mensagens",
                "📢 Avisos");
        }

        private void BtnConfiguracoes_Click(object sender, RoutedEventArgs e)
        {
            MostrarCards("Configurações",
                "⚙️ Perfil do ADM",
                "🔐 Segurança",
                "🎨 Aparência");
        }

        private void BtnUsuarios_Click(object sender, RoutedEventArgs e)
        {
            LimparConteudo("Usuários");

            DockPanel topo = new DockPanel
            {
                Margin = new Thickness(8, 0, 8, 18)
            };

            TextBlock titulo = CriarTitulo("Usuários cadastrados");

            Button btnNovo = new Button
            {
                Content = "➕ Novo usuário",
                Height = 40,
                Padding = new Thickness(18, 0, 18, 0),
                Background = new SolidColorBrush(Color.FromRgb(108, 61, 255)),
                Foreground = Brushes.White,
                BorderThickness = new Thickness(0),
                FontWeight = FontWeights.SemiBold,
                Cursor = System.Windows.Input.Cursors.Hand
            };

            DockPanel.SetDock(btnNovo, Dock.Right);

            topo.Children.Add(btnNovo);
            topo.Children.Add(titulo);

            AreaConteudo.Children.Add(topo);

            AreaConteudo.Children.Add(CriarCardUsuario(
                "Ryan Ronald",
                "@ryan",
                "ryan@email.com",
                "Administrador"));

            AreaConteudo.Children.Add(CriarCardUsuario(
                "Maria Silva",
                "@maria",
                "maria@email.com",
                "Cliente"));

            AreaConteudo.Children.Add(CriarCardUsuario(
                "Studio Music LTDA",
                "@studiomusic",
                "contato@studio.com",
                "Empresa"));

            AreaConteudo.Children.Add(CriarCardUsuario(
                "João Pereira",
                "@joaoguitar",
                "joao@email.com",
                "Profissional"));
        }

        private Border CriarCardUsuario(string nome, string usuario, string email, string tipo)
        {
            Border card = new Border
            {
                Background = new SolidColorBrush(Color.FromRgb(32, 32, 56)),
                CornerRadius = new CornerRadius(22),
                Padding = new Thickness(20),
                Margin = new Thickness(8, 8, 8, 8)
            };

            Grid grid = new Grid();

            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(60) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(260) });

            TextBlock icone = new TextBlock
            {
                Text = "👤",
                FontSize = 32,
                VerticalAlignment = VerticalAlignment.Center
            };

            StackPanel info = new StackPanel
            {
                VerticalAlignment = VerticalAlignment.Center
            };

            info.Children.Add(new TextBlock
            {
                Text = nome,
                Foreground = Brushes.White,
                FontSize = 18,
                FontWeight = FontWeights.Bold
            });

            info.Children.Add(new TextBlock
            {
                Text = $"{usuario} • {email}",
                Foreground = new SolidColorBrush(Color.FromRgb(184, 184, 209)),
                FontSize = 13,
                Margin = new Thickness(0, 4, 0, 0)
            });

            info.Children.Add(new TextBlock
            {
                Text = $"Tipo: {tipo}",
                Foreground = new SolidColorBrush(Color.FromRgb(244, 196, 48)),
                FontSize = 13,
                Margin = new Thickness(0, 4, 0, 0)
            });

            StackPanel acoes = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                HorizontalAlignment = HorizontalAlignment.Right,
                VerticalAlignment = VerticalAlignment.Center
            };

            acoes.Children.Add(CriarBotaoAcao("Editar"));
            acoes.Children.Add(CriarBotaoAcao("Detalhes"));
            acoes.Children.Add(CriarBotaoAcao("Excluir"));

            Grid.SetColumn(icone, 0);
            Grid.SetColumn(info, 1);
            Grid.SetColumn(acoes, 2);

            grid.Children.Add(icone);
            grid.Children.Add(info);
            grid.Children.Add(acoes);

            card.Child = grid;

            return card;
        }

        private void BtnLogout_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Logout realizado!");
        }
    }
}