using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Outlook = Microsoft.Office.Interop.Outlook;
using Office = Microsoft.Office.Core;
using log4net;
using System.Threading.Tasks;
using Cosmoser.PingAnMeetingRequest.Common.Scheduler;
using System.Threading;

namespace Cosmoser.PingAnMeetingRequest.Outlook2007
{
    public partial class ThisAddIn
    {
        private static ILog logger = LogManager.GetLogger(typeof(ThisAddIn));
        private WrapTask _task = null;

        private void ThisAddIn_Startup(object sender, System.EventArgs e)
        {
            logger.Info("ThisAddIn_Startup");
            this.Application.ItemContextMenuDisplay += new Outlook.ApplicationEvents_11_ItemContextMenuDisplayEventHandler(Application_ItemContextMenuDisplay);
            this.Application.ViewContextMenuDisplay += new Outlook.ApplicationEvents_11_ViewContextMenuDisplayEventHandler(Application_ViewContextMenuDisplay);
            this.Application.Inspectors.NewInspector += new Outlook.InspectorsEvents_NewInspectorEventHandler(Inspectors_NewInspector);

            Menus.MenuManager menuMgr = new Menus.MenuManager(this.Application);
            menuMgr.RemoveMenubar();
            menuMgr.AddMenuBar();

            
            //_task.Start();
        }

        void Inspectors_NewInspector(Outlook.Inspector Inspector)
        {
            Outlook.AppointmentItem appointmentItem = Inspector.CurrentItem as Outlook.AppointmentItem;

        }

        void Application_ViewContextMenuDisplay(Office.CommandBar CommandBar, Outlook.View View)
        {
            if (View.ViewType == Microsoft.Office.Interop.Outlook.OlViewType.olCalendarView)
            {
                var meetingMenu = CommandBar.Controls.Add(Office.MsoControlType.msoControlButton, Type.Missing, Type.Missing, 1, false) as Office.CommandBarButton;
                meetingMenu.Caption = "新建平安会议预约";

                meetingMenu.Click += new Office._CommandBarButtonEvents_ClickEventHandler(meetingMenu_Click);
            }
        }

        void meetingMenu_Click(Office.CommandBarButton Ctrl, ref bool CancelDefault)
        {
            Outlook.MAPIFolder currentFolder = Globals.ThisAddIn.Application.ActiveExplorer().CurrentFolder;

            if (currentFolder.CurrentView.ViewType == Microsoft.Office.Interop.Outlook.OlViewType.olCalendarView)
            {
                //set holiday ribbon
                //this._ribbon.RibbonType = BpmRibbonType.Holiday;

                //Create a holiday appointmet and set properties
                Outlook.AppointmentItem apptItem = (Outlook.AppointmentItem)currentFolder.Items.Add("IPM.Appointment.PingAnMeetingRequest");

                //display the appointment
                Outlook.Inspector inspect = Globals.ThisAddIn.Application.Inspectors.Add(apptItem);
                inspect.Display(false);
                //reset the ribbon to normal
                //this._ribbon.RibbonType = BpmRibbonType.original;
            }
        }

        void Application_ItemContextMenuDisplay(Office.CommandBar CommandBar, Outlook.Selection Selection)
        {

        }

        private void ThisAddIn_Shutdown(object sender, System.EventArgs e)
        {
        }

        #region VSTO generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InternalStartup()
        {
            this.Startup += new System.EventHandler(ThisAddIn_Startup);
            this.Shutdown += new System.EventHandler(ThisAddIn_Shutdown);
        }

        #endregion

        private void Log(object state)
        {
            string name = "No use";
            if(Application.Session.Accounts.Count > 0)
              name = Application.Session.Accounts[0].DisplayName;
            logger.Info(string.Format("Current time: {0} , current user: {1}", DateTime.Now,name));
        }        
    }
}
