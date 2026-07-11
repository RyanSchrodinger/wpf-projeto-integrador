using LiveChartsCore;
using LiveChartsCore.Measure;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using wpf_projeto_integrador.DTOs.Financeiro;
using wpf_projeto_integrador.ViewModels.Financeiro;

namespace wpf_projeto_integrador.View.Financeiro
{
    public partial class DashboardFinanceiroView : UserControl
    {
        private readonly DashboardFinanceiroViewModel _viewModel = new();

        private readonly CultureInfo _cultura = new("pt-BR");

        // Evita que o SelectionChanged seja disparado
        // antes de a tela estar completamente carregada.
        private bool _telaInicializada;

        public DashboardFinanceiroView()
        {
            InitializeComponent();

            cmbPeriodoDashboard.SelectedIndex = 0;

            _telaInicializada = true;

            CarregarDashboard();
        }

        /// <summary>
        /// Carrega os dados do serviço e atualiza toda a tela.
        /// </summary>
        private void CarregarDashboard(
            bool forcarAtualizacao = false)
        {
            try
            {
                string periodo = ObterPeriodoSelecionado();

                DashboardFinanceiroDto dados =
                    _viewModel.CarregarDados(
                        periodo,
                        forcarAtualizacao);

                AtualizarCards(dados);
                AtualizarResumo(dados);

                AtualizarGraficoReceitaCategoria(dados);
                AtualizarGraficoStatus(dados);
                AtualizarGraficoFormaPagamento(dados);
                AtualizarGraficoTopClientes(dados);
                AtualizarGraficoEmpresas(dados);
                AtualizarGraficoProfissionais(dados);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Não foi possível carregar o dashboard financeiro.\n\n{ex.Message}",
                    "Erro ao carregar dashboard",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Retorna o texto do período selecionado.
        /// </summary>
        private string ObterPeriodoSelecionado()
        {
            return (cmbPeriodoDashboard.SelectedItem as ComboBoxItem)?
                .Content?
                .ToString()
                ?? "Todos";
        }

        /// <summary>
        /// Atualiza os quatro cards superiores.
        /// </summary>
        private void AtualizarCards(
            DashboardFinanceiroDto dados)
        {
            txtTotalRecebido.Text =
                dados.TotalRecebido.ToString(
                    "C",
                    _cultura);

            txtValorPendente.Text =
                dados.ValorPendente.ToString(
                    "C",
                    _cultura);

            txtValorVencido.Text =
                dados.ValorVencido.ToString(
                    "C",
                    _cultura);

            txtQuantidadePagamentos.Text =
                dados.QuantidadePagamentos.ToString();
        }

        /// <summary>
        /// Atualiza os destaques exibidos no final do dashboard.
        /// </summary>
        private void AtualizarResumo(
            DashboardFinanceiroDto dados)
        {
            txtMelhorMesDashboard.Text =
                dados.MelhorMes;

            txtCategoriaDestaqueDashboard.Text =
                dados.CategoriaDestaque;

            txtFormaMaisUsada.Text =
                dados.FormaMaisUsada;

            txtClienteDestaque.Text =
                dados.ClienteDestaque;
        }

        /// <summary>
        /// Gráfico de receita paga por categoria.
        /// </summary>
        private void AtualizarGraficoReceitaCategoria(
            DashboardFinanceiroDto dados)
        {
            var lista = dados.ReceitaPorCategoria;

            if (lista.Count == 0)
            {
                LimparGraficoPizza(
                    graficoReceitaCategoria);

                return;
            }

            graficoReceitaCategoria.Series =
                lista.Select(item =>
                    new PieSeries<decimal>
                    {
                        Name = item.Nome,

                        Values = new[]
                        {
                            item.Valor
                        },

                        DataLabelsSize = 11,

                        DataLabelsPaint =
                            new SolidColorPaint(
                                SKColors.White),

                        DataLabelsPosition =
                            PolarLabelsPosition.Middle,

                        DataLabelsFormatter =
                            ponto =>
                                ponto.Model.ToString(
                                    "C0",
                                    _cultura),

                        ToolTipLabelFormatter =
                            ponto =>
                                $"{item.Nome}: " +
                                $"{ponto.Model.ToString("C", _cultura)}"
                    })
                .Cast<ISeries>()
                .ToArray();

            ConfigurarGraficoPizza(
                graficoReceitaCategoria);
        }

        /// <summary>
        /// Gráfico de quantidade de pagamentos por status.
        /// </summary>
        private void AtualizarGraficoStatus(
            DashboardFinanceiroDto dados)
        {
            var lista = dados.StatusPagamentos;

            if (lista.Count == 0)
            {
                LimparGraficoPizza(
                    graficoStatusPagamentos);

                return;
            }

            graficoStatusPagamentos.Series =
                lista.Select(item =>
                    new PieSeries<int>
                    {
                        Name = item.Nome,

                        Values = new[]
                        {
                            item.Quantidade
                        },

                        DataLabelsSize = 12,

                        DataLabelsPaint =
                            new SolidColorPaint(
                                SKColors.White),

                        DataLabelsPosition =
                            PolarLabelsPosition.Middle,

                        DataLabelsFormatter =
                            ponto =>
                                ponto.Model.ToString(),

                        ToolTipLabelFormatter =
                            ponto =>
                                $"{item.Nome}: " +
                                $"{ponto.Model} pagamento(s)"
                    })
                .Cast<ISeries>()
                .ToArray();

            ConfigurarGraficoPizza(
                graficoStatusPagamentos);
        }

        /// <summary>
        /// Gráfico de receita por forma de pagamento.
        /// </summary>
        private void AtualizarGraficoFormaPagamento(
            DashboardFinanceiroDto dados)
        {
            var lista = dados
                .ReceitaPorFormaPagamento
                .Take(6)
                .ToList();

            if (lista.Count == 0)
            {
                LimparGraficoCartesiano(
                    graficoFormaPagamento);

                return;
            }

            graficoFormaPagamento.Series =
                new ISeries[]
                {
                    new ColumnSeries<decimal>
                    {
                        Name = "Receita",

                        Values = lista
                            .Select(x => x.Valor)
                            .ToArray(),

                        Fill =
                            new SolidColorPaint(
                                SKColor.Parse(
                                    "#7C3AED")),

                        Stroke = null,

                        MaxBarWidth = 40,

                        Rx = 6,
                        Ry = 6,

                        DataLabelsSize = 10,

                        DataLabelsPosition =
                            DataLabelsPosition.Top,

                        DataLabelsPaint =
                            new SolidColorPaint(
                                SKColor.Parse(
                                    "#CBD5E1")),

                        DataLabelsFormatter =
                            ponto =>
                                ponto.Model > 0
                                    ? ponto.Model.ToString(
                                        "C0",
                                        _cultura)
                                    : string.Empty
                    }
                };

            graficoFormaPagamento.XAxes =
                new[]
                {
                    CriarEixoCategorias(
                        lista.Select(x => x.Nome))
                };

            graficoFormaPagamento.YAxes =
                new[]
                {
                    CriarEixoValores()
                };

            ConfigurarGraficoCartesiano(
                graficoFormaPagamento);
        }

        /// <summary>
        /// Ranking dos clientes com maior valor pago.
        /// </summary>
        private void AtualizarGraficoTopClientes(
            DashboardFinanceiroDto dados)
        {
            var lista = dados.TopClientes
                .Take(10)
                .Reverse()
                .ToList();

            if (lista.Count == 0)
            {
                LimparGraficoCartesiano(
                    graficoTopClientes);

                return;
            }

            graficoTopClientes.Series =
                new ISeries[]
                {
                    new RowSeries<decimal>
                    {
                        Name = "Receita",

                        Values = lista
                            .Select(x => x.Valor)
                            .ToArray(),

                        Fill =
                            new SolidColorPaint(
                                SKColor.Parse(
                                    "#38BDF8")),

                        Stroke = null,

                        MaxBarWidth = 24,

                        DataLabelsSize = 10,

                        DataLabelsPosition =
                            DataLabelsPosition.End,

                        DataLabelsPaint =
                            new SolidColorPaint(
                                SKColor.Parse(
                                    "#CBD5E1")),

                        DataLabelsFormatter =
                            ponto =>
                                ponto.Model.ToString(
                                    "C0",
                                    _cultura)
                    }
                };

            graficoTopClientes.YAxes =
                new[]
                {
                    CriarEixoCategorias(
                        lista.Select(x => x.Nome))
                };

            graficoTopClientes.XAxes =
                new[]
                {
                    CriarEixoValores()
                };

            ConfigurarGraficoCartesiano(
                graficoTopClientes);
        }

        /// <summary>
        /// Ranking das empresas com maior receita.
        /// </summary>
        private void AtualizarGraficoEmpresas(
            DashboardFinanceiroDto dados)
        {
            var lista = dados.ReceitaPorEmpresa
                .Take(10)
                .Reverse()
                .ToList();

            if (lista.Count == 0)
            {
                LimparGraficoCartesiano(
                    graficoEmpresas);

                return;
            }

            graficoEmpresas.Series =
                new ISeries[]
                {
                    new RowSeries<decimal>
                    {
                        Name = "Receita",

                        Values = lista
                            .Select(x => x.Valor)
                            .ToArray(),

                        Fill =
                            new SolidColorPaint(
                                SKColor.Parse(
                                    "#22C55E")),

                        Stroke = null,

                        MaxBarWidth = 24,

                        DataLabelsSize = 10,

                        DataLabelsPosition =
                            DataLabelsPosition.End,

                        DataLabelsPaint =
                            new SolidColorPaint(
                                SKColor.Parse(
                                    "#CBD5E1")),

                        DataLabelsFormatter =
                            ponto =>
                                ponto.Model.ToString(
                                    "C0",
                                    _cultura)
                    }
                };

            graficoEmpresas.YAxes =
                new[]
                {
                    CriarEixoCategorias(
                        lista.Select(x => x.Nome))
                };

            graficoEmpresas.XAxes =
                new[]
                {
                    CriarEixoValores()
                };

            ConfigurarGraficoCartesiano(
                graficoEmpresas);
        }

        /// <summary>
        /// Ranking dos profissionais com maior receita.
        /// </summary>
        private void AtualizarGraficoProfissionais(
            DashboardFinanceiroDto dados)
        {
            var lista = dados
                .ReceitaPorProfissional
                .Take(10)
                .Reverse()
                .ToList();

            if (lista.Count == 0)
            {
                LimparGraficoCartesiano(
                    graficoProfissionais);

                return;
            }

            graficoProfissionais.Series =
                new ISeries[]
                {
                    new RowSeries<decimal>
                    {
                        Name = "Receita",

                        Values = lista
                            .Select(x => x.Valor)
                            .ToArray(),

                        Fill =
                            new SolidColorPaint(
                                SKColor.Parse(
                                    "#F59E0B")),

                        Stroke = null,

                        MaxBarWidth = 24,

                        DataLabelsSize = 10,

                        DataLabelsPosition =
                            DataLabelsPosition.End,

                        DataLabelsPaint =
                            new SolidColorPaint(
                                SKColor.Parse(
                                    "#CBD5E1")),

                        DataLabelsFormatter =
                            ponto =>
                                ponto.Model.ToString(
                                    "C0",
                                    _cultura)
                    }
                };

            graficoProfissionais.YAxes =
                new[]
                {
                    CriarEixoCategorias(
                        lista.Select(x => x.Nome))
                };

            graficoProfissionais.XAxes =
                new[]
                {
                    CriarEixoValores()
                };

            ConfigurarGraficoCartesiano(
                graficoProfissionais);
        }

        /// <summary>
        /// Cria eixo com nomes de categorias.
        /// </summary>
        private Axis CriarEixoCategorias(
            IEnumerable<string> nomes)
        {
            return new Axis
            {
                Labels = nomes.ToArray(),

                LabelsPaint =
                    new SolidColorPaint(
                        SKColor.Parse(
                            "#CBD5E1")),

                TextSize = 10,

                SeparatorsPaint = null
            };
        }

        /// <summary>
        /// Cria eixo numérico formatado em moeda.
        /// </summary>
        private Axis CriarEixoValores()
        {
            return new Axis
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

                Labeler = valor =>
                    valor.ToString(
                        "C0",
                        _cultura)
            };
        }

        /// <summary>
        /// Aplica configurações comuns aos gráficos cartesianos.
        /// </summary>
        private static void ConfigurarGraficoCartesiano(
            LiveChartsCore.SkiaSharpView.WPF.CartesianChart grafico)
        {
            grafico.LegendPosition =
                LegendPosition.Hidden;

            grafico.TooltipTextPaint =
                new SolidColorPaint(
                    SKColors.White);

            grafico.TooltipBackgroundPaint =
                new SolidColorPaint(
                    SKColor.Parse(
                        "#202744"));
        }

        /// <summary>
        /// Aplica configurações comuns aos gráficos de pizza.
        /// </summary>
        private static void ConfigurarGraficoPizza(
            LiveChartsCore.SkiaSharpView.WPF.PieChart grafico)
        {
            grafico.LegendPosition =
                LegendPosition.Right;

            grafico.LegendTextPaint =
                new SolidColorPaint(
                    SKColor.Parse(
                        "#CBD5E1"));

            grafico.TooltipTextPaint =
                new SolidColorPaint(
                    SKColors.White);

            grafico.TooltipBackgroundPaint =
                new SolidColorPaint(
                    SKColor.Parse(
                        "#202744"));
        }

        /// <summary>
        /// Limpa um gráfico cartesiano sem dados.
        /// </summary>
        private static void LimparGraficoCartesiano(
            LiveChartsCore.SkiaSharpView.WPF.CartesianChart grafico)
        {
            grafico.Series =
                Array.Empty<ISeries>();

            grafico.XAxes =
                Array.Empty<Axis>();

            grafico.YAxes =
                Array.Empty<Axis>();
        }

        /// <summary>
        /// Limpa um gráfico de pizza sem dados.
        /// </summary>
        private static void LimparGraficoPizza(
            LiveChartsCore.SkiaSharpView.WPF.PieChart grafico)
        {
            grafico.Series =
                Array.Empty<ISeries>();
        }

        /// <summary>
        /// Atualiza os dados quando o período é alterado.
        /// </summary>
        private void FiltroDashboard_Changed(
            object sender,
            SelectionChangedEventArgs e)
        {
            if (!_telaInicializada)
                return;

            CarregarDashboard();
        }

        /// <summary>
        /// Força uma nova consulta ao banco.
        /// </summary>
        private void BtnAtualizarDashboard_Click(
            object sender,
            RoutedEventArgs e)
        {
            CarregarDashboard(
                forcarAtualizacao: true);
        }
    }
}   