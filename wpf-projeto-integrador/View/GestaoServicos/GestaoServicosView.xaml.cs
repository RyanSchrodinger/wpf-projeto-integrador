using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using wpf_projeto_integrador.Services.GestaoServicos;
using wpf_projeto_integrador.ViewModels.GestaoServicos;

namespace wpf_projeto_integrador.View.GestaoServicos
{
    public partial class GestaoServicosView : UserControl
    {
        // Serviço responsável por consultar e tratar os dados.
        private readonly GestaoServicosService _gestaoServicosService =
            new();

        // Cultura utilizada para nomes dos meses e números.
        private readonly CultureInfo _cultura =
            new("pt-BR");

        // Lista com os resultados já filtrados.
        private List<ServicoPedidoItemViewModel> _itensFiltrados =
            new();

        // Quantidade de linhas mostradas por página.
        private const int ItensPorPagina = 10;

        // Página atual do DataGrid.
        private int _paginaAtual = 1;

        // Quantidade total de páginas.
        private int _totalPaginas = 1;

        // Evita que os eventos dos filtros sejam executados
        // enquanto a tela está sendo inicializada.
        private bool _telaInicializada;

        public GestaoServicosView()
        {
            InitializeComponent();

            CarregarCombos();

            _telaInicializada = true;

            CarregarGestaoServicos();
        }

        /// <summary>
        /// Carrega os dados dos ComboBoxes.
        /// </summary>
        private void CarregarCombos()
        {
            try
            {
                cmbStatus.ItemsSource =
                    _gestaoServicosService.ObterStatus();

                cmbStatus.SelectedIndex = 0;

                cmbPeriodo.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                ExibirErro(
                    "Não foi possível carregar os filtros.",
                    ex);
            }
        }

        /// <summary>
        /// Consulta os dados usando os filtros atuais.
        /// </summary>
        private void CarregarGestaoServicos(
            bool forcarAtualizacao = false)
        {
            try
            {
                string busca =
                    txtBusca.Text?.Trim() ??
                    string.Empty;

                string status =
                    cmbStatus.SelectedItem?.ToString() ??
                    "Todos";

                string periodo =
                    ObterPeriodoSelecionado();

                _itensFiltrados =
                    _gestaoServicosService
                        .ObterServicosPedidosFiltrados(
                            busca,
                            status,
                            periodo,
                            forcarAtualizacao);

                _paginaAtual = 1;

                AtualizarTela();
            }
            catch (Exception ex)
            {
                ExibirErro(
                    "Não foi possível carregar a Gestão de Serviços.",
                    ex);
            }
        }

        /// <summary>
        /// Retorna o período atualmente selecionado.
        /// </summary>
        private string ObterPeriodoSelecionado()
        {
            return (cmbPeriodo.SelectedItem as ComboBoxItem)?
                .Content?
                .ToString()
                ?? "Todos";
        }

        /// <summary>
        /// Atualiza todos os elementos visuais da tela.
        /// </summary>
        private void AtualizarTela()
        {
            AtualizarPaginacao();
            AtualizarCards();
            AtualizarGraficoMensal();
        }

        /// <summary>
        /// Atualiza os quatro cards da Visão Geral.
        /// </summary>
        private void AtualizarCards()
        {
            try
            {
                int quantidadeServicosAtivos =
                    _gestaoServicosService
                        .ObterQuantidadeServicosAtivos();

                int pedidosPendentes =
                    _itensFiltrados.Count(item =>
                        StatusIgual(
                            item,
                            "Pendente"));

                int emAndamento =
                    _itensFiltrados.Count(item =>
                        StatusIgual(
                            item,
                            "Em andamento"));

                decimal mediaAvaliacoes =
                    _gestaoServicosService
                        .ObterMediaAvaliacoes();

                txtServicosAtivos.Text =
                    quantidadeServicosAtivos.ToString();

                txtPedidosPendentes.Text =
                    pedidosPendentes.ToString();

                txtEmAndamento.Text =
                    emAndamento.ToString();

                txtMediaAvaliacoes.Text =
                    mediaAvaliacoes.ToString(
                        "N1",
                        _cultura);
            }
            catch (Exception ex)
            {
                ExibirErro(
                    "Não foi possível atualizar os cards.",
                    ex);
            }
        }

