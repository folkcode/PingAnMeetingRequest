using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cosmoser.PingAnMeetingRequest.Common.Model
{
    public class MeetingListQuery
    {
        public string MeetingName { get; set; }
        public string RoomName { get; set; }
        public string ServiceKey { get; set; }
        public string Alias { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string ConferenceProperty { get; set; }
        public int StatVideoType { get; set; }
        public ConferenceType ConfType { get; set; }
    }
}
