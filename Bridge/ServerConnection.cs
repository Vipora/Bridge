using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WebSocketSharp;

namespace Bridge
{


  class ServerConnection
  {
    private const string debugUrl = "localhost:3333/";
    private const string liveUrl = "live-url/websocket?authorization=";

    private string baseUrl;
    private string token;
    private HttpClient httpClient;
    private User user;

    private WebSocket webSocket;

    public ServerConnection(string token)
    {
      this.baseUrl = (Debugger.IsAttached ? debugUrl : liveUrl);
      this.httpClient = new HttpClient();
      this.httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
      this.httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("bearer", token);
      this.httpClient.BaseAddress = new Uri("http://" + this.baseUrl + "api/v1/");

      this.token = token;
      Console.WriteLine(this.baseUrl);
    }

    public async Task<bool> Connect()
    {
      this.webSocket = new WebSocket("ws://" + this.baseUrl + "websocket?authorization=" + this.token);
      this.webSocket.OnMessage += WebSocket_OnMessage;
      this.webSocket.OnError += WebSocket_OnError;
      this.webSocket.OnClose += WebSocket_OnClose;
      try
      {
        var result = await httpClient.GetAsync("user");
        if (!result.IsSuccessStatusCode)
        {
          return false;
        }
        this.user = JsonConvert.DeserializeObject<User>(await result.Content.ReadAsStringAsync());
      } catch {
        return false;
      }

      this.webSocket.Connect();
      this.webSocket.Send(Encoding.ASCII.GetBytes("{\"t\":1, \"d\":{\"topic\":\"bridge:" + this.user.Email + "\"}}"));
      return true;
    }

    public void SendEvent(LCUMessage message)
    {
      if (message == null) return;
      var msg = new EventMessage
      {
        Data = new EventMessage.EventMessageData
        {
          Topic = "bridge:Test",
          EventType = "bridge",
          Message = message
        }
      };
      Console.WriteLine(JsonConvert.SerializeObject(msg));
      this.webSocket.Send(Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(msg)));
    }

    private void WebSocket_OnClose(object sender, CloseEventArgs e)
    {
      do
      {
        Console.WriteLine("Server disconnected");
        Console.WriteLine("Trying to reconnect...");
      } while (!Task.Run(() => this.Connect()).GetAwaiter().GetResult());
      Console.WriteLine("Reconnected!");
    }

    private void WebSocket_OnError(object sender, ErrorEventArgs e)
    {
      Console.WriteLine(e.Message);
    }

    private void WebSocket_OnMessage(object sender, MessageEventArgs e)
    {
      var message = JsonConvert.DeserializeObject<ServerMessage>(e.Data);
      // This is a login message
      // TODO: In case of reconnect, this task will run multiple times
      if (message.Type == 0)
      {
        PeriodicTask.Run(() =>
        {
          this.webSocket.Send(Encoding.ASCII.GetBytes("{\"t\":8}"));
        }, TimeSpan.FromMilliseconds(Convert.ToDouble(message.Data["clientInterval"])));
      }
      Console.WriteLine(e.Data);

    }
  }

  public class ServerMessage
  {
    [JsonProperty("t")]
    public int Type { get; set; }

    [JsonProperty("d")]
    public Dictionary<string, string> Data { get; set; }
  }

  public class EventMessage
  {
    [JsonProperty("t")]
    public int Type { get; } = 7;

    [JsonProperty("d")]
    public EventMessageData Data { get; set; }

    public class EventMessageData
    {
      [JsonProperty("topic")]
      public string Topic { get; set; }

      [JsonProperty("event")]
      public string EventType { get; set; }
      [JsonProperty("data")]
      public LCUMessage Message { get; set; }
    }
  }

  public class User
  {
    [JsonProperty("username")]
    public string Name { get; set; }

    [JsonProperty("email")]
    public string Email { get; set; }
  }
}
