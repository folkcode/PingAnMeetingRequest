using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cosmoser.PingAnMeetingRequest.Common.Model
{
    public enum ConferenceType
    {
        Immediate = 1,
        Furture,
        Recurring
    }

    public enum MideaType
    {
        Video = 1,
        Local
    }

    public enum MideaType2
    {
        Two = 1,
        Multiple,
        Midea,
        Local
    }

    public enum VideoSet
    {
        Audio = 1,
        MainRoom = 0,
        EqualScreen = 3,
        OneNScreen = 4,
        TwoNScreen = 5
    }

}
