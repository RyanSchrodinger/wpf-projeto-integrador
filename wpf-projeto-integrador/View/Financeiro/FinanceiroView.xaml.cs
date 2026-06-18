using ClosedXML.Excel;
using DocumentFormat.OpenXml.Drawing.Charts;
using LiveChartsCore;
using LiveChartsCore.Measure;
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
using wpf_projeto_integrador.Data;

namespace wpf_projeto_integrador.View.Financeiro
{
    public partial class FinanceiroView : UserControl
    {

        //define o tipo de moeda do brasil
        private readonly CultureInfo cultura = new("pt-BR");

        //Lista do tipo "FinanceiroViewModel" que vai receber valores do banco
        //Assim nao precisa consultar do banco toda hora q for aplicar um filtro 
        private List<FinanceiroViewModel> pagamentos = new();

        public FinanceiroView()
        {
            InitializeComponent();
            CarregarCombos();
            CarregarFinanceiro();
        }

        //Metodo que carrega os combobox
        private void CarregarCombos()
        {
            using var db = new MusicStationContext();

            //Busca todas as categorias cadastradas no banco e ordena pelo nome
            var categorias = db.CategoriasPagamento
                .OrderBy(c => c.Nome)
                .Select(c => c.Nome)
                .ToList();

            //Adiciona a opção "Todas"
            categorias.Insert(0, "Todas");

            cmbCategoria.ItemsSource = categorias;
            cmbCategoria.SelectedIndex = 0;

            //Busca todos os status cadastrados no banco e ordena pelo nome
           var status = db.StatusPagamentos
                .OrderBy(s => s.Nome)
                .Select(s => s.Nome)
                .ToList();

            //Busca todos os status cadastrados no banco e ordena pelo nome
            status.Insert(0, "Todos");

            cmbStatus.ItemsSource = status;
            cmbStatus.SelectedIndex = 0;
        }

        // Carrega os pagamentos do banco junto com seus relacionamentos
        private void CarregarFinanceiro()
        {
            using var db = new MusicStationContext();

            // Busca todos os pagamentos do banco e carrega também os dados relacionados (cliente, empresa, profissional,
            // forma de pagamento, status e categoria) facilitando e muito a vida do serhumano 
            var listaBanco = db.Pagamentos
                .Include(p => p.Cliente)
                .Include(p => p.Empresa)
                .Include(p => p.Profissional)
                .Include(p => p.FormaPagamento)
                .Include(p => p.StatusPagamento)
                .Include(p => p.CategoriaPagamento)
                .OrderByDescending(p => p.DataPagamento ?? p.DataVencimento)
                .ToList();


            //pego a parte mal tratada do banco e passo cada conteudo dentro ja formatado para o FinanceiroViewModel. No caso ele cria um novo
            //objeto com esses valores preenchidos q veio do select
            pagamentos = listaBanco.Select(p => new FinanceiroViewModel
            {
                //aqui precisamos passar o valor de cada propriedade dele, ja q estamos criando u novo objeto precisamos passar um valor ne 
                Id = p.Id,

                Cliente = p.Cliente != null
                    ? p.Cliente.Nome
                    : "Cliente não informado",

                Origem = ObterOrigem(p.Empresa?.NomeFantasia, p.Profissional?.Nome),

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

                ValorFormatado = p.Valor.ToString("C", cultura),

                DataReferencia = p.DataPagamento ?? p.DataVencimento,

                Data = (p.DataPagamento ?? p.DataVencimento).ToString("dd/MM/yyyy"),

                Observacoes = p.Observacoes ?? "Sem observações",

                CorCategoria = ObterCorCategoria(p.CategoriaPagamento?.Nome)
            })
            .ToList();

            AplicarFiltros();
        }

        private void AplicarFiltros()
        {
            var lista = pagamentos.AsEnumerable();

            string busca = txtBusca.Text.ToLower();

            if (!string.IsNullOrWhiteSpace(busca))
            {
                lista = lista.Where(p =>
                    p.Cliente.ToLower().Contains(busca) ||
                    p.Origem.ToLower().Contains(busca) ||
                    p.Categoria.ToLower().Contains(busca) ||
                    p.FormaPagamento.ToLower().Contains(busca) ||
                    p.Status.ToLower().Contains(busca));
            }

            string categoria = cmbCategoria.SelectedItem?.ToString() ?? "Todas";

            if (categoria != "Todas")
            {
                lista = lista.Where(p => p.Categoria == categoria);
            }

            string status = cmbStatus.SelectedItem?.ToString() ?? "Todos";

            if (status != "Todos")
            {
                lista = lista.Where(p => p.Status == status);
            }

            string periodo = ((ComboBoxItem)cmbPeriodo.SelectedItem)?.Content?.ToString() ?? "Todos";

            if (periodo == "Hoje")
            {
                lista = lista.Where(p => p.DataReferencia.Date == DateTime.Now.Date);
            }
            else if (periodo == "Este mês")
            {
                lista = lista.Where(p =>
                    p.DataReferencia.Month == DateTime.Now.Month &&
                    p.DataReferencia.Year == DateTime.Now.Year);
            }
            else if (periodo == "Este ano")
            {
                lista = lista.Where(p => p.DataReferencia.Year == DateTime.Now.Year);
            }

            var resultado = lista.ToList();

            dgPagamentos.ItemsSource = resultado;

            AtualizarCards(resultado);
            AtualizarGraficoCategorias(resultado);
            AtualizarGraficoMensal(resultado);
            AtualizarResumoRapido(resultado);
        }

