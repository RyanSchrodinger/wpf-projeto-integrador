using System;
using System.Windows;


namespace wpf_projeto_integrador.View.Dialog
{
    /// <summary>
    /// Lógica interna para DialogoCustomizado.xaml
    /// </summary>
    public partial class DialogoCustomizado : Window
    {
        public DialogoCustomizado(string titulo, object conteudo)
        {
            InitializeComponent();

            TxtTitulo.Text = titulo;
            DialogContent.Content = conteudo;
        }

        private void BtnOk_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        public static void Show(string titulo, object conteudo, Window owner = null)
        {
            var dialog = new DialogoCustomizado(titulo, conteudo);

            if (owner != null)
                dialog.Owner = owner;

            dialog.ShowDialog();
        }
    }
}

