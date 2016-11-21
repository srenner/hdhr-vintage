using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hdhr_vintage.Models
{
    /// <summary>
    /// Holds the DeviceID that you need to issue commands to a tuner
    /// From command "discover"
    /// </summary>
    public class Device
    {
        public string DeviceID { get; set; }
        public string IP { get; set; }
    }
}
