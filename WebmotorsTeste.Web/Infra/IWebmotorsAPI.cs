using System.Collections.Generic;
using System.Threading.Tasks;
using WebmotorsTeste.Web.Infra.Models;

namespace WebmotorsTeste.Web.Infra
{
    public interface IWebmotorsAPI
    {
        Task<IEnumerable<Make>> GetMakes();
        Task<IEnumerable<Model>> GetModels(long makeId);
        Task<IEnumerable<Version>> GetVersions(long modelId);
    }
}