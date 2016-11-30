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
    /// Represents the channel you'd see while watching a regular TV (e.g. "9.1 KETCHD")
    /// From command "get /tuner{0}/streaminfo"
    /// </summary>
    public class Program
    {
        public int ProgramID { get; set; }
        public string ProgramNumber { get; set; }
        public string FriendlyChannelNumber { get; set; }
        public string CallSign { get; set; }

        public int ChannelID { get; set; }
        public Channel Channel { get; set; }

        [NotMapped]
        public int FriendlyChannelMain
        {
            get
            {
                return 0;
            }
        }

        [NotMapped]
        public int FriendlyChannelSub
        {
            get
            {
                return 0;
            }
        }

        public override string ToString()
        {
            return FriendlyChannelNumber;
        }
    }
}