        private void AtualizarCards(List<FinanceiroViewModel> lista)
        {
            var pagos = lista
                .Where(p => p.Status == "Pago")
                .ToList();

            decimal receitaTotal = pagos.Sum(p => p.Valor);

            decimal receitaMes = pagos
                .Where(p => p.DataReferencia.Month == DateTime.Now.Month &&
                            p.DataReferencia.Year == DateTime.Now.Year)
                .Sum(p => p.Valor);

            decimal receitaHoje = pagos
                .Where(p => p.DataReferencia.Date == DateTime.Now.Date)
                .Sum(p => p.Valor);

            decimal ticketMedio = pagos.Any()
                ? pagos.Average(p => p.Valor)
                : 0;

            txtReceitaTotal.Text = receitaTotal.ToString("C", cultura);
            txtReceitaMes.Text = receitaMes.ToString("C", cultura);
            txtReceitaHoje.Text = receitaHoje.ToString("C", cultura);
            txtTicketMedio.Text = ticketMedio.ToString("C", cultura);

            txtPagos.Text = lista.Count(p => p.Status == "Pago").ToString();
            txtPendentes.Text = lista.Count(p => p.Status == "Pendente").ToString();
            txtVencidos.Text = lista.Count(p => p.Status == "Vencido").ToString();
        }

        private void AtualizarGraficoCategorias(List<FinanceiroViewModel> lista)
        {
            var resumo = lista
                .Where(p => p.Status == "Pago")
                .GroupBy(p => p.Categoria)
                .Select(g => new
                {
                    Categoria = g.Key,
                    Valor = g.Sum(p => p.Valor)
                })
                .Where(x => x.Valor > 0)
                .OrderByDescending(x => x.Valor)
                .ToList();

            if (!resumo.Any())
            {
                graficoCategorias.Series = Array.Empty<ISeries>();
                return;
            }

            graficoCategorias.Series = resumo
                .Select(c => new PieSeries<decimal>
                {
                    Name = c.Categoria,
                    Values = new[] { c.Valor },
                    DataLabelsSize = 12,
                    DataLabelsPaint = new SolidColorPaint(SKColors.White)
                   
                })
                .ToArray();
            graficoCategorias.LegendTextPaint = new SolidColorPaint(SKColors.White);
        }

        private void AtualizarGraficoMensal(List<FinanceiroViewModel> lista)
        {
            var meses = Enumerable.Range(1, 12)
                .Select(m => new
                {
                    Mes = m,
                    Nome = new DateTime(DateTime.Now.Year, m, 1)
                        .ToString("MMM", cultura)
                        .Replace(".", ""),

                    Valor = lista
                        .Where(p => p.Status == "Pago" &&
                                    p.DataReferencia.Month == m &&
                                    p.DataReferencia.Year == DateTime.Now.Year)
                        .Sum(p => p.Valor)
                })
                .ToList();

            graficoReceitaMensal.Series = new ISeries[]
            {
                new LineSeries<decimal>
                {
                    Name = "Receita",
                    Values = meses.Select(m => m.Valor).ToArray(),
                    GeometrySize = 10,
                    Stroke = new SolidColorPaint(SKColor.Parse("#8B5CF6")) { StrokeThickness = 3 },
                    Fill = null,
                    DataLabelsSize = 11,
                    DataLabelsPaint = new SolidColorPaint(SKColors.White),
                    
                }
            };

            graficoReceitaMensal.XAxes = new[]
            {
                new Axis
                {
                    Labels = meses.Select(m => m.Nome).ToArray(),
                    LabelsPaint = new SolidColorPaint(SKColors.White),
                    TextSize = 11
                }
            };

            graficoReceitaMensal.YAxes = new[]
            {
                new Axis
                {
                    LabelsPaint = new SolidColorPaint(SKColors.White),
                    TextSize = 11,
                    Labeler = valor => valor.ToString("C0", cultura)
                }
            };

            graficoReceitaMensal.LegendPosition = LiveChartsCore.Measure.LegendPosition.Hidden;
        }

