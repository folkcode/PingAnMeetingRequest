using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Outlook = Microsoft.Office.Interop.Outlook;
using Cosmoser.PingAnMeetingRequest.Common.Model;
using Cosmoser.PingAnMeetingRequest.Common.Utilities;

namespace Cosmoser.PingAnMeetingRequest.Outlook2007.Manager
{
    public class AppointmentManager
    {
        private static string path = "http://schemas.microsoft.com/mapi/string/{71227b02-8acf-4f1f-9a89-40fb98cfaa1c}/";

        public string GetMeetingIdFromAppointment(Outlook.AppointmentItem item)
        {
            try
            {
                return item.PropertyAccessor.GetProperty(path + "PingAnMeetingId");
            }
            catch { }
            return null;
        }

        public void SaveMeetingToAppointment(SVCMMeeting meeting, Outlook.AppointmentItem item)
        {
            item.PropertyAccessor.SetProperty(path + "PingAnMeeting", Toolbox.Serialize(meeting));
            item.PropertyAccessor.SetProperty(path + "PingAnMeetingId", meeting.Id);
        }

        public SVCMMeeting GetMeetingFromAppointment(Outlook.AppointmentItem item)
        {
            try
            {
                return Toolbox.Deserialize<SVCMMeeting>(item.PropertyAccessor.GetProperty(path + "PingAnMeeting"));
            }
            catch
            {
                //it is a new appointment
            }

            return null;
        }

        public void SetAppointmentDeleted(Outlook.AppointmentItem item, bool isDeleted)
        {
            item.PropertyAccessor.SetProperty(path + "IsDeleted", isDeleted);
        }

        public bool IsAppointmentStatusDeleted(Outlook.AppointmentItem item)
        {
            try
            {
                return (bool)item.PropertyAccessor.GetProperty(path + "IsDeleted");
            }
            catch
            {
                //it is a new appointment
            }

            return false;
        }

        internal bool TryValidateApppointmentUIInput(Outlook.AppointmentItem item, out string message)
        {
            var meeting = Toolbox.Deserialize<SVCMMeeting>( item.PropertyAccessor.GetProperty(path + "PingAnMeeting"));
            message = "error";
            return true;
        }
    }
}
