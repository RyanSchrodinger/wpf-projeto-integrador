    using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wpf_projeto_integrador.Models
{
    public class Profissional : Usuario
    {
        public int? EmpresaId { get; set; }

        public Empresa? Empresa { get; set; } = null!;
        public string Descricao { get; set; } = string.Empty;

        public string? Especialidade { get; set; }

        public string? Endereco { get; set; }

        public ICollection<ServicoPedido> ServicosPedidos { get; set; }
            = new List<ServicoPedido>();
        public ICollection<Servico> Servicos { get; set; } = new List<Servico>();


    }
}
