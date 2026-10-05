using DesafioTarget.App.Enums;
using DesafioTarget.App.Models;

namespace DesafioTarget.App.Repositorio
{
    public interface IEstoqueRepositorio
    {
        List<ProdutoEstoqueModel> Produtos { get; }
        List<MovimentacaoModel> Movimentacoes { get; }
        MovimentacaoModel Registrar(int codigoProduto, TipoMovimentacao tipo, int quantidade, string descricao);
    }
}
