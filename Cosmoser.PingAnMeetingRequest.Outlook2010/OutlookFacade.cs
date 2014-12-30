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
using Cosmoser.PingAnMeetingRequest.Common.Utilities;
using System.Windows.Forms;

namespace Cosmoser.PingAnMeetingRequest.Outlook2010
{
    public class OutlookFacade
    {
        private Outlook.Explorer _activeExplorer;
        private MyRibbon _ribbon;
        private Outlook.Application Application = Globals.ThisAddIn.Application;

        private static ILog logger = IosLogManager.GetLogger(typeof(OutlookFacade));
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
            try
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
            catch (Exception ex)
            {
                logger.Error("ThisAddIn_Startup 启动失败！" + ex.Message + "\n" + ex.StackTrace);
            }
        }

        void _activeExplorer_FolderSwitch()
        {
            try
            {
                if (this._activeExplorer.CurrentFolder.EntryID == OutlookFacade.Instance().CalendarFolder.MAPIFolder.EntryID 
                    || this._activeExplorer.CurrentFolder.Name == "日历" || this._activeExplorer.CurrentFolder.Name == "Calendar")
                    this.CalendarFolder.CalendarDataManager.SyncMeetingList();
            }
            catch (Exception ex)
            {
                logger.Error("切换到日历同步数据错误！" + ex.Message + "\n" + ex.StackTrace);
            }
        }

        private void InitializeSession()
        {
            try
            {
                Task task = Task.Factory.StartNew(() =>
                {
                    this._session = new HandlerSession();
                    this._session.UserName = System.Configuration.ConfigurationManager.AppSettings["Username"];

                    //var currentUser = this.Application.Session.CurrentUser.AddressEntry.GetExchangeUser();
                    //if(currentUser != null)
                    //    this._session.UserName = this.Application.Session.CurrentUser.AddressEntry.GetExchangeUser().PrimarySmtpAddress.Split("@".ToArray())[0];
                    this._session.IP = System.Configuration.ConfigurationManager.AppSettings["IP"];
                    this._session.Port = System.Configuration.ConfigurationManager.AppSettings["Port"];

                    ClientServiceFactory.Create().Login(ref this._session);

                    //if(currentUser == null)
                    //MessageBox.Show("找不到Exchange账号！");
                });
            }
            catch (Exception ex)
            {
                logger.Error("InitializeSession failed!", ex);
            }
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
                meetingMenu.Caption = "电子会议预约";

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

                Outlook.CalendarView calView = currentFolder.CurrentView as Outlook.CalendarView;

                //Create a holiday appointmet and set properties
                Outlook.AppointmentItem apptItem = (Outlook.AppointmentItem)currentFolder.Items.Add("IPM.Appointment.PingAnMeetingRequest");
                //设置选中日期时间
                if (calView.SelectedStartTime.Date != DateTime.Today)
                    apptItem.Start = calView.SelectedStartTime.AddHours(8);

                //display the appointment
                Outlook.Inspector inspect = Globals.ThisAddIn.Application.Inspectors.Add(apptItem);
                inspect.Display(false);
                //reset the ribbon to normal
                this.MyRibbon.RibbonType = MyRibbonType.Original;
            }
        }

        void Application_ItemContextMenuDisplay(Office.CommandBar CommandBar, Outlook.Selection Selection)
        {
            //if (CurrentExplorer.CurrentFolder.StoreID == this.CalendarFolder.MAPIFolder.StoreID)
            //{
            //    var meetingDetailMenu = CommandBar.Controls.Add(Office.MsoControlType.msoControlButton, Type.Missing, Type.Missing, 4, false) as Office.CommandBarButton;
            //    meetingDetailMenu.Caption = "查看电子会议详情";

            //    meetingDetailMenu.Click += new Office._CommandBarButtonEvents_ClickEventHandler(meetingDetailMenu_Click);
            //}
        }

        void meetingDetailMenu_Click(Office.CommandBarButton Ctrl, ref bool CancelDefault)
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
