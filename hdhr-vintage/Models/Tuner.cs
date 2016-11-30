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
        public int TunerID { get; set; }
        public int TunerNumber { get; set; }

        public string DeviceID { get; set; }
        public Device Device { get; set; }

        public string ChannelMap { get; set; }

        public ICollection<Channel> Channels { get; set; }
    }
}
