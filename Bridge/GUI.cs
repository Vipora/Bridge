using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Bridge
{
  public partial class GUI : Form
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

    private NotifyIcon trayIcon;
    private ContextMenu contextMenu;
    public GUI()
    {
      InitializeComponent();
      this.trayIcon = new NotifyIcon();
      this.trayIcon.Icon = GetIcon(LeagueClientState.NotRunning);
      this.trayIcon.Text = "Vipora";
      this.trayIcon.DoubleClick += TrayIcon_DoubleClick;
      this.contextMenu = new System.Windows.Forms.ContextMenu();
      var exitMenuItem = new System.Windows.Forms.MenuItem();

      // Initialize contextMenu
      this.contextMenu.MenuItems.AddRange(
                  new System.Windows.Forms.MenuItem[] { exitMenuItem });

      // Initialize Exit menu item
      exitMenuItem.Index = 0;
      exitMenuItem.Text = "E&xit";
      exitMenuItem.Click += ExitButton_Click;

      this.trayIcon.ContextMenu = this.contextMenu;

      this.ShowInTaskbar = false;
      this.WindowState = FormWindowState.Minimized;

      this.FormClosing += GUI_FormClosing;
    }

    public void UpdateLeagueClientState(LeagueClientState state)
    {
      this.trayIcon.Icon = GetIcon(state);
    }

    public void SetTrayIconVisibility(bool visible)
    {
      this.trayIcon.Visible = visible;
    }

    private void ExitButton_Click(object sender, EventArgs e)
    {
      Application.Exit();
    }

    private void TrayIcon_DoubleClick(object sender, EventArgs e)
    {
      // Set the WindowState to normal if the form is minimized.
      if (this.WindowState == FormWindowState.Minimized)
        this.WindowState = FormWindowState.Normal;

      // Activate the form.
      this.Activate();
    }

    private void GUI_FormClosing(object sender, FormClosingEventArgs e)
    {
      if (e.CloseReason == CloseReason.UserClosing)
      {
        e.Cancel = true;
        Hide();
      }
    }
  }
}
