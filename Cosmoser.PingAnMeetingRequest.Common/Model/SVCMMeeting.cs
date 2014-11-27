using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cosmoser.PingAnMeetingRequest.Common.Model
{
    class SVCMMeeting
    {
        public string Subject { get; set; }
        public List<MeetingRoom> Rooms { get; set; }
        public MeetingRoom MainRoom { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public MeetingParameter Parameter { get; set; }
        public MeetingType Type { get; set; }
    }
}
