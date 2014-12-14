using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cosmoser.PingAnMeetingRequest.Common.Model;
using System.Windows.Forms;

namespace Cosmoser.PingAnMeetingRequest.Outlook2010.Views
{
    public interface IMeetingRoomView
    {
        List<MeetingRoom> MeetingRoomList { get; set; }
        MeetingRoom MainRoom { get; set; }

        DialogResult Display();
    }
}