        private void AtualizarResumoRapido(List<FinanceiroViewModel> lista)
        {
            var pagos = lista
                .Where(p => p.Status == "Pago")
                .ToList();

            var categoriaDestaque = pagos
                .GroupBy(p => p.Categoria)
                .Select(g => new
                {
                    Categoria = g.Key,
                    Valor = g.Sum(p => p.Valor)
                })
                .OrderByDescending(x => x.Valor)
                .FirstOrDefault();

            txtCategoriaDestaque.Text = categoriaDestaque != null
                ? $"{categoriaDestaque.Categoria} ({categoriaDestaque.Valor.ToString("C", cultura)})"
                : "Nenhuma";

            var melhorMes = pagos
                .Where(p => p.DataReferencia.Year == DateTime.Now.Year)
                .GroupBy(p => p.DataReferencia.Month)
                .Select(g => new
                {
                    Mes = g.Key,
                    Valor = g.Sum(p => p.Valor)
                })
                .OrderByDescending(x => x.Valor)
                .FirstOrDefault();

            txtMelhorMes.Text = melhorMes != null
                ? $"{new DateTime(DateTime.Now.Year, melhorMes.Mes, 1).ToString("MMMM", cultura)} ({melhorMes.Valor.ToString("C", cultura)})"
                : "Nenhum";
        }

        private void BtnExportarExcel_Click(object sender, RoutedEventArgs e)
        {
            var lista = dgPagamentos.ItemsSource as IEnumerable<FinanceiroViewModel>;

            if (lista == null || !lista.Any())
            {
                MessageBox.Show("Não há dados para exportar.");
                return;
            }

            var dialog = new SaveFileDialog
            {
                Filter = "Arquivo Excel (*.xlsx)|*.xlsx",
                FileName = $"Relatorio_Financeiro_{DateTime.Now:dd-MM-yyyy}.xlsx"
            };

            if (dialog.ShowDialog() != true)
                return;

            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Financeiro");

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
                worksheet.Cell(linha, 1).Value = item.Cliente;
                worksheet.Cell(linha, 2).Value = item.Origem;
                worksheet.Cell(linha, 3).Value = item.Categoria;
                worksheet.Cell(linha, 4).Value = item.FormaPagamento;
                worksheet.Cell(linha, 5).Value = item.Status;
                worksheet.Cell(linha, 6).Value = item.Valor;
                worksheet.Cell(linha, 7).Value = item.Data;
                worksheet.Cell(linha, 8).Value = item.Observacoes;

                linha++;
            }

            var cabecalho = worksheet.Range(1, 1, 1, 8);
            cabecalho.Style.Font.Bold = true;
            cabecalho.Style.Fill.BackgroundColor = XLColor.FromHtml("#7C3AED");
            cabecalho.Style.Font.FontColor = XLColor.White;

            worksheet.Column(6).Style.NumberFormat.Format = "R$ #,##0.00";
            worksheet.Columns().AdjustToContents();

            workbook.SaveAs(dialog.FileName);

            MessageBox.Show("Relatório financeiro exportado com sucesso.");
        }

        private static string ObterOrigem(string? empresa, string? profissional)
        {
            if (!string.IsNullOrWhiteSpace(empresa))
                return empresa;

            if (!string.IsNullOrWhiteSpace(profissional))
                return profissional;

            return "Plataforma Music Station";
        }

        private static string ObterCorCategoria(string? categoria)
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

        private void txtBusca_TextChanged(object sender, TextChangedEventArgs e)
        {
            AplicarFiltros();
        }

        private void Filtro_Changed(object sender, SelectionChangedEventArgs e)
        {
            if (dgPagamentos != null)
            {
                AplicarFiltros();
            }
        }

        private void BtnAtualizar_Click(object sender, RoutedEventArgs e)
        {
            CarregarFinanceiro();
        }

        private void BtnVisualizar_Click(object sender, RoutedEventArgs e)
        {
            var pagamento = (sender as Button)?.DataContext as FinanceiroViewModel;

            if (pagamento == null)
            {
                MessageBox.Show("Pagamento não encontrado.");
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
                "Detalhes financeiros",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
        }
    }

    public class FinanceiroViewModel
    {
        public int Id { get; set; }

        public string Cliente { get; set; } = "";

        public string Origem { get; set; } = "";

        public string Categoria { get; set; } = "";

        public string FormaPagamento { get; set; } = "";

        public string Status { get; set; } = "";

        public decimal Valor { get; set; }

        public string ValorFormatado { get; set; } = "";

        public DateTime DataReferencia { get; set; }

        public string Data { get; set; } = "";

        public string Observacoes { get; set; } = "";

        public string CorCategoria { get; set; } = "";
    }
}