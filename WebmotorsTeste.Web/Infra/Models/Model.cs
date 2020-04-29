using System.Collections.Generic;

namespace WebmotorsTeste.Web.Infra.Models
{
    public class Model
    {
        public long Id { get; set; }
        public long MakeId { get; set; }
        public string Name { get; set; }

        public ICollection<Version> Versions { get; set; }
    }
}