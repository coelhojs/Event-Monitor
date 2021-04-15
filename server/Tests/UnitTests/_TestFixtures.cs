using EventMonitor.ViewObjects;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;

namespace UnitTests
{
    public class _TestFixtures : IDisposable
    {
        private readonly TestServer _server;

        public string AppUrl { get; }
        public HttpClient Client { get; }

        public _TestFixtures()
        {
            var builder = new WebHostBuilder()
                .UseStartup<EventMonitor.Startup>()
                .ConfigureAppConfiguration((context, config) =>
                {
                    config.SetBasePath(Path.Combine(
                        Directory.GetCurrentDirectory(),
                        "..\\..\\..\\..\\..\\..\\EventMonitor"));

                    config.AddJsonFile("appsettings.json");
                });

            using (var file = File.OpenText("..\\..\\..\\..\\..\\..\\EventMonitor\\Properties\\launchSettings.json"))
            {
                var reader = new JsonTextReader(file);
                var jObject = JObject.Load(reader);

                var variables = jObject
                    .GetValue("profiles")
                    //select a proper profile here
                    .SelectMany(profiles => profiles.Children())
                    .SelectMany(profile => profile.Children<JProperty>())
                    .Where(prop => prop.Name == "environmentVariables")
                    .SelectMany(prop => prop.Value.Children<JProperty>())
                    .ToList();

                foreach (var variable in variables)
                {
                    Environment.SetEnvironmentVariable(variable.Name, variable.Value.ToString());
                }
            }

            _server = new TestServer(builder);

            AppUrl = Environment.GetEnvironmentVariable("API_URL");
            Client = _server.CreateClient();
            Client.BaseAddress = new Uri(AppUrl);
        }

        public StringContent SerializeObject(RawEventVO mockEvent)
        {
            var serializedObject = JsonConvert.SerializeObject(mockEvent);

            return new StringContent(serializedObject, Encoding.UTF8, "application/json");
        }

        public void Dispose()
        {
            Client.Dispose();
            _server.Dispose();
        }
    }
}
