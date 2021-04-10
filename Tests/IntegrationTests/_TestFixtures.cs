using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Linq;
using System.Net.Http;

namespace IntegrationTests
{
    public class _TestFixtures : IDisposable
    {
        private readonly TestServer _server;

        public string AppUrl { get; } = "http://localhost:5000";
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

            Client = _server.CreateClient();
            Client.BaseAddress = new Uri(AppUrl);
        }

        public void Dispose()
        {
            Client.Dispose();
            _server.Dispose();
        }
    }
}
