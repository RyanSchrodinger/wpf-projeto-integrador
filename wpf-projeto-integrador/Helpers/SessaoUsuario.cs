

using System.Runtime.CompilerServices;
using wpf_projeto_integrador.Models;

namespace wpf_projeto_integrador.Helpers
{
    public static class SessaoUsuario
    {
        public static Administrador? usuarioLogado {  get; private set; }

        public static void Iniciar(Administrador? adm) 
        {
            usuarioLogado = adm;
        }

        public static void Encerrar()
        {
            usuarioLogado = null;
        }

    }
}
