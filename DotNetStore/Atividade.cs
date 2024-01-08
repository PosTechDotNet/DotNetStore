using DotNetStoreDurableFunction.DTO;
using DotNetStoreDurableFunction.Models;
using DotNetStoreDurableFunction.Services;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DotNetStoreDurableFunction
{
    public class Atividade
    {
        private readonly IPedidoService _pedidoService;
        public Atividade(IPedidoService pedidoService)
        {
            _pedidoService = pedidoService;
        }

        [FunctionName(nameof(IPedidoService.ObterUsuario))]
        public Pedido ObterUsuario([ActivityTrigger] Pedido pedido, ILogger log)
        {
            log.LogInformation("Montando pedido com os dados do Usuário");

            var dadosUsuario = _pedidoService.ObterUsuario(pedido);

            return dadosUsuario;
        }

        [FunctionName(nameof(IPedidoService.ObterProdutos))]
        public Pedido ObterProdutos([ActivityTrigger] Pedido pedido, ILogger log)
        {
            log.LogInformation("Montando pedido com os produtos");

            var dadosProdutos = _pedidoService.ObterProdutos(pedido);

            return dadosProdutos;
        }

        [FunctionName(nameof(IPedidoService.ObterEndereco))]
        public Pedido ObterEndereco([ActivityTrigger] Pedido pedido, ILogger log)
        {
            log.LogInformation("Montando pedido com os produtos");

            var dadosEndereco = _pedidoService.ObterEndereco(pedido);

            return dadosEndereco;
        }

        [FunctionName(nameof(IPedidoService.SalvarPedido))]
        public async Task<CadastrarPedidoDto> SalvarPedido([ActivityTrigger] Pedido pedido, ILogger log)
        {
            log.LogInformation("Salvando o pedido");

            var salvaPedido = await _pedidoService.SalvarPedido(pedido);

            //log.LogInformation($"Pedido {salvaPedido.NumeroPedido} recebido por {salvaPedido.Usuario.Nome} foi salvo!");

            return salvaPedido;
        }


    }
}
