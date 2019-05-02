using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SerialPortMonitor.Data.Models
{
    /// <summary>
    /// Helperclass to give the user and indication of how far the macro has progressed
    /// </summary>
    public class ProgressReportModel
    {
        /// <summary>
        /// Current progress value of the macro (typically in percent)
        /// </summary>
        public int Value { get; set; } = 0;
        /// <summary>
        /// Description of what the current step in the macro is doing
        /// </summary>
        public string Description { get; set; } = "";
    }
}
