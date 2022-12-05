using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using Cjwdev.WindowsApi;
using System.IO;
namespace WindowsService
{
    public partial class Service1 : ServiceBase
    {
        private static string appName = "智能藥庫系統(VM Server)";//the path of the exe file
        private string appStartPath = $@"C:\Users\hsonds01\Desktop\智能藥庫系統(VM Server)\智能藥庫系統(VM Server).exe";
        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            System.Timers.Timer timer;
            timer = new System.Timers.Timer();
            timer.Interval = 5000;//設定計時器事件間隔執行時間
            timer.Elapsed += new System.Timers.ElapsedEventHandler(circulation);
            timer.Enabled = true;
        }

        protected override void OnStop()
        {
        }
        private void circulation(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                //appStartPath = "程式路徑";
                IntPtr userTokenHandle = IntPtr.Zero;
                ApiDefinitions.WTSQueryUserToken(ApiDefinitions.WTSGetActiveConsoleSessionId(), ref userTokenHandle);

                ApiDefinitions.PROCESS_INFORMATION procInfo = new ApiDefinitions.PROCESS_INFORMATION();
                ApiDefinitions.STARTUPINFO startInfo = new ApiDefinitions.STARTUPINFO();
                startInfo.cb = (uint)System.Runtime.InteropServices.Marshal.SizeOf(startInfo);

                ApiDefinitions.CreateProcessAsUser(
                    userTokenHandle,
                    @appStartPath,
                    "",
                    IntPtr.Zero,
                    IntPtr.Zero,
                    false,
                    0,
                    IntPtr.Zero,
                    null,
                    ref startInfo,
                    out procInfo);

                if (userTokenHandle != IntPtr.Zero)
                    ApiDefinitions.CloseHandle(userTokenHandle);

                int _currentAquariusProcessId = (int)procInfo.dwProcessId;
            }
            catch (Exception ex)
            {

            }

            bool runFlag = false;
            Process[] myProcesses = Process.GetProcesses();
            foreach (Process myProcess in myProcesses)
            {
                if (myProcess.ProcessName == appName)
                {
                    runFlag = true;
                }

            }

            if (!runFlag)   //如果程式沒有啟動
            {
                Process proc = new Process();
                proc.StartInfo.FileName = appName;
                proc.StartInfo.WorkingDirectory = Path.GetDirectoryName(@appStartPath);
                proc.Start();


            }
        }
    }
}
