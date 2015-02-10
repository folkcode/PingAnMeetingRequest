using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using Office = Microsoft.Office.Core;
using Outlook = Microsoft.Office.Interop.Outlook;
using Cosmoser.PingAnMeetingRequest.Outlook2010.Manager;
using Cosmoser.PingAnMeetingRequest.Common.ClientService;
using Cosmoser.PingAnMeetingRequest.Outlook2010.Views;
using log4net;
using Cosmoser.PingAnMeetingRequest.Common.Utilities;
using System.Windows.Forms;
using Cosmoser.PingAnMeetingRequest.Common.Model;

// TODO:  Follow these steps to enable the Ribbon (XML) item:

// 1: Copy the following code block into the ThisAddin, ThisWorkbook, or ThisDocument class.

//  protected override Microsoft.Office.Core.IRibbonExtensibility CreateRibbonExtensibilityObject()
//  {
//      return new MyRibbon();
//  }

// 2. Create callback methods in the "Ribbon Callbacks" region of this class to handle user
//    actions, such as clicking a button. Note: if you have exported this Ribbon from the Ribbon designer,
//    move your code from the event handlers to the callback methods and modify the code to work with the
//    Ribbon extensibility (RibbonX) programming model.

// 3. Assign attributes to the control tags in the Ribbon XML file to identify the appropriate callback methods in your code.  

// For more information, see the Ribbon XML documentation in the Visual Studio Tools for Office Help.


namespace Cosmoser.PingAnMeetingRequest.Outlook2010
{
    public enum MyRibbonType
    {
        Original,
        SVCM
    }

    [ComVisible(true)]
    public class MyRibbon : Office.IRibbonExtensibility
    {
        internal static Office.IRibbonUI m_Ribbon;
        private Outlook.Application _application;
        private AppointmentManager _apptMgr = new AppointmentManager();
        static ILog logger = IosLogManager.GetLogger(typeof(MyRibbon));

        public MyRibbonType RibbonType
        {
            get;
            set;
        }

        public SVCMMeetingDetail MeetingDetail { get; set; }

        private Dictionary<int, SVCMMeetingDetail> _updatingQueueCollection = new Dictionary<int, SVCMMeetingDetail>();
        public Dictionary<int, SVCMMeetingDetail> UpdatingQueueCollection
        {
            get
            {
                return this._updatingQueueCollection;
            }
        }

        public int HashCode { get; set; }
        public MyRibbon(Outlook.Application application)
        {
            this._application = application;
        }

        #region IRibbonExtensibility Members

        public string GetCustomUI(string ribbonID)
        {
            string result = string.Empty;
            if (ribbonID == "Microsoft.Outlook.Appointment")
                result = GetResourceText("Cosmoser.PingAnMeetingRequest.Outlook2010.MyRibbon.xml");
            if (ribbonID == "Microsoft.Outlook.Explorer")
                result = GetResourceText("Cosmoser.PingAnMeetingRequest.Outlook2010.Ribbon.xml");
            return result;
        }

        #endregion

        #region Ribbon Callbacks
        //Create callback methods here. For more information about adding callback methods, select the Ribbon XML item in Solution Explorer and then press F1

        public void Ribbon_Load(Office.IRibbonUI ribbonUI)
        {
            m_Ribbon = ribbonUI;
        }

