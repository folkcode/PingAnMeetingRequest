using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using Office = Microsoft.Office.Core;
using Outlook = Microsoft.Office.Interop.Outlook;
using Cosmoser.PingAnMeetingRequest.Outlook2010.Manager;

// TODO:  Follow these steps to enable the Ribbon (XML) item:

// 1: Copy the following code block into the ThisAddin, ThisWorkbook, or ThisDocument class.

//  protected override Microsoft.Office.Core.IRibbonExtensibility CreateRibbonExtensibilityObject()
//  {
//      return new MyRibbon();
//  }

// 2. Create callback methods in the "Ribbon Callbacks" region of this class to handle user
//    actions, such as clicking a button. Note: if you have exported this Ribbon from the Ribbon designer,
//    move your code from the event handlers to the callback methods and modify the code to work with the
//    Ribbon extensibility (RibbonX) programming model.

// 3. Assign attributes to the control tags in the Ribbon XML file to identify the appropriate callback methods in your code.  

// For more information, see the Ribbon XML documentation in the Visual Studio Tools for Office Help.


namespace Cosmoser.PingAnMeetingRequest.Outlook2010
{
    public enum MyRibbonType
    {
        Original,
        SVCM
    }

    [ComVisible(true)]
    public class MyRibbon : Office.IRibbonExtensibility
    {
        internal static Office.IRibbonUI m_Ribbon;
        private Outlook.Application _application;
        private AppointmentManager _apptMgr = new AppointmentManager();


        public MyRibbonType RibbonType
        {
            get;
            set;
        }   

        public MyRibbon(Outlook.Application application)
        {
            this._application = application;
        }

        #region IRibbonExtensibility Members

        public string GetCustomUI(string ribbonID)
        {
            string result = string.Empty;
            if (ribbonID == "Microsoft.Outlook.Appointment")
                result = GetResourceText("Cosmoser.PingAnMeetingRequest.Outlook2010.MyRibbon.xml");
            return result;
        }

        #endregion

        #region Ribbon Callbacks
        //Create callback methods here. For more information about adding callback methods, select the Ribbon XML item in Solution Explorer and then press F1

        public void Ribbon_Load(Office.IRibbonUI ribbonUI)
        {
            m_Ribbon = ribbonUI;
        }

        /// <summary>
        /// tab idMso="TabAppointment"  getVisible="SystemBuildInVisible" 
        /// </summary>
        /// <param name="control"></param>
        /// <returns></returns>
        public bool SystemBuildInVisible(Office.IRibbonControl control)
        {
            if (this.RibbonType != MyRibbonType.Original)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// tab id="BpmCustomTabAppointment" getVisible="GetBpmCustomGroupVisible"
        /// </summary>
        /// <param name="control"></param>
        /// <returns></returns>
        public bool GetSVCMCustomGroupVisible(Office.IRibbonControl control)
        {
            if (this.RibbonType == MyRibbonType.SVCM)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void DoDelete(Office.IRibbonControl control)
        {
            Outlook.AppointmentItem item = Globals.ThisAddIn.Application.ActiveInspector().CurrentItem as Outlook.AppointmentItem;
            this._apptMgr.SetAppointmentDeleted(item, true);
            item.Delete();
        }

        public void DoSaveAndClose(Office.IRibbonControl control)
        {
            Outlook.AppointmentItem item = Globals.ThisAddIn.Application.ActiveInspector().CurrentItem as Outlook.AppointmentItem;
            string message;

            if (this._apptMgr.TryValidateApppointmentUIInput(item, out message))
            {
                Globals.ThisAddIn.Application.ActiveInspector().Close(Outlook.OlInspectorClose.olSave);
            }
            else
            {
                System.Windows.Forms.MessageBox.Show(message);
            }
        }

        #endregion

        #region Helpers

        private static string GetResourceText(string resourceName)
        {
            Assembly asm = Assembly.GetExecutingAssembly();
            string[] resourceNames = asm.GetManifestResourceNames();
            for (int i = 0; i < resourceNames.Length; ++i)
            {
                if (string.Compare(resourceName, resourceNames[i], StringComparison.OrdinalIgnoreCase) == 0)
                {
                    using (StreamReader resourceReader = new StreamReader(asm.GetManifestResourceStream(resourceNames[i])))
                    {
                        if (resourceReader != null)
                        {
                            return resourceReader.ReadToEnd();
                        }
                    }
                }
            }
            return null;
        }

        #endregion
    }
}
