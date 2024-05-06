using PcGameMonitor.Data;
using PcGameMonitor.LibreHardwareMonitor;
using System.Diagnostics;
using static PcGameMonitor.Data.MeasurementDiskWriterFactory;

namespace PcGameMonitor;

public static class SessionMonitor
{
    private static readonly TimeSpan SessionPollingInterval = TimeSpan.FromSeconds(10);
    private static readonly TimeSpan PassivePollingInterval = TimeSpan.FromMinutes(1);
    private static readonly string[] _applications = ["helldivers2", "notepad"];
    private static string? _runningApplication;
    private static SessionWriter? _sessionWriter;

    public static bool IsSessionRunning => _runningApplication != null;

    public static void StartMonitoring(CancellationToken ct)
    {
        var writerFactory = new MeasurementDiskWriterFactory(@"C:\Users\morten\Desktop\Monitoring Logs");
        var hardwareMonitor = new HardwareMonitor();

        Task.Factory.StartNew(() =>
        {
            while (!ct.IsCancellationRequested)
            {
                var updatedRunningApplication = _applications.FirstOrDefault(app => Process.GetProcessesByName(app).Length > 0);

                if (_runningApplication == null && updatedRunningApplication != null)
                {
                    StartSession(writerFactory, updatedRunningApplication, ct);
                }
                else if (_runningApplication != null && updatedRunningApplication == null)
                {
                    StopSession();
                }
                else
                {
                    _sessionWriter?.Write(hardwareMonitor.CollectMeasurements());
                }

                Thread.Sleep(IsSessionRunning ? SessionPollingInterval : PassivePollingInterval);
            }
        });
    }

    private static void StartSession(MeasurementDiskWriterFactory writerFactory, string updatedRunningApplication, CancellationToken ct)
    {
        _runningApplication = updatedRunningApplication;
        _sessionWriter = writerFactory.CreateSessionWriter(_runningApplication);

        Console.WriteLine($"[{DateTime.Now.ToShortTimeString()}]: Session started for application '{_runningApplication}'");
    }

    private static void StopSession()
    {
        _sessionWriter?.Dispose();
        _sessionWriter = null;
        _runningApplication = null;

        Console.WriteLine($"[{DateTime.Now.ToShortTimeString()}]:Session stopped for application '{_runningApplication}'");
    }
}
