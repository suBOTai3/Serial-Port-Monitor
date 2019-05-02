﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using SerialPortMonitor.Data.Models;
using Solid.Arduino;
using SerialPortMonitor.Data;
using System;
using Solid.Arduino.Firmata;
using System.Collections.Generic;

namespace SerialPortMonitor.DataTests
{
    [TestClass]
    public class MacroTests
    {
        [TestMethod]
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

        private static Macro GetMacroWithStepsMockup()
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

            Macro macro = new Macro() { Steps = retVal };
            return macro;
        }

        private ISerialConnection GetConnection()
        {
            return EnhancedSerialConnection.Find();

        }
    }
}
