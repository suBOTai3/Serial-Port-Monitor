using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SerialPortMonitor.Data
{
    public static class UserSettings
    {
        public static string CSVOutputPath()
        {
            System.Diagnostics.Debug.Print("here");
            return "";
        }
        public static void SaveCSVOutputPath(string destinationFilePath)
        {
            System.Diagnostics.Debug.Print(destinationFilePath);
        }
         
    }
}
