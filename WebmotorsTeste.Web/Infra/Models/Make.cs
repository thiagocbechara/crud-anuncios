using System.Collections.Generic;

namespace WebmotorsTeste.Web.Infra.Models
{
    public class Make
    {
        public long Id { get; set; }
        public string Name { get; set; }

        public ICollection<Model> Models { get; set; }
    }
}
