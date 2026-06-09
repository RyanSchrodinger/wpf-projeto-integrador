using Microsoft.EntityFrameworkCore;
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


        //Importância do uso de procedures como segurança para métodos de deburação, pois aparentemente conseguimos ter acesso em tempo real o que está acontecendo dentro da memória
        // Isso envolve testes, caixa preta, caixa branca


        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlServer(
        //          "Server=tcp:musicstation-db1.database.windows.net,1433;" +
        //            "Initial Catalog=MusicStation;" +
        //            "User ID=ryan;" +
        //            "Password=W@choswick01;" +
        //            "Encrypt=True;" +
        //            "TrustServerCertificate=False;",
        //            options =>
        //            {
        //                options.EnableRetryOnFailure();
        //            });
        //}
    }
}
