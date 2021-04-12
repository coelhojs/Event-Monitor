using System;
using System.Net.Http;

namespace Event_Simulator
{
    public class Simulator
    {
        public string AppUrl { get; } = "http://localhost:5000";
        public HttpClient Client { get; }

        public Simulator()
        {
            Client = new HttpClient();
            Client.BaseAddress = new Uri(AppUrl);
        }
    }
}
