using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Text;
using Office = Microsoft.Office.Core;
using Outlook = Microsoft.Office.Interop.Outlook;

using Cosmoser.PingAnMeetingRequest.Common.Model;
using Cosmoser.PingAnMeetingRequest.Outlook2010.Manager;
using Cosmoser.PingAnMeetingRequest.Outlook2010.Views;
using System.Windows.Forms;
using Cosmoser.PingAnMeetingRequest.Common.ClientService;
using log4net;
using Cosmoser.PingAnMeetingRequest.Common.Utilities;
using Microsoft.Vbe.Interop.Forms;
using System.Runtime.InteropServices;

namespace Cosmoser.PingAnMeetingRequest.Outlook2010
{
    public partial class PingAnMeetingRequestFormRegion
    {
        #region Form Region Factory

        [Microsoft.Office.Tools.Outlook.FormRegionMessageClass("IPM.Appointment.PingAnMeetingRequest")]
        [Microsoft.Office.Tools.Outlook.FormRegionName("Cosmoser.PingAnMeetingRequest.Outlook2010.PingAnMeetingRequestFormRegion")]
        public partial class PingAnMeetingRequestFormRegionFactory
        {
            private void InitializeManifest()
            {
                ResourceManager resources = new ResourceManager(typeof(PingAnMeetingRequestFormRegion));
                this.Manifest.FormRegionType = Microsoft.Office.Tools.Outlook.FormRegionType.Replacement;
                this.Manifest.Title = resources.GetString("Title");
                this.Manifest.FormRegionName = resources.GetString("FormRegionName");
                this.Manifest.Description = resources.GetString("Description");
                this.Manifest.ShowInspectorCompose = true;
                this.Manifest.ShowInspectorRead = true;
                this.Manifest.ShowReadingPane = true;

            }

            // Occurs before the form region is initialized.
            // To prevent the form region from appearing, set e.Cancel to true.
            // Use e.OutlookItem to get a reference to the current Outlook item.
            private void PingAnMeetingRequestFormRegionFactory_FormRegionInitializing(object sender, Microsoft.Office.Tools.Outlook.FormRegionInitializingEventArgs e)
            {
            }
        }

        #endregion

        private AppointmentManager _apptMgr = new AppointmentManager();
        static ILog logger = IosLogManager.GetLogger(typeof(PingAnMeetingRequestFormRegion));
        static DateTime startTime;
        static DateTime endTime;
        static int valueChangeCount = 0;

        private Outlook.AppointmentItem item;

        // Occurs before the form region is displayed.
        // Use this.OutlookItem to get a reference to the current Outlook item.
        // Use this.OutlookFormRegion to get a reference to the form region.
        private void PingAnMeetingRequestFormRegion_FormRegionShowing(object sender, System.EventArgs e)
        {
            try
            {
                OutlookFacade.Instance().MyRibbon.RibbonType = MyRibbonType.SVCM;

                item = (Outlook.AppointmentItem)this.OutlookFormRegion.Item;
                startTime = DateTime.Now;

                if ((startTime - endTime).TotalSeconds < 1)
                    return;

                this.InitializeUI();

                this.RegisterControlValueChangeEvents();
            }
            catch (Exception ex)
            {
                logger.Error("PingAnMeetingRequestFormRegion_FormRegionShowing error", ex);
            }
        }

        // Occurs when the form region is closed.
        // Use this.OutlookItem to get a reference to the current Outlook item.
        // Use this.OutlookFormRegion to get a reference to the form region.
        private void PingAnMeetingRequestFormRegion_FormRegionClosed(object sender, System.EventArgs e)
        {
            this.UnRegisterControlValueChangeEvents();
            OutlookFacade.Instance().MyRibbon.RibbonType = MyRibbonType.Original;

            endTime = DateTime.Now;
        }

