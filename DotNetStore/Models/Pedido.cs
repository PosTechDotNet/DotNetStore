using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace DotNetStoreDurableFunction.Models
{
    public class Pedido
    {
        public int Id { get; set; }
        public int NumeroPedido { get; set; }
        public int UsuarioId { get; set; }
        public Usuario Usuario { get; set; }
        public int ProdutoId { get; set; }
        public List<Produto> Produtos { get; set; }
        public double PrecoTotal { get; set; }
        public Endereco Endereco { get; set; }

        public int GerarNumeroPedido()
        {
            //Gera o número do pedido
            byte[] buffer = Guid.NewGuid().ToByteArray();
            var numeroPedido = BitConverter.ToInt32(buffer, 8);

            if (numeroPedido < 0)
            {
                return numeroPedido * -1;
            }

            return numeroPedido;
        }
    }
}
