using System.Collections.Generic;

namespace SerialPortMonitor.Data.Models
{
    public interface IMacro
    {
        List<MacroStep> Steps { get; set; }
    }
}