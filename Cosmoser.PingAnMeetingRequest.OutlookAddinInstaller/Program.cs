using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Cosmoser.PingAnMeetingRequest.OutlookAddinInstaller
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Count() == 1)
            {
                if (args[0] == "/i")
                {
                    Console.WriteLine("开始安装。。。");
                    RegisterInfo info = GetRegisterInfo();
                    Console.WriteLine("Addin描述文件:" + info.Manifest);
                    RegisterHelper.Instance().InstallOutlook(info);
                    Console.WriteLine("安装完成！");

                }
                else if (args[0] == "/u")
                {
                    Console.WriteLine("开始卸载。。。");
                    RegisterInfo info = GetRegisterInfo();
                    RegisterHelper.Instance().UnInstallOutlook(info);
                    Console.WriteLine("卸载完成！");

                }
                else
                {
                    Console.WriteLine("输入参数错误！请用以下命令：");
                    Console.WriteLine("OutlookAddinInstaller /i");
                    Console.WriteLine("OutlookAddinInstaller /u");
                }
            }
            else
            {
                Console.WriteLine("输入参数错误！请用以下命令：");
                Console.WriteLine("OutlookAddinInstaller /i");
                Console.WriteLine("OutlookAddinInstaller /u");
            }
            Console.Write("按任意键结束。。。");
            Console.ReadKey();
        }

        static RegisterInfo GetRegisterInfo()
        {
            //file:///D:/github/Kaihuilo/Cosmoser.kaihuilo.OutlookAddinInstaller/bin/Debug/OutlookAddinInstaller.EXE
            string currentFolder = Assembly.GetExecutingAssembly().CodeBase;
            currentFolder = currentFolder.Remove(currentFolder.LastIndexOf("/"));
            RegisterInfo info = new RegisterInfo("PingAnMeeting.OutlookAddin")
            {
                Description = "PingAnMeeting.OutlookAddin",
                FriendlyName = "Cosmoser.PingAnMeetingRequest.Outlook2010",
                Manifest = string.Format(@"{0}/PingAnMeeting.OutlookAddin.vsto|vstolocal", currentFolder)
            };

            return info;
        }
    }
}
