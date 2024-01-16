using System.ComponentModel.DataAnnotations.Schema;

namespace DotNetStoreDurableFunction.Models
{
    public class Produto
    {
        public int Id { get; set; }
        public int SKU { get; set; }
        public string Descricao { get; set; }
        public int Quantidade { get; set; }
        public double ValorUnitario { get; set; }
    }
}
