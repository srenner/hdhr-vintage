using hdhr_vintage.DataAccess;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace hdhr_vintage
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        
        //todo fetch this from the textbox
        private string ConfigExecutable = @"C:\Program Files\Silicondust\HDHomeRun\hdhomerun_config.exe";
        private string VideoPlayerExecutable = @"C:\Program Files (x86)\VideoLAN\VLC\vlc.exe";

        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnTunerScan_Click(object sender, RoutedEventArgs e)
        {
            DatabaseCommand.CreateDatabase();

            var service = new Service(ConfigExecutable, VideoPlayerExecutable);

            string configOutput = service.ExecuteConfigProcess("discover");

            var device = DatabaseCommand.CreateEntity(Service.ParseDevice(configOutput));

            string hardwareModel = service.ExecuteConfigProcess(HDHRConfigCommand.GetHardwareModel(device.DeviceID));

            int tunerCount = Service.GetTunerCount(hardwareModel);

            for(int i = 0; i < tunerCount; i++)
            {
                var tuner = new Models.Tuner();
                tuner.DeviceID = device.DeviceID;
                tuner.TunerID = i.ToString();
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
            text = text.Trim() + "\r\n==========\r\n" + txtInfo.Text;

            Dispatcher.Invoke(() => {
                txtInfo.Text = text;
            });
        }

        private void btnStreamInfo_Click(object sender, RoutedEventArgs e)
        {
            string args = HDHRConfigCommand.GetStreamInfo("10183772", "1");
            //var result = ExecuteCommand(args);
            var service = new Service(ConfigExecutable, VideoPlayerExecutable);
            var result = service.ExecuteConfigProcess(args);
            UpdateInfoText(result);
        }

        private void btnLaunch_Click(object sender, RoutedEventArgs e)
        {
            string ip = NetworkHelper.GetLocalIP();
            int port = NetworkHelper.GetAvailablePort(ip);

            string args = HDHRConfigCommand.GetBeginStreamCommand("10183772", "1", ip, port.ToString());

            var service = new Service(ConfigExecutable, VideoPlayerExecutable);

            string result = service.ExecuteConfigProcess(args);

            UpdateInfoText(result);

            var process = new Process();
            process.StartInfo.FileName = VideoPlayerExecutable;
            process.StartInfo.Arguments = "rtp://@" + ip + ":" + port.ToString();
            process.EnableRaisingEvents = true;
            process.Start();
            process.Exited += Process_Exited;
        }

        private void Process_Exited(object sender, EventArgs e)
        {
            UpdateStatusBarText("todo shut off stream");
        }

        private void btnChannelScan_Click(object sender, RoutedEventArgs e)
        {
            string args = HDHRConfigCommand.GetScan(DatabaseCommand.GetDevices()[0].DeviceID, "1");

            var svc = new Service(ConfigExecutable, VideoPlayerExecutable);
            string output = svc.ExecuteConfigProcess(args);

            UpdateInfoText(output);
        }
    }
}
