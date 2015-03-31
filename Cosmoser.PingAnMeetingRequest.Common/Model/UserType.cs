using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cosmoser.PingAnMeetingRequest.Common.Model
{
    public enum UserType
    {
    }

    public class UserRole
    {
        //系统管理员参数，1：是，0：否
        public bool IsSysManager { get; set; }
        //机构系统管理员参数，1：是，0：否
        public bool IsSysManager1 { get; set; }
        //会议室管理员参数，1：是，0：否
        public bool IsBoardroomAdmin { get; set; }
        //审批管理员参数，1：是，0：否
        public bool IsApproveAdmin { get; set; }
        //会议控制管理员参数，1：是，0：否
        public bool IsConfControlAdmin { get; set; }
    }
}
