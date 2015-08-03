using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.IO.Compression;
using PriceManagerDAL;

/// <summary>
/// Summary description for Base
/// </summary>
public class Base : System.Web.UI.Page
{
    //public static string EmailTo = ConfigurationManager.AppSettings["ErrorMailTo"];
    //public static string SMTPServer = ConfigurationManager.AppSettings["SMTPServer"];
    //public static string SMTPUsername = ConfigurationManager.AppSettings["SMTPUsername"];
    //public static string SMTPPassword = ConfigurationManager.AppSettings["SMTPPassword"];
    //public static int SMTPPort = System.Convert.ToInt32(ConfigurationManager.AppSettings["SMTPPort"], 10);
    //public static int SMTPTimeout = System.Convert.ToInt32(ConfigurationManager.AppSettings["SMTPTimeout"], 10);
    //public static System.Net.NetworkCredential MyCredentials = new System.Net.NetworkCredential(SMTPUsername, SMTPPassword);
    public string MessageBody;

    public string GetSecurityKey()
    {
        string key = GetCookie("sesk");
        if (key != null)
            return key;
        else
        {
            key = Guid.NewGuid().ToString();
            SaveCookie("sesk", key);
            return key;
        }
    }

    public string FullName
    {
        get { return GetCookie("fullname"); }
        set { SaveCookie("fullname", value); }
    }
    public string EmailAddress
    {
        get { return GetCookie("emailaddress"); }
        set { SaveCookie("emailaddress", value); }
    }
    public string RoleCode
    {
        get { return GetCookie("kirdar"); }
        set { SaveCookie("kirdar", value); }
    }
    public int PackageCode
    {
        get
        {
            if (string.IsNullOrEmpty(GetCookie("hissa")))
                return 0;
            else
                return int.Parse(GetCookie("hissa"));
        }
        set { SaveCookie("hissa", value.ToString()); }
    }

    public string LoginDetailCode
    {
        get { return GetCookie("logindetailcode"); }
        set { SaveCookie("logindetailcode", value); }
    }

    public int UserKey
    {
        get
        {
            if (string.IsNullOrEmpty(GetCookie("userkey")))
                return 0;
            else
                return int.Parse(GetCookie("userkey"));
        }
        set { SaveCookie("userkey", value.ToString()); }
    }

    public void LogError(Exception ex)
    {
        Base objBase = new Base();
        if (ex == null)
            ex = SetExceptionText();

        // Email Error
        //MessageBody = "<div style='font-family:Trebuchet MS; font-size:10pt'>"
        //       + "Page: " + HttpContext.Current.Request.Url.PathAndQuery
        //       + " <br/> User: " + objBase.FirstName + " (" + objBase.UserKey.ToString() + ")"
        //       + " <br/> Time: " + DateTime.Now.ToString("MMM dd, yyyy hh:mm tt")
        //       + " <br/> Host: " + HttpContext.Current.Request.UserHostName
        //       + " <br/> IP Address: " + HttpContext.Current.Request.UserHostAddress
        //       + " <br/><br/> Error:<br/> " + ex.Message
        //       + " <br/><br/> Stack Trace:<br/> " + ex.StackTrace
        //       + "</div>";
        //Application["CustomException"] = ex.Message;
        //System.Threading.Thread t = new System.Threading.Thread(new System.Threading.ThreadStart(SendMail));
        //t.Start();

        // Log In Database
        if (ValidateError(ex.Message))
        {
            Logging.WriteLog(LogType.Error, ex.Message);
        }

    }

    private bool ValidateError(string errorMessage)
    {
        // exclude common backend exceptions
        if (errorMessage == "File does not exist.")
            return false;
        else
            return true;
    }

    private Exception SetExceptionText()
    {
        Exception ex = HttpContext.Current.Server.GetLastError();
        if (ex != null)
        {
            if (ex.GetBaseException() != null)
            {
                ex = ex.GetBaseException();
                // lblError.Text = ex.Message;
            }
        }
        return ex;
    }

    private void SendMail()
    {
        /*System.Net.Mail.MailMessage message = new System.Net.Mail.MailMessage("kankaneh.auto@sbtjapan.com", EmailTo);
        message.Subject = "Kankaneh Error Reporting";
       message.IsBodyHtml = true;
       message.Body = MessageBody;

        System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient(SMTPServer, SMTPPort);
        client.Timeout = SMTPTimeout;
        client.Credentials = MyCredentials;
        client.Send(message);*/
    }

    public void SaveCookie(string strKey, string strValue)
    {
        if (HttpContext.Current.Request.Cookies[strKey] != null)
        {
            HttpContext.Current.Request.Cookies[strKey].Value = strValue;
        }
        else
        {
            HttpCookie cookie = new HttpCookie(strKey);
            cookie.Value = strValue;
            cookie.Expires = DateTime.Now.AddMinutes(120);
            HttpContext.Current.Response.Cookies.Add(cookie);
        }
    }
    public string GetCookie(string strKey)
    {
        if (HttpContext.Current.Request.Cookies[strKey] != null)
        {
            return HttpContext.Current.Request.Cookies[strKey].Value;
        }
        else
            return null;
    }

    public void ExpireCookie()
    {
        HttpRequest req = HttpContext.Current.Request;
        HttpResponse res = HttpContext.Current.Response;
        int count = req.Cookies.Count;
        for (int i = 0; i < count; i++)
        {
            HttpCookie cookie = new HttpCookie(req.Cookies[i].Name);
            cookie.Expires = DateTime.Now.AddDays(-1);
            res.Cookies.Add(cookie);
        }
    }

    public DateTime ConvertDate(object Date)
    {
        string[] f = Date.ToString().Split('/');
        Date = f[1] + "/" + f[0] + "/" + f[2];
        return Convert.ToDateTime(Date);
    }
}

public static class UIExtensions
{
    public static void EnableCompression(this HttpResponse response)
    {
        response.Filter = new GZipStream(response.Filter, CompressionMode.Compress);
        response.AddHeader("Content-Encoding", "gzip");
    }
}