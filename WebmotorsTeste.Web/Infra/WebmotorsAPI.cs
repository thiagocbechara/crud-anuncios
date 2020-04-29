using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using WebmotorsTeste.Web.Infra.Models;
using Version = WebmotorsTeste.Web.Infra.Models.Version;

namespace WebmotorsTeste.Web.Infra
{
    public class WebmotorsAPI : IWebmotorsAPI
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly string _baseApiUrl;

        public WebmotorsAPI(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
            _baseApiUrl = "http://desafioonline.webmotors.com.br/api/OnlineChallenge/";
        }

        public async Task<IEnumerable<Make>> GetMakes() => await HttpGet<Make>("Make");
        

        public async Task<IEnumerable<Model>> GetModels(long makeId) => await HttpGet<Model>($"Model?MakeID={makeId}");

        public async Task<IEnumerable<Version>> GetVersions(long modelId) => await HttpGet<Version>($"Version?ModelID={modelId}");

        private async Task<IEnumerable<T>> HttpGet<T>(string action)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"{_baseApiUrl}{action}");
            request.Headers.Add("Accept", "application/json");

            var client = _clientFactory.CreateClient();
            var response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsAsync<IEnumerable<T>>();
            }
            else
            {
                return Enumerable.Empty<T>();
            }
        }
    }
}