using System;
using System.Collections.Generic;

namespace wpf_projeto_integrador.Models
{
    public class Locacao
    {
        public int IdLocacao { get; set; }

        public int ClienteId { get; set; }

        public DateTime DataInicio { get; set; }

        public DateTime DataFim { get; set; }

        public DateTime DataLocacao { get; set; }

        public decimal ValorTotal { get; set; }

        public StatusLocacao Status { get; set; }

        public string? Observacao { get; set; }

        public Cliente Cliente { get; set; } = null!;

        public ICollection<ItemLocacao> ItensLocacao { get; set; } =
            new List<ItemLocacao>();
    }

    public enum StatusLocacao
    {
        Pendente = 1,
        Confirmada = 2,
        EmAndamento = 3,
        Concluida = 4,
        Cancelada = 5
    }
}