using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace Tectransit.Service
{
    class Program
    {
        /// <summary>
        /// 應用程式的主要進入點。
        /// </summary>
        static void Main()
        {
            //ServiceBase[] ServicesToRun;
            //ServicesToRun = new ServiceBase[]
            //{
            //    new Service1()
            //};
            //ServiceBase.Run(ServicesToRun);
            if (Environment.UserInteractive)
            {
                Service1 s = new Service1();
                s.start(null);
                Console.WriteLine("服務已啟動，請按下 Enter 鍵關閉服務...");
                // 必須要透過 Console.ReadLine(); 先停止程式執行
                // 因為 Windows Service 大多是利用多 Thread 或 Timer 執行長時間的工作
                // 所以雖然主執行緒停止執行了，但服務中的執行緒已經在運行了!
                Console.ReadLine();
                s.Stop();
                Console.WriteLine("服務已關閉");
            }
            else
            {
                ServiceBase[] ServicesToRun;
                ServicesToRun = new ServiceBase[]
                {
                    new Service1()
                };
                ServiceBase.Run(ServicesToRun);
            }
        }
    }
}
