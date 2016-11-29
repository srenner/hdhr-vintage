using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hdhr_vintage.Models
{
    /// <summary>
    /// Represents the channel you'd see while watching a regular TV (e.g. "9.1 KETCHD")
    /// From command "get /tuner{0}/streaminfo"
    /// </summary>
    public class Program
    {
        
        public string ProgramNumber { get; set; }
        [Key]
        public string FriendlyChannelNumber { get; set; }
        public string CallSign { get; set; }

        public int ChannelNumber { get; set; }
        public Channel Channel { get; set; }
    }
}
