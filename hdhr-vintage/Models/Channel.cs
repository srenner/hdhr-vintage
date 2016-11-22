using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        [Key]
        public int ChannelNumber { get; set; }

        public int TunerID { get; set; }
        public Tuner Tuner { get; set; }

    }
}
