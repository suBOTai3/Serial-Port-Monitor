using Microsoft.Win32;
using SerialPortMonitor.Data;
using SerialPortMonitor.Enums;
using Solid.Arduino;
using Solid.Arduino.Firmata;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SerialPortUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ObservableCollection<SessionCommand> PlayList { get; set; } = new ObservableCollection<SessionCommand>();
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void btnPlay_Clicked(object sender, RoutedEventArgs e)
        {
          
            foreach (var command in PlayList)
            {
                //session.Send(command);
            }
        }

        private async Task<ISerialConnection> GetConnection(IProgress<string> progress)
        {
            progress.Report("Searching Arduino connection...");
            ISerialConnection connection = await Task.Run(() => EnhancedSerialConnection.Find());

            if (connection == null)
                progress.Report("No connection found. Make sure your Arduino board is attached to a USB port.");
            else
                progress.Report($"Connected to port {connection.PortName} at {connection.BaudRate} baud.");

            return connection;
        }

        private void PerformBasicTest(IFirmataProtocol session, IProgress<string> progress)
        {
            var firmware = session.GetFirmware();
            progress.Report($"Firmware: {firmware.Name} version {firmware.MajorVersion}.{firmware.MinorVersion}");
            var protocolVersion = session.GetProtocolVersion();
            progress.Report($"Firmata protocol version {protocolVersion.Major}.{protocolVersion.Minor}");

            session.SetDigitalPinMode(10, PinMode.DigitalOutput);
            progress.Report("Command sent: Light on (pin 10) for 2 seconds");
            int i = 0;
            while(i < 200){

                session.SetDigitalPin(10, true);
                System.Threading.Thread.Sleep(10);
                i++;
            }
             
            session.SetDigitalPin(10, false);
            progress.Report("Command sent: Light off");
        }
 
        private void DisplayPortCapabilities()
        {
            using (var session = new ArduinoSession(new EnhancedSerialConnection("COM3", SerialBaudRate.Bps_57600)))
            {
                BoardCapability cap = session.GetBoardCapability();
                txtOutput.Text += "\nBoard Capability:";

                foreach (var pin in cap.Pins)
                {
                    txtOutput.Text += string.Format("\nPin {0}: " +
                        "\n\tInput: {1} " +
                        "\t\tOutput: {2} " +
                        "\t\tAnalog: {3}" +
                        "\t\tAnalog-Res: {4}" +
                        "\n\tPWM: {5}" +
                        "\t\tPWM-Res: {6}" + 
                        "\t\tServo: {7}" + 
                        "\t\tServo-Res: {8}" + 
                        "\n\tSerial: {9}" + 
                        "\t\tEncoder: {10}" + 
                        "\t\tInput-pullup: {11}",
                        pin.PinNumber,
                        pin.DigitalInput,
                        pin.DigitalOutput,
                        pin.Analog,
                        pin.AnalogResolution,
                        pin.Pwm,
                        pin.PwmResolution,
                        pin.Servo,
                        pin.ServoResolution,
                        pin.Serial,
                        pin.Encoder,
                        pin.InputPullup);
                }
            }
        }

        private async void btnDisplayDiscoveredCapabilities_Click(object sender, RoutedEventArgs e)
        {
            IProgress<string> progress = new Progress<string>(statusUpdate => txtOutput.Text += "\n" + statusUpdate);
            ISerialConnection connection = await GetConnection(progress);

            if (connection != null)
                using (var session = new ArduinoSession(connection))
                    PerformBasicTest(session, progress);

            DisplayPortCapabilities();
        }

        private void btnBrowseCSVOutput_Click(object sender, RoutedEventArgs e)
        {
            var saveFileDialog = new SaveFileDialog() {
                  Title = "Choose a file"
                , AddExtension = true
                , Filter = "Comma seperated files (*.csv)|*.csv|All files (*.*)|*.*"
                , FilterIndex = 0
                 };

            if (saveFileDialog.ShowDialog() == true)
            {
                txtOutputCSV.Text = saveFileDialog.FileName;
                UserSettings.SaveCSVOutputPath(saveFileDialog.FileName);
            }
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            LoadUserSettings();
        }

        private void LoadUserSettings()
        {
            //TODO: Load usersettings from persistent storage
        }
    }
}
