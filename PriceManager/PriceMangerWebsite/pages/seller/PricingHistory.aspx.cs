using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PriceManagerDAL;
using System.Data;

public partial class pages_seller_PricingHistory : Base
{
    protected void Page_Load(object sender, EventArgs e)
    {

        if (!IsPostBack)
        {
            if (Request.QueryString["ItemCode"] != null)
            {
                hfItemCode.Value = Request.QueryString["ItemCode"];
            }
            else
                Response.Redirect("~/pages/seller/pricing.aspx");
        }
        
    }

    
}