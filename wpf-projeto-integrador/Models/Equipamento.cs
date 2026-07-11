using System.Collections.Generic;

namespace wpf_projeto_integrador.Models
{
    public class Equipamento
    {
        public int IdEquipamento { get; set; }

        public int EmpresaId { get; set; }

        public string Nome { get; set; } = string.Empty;

        public string? Descricao { get; set; }

        public decimal Valor { get; set; }

        public int QuantidadeTotal { get; set; }

        public int QuantidadeDisponivel { get; set; }

        public bool Ativo { get; set; } = true;

        public Empresa Empresa { get; set; } = null!;

        public ICollection<ItemLocacao> ItensLocacao { get; set; } =
            new List<ItemLocacao>();
    }
}