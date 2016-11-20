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
        }

        private void UpdateStatusBarText(string text)
        {
            Dispatcher.Invoke(() =>
            {
                statusText.Text = text.Trim();
            });
        }
    }
}
