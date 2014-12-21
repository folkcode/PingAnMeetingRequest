using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cosmoser.PingAnMeetingRequest.Common.Model
{
    public class MeetingSchedulerQuery
    {
        public string RoomName { get; set; }
        public string LevelId { get; set; }
        public string SeriesId { get; set; }
        /// <summary>
        /// 二级机构名称，0：全部
        /// </summary>
        public string ProvinceCode { get; set; }
        /// <summary>
        /// 三级机构，0：全部
        /// </summary>
        public string CityCode { get; set; }
        /// <summary>
        /// 四级机构，0：全部
        /// </summary>
        public string BoroughCode { get; set; }
        /// <summary>
        /// 会议室使用状态，1：全部，0：空闲
        /// </summary>
        public int BoardRoomState { get; set; }
        /// <summary>
        /// 会议室功能，2：全部，0：非视频，1：视频
        /// </summary>
        public int RoomIfTerminal { get; set; }
        /// <summary>
        ///  会议室容纳人数，空：全部，"0,10"：0< 人数 <=10，"10,25"：10< 人数 <=25，"25,40"：25< 人数 <=40，"40,0"：40< 人数
        /// </summary>
        public string Capacity { get; set; }

        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }
}
