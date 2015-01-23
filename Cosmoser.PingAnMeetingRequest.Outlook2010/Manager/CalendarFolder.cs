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
using System.Windows.Forms;
using Cosmoser.PingAnMeetingRequest.Outlook2010.Views;

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
                    break;
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
            try
            {
                logger.Debug("RegisterAppointmentItemEvents");
                this._appointmentList.Clear();
                foreach (Outlook.AppointmentItem item in this._appointmentItems)
                {
                    if (IsPingAnMeetingAppointment(item))
                    {
                        SVCMMeetingDetail meeting = this._appointmentManager.GetMeetingFromAppointment(item, false);
                        if (meeting != null)
                        {
                            if (!string.IsNullOrWhiteSpace(meeting.Id) && this.CalendarDataManager.MeetingDetailDataLocal.ContainsKey(meeting.Id))
                            {
                                this._appointmentManager.SaveMeetingToAppointment(this.CalendarDataManager.MeetingDetailDataLocal[meeting.Id],item, false);
                            }

                            if (!this._appointmentList.ContainsKey(meeting.Id))
                            {
                                this._appointmentList.Add(meeting.Id, item);
                                item.BeforeDelete += new Outlook.ItemEvents_10_BeforeDeleteEventHandler(item_BeforeDelete);
                            }
                            else
                            {
                                item.Delete();
                            }
                        }
                        else
                        {
                            //无效appointment，删除
                            item.Delete();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message + ex.StackTrace);
            }
        }

        public void item_BeforeDelete(object Item, ref bool Cancel)
        {
            try
            {
                logger.Debug("item_BeforeDelete start!");
                Outlook.AppointmentItem appt = Item as Outlook.AppointmentItem;

                if (IsPingAnMeetingAppointment(appt))
                {
                    if (_appointmentManager.IsAppointmentStatusDeleted(appt))
                        return;

                    // add by robin at 20141231 start 
                    if ( MessageBox.Show("你确定要删除该会议?", "提示信息", MessageBoxButtons.YesNo) != DialogResult.Yes)
                    {
                        //this._appointmentManager.RemoveItemDeleteStatus(appt);
                        Cancel = true;
                        return;
                    }
                    // add by robin at 20141231 end 
                    string  error;
                    SVCMMeetingDetail meeting = this._appointmentManager.GetMeetingFromAppointment(appt, false);
                    if (this._calendarManager.MeetingDetailDataLocal.ContainsKey(meeting.Id))
                    {
                        bool suceed = ClientServiceFactory.Create().DeleteMeeting(meeting.Id, OutlookFacade.Instance().Session, out error);

                        if (suceed)
                        {
                            this._calendarManager.MeetingDetailDataLocal.Remove(meeting.Id);
                            this._calendarManager.SavaMeetingDataToCalendarFolder();
                            if (this._appointmentList.ContainsKey(meeting.Id))
                                this._appointmentList.Remove(meeting.Id);
                        }
                        else
                        {
                            // modify by robin at 20141231 start 
                            //System.Windows.Forms.MessageBox.Show("删除会议失败，请重试！");
                            //this._appointmentManager.RemoveItemDeleteStatus(appt);
                            //Cancel = true;
                            //System.Windows.Forms.MessageBox.Show("删除会议失败，请重试！");
                             System.Windows.Forms.MessageBox.Show(string.Format("向服务端删除会议失败！{0}！ 请重试。", error));
                            //this._appointmentManager.RemoveItemDeleteStatus(appt);
                            Cancel = true;
                            // modify by robin at 20141231 end 
                        }
                    }
                    else
                    {
                        logger.Debug(string.Format("item_BeforeDelete: meeting Id {0}, meetingName {1}. The meeting is not existing in server, no need update to server,only delete from outlook", meeting.Id, meeting.Name));
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error("item_BeforeDelete error!", ex);
                MessageBox.Show(ex.Message);
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
            try
            {
                this._calendarManager.SavaMeetingDataToCalendarFolder();
                //Outlook.AppointmentItem appt = Item as Outlook.AppointmentItem;
                //if (IsPingAnMeetingAppointment(appt))
                //{
                //    SVCMMeetingDetail meeting = this._appointmentManager.GetMeetingFromAppointment(appt, false);

                //    if (meeting != null && !string.IsNullOrEmpty(meeting.Id))
                //    {
                //        if (this._calendarManager.MeetingDetailDataLocal.ContainsKey(meeting.Id))
                //        {
                //            this._calendarManager.MeetingDetailDataLocal.Remove(meeting.Id);
                //        }

                //        this._calendarManager.MeetingDetailDataLocal.Add(meeting.Id, meeting);
                //        this._calendarManager.SavaMeetingDataToCalendarFolder();

                //    }
                //}
            }
            catch (Exception ex)
            {
                logger.Error("Items_ItemChange error!", ex);
                MessageBox.Show(ex.Message);
            }
        }

        void Items_ItemAdd(object Item)
        {
            try
            {
                Outlook.AppointmentItem appt = Item as Outlook.AppointmentItem;
                if (IsPingAnMeetingAppointment(appt))
                {
                    SVCMMeetingDetail meeting = this._appointmentManager.GetMeetingFromAppointment(appt, false);
                    if (meeting != null && !string.IsNullOrEmpty(meeting.Id))
                    {
                        if (this._calendarManager.MeetingDetailDataLocal.ContainsKey(meeting.Id))
                        {
                            this._calendarManager.MeetingDetailDataLocal.Remove(meeting.Id);
                        }

                        this._calendarManager.MeetingDetailDataLocal.Add(meeting.Id, meeting);
                        this._calendarManager.SavaMeetingDataToCalendarFolder();

                        if (!this._appointmentList.ContainsKey(meeting.Id))
                            this._appointmentList.Add(meeting.Id, appt);

                        appt.BeforeDelete -= new Outlook.ItemEvents_10_BeforeDeleteEventHandler(item_BeforeDelete);
                        appt.BeforeDelete += new Outlook.ItemEvents_10_BeforeDeleteEventHandler(item_BeforeDelete);
                    }
                    else
                    {
                        logger.Error("Item Added, Meeting or MeetingId is null!");
                        appt.Delete();
                        MessageBox.Show("会议参数不全，放弃保存！");
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error("Item Added error!", ex);
                MessageBox.Show(ex.Message);

            }
        }

        public void DoBookingMeeting()
        {
            try
            {
                bool login = false;
                if (!OutlookFacade.Instance().Session.IsActive)
                {
                    var session = OutlookFacade.Instance().Session;
                    login = ClientServiceFactory.Create().Login(ref session);
                    if (login)
                        OutlookFacade.Instance().CalendarFolder.CalendarDataManager.SyncMeetingList();

                    if (login)
                    {
                        //set holiday ribbon
                        OutlookFacade.Instance().MyRibbon.RibbonType = MyRibbonType.SVCM;

                        //Create a holiday appointmet and set properties
                        Outlook.AppointmentItem apptItem = OutlookFacade.Instance().CalendarFolder.MAPIFolder.Items.Add("IPM.Appointment.PingAnMeetingRequest");

                        //display the appointment
                        Outlook.Inspector inspect = Globals.ThisAddIn.Application.Inspectors.Add(apptItem);
                        inspect.Display(false);
                        //reset the ribbon to normal
                        OutlookFacade.Instance().MyRibbon.RibbonType = MyRibbonType.Original;
                    }
                    else
                    {
                        MessageBox.Show("登陆服务器失败，不能进行预约，请重试或联系管理员！");
                    }
                }
                else
                {
                    //set holiday ribbon
                    OutlookFacade.Instance().MyRibbon.RibbonType = MyRibbonType.SVCM;

                    //Create a holiday appointmet and set properties
                    Outlook.AppointmentItem apptItem = OutlookFacade.Instance().CalendarFolder.MAPIFolder.Items.Add("IPM.Appointment.PingAnMeetingRequest");

                    //display the appointment
                    Outlook.Inspector inspect = Globals.ThisAddIn.Application.Inspectors.Add(apptItem);
                    inspect.Display(false);
                    //reset the ribbon to normal
                    OutlookFacade.Instance().MyRibbon.RibbonType = MyRibbonType.Original;
                }
            }
            catch (Exception ex)
            {
                logger.Error("DoBookingMeeting failed.", ex);
            }
        }

        public void DoBookingMeeting(DateTime start)
        {
            try
            {
                bool login = false;
                if (!OutlookFacade.Instance().Session.IsActive)
                {
                    var session = OutlookFacade.Instance().Session;
                    login = ClientServiceFactory.Create().Login(ref session);
                    if (login)
                        OutlookFacade.Instance().CalendarFolder.CalendarDataManager.SyncMeetingList();

                    if (login)
                    {
                        //set holiday ribbon
                        OutlookFacade.Instance().MyRibbon.RibbonType = MyRibbonType.SVCM;

                        //Create a holiday appointmet and set properties
                        Outlook.AppointmentItem apptItem = OutlookFacade.Instance().CalendarFolder.MAPIFolder.Items.Add("IPM.Appointment.PingAnMeetingRequest");
                        apptItem.Start = start;
                        //display the appointment
                        Outlook.Inspector inspect = Globals.ThisAddIn.Application.Inspectors.Add(apptItem);
                        inspect.Display(false);
                        //reset the ribbon to normal
                        OutlookFacade.Instance().MyRibbon.RibbonType = MyRibbonType.Original;
                    }
                    else
                    {
                        MessageBox.Show("登陆服务器失败，不能进行预约，请重试或联系管理员！");
                    }
                }
                else
                {
                    //set holiday ribbon
                    OutlookFacade.Instance().MyRibbon.RibbonType = MyRibbonType.SVCM;

                    //Create a holiday appointmet and set properties
                    Outlook.AppointmentItem apptItem = OutlookFacade.Instance().CalendarFolder.MAPIFolder.Items.Add("IPM.Appointment.PingAnMeetingRequest");
                    apptItem.Start = start;
                    //display the appointment
                    Outlook.Inspector inspect = Globals.ThisAddIn.Application.Inspectors.Add(apptItem);
                    inspect.Display(false);
                    //reset the ribbon to normal
                    OutlookFacade.Instance().MyRibbon.RibbonType = MyRibbonType.Original;
                }
            }
            catch (Exception ex)
            {
                logger.Error("DoBookingMeeting failed.", ex);
            }
        }

        public void DoMeetingList()
        {
            try
            {
                bool login = false;
                MeetingCenterForm form = null;

                if (!OutlookFacade.Instance().Session.IsActive)
                {
                    var session = OutlookFacade.Instance().Session;
                    login = ClientServiceFactory.Create().Login(ref session);
                    if (login)
                        OutlookFacade.Instance().CalendarFolder.CalendarDataManager.SyncMeetingList();

                    if (login)
                    {
                        form = new MeetingCenterForm();

                    }
                    else
                    {
                        MessageBox.Show("登陆服务器失败，不能进行预约，请重试或联系管理员！");
                        return;
                    }
                }
                else
                {
                    form = new MeetingCenterForm();
                }

                if (form != null)
                    form.Show();
            }
            catch (Exception ex)
            {
                logger.Error("DoMeetingList failed!", ex);
            }
        }
    }
}
