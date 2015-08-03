using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net.Sockets;
using System.Net.Security;
using System.IO;

public partial class site_Default : Base
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (UserKey != 0)
            {

                //Aatif
                //Response.Redirect("/pages/GettingStarted.aspx");
                
                
            }
        }
        //RoundDownToNearest();
        
    }
    public Double RoundDownToNearest()
    {

        string username = Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes("utrnclin11"));
        string password = Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes("Po1Da7si"));
        TcpClient tcpclient = new TcpClient();
        tcpclient.Connect("mail.marvin-soft.de", 587);

        System.Net.Security.SslStream sslstream = new SslStream(tcpclient.GetStream());
        //sslstream.AuthenticateAsClient("mail.marvin-soft.de");
        // sslstream.LocalCertificate.Import(
        bool flag = sslstream.IsAuthenticated;
        byte[] read = new byte[10024];
        // sslstream.Write(System.Text.Encoding.ASCII.GetBytes("yourYahooID"), 0, System.Text.Encoding.ASCII.GetBytes("satalaj").Length);
        System.IO.StreamWriter sw = new StreamWriter(sslstream);
        sw.WriteLine("EHLO mail.marvin-soft.de");
        sw.WriteLine("AUTH LOGIN");
        sw.WriteLine("utrnclin11");
        //sw.Flush();
        // 
        sw.WriteLine("Po1Da7si");
        sw.Flush();
        System.Threading.Thread.Sleep(2000);
        sslstream.Read(read, 0, 1024);
        System.Threading.Thread.Sleep(3000);
        string str = System.Text.Encoding.ASCII.GetString(read);


        //  System.Threading.Thread.Sleep(2000);

        sw.WriteLine("Mail From:<noreply@marvin-soft.de>");
        //sw.Flush();
        sw.WriteLine("RCPT TO:<jawaid@aimviz.com>");
        //sw.Flush();
        sw.WriteLine("DATA ");
        //sw.Flush();
        sw.WriteLine("This is test message from marvin-soft");
        sw.WriteLine(".");
        //sw.WriteLine("quit");
        sw.Flush();

        sslstream.Read(read, 0, 10024);
        System.Threading.Thread.Sleep(3000);
        str = System.Text.Encoding.ASCII.GetString(read);

        return 2.5;
    }
}