        private void RegisterControlValueChangeEvents()
        {
            this.btnCanhuilingdao.Click += new Outlook.OlkCommandButtonEvents_ClickEventHandler(btnCanhuilingdao_Click);
            this.olkTxtLocation.Click += new Outlook.OlkTextBoxEvents_ClickEventHandler(olkTxtLocation_Click);
            this.olkbtnMobileTerm.Click += new Outlook.OlkCommandButtonEvents_ClickEventHandler(olkbtnMobileTerm_Click);

            this.obtliji.Click += new Outlook.OlkOptionButtonEvents_ClickEventHandler(obtliji_Click);
            this.obtyuyue.Click += new Outlook.OlkOptionButtonEvents_ClickEventHandler(obtyuyue_Click);
            this.obtbendi.Click += new Outlook.OlkOptionButtonEvents_ClickEventHandler(obtbendi_Click);
            this.obtshipin.Click += new Outlook.OlkOptionButtonEvents_ClickEventHandler(obtshipin_Click);

            this.olkTxtSubject.Change += new Outlook.OlkTextBoxEvents_ChangeEventHandler(ValueChanged);
            this.olkTxtLocation.Change += new Outlook.OlkTextBoxEvents_ChangeEventHandler(ValueChanged);

            this.olkStartDateControl.Change += new Outlook.OlkDateControlEvents_ChangeEventHandler(olkStartDateControl_Change);
            this.olkStartTimeControl.Change += new Outlook.OlkTimeControlEvents_ChangeEventHandler(olkStartTimeControl_Change);
            this.olkEndDateControl.Change += new Outlook.OlkDateControlEvents_ChangeEventHandler(olkEndDateControl_Change);
            this.olkEndTimeControl.Change += new Outlook.OlkTimeControlEvents_ChangeEventHandler(olkEndTimeControl_Change);

            this.txtPeopleCount.Change += new Outlook.OlkTextBoxEvents_ChangeEventHandler(txtPeopleCount_ValueChanged);
            this.txtPhone.Change += new Outlook.OlkTextBoxEvents_ChangeEventHandler(ValueChanged);

            this.obtxsms0.Change += new Outlook.OlkOptionButtonEvents_ChangeEventHandler(ValueChanged);
            this.obtxsms1.Change += new Outlook.OlkOptionButtonEvents_ChangeEventHandler(ValueChanged);
            this.obtxsms2.Change += new Outlook.OlkOptionButtonEvents_ChangeEventHandler(ValueChanged);
            this.obtxsms3.Change += new Outlook.OlkOptionButtonEvents_ChangeEventHandler(ValueChanged);
            this.obtxsms4.Change += new Outlook.OlkOptionButtonEvents_ChangeEventHandler(ValueChanged);


            this.txtIPCount.Change += new Outlook.OlkTextBoxEvents_ChangeEventHandler(ValueChanged);
            item.Write += new Outlook.ItemEvents_10_WriteEventHandler(item_Write);

            this.commandButton1.Click += new Outlook.OlkCommandButtonEvents_ClickEventHandler(commandButton1_Click);

            OutlookFacade.Instance().ItemSend += new EventHandler(PingAnMeetingRequestFormRegion_ItemSend);
        }

        void PingAnMeetingRequestFormRegion_ItemSend(object sender, EventArgs e)
        {
            MessageBox.Show("邮件发送成功！");
            isSendingMeeting = true;
            this.commandButton1.Caption = "发送更新";
        }

        bool isSendingMeeting = false;
        void commandButton1_Click()
        {
            Outlook._AppointmentItem appt = (Outlook._AppointmentItem)item;
            if (appt.MeetingStatus == Outlook.OlMeetingStatus.olMeeting)
            {
                appt.ForceUpdateToAllAttendees = true;
                appt.Save();
            }
            else
            {
                appt.MeetingStatus = Outlook.OlMeetingStatus.olMeeting;
            }

            appt.ForceUpdateToAllAttendees = true;
            Outlook.Recipient recipient = null;

            try
            {
                if (appt.Recipients.Count == 0)
                {
                    MessageBox.Show(string.Format("没有收件人，请设置收件人！"));
                    return;
                }

                foreach (var r in appt.Recipients)
                {
                    recipient = (Outlook.Recipient)r;
                    recipient.Resolve();
                    string email = recipient.Address;
                    if (string.IsNullOrEmpty(email))
                    {
                        MessageBox.Show(string.Format("收件人{0}不能识别，请修正！", recipient.Name));
                        return;
                    }

                    Marshal.ReleaseComObject(recipient);
                }

                isSendingMeeting = true;
                appt.Send();               
                
            }
            catch (Exception ex)
            {
                logger.Error("send meeting failed.", ex);
                if (recipient != null)
                    Marshal.ReleaseComObject(recipient);
            }
        }

