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

public partial class paypal_Unsubscribe : Base
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Request["vc"] == null)
            {
                Response.Redirect("~/Default.aspx");
            }
            else
            {
                hfSignedCode.Value = Request["vc"].ToString();
            }
        }
       
    }
    protected void btnUnsubscribe_Click(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(hfSignedCode.Value))
        {
            DataModelEntities enities = new DataModelEntities();
            var user = enities.Users.FirstOrDefault(f => f.Is_Active == true && f.Confirmation_Code == hfSignedCode.Value);
            if (user != null)
            {
                user.Is_Subscribed = false;
                enities.SaveChanges();
            }
        }
        Response.Redirect("~/Default.aspx");
    }
}



