using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using AppNoticias.Clases;

namespace AppNoticias.Servicios
{
    public static class LuisService
    {
        private static string LuisAppID = "Coloca tu AppID desde Settings";
        private static string LuisSubscriptionKey = "Coloca tu Subscription Key desde Publish";

        public static async Task<LuisObject> QueryAsync(string query)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    string response = await client.GetStringAsync(new Uri($"https://westus.api.cognitive.microsoft.com/luis/v2.0/apps/{LuisAppID}?subscription-key={LuisSubscriptionKey}&verbose=true&timezoneOffset=0&q={query}"));

                    LuisObject luis = JsonConvert.DeserializeObject<LuisObject>(response);
                    if (luis != null) return luis;
                }

                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
