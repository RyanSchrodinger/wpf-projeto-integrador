using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wpf_projeto_integrador.Models
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string SenhaHash { get; set; }
        public string NomeUsuario  { get; set; }

        public string? Email { get; set; } 
        public DateTime DataCriacao { get; set; }


    }
}
