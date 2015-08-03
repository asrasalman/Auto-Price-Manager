using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net.Mail;
using System.IO;
using System.Net;
using System.Text;
using System.Globalization;
using System.Text.RegularExpressions;
using PriceManagerDAL;

public partial class paypal_PaymentFailure : Base
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Request["custom"] == null)
            {
                Response.Redirect("~/pages/login.aspx");
            }
            else
            {
                hfSignedCode.Value = Request["custom"].ToString();
            }
        }
       
    }
    protected void btnMakePayment_Click(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(hfSignedCode.Value))
        {

            DataModelEntities enities = new DataModelEntities();
            var user = enities.Users.FirstOrDefault(f => f.Is_Active == true && f.Confirmation_Code == hfSignedCode.Value);
            if (user != null)
            {
                Response.Redirect(PayPal.GetPayPalURL(user.Confirmation_Code, (int)user.Package_Id));
            }
        }
        else
        {
            Response.Redirect("~/pages/login.aspx");
        }
    }
}



