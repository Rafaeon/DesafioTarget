namespace DesafioTarget.App.Repositorio
{
    public class CalculoJuros
    {
        public decimal CalcularJuros(decimal valor, DateOnly vencimento, DateOnly hoje)
        {
            if (valor < 0)
                throw new Exception("O valor não pode ser negativo!");

            var diasAtraso = hoje.DayNumber - vencimento.DayNumber;

            if (diasAtraso <= 0) return 0m;

            return Math.Round(valor * 0.025m * diasAtraso, 2);
        }
    }
}