        void olkEndDateControl_Change()
        {
            this.SaveMeetingToAppointment();
        }

        void olkStartDateControl_Change()
        {
            this.SaveMeetingToAppointment();
        }

        void olkEndTimeControl_Change()
        {
            //item.End = this.olkEndTimeControl.Time;
            //this.SaveMeetingToAppointment();
        }

        void olkStartTimeControl_Change()
        {
            //item.Start = this.olkStartTimeControl.Time;
            //this.SaveMeetingToAppointment();
        }

        private void UnRegisterControlValueChangeEvents()
        {
            this.olkTxtSubject.Change -= new Outlook.OlkTextBoxEvents_ChangeEventHandler(ValueChanged);
            this.olkTxtLocation.Change -= new Outlook.OlkTextBoxEvents_ChangeEventHandler(ValueChanged);
            this.olkStartDateControl.Change -= new Outlook.OlkDateControlEvents_ChangeEventHandler(ValueChanged);
            this.olkStartTimeControl.Change -= new Outlook.OlkTimeControlEvents_ChangeEventHandler(ValueChanged);
            this.olkEndDateControl.Change -= new Outlook.OlkDateControlEvents_ChangeEventHandler(ValueChanged);
            this.olkEndTimeControl.Change -= new Outlook.OlkTimeControlEvents_ChangeEventHandler(ValueChanged);
            this.obtbendi.Change -= new Outlook.OlkOptionButtonEvents_ChangeEventHandler(ValueChanged);
            this.obtliji.Change -= new Outlook.OlkOptionButtonEvents_ChangeEventHandler(ValueChanged);
            this.obtshipin.Change -= new Outlook.OlkOptionButtonEvents_ChangeEventHandler(ValueChanged);
            this.obtyuyue.Change -= new Outlook.OlkOptionButtonEvents_ChangeEventHandler(ValueChanged);

            this.txtPeopleCount.Change -= new Outlook.OlkTextBoxEvents_ChangeEventHandler(txtPeopleCount_ValueChanged);
            this.txtPhone.Change -= new Outlook.OlkTextBoxEvents_ChangeEventHandler(ValueChanged);


            this.obtxsms0.Change -= new Outlook.OlkOptionButtonEvents_ChangeEventHandler(ValueChanged);
            this.obtxsms1.Change -= new Outlook.OlkOptionButtonEvents_ChangeEventHandler(ValueChanged);
            this.obtxsms2.Change -= new Outlook.OlkOptionButtonEvents_ChangeEventHandler(ValueChanged);
            this.obtxsms3.Change -= new Outlook.OlkOptionButtonEvents_ChangeEventHandler(ValueChanged);
            this.obtxsms4.Change -= new Outlook.OlkOptionButtonEvents_ChangeEventHandler(ValueChanged);


            this.txtIPCount.Change -= new Outlook.OlkTextBoxEvents_ChangeEventHandler(ValueChanged);

            this.btnCanhuilingdao.Click -= new Outlook.OlkCommandButtonEvents_ClickEventHandler(btnCanhuilingdao_Click);
            this.olkTxtLocation.Click -= new Outlook.OlkTextBoxEvents_ClickEventHandler(olkTxtLocation_Click);
            this.olkbtnMobileTerm.Click -= new Outlook.OlkCommandButtonEvents_ClickEventHandler(olkbtnMobileTerm_Click);

            this.obtliji.Click -= new Outlook.OlkOptionButtonEvents_ClickEventHandler(obtliji_Click);
            this.obtyuyue.Click -= new Outlook.OlkOptionButtonEvents_ClickEventHandler(obtyuyue_Click);
            this.obtbendi.Click -= new Outlook.OlkOptionButtonEvents_ClickEventHandler(obtbendi_Click);
            this.obtshipin.Click -= new Outlook.OlkOptionButtonEvents_ClickEventHandler(obtshipin_Click);
            item.Write -= new Outlook.ItemEvents_10_WriteEventHandler(item_Write);

            OutlookFacade.Instance().ItemSend -= new EventHandler(PingAnMeetingRequestFormRegion_ItemSend);
        }

