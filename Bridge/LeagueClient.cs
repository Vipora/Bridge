using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Authentication;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WebSocketSharp;

namespace Bridge
{
    enum LeagueClientState
    {
        NotRunning = 0,
        ProcessFound = 1,
        ReadyToConnect = 2,
        Connected = 3        
    }

    class LeagueClient
    {
        private static readonly Regex INSTALL_REGEX = new Regex(@"--install-directory=([^""]*)", RegexOptions.Compiled);

        private HttpClient httpClient;
        private WebSocket webSocket;

        private Process process;
        private string installDirectory;
        private string token;
        private Uri baseUri;

        public LeagueClientState State { get; private set; }

        public LeagueClient()
        {
            this.State = LeagueClientState.NotRunning;
            var handler = new HttpClientHandler();
            handler.ClientCertificateOptions = ClientCertificateOption.Manual;
            handler.ServerCertificateCustomValidationCallback =
                (httpRequestMessage, cert, cetChain, policyErrors) =>
                {
                    return true;
                };

            httpClient = new HttpClient(handler);
        }

        public async Task<bool> Connect(List<string> events = null)
        {
            this.process = await this.FindLeagueProcess();
            this.State = LeagueClientState.ProcessFound;
            this.installDirectory = INSTALL_REGEX.Match(process.GetCommandLine()).Groups[1].Value;
            var lockfile = await this.GetLockfileContents();
            var items = lockfile.Split(':');
            this.token = items[3];
            this.baseUri = new Uri("https://127.0.0.1:" + items[2] + "/");
            this.State = LeagueClientState.ReadyToConnect;
            // Setup http client
            this.httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            this.httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.ASCII.GetBytes("riot:" + this.token)));
            this.httpClient.BaseAddress = this.baseUri;

            var socketAddress = $"wss://127.0.0.1:{items[2]}";
            this.webSocket = new WebSocket(socketAddress, "wamp");
            this.webSocket.SetCredentials("riot", this.token, true);
            this.webSocket.SslConfiguration.EnabledSslProtocols = SslProtocols.Tls12;
            this.webSocket.SslConfiguration.ServerCertificateValidationCallback = (response, cert, chain, errors) => true;

            this.webSocket.OnError += WebSocket_OnError;
            this.webSocket.OnMessage += WebSocket_OnMessage;

            await Task.Run(() => this.webSocket.Connect());
            if(events != null)
            {
                foreach(var e in events)
                {
                    this.StartListening(e);
                }
            }

            this.State = LeagueClientState.Connected;
            return true;
        }

        private void StartListening(string lcuEvent)
        {
            this.webSocket.Send("[5, \"" + lcuEvent + "\"]");
        }

        private void WebSocket_OnMessage(object sender, MessageEventArgs e)
        {
            Console.WriteLine(e.Data);
        }

        private void WebSocket_OnError(object sender, WebSocketSharp.ErrorEventArgs e)
        {
            Console.WriteLine(e);
            Console.WriteLine(e.Exception);
            Console.WriteLine(e.Message);
        }

        public async Task<HttpResponseMessage> Request(HttpMethod method, string endpoint, object data = null)
        {
            var json = data == null ? "" : JsonConvert.SerializeObject(data);
            if(method == HttpMethod.Get)
            {
                return await httpClient.GetAsync(endpoint);
            }
            else if(method == HttpMethod.Post)
            {
                return await httpClient.PostAsync(endpoint, new StringContent(json, Encoding.UTF8, "application/json"));
            }
            else if(method == HttpMethod.Put)
            {
                return await httpClient.PutAsync(endpoint, new StringContent(json, Encoding.UTF8, "application/json"));
            }
            else if(method == HttpMethod.Delete)
            {
                return await httpClient.DeleteAsync(endpoint);
            }
            else
            {
                throw new Exception("Unsupported HTTP method");
            }
        }

        public async Task<string> RequestToString(HttpMethod method, string endpoint, object data = null)
        {
            var result = await Request(method, endpoint, data);
            return await result.Content.ReadAsStringAsync();
        }


        private async Task<Process> FindLeagueProcess()
        {
            Process process = null;
            while (process == null)
            {
                var processes = Process.GetProcessesByName("LeagueClientUX");
                if (processes.Length > 0)
                    process = processes[0];
                else
                    await Task.Delay(1000);
            }
            return process;
        }

        private async Task<string> GetLockfileContents()
        {
            var lockfile = Path.Combine(this.installDirectory, "lockfile");
            using (var fileStream = new FileStream(lockfile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (var reader = new StreamReader(fileStream))
            {
                return await reader.ReadToEndAsync();                
            }
        }
    }
}
