using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PriceManagerDAL;
using System.Configuration;

public partial class pages_setup_Profile : Base
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (RoleCode == "1") // admin
            {
                divUserSelect.Visible = true;
            }
            BindDropdowns();
            fillDetails();
        }
    }

    private void BindDropdowns()
    {
        List<User> users = new DataModelEntities().Users.Where(u => u.Is_Active == true).OrderBy(u => u.Full_Name).ToList();

        Common.BindDropDown(ddlUserList, users, "Full_Name", "User_Code", false, false);
        ddlUserList.SelectedValue = UserKey.ToString();

        for (int i = 1; i <= 9; i++)
        {
            ddlStandardCode.Items.Add("S" + i.ToString());
            ddlExpressCode.Items.Add("X" + i.ToString());
            ddlExpressIntCode.Items.Add("ECM" + i.ToString());
            ddlStandardIntCode.Items.Add("SI" + i.ToString());
            ddlAirMailIntCode.Items.Add("SI" + i.ToString());
            ddlAusPostRegIntCode.Items.Add("RPI" + i.ToString());
        }

        for (int i = 1; i <= 6; i++)
        {
            ddlAirMailIntCode.Items.Add("AIR" + i.ToString());
        }

        ddlStandardCode.Items.Insert(0, "??");
        ddlExpressCode.Items.Insert(0, "??");
        ddlExpressIntCode.Items.Insert(0, "??");
        ddlStandardIntCode.Items.Insert(0, "??");
        ddlAirMailIntCode.Items.Insert(0, "??");
        ddlAusPostRegIntCode.Items.Insert(0, "??");
    }

    protected void btnSaveProfile_Click(object sender, EventArgs e)
    {
        if (IsValid)
        {
            int userCode = int.Parse(ddlUserList.SelectedValue);

            DataModelEntities enities = new DataModelEntities();
            User user = enities.Users.SingleOrDefault(u => u.User_Code == userCode);
            if (user != null)
            {
                string fileName = UploadLogo(user.User_Code.ToString());
                user.Full_Name = f_name.Value + " " + l_name.Value;
                user.First_Name = f_name.Value;
                user.Last_Name = l_name.Value;
                user.Company = company.Value;
                user.Address1 = txtAddress1.Value;
                user.Address2 = txtAddress2.Value;
                user.City = txtSuburb.Value;
                user.State = txtState.Value;
                user.Zip = txtPostcode.Value;
                user.Phone_Number = txtPhone.Value;
                user.Email_Address = txtEmail.Value;
                user.Role_Code = 2; // user;
                user.User_IP = Request.UserHostAddress;
                user.ABN_Number = txtABNNumber.Value;
                user.Charge_Code_Standard = ddlStandardCode.SelectedValue;
                user.Charge_Code_Express = ddlExpressCode.SelectedValue;
                user.Charge_Code_ExpressInt = ddlExpressIntCode.SelectedValue;
                user.Charge_Code_StandardInt = ddlStandardIntCode.SelectedValue;
                user.Charge_Code_AirMailInt = ddlAirMailIntCode.SelectedValue;
                user.Charge_Code_AusPostRegInt = ddlAusPostRegIntCode.SelectedValue;
                user.Flat_Rate_Client = chkFlatRate.Checked;

                if (!string.IsNullOrEmpty(fileName))
                {
                    user.Company_Logo = fileName;
                    imgLogo.ImageUrl = fileName;
                }
                /*Added by javed*/
                user.Minimum_Threshold_Alert = chkMinimumThresholdNotification.Checked;
                user.Floor_Limit_Alert = chkFloorLimitNotification.Checked;
                user.Automation_Time_Delay = int.Parse(ddlTimeDelay.SelectedValue);
                user.Automation_Include_Shipping = chkIncludeShipping.Checked;
                user.Search_Only_Top_Items = int.Parse(ddlSeachOnlyTop.SelectedValue);
                /*Added by javed*/
                enities.SaveChanges();

                lblMsg.CssClass = "profileMessage";
                lblMsg.Visible = true;

                lblMsg.Text = "Profile Updated!";
                enities = null;
            }
        }
    }

    private string UploadLogo(string userCode)
    {
        if (fuLogo.HasFile)
        {
            string extension = fuLogo.FileName.Substring(fuLogo.FileName.IndexOf('.') + 1);
            string fileName = "/files/logo/" + userCode + "." + extension;
            fuLogo.SaveAs(Server.MapPath(fileName));
            return fileName;
        }
        else
            return null;
    }

    void fillDetails()
    {
        int userCode = int.Parse(ddlUserList.SelectedValue);

        DataModelEntities enities = new DataModelEntities();
        User user = enities.Users.SingleOrDefault(u => u.User_Code == userCode);
        if (user != null)
        {
            f_name.Value = user.First_Name;
            l_name.Value = user.Last_Name;
            company.Value = user.Company;
            txtAddress1.Value = user.Address1;
            txtAddress2.Value = user.Address2;
            txtSuburb.Value = user.City;
            txtState.Value = user.State;
            txtPostcode.Value = user.Zip;
            txtPhone.Value = user.Phone_Number;
            txtEmail.Value = user.Email_Address;
            imgLogo.ImageUrl = user.Company_Logo;
            txtABNNumber.Value = user.ABN_Number;
            /*Added By Javed*/
            chkMinimumThresholdNotification.Checked = user.Minimum_Threshold_Alert != null ? (bool)user.Minimum_Threshold_Alert : false;
            chkFloorLimitNotification.Checked = user.Floor_Limit_Alert != null ? (bool)user.Floor_Limit_Alert : false;
            ddlTimeDelay.SelectedValue = user.Automation_Time_Delay != null ? user.Automation_Time_Delay.ToString() : "1";
            chkIncludeShipping.Checked = user.Automation_Include_Shipping != null ? (bool)user.Automation_Include_Shipping : false;
            ddlSeachOnlyTop.SelectedValue = user.Search_Only_Top_Items != null ? user.Search_Only_Top_Items.ToString() : "1";
            /*Added By Javed*/
            if (user.Flat_Rate_Client != null)
                chkFlatRate.Checked = user.Flat_Rate_Client.Value;
            if (!string.IsNullOrEmpty(user.Charge_Code_Express))
                ddlExpressCode.SelectedValue = user.Charge_Code_Express;
            if (!string.IsNullOrEmpty(user.Charge_Code_Standard))
                ddlStandardCode.SelectedValue = user.Charge_Code_Standard;
            if (!string.IsNullOrEmpty(user.Charge_Code_ExpressInt))
                ddlExpressIntCode.SelectedValue = user.Charge_Code_ExpressInt;
            if (!string.IsNullOrEmpty(user.Charge_Code_StandardInt))
                ddlStandardIntCode.SelectedValue = user.Charge_Code_StandardInt;
            if (!string.IsNullOrEmpty(user.Charge_Code_AirMailInt))
                ddlAirMailIntCode.SelectedValue = user.Charge_Code_AirMailInt;
            if (!string.IsNullOrEmpty(user.Charge_Code_AusPostRegInt))
                ddlAusPostRegIntCode.SelectedValue = user.Charge_Code_AusPostRegInt;
            enities = null;
        }
    }

    protected void btnChangePass_Click(object sender, EventArgs e)
    {
        lblMsg.Text = "";
        int userCode = int.Parse(ddlUserList.SelectedValue);
        DataModelEntities enities = new DataModelEntities();
        User user = enities.Users.SingleOrDefault(u => u.User_Code == userCode);
        if (user != null)
        {

            if (user.Password == Common.GetHash(oldPassword.Value))
            {
                user.Password = Common.GetHash(password.Value);
                enities.SaveChanges();
                lblMsg.CssClass = "profileMessage";
                lblMsg.Text = "Password Changed!";
                lblMsg.Visible = true;
                enities = null;
            }
            else
            {
                lblMsg.CssClass = "profileMessageError";
                lblMsg.Text = "Old password is not matched!";
                lblMsg.Visible = true;
            }
        }
    }
    protected void lbCloseAccount_Click(object sender, EventArgs e)
    {
        bool result = false;
        DataModelEntities context = new DataModelEntities();
        int userCode = int.Parse(ddlUserList.SelectedValue);
        User user = context.Users.First(u => u.User_Code == userCode);

        if (string.IsNullOrEmpty(user.Paypal_Subscription_ID))
        {
            result = true;
        }
        else
        {
            result = PayPal.CancelSubscription(user.Paypal_Subscription_ID);
        }

        if (result == true)
        {
            // send email to support for manual confirmation of paypal cancellation - Added 19 Sep 2013
            string support = ConfigurationManager.AppSettings["SenderEmailAddress"];

            string emailText = "This is to inform that user '" + FullName + "' (UserCode: " + UserKey.ToString() + ") has just cancelled his/her account. Paypal subscription is automatically cancelled for that user. You are requested to please confirm his account status";

            Email.SendMail(support, "Account Cancellation Copy", emailText, "adeel@aimviz.com");

            user.Is_Paypal_Expired = true;
            context.SaveChanges();

            // remove session before sending to default page.
            Session.Abandon();
            new Base().ExpireCookie();
            Response.Redirect("/default.aspx", true);
        }
        else
        {
            lblMsg.Text = "Some problem occured while cancelling Paypal subscription. Please try later";
            lblMsg.CssClass = "profileMessageError";
            lblMsg.Visible = true;
        }
    }
    protected void ddlUserList_SelectedIndexChanged(object sender, EventArgs e)
    {
        fillDetails();
    }
}