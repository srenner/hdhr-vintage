using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hdhr_vintage.Models
{
    public class Tuner
    {
        public string TunerID { get; set; }

        public string DeviceID { get; set; }
        public Device Device { get; set; }
    }
}
