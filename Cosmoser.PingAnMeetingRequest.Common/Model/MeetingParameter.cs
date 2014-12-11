using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cosmoser.PingAnMeetingRequest.Common.Model
{
    public enum ConferenceType
    {
        Immediate = 1,
        Furture
    }

    public enum MideaType
    {
        Video = 1,
        Local
    }

    public enum VideoSet
    {
        Audio = 1,
        MainRoom,
        EqualScreen,
        OneNScreen,
        TwoNScreen
    }

}
