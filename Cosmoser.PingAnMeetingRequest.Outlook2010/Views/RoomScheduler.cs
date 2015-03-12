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

        public static List<RoomScheduler> PopulateFromMeetingScheduler(List<MeetingScheduler> meetingSchedulerList, DateTime startTime, DateTime endTime, bool isAll)
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

                if (roomList.Count == 1 && string.IsNullOrEmpty(roomList[0].ConferId))
                {
                    int e = (int)(endTime - startTime).TotalMinutes / 30;

                    for (int i = 0; i < e; i++)
                    {
                        if (!rScheduler.TimeSheduler.ContainsKey(i))
                            rScheduler.TimeSheduler.Add(i, Color.FromArgb(0xae, 0xbb, 0x66));
                    }

                    list.Add(rScheduler);
                }
                else
                {
                    if (isAll)
                    {
                        foreach (var room in roomList)
                        {
                            int start = (int)(room.StartTime - startTime).TotalMinutes / 30;
                            int end = (int)(room.EndTime - startTime).TotalMinutes / 30;

                            for (int i = start; i < end; i++)
                            {
                                Color c = Color.Blue;

                                //switch (room.Status)
                                //{
                                //    case 1:
                                //    case 2:
                                //    case 3:
                                //        c = Color.FromArgb(0xff, 0x79, 0x00);
                                //        break;
                                //    case 6:
                                //        c = Color.FromArgb(0xff, 0xff, 0x00);
                                //        break;
                                //}

                                switch (room.ApproveStatus)
                                {
                                    case 0:
                                        c = Color.FromArgb(0xff, 0xff, 0x00);
                                        break;
                                    case 1:
                                        c = Color.FromArgb(0xff, 0x79, 0x00);
                                        break;
                                }

                                if (!rScheduler.TimeSheduler.ContainsKey(i))
                                    rScheduler.TimeSheduler.Add(i, c);
                            }
                        }

                        //int s = (startTime - startTime.Date).Minutes / 30;
                        int e = (int)(endTime - startTime).TotalMinutes / 30;

                        for (int i = 0; i < e; i++)
                        {
                            if (!rScheduler.TimeSheduler.ContainsKey(i))
                                rScheduler.TimeSheduler.Add(i, Color.FromArgb(0xae, 0xbb, 0x66));
                        }

                        list.Add(rScheduler);
                    }
                }

                

            }

            return list;
        }
    }
}
