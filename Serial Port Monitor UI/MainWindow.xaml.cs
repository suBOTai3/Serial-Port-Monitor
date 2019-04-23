using SerialPortMonitor.Data;
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

        private void btnPlay_Clicked(object sender, RoutedEventArgs e)
        {
            ISerialConnection connection = GetConnection();

            if (connection != null)
                using (var session = new ArduinoSession(connection))
                    PerformBasicTest(session);

            foreach (var command in PlayList)
            {
                //session.Send(command);
            }
        }

        private ISerialConnection GetConnection()
        {
            txtOutput.Text += "Searching Arduino connection...";
            ISerialConnection connection = EnhancedSerialConnection.Find();

            if (connection == null)
                txtOutput.Text += "No connection found. Make shure your Arduino board is attached to a USB port.";
            else
                txtOutput.Text += $"Connected to port {connection.PortName} at {connection.BaudRate} baud.";

            return connection;
        }

        private void PerformBasicTest(IFirmataProtocol session)
        {
            var firmware = session.GetFirmware();
            txtOutput.Text += $"Firmware: {firmware.Name} version {firmware.MajorVersion}.{firmware.MinorVersion}";
            var protocolVersion = session.GetProtocolVersion();
            txtOutput.Text += $"Firmata protocol version {protocolVersion.Major}.{protocolVersion.Minor}";

            session.SetDigitalPinMode(10, PinMode.DigitalOutput);
            session.SetDigitalPin(10, true);
            txtOutput.Text += "Command sent: Light on (pin 10)";
            txtOutput.Text += "Press a key";
             
            session.SetDigitalPin(10, false);
            txtOutput.Text += "Command sent: Light off";
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
                        "\n\tOutput: {2} " +
                        "\n\tAnalog: {3}" +
                        "\n\tAnalog-Res: {4}, PWM: {5}, PWM-Res: {6}" + 
                        "\n\tServo: {7}" + 
                        "\n\tServo-Res: {8}" + 
                        "\n\tSerial: {9}" + 
                        "\n\tEncoder: {10}" + 
                        "\n\tInput-pullup: {11}",
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

        private void btnDisplayDiscoveredCapabilities_Click(object sender, RoutedEventArgs e)
        {
            DisplayPortCapabilities();
        }
    }
}
