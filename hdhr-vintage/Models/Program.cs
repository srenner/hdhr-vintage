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

        public bool IsFavorite { get; set; }

        public int ChannelID { get; set; }
        public Channel Channel { get; set; }

        [NotMapped]
        public int FriendlyChannelMain
        {
            get
            {
                int output = 0;
                string delimiter = GetChannelDelimiter();
                
                if(delimiter == null)
                {
                    return output;
                }
                else
                {
                    int.TryParse(FriendlyChannelNumber.Substring(0, FriendlyChannelNumber.IndexOf(delimiter[0])), out output);
                    return output;
                }
            }
        }

        [NotMapped]
        public int FriendlyChannelSub
        {
            get
            {
                int output = 0;
                string delimiter = GetChannelDelimiter();

                if (delimiter == null)
                {
                    return output;
                }
                else
                {
                    int.TryParse(FriendlyChannelNumber.Substring(FriendlyChannelNumber.IndexOf(delimiter[0]) + 1), out output);
                    return output;
                }
            }
        }

        /// <summary>
        /// Get the character that separates the main channel number from the sub number.
        /// ex: Channel 9.1 has a main of 9 and sub of 1, and this would return "."
        /// </summary>
        /// <returns></returns>
        private string GetChannelDelimiter()
        {
            if (FriendlyChannelNumber.Contains("."))
            {
                return ".";
            }
            else if (FriendlyChannelNumber.Contains("-"))
            {
                return "-";
            }
            else
            {
                return null;
            }
        }

        public override string ToString()
        {
            return FriendlyChannelNumber + " " + CallSign;
        }
    }
}
