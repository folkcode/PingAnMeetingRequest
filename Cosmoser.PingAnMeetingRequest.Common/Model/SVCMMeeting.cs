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
        public string Status { get; set; }
        public int Type { get; set; }
        public string MainRoom { get; set; }
        //呼入号
        public string ServiceKey { get; set; }
        public string Password { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        public string MideaTypeStr
        {
            get
            {
                switch (this.Type)
                {
                    case 1:
                        return "两方会议";
                    case 2:
                        return "多方会议";
                    case 3:
                        return "多媒体会议";
                    case 4:
                        return "本地会议";
                }

                return string.Empty;
            }
        }
    }
}
