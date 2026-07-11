using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using wpf_projeto_integrador.Data;
using wpf_projeto_integrador.Models;
using wpf_projeto_integrador.ViewModels.GestaoServicos;

namespace wpf_projeto_integrador.Services.GestaoServicos
{
    public class GestaoServicosService
    {
        private readonly CultureInfo _cultura =
            new("pt-BR");

        /*
         * Cache simples para evitar várias consultas seguidas
         * ao banco enquanto o usuário utiliza os filtros.
         */
        private static List<ServicoPedidoItemViewModel>? _cachePedidos;

        private static DateTime _dataCache =
            DateTime.MinValue;

        private static readonly TimeSpan DuracaoCache =
            TimeSpan.FromMinutes(2);

        /// <summary>
        /// Retorna todos os itens de serviços dos pedidos.
        /// </summary>
        public List<ServicoPedidoItemViewModel> ObterServicosPedidos(
            bool forcarAtualizacao = false)
        {
            bool cacheValido =
                _cachePedidos != null &&
                DateTime.Now - _dataCache < DuracaoCache;

            if (!forcarAtualizacao && cacheValido)
            {
                return _cachePedidos!
                    .Select(CopiarItem)
                    .ToList();
            }

            using var db = new MusicStationContext();

            var itensBanco = db.ServicosPedidos
                .AsNoTracking()

                .Include(sp => sp.Pedido)
                    .ThenInclude(p => p.Cliente)

                .Include(sp => sp.Servico)
                    .ThenInclude(s => s.Empresa)

                .Include(sp => sp.Servico)
                    .ThenInclude(s => s.Profissional)

                .Include(sp => sp.Profissional)
                    .ThenInclude(p => p.Empresa)

                .OrderByDescending(sp => sp.Pedido.DataPedido)
                .ToList();

            _cachePedidos = itensBanco
                .Select(MapearItem)
                .ToList();

            _dataCache = DateTime.Now;

            return _cachePedidos
                .Select(CopiarItem)
                .ToList();
        }

        /// <summary>
        /// Retorna os serviços aplicando busca, status e período.
        /// </summary>
        public List<ServicoPedidoItemViewModel>
            ObterServicosPedidosFiltrados(
                string? busca,
                string? status,
                string? periodo,
                bool forcarAtualizacao = false)
        {
            var lista = ObterServicosPedidos(
                    forcarAtualizacao)
                .AsEnumerable();

            busca = busca?.Trim();

            if (!string.IsNullOrWhiteSpace(busca))
            {
                lista = lista.Where(item =>
                    ContemTexto(item.Cliente, busca) ||
                    ContemTexto(item.Servico, busca) ||
                    ContemTexto(item.Profissional, busca) ||
                    ContemTexto(item.Prestador, busca) ||
                    ContemTexto(item.Status, busca) ||
                    ContemTexto(item.Observacao, busca));
            }

            if (!string.IsNullOrWhiteSpace(status) &&
                status != "Todos")
            {
                lista = lista.Where(item =>
                    item.Status.Equals(
                        status,
                        StringComparison.OrdinalIgnoreCase));
            }

            lista = AplicarFiltroPeriodo(
                lista,
                periodo);

            return lista
                .OrderByDescending(item =>
                    item.DataReferencia)
                .ToList();
        }

        /// <summary>
        /// Retorna os status usados no filtro da tela.
        /// </summary>
        public List<string> ObterStatus()
        {
            return new List<string>
            {
                "Todos",
                "Pendente",
                "Em andamento",
                "Concluído",
                "Cancelado"
            };
        }

        /// <summary>
        /// Retorna a quantidade de serviços ativos cadastrados.
        /// </summary>
        public int ObterQuantidadeServicosAtivos()
        {
            using var db = new MusicStationContext();

            return db.Servicos
                .AsNoTracking()
                .Count(s => s.Ativo);
        }

