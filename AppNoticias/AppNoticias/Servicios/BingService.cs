using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using AppNoticias.Clases;
using System.Collections.Generic;

namespace AppNoticias.Servicios
{
    public static class BingService
    {
        public static async Task<List<NewsInformation>> QueryAsync(string query)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    string response = await client.GetStringAsync(new Uri($"http://traininglabservices.azurewebsites.net/api/news?{query}"));

                    List<NewsInformation> luis = JsonConvert.DeserializeObject<List<NewsInformation>>(response);
                    return luis;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

    }
}
