using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using wpf_projeto_integrador.Models;
using wpf_projeto_integrador.View;

namespace wpf_projeto_integrador
{
    public partial class FormMenu : Window
    {

        public FormMenu()
        {
            InitializeComponent();


        }

        private void btnLogs_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new LogsControl();
        }
    }
        
}