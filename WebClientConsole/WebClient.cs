using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net.Http;

namespace WebClient
{
    public class WebClient : IDisposable
    {
        HttpClient client_;

        public WebClient()
        {
            client_ = new HttpClient();
        }

        public void Dispose()
        {
            ((IDisposable)client_).Dispose();
            client_ = null;
        }

        public async Task<byte[]> Get(string url)
        {
            try
            {
                var response = await client_.GetAsync(url);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadAsByteArrayAsync();
            }
            catch (HttpRequestException e)
            {
                System.Console.Write(e);
                return null;
            }
        }
    }
}
