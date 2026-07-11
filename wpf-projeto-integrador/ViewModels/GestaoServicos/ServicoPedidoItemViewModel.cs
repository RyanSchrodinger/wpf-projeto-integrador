using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wpf_projeto_integrador.ViewModels.GestaoServicos
{
    public class ServicoPedidoItemViewModel
    {
        public int Id { get; set; }

        public int PedidoId { get; set; }

        public string Cliente { get; set; } = string.Empty;

        public string Servico { get; set; } = string.Empty;

        public string Profissional { get; set; } = string.Empty;

        public string Prestador { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;

        public decimal Valor { get; set; }

        public string ValorFormatado { get; set; } = string.Empty;

        public DateTime DataReferencia { get; set; }

        public string Data { get; set; } = string.Empty;

        public string Observacao { get; set; } = string.Empty;
    }
}
