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
        private string ConfigExecutable = "C:\\Program Files\\Silicondust\\HDHomeRun\\hdhomerun_config.exe";
        private string TunerID = "";

        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnTunerScan_Click(object sender, RoutedEventArgs e)
        {
            string discoverCommand = ConfigExecutable + " discover";
            Console.WriteLine(DateTime.Now.ToString("HH:mm:ss.fff") + " - " + "performing tuner scan");

            var proc = new Process();
            proc.StartInfo.FileName = ConfigExecutable;
            proc.StartInfo.Arguments = "discover";
            proc.StartInfo.UseShellExecute = false;
            proc.StartInfo.RedirectStandardOutput = true;
            proc.StartInfo.CreateNoWindow = true;
            proc.Start();
            string output = proc.StandardOutput.ReadToEnd();
            proc.WaitForExit();
            int exitCode = proc.ExitCode;
            proc.Close();

            Console.WriteLine(DateTime.Now.ToString("HH:mm:ss.fff") + " - " + "tuner scan output (exit code " + exitCode + ") - " + output);

            UpdateStatusBarText(output);
            UpdateInfoText(output);
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
            string command = String.Format(HDHRConfigCommand.GetStreamInfo.Value, "10183772", "1");

            var result = ExecuteCommand(command);

            UpdateInfoText(result);
        }


        private string ExecuteCommand(string arguments)
        {
            var process = new Process();
            process.StartInfo.FileName = ConfigExecutable;
            process.StartInfo.Arguments = arguments;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.CreateNoWindow = true;
            process.Start();
            string output = process.StandardOutput.ReadToEnd();
            process.WaitForExit();
            int exitCode = process.ExitCode;
            process.Close();

            return output;
        }

        private void btnLaunch_Click(object sender, RoutedEventArgs e)
        {


            //string commandArgs = "10183772 set /tuner1/target rtp://192.168.1.81:5000";
            //ExecuteCommand(commandArgs);
            string ip = NetworkHelper.GetLocalIP();
            int port = NetworkHelper.GetAvailablePort(ip);


            string args = HDHRConfigCommand.GetBeginStreamCommand("10183772", "1", ip, port.ToString());
            string result = ExecuteCommand(args);

            UpdateInfoText(result);


            var process = new Process();
            process.StartInfo.FileName = @"C:\Program Files (x86)\VideoLAN\VLC\vlc.exe";
            process.StartInfo.Arguments = "rtp://@" + ip + ":" + port.ToString();

            process.Start();


        }
    }
}
