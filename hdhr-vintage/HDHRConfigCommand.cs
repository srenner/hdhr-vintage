using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hdhr_vintage
{
    public class HDHRConfigCommand
    {
        #region enum-style strings
        public string Value { get; set; }
        private HDHRConfigCommand(string value) { Value = value; }
        //public static HDHRConfigCommand GetStreamInfo { get { return new HDHRConfigCommand("{0} get /tuner{1}/streaminfo"); } }
        #endregion

        public static string GetStreamInfo(string tunerID, string tunerNumber)
        {
            return tunerID + " get /tuner" + tunerNumber + "/streaminfo";
        }

        public static string GetBeginStreamCommand(string tunerID, string tunerNumber, string ip, string port)
        {
            return " " + tunerID + " set /tuner" + tunerNumber + "/target rtp://" + ip + ":" + port;
        }
    }
}
