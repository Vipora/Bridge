using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Bridge
{
  class EventParser
  {

    private LeagueClient client;

    public EventParser(LeagueClient client)
    {
      this.client = client;
    }

    public LCUMessage ParseEvent(LCUMessage message)
    {
      switch (message.Uri)
      {
        case "/lol-login/v1/session":
          if (message.Data.Value<string>("state") != "SUCCEEDED") return null;
          return message;
        default:
          return message;
      }
    }
  }
}
