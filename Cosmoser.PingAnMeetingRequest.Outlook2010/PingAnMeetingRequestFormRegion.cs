using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Text;
using Office = Microsoft.Office.Core;
using Outlook = Microsoft.Office.Interop.Outlook;
using Cosmoser.PingAnMeetingRequest.Common.Model;
using Cosmoser.PingAnMeetingRequest.Outlook2010.Manager;

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

            OutlookFacade.Instance().MyRibbon.RibbonType = MyRibbonType.SVCM;

            this.InitializeUI();
            Outlook.AppointmentItem item = this.OutlookItem as Outlook.AppointmentItem;
            item.Write += new Outlook.ItemEvents_10_WriteEventHandler(item_Write);

            this.RegisterControlValueChangeEvents();
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

            this.txtPeopleCount.Change += new Outlook.OlkTextBoxEvents_ChangeEventHandler(ValueChanged);
            this.txtPhone.Change += new Outlook.OlkTextBoxEvents_ChangeEventHandler(ValueChanged);

            this.obtxsms0.Change += new Outlook.OlkOptionButtonEvents_ChangeEventHandler(ValueChanged);
            this.obtxsms1.Change += new Outlook.OlkOptionButtonEvents_ChangeEventHandler(ValueChanged);
            this.obtxsms2.Change += new Outlook.OlkOptionButtonEvents_ChangeEventHandler(ValueChanged);
            this.obtxsms3.Change += new Outlook.OlkOptionButtonEvents_ChangeEventHandler(ValueChanged);
            this.obtxsms4.Change += new Outlook.OlkOptionButtonEvents_ChangeEventHandler(ValueChanged);

            this.txtVideoCount.Change += new Outlook.OlkTextBoxEvents_ChangeEventHandler(ValueChanged);

            this.txtPassword.Change += new Outlook.OlkTextBoxEvents_ChangeEventHandler(ValueChanged);
            this.txtIPCount.Change += new Outlook.OlkTextBoxEvents_ChangeEventHandler(ValueChanged);

            this.optOtherBooking.Change += new Outlook.OlkOptionButtonEvents_ChangeEventHandler(ValueChanged);
            this.optselfbooking.Change += new Outlook.OlkOptionButtonEvents_ChangeEventHandler(ValueChanged);
        }

        void ValueChanged()
        {
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
                meeting = this._apptMgr.GetMeetingFromAppointment(this.OutlookItem as Outlook.AppointmentItem);
                this.txtPassword.Text = meeting.Password;
                if (meeting.ConfType == ConferenceType.Immediate)
                    this.obtliji.Value = true;
                else
                    this.obtyuyue.Value = true;

            }
            else
            {
                this.meeting = new SVCMMeetingDetail();
                this.meeting.Id = Guid.NewGuid().ToString();
            }
        }

        void olkTxtLocation_Click()
        {
            new Views.MeetingRoomSelection().ShowDialog();
        }

        void btnCanhuilingdao_Click()
        {
            new Views.AttendedBossForm().ShowDialog();
        }

        private void SaveMeetingToAppointment()
        {
            Outlook.AppointmentItem item = this.OutlookItem as Outlook.AppointmentItem;

            meeting.Name = this.olkTxtSubject.Text;
            meeting.StartTime = this.olkStartDateControl.Date;
            meeting.StartTime = this.olkStartTimeControl.Time;
            meeting.EndTime = this.olkEndDateControl.Date;
            meeting.EndTime = this.olkEndTimeControl.Time;


            meeting.Password = this.txtPassword.Text;

            this._apptMgr.SaveMeetingToAppointment(meeting, item);

        }
    }
}
