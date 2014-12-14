using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cosmoser.PingAnMeetingRequest.Common.Interfaces;

namespace Cosmoser.PingAnMeetingRequest.Common.ClientService
{
    public static class ClientServiceFactory
    {
        private static IConferenceHandler handler;
        private static object locker = new object();
        public static IConferenceHandler Create()
        {
            lock (locker)
            {
                if (handler == null)
                    handler = new RestXmlClientService();
                return handler;
            }
        }
    }
}
