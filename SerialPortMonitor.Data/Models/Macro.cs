using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SerialPortMonitor.Data.Models
{
    public class Macro
    {
        public List<MacroStep> Steps { get; set; } = new List<MacroStep>();
    }
}
