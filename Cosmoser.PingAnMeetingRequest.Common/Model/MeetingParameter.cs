using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cosmoser.PingAnMeetingRequest.Common.Model
{
    public enum MeetingParameter
    {
        Immediate,
        Furture
    }

    public enum MeetingType
    {
        Local,
        Video
    }

    public enum DisplayMode
    {
        Audio,
        MainRoom,
        EqualScreen,
        OneNScreen,
        TwoNScreen
    }
}
