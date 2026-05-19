using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wpf_projeto_integrador.Models
{
    public class Mensagem
    {
        public int Id { get; set; }

        public int ChatId { get; set; }
        public Chat Chat { get; set; }

        // Quem enviou a mensagem
        public int RemetenteId { get; set; }
        public Usuario Remetente { get; set; }

        public string Texto { get; set; }

        public bool Visualizada { get; set; }

        public DateTime DataEnvio { get; set; } = DateTime.Now;

    }
}
