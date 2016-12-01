using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using hdhr_vintage.Models;
using System.IO;

namespace hdhr_vintage
{
    public class Service
    {
        public string ConfigPath = @"C:\Program Files\Silicondust\HDHomeRun\hdhomerun_config.exe";
        public string VideoPlayerPath = @"C:\Program Files (x86)\VideoLAN\VLC\vlc.exe";

        public Service(string configPath, string videoPlayerPath)
        {
            ConfigPath = configPath;
            VideoPlayerPath = videoPlayerPath;
        }

        public string ExecuteConfigProcess(string args)
        {
            var proc = new Process();
            proc.StartInfo.FileName = ConfigPath;
            proc.StartInfo.Arguments = args;
            proc.StartInfo.UseShellExecute = false;
            proc.StartInfo.RedirectStandardOutput = true;
            proc.StartInfo.CreateNoWindow = true;
            proc.Start();
            string output = proc.StandardOutput.ReadToEnd().Trim();
            proc.WaitForExit();
            int exitCode = proc.ExitCode;
            proc.Close();

            return output;
        }

        public StreamReader ExecuteConfigStream(string args)
        {
            var proc = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = ConfigPath,
                    Arguments = args,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true
                }
            };
            proc.Start();
            return proc.StandardOutput;
        }

        public void ParseChannelScan(string scanOutput, Tuner tuner)
        {
            string[] lines = scanOutput.Split('\n');
            Channel channel = new Channel();
            foreach (string line in lines)
            {
                if (line.Contains(tuner.ChannelMap + ":"))
                {
                    channel = new Channel
                    {
                        ChannelNumber = int.Parse(line.Substring(line.IndexOf(tuner.ChannelMap) + tuner.ChannelMap.Length + 1).Trim().TrimEnd(')')),
                        TunerID = tuner.TunerID
                    };
                    channel = DataAccess.DatabaseCommand.CreateEntity(channel);
                }

                if (line.ToUpper().StartsWith("PROGRAM"))
                {
                    if(channel.ChannelNumber > 0)
                    {
                        var program = new Program();
                        program.ChannelID = channel.ChannelID;

                        string[] programComponents = line.Split(' ');

                        program.ProgramNumber = programComponents[1].TrimEnd(':');
                        program.FriendlyChannelNumber = programComponents[2];
                        program.CallSign = programComponents[3].TrimEnd('\r');

                        program = DataAccess.DatabaseCommand.CreateEntity(program);
                    }
                }
            }
        }


        public static Device ParseDevice(string unparsedText)
        {
            //example
            //hdhomerun device 10183772 found at 192.168.1.184

            var device = new Device();
            if (unparsedText.ToLower().StartsWith("hdhomerun device ") && unparsedText.ToLower().Contains(" found at "))
            {
                //hdhomerun_config docs strongly imply the device id is always 8 characters
                //but this parsing could be improved anyway
                device.DeviceID = unparsedText.Substring("hdhomerun device ".Length, 8);
                device.IP = unparsedText.Substring(unparsedText.Trim().LastIndexOf(' ') + 1);
            }
            else
            {
                //
            }
            return device;
        }

        public static int GetTunerCount(string model)
        {
            switch (model.ToLower())
            {
                case "hdhr-us":
                    {
                        return 2;
                    }
                default:
                    {
                        return 2;
                    }
            }
        }
    }
}
