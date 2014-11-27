using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cosmoser.PingAnMeetingRequest.Common.Model
{
    public class MeetingRoom
    {
        public RoomCategory Category { get; set; }
        public RoomLevel Level { get; set; }
        public string Name { get; set; }
    }
}
