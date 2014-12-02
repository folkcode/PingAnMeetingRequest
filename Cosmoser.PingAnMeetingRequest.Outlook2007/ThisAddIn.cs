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
using System.Windows.Forms;

namespace Cosmoser.PingAnMeetingRequest.Outlook2007
{
    public partial class ThisAddIn
    {
       

        private void ThisAddIn_Startup(object sender, System.EventArgs e)
        {

            OutlookFacade.Instance().StartupOutlook();
            
            //_task.Start();
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

        /// <summary>
        /// Load Ribbon
        /// </summary>
        /// <param name="serviceGuid"></param>
        /// <returns></returns>
        protected override object RequestService(Guid serviceGuid)
        {
            if (serviceGuid == typeof(Office.IRibbonExtensibility).GUID)
            {
                if (OutlookFacade.Instance().MyRibbon == null)
                {
                    OutlookFacade.Instance().MyRibbon = new MyRibbon(Application);
                }
                return OutlookFacade.Instance().MyRibbon;
            }
            return base.RequestService(serviceGuid);
        }

            
    }
}
