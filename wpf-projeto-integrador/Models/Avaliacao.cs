using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wpf_projeto_integrador.Models
{
    public class Avaliacao
    {
        public int Id { get; set; }

        [Required]
        public int ServicoPedidoId { get; set; }

        public ServicoPedido ServicoPedido { get; set; } = null!;

        [Required]
        public int ClienteId { get; set; }

        public Cliente Cliente { get; set; } = null!;

        [Required]
        [Range(1, 5, ErrorMessage = "A nota deve ser entre 1 e 5.")]
        public int Nota { get; set; }

        [MaxLength(300)]
        public string? Comentario { get; set; }

        public DateTime DataAvaliacao { get; set; } = DateTime.Now;
    }
}
