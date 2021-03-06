﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hdhr_vintage
{
    public static class HDHRConfigCommand
    {
        public static string GetStreamInfo(string deviceID, string tunerNumber)
        {
            return deviceID + " get /tuner" + tunerNumber + "/streaminfo";
        }

        public static string GetBeginStreamCommand(string deviceID, string tunerNumber, string ip, string port)
        {
            return " " + deviceID + " set /tuner" + tunerNumber + "/target rtp://" + ip + ":" + port;
        }

        public static string GetEndStreamCommand(string deviceID, string tunerNumber)
        {
            return " " + deviceID + " set /tuner" + tunerNumber + "/target none";
        }

        public static string GetSetChannelCommand(string deviceID, string tunerNumber, string channelNumber)
        {
            return " " + deviceID + " set /tuner" + tunerNumber + "/channel " + channelNumber;
        }

        public static string GetSetProgramCommand(string deviceID, string tunerNumber, string programNumber)
        {
            return " " + deviceID + " set /tuner" + tunerNumber + "/program " + programNumber;
        }

        public static string GetHardwareModel(string deviceID)
        {
            return " " + deviceID + " get /sys/hwmodel";
        }

        public static string GetScan(string deviceID, int tunerNumber)
        {
            return " " + deviceID + " scan " + tunerNumber;
        }
    }
}
