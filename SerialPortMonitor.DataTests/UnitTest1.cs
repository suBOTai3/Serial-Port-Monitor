using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SerialPortMonitorSVC.Models;

namespace SerialPortMonitor.DataTests
{
    [TestClass]
    public class MacroTests
    {
        [TestMethod]
        public void BasicPinMacroTest()
        {
            PinMacro pm = new PinMacro();
            pm.Delay = MacroDelay.Value.GetValueOrDefault();
            pm.PinNumber = MacroPin.Value.GetValueOrDefault();
        }
    }
}
