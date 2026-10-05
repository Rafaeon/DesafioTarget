using DesafioTarget.App.Models;

namespace DesafioTarget.App.Repositorio
{
    public interface IVendaRepositorio
    {
        List<VendaModel> BuscarTodos();
        decimal CalcularComissaoDaVenda(decimal valor);
        List<ResultadoComissaoModel> CalcularPorVendedor(List<VendaModel> vendas);
    }
}
