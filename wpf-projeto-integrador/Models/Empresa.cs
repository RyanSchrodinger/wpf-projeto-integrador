using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wpf_projeto_integrador.Models
{
    public class Empresa : Usuario
    {
        public string Cnpj { get; set; }
        public string NomeFantasia { get; set; }
        public string? Telefone { get; set; }
        public string? Endereco { get; set; }

    }
}