        void olkbtnMobileTerm_Click()
        {
            IMobileTermView view = new MobileTermForm();
            view.MobileTermList = new List<MobileTerm>();
            view.MobileTermList.AddRange(MeetingDetail.MobileTermList);
            if (view.Display() == System.Windows.Forms.DialogResult.OK)
            {
                MeetingDetail.MobileTermList = view.MobileTermList;
                this.SaveMeetingToAppointment();
            }
        }

        void obtliji_Click()
        {
            item.Start = DateTime.Now.AddMinutes(3);

            this.olkStartDateControl.Enabled = false;
            this.olkStartTimeControl.Enabled = false;

            item.End = DateTime.Now.AddMinutes(33);

            this.SaveMeetingToAppointment();
        }

        void obtyuyue_Click()
        {
            this.olkStartDateControl.Enabled = true;
            this.olkStartTimeControl.Enabled = true;

            this.SaveMeetingToAppointment();
        }

        void obtshipin_Click()
        {
            this.EnableVideoSet(true);

            this.SaveMeetingToAppointment();
        }

        private void EnableVideoSet(bool p)
        {
            this.obtxsms0.Enabled = p;
            this.obtxsms1.Enabled = p;
            this.obtxsms2.Enabled = p;
            this.obtxsms3.Enabled = p;
            this.obtxsms4.Enabled = p;
        }

        void obtbendi_Click()
        {
            this.EnableVideoSet(false);

            this.SaveMeetingToAppointment();
        }

        void ValueChanged()
        {
            if (valueChangeCount > 1)
                return;
            valueChangeCount++;
            this.SaveMeetingToAppointment();
            valueChangeCount--;
        }

        void LijiMeetingChanged()
        {
            this.SaveMeetingToAppointment();
        }

        void YuyueMeetingChanged()
        {
            this.SaveMeetingToAppointment();
        }

        void txtPeopleCount_ValueChanged()
        {
            if (!string.IsNullOrEmpty(this.txtPeopleCount.Text))
            {
                int pcount;
                if (!int.TryParse(this.txtPeopleCount.Text.Trim(), out pcount))
                {
                    System.Windows.Forms.MessageBox.Show("请输入一个数字");
                    this.txtPeopleCount.Text = string.Empty;
                    return;
                }
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("请填写参会人数，且参会人数大于0！");
                MeetingDetail.ParticipatorNumber = 0;
            }

            this.SaveMeetingToAppointment();
        }

        void item_Write(ref bool Cancel)
        {
            if (isSendingMeeting)
            {
                isSendingMeeting = false;
                return;
            }

            if (OutlookFacade.Instance().MyRibbon.IsRibbonAction)
            {
                return;
            }

            var updatingMeeting = OutlookFacade.Instance().MyRibbon.MeetingDetail;//this.MeetingDetail;//this._apptMgr.GetMeetingFromAppointment(item, true);
            if (updatingMeeting != null)
            {
                MessageBox.Show("如果想保存修改，请使用保存关闭按钮操作！");
                Cancel = true;
            }
        }

