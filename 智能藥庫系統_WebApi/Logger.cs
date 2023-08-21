using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace 智慧藥庫系統_WebApi
{
    public class Logger
    {
        private readonly string logPath;
        private static object lockObj = new object();

        public Logger(IWebHostEnvironment env)
        {
            logPath = Path.Combine(env.ContentRootPath, "Logs");
        }

        public void WriteLog(string Name ,string message)
        {
            lock (lockObj)
            {
                if (!Directory.Exists(logPath))
                {
                    Directory.CreateDirectory(logPath);
                }

                string fileName = $"{Name}-" + DateTime.Now.ToString("yyyyMM") + ".log";
                string fullPath = Path.Combine(logPath, fileName);

                using (StreamWriter sw = new StreamWriter(fullPath, true))
                {
                    sw.WriteLine($"[{DateTime.Now:G}]\n{message}");
                }
            }
        }
    }
}
