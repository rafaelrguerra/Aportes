using FileHelpers;

namespace Aportes
{
    [IgnoreFirst(1)]
    [DelimitedRecord(";")]
    public class Custodia
    {
        public string C1 { get; set; }
        public string Papel { get; set; }
        public int Quantidade { get; set; }
        public string C4 { get; set; }
        public string C5 { get; set; }
        public string C6 { get; set; }
        public string C7 { get; set; }
        public string C8 { get; set; }
        public string C9 { get; set; }
        public string C10 { get; set; }
        public string C11 { get; set; }
        public string Total { get; set; }
        public double TotalNumero { get => Convert.ToDouble(Total); }
        public string C13 { get; set; }
    }
}