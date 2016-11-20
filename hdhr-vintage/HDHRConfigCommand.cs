using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hdhr_vintage
{
    public class HDHRConfigCommand
    {
        public string Value { get; set; }

        private HDHRConfigCommand(string value) { Value = value; }

        public static HDHRConfigCommand DoThing { get { return new HDHRConfigCommand("Do Thing"); } }

        public static HDHRConfigCommand GetStreamInfo { get { return new HDHRConfigCommand("{0} get /tuner{1}/streaminfo"); } }
    }
}
