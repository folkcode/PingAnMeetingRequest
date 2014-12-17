using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cosmoser.PingAnMeetingRequest.Common.Model
{
    public class MobileTerm
    {
        public string RoomId { get; set; }
        public string RoomName { get; set; }

        public override string ToString()
        {
            return RoomName;
        }

        public override bool Equals(object obj)
        {
            MobileTerm term = obj as MobileTerm;
            if (term != null)
                return this.RoomId == term.RoomId;
            else
                return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
