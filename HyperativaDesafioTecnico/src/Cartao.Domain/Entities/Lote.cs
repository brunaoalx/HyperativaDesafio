namespace HyperativaDesafio.Domain.Entities
{
    public class Lote
    {

        public int id { get; set; }
        public string nome { get; set; }
        public string data { get; set; }
        public string lote { get; set; }
        public string qtdeRegistros { get;set; }
        public string header { get; set; }
        public string trailer { get; set; }
        public string dataProcessamento { get; set; }
        public string tipoLote { get; set; }
    }
}