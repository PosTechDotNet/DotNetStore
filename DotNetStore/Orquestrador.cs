﻿using DotNetStoreDurableFunction.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace DotNetStoreDurableFunction
{
    public class Orquestrador
    {
        [FunctionName(nameof(Orquestrador))]
        public async Task<Pedido> RunOrchestrator(
            [OrchestrationTrigger] IDurableOrchestrationContext context, ILogger log)
        {
            var pedido = context.GetInput<Pedido>();

            try
            {
                var pedido1 = await context.CallActivityAsync<Pedido>("ObterUsuario", pedido);
                var pedido2 = await context.CallActivityAsync<Pedido>("ObterProdutos", pedido1);
                var pedido3 = await context.CallActivityAsync<Pedido>("ObterEndereco", pedido2);
                var pedido4 = await context.CallActivityAsync<Pedido>("SalvarPedido", pedido3);
                return pedido4;
            }
            catch (Exception ex)
            {
                log.LogInformation("Falha ao processar o pedido", ex);
                return default;
            }
        }
    }
}
