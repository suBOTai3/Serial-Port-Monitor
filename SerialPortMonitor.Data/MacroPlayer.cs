using SerialPortMonitor.Data.Models;
using Solid.Arduino;
using System;

namespace SerialPortMonitor.Data
{
    public class MacroPlayer : IMacroPlayer
    {
        public ArduinoSession Arduino { get; }
        public MacroPlayer(ArduinoSession _session)
        {
            this.Arduino = _session;
        }


        public void PlayWithProgress(Macro macro, IProgress<ProgressReportModel> progress)
        {
            var protocolVersion = this.Arduino.GetProtocolVersion();
            int stepCount = 1;

            foreach (MacroStep ms in macro.Steps)
            {
                progress.Report(new ProgressReportModel() { Description = $"Playing step {stepCount}", Value = (int)(stepCount / macro.Steps.Count) });
                Arduino.SetDigitalPinMode(ms.PinNumber, ms.PinMode);
                Arduino.SetDigitalPin(ms.PinNumber, ms.PinOutValue == Enums.PinOutMode.HIGH);
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
