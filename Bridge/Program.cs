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

    private NotifyIcon trayIcon;
    private ContextMenu contextMenu;
    private LeagueClient client;
    private GUI gui;

    public Program()
    {
      this.client = new LeagueClient();
      this.client.LeagueClientStateChanged += Client_LeagueClientStateChanged;
    }

    private void Client_LeagueClientStateChanged(object sender, ClientStateChangedEventArgs e)
    {
      this.gui.UpdateLeagueClientState(e.State);
    }

    public async Task Run()
    {
      client.Connect();
      await Task.Run(() =>
      {
        // Create the Gui here to cricumvent some threading issues
        // TODO: Find a better solution
        this.gui = new GUI();
        this.gui.SetTrayIconVisibility(true);
        Application.Run(this.gui);
        this.gui.SetTrayIconVisibility(false);
      });
    }

  }
}