        /// <summary>
        /// Retorna a média geral das avaliações.
        /// </summary>
        public decimal ObterMediaAvaliacoes()
        {
            using var db = new MusicStationContext();

            bool possuiAvaliacoes =
                db.Avaliacoes
                    .AsNoTracking()
                    .Any();

            if (!possuiAvaliacoes)
                return 0;

            return db.Avaliacoes
                .AsNoTracking()
                .Average(a => (decimal)a.Nota);
        }

        /// <summary>
        /// Limpa o cache para forçar nova consulta.
        /// </summary>
        public void LimparCache()
        {
            _cachePedidos = null;
            _dataCache = DateTime.MinValue;
        }

        /// <summary>
        /// Converte a entidade do banco para o ViewModel da tela.
        /// </summary>
        private ServicoPedidoItemViewModel MapearItem(
            ServicoPedido item)
        {
            string nomeProfissional =
                item.Profissional?.Nome ??
                "Não definido";

            string prestador =
                item.Servico?.Empresa?.NomeFantasia
                ?? item.Servico?.Profissional?.Nome
                ?? "Prestador não informado";

            string status =
                FormatarStatus(
                    item.Status.ToString());

            DateTime dataPedido =
                item.Pedido?.DataPedido ??
                DateTime.MinValue;

            return new ServicoPedidoItemViewModel
            {
                Id = item.IdItem,

                PedidoId =
                    item.PedidoId,

                Cliente =
                    item.Pedido?.Cliente?.Nome ??
                    "Cliente não informado",

                Servico =
                    item.Servico?.Nome ??
                    "Serviço não informado",

                Profissional =
                    nomeProfissional,

                Prestador = prestador,

                Status =
                    status,

                Valor =
                    item.ValorServico,

                ValorFormatado =
                    item.ValorServico.ToString(
                        "C",
                        _cultura),

                DataReferencia =
                    dataPedido,

                Data =
                    dataPedido == DateTime.MinValue
                        ? "Não informada"
                        : dataPedido.ToString(
                            "dd/MM/yyyy"),

                Observacao =
                    string.IsNullOrWhiteSpace(
                        item.Observacao)
                        ? "Sem observações"
                        : item.Observacao
            };
        }

        /// <summary>
        /// Cria uma cópia para não devolver diretamente o cache.
        /// </summary>
        private static ServicoPedidoItemViewModel CopiarItem(
            ServicoPedidoItemViewModel item)
        {
            return new ServicoPedidoItemViewModel
            {
                Id = item.Id,
                PedidoId = item.PedidoId,
                Cliente = item.Cliente,
                Servico = item.Servico,
                Profissional = item.Profissional,
                Prestador = item.Prestador,
                Status = item.Status,
                Valor = item.Valor,
                ValorFormatado = item.ValorFormatado,
                DataReferencia = item.DataReferencia,
                Data = item.Data,
                Observacao = item.Observacao
            };
        }

        /// <summary>
        /// Aplica o filtro de período.
        /// </summary>
        private static IEnumerable<ServicoPedidoItemViewModel>
            AplicarFiltroPeriodo(
                IEnumerable<ServicoPedidoItemViewModel> lista,
                string? periodo)
        {
            DateTime hoje =
                DateTime.Today;

            return periodo switch
            {
                "Hoje" => lista.Where(item =>
                    item.DataReferencia.Date ==
                    hoje),

                "Este mês" => lista.Where(item =>
                    item.DataReferencia.Month ==
                    hoje.Month &&
                    item.DataReferencia.Year ==
                    hoje.Year),

                "Este ano" => lista.Where(item =>
                    item.DataReferencia.Year ==
                    hoje.Year),

                _ => lista
            };
        }

        /// <summary>
        /// Faz a busca ignorando letras maiúsculas e minúsculas.
        /// </summary>
        private static bool ContemTexto(
            string? texto,
            string busca)
        {
            if (string.IsNullOrWhiteSpace(texto))
                return false;

            return texto.Contains(
                busca,
                StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Transforma o texto do enum em um texto amigável.
        /// </summary>
        private static string FormatarStatus(
            string status)
        {
            return status switch
            {
                "EmAndamento" => "Em andamento",
                "Concluido" => "Concluído",
                "Cancelado" => "Cancelado",
                _ => "Pendente"
            };
        }
    }
}