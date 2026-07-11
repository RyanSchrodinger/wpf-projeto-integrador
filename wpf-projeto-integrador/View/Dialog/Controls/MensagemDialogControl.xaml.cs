using System;
using System.Collections.Generic;
using System.Linq;

using System.Windows.Controls;


namespace wpf_projeto_integrador.View.Dialog.Controls
{
    /// <summary>
    /// Interação lógica para MensagemDialogControl.xam
    /// </summary>
    public partial class MensagemDialogControl : UserControl
    {
        public MensagemDialogControl(string mensagem)
        {
            InitializeComponent();
            TxtMensagem.Text = mensagem;
        }
    }
}
