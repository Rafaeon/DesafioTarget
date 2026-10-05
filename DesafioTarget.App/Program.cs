using System.Globalization;
using DesafioTarget.App.Enums;
using DesafioTarget.App.Models;
using DesafioTarget.App.Repositorio;

var cultura = new CultureInfo("pt-BR");
CultureInfo.DefaultThreadCurrentCulture = cultura;
CultureInfo.CurrentCulture = cultura;
Console.OutputEncoding = System.Text.Encoding.UTF8;

var pastaDados = Path.Combine(AppContext.BaseDirectory, "Data");
var caminhoVendas = Path.Combine(pastaDados, "vendas.json");
var caminhoEstoque = Path.Combine(pastaDados, "estoque.json");

while (true)
{
    Console.WriteLine();
    Console.WriteLine("=== Desafio Tecnico ===");
    Console.WriteLine("1 - Comissao dos vendedores");
    Console.WriteLine("2 - Movimentacao de estoque");
    Console.WriteLine("3 - Calculo de juros");
    Console.WriteLine("0 - Sair");
    Console.Write("Opcao: ");

    var opcao = Console.ReadLine();

    if (opcao == "1")
    {
        ExecutarComissao();
    }
    else if (opcao == "2")
    {
        ExecutarEstoque();
    }
    else if (opcao == "3")
    {
        ExecutarJuros();
    }
    else if (opcao == "0")
    {
        break;
    }
    else
    {
        Console.WriteLine("Opcao invalida.");
    }
}

void ExecutarComissao()
{
    try
    {
        var repositorio = new VendaRepositorio(caminhoVendas);
        var vendas = repositorio.BuscarTodos();
        var resultados = repositorio.CalcularPorVendedor(vendas);

        Console.WriteLine();
        Console.WriteLine("Vendedor              Total vendido         Comissao");

        foreach (var r in resultados)
        {
            Console.WriteLine($"{r.Vendedor,-20}{r.TotalVendido,18:C2}{r.Comissao,14:C2}");
        }
    }
    catch (Exception erro)
    {
        Console.WriteLine($"Erro, nao conseguimos calcular a comissao, tente novamente, detalhe do erro:{erro.Message}");
    }
}

void ExecutarEstoque()
{
    try
    {
        var repositorio = new EstoqueRepositorio(caminhoEstoque);

        Console.WriteLine();
        Console.WriteLine("Produtos:");

        foreach (var p in repositorio.Produtos)
        {
            Console.WriteLine($"  {p.CodigoProduto} - {p.DescricaoProduto} (saldo: {p.Estoque})");
        }

        Console.Write("Codigo do produto: ");
        var codigo = LerInteiro(Console.ReadLine());

        Console.Write("Tipo (1 = Entrada, 2 = Saida): ");
        var tipoValor = LerInteiro(Console.ReadLine());

        if (tipoValor != 1 && tipoValor != 2)
        {
            Console.WriteLine("Tipo de movimentacao invalido.");
            return;
        }

        var tipo = (TipoMovimentacao)tipoValor;

        Console.Write("Quantidade: ");
        var quantidade = LerInteiro(Console.ReadLine());

        Console.Write("Descricao da movimentacao: ");
        var descricao = Console.ReadLine() ?? string.Empty;

        var mov = repositorio.Registrar(codigo, tipo, quantidade, descricao);

        Console.WriteLine();
        Console.WriteLine($"Movimentacao #{mov.Id} registrada ({mov.Tipo} - {mov.Descricao}).");
        Console.WriteLine($"Estoque final do produto {mov.CodigoProduto}: {mov.SaldoFinal}");
    }
    catch (Exception erro)
    {
        Console.WriteLine($"Erro, nao conseguimos registrar a movimentacao, tente novamente, detalhe do erro:{erro.Message}");
    }
}

void ExecutarJuros()
{
    try
    {
        Console.Write("Valor: ");
        if (!decimal.TryParse(Console.ReadLine(), NumberStyles.Number, cultura, out var valor))
        {
            Console.WriteLine("Valor invalido.");
            return;
        }

        Console.Write("Data de vencimento (dd/MM/aaaa): ");
        if (!DateOnly.TryParseExact(Console.ReadLine(), "dd/MM/yyyy", cultura, DateTimeStyles.None, out var vencimento))
        {
            Console.WriteLine("Data invalida.");
            return;
        }

        var hoje = DateOnly.FromDateTime(DateTime.Today);
        var calculo = new CalculoJuros();
        var juros = calculo.CalcularJuros(valor, vencimento, hoje);

        Console.WriteLine($"Juros em {hoje:dd/MM/yyyy}: {juros:C2}");
        Console.WriteLine($"Valor atualizado: {valor + juros:C2}");
    }
    catch (Exception erro)
    {
        Console.WriteLine($"Erro, nao conseguimos calcular os juros, tente novamente, detalhe do erro:{erro.Message}");
    }
}

int LerInteiro(string texto)
{
    if (!int.TryParse(texto, out var numero))
        throw new Exception("Informe um numero inteiro valido!");

    return numero;
}
