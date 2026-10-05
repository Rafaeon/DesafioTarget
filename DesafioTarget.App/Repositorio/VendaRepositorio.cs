using System.Text.Json;
using DesafioTarget.App.Models;

namespace DesafioTarget.App.Repositorio
{
    public class VendaRepositorio : IVendaRepositorio
    {
        private readonly string _caminhoJson;

        public VendaRepositorio()
        {
        }

        public VendaRepositorio(string caminhoJson)
        {
            _caminhoJson = caminhoJson;
        }

        public List<VendaModel> BuscarTodos()
        {
            var json = File.ReadAllText(_caminhoJson);
            var opcoes = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var arquivo = JsonSerializer.Deserialize<ArquivoVendas>(json, opcoes);

            if (arquivo == null)
                throw new Exception("Houve um erro ao carregar as vendas!");

            return arquivo.Vendas;
        }

        public decimal CalcularComissaoDaVenda(decimal valor)
        {
            // abaixo de 100 = 0%, de 100 a 499,99 = 1%, a partir de 500 = 5%
            if (valor < 100m) return 0m;
            if (valor < 500m) return valor * 0.01m;
            return valor * 0.05m;
        }

        public List<ResultadoComissaoModel> CalcularPorVendedor(List<VendaModel> vendas)
        {
            var resultado = new List<ResultadoComissaoModel>();

            foreach (var grupo in vendas.GroupBy(v => v.Vendedor))
            {
                var item = new ResultadoComissaoModel();
                item.Vendedor = grupo.Key;
                item.TotalVendido = grupo.Sum(v => v.Valor);
                item.Comissao = Math.Round(grupo.Sum(v => CalcularComissaoDaVenda(v.Valor)), 2);
                resultado.Add(item);
            }

            return resultado;
        }

        private class ArquivoVendas
        {
            public List<VendaModel> Vendas { get; set; }
        }
    }
}
