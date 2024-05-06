using PcGameMonitor;

var cancellationTokenSource = new CancellationTokenSource();
SessionMonitor.StartMonitoring(cancellationTokenSource.Token);
Console.WriteLine("Monitoring started. Press any key to stop...");
Console.WriteLine();
Console.ReadLine();
cancellationTokenSource.Cancel();
Console.WriteLine("Goodbye...");
Thread.Sleep(3000);

/* TODO
 * - Make sure we handle forced restarts correctly.
 * - Correlate data with event log errors on start
 * - Make installable as a windows service
 * - Pick a better spot for storing logs
 * - Figure out how to deal with app name whitelisting
 * - Automatic cleanup of old data?
 * - Comparison across sessions (fx. look at max temp for each session and highlight sessions with errors)
 * - Create a different console app which can deal with the presentation of the files
 */