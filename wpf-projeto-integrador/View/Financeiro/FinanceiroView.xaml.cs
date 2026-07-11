using ClosedXML.Excel;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using wpf_projeto_integrador.Data;

namespace wpf_projeto_integrador.View.Financeiro
{
    public partial class FinanceiroView : UserControl
    {
        // Define o padrão de moeda brasileira.
        private readonly CultureInfo cultura = new("pt-BR");

        // Armazena todos os pagamentos carregados do banco.
        private List<FinanceiroViewModel> pagamentos = new();

        // Armazena os pagamentos depois da aplicação dos filtros.
        private List<FinanceiroViewModel> pagamentosFiltrados = new();

        // Quantidade de registros exibidos em cada página.
        private const int ItensPorPagina = 10;

        // Página atualmente exibida.
        private int paginaAtual = 1;

        // Quantidade total de páginas.
        private int totalPaginas = 1;

        public FinanceiroView()
        {
            InitializeComponent();

            CarregarCombos();
            CarregarFinanceiro();
        }

        /// <summary>
        /// Carrega os valores dos ComboBoxes.
        /// </summary>
        private void CarregarCombos()
        {
            try
            {
                using var db = new MusicStationContext();

                var categorias = db.CategoriasPagamento
                    .AsNoTracking()
                    .OrderBy(c => c.Nome)
                    .Select(c => c.Nome)
                    .ToList();

                categorias.Insert(0, "Todas");

                cmbCategoria.ItemsSource = categorias;
                cmbCategoria.SelectedIndex = 0;

                var status = db.StatusPagamentos
                    .AsNoTracking()
                    .OrderBy(s => s.Nome)
                    .Select(s => s.Nome)
                    .ToList();

                status.Insert(0, "Todos");

                cmbStatus.ItemsSource = status;
                cmbStatus.SelectedIndex = 0;

                cmbPeriodo.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Não foi possível carregar os filtros.\n\n{ex.Message}",
                    "Erro ao carregar filtros",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Busca todos os pagamentos e seus relacionamentos no banco.
        /// </summary>
        private void CarregarFinanceiro()
        {
            try
            {
                using var db = new MusicStationContext();

                var listaBanco = db.Pagamentos
                    .AsNoTracking()
                    .Include(p => p.Cliente)
                    .Include(p => p.Empresa)
                    .Include(p => p.Profissional)
                    .Include(p => p.FormaPagamento)
                    .Include(p => p.StatusPagamento)
                    .Include(p => p.CategoriaPagamento)
                    .OrderByDescending(p => p.DataPagamento ?? p.DataVencimento)
                    .ToList();

                pagamentos = listaBanco
                    .Select(p => new FinanceiroViewModel
                    {
                        Id = p.Id,

                        Cliente = p.Cliente != null
                            ? p.Cliente.Nome
                            : "Cliente não informado",

                        Origem = ObterOrigem(
                            p.Empresa?.NomeFantasia,
                            p.Profissional?.Nome),

                        Categoria = p.CategoriaPagamento != null
                            ? p.CategoriaPagamento.Nome
                            : "Não informado",

                        FormaPagamento = p.FormaPagamento != null
                            ? p.FormaPagamento.Nome
                            : "Não informado",

                        Status = p.StatusPagamento != null
                            ? p.StatusPagamento.Nome
                            : "Não informado",

                        Valor = p.Valor,

                        ValorFormatado = p.Valor.ToString(
                            "C",
                            cultura),

                        DataReferencia =
                            p.DataPagamento ?? p.DataVencimento,

                        Data = (p.DataPagamento ?? p.DataVencimento)
                            .ToString("dd/MM/yyyy"),

                        Observacoes = string.IsNullOrWhiteSpace(p.Observacoes)
                            ? "Sem observações"
                            : p.Observacoes,

                        CorCategoria = ObterCorCategoria(
                            p.CategoriaPagamento?.Nome)
                    })
                    .ToList();

                AplicarFiltros();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Não foi possível carregar os dados financeiros.\n\n{ex.Message}",
                    "Erro ao carregar financeiro",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Aplica pesquisa, categoria, status e período.
        /// </summary>
        private void AplicarFiltros()
        {
            if (txtBusca == null ||
                cmbCategoria == null ||
                cmbStatus == null ||
                cmbPeriodo == null ||
                dgPagamentos == null)
            {
                return;
            }

            var lista = pagamentos.AsEnumerable();

            string busca = txtBusca.Text?
                .Trim()
                .ToLower(cultura) ?? string.Empty;

            if (!string.IsNullOrWhiteSpace(busca))
            {
                lista = lista.Where(p =>
                    p.Cliente.ToLower(cultura).Contains(busca) ||
                    p.Origem.ToLower(cultura).Contains(busca) ||
                    p.Categoria.ToLower(cultura).Contains(busca) ||
                    p.FormaPagamento.ToLower(cultura).Contains(busca) ||
                    p.Status.ToLower(cultura).Contains(busca));
            }

            string categoria =
                cmbCategoria.SelectedItem?.ToString() ?? "Todas";

            if (categoria != "Todas")
            {
                lista = lista.Where(p =>
                    p.Categoria.Equals(
                        categoria,
                        StringComparison.OrdinalIgnoreCase));
            }

            string status =
                cmbStatus.SelectedItem?.ToString() ?? "Todos";

            if (status != "Todos")
            {
                lista = lista.Where(p =>
                    p.Status.Equals(
                        status,
                        StringComparison.OrdinalIgnoreCase));
            }

            string periodo =
                (cmbPeriodo.SelectedItem as ComboBoxItem)?
                .Content?
                .ToString() ?? "Todos";

            DateTime hoje = DateTime.Today;

            if (periodo == "Hoje")
            {
                lista = lista.Where(p =>
                    p.DataReferencia.Date == hoje);
            }
            else if (periodo == "Este mês")
            {
                lista = lista.Where(p =>
                    p.DataReferencia.Month == hoje.Month &&
                    p.DataReferencia.Year == hoje.Year);
            }
            else if (periodo == "Este ano")
            {
                lista = lista.Where(p =>
                    p.DataReferencia.Year == hoje.Year);
            }

            pagamentosFiltrados = lista
                .OrderByDescending(p => p.DataReferencia)
                .ToList();

            // Sempre que os filtros mudarem, retorna à primeira página.
            paginaAtual = 1;

            AtualizarPaginacao();
            AtualizarCards(pagamentosFiltrados);
            AtualizarGraficoMensal(pagamentosFiltrados);
        }

        /// <summary>
        /// Exibe somente os registros pertencentes à página atual.
        /// </summary>
        private void AtualizarPaginacao()
        {
            int totalRegistros = pagamentosFiltrados.Count;

            totalPaginas = (int)Math.Ceiling(
                totalRegistros / (double)ItensPorPagina);

            if (totalPaginas < 1)
                totalPaginas = 1;

            if (paginaAtual > totalPaginas)
                paginaAtual = totalPaginas;

            if (paginaAtual < 1)
                paginaAtual = 1;

            var registrosPagina = pagamentosFiltrados
                .Skip((paginaAtual - 1) * ItensPorPagina)
                .Take(ItensPorPagina)
                .ToList();

            dgPagamentos.ItemsSource = registrosPagina;

            int primeiroRegistro = totalRegistros == 0
                ? 0
                : ((paginaAtual - 1) * ItensPorPagina) + 1;

            int ultimoRegistro = Math.Min(
                paginaAtual * ItensPorPagina,
                totalRegistros);

            txtQuantidadeRegistros.Text = totalRegistros == 1
                ? "1 registro"
                : $"{totalRegistros} registros";

            txtPaginaAtual.Text =
                $"Página {paginaAtual} de {totalPaginas}";

            txtResumoPagina.Text = totalRegistros == 0
                ? "Nenhum registro encontrado"
                : $"Mostrando {primeiroRegistro} a {ultimoRegistro} de {totalRegistros} registros";

            btnPaginaAnterior.IsEnabled =
                paginaAtual > 1;

            btnProximaPagina.IsEnabled =
                paginaAtual < totalPaginas;

            btnPaginaAnterior.Opacity =
                btnPaginaAnterior.IsEnabled ? 1 : 0.4;

            btnProximaPagina.Opacity =
                btnProximaPagina.IsEnabled ? 1 : 0.4;

            btnPaginaAnterior.Cursor =
                btnPaginaAnterior.IsEnabled
                    ? System.Windows.Input.Cursors.Hand
                    : System.Windows.Input.Cursors.Arrow;

            btnProximaPagina.Cursor =
                btnProximaPagina.IsEnabled
                    ? System.Windows.Input.Cursors.Hand
                    : System.Windows.Input.Cursors.Arrow;
        }

        /// <summary>
        /// Atualiza os quatro cards superiores.
        /// </summary>
        private void AtualizarCards(
            List<FinanceiroViewModel> lista)
        {
            var pagos = lista
                .Where(p => p.Status.Equals(
                    "Pago",
                    StringComparison.OrdinalIgnoreCase))
                .ToList();

            DateTime hoje = DateTime.Today;

            decimal receitaTotal =
                pagos.Sum(p => p.Valor);

            decimal receitaMes = pagos
                .Where(p =>
                    p.DataReferencia.Month == hoje.Month &&
                    p.DataReferencia.Year == hoje.Year)
                .Sum(p => p.Valor);

            decimal receitaHoje = pagos
                .Where(p =>
                    p.DataReferencia.Date == hoje)
                .Sum(p => p.Valor);

            decimal ticketMedio = pagos.Count > 0
                ? pagos.Average(p => p.Valor)
                : 0;

            txtReceitaTotal.Text =
                receitaTotal.ToString("C", cultura);

            txtReceitaMes.Text =
                receitaMes.ToString("C", cultura);

            txtReceitaHoje.Text =
                receitaHoje.ToString("C", cultura);

            txtTicketMedio.Text =
                ticketMedio.ToString("C", cultura);
        }

        /// <summary>
        /// Atualiza o gráfico anual de receita mensal.
        /// </summary>
        private void AtualizarGraficoMensal(
            List<FinanceiroViewModel> lista)
        {
            int anoAtual = DateTime.Today.Year;

            var meses = Enumerable.Range(1, 12)
                .Select(numeroMes => new
                {
                    Numero = numeroMes,

                    Nome = new DateTime(
                            anoAtual,
                            numeroMes,
                            1)
                        .ToString("MMM", cultura)
                        .Replace(".", ""),

                    Valor = lista
                        .Where(p =>
                            p.Status.Equals(
                                "Pago",
                                StringComparison.OrdinalIgnoreCase) &&
                            p.DataReferencia.Month == numeroMes &&
                            p.DataReferencia.Year == anoAtual)
                        .Sum(p => p.Valor)
                })
                .ToList();

            graficoReceitaMensal.Series = new ISeries[]
            {
                new ColumnSeries<decimal>
                {
                    Name = "Receita",

                    Values = meses
                        .Select(m => m.Valor)
                        .ToArray(),

                    Fill = new SolidColorPaint(
                        SKColor.Parse("#7C3AED")),

                    Stroke = null,

                    MaxBarWidth = 38,

                    Rx = 5,
                    Ry = 5,

                    DataLabelsSize = 10,

                    DataLabelsPosition =
                        LiveChartsCore.Measure.DataLabelsPosition.Top,

                    DataLabelsPaint =
                        new SolidColorPaint(
                            SKColor.Parse("#CBD5E1")),

                    DataLabelsFormatter = ponto =>
                        ponto.Model > 0
                            ? ponto.Model.ToString("C0", cultura)
                            : string.Empty
                }
            };

            graficoReceitaMensal.XAxes = new[]
            {
                new Axis
                {
                    Labels = meses
                        .Select(m => m.Nome)
                        .ToArray(),

                    LabelsPaint =
                        new SolidColorPaint(
                            SKColor.Parse("#CBD5E1")),

                    SeparatorsPaint =
                        new SolidColorPaint(
                            SKColor.Parse("#1F2942"))
                        {
                            StrokeThickness = 1
                        },

                    TextSize = 11
                }
            };

            graficoReceitaMensal.YAxes = new[]
            {
                new Axis
                {
                    LabelsPaint =
                        new SolidColorPaint(
                            SKColor.Parse("#94A3B8")),

                    SeparatorsPaint =
                        new SolidColorPaint(
                            SKColor.Parse("#27314D"))
                        {
                            StrokeThickness = 1
                        },

                    TextSize = 10,

                    Labeler = valor =>
                        valor.ToString("C0", cultura),

                    MinLimit = 0
                }
            };

            graficoReceitaMensal.LegendPosition =
                LiveChartsCore.Measure.LegendPosition.Hidden;

            graficoReceitaMensal.TooltipTextPaint =
                new SolidColorPaint(SKColors.White);

            graficoReceitaMensal.TooltipBackgroundPaint =
                new SolidColorPaint(
                    SKColor.Parse("#202744"));
        }

        /// <summary>
        /// Vai para a página anterior.
        /// </summary>
        private void BtnPaginaAnterior_Click(
            object sender,
            RoutedEventArgs e)
        {
            if (paginaAtual <= 1)
                return;

            paginaAtual--;

            AtualizarPaginacao();
        }

        /// <summary>
        /// Vai para a próxima página.
        /// </summary>
        private void BtnProximaPagina_Click(
            object sender,
            RoutedEventArgs e)
        {
            if (paginaAtual >= totalPaginas)
                return;

            paginaAtual++;

            AtualizarPaginacao();
        }

        /// <summary>
        /// Exporta todos os resultados filtrados para Excel.
        /// </summary>
        private void BtnExportarExcel_Click(
            object sender,
            RoutedEventArgs e)
        {
            var lista = pagamentosFiltrados
                .AsEnumerable();

            if (!lista.Any())
            {
                MessageBox.Show(
                    "Não há dados para exportar.",
                    "Exportação",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);

                return;
            }

            var dialog = new SaveFileDialog
            {
                Filter =
                    "Arquivo Excel (*.xlsx)|*.xlsx",

                FileName =
                    $"Relatorio_Financeiro_{DateTime.Now:dd-MM-yyyy}.xlsx"
            };

            if (dialog.ShowDialog() != true)
                return;

            try
            {
                using var workbook = new XLWorkbook();

                var worksheet =
                    workbook.Worksheets.Add("Financeiro");

                worksheet.Cell(1, 1).Value = "Cliente";
                worksheet.Cell(1, 2).Value = "Origem";
                worksheet.Cell(1, 3).Value = "Categoria";
                worksheet.Cell(1, 4).Value = "Forma de Pagamento";
                worksheet.Cell(1, 5).Value = "Status";
                worksheet.Cell(1, 6).Value = "Valor";
                worksheet.Cell(1, 7).Value = "Data";
                worksheet.Cell(1, 8).Value = "Observações";

                int linha = 2;

                foreach (var item in lista)
                {
                    worksheet.Cell(linha, 1).Value =
                        item.Cliente;

                    worksheet.Cell(linha, 2).Value =
                        item.Origem;

                    worksheet.Cell(linha, 3).Value =
                        item.Categoria;

                    worksheet.Cell(linha, 4).Value =
                        item.FormaPagamento;

                    worksheet.Cell(linha, 5).Value =
                        item.Status;

                    worksheet.Cell(linha, 6).Value =
                        item.Valor;

                    worksheet.Cell(linha, 7).Value =
                        item.DataReferencia;

                    worksheet.Cell(linha, 8).Value =
                        item.Observacoes;

                    linha++;
                }

                var cabecalho =
                    worksheet.Range(1, 1, 1, 8);

                cabecalho.Style.Font.Bold = true;

                cabecalho.Style.Fill.BackgroundColor =
                    XLColor.FromHtml("#7C3AED");

                cabecalho.Style.Font.FontColor =
                    XLColor.White;

                cabecalho.Style.Alignment.Horizontal =
                    XLAlignmentHorizontalValues.Center;

                worksheet.Column(6)
                    .Style
                    .NumberFormat
                    .Format = "R$ #,##0.00";

                worksheet.Column(7)
                    .Style
                    .NumberFormat
                    .Format = "dd/MM/yyyy";

                var intervaloDados = worksheet.Range(
                    1,
                    1,
                    linha - 1,
                    8);

                intervaloDados.Style.Border
                    .BottomBorder =
                    XLBorderStyleValues.Thin;

                intervaloDados.Style.Border
                    .BottomBorderColor =
                    XLColor.FromHtml("#D1D5DB");

                worksheet.SheetView.FreezeRows(1);

                worksheet.RangeUsed()?.SetAutoFilter();

                worksheet.Columns()
                    .AdjustToContents();

                workbook.SaveAs(dialog.FileName);

                MessageBox.Show(
                    "Relatório financeiro exportado com sucesso.",
                    "Exportação concluída",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Não foi possível exportar o relatório.\n\n{ex.Message}",
                    "Erro na exportação",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Retorna o nome da empresa ou profissional responsável.
        /// </summary>
        private static string ObterOrigem(
            string? empresa,
            string? profissional)
        {
            if (!string.IsNullOrWhiteSpace(empresa))
                return empresa;

            if (!string.IsNullOrWhiteSpace(profissional))
                return profissional;

            return "Plataforma Music Station";
        }
        private void dgPagamentos_PreviewMouseWheel(
            object sender,
            MouseWheelEventArgs e)
        {
            DependencyObject elemento = dgPagamentos;

            while (elemento != null)
            {
                elemento = VisualTreeHelper.GetParent(elemento);

                if (elemento is ScrollViewer scrollViewer)
                {
                    scrollViewer.ScrollToVerticalOffset(
                        scrollViewer.VerticalOffset - e.Delta);

                    e.Handled = true;
                    return;
                }
            }
        }

        /// <summary>
        /// Define a cor visual de cada categoria.
        /// </summary>
        private static string ObterCorCategoria(
            string? categoria)
        {
            return categoria switch
            {
                "Serviço" => "#7C3AED",
                "Locação" => "#38BDF8",
                "Venda" => "#22C55E",
                "Multa" => "#EF4444",
                "Outro" => "#F59E0B",
                _ => "#8B5CF6"
            };
        }

        /// <summary>
        /// Executado quando o usuário digita na pesquisa.
        /// </summary>
        private void txtBusca_TextChanged(
            object sender,
            TextChangedEventArgs e)
        {
            AplicarFiltros();
        }

        /// <summary>
        /// Executado quando categoria, status ou período mudam.
        /// </summary>
        private void Filtro_Changed(
            object sender,
            SelectionChangedEventArgs e)
        {
            if (dgPagamentos == null)
                return;

            AplicarFiltros();
        }

        /// <summary>
        /// Atualiza novamente os dados do banco.
        /// </summary>
        private void BtnAtualizar_Click(
            object sender,
            RoutedEventArgs e)
        {
            CarregarFinanceiro();
        }

        /// <summary>
        /// Mostra os detalhes do pagamento selecionado.
        /// </summary>
        private void BtnVisualizar_Click(
            object sender,
            RoutedEventArgs e)
        {
            var pagamento =
                (sender as Button)?.DataContext
                as FinanceiroViewModel;

            if (pagamento == null)
            {
                MessageBox.Show(
                    "Pagamento não encontrado.",
                    "Detalhes financeiros",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);

                return;
            }

            MessageBox.Show(
                $"Cliente: {pagamento.Cliente}\n" +
                $"Origem: {pagamento.Origem}\n" +
                $"Categoria: {pagamento.Categoria}\n" +
                $"Forma de pagamento: {pagamento.FormaPagamento}\n" +
                $"Status: {pagamento.Status}\n" +
                $"Valor: {pagamento.ValorFormatado}\n" +
                $"Data: {pagamento.Data}\n\n" +
                $"Observações:\n{pagamento.Observacoes}",
                $"Pagamento #{pagamento.Id}",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
        }
    }

    /// <summary>
    /// Modelo utilizado somente para exibição dos dados financeiros.
    /// </summary>
    public class FinanceiroViewModel
    {
        public int Id { get; set; }

        public string Cliente { get; set; } = string.Empty;

        public string Origem { get; set; } = string.Empty;

        public string Categoria { get; set; } = string.Empty;

        public string FormaPagamento { get; set; } = string.Empty;

        public string Status { get; set; } = string.Empty;

        public decimal Valor { get; set; }

        public string ValorFormatado { get; set; } = string.Empty;

        public DateTime DataReferencia { get; set; }

        public string Data { get; set; } = string.Empty;

        public string Observacoes { get; set; } = string.Empty;

        public string CorCategoria { get; set; } = string.Empty;
    }
}