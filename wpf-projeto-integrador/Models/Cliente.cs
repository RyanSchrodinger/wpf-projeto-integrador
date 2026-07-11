using System.ComponentModel.DataAnnotations;

namespace wpf_projeto_integrador.Models
{
    public class Cliente : Usuario
    {
        public string? Rua { get; set; }

        public string? Numero { get; set; }

        public string? Cidade { get; set; }

        public string? Bairro { get; set; }

        public string? Cep { get; set; }

        public string? Estado { get; set; }
        public ICollection<Avaliacao> Avaliacoes { get; set; } = new List<Avaliacao>();
        public ICollection<Pedido> Pedidos { get; set; }   = new List<Pedido>();
        public ICollection<Locacao> Locacoes { get; set; } =new List<Locacao>();
        public ICollection<Pagamento> Pagamentos { get; set; }  = new List<Pagamento>();

       
    }
}