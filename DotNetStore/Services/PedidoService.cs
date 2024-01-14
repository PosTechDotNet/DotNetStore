using DotNetStoreDurableFunction.Data;
using DotNetStoreDurableFunction.DTO;
using DotNetStoreDurableFunction.Mapping;
using DotNetStoreDurableFunction.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Protocols;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetStoreDurableFunction.Services
{
    public class PedidoService : IPedidoService
    {
        private readonly DataContext _dataContext;
        private readonly List<Usuario> _usuarios;
        private readonly List<Endereco> _enderecos;
        private readonly List<Produto> _produtos;
        public PedidoService(DataContext dataContext)
        {
            _dataContext = dataContext;

            _usuarios = CarregarDadosJson<List<Usuario>>("Data/Usuario.json");
            _enderecos = CarregarDadosJson<List<Endereco>>("Data/Endereco.json");
            _produtos = CarregarDadosJson<List<Produto>>("Data/Produtos.json");
        }

        private T CarregarDadosJson<T>(string caminho)
        {
            var json = File.ReadAllText(caminho);
            return JsonConvert.DeserializeObject<T>(json);
        }
        public Pedido ObterUsuario(Pedido pedido)
        {
            var dadosUsuario = _usuarios.FirstOrDefault(u => u.Id == pedido.UsuarioId);

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
                var produto = _produtos.FirstOrDefault(p => p.SKU == item.SKU);

                if (produto != null)
                    obterDadosProdutos.Add(produto);
            }           

            //Calcula o total do preços dos produtos
            decimal precoTotal = 0;
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
            // Obtém o usuário da lista com o ID correspondente
            var usuario = _usuarios.FirstOrDefault(u => u.Id == pedido.UsuarioId);

            if (usuario != null)
            {
                // Calcula total dos valor dos produtos + frete
                decimal totalProdutoFrete = pedido.PrecoTotal + usuario.Endereco.ValorFrete;

                // Popula dados dentro do pedido
                var pedidoComDadosEndereco = new Pedido()
                {
                    UsuarioId = pedido.UsuarioId,
                    Usuario = new Usuario
                    {
                        Id = usuario.Id,
                        CPF = usuario.CPF,
                        Nome = usuario.Nome
                    },
                    Produtos = pedido.Produtos,
                    PrecoTotal = totalProdutoFrete,
                    Endereco = usuario.Endereco
                };

                return pedidoComDadosEndereco;
            }
            return null;
        }

        public async Task<CadastrarPedidoDto> SalvarPedido(Pedido pedido)
        {

            //Mapear para uma classe com os campos necessários na tabela Pedido
            // Id - NumeroPedido - Usuario - Produtos - Endereco - PrecoTotal
            var salvarPedido = pedido.MapearParaCadastrarPedidoDto();

            //Salva na tabela pedido no Azure Table Storage
            SalvaNaTabelaAzure(salvarPedido);
            
            return salvarPedido;
        }

        private static CadastrarPedidoDto SalvaNaTabelaAzure(CadastrarPedidoDto salvarPedido)
        {

            var connectionString = ""; // Substitua pela sua string de conexão
            
            CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(connectionString);
            
            CloudTableClient cloudTableClient = cloudStorageAccount.CreateCloudTableClient();
            
            CloudTable cloudTable = cloudTableClient.GetTableReference("pedidos");
            
            cloudTable.CreateIfNotExistsAsync();            
            
            TableOperation tableOperation = TableOperation.Insert(salvarPedido);
            
            cloudTable.ExecuteAsync(tableOperation);
            
            return salvarPedido;
           
        }
    }
}
