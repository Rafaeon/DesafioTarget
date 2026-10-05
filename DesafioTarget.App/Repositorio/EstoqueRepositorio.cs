using System.Text.Json;
using System.Text.Json.Serialization;
using DesafioTarget.App.Enums;
using DesafioTarget.App.Models;

namespace DesafioTarget.App.Repositorio
{
    public class EstoqueRepositorio : IEstoqueRepositorio
    {
        private static readonly JsonSerializerOptions OpcoesJson = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true,
            Converters = { new JsonStringEnumConverter() }
        };

        private readonly string _caminhoJson;
        private readonly ArquivoEstoque _dados;

        public EstoqueRepositorio(string caminhoJson)
        {
            _caminhoJson = caminhoJson;
            var json = File.ReadAllText(caminhoJson);
            _dados = JsonSerializer.Deserialize<ArquivoEstoque>(json, OpcoesJson);

            if (_dados == null)
                throw new Exception("Houve um erro ao carregar o estoque!");
        }

        public List<ProdutoEstoqueModel> Produtos => _dados.Estoque;
        public List<MovimentacaoModel> Movimentacoes => _dados.Movimentacoes;

        public MovimentacaoModel Registrar(int codigoProduto, TipoMovimentacao tipo, int quantidade, string descricao)
        {
            if (quantidade <= 0)
                throw new Exception("A quantidade deve ser maior que zero!");

            if (string.IsNullOrWhiteSpace(descricao))
                throw new Exception("Informe uma descrição para a movimentação!");

            var produto = _dados.Estoque.FirstOrDefault(p => p.CodigoProduto == codigoProduto);

            if (produto == null)
                throw new Exception($"Produto {codigoProduto} não encontrado!");

            if (tipo == TipoMovimentacao.Saida && quantidade > produto.Estoque)
                throw new Exception($"Estoque insuficiente do produto {produto.DescricaoProduto}!");

            if (tipo == TipoMovimentacao.Entrada)
                produto.Estoque += quantidade;
            else
                produto.Estoque -= quantidade;

            var movimentacao = new MovimentacaoModel();
            movimentacao.Id = ProximoId();
            movimentacao.CodigoProduto = codigoProduto;
            movimentacao.Tipo = tipo;
            movimentacao.Quantidade = quantidade;
            movimentacao.Descricao = descricao.Trim();
            movimentacao.DataHora = DateTime.Now;
            movimentacao.SaldoFinal = produto.Estoque;

            _dados.Movimentacoes.Add(movimentacao);
            Salvar();

            return movimentacao;
        }

        private long ProximoId()
        {
            if (_dados.Movimentacoes.Count == 0) return 1;
            return _dados.Movimentacoes.Max(m => m.Id) + 1;
        }

        private void Salvar()
        {
            File.WriteAllText(_caminhoJson, JsonSerializer.Serialize(_dados, OpcoesJson));
        }

        private class ArquivoEstoque
        {
            public List<ProdutoEstoqueModel> Estoque { get; set; } = new List<ProdutoEstoqueModel>();
            public List<MovimentacaoModel> Movimentacoes { get; set; } = new List<MovimentacaoModel>();
        }
    }
}
