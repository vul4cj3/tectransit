using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Tectransit.Service.Library;
using static Tectransit.Service.Library.Tools;

namespace Tectransit.Service
{
    partial class Tectrans : ServiceBase
    {
        private List<mission> listMission = new List<mission>();

        public Tectrans()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            writeLog("Service啟動");

            //廠商匯入&異動資料(未入庫)拋轉到厚生倉
            listMission.Add(new Bussiness.TRANSDEPOT());
            //廠商託運單號(嘉里大榮)拋轉到台空貨況
            listMission.Add(new Bussiness.TRANSTECECO());

            writeLog("Service結束");
        }

        protected override void OnStop()
        {
            // TODO: 在此加入停止服務所需執行的終止程式碼。
        }

        private void Run()
        {
            try
            {
                writeLog("timer 觸發");

                timer1.AutoReset = false;
                for (int i = 0; i < listMission.Count(); i++)
                {
                    writeLog(listMission[i]._Name + " 起始狀態 " + (listMission[i].IsRunning == true ? "執行中" : "休息中"));
                    if (listMission[i].IsRunning == false)
                    {
                        listMission[i].IsRunning = true;
                        Thread newThread = new Thread(listMission[i].runn);
                        newThread.Start();
                    }
                    writeLog(listMission[i]._Name + " 結束狀態 " + (listMission[i].IsRunning == true ? "執行中" : "休息中"));
                }

                writeLog("timer 結束");
            }
            catch (Exception ex)
            {
                writeLog(ex.ToString());
            }
            finally
            {
                timer1.AutoReset = true;
            }
        }

        private void timer1_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            Run();
        }

        public void start(string[] args)
        {
            this.OnStart(args);

        }

        public void stop()
        {
            this.OnStop();
        }
    }
}
