using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wpf_projeto_integrador.ViewModels.Financeiro
{
    public class FinanceiroItemViewModel
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

        public string Empresa { get; set; } = string.Empty;

        public string Profissional { get; set; } = string.Empty;
    }
}
