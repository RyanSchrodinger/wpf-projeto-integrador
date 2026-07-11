using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wpf_projeto_integrador.Models
{
    public class Servico
    {
        public int IdServico { get; set; }

        public int? EmpresaId { get; set; }
        public int? ProfissionalId { get; set; }
        public Profissional? Profissional { get; set; }

        public string Nome { get; set; } = string.Empty;

        public string Descricao { get; set; } = string.Empty;

        public decimal Preco { get; set; }

        public bool Ativo { get; set; } = true;

        public Empresa? Empresa { get; set; } = null!;

        public ICollection<ServicoPedido> ServicosPedidos { get; set; }
            = new List<ServicoPedido>();
    }
}
