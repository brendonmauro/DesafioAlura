using Domain.Interfaces;

namespace Domain
{
    public class ItemResult : IItemResult
    {

        public string Titulo { get; set; }
        public string Professor { get; set; }
        public int  CargaHoraria { get; set; }
        public string Descricao { get; set; }
    }
}
