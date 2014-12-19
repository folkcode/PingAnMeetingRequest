using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Outlook = Microsoft.Office.Interop.Outlook;
using Office = Microsoft.Office.Core;
using log4net;
using Cosmoser.PingAnMeetingRequest.Common.Scheduler;
using Cosmoser.PingAnMeetingRequest.Outlook2010.Manager;
using Cosmoser.PingAnMeetingRequest.Common.Model;
using Cosmoser.PingAnMeetingRequest.Common.ClientService;
using System.Threading.Tasks;

namespace Cosmoser.PingAnMeetingRequest.Outlook2010
{
    public class OutlookFacade
    {
        private Outlook.Explorer _activeExplorer;
        private MyRibbon _ribbon;
        private Outlook.Application Application = Globals.ThisAddIn.Application;

        private static ILog logger = LogManager.GetLogger(typeof(OutlookFacade));
        private WrapTask _task = null;
        Menus.MenuManager _menuMgr = null;
        private CalendarFolder _calendarFolder;
        private HandlerSession _session;

        public HandlerSession Session
        {
            get
            {
                return this._session;
            }
        }

        public MyRibbon MyRibbon
        {
            get
            {
                return this._ribbon;
            }
            set
            {
                this._ribbon = value;
            }
        }

        public MeetingDetailData MeetingDetaiData
        {
            get
            {
                return this._calendarFolder.CalendarDataManager.MeetingDetailDataLocal;
            }
        }

        public Outlook.Explorer CurrentExplorer
        {
            get
            {
                return this._activeExplorer;
            }
        }

        public CalendarFolder CalendarFolder
        {
            get { return this._calendarFolder; }
        }

        private static OutlookFacade _outlookFacade;
        public static OutlookFacade Instance()
        {
            if (_outlookFacade == null)
            {
                _outlookFacade = new OutlookFacade();
            }
            return _outlookFacade;
        }

        public void StartupOutlook()
        {
            logger.Info("ThisAddIn_Startup");
            this.InitializeSession();

            this._activeExplorer = Globals.ThisAddIn.Application.ActiveExplorer();
            Globals.ThisAddIn.Application.ItemContextMenuDisplay += new Outlook.ApplicationEvents_11_ItemContextMenuDisplayEventHandler(Application_ItemContextMenuDisplay);
            Globals.ThisAddIn.Application.ViewContextMenuDisplay += new Outlook.ApplicationEvents_11_ViewContextMenuDisplayEventHandler(Application_ViewContextMenuDisplay);
            Globals.ThisAddIn.Application.Inspectors.NewInspector += new Outlook.InspectorsEvents_NewInspectorEventHandler(Inspectors_NewInspector);
            this._activeExplorer.FolderSwitch += new Outlook.ExplorerEvents_10_FolderSwitchEventHandler(_activeExplorer_FolderSwitch);

            this._calendarFolder = new CalendarFolder();
            this._calendarFolder.Initialize();
        }

        void _activeExplorer_FolderSwitch()
        {
            this.CalendarFolder.CalendarDataManager.SyncMeetingList();
        }

        private void InitializeSession()
        {
            this._session = new HandlerSession();
            this._session.UserName = this.Application.Session.CurrentUser.Name;
            this._session.IP = System.Configuration.ConfigurationManager.AppSettings["IP"];
            this._session.Port = System.Configuration.ConfigurationManager.AppSettings["Port"];
            this._session.UserName = System.Configuration.ConfigurationManager.AppSettings["Username"]; ;
            ClientServiceFactory.Create().Login(ref this._session);
        }

        void Inspectors_NewInspector(Outlook.Inspector Inspector)
        {
            Outlook.AppointmentItem appointmentItem = Inspector.CurrentItem as Outlook.AppointmentItem;

            if (appointmentItem != null)
            {
                if (appointmentItem.MessageClass == "IPM.Appointment.PingAnMeetingRequest")
                    this.MyRibbon.RibbonType = MyRibbonType.SVCM;
                else
                    this.MyRibbon.RibbonType = MyRibbonType.Original;
            }

            if (MyRibbon.m_Ribbon != null)
                MyRibbon.m_Ribbon.Invalidate();
        }

        void Application_ViewContextMenuDisplay(Office.CommandBar CommandBar, Outlook.View View)
        {
            if (this.MyRibbon == null)
                this.MyRibbon = new MyRibbon(this.Application);

            if (View.ViewType == Microsoft.Office.Interop.Outlook.OlViewType.olCalendarView)
            {
                var meetingMenu = CommandBar.Controls.Add(Office.MsoControlType.msoControlButton, Type.Missing, Type.Missing, 4, false) as Office.CommandBarButton;
                meetingMenu.Caption = "SVCM会议预约";

                meetingMenu.Click += new Office._CommandBarButtonEvents_ClickEventHandler(meetingMenu_Click);
            }
        }

        void meetingMenu_Click(Office.CommandBarButton Ctrl, ref bool CancelDefault)
        {
            Outlook.MAPIFolder currentFolder = Globals.ThisAddIn.Application.ActiveExplorer().CurrentFolder;

            if (currentFolder.CurrentView.ViewType == Microsoft.Office.Interop.Outlook.OlViewType.olCalendarView)
            {
                //set SVCM ribbon
                this.MyRibbon.RibbonType = MyRibbonType.SVCM;

                //Create a holiday appointmet and set properties
                Outlook.AppointmentItem apptItem = (Outlook.AppointmentItem)currentFolder.Items.Add("IPM.Appointment.PingAnMeetingRequest");

                //display the appointment
                Outlook.Inspector inspect = Globals.ThisAddIn.Application.Inspectors.Add(apptItem);
                inspect.Display(false);
                //reset the ribbon to normal
                this.MyRibbon.RibbonType = MyRibbonType.Original;
            }
        }

        void Application_ItemContextMenuDisplay(Office.CommandBar CommandBar, Outlook.Selection Selection)
        {

        }

        private void Log(object state)
        {
            string name = "No use";
            if (Application.Session.Accounts.Count > 0)
                name = Application.Session.Accounts[0].DisplayName;
            logger.Info(string.Format("Current time: {0} , current user: {1}", DateTime.Now, name));
        }

        

    }
}
