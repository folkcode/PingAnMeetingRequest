using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cosmoser.PingAnMeetingRequest.Common.Model
{
    public class MeetingScheduler
    {
        public string RoomId { get; set; }
        public string RoomName { get; set; }
        public string SeriesName { get; set; }
        /// <summary>
        /// 视频属性，1：视频，3：视频故障，其他：非视频
        /// </summary>
        public int IfTerminal { get; set; }
        /// <summary>
        /// 是否需审批，2：需要审批，其他：不需审批
        /// </summary>
        public string Property { get; set; }

        public string Address { get; set; }
        public int MeetingId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        /// <summary>
        /// 会议状态 0：正在申请；1：预定成功；2：MCU正在处理中；3：正在进行；4：会议结束；6：待审批；7：会议被删除
        /// </summary>
        public int Status { get; set; }

        public string ConferId { get; set; }

        /// <summary>
        /// 0  待审批， 1 审批通过
        /// </summary>
        public int ApproveStatus { get; set; }

    }
}
