using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cosmoser.PingAnMeetingRequest.Outlook2010.Views
{
    public class CapacityInfo
    {
        public string Label { get; set; }
        public string Value { get; set; }

        public override string ToString()
        {
            return Label;
        }
    }
}
