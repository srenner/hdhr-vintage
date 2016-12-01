using hdhr_vintage.DataAccess;
using hdhr_vintage.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace hdhr_vintage
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Process _activeSession;
        private Service _service;

        public MainWindow()
        {
            InitializeComponent();

            //todo make paths configurable
            _service = new Service(@"C:\Program Files\Silicondust\HDHomeRun\hdhomerun_config.exe", @"C:\Program Files (x86)\VideoLAN\VLC\vlc.exe");

            //todo this will break if the database is empty
            var tuner = DatabaseCommand.GetTuner(DatabaseCommand.GetDevices()[0].DeviceID, 1);
            var programs = DatabaseCommand.GetPrograms(tuner.TunerID);
            gridPrograms.ItemsSource = programs;
        }

        private void btnTunerScan_Click(object sender, RoutedEventArgs e)
        {
            DatabaseCommand.CreateDatabase();

            string configOutput = _service.ExecuteConfigProcess("discover");

            var device = DatabaseCommand.CreateEntity(Service.ParseDevice(configOutput));

            string hardwareModel = _service.ExecuteConfigProcess(HDHRConfigCommand.GetHardwareModel(device.DeviceID));

            int tunerCount = Service.GetTunerCount(hardwareModel);

            for(int i = 0; i < tunerCount; i++)
            {
                //todo get real channelmap
                var tuner = new Models.Tuner();
                tuner.DeviceID = device.DeviceID;
                tuner.TunerNumber = i;
                tuner.ChannelMap = "us-bcast"; //temporary
                DatabaseCommand.CreateEntity(tuner);
            }

            

            var devices = DatabaseCommand.GetDevices();

            UpdateStatusBarText("Found " + device.DeviceID + " at " + device.IP);
        }

        private void UpdateStatusBarText(string text)
        {
            Dispatcher.Invoke(() =>
            {
                statusText.Text = text.Trim();
            });
        }

        private void UpdateInfoText(string text)
        {

            string newText = text + System.Environment.NewLine;

            Dispatcher.Invoke(() => {
                txtInfo.Text = newText + txtInfo.Text;
            });
        }

        private void btnStreamInfo_Click(object sender, RoutedEventArgs e)
        {
            string args = HDHRConfigCommand.GetStreamInfo("10183772", "1");
            var result = _service.ExecuteConfigProcess(args);
            UpdateInfoText(result);
        }

        private void Process_Exited(object sender, EventArgs e)
        {
            string args = HDHRConfigCommand.GetEndStreamCommand("10183772", "1");
            string result = _service.ExecuteConfigProcess(args);
        }

        private async void btnChannelScan_Click(object sender, RoutedEventArgs e)
        {
            //todo fix this
            var tuner = DatabaseCommand.GetTuner(DatabaseCommand.GetDevices()[0].DeviceID, 1);

            string args = HDHRConfigCommand.GetScan(tuner.DeviceID, tuner.TunerNumber);
            var scanStream = _service.ExecuteConfigStream(args);

            var sbStreamText = new StringBuilder();

            if (scanStream != null)
            {
                int programCount = 0;
                string channelScanBaseText = "Performing channel scan";
                UpdateStatusBarText(channelScanBaseText);
                await Task.Factory.StartNew(() =>
                {

                    while (!scanStream.EndOfStream)
                    {
                        string line = scanStream.ReadLine();
                        if(line.ToUpper().StartsWith("PROGRAM"))
                        {
                            programCount++;
                            UpdateStatusBarText(channelScanBaseText + " (found " + programCount + ")");
                        }
                        sbStreamText.AppendLine(line);
                        UpdateInfoText(line);
                    }
                    UpdateStatusBarText("Channel scan complete (found " + programCount + ")");
                });
            }

            _service.ParseChannelScan(sbStreamText.ToString(), tuner);
        }

        private void btnPrograms_Click(object sender, RoutedEventArgs e)
        {
            //temporary
            var tuner = DatabaseCommand.GetTuner(DatabaseCommand.GetDevices()[0].DeviceID, 1);

            var programs = DatabaseCommand.GetPrograms(tuner.TunerID);
            gridPrograms.ItemsSource = programs;
        }

        private void btnWatch_Click(object sender, RoutedEventArgs e)
        {
            if (_activeSession != null && !_activeSession.HasExited)
            {
                _activeSession.CloseMainWindow();
            }


            Button btn = (Button)sender;
            Program program = (Program)btn.DataContext;

            string channelArgs = HDHRConfigCommand.GetSetChannelCommand(program.Channel.Tuner.DeviceID, program.Channel.Tuner.TunerNumber.ToString(), program.Channel.ChannelNumber.ToString());
            string channelResult = _service.ExecuteConfigProcess(channelArgs);

            string programArgs = HDHRConfigCommand.GetSetProgramCommand(program.Channel.Tuner.DeviceID, program.Channel.Tuner.TunerNumber.ToString(), program.ProgramNumber);
            string programResult = _service.ExecuteConfigProcess(programArgs);

            string ip = NetworkHelper.GetLocalIP();
            int port = NetworkHelper.GetAvailablePort(ip);
            string streamArgs = HDHRConfigCommand.GetBeginStreamCommand(program.Channel.Tuner.DeviceID, program.Channel.Tuner.TunerNumber.ToString(), ip, port.ToString());
            string result = _service.ExecuteConfigProcess(streamArgs);

            UpdateInfoText(result);

            _activeSession = new Process();
            _activeSession.StartInfo.FileName = _service.VideoPlayerPath;
            _activeSession.StartInfo.Arguments = "rtp://@" + ip + ":" + port.ToString();
            _activeSession.EnableRaisingEvents = true;
            _activeSession.Start();
            _activeSession.Exited += Process_Exited;
        }
    }
}
