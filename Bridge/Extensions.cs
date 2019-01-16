using System;
using System.Diagnostics;
using System.Linq;
using System.Management;
using System.Threading;
using System.Threading.Tasks;

namespace Bridge
{
  public static class Extensions
  {
    public static string GetCommandLine(this Process process)
    {
      using (ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT CommandLine FROM Win32_Process WHERE ProcessId = " + process.Id))
      using (ManagementObjectCollection objects = searcher.Get())
      {
        return objects.Cast<ManagementBaseObject>().SingleOrDefault()?["CommandLine"]?.ToString();
      }

    }
  }
  public class PeriodicTask
  {
    public static async Task Run(Action action, TimeSpan period, CancellationToken cancellationToken)
    {
      while (!cancellationToken.IsCancellationRequested)
      {
        await Task.Delay(period, cancellationToken);

        if (!cancellationToken.IsCancellationRequested)
          action();
      }
    }

    public static Task Run(Action action, TimeSpan period)
    {
      return Run(action, period, CancellationToken.None);
    }
  }
}
