using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Cosmoser.PingAnMeetingRequest.Common.Model;

namespace Cosmoser.PingAnMeetingRequest.Outlook2010.Views
{
    public class RoomScheduler
    {
        public string SeriesName { get; set; }
        public string RoomName { get; set; }
        public string Type { get; set; }

        public Dictionary<int, Color> TimeSheduler = new Dictionary<int, Color>();

        public static List<RoomScheduler> PopulateFromMeetingScheduler(List<MeetingScheduler> meetingSchedulerList, DateTime startTime, DateTime endTime)
        {
            var list = new List<RoomScheduler>();

            var roomnameList = meetingSchedulerList.Select(x => x.RoomName).Distinct();

            foreach (var item in roomnameList)
            {
                var roomList = meetingSchedulerList.FindAll(x => x.RoomName == item);
                RoomScheduler rScheduler = new RoomScheduler();
                rScheduler.RoomName = roomList[0].RoomName;
                rScheduler.SeriesName = roomList[0].SeriesName;
                rScheduler.Type = roomList[0].IfTerminal == 1 ? "视频" : "非视频";

                foreach (var room in roomList)
                {
                    int start = (room.StartTime - room.StartTime.Date).Minutes / 30;
                    int end = (room.EndTime - room.StartTime.Date).Minutes / 30 + 1;

                    for (int i = start; i <= end; i++)
                    {
                        Color c = Color.Blue;

                        switch(room.Status)
                        {
                            case 1:
                            case 2:
                            case 3:
                                c = Color.Red;
                                    break;
                            case 6:
                                c = Color.Yellow;
                                break;
                        }

                        if (!rScheduler.TimeSheduler.ContainsKey(i))
                            rScheduler.TimeSheduler.Add(i, c);
                    }
                }

                int s = (startTime - startTime.Date).Minutes / 30;
                int e = (endTime - startTime.Date).Minutes / 30 + 1;

                for (int i = s; i <= e; i++)
                {
                    if (!rScheduler.TimeSheduler.ContainsKey(i))
                        rScheduler.TimeSheduler.Add(i, Color.Blue);
                }
            }

            return list;
        }
    }
}
