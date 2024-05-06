using LibreHardwareMonitor.Hardware;
using PcGameMonitor.Data;

namespace PcGameMonitor.LibreHardwareMonitor
{
    public class HardwareMonitor
    {
        private Computer _computer;
        private UpdateVisitor _visitor;

        public HardwareMonitor()
        {
            _visitor = new UpdateVisitor();
            _computer = new Computer();
            _computer.Open();
            _computer.IsGpuEnabled = true;
        }

        public IEnumerable<TemperatureMeasurement> CollectMeasurements()
        {
            _computer.Accept(_visitor);

            for (int i = 0; i < _computer.Hardware.Count; i++)
            {
                if (_computer.Hardware[i].HardwareType == HardwareType.GpuNvidia)
                {
                    for (int j = 0; j < _computer.Hardware[i].Sensors.Length; j++)
                    {
                        if (_computer.Hardware[i].Sensors[j].SensorType == SensorType.Temperature)
                        {

                            if (_computer.Hardware[i].Sensors[j].Value.HasValue)
                            {
                                yield return new(_computer.Hardware[i].Sensors[j].Value!.Value, DateTime.Now, _computer.Hardware[i].Sensors[j].Name);
                            }
                        }
                    }
                }
            }
        }
    }
}
