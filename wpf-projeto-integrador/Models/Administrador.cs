using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wpf_projeto_integrador.Models
{
    public class Administrador : Usuario
    {

        [Required]
        public NivelAcessoEnum NivelAcesso { get; set; }
        public string? Observacao { get; set; }


        public enum NivelAcessoEnum
        {
            Baixo,  
            Medio,
            Alto
        }
    }
}
