using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Text;
using Office = Microsoft.Office.Core;
using Outlook = Microsoft.Office.Interop.Outlook;
using Cosmoser.PingAnMeetingRequest.Outlook2007.Manager;
using Cosmoser.PingAnMeetingRequest.Common.Model;

namespace Cosmoser.PingAnMeetingRequest.Outlook2007
{
    public partial class PingAnMeetingRequestFormRegion
    {
        #region Form Region Factory

        [Microsoft.Office.Tools.Outlook.FormRegionMessageClass("IPM.Appointment.PingAnMeetingRequest")]
        [Microsoft.Office.Tools.Outlook.FormRegionName("Cosmoser.PingAnMeetingRequest.Outlook2007.PingAnMeetingRequestFormRegion")]
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
        SVCMMeeting meeting;

        // Occurs before the form region is displayed.
        // Use this.OutlookItem to get a reference to the current Outlook item.
        // Use this.OutlookFormRegion to get a reference to the form region.
        private void PingAnMeetingRequestFormRegion_FormRegionShowing(object sender, System.EventArgs e)
        {
            this.btnCanhuilingdao.Click += new Outlook.OlkCommandButtonEvents_ClickEventHandler(btnCanhuilingdao_Click);
            this.olkTxtLocation.Click += new Outlook.OlkTextBoxEvents_ClickEventHandler(olkTxtLocation_Click);

            OutlookFacade.Instance().MyRibbon.RibbonType = MyRibbonType.SVCM;

            if (this._apptMgr.GetMeetingIdFromAppointment(this.OutlookItem as Outlook.AppointmentItem) != null)
            {
                meeting = this._apptMgr.GetMeetingFromAppointment(this.OutlookItem as Outlook.AppointmentItem);
                this.txtPassword.Text = meeting.Password;
                if (meeting.Parameter == MeetingParameter.Immediate)
                    this.obtliji.Value = true;
                else
                    this.obtyuyue.Value = true;

            }
            else
            {
                this.meeting = new SVCMMeeting();
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

        // Occurs when the form region is closed.
        // Use this.OutlookItem to get a reference to the current Outlook item.
        // Use this.OutlookFormRegion to get a reference to the form region.
        private void PingAnMeetingRequestFormRegion_FormRegionClosed(object sender, System.EventArgs e)
        {
            OutlookFacade.Instance().MyRibbon.RibbonType = MyRibbonType.Original;
            this.SaveMeetingToAppointment();
        }

        private void SaveMeetingToAppointment()
        {
            meeting.Name = this.olkTxtSubject.Text;
            meeting.Password = this.txtPassword.Text;

            this._apptMgr.SaveMeetingToAppointment(meeting, (this.OutlookItem as Outlook.AppointmentItem));
            
        }
    }
}
