using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using Outlook = Microsoft.Office.Interop.Outlook;
using Office = Microsoft.Office.Core;
using Cosmoser.PingAnMeetingRequest.Common.Model;
using Cosmoser.PingAnMeetingRequest.Common.ClientService;
using Cosmoser.PingAnMeetingRequest.Common.Utilities;

namespace Cosmoser.PingAnMeetingRequest.Outlook2010.Manager
{
    public class CalendarFolder
    {
        private CalendarDataManager _calendarManager = null;
        private AppointmentManager _appointmentManager = null;
        private Outlook.Items _appointmentItems;
        Microsoft.Office.Interop.Outlook.MAPIFolder _mapiFolder;

        private static ILog logger = IosLogManager.GetLogger(typeof(CalendarFolder));

        private Dictionary<string, Outlook.AppointmentItem> _appointmentList = new Dictionary<string, Outlook.AppointmentItem>();

        public CalendarFolder()
        {
            this._appointmentManager = new AppointmentManager();
        }

        /// <summary>
        /// Get the MAPIFolder of calendar. If not existed, we need to recreate it.
        /// </summary>
        public Microsoft.Office.Interop.Outlook.MAPIFolder MAPIFolder
        {
            get
            {
                return this._mapiFolder;// Globals.ThisAddIn.Application.ActiveExplorer().CurrentFolder.Folders["日历"];
            }
        }

        public CalendarDataManager CalendarDataManager
        {
            get
            {
                return this._calendarManager;
            }
        }

        /// <summary>
        /// Keep all the Bpm appointments to avoid losing the item events.
        /// </summary>
        public Dictionary<string,Outlook.AppointmentItem> AppointmentCollection
        {
            get
            {
                return this._appointmentList;
            }
        }

        public void Initialize()
        {
            logger.Debug("GetRootFolder: " + Globals.ThisAddIn.Application.Session.DefaultStore.GetRootFolder().Name);
            foreach (Outlook.Folder folder in Globals.ThisAddIn.Application.Session.DefaultStore.GetRootFolder().Folders)
            {
                if (folder.DefaultItemType == Outlook.OlItemType.olAppointmentItem || folder.Name == "日历" || folder.Name == "Calendar")
                {
                    this._mapiFolder = folder;
                    logger.Debug("Calendar Folder catched!");
                }
            }

            if (this._mapiFolder == null)
            {
                logger.Error("没有找到日历目录!");
            }

            this._calendarManager = new CalendarDataManager(this);

            this._appointmentItems = this._mapiFolder.Items;
            _appointmentItems.ItemAdd += new Outlook.ItemsEvents_ItemAddEventHandler(Items_ItemAdd);
            _appointmentItems.ItemChange += new Outlook.ItemsEvents_ItemChangeEventHandler(Items_ItemChange);
            _appointmentItems.ItemRemove += new Outlook.ItemsEvents_ItemRemoveEventHandler(Items_ItemRemove);

            this.MAPIFolder.Application.ItemContextMenuDisplay += new Outlook.ApplicationEvents_11_ItemContextMenuDisplayEventHandler(Application_ItemContextMenuDisplay);
            this.RegisterAppointmentItemEvents();
        }

        private void RegisterAppointmentItemEvents()
        {
            this._appointmentList.Clear();
            foreach (Outlook.AppointmentItem item in this._appointmentItems)
            {
                if (IsPingAnMeetingAppointment(item))
                {
                    SVCMMeetingDetail meeting = this._appointmentManager.GetMeetingFromAppointment(item,false);
                    if (meeting != null)
                        this._appointmentList.Add(meeting.Id, item);
                    item.BeforeDelete += new Outlook.ItemEvents_10_BeforeDeleteEventHandler(item_BeforeDelete);
                }
            }      
        }

        void item_BeforeDelete(object Item, ref bool Cancel)
        {
            Outlook.AppointmentItem appt = Item as Outlook.AppointmentItem;
            if (IsPingAnMeetingAppointment(appt))
            {
                SVCMMeetingDetail meeting = this._appointmentManager.GetMeetingFromAppointment(appt,false);
                bool suceed = ClientServiceFactory.Create().DeleteMeeting(meeting.Id, OutlookFacade.Instance().Session);

                if (suceed)
                {
                    this._calendarManager.MeetingDetailDataLocal.Remove(meeting.Id);
                    this._calendarManager.SavaMeetingDataToCalendarFolder();
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show("删除会议失败，请重试！");
                    this._appointmentManager.RemoveItemDeleteStatus(appt);
                    Cancel = true;
                }
            }
        }

        private bool IsPingAnMeetingAppointment(Outlook.AppointmentItem item)
        {
            if (item.MessageClass == "IPM.Appointment.PingAnMeetingRequest")
            {
                return true;
            }

            return false;
        }

        void Application_ItemContextMenuDisplay(Microsoft.Office.Core.CommandBar CommandBar, Outlook.Selection Selection)
        {
           
        }

        void Items_ItemRemove()
        {
            
        }

        void Items_ItemChange(object Item)
        {
            Outlook.AppointmentItem appt = Item as Outlook.AppointmentItem;
            if (IsPingAnMeetingAppointment(appt))
            {
                SVCMMeetingDetail meeting = this._appointmentManager.GetMeetingFromAppointment(appt,false);
                if (this._calendarManager.MeetingDetailDataLocal.ContainsKey(meeting.Id))
                {
                    this._calendarManager.MeetingDetailDataLocal.Remove(meeting.Id);
                }

                this._calendarManager.MeetingDetailDataLocal.Add(meeting.Id, meeting);
                this._calendarManager.SavaMeetingDataToCalendarFolder();
            }
        }

        void Items_ItemAdd(object Item)
        {
            Outlook.AppointmentItem appt = Item as Outlook.AppointmentItem;
            if (IsPingAnMeetingAppointment(appt ))
            {
                SVCMMeetingDetail meeting = this._appointmentManager.GetMeetingFromAppointment(appt,false);
                if (this._calendarManager.MeetingDetailDataLocal.ContainsKey(meeting.Id))
                {
                    this._calendarManager.MeetingDetailDataLocal.Remove(meeting.Id);
                }

                this._calendarManager.MeetingDetailDataLocal.Add(meeting.Id, meeting);
                this._calendarManager.SavaMeetingDataToCalendarFolder();

                if (!this._appointmentList.ContainsKey(meeting.Id))
                    this._appointmentList.Add(meeting.Id, appt);

                appt.BeforeDelete += new Outlook.ItemEvents_10_BeforeDeleteEventHandler(item_BeforeDelete);
            }
        }

        internal void AddAppointment(SVCMMeeting item)
        {
            throw new NotImplementedException();
        }
    }
}
