using System;
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

        public static string GetHardwareModel(string deviceID)
        {
            return " " + deviceID + " get /sys/hwmodel";
        }

        public static string GetScan(string deviceID, string tunerNumber)
        {
            return " " + deviceID + " scan " + tunerNumber;
        }
    }
}
