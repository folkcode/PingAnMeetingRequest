using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using Outlook = Microsoft.Office.Interop.Outlook;
using Office = Microsoft.Office.Core;
using Cosmoser.PingAnMeetingRequest.Common.Model;

namespace Cosmoser.PingAnMeetingRequest.Outlook2007.Manager
{
    public class CalendarFolder
    {
        private CalendarDataManager _calendarManager = null;
        private AppointmentManager _appointmentManager = null;
        private Outlook.Items _appointmentItems;

        private static ILog logger = LogManager.GetLogger(typeof(CalendarFolder));

        private Dictionary<string, Outlook.AppointmentItem> _appointmentList = new Dictionary<string, Outlook.AppointmentItem>();

        public CalendarFolder()
        {
            this._appointmentManager = new AppointmentManager();
            this._calendarManager = new CalendarDataManager(this);
        }

        /// <summary>
        /// Get the MAPIFolder of calendar. If not existed, we need to recreate it.
        /// </summary>
        public override Microsoft.Office.Interop.Outlook.MAPIFolder MAPIFolder
        {
            get
            {
                return Globals.ThisAddIn.Application.Session.Folders["Calendar"];
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
            Outlook.MAPIFolder calendarFolder = this.MAPIFolder;

            this._appointmentItems = this.MAPIFolder.Items;
            calendarFolder.Items.ItemAdd += new Outlook.ItemsEvents_ItemAddEventHandler(Items_ItemAdd);
            calendarFolder.Items.ItemChange += new Outlook.ItemsEvents_ItemChangeEventHandler(Items_ItemChange);
            calendarFolder.Items.ItemRemove += new Outlook.ItemsEvents_ItemRemoveEventHandler(Items_ItemRemove);

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
                    string id = this._appointmentManager.GetMeetingIdFromAppointment(item);
                    this._appointmentList.Add(id,item);
                    item.BeforeDelete += new Outlook.ItemEvents_10_BeforeDeleteEventHandler(item_BeforeDelete);
                }
            }      
        }

        void item_BeforeDelete(object Item, ref bool Cancel)
        {
            //throw new NotImplementedException();
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
                SVCMMeeting meeting = this._appointmentManager.GetMeetingFromAppointment(appt);
                if (this._calendarManager.MeetingDataLocal.ContainsKey(meeting.Id))
                {
                    this._calendarManager.MeetingDataLocal.Remove(meeting.Id);
                }

                this._calendarManager.MeetingDataLocal.Add(meeting.Id, meeting);
                CalendarDataManager.SavaMeetingDataToCalendarFolder(this.MAPIFolder, this.CalendarDataManager.MeetingDataLocal);
            }
        }

        void Items_ItemAdd(object Item)
        {
            Outlook.AppointmentItem appt = Item as Outlook.AppointmentItem;
            if (IsPingAnMeetingAppointment(appt ))
            {
                SVCMMeeting meeting = this._appointmentManager.GetMeetingFromAppointment(appt);
                if (this._calendarManager.MeetingDataLocal.ContainsKey(meeting.Id))
                {
                    this._calendarManager.MeetingDataLocal.Remove(meeting.Id);
                }

                this._calendarManager.MeetingDataLocal.Add(meeting.Id, meeting);
                CalendarDataManager.SavaMeetingDataToCalendarFolder(this.MAPIFolder, this.CalendarDataManager.MeetingDataLocal);
            }
        }
    }
}
