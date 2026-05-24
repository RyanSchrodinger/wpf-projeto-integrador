using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wpf_projeto_integrador.Models
{
    public class Cliente : Usuario
    {
        
        public string? Rua { get; set; }
        public string? Numero { get; set; }
        public string? Cidade { get; set; }
    }
}
