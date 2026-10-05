# Desafio Tecnico - Target Sistemas

Aplicacao de console em .NET 9 / C#.

## Como executar

```
dotnet run --project DesafioTarget.App
```

## Estrutura

- `DesafioTarget.App/Models` - modelos (VendaModel, ProdutoEstoqueModel, etc.)
- `DesafioTarget.App/Enums` - enumeracoes
- `DesafioTarget.App/Repositorio` - acesso aos dados e regras (comissao, estoque, juros)
- `DesafioTarget.App/Data` - arquivos JSON usados como base

## Logica

O app roda um menu simples. Cada opcao le os JSONs de `Data/`, aplica a regra e imprime o resultado. Estoque persiste o JSON a cada movimentacao.

### 1. Comissao dos vendedores

Le `vendas.json` (vendedor + valor). A comissao e calculada **por venda**, nao por total do vendedor:

| Valor da venda | Comissao |
|----------------|----------|
| Abaixo de R$ 100,00 | 0% |
| De R$ 100,00 ate R$ 499,99 | 1% |
| A partir de R$ 500,00 | 5% |

Depois agrupa por vendedor, soma o total vendido e soma as comissoes **sem arredondar**. O arredondamento (2 casas) e feito **so no total final** de cada vendedor — se arredondasse em cada venda, o resultado do desafio nao bateria.

### 2. Movimentacao de estoque

Le `estoque.json` (produtos + movimentacoes ja registradas).

1. Lista os produtos com o saldo atual
2. Ped codigo, tipo (1 = entrada, 2 = saida), quantidade e descricao
3. Entrada soma no saldo; saida subtrai
4. Regra de aceite:
   - quantidade tem que ser maior que zero
   - descricao obrigatoria
   - produto precisa existir
   - saida nao pode passar do saldo em estoque
5. Cada aceite gera uma `MovimentacaoModel` com ID sequencial (proximo maior ID + 1)
6. Salva de volta no JSON — por isso o arquivo em `bin/.../Data/` muda a cada movimentacao

Para voltar ao estado original, copia o `estoque.json` de `DesafioTarget.App/Data/` de novo.

### 3. Juros simples

Ped valor e data de vencimento. Calculo:

```
juros = valor * 2,5% * dias de atraso
```

- Dias de atraso = data de hoje - vencimento
- Se vencimento for hoje ou futuro: juros = 0
- Valor negativo: rejeitado
- Arredonda o juros em 2 casas

Nao usa juros compostos — o enunciado cita multa/juros de 2,5% ao dia, e o caminho mais simples e o diario sobre o valor original.
