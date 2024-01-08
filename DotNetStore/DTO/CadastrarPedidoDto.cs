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
        //public uint NumeroPedido { get; set; }
        //public int UsuarioId { get; set; }
        //public Usuario Usuario { get; set; }
        //public List<Produto> Produtos { get; set; }
        //[Column(TypeName = "decimal(18,2)")]
        public string PrecoTotal { get; set; }
        //public Endereco Endereco { get; set; }

        public CadastrarPedidoDto()
        {
            PartitionKey = "pedidos";
            RowKey = Guid.NewGuid().ToString();
        }

    }
}
