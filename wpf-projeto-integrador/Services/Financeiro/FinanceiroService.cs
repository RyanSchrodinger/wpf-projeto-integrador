using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using wpf_projeto_integrador.Data;
using wpf_projeto_integrador.DTOs.Financeiro;
using wpf_projeto_integrador.Models;
using wpf_projeto_integrador.ViewModels.Financeiro;

namespace wpf_projeto_integrador.Services.Financeiro
{
    public class FinanceiroService
    {
        private readonly CultureInfo cultura = new("pt-BR");    

        // Cache compartilhado entre a Visão Geral e o Dashboard.
        private static List<FinanceiroItemViewModel>? cachePagamentos;

        private static DateTime dataCache = DateTime.MinValue;

        private static readonly TimeSpan DuracaoCache =
            TimeSpan.FromMinutes(2);

        public List<string> ObterCategorias()
        {
            using var db = new MusicStationContext();

            var categorias = db.CategoriasPagamento
                .AsNoTracking()
                .OrderBy(c => c.Nome)
                .Select(c => c.Nome)
                .ToList();

            categorias.Insert(0, "Todas");

            return categorias;
        }

        public List<string> ObterStatus()
        {
            using var db = new MusicStationContext();

            var status = db.StatusPagamentos
                .AsNoTracking()
                .OrderBy(s => s.Nome)
                .Select(s => s.Nome)
                .ToList();

            status.Insert(0, "Todos");

            return status;
        }

        public List<FinanceiroItemViewModel> ObterPagamentos(
            bool forcarAtualizacao = false)
        {
            bool cacheValido =
                cachePagamentos != null &&
                DateTime.Now - dataCache < DuracaoCache;

            if (!forcarAtualizacao && cacheValido)
            {
                return cachePagamentos!
                    .Select(CopiarItem)
                    .ToList();
            }

            using var db = new MusicStationContext();

            var pagamentosBanco = db.Pagamentos
                .AsNoTracking()
                .Include(p => p.Cliente)
                .Include(p => p.Empresa)
                .Include(p => p.Profissional)
                .Include(p => p.FormaPagamento)
                .Include(p => p.StatusPagamento)
                .Include(p => p.CategoriaPagamento)
                .OrderByDescending(
                    p => p.DataPagamento ?? p.DataVencimento)
                .ToList();

            cachePagamentos = pagamentosBanco
                .Select(MapearPagamento)
                .ToList();

            dataCache = DateTime.Now;

            return cachePagamentos
                .Select(CopiarItem)
                .ToList();
        }

        public List<FinanceiroItemViewModel> ObterPagamentosFiltrados(
            string? busca,
            string? categoria,
            string? status,
            string? periodo,
            bool forcarAtualizacao = false)
        {
            var lista = ObterPagamentos(forcarAtualizacao)
                .AsEnumerable();

            busca = busca?.Trim();

            if (!string.IsNullOrWhiteSpace(busca))
            {
                lista = lista.Where(p =>
                    ContemTexto(p.Cliente, busca) ||
                    ContemTexto(p.Origem, busca) ||
                    ContemTexto(p.Categoria, busca) ||
                    ContemTexto(p.FormaPagamento, busca) ||
                    ContemTexto(p.Status, busca));
            }

            if (!string.IsNullOrWhiteSpace(categoria) &&
                categoria != "Todas")
            {
                lista = lista.Where(p =>
                    p.Categoria.Equals(
                        categoria,
                        StringComparison.OrdinalIgnoreCase));
            }

            if (!string.IsNullOrWhiteSpace(status) &&
                status != "Todos")
            {
                lista = lista.Where(p =>
                    p.Status.Equals(
                        status,
                        StringComparison.OrdinalIgnoreCase));
            }

            lista = AplicarFiltroPeriodo(lista, periodo);

            return lista
                .OrderByDescending(p => p.DataReferencia)
                .ToList();
        }

        public DashboardFinanceiroDto ObterDashboard(
            string? periodo,
            bool forcarAtualizacao = false)
        {
            var lista = AplicarFiltroPeriodo(
                    ObterPagamentos(forcarAtualizacao),
                    periodo)
                .ToList();

            var pagos = lista
                .Where(p => StatusIgual(p, "Pago"))
                .ToList();

            var pendentes = lista
                .Where(p => StatusIgual(p, "Pendente"))
                .ToList();

            var vencidos = lista
                .Where(p => StatusIgual(p, "Vencido"))
                .ToList();

            var resultado = new DashboardFinanceiroDto
            {
                TotalRecebido = pagos.Sum(p => p.Valor),

                ValorPendente = pendentes.Sum(p => p.Valor),

                ValorVencido = vencidos.Sum(p => p.Valor),

                QuantidadePagamentos = lista.Count,

                ReceitaPorCategoria =
                    CriarReceitaPorCategoria(pagos),

                StatusPagamentos =
                    CriarResumoStatus(lista),

                ReceitaPorFormaPagamento =
                    CriarReceitaPorFormaPagamento(pagos),

                TopClientes =
                    CriarTopClientes(pagos),

                ReceitaPorEmpresa =
                    CriarReceitaPorEmpresa(pagos),

                ReceitaPorProfissional =
                    CriarReceitaPorProfissional(pagos),

                ReceitaMensal =
                    CriarReceitaMensal(pagos)
            };

            PreencherDestaques(resultado, pagos);

            return resultado;
        }

