using System;
using System.Linq;
using System.Threading.Tasks;

using Xamarin.Forms;
using AppNoticias.Clases;
using AppNoticias.Servicios;
using System.Net.Http;
using Newtonsoft.Json;
using System.Collections.ObjectModel;

using Xamarin.Forms.Xaml;
using System.Collections.Generic;

namespace AppNoticias.Paginas
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PaginaNoticias : ContentPage
    {
        public ObservableCollection<ChatMessage> Chat { get; set; }

        public PaginaNoticias()
        {
            InitializeComponent();

            Chat = new ObservableCollection<ChatMessage>();
            lsvChat.ItemsSource = Chat;
        }

        async void btnSend_Clicked(object sender, EventArgs e)
        {
            try
            {
                string message = txtMessage.Text;
                Chat.Add(new ChatMessage() { Message = message, IsIncoming = true });

                List<string> response = await EnviaMensaje(message);

                foreach (var item in response)
                {
                    Chat.Add(new ChatMessage() { Message = item, IsIncoming = false });
                }
            }
            catch (Exception ex)
            {
                Chat.Add(new ChatMessage() { Message = "Sorry, I did not understand you. Try again, please :-)" });
            }
            finally
            {
                txtMessage.Text = "";
            }
        }

        async Task<List<string>> EnviaMensaje(string query)
        {
            LuisObject luis = await LuisService.QueryAsync(query);
            List<string> mensajes = new List<string>();

            if (luis == null)
            {
                mensajes.Add("Error: LUIS SERVICE NOT FOUND");
                return mensajes;
            }

            if (luis.intents.Count() == 0)
            {
                mensajes.Add("Error: LUIS SERVICE INTENTS NOT FOUND");
                return mensajes;
            }

            switch (luis.intents[0]?.intent)
            {
                case "BusquedaNoticias":
                    if (luis.entities.Count() == 0)
                    {
                        mensajes.Add("Error: LUIS SERVICE ENTITIES NOT FOUND");
                        return mensajes;
                    }

                    var entityType = luis.entities[0].type;
                    var searchQueryString = "";

                    switch (entityType)
                    {
                        case "TemaNoticias":
                            searchQueryString = $"searchQuery={luis.entities[0].entity}";
                            break;
                        case "CategoriaNoticias":
                            searchQueryString = $"searchQuery={luis.entities[0].entity}";
                            break;
                        default:
                            searchQueryString = $"searchQuery={luis.entities[0].entity}";
                            break;
                    }

                    var lista = await Servicios.BingService.QueryAsync(searchQueryString);

                    foreach (var item in lista)
                    {
                        mensajes.Add(item.Title);
                    }

                    return mensajes;
                    break;
                default:
                    mensajes.Add("Sorry, I did not understand you. Try again, please :-)");
                    return mensajes;
            }
        }

    }
}