using DotNetStoreDurableFunction;
using DotNetStoreDurableFunction.Data;
using DotNetStoreDurableFunction.Services;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(Startup))]
namespace DotNetStoreDurableFunction
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddScoped<IPedidoService, PedidoService>();
            builder.Services.AddScoped<IDataContextMock, DataContextMock>();
        }
    }
}
