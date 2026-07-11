using System.Collections.Generic;

namespace wpf_projeto_integrador.DTOs.Financeiro
{
    public class DashboardFinanceiroDto
    {
        public decimal TotalRecebido { get; set; }

        public decimal ValorPendente { get; set; }

        public decimal ValorVencido { get; set; }

        public int QuantidadePagamentos { get; set; }

        public string MelhorMes { get; set; } = "Nenhum";

        public string CategoriaDestaque { get; set; } = "Nenhuma";

        public string FormaMaisUsada { get; set; } = "Nenhuma";

        public string ClienteDestaque { get; set; } = "Nenhum";

        public List<ResumoGraficoDto> ReceitaPorCategoria { get; set; } = new();

        public List<ResumoGraficoDto> StatusPagamentos { get; set; } = new();

        public List<ResumoGraficoDto> ReceitaPorFormaPagamento { get; set; } = new();

        public List<ResumoGraficoDto> TopClientes { get; set; } = new();

        public List<ResumoGraficoDto> ReceitaPorEmpresa { get; set; } = new();

        public List<ResumoGraficoDto> ReceitaPorProfissional { get; set; } = new();

        public List<ResumoMensalDto> ReceitaMensal { get; set; } = new();
    }

    public class ResumoGraficoDto
    {
        public string Nome { get; set; } = string.Empty;

        public decimal Valor { get; set; }

        public int Quantidade { get; set; }
    }

    public class ResumoMensalDto
    {
        public int NumeroMes { get; set; }

        public string Mes { get; set; } = string.Empty;

        public decimal Valor { get; set; }
    }
}