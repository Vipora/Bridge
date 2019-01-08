using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Bridge
{
  public partial class GUI : Form
  {
    private static readonly Color c_lightest = Color.FromArgb(0xE6, 0xE8, 0xFF);
    private static readonly Color c_lighter = Color.FromArgb(0xB2, 0xB7, 0xFF);
    private static readonly Color c_light = Color.FromArgb(0x78, 0x86, 0xD7);
    private static readonly Color c_base = Color.FromArgb(0x65, 0x74, 0xCD);
    private static readonly Color c_dark = Color.FromArgb(0x56, 0x61, 0xB3);
    private static readonly Color c_darker = Color.FromArgb(0x2F, 0x36, 0x5F);
    private static readonly Color c_darkest = Color.FromArgb(0x19, 0x1E, 0x38);

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
    public string accessToken;
    public event EventHandler<AccessTokenChangedEventArgs> AccessTokenChanged;
    public GUI(string accessToken = "")
    {
      InitializeComponent();
      this.BackColor = c_darker;
      this.saveBtn.BackColor = c_base;
      this.exitBtn.BackColor = c_base;
      this.accessToken = accessToken;
      this.txtAccessToken.Text = this.accessToken;

      this.trayIcon = new NotifyIcon();
      this.trayIcon.Icon = GetIcon(LeagueClientState.NotRunning);
      this.trayIcon.Text = "Vipora";
      this.trayIcon.DoubleClick += TrayIcon_DoubleClick;
      this.contextMenu = new System.Windows.Forms.ContextMenu();
      var exitMenuItem = new System.Windows.Forms.MenuItem();
      var optionsMenuItem = new System.Windows.Forms.MenuItem();

      // Initialize contextMenu
      this.contextMenu.MenuItems.AddRange(
                  new System.Windows.Forms.MenuItem[] { exitMenuItem, optionsMenuItem });

      // Initialize Exit menu item
      exitMenuItem.Index = 1;
      exitMenuItem.Text = "E&xit";
      exitMenuItem.Click += ExitButton_Click;

      // Initialize Options menu item
      optionsMenuItem.Index = 0;
      optionsMenuItem.Text = "Options";
      optionsMenuItem.Click += TrayIcon_DoubleClick;

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

    private void saveBtn_Click(object sender, EventArgs e)
    {
      if (this.accessToken != this.txtAccessToken.Text)
      {
        this.accessToken = this.txtAccessToken.Text;
        if (this.AccessTokenChanged != null)
        {
          var args = new AccessTokenChangedEventArgs();
          args.AccessToken = this.accessToken;
          this.AccessTokenChanged(this, args);
        }
      }
    }
  }
  public class AccessTokenChangedEventArgs : EventArgs
  {
    public string AccessToken { get; set; }
  }

}
