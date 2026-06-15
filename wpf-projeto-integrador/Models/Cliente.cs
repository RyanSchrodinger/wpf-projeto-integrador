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
    }
}