        void InitializeUI()
        {
            logger.Debug("InitializeUI");
            logger.Debug("Begin getting MeetingId");
            this.txtPhone.MaxLength = 20;
            this.txtPeopleCount.MaxLength = 4;

            this.label1.Caption = "邀请与会者";
           

            if (item.Recipients.Count > 0)
                this.commandButton1.Caption = "发送更新";
            else
                this.commandButton1.Caption = "发送";

            string meetingId = this._apptMgr.GetMeetingIdFromAppointment(item);
            int hashCode = item.GetHashCode();
            if (meetingId != null)
            {
                SVCMMeetingDetail meeting;
                if (!ClientServiceFactory.Create().TryGetMeetingDetail(meetingId, OutlookFacade.Instance().Session, out meeting))
                {
                    if (OutlookFacade.Instance().CalendarFolder.CalendarDataManager.MeetingDetailDataLocal.ContainsKey(meetingId))
                        this.MeetingDetail = OutlookFacade.Instance().CalendarFolder.CalendarDataManager.MeetingDetailDataLocal[meetingId];
                    else
                        this.MeetingDetail = this._apptMgr.GetMeetingFromAppointment(item, false);

                    if (!OutlookFacade.Instance().MyRibbon.UpdatingQueueCollection.ContainsKey(hashCode))
                        OutlookFacade.Instance().MyRibbon.UpdatingQueueCollection.Add(hashCode, this.MeetingDetail);
                }
                else
                {
                    this.MeetingDetail = meeting;
                    if (!OutlookFacade.Instance().MyRibbon.UpdatingQueueCollection.ContainsKey(hashCode))
                        OutlookFacade.Instance().MyRibbon.UpdatingQueueCollection.Add(item.GetHashCode(), this.MeetingDetail);

                    //update local meetingDetailData
                    if (OutlookFacade.Instance().CalendarFolder.CalendarDataManager.MeetingDetailDataLocal.ContainsKey(meeting.Id))
                        OutlookFacade.Instance().CalendarFolder.CalendarDataManager.MeetingDetailDataLocal.Remove(meeting.Id);
                    OutlookFacade.Instance().CalendarFolder.CalendarDataManager.MeetingDetailDataLocal.Add(meeting.Id, meeting);
                    OutlookFacade.Instance().CalendarFolder.CalendarDataManager.SavaMeetingDataToCalendarFolder();
                    //this._apptMgr.SaveMeetingToAppointment(MeetingDetail, item, false);
                }

                //item.Start = MeetingDetail.StartTime;
                //item.End = MeetingDetail.EndTime;

                this.olkTxtSubject.Text = MeetingDetail.Name;
                item.Location = MeetingDetail.RoomsStr;
                //this.olkTxtLocation.Text = MeetingDetail.RoomsStr;

                if (MeetingDetail.ConfType == ConferenceType.Immediate)
                {
                    this.obtliji.Value = true;
                    //this.obtliji.Enabled = false;
                    //this.obtbendi.Enabled = false;
                }
                else if (MeetingDetail.ConfType == ConferenceType.Furture)
                {
                    this.obtyuyue.Value = true;
                    //this.obtliji.Enabled = true;
                    //this.obtbendi.Enabled = true;
                }
                else
                {
                    this.obtliji.Value = false;
                    this.obtyuyue.Value = false;

                }

                if (MeetingDetail.ConfMideaType == MideaType.Local)
                    this.obtbendi.Value = true;
                else
                    this.obtshipin.Value = true;
                this.txtPeopleCount.Text = MeetingDetail.ParticipatorNumber.ToString();
                this.txtPhone.Text = MeetingDetail.Phone;

                switch (MeetingDetail.VideoSet)
                {
                    case VideoSet.Audio:
                        this.obtxsms0.Value = true;
                        break;
                    case VideoSet.MainRoom:
                        this.obtxsms1.Value = true;
                        break;
                    case VideoSet.EqualScreen:
                        this.obtxsms2.Value = true;
                        break;
                    case VideoSet.OneNScreen:
                        this.obtxsms3.Value = true;
                        break;
                    case VideoSet.TwoNScreen:
                        this.obtxsms4.Value = true;
                        break;
                }

                this.txtIPCount.Text = MeetingDetail.IPDesc;
                item.Body = MeetingDetail.Memo;

                //以下不能修改
                this.olkTxtSubject.Enabled = false;

                //this.SaveMeetingToAppointment();
            }
            else
            {
                OutlookFacade.Instance().MyRibbon.MeetingDetail = new SVCMMeetingDetail();
                this.MeetingDetail = OutlookFacade.Instance().MyRibbon.MeetingDetail;

                if (!OutlookFacade.Instance().MyRibbon.UpdatingQueueCollection.ContainsKey(hashCode))
                    OutlookFacade.Instance().MyRibbon.UpdatingQueueCollection.Add(hashCode, this.MeetingDetail);
                //默认语音激励
                this.obtxsms0.Value = true;
                //默认视频会议
                this.obtshipin.Value = true;
                //this.SaveMeetingToAppointment();

                this.olkTextBox1.Text = OutlookFacade.Instance().Session.Address;
            }
        }

