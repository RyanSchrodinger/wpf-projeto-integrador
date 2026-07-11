using DocumentFormat.OpenXml.Drawing.Charts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wpf_projeto_integrador.Models
{
    public class ServicoPedido
    {
        public int IdItem { get; set; }

        public int PedidoId { get; set; }

        public int ServicoId { get; set; }

        public int ProfissionalId { get; set; }

        public decimal ValorServico { get; set; }

        public string Observacao { get; set; } = "-";

        public StatusServicoPedido Status { get; set; } = StatusServicoPedido.Pendente;

        public Pedido Pedido { get; set; } = null!;

        public Servico Servico { get; set; } = null!;

        public Profissional Profissional { get; set; } = null!;

        public Avaliacao? Avaliacao { get; set; }
    }
}
