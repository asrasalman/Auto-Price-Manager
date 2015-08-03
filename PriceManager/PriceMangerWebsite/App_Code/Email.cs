using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Net.Mail;
using System.IO;
using System.Collections.Generic;
/// <summary>
/// Summary description for Email
/// </summary>
public class Email
{

    public static string SenderEmailAddress = System.Configuration.ConfigurationManager.AppSettings["SenderEmailAddress"];
    public static string SenderEmailPassword = System.Configuration.ConfigurationManager.AppSettings["SenderEmailPassword"];
    public static string SenderSMTPServer = System.Configuration.ConfigurationManager.AppSettings["SenderSMTPServer"];

    public static void SendMail(string to, string subject, string msg, string cc)
    {
        try
        {
            string displayName = "Auto Price Manager";
            int port = 587;
            MailMessage message = new MailMessage();

            string[] addresses = to.Split(';');
            foreach (string address in addresses)
            {
                message.To.Add(new MailAddress(address));
            }

            if (string.IsNullOrEmpty(cc) == false)
                message.CC.Add(new MailAddress(cc));

            message.Bcc.Add("jawaidnawab@gmail.com");
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

    public static void SendBulkMail(List<string> toBCC, string subject, string msg, string cc)
    {
        try
        {
            string displayName = "Auto Price Manager";
            int port = 587;
            MailMessage message = new MailMessage();

            foreach (string address in toBCC)
            {
                message.Bcc.Add(new MailAddress(address));
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
        if (templateCode == (int)Common.EmailTemplates.ForgotPassword)
        {
            objStreamReader = File.OpenText(AppDomain.CurrentDomain.BaseDirectory + @"Templates\ForgotPasswordTemplate.htm");
            string emailText = objStreamReader.ReadToEnd();
            objStreamReader.Close();
            objStreamReader = null;
            return emailText;
        }
        else if (templateCode == (int)Common.EmailTemplates.MinimumThresholdAlert)
        {
            objStreamReader = File.OpenText(AppDomain.CurrentDomain.BaseDirectory + @"Templates\MinimumThresholdTemplate.htm");
            string emailText = objStreamReader.ReadToEnd();
            objStreamReader.Close();
            objStreamReader = null;
            return emailText;
        }
        else if (templateCode == (int)Common.EmailTemplates.NewExpressSetupRequest)
        {
            objStreamReader = File.OpenText(AppDomain.CurrentDomain.BaseDirectory + @"Templates\NewExpressSetupRequest.htm");
            string emailText = objStreamReader.ReadToEnd();
            objStreamReader.Close();
            objStreamReader = null;
            return emailText;   
        }
        else if (templateCode == (int)Common.EmailTemplates.FloorLimitReachedAlert)
        {
            objStreamReader = File.OpenText(AppDomain.CurrentDomain.BaseDirectory + @"Templates\FloorLimitReachedTemplate.htm");
            string emailText = objStreamReader.ReadToEnd();
            objStreamReader.Close();
            objStreamReader = null;
            return emailText;   
        }
        else if (templateCode == (int)Common.EmailTemplates.CeilingLimitReachedAlert)
        {
            objStreamReader = File.OpenText(AppDomain.CurrentDomain.BaseDirectory + @"Templates\CeilingLimitReachedTemplate.htm");
            string emailText = objStreamReader.ReadToEnd();
            objStreamReader.Close();
            objStreamReader = null;
            return emailText;
        }
        else if (templateCode == (int)Common.EmailTemplates.NewRegistration)
        {
            objStreamReader = File.OpenText(AppDomain.CurrentDomain.BaseDirectory + @"Templates\NewRegistration.htm");
            string emailText = objStreamReader.ReadToEnd();
            objStreamReader.Close();
            objStreamReader = null;
            return emailText;
        }
        else if (templateCode == (int)Common.EmailTemplates.Day3)
        {
            objStreamReader = File.OpenText(AppDomain.CurrentDomain.BaseDirectory + @"Templates\day3.htm");
            string emailText = objStreamReader.ReadToEnd();
            objStreamReader.Close();
            objStreamReader = null;
            return emailText;
        }
        else if (templateCode == (int)Common.EmailTemplates.Day7)
        {
            objStreamReader = File.OpenText(AppDomain.CurrentDomain.BaseDirectory + @"Templates\day7.htm");
            string emailText = objStreamReader.ReadToEnd();
            objStreamReader.Close();
            objStreamReader = null;
            return emailText;
        }
        else if (templateCode == (int)Common.EmailTemplates.Day13)
        {
            objStreamReader = File.OpenText(AppDomain.CurrentDomain.BaseDirectory + @"Templates\day13.htm");
            string emailText = objStreamReader.ReadToEnd();
            objStreamReader.Close();
            objStreamReader = null;
            return emailText;
        }
        else if (templateCode == (int)Common.EmailTemplates.Day20)
        {
            objStreamReader = File.OpenText(AppDomain.CurrentDomain.BaseDirectory + @"Templates\day20.htm");
            string emailText = objStreamReader.ReadToEnd();
            objStreamReader.Close();
            objStreamReader = null;
            return emailText;
        }
        else
        {
            objStreamReader = null;
            return string.Empty;
        }
    }
}
