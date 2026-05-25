using System.Security.Cryptography.Pkcs;
using wpf_projeto_integrador.Data;
using wpf_projeto_integrador.Models;

namespace wpf_projeto_integrador.Helpers
{
    public static class GerenciadorLogs
    {
        public static void FazerRegistro(MusicStationContext db, int? usuarioId, TipoAcaoLog tipoAcao, string descricao, string? entidade = null, int? entidadeId = null
            ,bool sucesso = true, string? erro = null, string? tela = null)
        {
            var log = new LogSistema
            {
                UsuarioId = usuarioId,
                
                TipoAcao = tipoAcao,
                Descricao = descricao,
                EntidadeAfetada = entidade,
                EntidadeId = entidadeId,
                NomeComputador = Environment.MachineName,
                Tela = tela,
                Sucesso = sucesso,
                Erro = erro,

            };
            db.LogsSistema.Add(log);


        }


    }
}
