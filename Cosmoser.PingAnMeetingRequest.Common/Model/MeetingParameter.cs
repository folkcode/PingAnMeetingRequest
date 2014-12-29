using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cosmoser.PingAnMeetingRequest.Common.Model
{
    public enum ConferenceType
    {
        Immediate = 1,
        Furture = 2,
        Recurring = 3
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
        Audio = 0,
        MainRoom = 1,        
        EqualScreen = 2,
        OneNScreen = 3,
        TwoNScreen = 4
    }

}
