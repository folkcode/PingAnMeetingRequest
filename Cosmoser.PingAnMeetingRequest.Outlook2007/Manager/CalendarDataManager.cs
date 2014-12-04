using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cosmoser.PingAnMeetingRequest.Common.Model;
using Cosmoser.PingAnMeetingRequest.Common.Utilities;
using Outlook = Microsoft.Office.Interop.Outlook;

namespace Cosmoser.PingAnMeetingRequest.Outlook2007.Manager
{
    public class CalendarDataManager
    {
        private CalendarFolder _calendarFolder;
        private static string path = "http://schemas.microsoft.com/mapi/string/{71227b02-8acf-4f1f-9a89-40fb98cfaa1c}/";
        private MeetingData _meetingDataLocal = new MeetingData();
        private MeetingData MeetingListServer;

        public MeetingData MeetingDataLocal
        {
            get
            {
                return this._meetingDataLocal;
            }
        }

        public CalendarDataManager(CalendarFolder folder)
        {
            this._calendarFolder = folder;

            this._meetingDataLocal = this.GetMeetingDataFromLocal();
        }

        private MeetingData GetMeetingDataFromLocal()
        {
            MeetingData meetingData = null;
            try
            {
                string caledarDataString = (string)this._calendarFolder.MAPIFolder.PropertyAccessor.GetProperty(path + "PingAnMeeting");
                meetingData = Toolbox.Deserialize<MeetingData>(caledarDataString);
            }
            catch
            {
                meetingData = new MeetingData();
                this._calendarFolder.MAPIFolder.PropertyAccessor.SetProperty(path + "PingAnMeeting", Toolbox.Serialize(meetingData));
            }

            return meetingData;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="calendarFolder"></param>
        /// <param name="meetingData"></param>
        public void SavaMeetingDataToCalendarFolder()
        {
            string dataString = Toolbox.Serialize(this._meetingDataLocal);
            this._calendarFolder.MAPIFolder.PropertyAccessor.SetProperty(path + "PingAnMeeting", dataString);
        }

    
    }
}