        void olkTxtLocation_Click()
        {
            IMeetingRoomView view = new Views.MeetingRoomSelection();
            view.MeetingRoomList = new List<MeetingRoom>();
            view.MeetingRoomList.AddRange(MeetingDetail.Rooms);
            view.MainRoom = MeetingDetail.MainRoom;

            view.ConfType = MeetingDetail.ConfMideaType;
            view.StarTime = MeetingDetail.StartTime;
            view.EndTime = MeetingDetail.EndTime;

            if (view.Display() == System.Windows.Forms.DialogResult.OK)
            {
                MeetingDetail.Rooms = view.MeetingRoomList;
                MeetingDetail.MainRoom = view.MainRoom;
                item.Location = MeetingDetail.RoomsStr;
                //this.olkTxtLocation.Text = MeetingDetail.RoomsStr;
                this.SaveMeetingToAppointment();
            }
        }

        void btnCanhuilingdao_Click()
        {
            IAttendedLeadersView view = new Views.AttendedBossForm();
            view.LeaderRoom = MeetingDetail.LeaderRoom;
            view.LeaderList = new List<MeetingLeader>();
            view.LeaderList.AddRange(MeetingDetail.LeaderList);

            if (view.Display() == System.Windows.Forms.DialogResult.OK)
            {
                MeetingDetail.LeaderList = view.LeaderList;
                MeetingDetail.LeaderRoom = view.LeaderRoom;
                this.SaveMeetingToAppointment();
            }
        }

        private void SaveMeetingToAppointment()
        {
            try
            {
                logger.Debug("SaveMeetingToAppointment");
                MeetingDetail.Name = this.olkTxtSubject.Text;

                //MeetingDetail.StartTime = item.Start;// DateTime.Parse(this.olkStartDateControl.Date.ToString("yyyy-MM-dd ") + this.olkStartTimeControl.Time.ToString("HH:mm:ss")); //item.Start;
                //MeetingDetail.EndTime = item.End;// DateTime.Parse(this.olkEndDateControl.Date.ToString("yyyy-MM-dd ") + this.olkEndTimeControl.Time.ToString("HH:mm:ss")); //item.End;

                if (this.obtliji.Value == true)
                {
                    MeetingDetail.ConfType = ConferenceType.Immediate;
                }
                else if (this.obtyuyue.Value == true)
                {
                    MeetingDetail.ConfType = ConferenceType.Furture;
                }
                else
                {
                    MeetingDetail.ConfType = ConferenceType.Recurring;
                }

                if (this.obtbendi.Value == true)
                    MeetingDetail.ConfMideaType = MideaType.Local;
                else
                    MeetingDetail.ConfMideaType = MideaType.Video;

                if (!string.IsNullOrEmpty(this.txtPeopleCount.Text))
                    MeetingDetail.ParticipatorNumber = int.Parse(this.txtPeopleCount.Text.Trim());
                MeetingDetail.IPDesc = this.txtIPCount.Text == null ? string.Empty : this.txtIPCount.Text.Trim();
                MeetingDetail.Phone = this.txtPhone.Text == null ? string.Empty : this.txtPhone.Text.Trim();
                MeetingDetail.Memo = item.Body;

                if (this.obtxsms0.Value)
                {
                    MeetingDetail.VideoSet = VideoSet.Audio;
                }
                else if (this.obtxsms1.Value)
                {
                    MeetingDetail.VideoSet = VideoSet.MainRoom;
                }
                else if (this.obtxsms2.Value)
                {
                    MeetingDetail.VideoSet = VideoSet.EqualScreen;
                }
                else if (this.obtxsms3.Value)
                {
                    MeetingDetail.VideoSet = VideoSet.OneNScreen;
                }
                else if (this.obtxsms4.Value)
                {
                    MeetingDetail.VideoSet = VideoSet.TwoNScreen;
                }

                //this._apptMgr.SaveMeetingToAppointment(meeting, item, true);
            }
            catch (Exception ex)
            {
                logger.Error("SaveMeetingToAppointment error", ex);
            }
        }

        public SVCMMeetingDetail MeetingDetail
        {
            get;
            set;
        }
    }
}
