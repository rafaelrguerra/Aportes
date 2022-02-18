using Aportes;
using FileHelpers;

string custodia = System.IO.File.ReadAllText("Custódia.txt");
var engine = new FileHelperEngine<Custodia>();

custodia = "C1;Papel;Quantidade;C4;C5;C6;C7;C8;C9;C10;C11;Total;C13\r\n" + custodia.Replace("	", ";");

File.WriteAllText("custodia.csv", custodia);
var records = engine.ReadFile("custodia.csv").ToList();

Console.Write("Valor do aporte (somente números): ");
string aporte = Console.ReadLine();

var patrimonio = records.Sum(x => x.TotalNumero) + Convert.ToDouble(aporte);

var quantidadeDeFiis = records.Where(x => x.Papel.Contains("11")).Count();
var quantidadeDeAcoes = records.Count() - quantidadeDeFiis;

var valorPorAcao = patrimonio / 2 / quantidadeDeAcoes;
var valorPorFii = patrimonio / 2 / quantidadeDeFiis;

var listaDeCompra = new List<Compra>();

//ações
foreach (var item in records.Where(x => !x.Papel.Contains("11")))
    listaDeCompra.Add(new Compra()
    {
        Papel = item.Papel,
        Valor = valorPorAcao - item.TotalNumero,
        PrecoUnitario = item.TotalNumero / item.Quantidade
    });

//fiis
foreach (var item in records.Where(x => x.Papel.Contains("11")))
    listaDeCompra.Add(new Compra()
    {
        Papel = item.Papel,
        Valor = valorPorFii - item.TotalNumero,
        PrecoUnitario = item.TotalNumero / item.Quantidade
    });

double totalGastoTeorico = 0;
foreach (var item in listaDeCompra.Where(x => x.Valor > 0).OrderByDescending(x => x.Valor))
{
    var compraAproximada = Convert.ToInt32(item.Valor / item.PrecoUnitario);

    var quantoSeraGasto = item.PrecoUnitario * compraAproximada;

    if (Convert.ToDouble(aporte) >= totalGastoTeorico + quantoSeraGasto)
    {
        Console.WriteLine(item.Papel + ": " + quantoSeraGasto + "; Comprar: " + compraAproximada + "; Preço unitário: " + item.PrecoUnitario);

        totalGastoTeorico += quantoSeraGasto;
    }
    else
    {
        var dinheiroSobrando = Convert.ToDouble(aporte) - totalGastoTeorico;
        double x = 0;
        int cont = 0;
        while (dinheiroSobrando > item.PrecoUnitario * (cont + 1))
        {
            cont++;
            x = item.PrecoUnitario * cont;
        }

        if (cont > 0)
            Console.WriteLine(item.Papel + ": " + x + "; Comprar: " + cont + "; Preço unitário: " + item.PrecoUnitario);

        totalGastoTeorico += x;
    }
}

Console.WriteLine("Quanto vai gastar no total: " + totalGastoTeorico);

File.Delete("custodia.csv");
Console.Read();