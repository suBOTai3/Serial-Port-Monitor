using System;
using SerialPortMonitor.Data.Models;
using Solid.Arduino;

namespace SerialPortMonitor.Data
{
    public interface IMacroPlayer
    {
        ArduinoSession Arduino { get; }

        void PlayWithProgress(Macro macro, IProgress<ProgressReportModel> progress);
    }
}