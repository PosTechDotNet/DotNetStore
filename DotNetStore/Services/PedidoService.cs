using DotNetStoreDurableFunction.Data;
using DotNetStoreDurableFunction.DTO;
using DotNetStoreDurableFunction.Mapping;
using DotNetStoreDurableFunction.Models;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DotNetStoreDurableFunction.Services
{
    public class PedidoService : IPedidoService
    {
        private readonly IDataContextMock _dataContextMock;
        public PedidoService(IDataContextMock dataContextMock)
        {
            _dataContextMock = dataContextMock;
        }


        public Pedido ObterUsuario(Pedido pedido)
        {

            var dadosUsuario = _dataContextMock.ListarUsuarios().FirstOrDefault(u => u.Id == pedido.UsuarioId);

            Pedido pedidoComDadosUsuario = new Pedido()
            {
                UsuarioId = dadosUsuario.Id,
                Usuario = new Usuario
                {
                    CPF = dadosUsuario.CPF,
                    Nome = dadosUsuario.Nome
                },
                Produtos = pedido.Produtos,
                PrecoTotal = 0,
                Endereco = pedido.Endereco

            };

            return pedidoComDadosUsuario;
        }

        public Pedido ObterProdutos(Pedido pedido)
        {
            var obterDadosProdutos = new List<Produto>();
            foreach (var item in pedido.Produtos)
            {
                var produto = _dataContextMock.ListarProdutos().FirstOrDefault(p => p.SKU == item.SKU);
                if (produto != null)
                    obterDadosProdutos.Add(produto);
            }

            double precoTotal = 0;
            obterDadosProdutos.ForEach(produto => precoTotal += produto.ValorUnitario * produto.Quantidade);

            var pedidoComDadosProdutos = new Pedido()
            {
                UsuarioId = pedido.UsuarioId,
                Usuario = new Usuario
                {
                    CPF = pedido.Usuario.CPF,
                    Nome = pedido.Usuario.Nome
                },
                Produtos = obterDadosProdutos,
                PrecoTotal = precoTotal,
                Endereco = pedido.Endereco

            };

            return pedidoComDadosProdutos;
        }
        
        public Pedido ObterEndereco(Pedido pedido)
        {
            var enderecoUsuario = _dataContextMock.ListarEnderecos().FirstOrDefault(u => u.UsuarioId == pedido.UsuarioId);

            if (enderecoUsuario != null)
            {
                double totalProdutoFrete = pedido.PrecoTotal + enderecoUsuario.ValorFrete;

                var pedidoComDadosEndereco = new Pedido()
                {
                    UsuarioId = pedido.UsuarioId,
                    Usuario = new Usuario
                    {
                        CPF = pedido.Usuario.CPF,
                        Nome = pedido.Usuario.Nome
                    },
                    Produtos = pedido.Produtos,
                    PrecoTotal = totalProdutoFrete,
                    Endereco = enderecoUsuario
                };

                return pedidoComDadosEndereco;
            }
            return null;
        }

        public Pedido SalvarPedido(Pedido pedido)
        {

            var salvarPedido = pedido.MapearParaCadastrarPedidoDto();

            SalvarNaTabelaAzure(salvarPedido);
            
            pedido.NumeroPedido = salvarPedido.NumeroPedido;
            
            return pedido;
        }

        private static void SalvarNaTabelaAzure(CadastrarPedidoDto salvarPedido)
        {
            var connectionString = Environment.GetEnvironmentVariable("AzTbStorageConnectionString");

            CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(connectionString);

            CloudTableClient cloudTableClient = cloudStorageAccount.CreateCloudTableClient();

            CloudTable cloudTable = cloudTableClient.GetTableReference("Pedidos");

            cloudTable.CreateIfNotExistsAsync();

            TableOperation tableOperation = TableOperation.Insert(salvarPedido);

            cloudTable.ExecuteAsync(tableOperation);

        }
    }
}
