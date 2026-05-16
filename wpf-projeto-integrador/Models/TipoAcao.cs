using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wpf_projeto_integrador.Models
{
    public class TipoAcao
    {
        public int Id { get; set; }


        [Required]
        [MaxLength(100)]
        public string Nome { get; set; }

        public ICollection<LogSistema> Logs { get; set; } = new List<LogSistema>();
    }
}
