namespace HyperativaDesafio.Domain.Entities
{
    public class Cartao
    {
        public int id { get; set; }
        public string numeroHash { get; set; }
        public string numeroMascara { get; set; }
        public string numeracaoNoLote { get; set; }
        public DateTime dataCadastro { get; set; }
        public int lote { get; set; }
    }
}
