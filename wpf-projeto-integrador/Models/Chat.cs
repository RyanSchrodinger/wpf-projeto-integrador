using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wpf_projeto_integrador.Models
{
    public class Chat
    {
        public int Id { get; set; }

        // Usuário que iniciou a conversa
        public int Usuario1Id { get; set; }
        public Usuario Usuario1 { get; set; }

        // Usuário que recebeu
        public int Usuario2Id { get; set; }
        public Usuario Usuario2 { get; set; }

        public DateTime DataCriacao { get; set; } = DateTime.Now;

        // Navegação
        public ICollection<Mensagem> Mensagens { get; set; }
            = new List<Mensagem>();
    }
}
