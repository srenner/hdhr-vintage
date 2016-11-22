using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hdhr_vintage.Models
{
    public class Tuner
    {
        [Key]
        public string TunerID { get; set; }

        public string DeviceID { get; set; }
        public Device Device { get; set; }
    }
}
