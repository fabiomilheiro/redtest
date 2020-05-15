using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Fabio.Web.ServiceTests.TestHelpers
{
    public static class HttpClientExtensions
    {
        public static async Task<HttpResponseMessage> PostJsonAsync(this HttpClient client, string requestUri, object body)
        {
            return await client
                .PostAsync(
                    requestUri,
                    new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, "application/json"));
        }
    }
}