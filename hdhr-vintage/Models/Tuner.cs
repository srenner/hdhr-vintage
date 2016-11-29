using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hdhr_vintage.Models
{
    public class Tuner
    {
        [Key, Column(Order = 0)]
        public int TunerID { get; set; }

        [Key, Column(Order = 1)]
        public string DeviceID { get; set; }
        public Device Device { get; set; }

        public string ChannelMap { get; set; }
    }
}