        public void LimparCache()
        {
            cachePagamentos = null;
            dataCache = DateTime.MinValue;
        }

        private FinanceiroItemViewModel MapearPagamento(
            Pagamento pagamento)
        {
            DateTime dataReferencia =
                pagamento.DataPagamento ??
                pagamento.DataVencimento;

            string empresa =
                pagamento.Empresa?.NomeFantasia ??
                string.Empty;

            string profissional =
                pagamento.Profissional?.Nome ??
                string.Empty;

            return new FinanceiroItemViewModel
            {
                Id = pagamento.Id,

                Cliente =
                    pagamento.Cliente?.Nome ??
                    "Cliente não informado",

                Origem = ObterOrigem(
                    empresa,
                    profissional),

                Categoria =
                    pagamento.CategoriaPagamento?.Nome ??
                    "Não informado",

                FormaPagamento =
                    pagamento.FormaPagamento?.Nome ??
                    "Não informado",

                Status =
                    pagamento.StatusPagamento?.Nome ??
                    "Não informado",

                Valor = pagamento.Valor,

                ValorFormatado =
                    pagamento.Valor.ToString(
                        "C",
                        cultura),

                DataReferencia = dataReferencia,

                Data = dataReferencia.ToString(
                    "dd/MM/yyyy"),

                Observacoes =
                    string.IsNullOrWhiteSpace(
                        pagamento.Observacoes)
                        ? "Sem observações"
                        : pagamento.Observacoes,

                CorCategoria = ObterCorCategoria(
                    pagamento.CategoriaPagamento?.Nome),

                Empresa = empresa,

                Profissional = profissional
            };
        }

        private static FinanceiroItemViewModel CopiarItem(
            FinanceiroItemViewModel item)
        {
            return new FinanceiroItemViewModel
            {
                Id = item.Id,
                Cliente = item.Cliente,
                Origem = item.Origem,
                Categoria = item.Categoria,
                FormaPagamento = item.FormaPagamento,
                Status = item.Status,
                Valor = item.Valor,
                ValorFormatado = item.ValorFormatado,
                DataReferencia = item.DataReferencia,
                Data = item.Data,
                Observacoes = item.Observacoes,
                CorCategoria = item.CorCategoria,
                Empresa = item.Empresa,
                Profissional = item.Profissional
            };
        }

        private static IEnumerable<FinanceiroItemViewModel>
            AplicarFiltroPeriodo(
                IEnumerable<FinanceiroItemViewModel> lista,
                string? periodo)
        {
            DateTime hoje = DateTime.Today;

            return periodo switch
            {
                "Hoje" => lista.Where(p =>
                    p.DataReferencia.Date == hoje),

                "Este mês" => lista.Where(p =>
                    p.DataReferencia.Month == hoje.Month &&
                    p.DataReferencia.Year == hoje.Year),

                "Este ano" => lista.Where(p =>
                    p.DataReferencia.Year == hoje.Year),

                _ => lista
            };
        }

        private static bool ContemTexto(
            string? texto,
            string busca)
        {
            if (string.IsNullOrWhiteSpace(texto))
                return false;

            return texto.Contains(
                busca,
                StringComparison.OrdinalIgnoreCase);
        }

        private static bool StatusIgual(
            FinanceiroItemViewModel pagamento,
            string status)
        {
            return pagamento.Status.Equals(
                status,
                StringComparison.OrdinalIgnoreCase);
        }

        private static List<ResumoGraficoDto>
            CriarReceitaPorCategoria(
                List<FinanceiroItemViewModel> pagos)
        {
            return pagos
                .GroupBy(p => p.Categoria)
                .Select(g => new ResumoGraficoDto
                {
                    Nome = g.Key,
                    Valor = g.Sum(p => p.Valor),
                    Quantidade = g.Count()
                })
                .Where(x => x.Valor > 0)
                .OrderByDescending(x => x.Valor)
                .ToList();
        }

        private static List<ResumoGraficoDto>
            CriarResumoStatus(
                List<FinanceiroItemViewModel> pagamentos)
        {
            return pagamentos
                .GroupBy(p => p.Status)
                .Select(g => new ResumoGraficoDto
                {
                    Nome = g.Key,
                    Quantidade = g.Count(),
                    Valor = g.Sum(p => p.Valor)
                })
                .OrderByDescending(x => x.Quantidade)
                .ToList();
        }

