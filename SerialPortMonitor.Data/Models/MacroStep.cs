using SerialPortMonitor.Enums;
using Solid.Arduino.Firmata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SerialPortMonitor.Data.Models
{
    public class MacroStep
    {
        /// <summary>
        /// How long the system thread should wait before executing the next step / command 
        /// </summary>
        public long Delay { get; set; } = 1000;
        /// <summary>
        /// The pin to be manipulated via input / output
        /// </summary>
        public int PinNumber { get; set; } = 0;
        /// <summary>
        /// The type of mode to use on the pin
        /// </summary>
        public PinMode PinMode { get; set; } = PinMode.DigitalOutput;
        /// <summary>
        /// The value to set the pin to: HIGH / LOW
        /// </summary>
        public PinOutMode PinOutValue { get; set; } = PinOutMode.HIGH;

    }
}
