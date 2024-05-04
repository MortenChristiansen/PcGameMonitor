using LibreHardwareMonitor.Hardware;
using System;
namespace PcGameMonitor.LibreHardwareMonitor
{
    public static class HardwareInfo
    {
        public static void PrintGpuTemperatures()
        {
            var updateVisitor = new UpdateVisitor();
            var computer = new Computer();
            computer.Open();
            computer.IsGpuEnabled = true;
            computer.Accept(updateVisitor);

            Console.WriteLine("GPU Temps:");
            for (int i = 0; i < computer.Hardware.Count; i++)
            {
                if (computer.Hardware[i].HardwareType == HardwareType.GpuNvidia)
                {
                    for (int j = 0; j < computer.Hardware[i].Sensors.Length; j++)
                    {
                        if (computer.Hardware[i].Sensors[j].SensorType == SensorType.Temperature)
                            Console.WriteLine(computer.Hardware[i].Sensors[j].Name + ":" + computer.Hardware[i].Sensors[j].Value.ToString() + "\r");
                    }
                }
            }
            computer.Close();
        }
    }
}
