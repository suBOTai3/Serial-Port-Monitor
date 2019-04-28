using SerialPortMonitor.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SerialPortMonitor.Data
{
    internal class RuntimeSettings
    {
        public string CSVPath { get; set; }
        public OutputType_e OutputType { get; set; }
    }
}
