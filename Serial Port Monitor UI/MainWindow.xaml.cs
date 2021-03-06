﻿using Microsoft.Win32;
using SerialPortMonitor.Data;
using SerialPortMonitor.Data.Models;
using Solid.Arduino;
using Solid.Arduino.Firmata;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace SerialPortUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region "Dependency Properties"

        public int AnalogPinCount
        {
            get { return (int)GetValue(AnalogPinCountProperty); }
            set { SetValue(AnalogPinCountProperty, value); }
        }

        public static readonly DependencyProperty AnalogPinCountProperty =
            DependencyProperty.Register("AnalogPinCount", typeof(int), typeof(MainWindow), new PropertyMetadata(0));

        public ArduinoSession CurrentSession { get; set; } = null;

        public int DigitalPinInCount
        {
            get { return (int)GetValue(DigitalPinInCountProperty); }
            set { SetValue(DigitalPinInCountProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DigitalPinInCount.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DigitalPinInCountProperty =
            DependencyProperty.Register("DigitalPinInCount", typeof(int), typeof(MainWindow), new PropertyMetadata(0));

        public int DigitalPinOutCount
        {
            get { return (int)GetValue(DigitalPinOutCountProperty); }
            set { SetValue(DigitalPinOutCountProperty, value); }
        }

        public static readonly DependencyProperty DigitalPinOutCountProperty =
            DependencyProperty.Register("DigitalPinCount", typeof(int), typeof(MainWindow), new PropertyMetadata(0));

        #endregion "Dependency Properties"

        private ObservableCollection<SessionCommand> PlayList { get; set; } = new ObservableCollection<SessionCommand>();

        public MainWindow()
        {
            InitializeComponent();
            LoadDefaultSettings();
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
            while (i < 200)
            {
                session.SetDigitalPin(10, true);
                System.Threading.Thread.Sleep(10);
                i++;
            }

            session.SetDigitalPin(10, false);
            progress.Report("Command sent: Light off");
        }

        private void DisplayPortCapabilities()
        {
            var port = GetPortFromSelection();
            var baud = GetBaudRateFromSelection();
            ISerialConnection connection = EnhancedSerialConnection.Find();
            if (CurrentSession == null)
                //CurrentSession = new ArduinoSession(new EnhancedSerialConnection(port, baud));
                CurrentSession = new ArduinoSession(connection);

            BoardCapability boardCapability = CurrentSession.GetBoardCapability();
            txtOutput.Text += "\nBoard Capability:";

            foreach (var pin in boardCapability.Pins)
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

        private async void btnDisplayDiscoveredCapabilities_Click(object sender, RoutedEventArgs e)
        {
        }

        private void DisplayBoardInfo()
        {
            DisplayPortCapabilities();
        }

        private void btnBrowseCSVOutput_Click(object sender, RoutedEventArgs e)
        {
            var saveFileDialog = new SaveFileDialog()
            {
                Title = "Choose a file"
                ,
                AddExtension = true
                ,
                Filter = "Comma seperated files (*.csv)|*.csv|All files (*.*)|*.*"
                ,
                FilterIndex = 0
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

        private void LoadDefaultSettings()
        {
            foreach (SerialBaudRate baudRate in (SerialBaudRate[])Enum.GetValues(typeof(SerialBaudRate)))
                cboBaudrateSelection.Items.Add(baudRate.ToString().Replace("Bps_", ""));

            cboPortSelection.Items.Add("COM1");
            cboPortSelection.Items.Add("COM2");
            cboPortSelection.Items.Add("COM3");
            cboPortSelection.Items.Add("COM4");
            cboPortSelection.Items.Add("COM5");
            cboPortSelection.Items.Add("COM6");
            cboPortSelection.Items.Add("COM7");
            cboPortSelection.Items.Add("COM8");
        }

        private void LoadUserSettings()
        {
            //TODO: Load usersettings from persistent storage
        }

        private void LoadConnectionSettings(string comPort, SerialBaudRate baudRate)
        {
            if (CurrentSession == null)
                CurrentSession = new ArduinoSession(new EnhancedSerialConnection(comPort, baudRate));

            BoardCapability boardCapability = CurrentSession.GetBoardCapability();
            this.DigitalPinInCount = boardCapability.Pins.Select(pp => pp.DigitalInput).Count();
            this.DigitalPinOutCount = boardCapability.Pins.Select(pp => pp.DigitalOutput).Count();
            this.AnalogPinCount = boardCapability.Pins.Select(pp => pp.Analog).Count();
        }

        private void BtnTestPinSettings_Click(object sender, RoutedEventArgs e)
        {
            MacroStep pm = new MacroStep();
            pm.Delay = MacroDelay.Value.GetValueOrDefault();
            pm.PinNumber = MacroPin.Value.GetValueOrDefault();
        }

        private async void Connection_Checked(object sender, RoutedEventArgs e)
        {
            if (sender is System.Windows.Controls.Primitives.ToggleButton tButton)
            {
                if (btnConnectToBoard.IsChecked.GetValueOrDefault())
                {
                    //DisplayBoardInfo();
                    //LoadConnectionSettings(GetPortFromSelection(), GetBaudRateFromSelection());
                    IProgress<string> progress = new Progress<string>(statusUpdate => txtOutput.Text += "\n" + statusUpdate);
                    ISerialConnection connection = await GetConnection(progress);

                    if (connection != null)
                    {
                        var session = new ArduinoSession(connection);
                        PerformBasicTest(session, progress);
                        session.Dispose();
                        connection.Close();
                        DisplayPortCapabilities();
                    }
                    else
                        btnConnectToBoard.IsChecked = false;
                }
                else
                {
                    if (CurrentSession != null)
                        CurrentSession.Dispose();
                }
            }
        }

        private string GetPortFromSelection()
        {
            return cboPortSelection.SelectedValue.ToString();
        }

        private SerialBaudRate GetBaudRateFromSelection()
        {
            return ((SerialBaudRate)int.Parse(cboBaudrateSelection.SelectedValue.ToString()));
        }
    }
}