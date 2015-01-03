using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cosmoser.PingAnMeetingRequest.Common.Model;
using Cosmoser.PingAnMeetingRequest.Common.Utilities;
using Outlook = Microsoft.Office.Interop.Outlook;
using System.Threading.Tasks;
using Cosmoser.PingAnMeetingRequest.Common.ClientService;
using log4net;
using System.Runtime.InteropServices;

namespace Cosmoser.PingAnMeetingRequest.Outlook2010.Manager
{
    public class CalendarDataManager
    {
        private CalendarFolder _calendarFolder;
        private static string path = "http://schemas.microsoft.com/mapi/string/{71227b02-8acf-4f1f-9a89-40fb98cfaa1c}/";
        private static string propertyKey = "PingAnMeeting";
        private MeetingDetailData _meetingDataLocal = new MeetingDetailData();
        private MeetingData _meetingListServer = new MeetingData();
        private AppointmentManager _appointmentManager;

        static ILog logger = IosLogManager.GetLogger(typeof(CalendarDataManager));

        public MeetingDetailData MeetingDetailDataLocal
        {
            get
            {
                return this._meetingDataLocal;
            }
        }

        public MeetingData MeetingDataServer
        {
            get
            {
                return _meetingListServer;
            }
        }

        public CalendarDataManager(CalendarFolder folder)
        {
            this._calendarFolder = folder;

            this._meetingDataLocal = this.GetMeetingDataFromLocal();

            this._appointmentManager = new AppointmentManager();
        }