        /// <summary>
        /// tab idMso="TabAppointment"  getVisible="SystemBuildInVisible" 
        /// </summary>
        /// <param name="control"></param>
        /// <returns></returns>
        public bool SystemBuildInVisible(Office.IRibbonControl control)
        {
            if (this.RibbonType != MyRibbonType.Original)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// tab id="BpmCustomTabAppointment" getVisible="GetBpmCustomGroupVisible"
        /// </summary>
        /// <param name="control"></param>
        /// <returns></returns>
        public bool GetSVCMCustomGroupVisible(Office.IRibbonControl control)
        {
            if (this.RibbonType == MyRibbonType.SVCM)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool GetSaveAndCloseEnabled(Office.IRibbonControl control)
        {
            Outlook.AppointmentItem item = Globals.ThisAddIn.Application.ActiveInspector().CurrentItem as Outlook.AppointmentItem;

            var meeting = new AppointmentManager().GetMeetingFromAppointment(item, false);

            
            if (meeting != null && !string.IsNullOrEmpty(meeting.Status) && meeting.Status == "3")
                return false;
            else
                return true;
        }

        public bool GetDeleteEnabled(Office.IRibbonControl control)
        {
            Outlook.AppointmentItem item = Globals.ThisAddIn.Application.ActiveInspector().CurrentItem as Outlook.AppointmentItem;

            var meeting = new AppointmentManager().GetMeetingFromAppointment(item, false);

            if (meeting != null && !string.IsNullOrEmpty(meeting.Status) && meeting.Status == "3")
                return false;
            else
                return true;
        }

        public void DoDelete(Office.IRibbonControl control)
        {
            try
            {
                Outlook.AppointmentItem item = Globals.ThisAddIn.Application.ActiveInspector().CurrentItem as Outlook.AppointmentItem;
                logger.Debug("DoDelete appointment:" + item.Subject);

                string error;
                SVCMMeetingDetail meeting = this._apptMgr.GetMeetingFromAppointment(item, false);
                if (meeting != null)
                {
                    if (MessageBox.Show("你确定要删除该会议并发送取消会议邮件?", "提示信息", MessageBoxButtons.YesNo) != DialogResult.Yes)
                    {
                        return;
                    }

                    var calendarManager = OutlookFacade.Instance().CalendarFolder.CalendarDataManager;

                    if (calendarManager.MeetingDetailDataLocal.ContainsKey(meeting.Id))
                    {
                        bool suceed = ClientServiceFactory.Create().DeleteMeeting(meeting.Id, OutlookFacade.Instance().Session, out error);

                        if (suceed)
                        {
                            calendarManager.MeetingDetailDataLocal.Remove(meeting.Id);
                            calendarManager.SavaMeetingDataToCalendarFolder();

                            //48 issue
                            OutlookFacade.Instance().ItemSend += new EventHandler(MyRibbon_ItemSend);
                            Outlook._AppointmentItem appt = (Outlook._AppointmentItem)item;
                            appt.MeetingStatus = Outlook.OlMeetingStatus.olMeetingCanceled;
                            appt.Send();
                            OutlookFacade.Instance().ItemSend -= new EventHandler(MyRibbon_ItemSend);
                        }
                        else
                        {
                            System.Windows.Forms.MessageBox.Show(string.Format("向服务端删除会议失败！{0}！ 请重试。", error));
                            return;
                        }
                    }
                    else
                    {
                        logger.Debug(string.Format("item_BeforeDelete: meeting Id {0}, meetingName {1}. The meeting is not existing in server, no need update to server,only delete from outlook", meeting.Id, meeting.Name));
                    }

                    this._apptMgr.SetAppointmentDeleted(item, true);
                    item.Delete();
                }
                else
                {
                    item.Delete();
                }
            }
            catch (Exception ex)
            {
                logger.Error("DoDelete error", ex);
                MessageBox.Show(ex.Message);
            }
        }

        void MyRibbon_ItemSend(object sender, EventArgs e)
        {
            MessageBox.Show("取消会议发送成功！");
            OutlookFacade.Instance().ItemSend -= new EventHandler(MyRibbon_ItemSend);
        }

        public void DoSaveAndClose(Office.IRibbonControl control)
        {
            try
            {
                logger.Debug("Begin DoSaveAndClose appointment!");
                Outlook.AppointmentItem item = Globals.ThisAddIn.Application.ActiveInspector().CurrentItem as Outlook.AppointmentItem;
                string message;
                int hashCode = item.GetHashCode();

                if (this._updatingQueueCollection.ContainsKey(hashCode))
                    this.MeetingDetail = this._updatingQueueCollection[hashCode];

                logger.Debug("TryValidateApppointmentUIInput!");
                if (this._apptMgr.TryValidateApppointmentUIInput(this.MeetingDetail, out message))
                {

                    if (this.MeetingDetail != null)
                    {
                        //set comment
                        this.MeetingDetail.Memo = item.Body;
                        this.MeetingDetail.StartTime = item.Start;
                        this.MeetingDetail.EndTime = item.End;

                        string error;
                        if (string.IsNullOrEmpty(this.MeetingDetail.Id))
                        {
                            logger.Debug("This is a new appointment, booking Meeting to server!");

                            bool succeed = ClientServiceFactory.Create().BookingMeeting(this.MeetingDetail, OutlookFacade.Instance().Session, out error);

                            if (succeed)
                            {
                                this._apptMgr.SaveMeetingToAppointment(this.MeetingDetail, item, false);
                                this.MeetingDetail = null;
                                this._updatingQueueCollection.Remove(hashCode);
                                //this._apptMgr.RemoveUpdatingMeetingFromAppt(item);
                                Globals.ThisAddIn.Application.ActiveInspector().Close(Outlook.OlInspectorClose.olSave);
                            }
                            else
                            {
                                System.Windows.Forms.MessageBox.Show(string.Format("向服务端预约会议失败！{0} 请重试。", error));
                            }
                        }
                        else
                        {
                            logger.Debug("This is a existing appointment, updating Meeting to server!");
                            string errorCode;
                            bool succeed = ClientServiceFactory.Create().UpdateMeeting(this.MeetingDetail, "1", OutlookFacade.Instance().Session, out error, out errorCode);

                            if (succeed)
                            {
                                //this._apptMgr.SaveMeetingToAppointment(this.MeetingDetail, item, false);
                                this.MeetingDetail = null;
                                this._updatingQueueCollection.Remove(hashCode);
                                //this._apptMgr.RemoveUpdatingMeetingFromAppt(item);
                                Globals.ThisAddIn.Application.ActiveInspector().Close(Outlook.OlInspectorClose.olSave);
                            }
                            else
                            {
                                if (errorCode != "200" && errorCode != "500")
                                {
                                    if (MessageBox.Show(error, "", MessageBoxButtons.YesNo) == DialogResult.Yes)
                                    {
                                        succeed = ClientServiceFactory.Create().UpdateMeeting(this.MeetingDetail, "2", OutlookFacade.Instance().Session, out error, out errorCode);
                                        if (succeed)
                                        {
                                            //this._apptMgr.SaveMeetingToAppointment(this.MeetingDetail, item, false);
                                            this.MeetingDetail = null;
                                            this._updatingQueueCollection.Remove(hashCode);
                                            //this._apptMgr.RemoveUpdatingMeetingFromAppt(item);
                                            Globals.ThisAddIn.Application.ActiveInspector().Close(Outlook.OlInspectorClose.olSave);
                                        }
                                        else
                                        {
                                            System.Windows.Forms.MessageBox.Show(string.Format("向服务端更新会议失败！{0}！ 请重试。", error));
                                        }

                                    }
                                    else
                                    {
                                        System.Windows.Forms.MessageBox.Show("你已放弃修改会议！");
                                    }
                                }
                                else
                                {
                                    System.Windows.Forms.MessageBox.Show(string.Format("向服务端更新会议失败！{0}！ 请重试。", error));
                                }
                            }
                        }
                    }
                    else
                    {
                        //no updating, just close
                        logger.Debug("DoSaveAndClose: No updating, just close.");
                        Globals.ThisAddIn.Application.ActiveInspector().Close(Outlook.OlInspectorClose.olSave);
                    }
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show(message);
                }
            }
            catch (Exception ex)
            {
                logger.Error("DoSaveAndClose error", ex);
                MessageBox.Show(ex.Message);
            }
        }

        public void DoBookingMeeting(Office.IRibbonControl control)
        {
            OutlookFacade.Instance().CalendarFolder.DoBookingMeeting();
        }

        public void DoMeetingList(Office.IRibbonControl control)
        {
            OutlookFacade.Instance().CalendarFolder.DoMeetingList();
        }

        public void DoSchedulerSearch(Office.IRibbonControl control)
        {
            try
            {
                Outlook.AppointmentItem item = Globals.ThisAddIn.Application.ActiveInspector().CurrentItem as Outlook.AppointmentItem;

                MeetingDateSearchForm form = new MeetingDateSearchForm();
                form.SelectedDate = item.Start;
                form.Show();
            }
            catch (Exception ex)
            {
                logger.Error("DoSchedulerSearch error", ex);
            }
        }

        #endregion

        #region Helpers

        private static string GetResourceText(string resourceName)
        {
            Assembly asm = Assembly.GetExecutingAssembly();
            string[] resourceNames = asm.GetManifestResourceNames();
            for (int i = 0; i < resourceNames.Length; ++i)
            {
                if (string.Compare(resourceName, resourceNames[i], StringComparison.OrdinalIgnoreCase) == 0)
                {
                    using (StreamReader resourceReader = new StreamReader(asm.GetManifestResourceStream(resourceNames[i])))
                    {
                        if (resourceReader != null)
                        {
                            return resourceReader.ReadToEnd();
                        }
                    }
                }
            }
            return null;
        }

        #endregion
    }
}
