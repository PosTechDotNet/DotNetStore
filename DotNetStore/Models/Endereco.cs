namespace DotNetStoreDurableFunction.Models
{
    public class Endereco
    {
        public string Logradouro { get; set; }
        public string Bairro { get; set; }
        public string Cidade { get; set; }
        public string UF { get; set; }
        public int Numero { get; set; }
        public string Complemento { get; set; }
        public string CEP { get; set; }
        public decimal ValorFrete { get; set; }
        public int UsuarioId { get; set; }
    }
}
