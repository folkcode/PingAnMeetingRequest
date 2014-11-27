using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cosmoser.PingAnMeetingRequest.Common.Model
{
    public class SvcmUser
    {
        public string Username { get; set; }
        public string DisplayName { get; set; }
        public UserType Role { get; set; }
    }
}
