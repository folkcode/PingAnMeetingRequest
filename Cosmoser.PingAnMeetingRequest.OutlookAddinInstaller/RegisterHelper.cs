using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Win32;

namespace Cosmoser.PingAnMeetingRequest.OutlookAddinInstaller
{
    public class RegisterHelper
    {
        private static RegisterHelper _registerHelper = new RegisterHelper();

        private RegisterHelper()
        {
        }

        public static RegisterHelper Instance()
        {
            return _registerHelper;
        }
        /// <summary>
        /// register the addin in outlook
        /// </summary>
        /// <returns></returns>
        public void InstallOutlook(RegisterInfo registerInfo)
        {
            RegistryKey bk = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Office\Outlook\Addins", true);
            Console.WriteLine("创建注册表项" + registerInfo.KeyName);
            RegistryKey bkk = bk.CreateSubKey(registerInfo.KeyName);
            bkk.SetValue("CommandLineSafe", 1);
            bkk.SetValue("Description", registerInfo.Description);
            bkk.SetValue("FriendlyName", registerInfo.FriendlyName);
            bkk.SetValue("LoadBehavior", 3);
            bkk.SetValue("Manifest", registerInfo.Manifest);
            bkk.Close();
            bk.Close();
            Console.WriteLine("安装FormRegion。。。");
            this.InstallFormRegion(registerInfo);

        }

        public void UnInstallOutlook(RegisterInfo registerInfo)
        {
            Console.WriteLine("删除注册表项" + registerInfo.KeyName);
            RegistryKey bk = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Office\Outlook\Addins", true);
            if (bk.OpenSubKey(registerInfo.KeyName) != null)
            {
                bk.DeleteSubKeyTree(registerInfo.KeyName);                
            }
            Console.WriteLine("删除FormRegion。。。");
            this.UnInstallFormRegion();
            
        }

        public void InstallFormRegion(RegisterInfo registerInfo)
        {
            RegistryKey bk = null;
            bk = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Office\Outlook\FormRegions", true);
            if (bk == null)
            {
                RegistryKey ok = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Office\Outlook", true);
                bk = ok.CreateSubKey("FormRegions");              
            }

            if (bk.OpenSubKey("IPM.Appointment.PingAnMeetingRequest") == null)
            {
                RegistryKey akk = bk.CreateSubKey("IPM.Appointment.PingAnMeetingRequest");
                akk.SetValue("Cosmoser.PingAnMeetingRequest.Outlook2010.PingAnMeetingRequestFormRegion", "=" + registerInfo.KeyName);
                akk.Close();
            }
           
            bk.Close();
        }

        public void UnInstallFormRegion()
        {
            RegistryKey bk = null;
            bk = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Office\Outlook\FormRegions", true);
            if (bk == null)
            {
                RegistryKey ok = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Office\Outlook", true);
                bk = ok.CreateSubKey("FormRegions");               
            }

            if (bk.OpenSubKey("IPM.Appointment.PingAnMeetingRequest") != null)
            {
                bk.DeleteSubKeyTree("IPM.Appointment.PingAnMeetingRequest");
            }

            bk.Close();
        }   
          
        
    }
}
