using System;
using System.Data;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Net.Mail;
using System.IO;
using System.Data.Entity;
using PriceManagerDAL;
using System.Data.Objects;


namespace PriceManagerDAL
{
    public class Email
    {

        public static string SenderEmailAddress = System.Configuration.ConfigurationManager.AppSettings["SenderEmailAddress"];
        public static string SenderEmailPassword = System.Configuration.ConfigurationManager.AppSettings["SenderEmailPassword"];
        public static string SenderSMTPServer = System.Configuration.ConfigurationManager.AppSettings["SenderSMTPServer"];

        public void RunFollowUpEmailProcess(bool isLive)
        {
            DataModelEntities context = new DataModelEntities();
            DateTime minDate = DateTime.Today.AddDays(-5);
            List<User> emailUsers = context.Users.Where(u => u.Created_Date > minDate).OrderByDescending(u => u.Created_Date).ToList();

            foreach (User user in emailUsers)
            {
                
                string to = "xtremeleekool@gmail.com"; // for testing purpose
                if (isLive)
                {
                    to = user.Email_Address;
                }
                string subject = string.Empty;
                string body = string.Empty;

                TimeSpan diff = DateTime.Now - user.Created_Date.Value;

                if (diff.Days == 0)
                {
                    continue;
                }

                if (diff.Days == 1) // first mail;
                {
                    subject = "Welcome to Parcel Solutions!";
                    body = Email.GetTemplateString((int)EmailTemplates.FollowUp1);

                }
                else if (diff.Days == 2) // second mail;
                {
                    subject = "eBay to Parcel Solutions 'Custom Labels";
                    body = Email.GetTemplateString((int)EmailTemplates.FollowUp2);
                }
                else if (diff.Days == 3) // third mail;
                {
                    subject = "Parcel Solutions - The 5 steps!";
                    body = Email.GetTemplateString((int)EmailTemplates.FollowUp3);
                }
                else if (diff.Days == 4) // forth mail;
                {
                    subject = "Parcel Solutions - Mastering the 'Part Master'";
                    body = Email.GetTemplateString((int)EmailTemplates.FollowUp4);

                }
                else // fifth Email
                {
                    subject = "Parcel Solutions - Labelling the Labels";
                    body = Email.GetTemplateString((int)EmailTemplates.FollowUp5);
                }
                Email.SendMail(to, subject, body, null);
            }
        }

        // public static int SendMail(string from, string to, string subject, string message)
        public static void SendMail(string to, string subject, string msg, string cc)
        {
            try
            {
                string displayName = "Parcel Solutions";
                int port = 587;
                MailMessage message = new MailMessage();

                string[] addresses = to.Split(';');
                foreach (string address in addresses)
                {
                    message.To.Add(new MailAddress(address));
                }

                if (string.IsNullOrEmpty(cc) == false)
                    message.CC.Add(new MailAddress(cc));

                message.From = new MailAddress(SenderEmailAddress, displayName);
                message.Subject = subject;
                message.Body = msg;
                message.IsBodyHtml = true;
                SmtpClient client = new SmtpClient();
                client.Port = port;
                client.Host = SenderSMTPServer;
                System.Net.NetworkCredential nc = new System.Net.NetworkCredential(SenderEmailAddress, SenderEmailPassword);
                client.EnableSsl = true;
                client.UseDefaultCredentials = false;
                client.Credentials = nc;
                client.Send(message);
            }
            catch (Exception ex)
            {
                // do logging
                return;
            }
        }

        public static string GetTemplateString(int templateCode)
        {
            StreamReader objStreamReader;
            if (templateCode == (int)EmailTemplates.FollowUp1)
            {
                objStreamReader = File.OpenText(AppDomain.CurrentDomain.BaseDirectory + @"Templates\Followup1.htm");
            }
            else if (templateCode == (int)EmailTemplates.FollowUp2)
            {
                objStreamReader = File.OpenText(AppDomain.CurrentDomain.BaseDirectory + @"Templates\Followup2.htm");
            }
            else if (templateCode == (int)EmailTemplates.FollowUp3)
            {
                objStreamReader = File.OpenText(AppDomain.CurrentDomain.BaseDirectory + @"Templates\Followup3.htm");
            }
            else if (templateCode == (int)EmailTemplates.FollowUp4)
            {
                objStreamReader = File.OpenText(AppDomain.CurrentDomain.BaseDirectory + @"Templates\Followup4.htm");
            }
            else if (templateCode == (int)EmailTemplates.FollowUp5)
            {
                objStreamReader = File.OpenText(AppDomain.CurrentDomain.BaseDirectory + @"Templates\Followup5.htm");
            }
            else if (templateCode == (int)EmailTemplates.FollowUp5)
            {
                objStreamReader = File.OpenText(AppDomain.CurrentDomain.BaseDirectory + @"Templates\FloorLimitReachedTemplate.htm");
            }
            else
                objStreamReader = null;

            string emailText = objStreamReader.ReadToEnd();
            objStreamReader.Close();
            objStreamReader = null;
            return emailText;
        }
    }

    public enum EmailTemplates
    {
        FollowUp1 = 1,
        FollowUp2 = 2,
        FollowUp3 = 3,
        FollowUp4 = 4,
        FollowUp5 = 5
    }
}