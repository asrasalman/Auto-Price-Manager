using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;

namespace PriceManagerService
{
    public partial class AutoPriceManagerService : ServiceBase
    {
        System.Timers.Timer timerEmail = new System.Timers.Timer(60000); // 1 minute
        System.Timers.Timer timerDownloader = new System.Timers.Timer(1800000); // 30 minutes
        System.Timers.Timer timerUpdateRank = new System.Timers.Timer(3600000); // 60 minutes
        System.Timers.Timer timerUpdatePrice12 = new System.Timers.Timer(43200000); // 12HRS
        System.Timers.Timer timerUpdatePrice24 = new System.Timers.Timer(86400000); // 24HRS
        System.Timers.Timer timerUpdatePrice48 = new System.Timers.Timer(172800000);// 48HRS
        System.Timers.Timer timerUpdateWeekly = new System.Timers.Timer(604800000); // Weekly
        public AutoPriceManagerService()
        {
            InitializeComponent();

            Logging.WriteLog(LogType.Info, "Service Started");
        }

        protected override void OnStart(string[] args)
        {
            Logging.WriteLog(LogType.Info, "Service On-Start Called");

            timerEmail.Enabled = true;
            timerEmail.Elapsed += new System.Timers.ElapsedEventHandler(timerEmail_Elapsed);
            timerEmail.Start();

            timerDownloader.Enabled = true;
            timerDownloader.Elapsed += new System.Timers.ElapsedEventHandler(timerDownloader_Elapsed);
            timerDownloader.Start();
            timerDownloader_Elapsed(null, null);

            timerUpdatePrice12.Enabled = true;
            timerUpdatePrice12.Elapsed += new System.Timers.ElapsedEventHandler(timerUpdatePrice12_Elapsed);
            timerUpdatePrice12.Start();

            timerUpdatePrice24.Enabled = true;
            timerUpdatePrice24.Elapsed += new System.Timers.ElapsedEventHandler(timerUpdatePrice24_Elapsed);
            timerUpdatePrice24.Start();

            timerUpdatePrice48.Enabled = true;
            timerUpdatePrice48.Elapsed += new System.Timers.ElapsedEventHandler(timerUpdatePrice48_Elapsed);
            timerUpdatePrice48.Start();

            timerUpdateWeekly.Enabled = true;
            timerUpdateWeekly.Elapsed += new System.Timers.ElapsedEventHandler(timerUpdatePriceWeekly_Elapsed);
            timerUpdateWeekly.Start();

            timerUpdateRank.Enabled = true;
            timerUpdateRank.Elapsed += new System.Timers.ElapsedEventHandler(timerUpdateRank_Elapsed);
            timerUpdateRank.Start();
        }

        protected override void OnStop()
        {
        }

        private void timerUpdatePrice12_Elapsed(object sender, EventArgs e)
        {
            try
            {
                Logging.WriteLog(LogType.Info, "12 HRS Updating Price.");

                ParcelService.ParcelServiceSoapClient client = new ParcelService.ParcelServiceSoapClient();

                bool result = client.UpdateProductPrice(2);

                client.Close();
            }
            catch (Exception ex)
            {
                Logging.WriteLog(LogType.Error, "Error in Updating Price 12 HRS Tick Method. Detail: " + ex.Message);
            }
        }

        private void timerUpdatePrice24_Elapsed(object sender, EventArgs e)
        {
            try
            {
                Logging.WriteLog(LogType.Info, "24 HRS Updating Price.");

                ParcelService.ParcelServiceSoapClient client = new ParcelService.ParcelServiceSoapClient();

                /*SendEmailNotifications will be fired at every 24 hrs*/
                bool result = client.SendEmailNotifications();


                result = client.UpdateProductPrice(3);
                client.Close();
            }
            catch (Exception ex)
            {
                Logging.WriteLog(LogType.Error, "Error in Updating Price 24 HRS Tick Method. Detail: " + ex.Message);
            }
        }

        private void timerUpdatePrice48_Elapsed(object sender, EventArgs e)
        {
            try
            {
                Logging.WriteLog(LogType.Info, "48 HRS Updating Price.");

                ParcelService.ParcelServiceSoapClient client = new ParcelService.ParcelServiceSoapClient();

                bool result = client.UpdateProductPrice(4);

                client.Close();
            }
            catch (Exception ex)
            {
                Logging.WriteLog(LogType.Error, "Error in Updating Price 48 HRS Tick Method. Detail: " + ex.Message);
            }
        }

        private void timerUpdatePriceWeekly_Elapsed(object sender, EventArgs e)
        {
            try
            {
                Logging.WriteLog(LogType.Info, "Weekly Updating Price.");

                ParcelService.ParcelServiceSoapClient client = new ParcelService.ParcelServiceSoapClient();

                bool result = client.UpdateProductPrice(5);

                client.Close();
            }
            catch (Exception ex)
            {
                Logging.WriteLog(LogType.Error, "Error in Updating Price Weekly Tick Method. Detail: " + ex.Message);
            }
        }

        private void timerDownloader_Elapsed(object sender, EventArgs e)
        {
            try
            {
                Logging.WriteLog(LogType.Info, "Downloading Transactions.");

                ParcelService.ParcelServiceSoapClient client = new ParcelService.ParcelServiceSoapClient();

                bool result = client.RefreshTransactions();

                client.Close();
            }
            catch (Exception ex)
            {
                Logging.WriteLog(LogType.Error, "Error in Downloader Tick Method. Detail: " + ex.Message);
            }
        }

        private void timerEmail_Elapsed(object sender, EventArgs e)
        {
            try
            {
                TimeSpan emailTime = TimeSpan.Parse(System.Configuration.ConfigurationManager.AppSettings["EmailTime"]);
                if (DateTime.Now.Hour == emailTime.Hours && DateTime.Now.Minute == emailTime.Minutes)
                {
                    Logging.WriteLog(LogType.Info, "Time Matched.");
                    // Run the process
                    bool isLive = bool.Parse(System.Configuration.ConfigurationManager.AppSettings["IsLive"]);
                    new PriceManagerDAL.Email().RunFollowUpEmailProcess(isLive);
                }
            }
            catch (Exception ex)
            {
                Logging.WriteLog(LogType.Error, "Error in Tick Method. Detail: " + ex.Message);
            }
        }

        private void timerUpdateRank_Elapsed(object sender, EventArgs e)
        {
            try
            {
                Logging.WriteLog(LogType.Info, "Updating Item Rank Transactions.");

                ParcelService.ParcelServiceSoapClient client = new ParcelService.ParcelServiceSoapClient();

                bool result = client.UpdateProductRank();

                client.Close();
            }
            catch (Exception ex)
            {
                Logging.WriteLog(LogType.Error, "Error in Updating Item Rank Tick Method. Detail: " + ex.Message);
            }
        }
    }
}