        /// <summary>
        /// Atualiza os dados mostrados no DataGrid.
        /// </summary>
        private void AtualizarPaginacao()
        {
            int totalRegistros =
                _itensFiltrados.Count;

            _totalPaginas = (int)Math.Ceiling(
                totalRegistros /
                (double)ItensPorPagina);

            if (_totalPaginas < 1)
                _totalPaginas = 1;

            if (_paginaAtual > _totalPaginas)
                _paginaAtual = _totalPaginas;

            if (_paginaAtual < 1)
                _paginaAtual = 1;

            var registrosPagina =
                _itensFiltrados
                    .Skip(
                        (_paginaAtual - 1) *
                        ItensPorPagina)
                    .Take(ItensPorPagina)
                    .ToList();

            dgServicosPedidos.ItemsSource =
                registrosPagina;

            int primeiroRegistro =
                totalRegistros == 0
                    ? 0
                    : ((_paginaAtual - 1) *
                       ItensPorPagina) + 1;

            int ultimoRegistro =
                Math.Min(
                    _paginaAtual * ItensPorPagina,
                    totalRegistros);

            txtQuantidadeRegistros.Text =
                totalRegistros == 1
                    ? "1 registro"
                    : $"{totalRegistros} registros";

            txtPaginaAtual.Text =
                $"Página {_paginaAtual} de {_totalPaginas}";

            txtResumoPagina.Text =
                totalRegistros == 0
                    ? "Nenhum registro encontrado"
                    : $"Mostrando {primeiroRegistro} a " +
                      $"{ultimoRegistro} de " +
                      $"{totalRegistros} registros";

            bool possuiPaginaAnterior =
                _paginaAtual > 1;

            bool possuiProximaPagina =
                _paginaAtual < _totalPaginas;

            ConfigurarBotaoPaginacao(
                btnPaginaAnterior,
                possuiPaginaAnterior);

            ConfigurarBotaoPaginacao(
                btnProximaPagina,
                possuiProximaPagina);
        }

        /// <summary>
        /// Habilita ou desabilita visualmente
        /// os botões da paginação.
        /// </summary>
        private static void ConfigurarBotaoPaginacao(
            Button botao,
            bool habilitado)
        {
            botao.IsEnabled =
                habilitado;

            botao.Opacity =
                habilitado
                    ? 1
                    : 0.4;

            botao.Cursor =
                habilitado
                    ? Cursors.Hand
                    : Cursors.Arrow;
        }

        /// <summary>
        /// Atualiza o gráfico com a quantidade
        /// de serviços solicitados por mês.
        /// </summary>
        private void AtualizarGraficoMensal()
        {
            int anoAtual =
                DateTime.Today.Year;

            var meses = Enumerable
                .Range(1, 12)
                .Select(numeroMes => new
                {
                    Nome = new DateTime(
                            anoAtual,
                            numeroMes,
                            1)
                        .ToString(
                            "MMM",
                            _cultura)
                        .Replace(".", ""),

                    Quantidade =
                        _itensFiltrados.Count(item =>
                            item.DataReferencia.Year ==
                                anoAtual &&
                            item.DataReferencia.Month ==
                                numeroMes)
                })
                .ToList();

            graficoPedidosMensais.Series =
                new ISeries[]
                {
                    new ColumnSeries<int>
                    {
                        Name = "Serviços",

                        Values = meses
                            .Select(m => m.Quantidade)
                            .ToArray(),

                        Fill =
                            new SolidColorPaint(
                                SKColor.Parse(
                                    "#7C3AED")),

                        Stroke = null,

                        MaxBarWidth = 38,

                        Rx = 5,

                        Ry = 5,

                        DataLabelsSize = 10,

                        DataLabelsPosition =
                            LiveChartsCore.Measure
                                .DataLabelsPosition.Top,

                        DataLabelsPaint =
                            new SolidColorPaint(
                                SKColor.Parse(
                                    "#CBD5E1")),

                        DataLabelsFormatter =
                            ponto =>
                                ponto.Model > 0
                                    ? ponto.Model.ToString()
                                    : string.Empty
                    }
                };

            graficoPedidosMensais.XAxes =
                new[]
                {
                    new Axis
                    {
                        Labels = meses
                            .Select(m => m.Nome)
                            .ToArray(),

                        LabelsPaint =
                            new SolidColorPaint(
                                SKColor.Parse(
                                    "#CBD5E1")),

                        SeparatorsPaint =
                            new SolidColorPaint(
                                SKColor.Parse(
                                    "#1F2942"))
                            {
                                StrokeThickness = 1
                            },

                        TextSize = 11
                    }
                };

            graficoPedidosMensais.YAxes =
                new[]
                {
                    new Axis
                    {
                        LabelsPaint =
                            new SolidColorPaint(
                                SKColor.Parse(
                                    "#94A3B8")),

                        SeparatorsPaint =
                            new SolidColorPaint(
                                SKColor.Parse(
                                    "#27314D"))
                            {
                                StrokeThickness = 1
                            },

                        TextSize = 10,

                        MinLimit = 0,

                        MinStep = 1,

                        Labeler = valor =>
                            ((int)valor).ToString()
                    }
                };

            graficoPedidosMensais.LegendPosition =
                LiveChartsCore.Measure
                    .LegendPosition.Hidden;

            graficoPedidosMensais.TooltipTextPaint =
                new SolidColorPaint(
                    SKColors.White);

            graficoPedidosMensais
                .TooltipBackgroundPaint =
                new SolidColorPaint(
                    SKColor.Parse(
                        "#202744"));
        }

