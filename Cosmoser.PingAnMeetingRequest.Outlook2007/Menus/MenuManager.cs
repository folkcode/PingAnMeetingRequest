using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Office = Microsoft.Office.Core;
using Outlook = Microsoft.Office.Interop.Outlook;

namespace Cosmoser.PingAnMeetingRequest.Outlook2007.Menus
{
    public class MenuManager
    {
        private Office.CommandBar menuBar;
        private Office.CommandBarPopup newMenuBar;
        private Office.CommandBarButton buttonOne;
        private Office.CommandBarButton buttonTwo;
        private Office.CommandBarButton buttonThree;
        private Office.CommandBarButton button4;
        private string menuTag = "PingAnMeetingRequestMenu";

        private Outlook.Application _application;

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
                    buttonOne = this.CreateMenu(newMenuBar, "预约会议", "yuyue");
                    buttonOne.Click += new Office._CommandBarButtonEvents_ClickEventHandler(buttonOne_Click);

                    buttonTwo = this.CreateMenu(newMenuBar, "修改会议", "");
                    buttonTwo.Click += new Office._CommandBarButtonEvents_ClickEventHandler(buttonTwo_Click);

                    buttonThree = this.CreateMenu(newMenuBar, "删除会议", "");
                    buttonThree.Click += new Office._CommandBarButtonEvents_ClickEventHandler(buttonThree_Click);


                    button4 = this.CreateMenu(newMenuBar, "会议列表查询", "");
                    button4.Click += new Office._CommandBarButtonEvents_ClickEventHandler(button4_Click);
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
        }

        void button4_Click(Office.CommandBarButton Ctrl, ref bool CancelDefault)
        {
            throw new NotImplementedException();
        }

        void buttonThree_Click(Office.CommandBarButton Ctrl, ref bool CancelDefault)
        {
            throw new NotImplementedException();
        }

        void buttonTwo_Click(Office.CommandBarButton Ctrl, ref bool CancelDefault)
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
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
