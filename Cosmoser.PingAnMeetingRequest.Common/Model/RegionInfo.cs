using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cosmoser.PingAnMeetingRequest.Common.Model
{
    public class RegionInfo
    {
        public string Code { get; set; }
        public string Name { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
