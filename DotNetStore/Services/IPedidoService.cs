using DotNetStoreDurableFunction.DTO;
using DotNetStoreDurableFunction.Models;
using Microsoft.Azure.WebJobs;
using System.Threading.Tasks;

namespace DotNetStoreDurableFunction.Services
{
    public interface IPedidoService
    {
        Pedido ObterUsuario(Pedido pedido);
        Pedido ObterProdutos(Pedido pedido);
        Pedido ObterEndereco(Pedido pedido);
        Task<CadastrarPedidoDto> SalvarPedido(Pedido pedido);
    }
}
