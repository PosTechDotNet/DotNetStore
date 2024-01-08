using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace DotNetStoreDurableFunction.Models
{
    public class Pedido
    {
        public int Id { get; set; }
        public uint NumeroPedido { get; set; }
        public int UsuarioId { get; set; }
        public Usuario Usuario { get; set; }
        public int ProdutoId { get; set; }
        public List<Produto> Produtos { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal PrecoTotal { get; set; }
        public Endereco Endereco { get; set; }       

        public uint GerarNumeroPedido()
        {
            //Gera o número do pedido
            byte[] buffer = Guid.NewGuid().ToByteArray();
            var numeroPedido = BitConverter.ToUInt32(buffer, 8);
            return numeroPedido;
        }
    }
}
