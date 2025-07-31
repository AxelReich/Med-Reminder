using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MedReminder.API.Util
{
    public class WebRequestHandler
    {
        private readonly string host;
        private readonly string port;
        private readonly HttpClient client;

        public WebRequestHandler(string host = "localhost", string port = "1234")
        {
            this.host = host;
            this.port = port;
            client = new HttpClient();
        }

        private string BuildUrl(string url) => $"https://{host}:{port}{url}";

        private async Task<string?> SendRequestAsync(HttpMethod method, string url, object? data = null, CancellationToken cancellationToken = default)
        {
            try
            {
                var request = new HttpRequestMessage(method, BuildUrl(url));

                // Attach JSON content for POST/PUT/PATCH
                if (data != null)
                {
                    var json = JsonConvert.SerializeObject(data);
                    request.Content = new StringContent(json, Encoding.UTF8, "application/json");
                }

                var response = await client.SendAsync(request, cancellationToken);
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsStringAsync(cancellationToken);
                }

                return $"ERROR: {response.StatusCode}";
            }
            catch (Exception e)
            {
                Console.WriteLine($"[{method}] {url} failed: {e.Message}");
                return null;
            }
        }

        public Task<string?> GetAsync(string url, CancellationToken cancellationToken = default)
            => SendRequestAsync(HttpMethod.Get, url, null, cancellationToken);

        public Task<string?> DeleteAsync(string url, CancellationToken cancellationToken = default)
            => SendRequestAsync(HttpMethod.Delete, url, null, cancellationToken);

        public Task<string?> PostAsync(string url, object data, CancellationToken cancellationToken = default)
            => SendRequestAsync(HttpMethod.Post, url, data, cancellationToken);

        public Task<string?> PutAsync(string url, object data, CancellationToken cancellationToken = default)
            => SendRequestAsync(HttpMethod.Put, url, data, cancellationToken);
    }
}
