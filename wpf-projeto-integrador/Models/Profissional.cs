    using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wpf_projeto_integrador.Models
{
    public class Profissional : Usuario
    {
        public string Descricao { get; set; }

        public string? Especialidade { get; set; }

        public string? Telefone { get; set; }

        public string? Endereco { get; set; }

        public bool Ativo { get; set; }

        public int? EmpresaId { get; set; }
        public Empresa? Empresa { get; set; }
    }
}
