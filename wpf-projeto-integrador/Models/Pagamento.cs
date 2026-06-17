using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wpf_projeto_integrador.Models
{
    public class Pagamento
    {
        public int Id { get; set; }

        public decimal Valor { get; set; }

        public DateTime DataVencimento { get; set; }
        public DateTime? DataPagamento { get; set; }

        public int FormaPagamentoId { get; set; }
        public FormaPagamento FormaPagamento { get; set; }

        public int StatusPagamentoId { get; set; }
        public StatusPagamento StatusPagamento { get; set; }

        public int CategoriaPagamentoId { get; set; }
        public CategoriaPagamento CategoriaPagamento { get; set; }

        public string? Observacoes { get; set; }

        public int ClienteId { get; set; }
        public Cliente Cliente { get; set; }

        public int? EmpresaId { get; set; }
        public Empresa? Empresa { get; set; }

        public int? ProfissionalId { get; set; }
        public Profissional? Profissional { get; set; }
    }
}
