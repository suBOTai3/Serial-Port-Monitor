using SerialPortMonitor.Data.Models;
using Solid.Arduino;
using SerialPortMonitor.Data;
using System;
using Solid.Arduino.Firmata;
using System.Collections.Generic;
using Xunit;
using Autofac.Extras.Moq;

namespace SerialPortMonitor.DataTests
{
     
    public class MacroTests
    {
        [Fact]
        public void MockMacroPlayerFact_Test()
        {
            using (var mock = AutoMock.GetLoose())
            {
                mock.Mock<IMacroPlayer>()
                    .Setup(mm => mm.PlayWithProgress    )

                    //see https://youtu.be/DwbYxP-etMY?t=1958

            }
        }
        
        [Fact]
        public void BasicPinMacroTest()
        {
            ISerialConnection connection = GetConnection();
            var macroWithSteps = GetMacroWithStepsMockup();
            
            IProgress<ProgressReportModel> progress = new Progress<ProgressReportModel>();

            if (connection != null)
                using (var session = new ArduinoSession(connection))
                {
                    session.PlayWithProgress(macroWithSteps, progress);
                }
        }

        private static IMacro GetMacroWithStepsMockup()
        {
            var retVal = new List<MacroStep>();

            MacroStep s1 = new MacroStep
            {
                Delay = 1000,
                PinNumber = 10,
                PinMode = PinMode.DigitalOutput,
                PinOutValue = Enums.PinOutMode.HIGH
            };

            MacroStep s2 = new MacroStep
            {
                Delay = 500,
                PinNumber = 10,
                PinMode = PinMode.DigitalOutput,
                PinOutValue = Enums.PinOutMode.LOW
            };

            retVal.Add(s1); // On
            retVal.Add(s2); // Off
            retVal.Add(s1); // On
            retVal.Add(s2); // Off
            retVal.Add(s1); // On
            retVal.Add(s2); // Off

            IMacro macro = new Macro() { Steps = retVal };
            return macro;
        }

        private ISerialConnection GetConnection()
        {
            return EnhancedSerialConnection.Find();

        }
    }
}
