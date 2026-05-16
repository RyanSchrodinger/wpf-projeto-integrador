using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Windows.Controls;

using wpf_projeto_integrador.Data;

namespace wpf_projeto_integrador.View
{
    /// <summary>
    /// Interação lógica para LogsControl.xam
    /// </summary>
    public partial class LogsControl : UserControl
    {
        public LogsControl()
        {
            InitializeComponent();
            CarregarLogs();
        }

        public void CarregarLogs()
        {
            using (var db = new MusicStationContext())
            {
                var logs = db.LogsSistema
                    .Include(l => l.Usuario)
                    .Include(l => l.TipoAcao)
                    .OrderByDescending(l => l.DataHora)
                    .ToList();

                DataContext = new
                {
                    Logs = logs
                };
            }
        }
    }
}
