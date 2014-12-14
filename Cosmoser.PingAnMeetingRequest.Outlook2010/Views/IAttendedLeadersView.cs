using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cosmoser.PingAnMeetingRequest.Common.Model;
using System.Windows.Forms;

namespace Cosmoser.PingAnMeetingRequest.Outlook2010.Views
{
    public interface IAttendedLeadersView
    {
        List<MeetingLeader> LeaderList { get; set; }
        string LeaderRoom { get; set; }

        DialogResult Display();
    }
}
