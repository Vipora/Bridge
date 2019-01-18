using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using WebSocketSharp;

namespace Bridge
{


  class ServerConnection
  {
    private const string debugUrl = "ws://localhost:3333/websocket?authorization=";
    private const string liveUrl = "ws://live-url/websocket?authorization=";

    private string url;
    private WebSocket webSocket;

    public ServerConnection(string token)
    {
      this.url = (Debugger.IsAttached ? debugUrl : liveUrl) + token;
      Console.WriteLine(this.url);
    }

    public bool Connect()
    {
      this.webSocket = new WebSocket(this.url);
      this.webSocket.OnMessage += WebSocket_OnMessage;
      this.webSocket.OnError += WebSocket_OnError;
      this.webSocket.OnClose += WebSocket_OnClose;

      this.webSocket.Connect();
      this.webSocket.Send(Encoding.ASCII.GetBytes("{\"t\":1, \"d\":{\"topic\":\"bridge:Test\"}}"));
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
      Console.WriteLine(e);
    }

    private void WebSocket_OnError(object sender, ErrorEventArgs e)
    {
      Console.WriteLine(e);
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
}
