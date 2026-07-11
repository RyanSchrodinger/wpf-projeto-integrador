using System;

namespace wpf_projeto_integrador.ViewModels.GestaoLocacoes
{
    public class LocacaoItemViewModel
    {
        public int Id { get; set; }

        public int PedidoId { get; set; }

        public string Cliente { get; set; } =
            string.Empty;

        public string ItemLocado { get; set; } =
            string.Empty;

        public string Prestador { get; set; } =
            string.Empty;

        public string Status { get; set; } =
            string.Empty;

        public decimal Valor { get; set; }

        public string ValorFormatado { get; set; } =
            string.Empty;

        public DateTime DataReferencia { get; set; }

        public string DataInicio { get; set; } =
            string.Empty;

        public string DataFim { get; set; } =
            string.Empty;

        public string Periodo { get; set; } =
            string.Empty;

        public string Observacao { get; set; } =
            string.Empty;
    }
}