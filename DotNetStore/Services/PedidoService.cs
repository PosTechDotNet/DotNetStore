using DotNetStoreDurableFunction.Data;
using DotNetStoreDurableFunction.DTO;
using DotNetStoreDurableFunction.Mapping;
using DotNetStoreDurableFunction.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DotNetStoreDurableFunction.Services
{
    public class PedidoService : IPedidoService
    {
        private readonly DataContext _dataContext;
        public PedidoService(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        public Pedido ObterUsuario(Pedido pedido)
        {
            var dadosUsuario = _dataContext.Usuarios.Select(u => u).Where(u => u.Id == pedido.UsuarioId).FirstOrDefault();

            //popular dados dentro do pedido
            Pedido pedidoComDadosUsuario = new Pedido()
            {
                UsuarioId = dadosUsuario.Id,
                Usuario = new Usuario
                {
                    Id = dadosUsuario.Id,
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
                var produto = _dataContext.Produtos
                                .Select(produto => produto)
                                .Where(produto => produto.SKU == item.SKU)
                                .FirstOrDefault();

                if (produto != null)
                    obterDadosProdutos.Add(produto);
            }

            //Calcula o total do preços dos produtos
            double precoTotal = 0;
            obterDadosProdutos.ForEach(produto => precoTotal += produto.ValorUnitario * produto.Quantidade);

            //popular dados dentro do pedido
            var pedidoComDadosProdutos = new Pedido()
            {
                UsuarioId = pedido.Usuario.Id,
                Usuario = new Usuario
                {
                    Id = pedido.Usuario.Id,
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

            //Vai na tabela de usuário para pegar o seu endereco a partir do Id
            var obterDadosEndereco = _dataContext.Usuarios
                .Include(u => u.Endereco)
                .Where(u => u.Id == pedido.UsuarioId).FirstOrDefault();


            //Calcula total dos valor dos produtos + frete
            var totalProdutoFrete = pedido.PrecoTotal + obterDadosEndereco.Endereco.ValorFrete;

            //popular dados dentro do pedido
            var pedidoComDadosEndereco = new Pedido()
            {
                UsuarioId = pedido.Id,
                Usuario = new Usuario
                {
                    Id = obterDadosEndereco.Id,
                    CPF = obterDadosEndereco.CPF,
                    Nome = obterDadosEndereco.Nome
                },
                Produtos = pedido.Produtos,
                PrecoTotal = totalProdutoFrete,
                Endereco = obterDadosEndereco.Endereco

            };

            return pedidoComDadosEndereco;
        }

        public Pedido SalvarPedido(Pedido pedido)
        {

            //Mapear para uma classe com os campos necessários na tabela Pedido
            // Id - NumeroPedido - Cliente - Produtos - Endereco - PrecoTotal
            var salvarPedido = pedido.MapearParaCadastrarPedidoDto();

            //Salva na tabela pedido no Azure Table Storage
            SalvarNaTabelaAzure(salvarPedido);

            return pedido;
        }

        private static void SalvarNaTabelaAzure(CadastrarPedidoDto salvarPedido)
        {
            // Substitua pela sua string de conexão
            var connectionString = Environment.GetEnvironmentVariable("AzTbStorageConnectionString");

            CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(connectionString);

            CloudTableClient cloudTableClient = cloudStorageAccount.CreateCloudTableClient();

            CloudTable cloudTable = cloudTableClient.GetTableReference("pedidos");

            cloudTable.CreateIfNotExistsAsync();

            TableOperation tableOperation = TableOperation.Insert(salvarPedido);

            cloudTable.ExecuteAsync(tableOperation);

        }
    }
}
