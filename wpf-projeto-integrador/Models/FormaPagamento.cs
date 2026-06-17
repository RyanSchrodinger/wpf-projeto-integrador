using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wpf_projeto_integrador.Models
{
    public class FormaPagamento
    {
        public int Id { get; set; }
        public string Nome { get; set; }

        public ICollection<Pagamento> Pagamentos { get; set; } = new List<Pagamento>();
    }
}
