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
        }

        public static MeetingData GetMeetingDataFromLocal(Outlook.MAPIFolder calendarFolder)
        {
            MeetingData meetingData = null;
            try
            {
                string caledarDataString = (string)calendarFolder.PropertyAccessor.GetProperty(path + "PingAnMeeting");
                meetingData = Toolbox.Deserialize<MeetingData>(caledarDataString);
            }
            catch
            {
                meetingData = new MeetingData();
                calendarFolder.PropertyAccessor.SetProperty(path + "PingAnMeeting", Toolbox.Serialize(meetingData));
            }

            return meetingData;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="calendarFolder"></param>
        /// <param name="meetingData"></param>
        private static void SavaMeetingDataToCalendarFolder(Outlook.MAPIFolder calendarFolder, MeetingData meetingData)
        {
            string dataString = Toolbox.Serialize(meetingData);
            calendarFolder.PropertyAccessor.SetProperty(path + "PingAnMeeting", dataString);
        }

    
    }
}
