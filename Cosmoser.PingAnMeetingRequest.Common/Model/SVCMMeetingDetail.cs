using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cosmoser.PingAnMeetingRequest.Common.Model
{
    public class SVCMMeetingDetail
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        public int DurantionHours
        {
            get
            {
                return (EndTime - StartTime).Hours;
            }
        }

        public int DurantionMinutes
        {
            get
            {
                return (EndTime - StartTime).Minutes;
            }
        }
        public string Password { get; set; }
        public string Memo { get; set; }
        public MeetingRoom MainRoom { get; set; }
        public List<MobileTerm> MobileTermList { get; set; }
        public VideoSet VideoSet { get; set; }
        public int ParticipatorNumber { get; set; }
        public string Phone { get; set; }
        public string IPDesc { get; set; }
        //召集人姓名
        public string AccountName { get; set; }
        public string Status { get; set; }
        //点对点会议是否上MCU，0：不上MCU，1：上MCU，平安业务新增字段, 保留，默认0
        public int InMCU { get; set; }
        public List<MeetingLeader> LeaderList { get; set; }
        public string LeaderRoom { get; set; }
        public ConferenceType ConfType { get; set; }
        public MideaType ConfMideaType { get; set; }
        public List<MeetingRoom> Rooms { get; set; }

        public string LeaderListStr
        {

            get
            {

                return string.Join(",", this.LeaderList.Select(x => x.UserName).ToList());
            }
        }

        public string RoomsStr
        {
            get
            {
                StringBuilder sb = new StringBuilder(this.MainRoom.Name + "(主会场)");
                foreach (var item in this.Rooms)
                {
                    sb.Append("," + item.Name);
                }

                return sb.ToString();
            }
        }

        public string RoomIds
        {
            get
            {
                StringBuilder sb = new StringBuilder();

                foreach (var item in this.Rooms)
                {
                    sb.Append("," + item.RoomId.Split(",".ToArray())[0]);
                }

                foreach (var item in this.MobileTermList)
                {
                    sb.Append("," + item.RoomId);
                }

                return sb.Remove(0, 1).ToString();
            }
        }
        //周期性会议类型，1，日例会，2，周例会，3，月例会
        public int RegularMeetingType { get; set; }

        //每种例会类型允许的最大周期范围，以月为单位
        public int RegularMaxNum { get; set; }

        //例会总数
        public int RegularMeetingNum { get; set; }

        //日例会, 除了星期日=1,除了星期一=2，以此类推，多个以逗号分隔
        public string MultiExceptDay { get; set; }

        //周例会,星期日=1，星期一=2，以此类推，多个以逗号分隔
        public string MultiExceptWeek { get; set; }

        //每X个月的
        public int EveryFewMonths { get; set; }

        //第一个=1，第二个=2，第三个=3，第四个=4，最后一个=5-
        public int TheFirstFew { get; set; }

        //星期日=1，星期一=2，星期二=3，以此类推，星期六=7
        public int Week { get; set; }

        //呼入号
        public string ServiceKey { get; set; }

        public MeetingSeries Series { get; set; }

        public SVCMMeetingDetail()
        {
            VideoSet = Model.VideoSet.Audio;
            this.Status = "";
            ConfType = ConferenceType.Immediate;
            ConfMideaType = MideaType.Local;
            Id = string.Empty;
            Series = new MeetingSeries();
            LeaderList = new List<MeetingLeader>();
            MobileTermList = new List<MobileTerm>();
            Rooms = new List<MeetingRoom>();
        }
    }
}
