using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cosmoser.PingAnMeetingRequest.Common.Model
{
    public class MeetingRoomListQuery
    {
        public string SeriesId { get; set; }
        public string LevelId { get; set; }
        public MideaType ConfType { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }
}
