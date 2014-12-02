using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using Outlook = Microsoft.Office.Interop.Outlook;
using Office = Microsoft.Office.Core;

namespace Cosmoser.PingAnMeetingRequest.Outlook2007.Manager
{
    public class CalendarFolder
    {
        private CalendarDataManager _calendarManager = null;
        private AppointmentManager _appointmentManager = null;
        private Outlook.Items _appointmentItems;

        private static ILog logger = LogManager.GetLogger(typeof(CalendarFolder));

        private List<Outlook.AppointmentItem> _appointmentList = new List<Microsoft.Office.Interop.Outlook.AppointmentItem>();

        public CalendarFolder()
        {
            this._appointmentManager = new AppointmentManager();
            this._calendarManager = new CalendarDataManager();
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
        public List<Outlook.AppointmentItem> AppointmentList
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
                    this._appointmentList.Add(item);
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
            throw new NotImplementedException();
        }

        void Items_ItemChange(object Item)
        {
            throw new NotImplementedException();
        }

        void Items_ItemAdd(object Item)
        {
            throw new NotImplementedException();
        }
    }
}
