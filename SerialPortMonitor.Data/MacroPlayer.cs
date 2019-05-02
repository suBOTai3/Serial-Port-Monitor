using SerialPortMonitor.Data.Models;
using Solid.Arduino;
using System;

namespace SerialPortMonitor.Data
{
    public static class MacroPlayer
    {
        
        public static void PlayWithProgress(this ArduinoSession session, Macro macro, IProgress<ProgressReportModel> progress)
        {
            var protocolVersion = session.GetProtocolVersion();
            int stepCount = 1;

            foreach(MacroStep ms in macro.Steps)
            {
                progress.Report(new ProgressReportModel() { Description = $"Playing step {stepCount}", Value = (int)(stepCount / macro.Steps.Count) });
                session.SetDigitalPinMode(ms.PinNumber, ms.PinMode);
                session.SetDigitalPin(ms.PinNumber, ms.PinOutValue == Enums.PinOutMode.HIGH);
                long i = 0;
                while (i < ms.Delay)
                {
                    System.Threading.Thread.Sleep(10);
                    i += 10;
                }
            }

        }
    }
}
