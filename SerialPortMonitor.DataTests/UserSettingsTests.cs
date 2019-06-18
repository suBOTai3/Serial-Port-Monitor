using Autofac.Extras.Moq;
using SerialPortMonitor.Data;
using SerialPortMonitor.Data.Models;
using Solid.Arduino.Firmata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SerialPortMonitor.Data.Tests
{
    public class UserSettingsTests
    {
        
        [Theory]
        [InlineData("Andries Koorzen")]
        public void MyTheory(string username)
        {
            var expected = "Andries Koorzen";
            Assert.Equal(expected, username);
        }

        private static IMacro GetMacroWith2StepsMock()
        {
            var retVal = new List<MacroStep>() {

                new MacroStep
                {
                    Delay = 1000,
                    PinNumber = 10,
                    PinMode = PinMode.DigitalOutput,
                    PinOutValue = Enums.PinOutMode.HIGH
                },

                 new MacroStep
                {
                    Delay = 500,
                    PinNumber = 10,
                    PinMode = PinMode.DigitalOutput,
                    PinOutValue = Enums.PinOutMode.LOW
                }
            };

            return new Macro() { Steps = retVal };
            
        }

    }
}