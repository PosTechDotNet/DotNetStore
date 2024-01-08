using DotNetStoreDurableFunction.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;

namespace DotNetStoreDurableFunction
{
    public class Encadeamento
    {
        [FunctionName($"{nameof(Atividade)}_HttpStart")]
        public async Task<HttpResponseMessage> HttpStart(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequestMessage req,
            [DurableClient] IDurableOrchestrationClient starter,
            ILogger log)
        {
            string jsonContent = await req.Content.ReadAsStringAsync();
            var pedido = JsonConvert.DeserializeObject<Pedido>(jsonContent);

            string instanceId = await starter.StartNewAsync(nameof(Orquestrador), pedido);

            log.LogInformation("Inicio do Orquestrador para processamento de um pedido com o ID = '{instanceId}'.", instanceId);

            return starter.CreateCheckStatusResponse(req, instanceId);
        }
    }
}


