namespace wpf_projeto_integrador.Models
{
    public class ItemLocacao
    {
        public int IdItemLocacao { get; set; }

        public int LocacaoId { get; set; }

        public int EquipamentoId { get; set; }

        public int Quantidade { get; set; }

        public decimal Valor { get; set; }

        public decimal ValorTotal { get; set; }

        public Locacao Locacao { get; set; } = null!;

        public Equipamento Equipamento { get; set; } = null!;
    }
}