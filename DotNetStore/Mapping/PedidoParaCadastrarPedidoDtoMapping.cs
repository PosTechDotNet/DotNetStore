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
                NumeroPedido = pedido.GerarNumeroPedido(),
                Nome = pedido.Usuario.Nome,
                CPF = pedido.Usuario.CPF,
                Produtos = MontarProdutos(pedido.Produtos),
                Endereco = MontarEndereco(pedido.Endereco),
                PrecoTotal = pedido.PrecoTotal,
            };
        }

        private static string MontarEndereco(Endereco endereco)
        {
            string enderecoCliente = string.Concat(endereco.Logradouro, ", ",
                endereco.Numero.ToString(), ", ",
                endereco.Bairro, ", ",
                endereco.Cidade, ", ",
                endereco.UF, ", ",
                endereco.CEP, ", ",
                endereco.Complemento
            );

            return enderecoCliente;

        }

        private static string MontarProdutos(List<Produto> produtos)
        {
            StringBuilder produtosCliente = new StringBuilder();
            foreach (var produto in produtos)
            {
                produtosCliente
                    .Append("[SKU: ")
                    .Append(produto.SKU)
                    .Append(", Descrição: ")
                    .Append(produto.Descricao)
                    .Append(", Qtd: ")
                    .Append(produto.Quantidade.ToString())
                    .Append(", Valor: ") 
                    .Append(produto.ValorUnitario.ToString())
                    .Append("] ");
            };

            return produtosCliente.ToString();
        }
    }
}
