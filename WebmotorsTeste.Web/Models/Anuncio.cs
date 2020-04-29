using System.ComponentModel;

namespace WebmotorsTeste.Web.Models
{
    public class Anuncio
    {
        public long Id { get; set; }
        public string Marca { get; set; }
        public string Modelo { get; set; }
        public int Ano { get; set; }

        [DisplayName("Versão")]
        public string Versao { get; set; }

        [DisplayName("Km")]
        public int Quilometragem { get; set; }

        [DisplayName("Observação")]
        public string Observacao { get; set; }
    }
}