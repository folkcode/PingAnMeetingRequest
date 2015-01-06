using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Office = Microsoft.Office.Core;
using Outlook = Microsoft.Office.Interop.Outlook;

namespace Cosmoser.PingAnMeetingRequest.Outlook2010.Menus
{
    public class MenuManager
    {
        private Office.CommandBar menuBar;
        private Office.CommandBarPopup newMenuBar;
        private Office.CommandBarButton buttonOne;
        private Office.CommandBarButton buttonTwo;
        private Office.CommandBarButton button4;
        private string menuTag = "PingAnMeetingRequestMenu";

        private Outlook.Application _application;

        public MyRibbon mRibbon { get; set; }

        public MenuManager(Outlook.Application application)
        {
            this._application = application;
        }

        public void AddMenuBar()
        {
            try
            {
               
                menuBar = this._application.ActiveExplorer().CommandBars.ActiveMenuBar;
                newMenuBar = (Office.CommandBarPopup)menuBar.Controls.Add(Office.MsoControlType.msoControlPopup);
                if (newMenuBar != null)
                {
                    newMenuBar.Caption = "定制会议";
                    newMenuBar.Tag = menuTag;
                    buttonOne = this.CreateMenu(newMenuBar, "预约会议", "booking");
                    buttonOne.Click += new Office._CommandBarButtonEvents_ClickEventHandler(buttonOne_Click);

                    buttonTwo = this.CreateMenu(newMenuBar, "个人会议中心", "MeetingCenter");
                    buttonTwo.Click += new Office._CommandBarButtonEvents_ClickEventHandler(buttonTwo_Click);

                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
        }

        void buttonTwo_Click(Office.CommandBarButton Ctrl, ref bool CancelDefault)
        {
            OutlookFacade.Instance().CalendarFolder.DoMeetingList();
        }

        public void RemoveMenubar()
        {
            // If the menu already exists, remove it. 
            try
            {
                Office.CommandBarPopup foundMenu = (Office.CommandBarPopup)
                    this._application.ActiveExplorer().CommandBars.ActiveMenuBar.
                    FindControl(Office.MsoControlType.msoControlPopup,Type.Missing,menuTag,true,true);
                if (foundMenu != null)
                {
                    foundMenu.Delete(true);
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
        }

        void buttonOne_Click(Office.CommandBarButton Ctrl, ref bool CancelDefault)
        {
            OutlookFacade.Instance().CalendarFolder.DoBookingMeeting();
        }

        private Office.CommandBarButton CreateMenu(Office.CommandBarPopup newMenuBar, string caption, string tag)
        {
            Office.CommandBarButton buttonOne = (Office.CommandBarButton)newMenuBar.Controls.
                    Add(Office.MsoControlType.msoControlButton);
            buttonOne.Style = Office.MsoButtonStyle.
                msoButtonIconAndCaption;
            buttonOne.Caption = caption;
            buttonOne.FaceId = 65;
            buttonOne.Tag = tag;
            return buttonOne;
        }


    }
}
