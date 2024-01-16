using DotNetStoreDurableFunction.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetStoreDurableFunction.Data
{
    public interface IDataContextMock
    {
        List<Usuario> ListarUsuarios();
        List<Endereco> ListarEnderecos();
        List<Produto> ListarProdutos();
    }
}
