using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wpf_projeto_integrador.Models
{
    public class Pedido
    {
        public int IdPedido { get; set; }

        public int ClienteId { get; set; }

        public DateTime DataPedido { get; set; }

        public decimal Total { get; set; }

        public StatusPedido Status { get; set; } = StatusPedido.Pendente;

        // Navegação
        public Cliente Cliente { get; set; } = null!;

        public ICollection<ServicoPedido> ServicosPedidos { get; set; }  = new List<ServicoPedido>();

        public ICollection<Pagamento> Pagamentos { get; set; } = new List<Pagamento>();
    }

    public enum StatusPedido
    {
        Pendente,
        EmAndamento,
        Concluido,
        Cancelado
    }

    public enum StatusServicoPedido
    {
        Pendente,
        EmAndamento,
        Concluido,
        Cancelado
    }
}
