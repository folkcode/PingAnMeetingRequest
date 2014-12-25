using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Outlook = Microsoft.Office.Interop.Outlook;
using Cosmoser.PingAnMeetingRequest.Common.Model;
using Cosmoser.PingAnMeetingRequest.Common.Utilities;
using log4net;

namespace Cosmoser.PingAnMeetingRequest.Outlook2010.Manager
{
    public class AppointmentManager
    {
        private static string path = "http://schemas.microsoft.com/mapi/string/{71227b02-8acf-4f1f-9a89-40fb98cfaa1c}/";
        private static ILog logger = IosLogManager.GetLogger(typeof(AppointmentManager));

        public string GetMeetingIdFromAppointment(Outlook.AppointmentItem item)
        {
            string meetingId = null;
            try
            {
                meetingId = item.PropertyAccessor.GetProperty(path + "PingAnMeetingId");
            }
            catch { }

            SVCMMeetingDetail detail = this.GetMeetingFromAppointment(item, false);

            if (!string.IsNullOrEmpty(meetingId))
                return meetingId;
            else if (detail != null && string.IsNullOrEmpty(detail.Id))
                return detail.Id;
            else
                return null;
        }

        public void SaveMeetingToAppointment(SVCMMeetingDetail meeting, Outlook.AppointmentItem item, bool isUpdating)
        {
            if (isUpdating)
                item.PropertyAccessor.SetProperty(path + "PingAnMeetingUpdating", Toolbox.Serialize(meeting));
            else
                item.PropertyAccessor.SetProperty(path + "PingAnMeeting", Toolbox.Serialize(meeting));
            
            if (!string.IsNullOrEmpty(meeting.Id))
                item.PropertyAccessor.SetProperty(path + "PingAnMeetingId", meeting.Id);
        }

        public SVCMMeetingDetail GetMeetingFromAppointment(Outlook.AppointmentItem item, bool isUpdating)
        {
            try
            {
                if (isUpdating)
                    return Toolbox.Deserialize<SVCMMeetingDetail>(item.PropertyAccessor.GetProperty(path + "PingAnMeetingUpdating"));
                else
                    return Toolbox.Deserialize<SVCMMeetingDetail>(item.PropertyAccessor.GetProperty(path + "PingAnMeeting"));
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

        public void RemoveItemDeleteStatus(Outlook.AppointmentItem item)
        {
            try
            {
                item.PropertyAccessor.DeleteProperty(path + "IsDeleted");
            }
            catch
            {
                //it is a new appointment
            }
        }

        internal bool TryValidateApppointmentUIInput(Outlook.AppointmentItem item, out string message)
        {
            StringBuilder sb = new StringBuilder();

            var meeting = this.GetMeetingFromAppointment(item, true);

            if (meeting.MainRoom == null || string.IsNullOrEmpty(meeting.MainRoom.RoomId))
                sb.AppendLine("请设定一个主会场！");

            if (string.IsNullOrEmpty(meeting.Name))
                sb.AppendLine("请填写会议名称！");

            message = sb.ToString();

            if (sb.Length > 0)
                return false;
            return true;
        }

        internal Outlook.AppointmentItem AddAppointment(Outlook.MAPIFolder mAPIFolder, SVCMMeetingDetail detail)
        {
            Outlook.AppointmentItem item = mAPIFolder.Application.CreateItem(Outlook.OlItemType.olAppointmentItem);
            item.Subject = detail.Name;
            item.Start = detail.StartTime;
            item.End = detail.EndTime;
            item.MessageClass = "IPM.Appointment.PingAnMeetingRequest";

            this.SaveMeetingToAppointment(detail, item, false);
            item.Save();

            return item;
        }
    }
}
