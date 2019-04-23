using System.ServiceProcess;

namespace SerialPortMonitorSVC
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        private static void Main()
        {
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new SerialPortMonitorSVC()
                {
                    ServiceName = "SerialPortMonitorSVC",
                    CanStop = true,
                    CanPauseAndContinue = true,
                     ExitCode = 0,
                                  AutoLog = true,
                }
            };
            ServiceBase.Run(ServicesToRun);
        }
    }
}