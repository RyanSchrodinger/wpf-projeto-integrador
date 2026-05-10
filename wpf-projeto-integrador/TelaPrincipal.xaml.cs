using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using wpf_projeto_integrador.Models;

namespace wpf_projeto_integrador
{
    /// <summary>
    /// Lógica interna para TelaPrincipal.xaml
    /// </summary>
    public partial class TelaPrincipal : Window
    {
        private Administrador _administrador;
        public TelaPrincipal(Administrador adm)
        {
            InitializeComponent();
            _administrador = adm;
            txtBemVindo.Text = $"Bem-vindo, {_administrador.Nome}!";
        }

        
    }
}
