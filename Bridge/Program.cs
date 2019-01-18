using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Bridge
{

  class Program
  {
    static void Main(string[] args)
    {
      var program = new Program();
      program.Run().GetAwaiter().GetResult();
    }

    private LeagueClient client;
    private GUI gui;

    public Program()
    {
      this.client = new LeagueClient();
      this.client.LeagueClientStateChanged += Client_LeagueClientStateChanged;
    }

    private void Client_LeagueClientStateChanged(object sender, ClientStateChangedEventArgs e)
    {
      if (this.gui != null)
        this.gui.UpdateLeagueClientState(e.State);
    }

    public async Task Run()
    {
      //TODO: Change the storage of tokens later on
      string token = await ReadAccessTokenFromFile();

      var server = new ServerConnection(token);
      Console.WriteLine("Connecting to server");
      Console.WriteLine(server.Connect());
      client.Server = server;
      var clientTask = client.Connect(new List<string>{
        "OnJsonApiEvent_lol-login_v1_session",
        "OnJsonApiEvent_lol-champ-select_v1_current-champion",
        "OnJsonApiEvent_lol-champ-select_v1_session",
        "OnJsonApiEvent_lol-champ-select_v1_team-boost" 
       });
      var appTask = Task.Run(() =>
      {
        // Create the Gui here to cricumvent some threading issues
        // TODO: Find a better solution
        this.gui = new GUI(token);
        this.gui.AccessTokenChanged += Gui_AccessTokenChanged;
        this.gui.SetTrayIconVisibility(true);
        this.gui.UpdateLeagueClientState(client.State);
        Application.Run(this.gui);
        this.gui.SetTrayIconVisibility(false);
      });
      await Task.WhenAll(clientTask, appTask);
    }

    private void Gui_AccessTokenChanged(object sender, AccessTokenChangedEventArgs e)
    {
      //Store new access token
      var filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Vipora", "token.txt");
      System.IO.FileInfo file = new System.IO.FileInfo(filePath);
      file.Directory.Create(); // If the directory already exists, this method does nothing.
      System.IO.File.WriteAllText(file.FullName, e.AccessToken);
    }

    public async Task<string> ReadAccessTokenFromFile()
    {
      var filepath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Vipora", "token.txt");
      if (File.Exists(filepath))
      {
        using (var fileStream = new FileStream(filepath, FileMode.Open, FileAccess.Read))
        using (var reader = new StreamReader(fileStream))
        {
          return await reader.ReadToEndAsync();
        }
      }
      return string.Empty;
    }

  }
}
