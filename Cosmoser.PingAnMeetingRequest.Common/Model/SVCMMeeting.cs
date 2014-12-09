using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cosmoser.PingAnMeetingRequest.Common.Model
{
    public class SVCMMeeting
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string AccountName { get; set; }
        public MeetingStatus Status { get; set; }
        bool IsManualConf { get; set; }
        public MideaType Type { get; set; }
        public List<MeetingRoom> Rooms { get; set; }
        public MeetingRoom MainRoom { get; set; }
        //呼入号
        public string ServiceKey { get; set; }
        public string Password { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public ConferenceType Parameter { get; set; }
        
    }
}
