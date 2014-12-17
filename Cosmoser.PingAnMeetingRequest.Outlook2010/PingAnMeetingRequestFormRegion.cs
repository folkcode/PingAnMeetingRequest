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
                this.Manifest.FormRegionType = Microsoft.Office.Tools.Outlook.FormRegionType.ReplaceAll;
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
        SVCMMeetingDetail meeting;

        // Occurs before the form region is displayed.
        // Use this.OutlookItem to get a reference to the current Outlook item.
        // Use this.OutlookFormRegion to get a reference to the form region.
        private void PingAnMeetingRequestFormRegion_FormRegionShowing(object sender, System.EventArgs e)
        {
            this.btnCanhuilingdao.Click += new Outlook.OlkCommandButtonEvents_ClickEventHandler(btnCanhuilingdao_Click);
            this.olkTxtLocation.Click += new Outlook.OlkTextBoxEvents_ClickEventHandler(olkTxtLocation_Click);
            this.olkbtnMobileTerm.Click += new Outlook.OlkCommandButtonEvents_ClickEventHandler(olkbtnMobileTerm_Click);

            OutlookFacade.Instance().MyRibbon.RibbonType = MyRibbonType.SVCM;

            this.InitializeUI();
            Outlook.AppointmentItem item = this.OutlookItem as Outlook.AppointmentItem;
            item.Write += new Outlook.ItemEvents_10_WriteEventHandler(item_Write);

            this.RegisterControlValueChangeEvents();
        }

        void olkbtnMobileTerm_Click()
        {
            IMobileTermView view = new MobileTermForm();
            view.MobileTermList = new List<MobileTerm>();
            view.MobileTermList.AddRange(meeting.MobileTermList);
            if (view.Display() == System.Windows.Forms.DialogResult.OK)
            {
                meeting.MobileTermList = view.MobileTermList;
                this.SaveMeetingToAppointment();
            }
        }

        // Occurs when the form region is closed.
        // Use this.OutlookItem to get a reference to the current Outlook item.
        // Use this.OutlookFormRegion to get a reference to the form region.
        private void PingAnMeetingRequestFormRegion_FormRegionClosed(object sender, System.EventArgs e)
        {
            OutlookFacade.Instance().MyRibbon.RibbonType = MyRibbonType.Original;

            Outlook.AppointmentItem item = this.OutlookItem as Outlook.AppointmentItem;
            if (!this._apptMgr.IsAppointmentStatusDeleted(item) && item.Saved)
            {
                this.SaveMeetingToAppointment();
            }
        }

        private void RegisterControlValueChangeEvents()
        {
            this.olkTxtSubject.Change += new Outlook.OlkTextBoxEvents_ChangeEventHandler(ValueChanged);
            this.olkTxtLocation.Change += new Outlook.OlkTextBoxEvents_ChangeEventHandler(ValueChanged);
            this.olkStartDateControl.Change += new Outlook.OlkDateControlEvents_ChangeEventHandler(ValueChanged);
            this.olkStartTimeControl.Change += new Outlook.OlkTimeControlEvents_ChangeEventHandler(ValueChanged);
            this.olkEndDateControl.Change += new Outlook.OlkDateControlEvents_ChangeEventHandler(ValueChanged);
            this.olkEndTimeControl.Change += new Outlook.OlkTimeControlEvents_ChangeEventHandler(ValueChanged);
            this.obtbendi.Change += new Outlook.OlkOptionButtonEvents_ChangeEventHandler(ValueChanged);
            this.obtliji.Change += new Outlook.OlkOptionButtonEvents_ChangeEventHandler(ValueChanged);
            this.obtshipin.Change += new Outlook.OlkOptionButtonEvents_ChangeEventHandler(ValueChanged);
            this.obtyuyue.Change += new Outlook.OlkOptionButtonEvents_ChangeEventHandler(ValueChanged);

            this.txtPeopleCount.Change += new Outlook.OlkTextBoxEvents_ChangeEventHandler(txtPeopleCount_ValueChanged);
            this.txtPhone.Change += new Outlook.OlkTextBoxEvents_ChangeEventHandler(ValueChanged);

            this.obtxsms0.Change += new Outlook.OlkOptionButtonEvents_ChangeEventHandler(ValueChanged);
            this.obtxsms1.Change += new Outlook.OlkOptionButtonEvents_ChangeEventHandler(ValueChanged);
            this.obtxsms2.Change += new Outlook.OlkOptionButtonEvents_ChangeEventHandler(ValueChanged);
            this.obtxsms3.Change += new Outlook.OlkOptionButtonEvents_ChangeEventHandler(ValueChanged);
            this.obtxsms4.Change += new Outlook.OlkOptionButtonEvents_ChangeEventHandler(ValueChanged);


            this.txtPassword.Change += new Outlook.OlkTextBoxEvents_ChangeEventHandler(ValueChanged);
            this.txtIPCount.Change += new Outlook.OlkTextBoxEvents_ChangeEventHandler(ValueChanged);

            //this.optOtherBooking.Change += new Outlook.OlkOptionButtonEvents_ChangeEventHandler(ValueChanged);
            //this.optselfbooking.Change += new Outlook.OlkOptionButtonEvents_ChangeEventHandler(ValueChanged);
        }

        void obtliji_Click()
        {
            this.obtyuyue.Value = false;
            this.obtliji.Value = true;

            this.SaveMeetingToAppointment();
        }

        void obtyuyue_Click()
        {
            this.obtyuyue.Value = true;
            this.obtliji.Value = false;

            this.SaveMeetingToAppointment();
        }

        void obtshipin_Click()
        {
            this.obtshipin.Value = true;
            this.obtbendi.Value = false;

            this.SaveMeetingToAppointment();
        }

        void obtbendi_Click()
        {
            this.obtshipin.Value = true;
            this.obtbendi.Value = false;

            this.SaveMeetingToAppointment();
        }

        void ValueChanged()
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
            this.SaveMeetingToAppointment();
        }

        void item_Write(ref bool Cancel)
        {
            this.SaveMeetingToAppointment();
        }

        void InitializeUI()
        {
            if (this._apptMgr.GetMeetingIdFromAppointment(this.OutlookItem as Outlook.AppointmentItem) != null)
            {
                meeting = this._apptMgr.GetMeetingFromAppointment(this.OutlookItem as Outlook.AppointmentItem,false);
                this.olkStartDateControl.Date = meeting.StartTime.Date;
                this.olkStartTimeControl.Time = meeting.StartTime;
                this.olkEndDateControl.Date = meeting.EndTime.Date;
                this.olkEndTimeControl.Time = meeting.EndTime;

                this.olkTxtSubject.Text = meeting.Name;
                this.olkTxtLocation.Text = meeting.RoomsStr;

                this.txtPassword.Text = meeting.Password;
                if (meeting.ConfType == ConferenceType.Immediate)
                    this.obtliji.Value = true;
                else if (meeting.ConfType == ConferenceType.Furture)
                    this.obtyuyue.Value = true;
                else
                {
                    this.obtliji.Value = false;
                    this.obtyuyue.Value = false;
                }

                if (meeting.ConfMideaType == MideaType.Local)
                    this.obtbendi.Value = true;
                else
                    this.obtshipin.Value = true;
                this.txtPeopleCount.Text = meeting.ParticipatorNumber.ToString();
                this.txtPhone.Text = meeting.Phone;

                switch (meeting.VideoSet)
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

                this.txtIPCount.Text = meeting.IPDesc;
                (this.OutlookItem as Outlook.AppointmentItem).Body = meeting.Memo;

            }
            else
            {
                this.meeting = new SVCMMeetingDetail();
            }
        }

        void olkTxtLocation_Click()
        {
            IMeetingRoomView view = new Views.MeetingRoomSelection();
            view.MeetingRoomList = new List<MeetingRoom>();
            view.MeetingRoomList.AddRange(meeting.Rooms);
            view.MainRoom = new MeetingRoom();

            view.ConfType = meeting.ConfType;

            if (view.Display() == System.Windows.Forms.DialogResult.OK)
            {
                meeting.Rooms = view.MeetingRoomList;
                meeting.MainRoom = view.MainRoom;
                this.olkTxtLocation.Text = meeting.RoomsStr;
                this.SaveMeetingToAppointment();
            }
        }

        void btnCanhuilingdao_Click()
        {
            IAttendedLeadersView view = new Views.AttendedBossForm();
            view.LeaderRoom = meeting.LeaderRoom;
            view.LeaderList = new List<MeetingLeader>();
            view.LeaderList.AddRange(meeting.LeaderList);
            if (view.Display() == System.Windows.Forms.DialogResult.OK)
            {
                meeting.LeaderList = view.LeaderList;
                meeting.LeaderRoom = view.LeaderRoom;
                this.SaveMeetingToAppointment();
            }
        }

        private void SaveMeetingToAppointment()
        {
            Outlook.AppointmentItem item = this.OutlookItem as Outlook.AppointmentItem;

            meeting.Name = this.olkTxtSubject.Text;
            meeting.StartTime = this.olkStartDateControl.Date;
            meeting.StartTime = this.olkStartTimeControl.Time;
            meeting.EndTime = this.olkEndDateControl.Date;
            meeting.EndTime = this.olkEndTimeControl.Time;

            if (this.obtliji.Value == true)
            {
                meeting.ConfType = ConferenceType.Immediate;
            }
            else if (this.obtyuyue.Value == true)
            {
                meeting.ConfType = ConferenceType.Furture;
            }
            else
            {
                meeting.ConfType = ConferenceType.Recurring;
            }

            if (this.obtbendi.Value == true)
                meeting.ConfMideaType = MideaType.Local;
            else
                meeting.ConfMideaType = MideaType.Video;

            if (!string.IsNullOrEmpty(this.txtPeopleCount.Text))
                meeting.ParticipatorNumber = int.Parse(this.txtPeopleCount.Text);
            meeting.Password = this.txtPassword.Text;
            meeting.IPDesc = this.txtIPCount.Text;
            meeting.Phone = this.txtPhone.Text;
            meeting.Memo = item.Body;

            if (this.obtxsms0.Value)
            {
                meeting.VideoSet = VideoSet.Audio;
            }
            else if (this.obtxsms1.Value)
            {
                meeting.VideoSet = VideoSet.MainRoom;
            }
            else if (this.obtxsms2.Value)
            {
                meeting.VideoSet = VideoSet.EqualScreen;
            }
            else if (this.obtxsms3.Value)
            {
                meeting.VideoSet = VideoSet.OneNScreen;
            }
            else if (this.obtxsms4.Value)
            {
                meeting.VideoSet = VideoSet.TwoNScreen;
            }

            this._apptMgr.SaveMeetingToAppointment(meeting, item,true);

        }
    }
}
