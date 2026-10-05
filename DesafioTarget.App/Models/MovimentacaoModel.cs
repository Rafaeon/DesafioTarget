using DesafioTarget.App.Enums;

namespace DesafioTarget.App.Models
{
    public class MovimentacaoModel
    {
        public long Id { get; set; }
        public int CodigoProduto { get; set; }
        public TipoMovimentacao Tipo { get; set; }
        public int Quantidade { get; set; }
        public string Descricao { get; set; }
        public DateTime DataHora { get; set; }
        public int SaldoFinal { get; set; }
    }
}