        /// <summary>
        /// Verifica o status ignorando letras
        /// maiúsculas e minúsculas.
        /// </summary>
        private static bool StatusIgual(
            ServicoPedidoItemViewModel item,
            string status)
        {
            return item.Status.Equals(
                status,
                StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Volta uma página.
        /// </summary>
        private void BtnPaginaAnterior_Click(
            object sender,
            RoutedEventArgs e)
        {
            if (_paginaAtual <= 1)
                return;

            _paginaAtual--;

            AtualizarPaginacao();
        }

        /// <summary>
        /// Avança uma página.
        /// </summary>
        private void BtnProximaPagina_Click(
            object sender,
            RoutedEventArgs e)
        {
            if (_paginaAtual >= _totalPaginas)
                return;

            _paginaAtual++;

            AtualizarPaginacao();
        }

        /// <summary>
        /// Repassa a rolagem do DataGrid
        /// para o ScrollViewer principal.
        /// </summary>
        private void DgServicosPedidos_PreviewMouseWheel(
            object sender,
            MouseWheelEventArgs e)
        {
            DependencyObject elemento =
                dgServicosPedidos;

            while (elemento != null)
            {
                elemento =
                    VisualTreeHelper.GetParent(
                        elemento);

                if (elemento is ScrollViewer scrollViewer)
                {
                    scrollViewer.ScrollToVerticalOffset(
                        scrollViewer.VerticalOffset -
                        e.Delta);

                    e.Handled = true;

                    return;
                }
            }
        }

        /// <summary>
        /// Executado sempre que o usuário
        /// digita no campo de busca.
        /// </summary>
        private void TxtBusca_TextChanged(
            object sender,
            TextChangedEventArgs e)
        {
            if (!_telaInicializada)
                return;

            CarregarGestaoServicos();
        }

        /// <summary>
        /// Executado quando status ou período mudam.
        /// </summary>
        private void Filtro_Changed(
            object sender,
            SelectionChangedEventArgs e)
        {
            if (!_telaInicializada)
                return;

            CarregarGestaoServicos();
        }

        /// <summary>
        /// Limpa o cache e atualiza os dados do banco.
        /// </summary>
        private void BtnAtualizar_Click(
            object sender,
            RoutedEventArgs e)
        {
            _gestaoServicosService.LimparCache();

            CarregarGestaoServicos(
                forcarAtualizacao: true);
        }

        /// <summary>
        /// Abre o drawer lateral.
        /// </summary>
        private void AbrirDrawer()
        {
            FundoDrawer.Visibility =
                Visibility.Visible;

            DrawerServico.Visibility =
                Visibility.Visible;

            var animacaoFundo =
                new DoubleAnimation
                {
                    From = 0,
                    To = 1,
                    Duration =
                        TimeSpan.FromMilliseconds(
                            180)
                };

            FundoDrawer.BeginAnimation(
                OpacityProperty,
                animacaoFundo);

            var animacaoDrawer =
                new DoubleAnimation
                {
                    From = 410,
                    To = 0,
                    Duration =
                        TimeSpan.FromMilliseconds(
                            260),

                    EasingFunction =
                        new CubicEase
                        {
                            EasingMode =
                                EasingMode.EaseOut
                        }
                };

            TransformDrawer.BeginAnimation(
                TranslateTransform.XProperty,
                animacaoDrawer);
        }

        /// <summary>
        /// Fecha o drawer lateral.
        /// </summary>
        private void FecharDrawer()
        {
            var animacaoFundo =
                new DoubleAnimation
                {
                    From = FundoDrawer.Opacity,
                    To = 0,
                    Duration =
                        TimeSpan.FromMilliseconds(
                            180)
                };

            FundoDrawer.BeginAnimation(
                OpacityProperty,
                animacaoFundo);

            var animacaoDrawer =
                new DoubleAnimation
                {
                    From = TransformDrawer.X,
                    To = 410,
                    Duration =
                        TimeSpan.FromMilliseconds(
                            220),

                    EasingFunction =
                        new CubicEase
                        {
                            EasingMode =
                                EasingMode.EaseIn
                        }
                };

            animacaoDrawer.Completed += (_, _) =>
            {
                DrawerServico.Visibility =
                    Visibility.Collapsed;

                FundoDrawer.Visibility =
                    Visibility.Collapsed;

                FundoDrawer.Opacity = 0;

                TransformDrawer.X = 410;
            };

            TransformDrawer.BeginAnimation(
                TranslateTransform.XProperty,
                animacaoDrawer);
        }

        /// <summary>
        /// Preenche o drawer com os dados selecionados.
        /// </summary>
        private void PreencherDrawer(
            ServicoPedidoItemViewModel item)
        {
            txtDrawerTitulo.Text =
                $"Pedido #{item.PedidoId}";

            txtDrawerCliente.Text =
                item.Cliente;

            txtDrawerServico.Text =
                item.Servico;

            txtDrawerProfissional.Text =
                item.Profissional;

            txtDrawerPrestador.Text =
                item.Prestador;

            txtDrawerStatus.Text =
                item.Status;

            txtDrawerValor.Text =
                item.ValorFormatado;

            txtDrawerData.Text =
                item.Data;

            txtDrawerObservacao.Text =
                string.IsNullOrWhiteSpace(
                    item.Observacao)
                    ? "Sem observações"
                    : item.Observacao;

            ConfigurarCorStatus(
                item.Status);
        }

        /// <summary>
        /// Configura as cores do status dentro do drawer.
        /// </summary>
        private void ConfigurarCorStatus(
            string status)
        {
            string corFundo;
            string corTexto;

            switch (status)
            {
                case "Em andamento":
                    corFundo = "#11315B";
                    corTexto = "#38BDF8";
                    break;

                case "Concluído":
                    corFundo = "#10382B";
                    corTexto = "#22C55E";
                    break;

                case "Cancelado":
                    corFundo = "#451927";
                    corTexto = "#EF4444";
                    break;

                default:
                    corFundo = "#3A3012";
                    corTexto = "#FACC15";
                    break;
            }

            var conversor =
                new BrushConverter();

            brdDrawerStatus.Background =
                (Brush)conversor.ConvertFromString(
                    corFundo)!;

            txtDrawerStatus.Foreground =
                (Brush)conversor.ConvertFromString(
                    corTexto)!;

            elipseDrawerStatus.Fill =
                (Brush)conversor.ConvertFromString(
                    corTexto)!;
        }

        /// <summary>
        /// Abre os detalhes do registro selecionado.
        /// </summary>
        private void BtnVisualizar_Click(
            object sender,
            RoutedEventArgs e)
        {
            var item =
                (sender as Button)?
                    .DataContext
                as ServicoPedidoItemViewModel;

            if (item == null)
            {
                MessageBox.Show(
                    "Serviço não encontrado.",
                    "Detalhes do serviço",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);

                return;
            }

            PreencherDrawer(item);

            AbrirDrawer();
        }

        private void BtnFecharDrawer_Click(
            object sender,
            RoutedEventArgs e)
        {
            FecharDrawer();
        }

        private void FundoDrawer_MouseLeftButtonDown(
            object sender,
            MouseButtonEventArgs e)
        {
            FecharDrawer();
        }

        /// <summary>
        /// Exibe mensagem padronizada de erro.
        /// </summary>
        private static void ExibirErro(
            string mensagem,
            Exception ex)
        {
            MessageBox.Show(
                $"{mensagem}\n\n{ex.InnerException?.Message ?? ex.Message}",
                "Erro",
                MessageBoxButton.OK,
                MessageBoxImage.Error);
        }
    }
}