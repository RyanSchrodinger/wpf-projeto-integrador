using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wpf_projeto_integrador.Models
{
    public class LogSistema
    {
        public int Id { get; set; }

        public int UsuarioId { get; set; }
        public Usuario Usuario { get; set; }

        public int TipoAcaoId { get; set; }
        public TipoAcao TipoAcao { get; set; }

        public string? Entidade { get; set; }
        public int? EntidadeId { get; set; }

        public string? NomeComputador { get; set; }


        public string Descricao { get; set; }
        public DateTime DataHora { get; set; }

    }
}
