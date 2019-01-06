using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace Bridge
{
    
    class Program
    {       

        static readonly Regex INSTALL_REGEX =  new Regex(@"--install-directory=([^""]*)", RegexOptions.Compiled);

        static void Main(string[] args)
        {
            var client = new LeagueClient();
            client.Connect(new List<string>() { "OnJsonApiEvent_lol-login_v1_login-platform-credentials" });
            while (client.State != LeagueClientState.Connected)
            {
                Console.WriteLine(client.State);
                Thread.Sleep(500);
            }
            // Console.WriteLine(client.RequestToString(HttpMethod.Post, "help").GetAwaiter().GetResult());
            Console.ReadKey();
        }     

    }
}
