using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hdhr_vintage.Models
{
    /// <summary>
    /// RF Channel that end users don't usually recognize or care about
    /// From command "get /tuner{0}/channel"
    /// </summary>
    public class Channel
    {
        public int ChannelID { get; set; }

        public int ChannelNumber { get; set; }

        public int TunerID { get; set; }
        public Tuner Tuner { get; set; }

        public ICollection<Program> Programs { get; set; }
    }
}
