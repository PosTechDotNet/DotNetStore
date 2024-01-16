using DotNetStoreDurableFunction.Models;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace DotNetStoreDurableFunction.Data
{
    public class DataContextMock : IDataContextMock
    {
        public List<Usuario> Usuarios { get; set; }
        public List<Produto> Produtos { get; set; }
        public List<Endereco> Enderecos { get; set; }

       
        public List<Usuario> ListarUsuarios()
        {
            return Usuarios = CarregarDadosJson<DataContextMock>("Data/Usuario.json").Usuarios;
        }

        public List<Endereco> ListarEnderecos()
        {
            return Enderecos = CarregarDadosJson<DataContextMock>("Data/Endereco.json").Enderecos;
        }

        public List<Produto> ListarProdutos()
        {
            return Produtos = CarregarDadosJson<DataContextMock>("Data/Produtos.json").Produtos;
        }

        public DataContextMock CarregarDadosJson<DataContextMock>(string caminho)
        {
            var json = File.ReadAllText(caminho);
            DataContextMock dadosMockados = JsonSerializer.Deserialize<DataContextMock>(json);
            return dadosMockados;
        }
        
    }
}
