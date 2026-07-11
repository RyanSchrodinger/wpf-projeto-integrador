using System;
using wpf_projeto_integrador.DTOs.Financeiro;
using wpf_projeto_integrador.Services.Financeiro;

namespace wpf_projeto_integrador.ViewModels.Financeiro
{
    public class DashboardFinanceiroViewModel
    {
        private readonly FinanceiroService _financeiroService;

        public DashboardFinanceiroDto Dados { get; private set; } = new();

        public DashboardFinanceiroViewModel()
        {
            _financeiroService = new FinanceiroService();
        }

        /// <summary>
        /// Carrega os dados analíticos do período selecionado.
        /// </summary>
        public DashboardFinanceiroDto CarregarDados(
            string periodo,
            bool forcarAtualizacao = false)
        {
            Dados = _financeiroService.ObterDashboard(
                periodo,
                forcarAtualizacao);

            return Dados;
        }

        /// <summary>
        /// Limpa o cache do módulo financeiro.
        /// </summary>
        public void LimparCache()
        {
            _financeiroService.LimparCache();
        }
    }
}