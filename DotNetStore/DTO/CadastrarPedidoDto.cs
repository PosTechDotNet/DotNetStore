using DotNetStoreDurableFunction.Models;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetStoreDurableFunction.DTO
{
    public class CadastrarPedidoDto : TableEntity
    {
        //public int Id { get; set; }
        public int NumeroPedido { get; set; }
        //public int UsuarioId { get; set; }
        public string Nome { get; set; }
        public string CPF { get; set; }
        public string Produtos { get; set; }
        public double PrecoTotal { get; set; }
        public string Endereco { get; set; }

        public CadastrarPedidoDto()
        {
            PartitionKey = "pedidos";
            RowKey = Guid.NewGuid().ToString();
        }

    }
}
