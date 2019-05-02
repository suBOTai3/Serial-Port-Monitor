using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SerialPortMonitor.Enums
{
    /// <summary>
    /// Output type for pins
    /// </summary>
    public enum PinOutMode
    {
        LOW,
        HIGH
    }
    
    /// <summary>
    /// The type of output to use when recording data from the hardware
    /// </summary>
    public enum OutputType_e
    {
        NONE,
        CSV,
        SQL,
    }
}
