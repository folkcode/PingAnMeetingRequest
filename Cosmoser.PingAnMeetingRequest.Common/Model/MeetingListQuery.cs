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
        /// <summary>
        ///会议性质，''：全部，2：非执委会议，1：执委会议 
        /// </summary>
        public string ConferenceProperty { get; set; }
        /// <summary>
        /// 会议类型，-1：全部，4：本地会议，1：两方视频会议，2：多方视频会议 
        /// </summary>
        public int StatVideoType { get; set; }
        /// <summary>
        /// 视频\本地，-1：全部，1：视频，2：本地 
        /// </summary>
        public string ConfType { get; set; }
    }
}