        private static List<ResumoGraficoDto>
            CriarReceitaPorFormaPagamento(
                List<FinanceiroItemViewModel> pagos)
        {
            return pagos
                .GroupBy(p => p.FormaPagamento)
                .Select(g => new ResumoGraficoDto
                {
                    Nome = g.Key,
                    Valor = g.Sum(p => p.Valor),
                    Quantidade = g.Count()
                })
                .OrderByDescending(x => x.Valor)
                .ToList();
        }

        private static List<ResumoGraficoDto>
            CriarTopClientes(
                List<FinanceiroItemViewModel> pagos)
        {
            return pagos
                .GroupBy(p => p.Cliente)
                .Select(g => new ResumoGraficoDto
                {
                    Nome = g.Key,
                    Valor = g.Sum(p => p.Valor),
                    Quantidade = g.Count()
                })
                .OrderByDescending(x => x.Valor)
                .Take(10)
                .ToList();
        }

        private static List<ResumoGraficoDto>
            CriarReceitaPorEmpresa(
                List<FinanceiroItemViewModel> pagos)
        {
            return pagos
                .Where(p =>
                    !string.IsNullOrWhiteSpace(p.Empresa))
                .GroupBy(p => p.Empresa)
                .Select(g => new ResumoGraficoDto
                {
                    Nome = g.Key,
                    Valor = g.Sum(p => p.Valor),
                    Quantidade = g.Count()
                })
                .OrderByDescending(x => x.Valor)
                .Take(10)
                .ToList();
        }

        private static List<ResumoGraficoDto>
            CriarReceitaPorProfissional(
                List<FinanceiroItemViewModel> pagos)
        {
            return pagos
                .Where(p =>
                    !string.IsNullOrWhiteSpace(
                        p.Profissional))
                .GroupBy(p => p.Profissional)
                .Select(g => new ResumoGraficoDto
                {
                    Nome = g.Key,
                    Valor = g.Sum(p => p.Valor),
                    Quantidade = g.Count()
                })
                .OrderByDescending(x => x.Valor)
                .Take(10)
                .ToList();
        }

        private List<ResumoMensalDto> CriarReceitaMensal(
            List<FinanceiroItemViewModel> pagos)
        {
            int anoAtual = DateTime.Today.Year;

            return Enumerable.Range(1, 12)
                .Select(mes => new ResumoMensalDto
                {
                    NumeroMes = mes,

                    Mes = new DateTime(
                            anoAtual,
                            mes,
                            1)
                        .ToString("MMM", cultura)
                        .Replace(".", ""),

                    Valor = pagos
                        .Where(p =>
                            p.DataReferencia.Year == anoAtual &&
                            p.DataReferencia.Month == mes)
                        .Sum(p => p.Valor)
                })
                .ToList();
        }

        private void PreencherDestaques(
            DashboardFinanceiroDto dto,
            List<FinanceiroItemViewModel> pagos)
        {
            var melhorMes = pagos
                .GroupBy(p => new
                {
                    p.DataReferencia.Year,
                    p.DataReferencia.Month
                })
                .Select(g => new
                {
                    g.Key.Year,
                    g.Key.Month,
                    Valor = g.Sum(p => p.Valor)
                })
                .OrderByDescending(x => x.Valor)
                .FirstOrDefault();

            if (melhorMes != null)
            {
                string nomeMes = new DateTime(
                        melhorMes.Year,
                        melhorMes.Month,
                        1)
                    .ToString("MMMM", cultura);

                dto.MelhorMes =
                    $"{PrimeiraMaiuscula(nomeMes)} " +
                    $"({melhorMes.Valor.ToString("C", cultura)})";
            }

            var categoria =
                dto.ReceitaPorCategoria.FirstOrDefault();

            if (categoria != null)
            {
                dto.CategoriaDestaque =
                    $"{categoria.Nome} " +
                    $"({categoria.Valor.ToString("C", cultura)})";
            }

            var forma = pagos
                .GroupBy(p => p.FormaPagamento)
                .Select(g => new
                {
                    Nome = g.Key,
                    Quantidade = g.Count()
                })
                .OrderByDescending(x => x.Quantidade)
                .FirstOrDefault();

            if (forma != null)
            {
                dto.FormaMaisUsada =
                    $"{forma.Nome} ({forma.Quantidade})";
            }

            var cliente =
                dto.TopClientes.FirstOrDefault();

            if (cliente != null)
            {
                dto.ClienteDestaque =
                    $"{cliente.Nome} " +
                    $"({cliente.Valor.ToString("C", cultura)})";
            }
        }

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

        private static string PrimeiraMaiuscula(
            string texto)
        {
            if (string.IsNullOrWhiteSpace(texto))
                return texto;

            return char.ToUpper(texto[0]) +
                   texto[1..];
        }
    }
}