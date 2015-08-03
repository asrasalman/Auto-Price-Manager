using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PriceManagerDAL;

public partial class master_Main : System.Web.UI.MasterPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Response.EnableCompression();
        Base baseObj = new Base();
        if (baseObj.UserKey > 0)
        {
            lnkLogin.Text = "My Account";
            lnkLogin.NavigateUrl = "/pages/setup/profile.aspx";
            lnkSignout.Visible = true;
            ulLogout.Visible = true;
        }
        else
        {
            lnkLogin.Text = "Login";
            lnkLogin.NavigateUrl = "/pages/login.aspx";
            lnkSignout.Visible = false;
            ulLogout.Visible = false;
        }
    }

    protected void lnkSignout_Click(object sender, EventArgs e)
    {
        // remove session before sending to login page.
        Base baseClass = new Base();
        if (!string.IsNullOrEmpty(baseClass.LoginDetailCode))
        {
            DataModelEntities context = new DataModelEntities();
            int LoginDetailCode = int.Parse(new Base().LoginDetailCode);
            LoginDetail ld = context.LoginDetails.FirstOrDefault(l => l.Login_Detail_Code == LoginDetailCode);

            ld.User_Code = baseClass.UserKey;
            ld.Browser = Request.Browser.Browser;
            ld.Operating_System = Request.Browser.Platform;
            ld.Logout_Date_Time = System.DateTime.Now;
            ld.Created_By = baseClass.UserKey;
            ld.Created_Date = ld.Login_Date_Time;
            ld.User_IP = Request.UserHostAddress;
            context.SaveChanges();
        }

        Session.Abandon();
        new Base().ExpireCookie();
        Response.Redirect("/default.aspx", true);
    }
}
