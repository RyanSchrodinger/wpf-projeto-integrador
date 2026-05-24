using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wpf_projeto_integrador.Models
{
    public class Usuario
    {

        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Nome { get; set; }

        [Required, MaxLength(255)]
        public string SenhaHash { get; set; }

        [Required,  MaxLength(20)]
        public string NomeUsuario  { get; set; }
        public string? Telefone {  get; set; }

        [Required]
        [EmailAddress]
        [StringLength(255)]
        public string Email { get; set; } 
        public bool Ativo { get; set; } = true;
        public DateTime DataCriacao { get; set; }

        public ICollection<LogSistema> Logs { get; set; } = new List<LogSistema>();


    }
}
