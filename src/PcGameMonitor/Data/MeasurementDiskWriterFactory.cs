namespace PcGameMonitor.Data;

public class MeasurementDiskWriterFactory
{
    private readonly string _location;

    public MeasurementDiskWriterFactory(string location)
    {
        _location = location;

        if (!Directory.Exists(location))
            throw new ArgumentException("Directory does not exist");
    }

    public SessionWriter CreateSessionWriter(string application)
    {
        return new SessionWriter(_location, application);
    }

    public class SessionWriter : IDisposable
    {
        private readonly StreamWriter _writer;

        public SessionWriter(string location, string application)
        {
            var filename = $"{application} - {DateTime.UtcNow}.session".Replace(":", "_").Replace("/", "-");
            var filepath = Path.Combine(location, filename);
            _writer = File.CreateText(filepath);
        }

        public void Dispose()
        {
            _writer.Dispose();
        }

        public void Write(IEnumerable<TemperatureMeasurement> measurements)
        {
            foreach (var measurement in measurements.ToList())
                _writer.WriteLine($"{measurement.Sensor},{measurement.Temperature},{measurement.Timestamp}");

            _writer.Flush();
        }
    }
}
