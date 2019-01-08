using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Bridge
{

  class Program
  {

    public static readonly Dictionary<LeagueClientState, Icon> icons = new Dictionary<LeagueClientState, Icon>()
    {
      { LeagueClientState.NotRunning, Bridge.Properties.Resources.NotRunningStateIcon },
      { LeagueClientState.ProcessFound, Bridge.Properties.Resources.ReadyToConnectStateIcon },
      { LeagueClientState.ReadyToConnect, Bridge.Properties.Resources.ReadyToConnectStateIcon },
      { LeagueClientState.Connected, Bridge.Properties.Resources.RunningStateIcon }
    };

    public static Icon GetIcon(LeagueClientState state = LeagueClientState.Unkown)
    {
      return icons.ContainsKey(state) ? icons[state] : Bridge.Properties.Resources.ErrorStateIcon;
    }

    static void Main(string[] args)
    {
      var program = new Program();
      program.Run().GetAwaiter().GetResult();
    }

    private NotifyIcon trayIcon;
    private LeagueClient client;
    private ContextMenu contextMenu;

    public Program()
    {
      this.trayIcon = new NotifyIcon();
      this.trayIcon.Icon = GetIcon(LeagueClientState.NotRunning);
      this.trayIcon.Text = "Vipora";
      this.contextMenu = new System.Windows.Forms.ContextMenu();
      var exitMenuItem = new System.Windows.Forms.MenuItem();

      // Initialize contextMenu1
      this.contextMenu.MenuItems.AddRange(
                  new System.Windows.Forms.MenuItem[] { exitMenuItem });

      // Initialize menuItem1
      exitMenuItem.Index = 0;
      exitMenuItem.Text = "E&xit";
      exitMenuItem.Click += ExitButton_Click;

      this.trayIcon.ContextMenu = this.contextMenu;

      this.client = new LeagueClient();
      this.client.LeagueClientStateChanged += Client_LeagueClientStateChanged;
    }

    private void ExitButton_Click(object sender, EventArgs e)
    {
      Application.Exit();
    }

    private void Client_LeagueClientStateChanged(object sender, ClientStateChangedEventArgs e)
    {
      this.trayIcon.Icon = GetIcon(e.State);
    }

    public async Task Run()
    {
      client.Connect();
      await Task.Run(() =>
      {
        this.trayIcon.Visible = true;
        Application.Run();
        this.trayIcon.Visible = false;
      });
    }

  }
}
