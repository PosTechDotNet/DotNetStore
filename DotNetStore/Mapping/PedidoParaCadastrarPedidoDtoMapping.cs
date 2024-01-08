using DotNetStoreDurableFunction.DTO;
using DotNetStoreDurableFunction.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetStoreDurableFunction.Mapping
{
    public static class PedidoParaCadastrarPedidoDtoMapping
    {
        public static CadastrarPedidoDto MapearParaCadastrarPedidoDto(this Pedido pedido)
        {
            return new CadastrarPedidoDto
            {
                //NumeroPedido = pedido.GerarNumeroPedido(),
                //Usuario = pedido.Usuario,
                //Produtos = pedido.Produtos,
                //Endereco = pedido.Endereco,
                PrecoTotal = pedido.PrecoTotal.ToString(),
            };
        }
    }
}
