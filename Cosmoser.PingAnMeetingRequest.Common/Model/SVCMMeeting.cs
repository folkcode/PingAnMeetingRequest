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
        public int StatusCode { get; set; }
        public string Status
        {
            get
            {
                switch (this.StatusCode)
                {
                    case 0:
                        return "正在申请";
                    case 1:
                        return "预定成功";
                    case 2:
                        return "MCU正在处理";
                    case 3:
                        return "正在召开";
                    case 4:
                        return "会议结束";
                    case 6:
                        return "待审批";
                    case 7:
                        return "会议删除";
                    default:
                        return "未知";
                }
            }
        }
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