        private MeetingDetailData GetMeetingDataFromLocal()
        {
            MeetingDetailData meetingData = null;
            try
            {
                try
                {
                    //string caledarDataString = (string)this._calendarFolder.MAPIFolder.PropertyAccessor.GetProperty(path + "PingAnMeeting");
                    Microsoft.Office.Interop.Outlook.StorageItem storage = this._calendarFolder.MAPIFolder.GetStorage(propertyKey, Microsoft.Office.Interop.Outlook.OlStorageIdentifierType.olIdentifyBySubject);
                    Microsoft.Office.Interop.Outlook.UserProperty pop = storage.UserProperties[propertyKey];
                    if (pop != null)
                    {
                        string caledarDataString = pop.Value;
                        meetingData = Toolbox.Deserialize<MeetingDetailData>(caledarDataString);

                    }
                    else
                    {
                        pop = storage.UserProperties.Add(propertyKey, Outlook.OlUserPropertyType.olText);
                        meetingData = new MeetingDetailData();
                    }
                }
                catch
                {
                    meetingData = new MeetingDetailData();
                    this._calendarFolder.MAPIFolder.PropertyAccessor.SetProperty(path + "PingAnMeeting", Toolbox.Serialize(meetingData));
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message + ex.StackTrace);
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
            try
            {
                string dataString = Toolbox.Serialize(this._meetingDataLocal);
                //this._calendarFolder.MAPIFolder.PropertyAccessor.DeleteProperty(path + "PingAnMeeting");
                //this._calendarFolder.MAPIFolder.PropertyAccessor.SetProperty(path + "PingAnMeeting", dataString);

                Microsoft.Office.Interop.Outlook.StorageItem storage = this._calendarFolder.MAPIFolder.GetStorage(propertyKey, Microsoft.Office.Interop.Outlook.OlStorageIdentifierType.olIdentifyBySubject);
                Microsoft.Office.Interop.Outlook.UserProperty pop = storage.UserProperties[propertyKey];
                if (pop == null)
                    pop = storage.UserProperties.Add(propertyKey, Outlook.OlUserPropertyType.olText);
                pop.Value = dataString;
                storage.Save();
                Marshal.ReleaseComObject(storage);
            }
            catch (System.Exception ex)
            {
                logger.Error("SavaMeetingDataToCalendarFolder error!", ex);
            }
        }

        public void SyncMeetingList()
        {
           
            try
            {
                if (OutlookFacade.Instance().Session.IsActive)
                {
                    Func<MeetingData, bool> func = LoadMeetingdataFromServer;
                    MeetingData meetingData = new MeetingData();
                    Task.Factory.FromAsync<MeetingData, bool>(func.BeginInvoke, func.EndInvoke, meetingData, null).ContinueWith((result) =>
                    {
                        bool succed = result.Result;
                        if (succed)
                        {
                            this._meetingListServer = meetingData;

                            foreach (var item in this._meetingListServer.Values)
                            {
                                if ((!this._calendarFolder.AppointmentCollection.ContainsKey(item.Id)))
                                {
                                    SVCMMeetingDetail detail = this.ConvertDetail(item);

                                    var appt = this._appointmentManager.AddAppointment(this._calendarFolder.MAPIFolder, detail);

                                    this._calendarFolder.AppointmentCollection.Add(item.Id, appt);

                                }

                                if (!this.MeetingDetailDataLocal.ContainsKey(item.Id))
                                {
                                    SVCMMeetingDetail detail = this.ConvertDetail(item);
                                    this.MeetingDetailDataLocal.Add(detail.Id, detail);
                                }
                            }
                            List<string> removeList = new List<string>();
                            foreach (var item in _calendarFolder.AppointmentCollection.Keys)
                            {
                                if (!this._meetingListServer.ContainsKey(item))
                                {
                                    logger.Debug(string.Format("MeetingId {0} is deleted from server, remove it from outlook.", item));
                                    var appt = this._calendarFolder.AppointmentCollection[item];

                                    if (appt.End > DateTime.Now)
                                    {
                                        appt.BeforeDelete -= new Outlook.ItemEvents_10_BeforeDeleteEventHandler(this._calendarFolder.item_BeforeDelete);
                                        appt.Delete();
                                        this._meetingDataLocal.Remove(item);
                                        removeList.Add(item);
                                    }
                                }
                            }

                            foreach (var item in removeList)
                            {
                                _calendarFolder.AppointmentCollection.Remove(item);
                            }

                            this.SavaMeetingDataToCalendarFolder();
                        }
                        else
                        {
                            logger.Error("同步会议列表信息错误！");
                        }
                    });
                }
                else
                {
                    logger.Debug("未登陆或session错误，没有执行同步！");
                }

            }
            catch (Exception ex)
            {
                logger.Error("同步失败！", ex);
            }

                

        }

        private SVCMMeetingDetail ConvertDetail(SVCMMeeting item)
        {
            var detail = new SVCMMeetingDetail();
            detail.Id = item.Id;
            detail.StartTime = item.StartTime;
            detail.EndTime = item.EndTime;
            detail.Name = item.Name;
            detail.Password = item.Password;

            return detail;
        }

        /// <summary>
        /// 获取一个异步同步会议列表任务
        /// </summary>
        /// <returns></returns>
        public Task<MeetingData> GetMeetingListSyncTask()
        {
            return Task<MeetingData>.Factory.StartNew(() =>
            {
                MeetingListQuery query = new MeetingListQuery();

                query.Alias = string.Empty;
                query.ConferenceProperty = string.Empty;
                query.ConfType = "-1";
                query.MeetingName = string.Empty;
                query.RoomName = string.Empty;
                query.ServiceKey = string.Empty;
                query.StartTime = DateTime.Now;
                query.EndTime = DateTime.Now.AddMonths(2);
                query.StatVideoType = -1;

                List<SVCMMeeting> list;
                MeetingData meetingData = new MeetingData();
                bool succeed = ClientServiceFactory.Create().TryGetMeetingList(query, OutlookFacade.Instance().Session, out list);

                if (succeed)
                {
                    foreach (var item in list)
                    {
                        meetingData.Add(item.Id, item);
                    }
                }

                return meetingData;
            });
        }

        /// <summary>
        /// 获取默认会议列表
        /// </summary>
        /// <returns></returns>
        public bool LoadMeetingdataFromServer(MeetingData meetingData)
        {
            MeetingListQuery query = new MeetingListQuery();

            query.Alias = string.Empty;
            query.ConferenceProperty = string.Empty;
            query.ConfType = "-1";
            query.MeetingName = string.Empty;
            query.RoomName = string.Empty;
            query.ServiceKey = string.Empty;
            query.StartTime = DateTime.Now;
            query.EndTime = DateTime.Now.AddMonths(2);
            query.StatVideoType = -1;

            List<SVCMMeeting> list;
            
            bool succeed = ClientServiceFactory.Create().TryGetMeetingList(query, OutlookFacade.Instance().Session, out list);

            if (succeed)
            {
                foreach (var item in list)
                {
                    meetingData.Add(item.Id, item);
                }

                return true;
            }

            return false;
        }
            
    }
}
