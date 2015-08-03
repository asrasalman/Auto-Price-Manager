using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PriceManagerDAL;

public partial class pages_seller_Pricing : Base
{
    protected void Page_Load(object sender, EventArgs e)
    {

        if (!IsPostBack)
        {
            AutomatePricingSettings();
            BindEBayAccount();
            BindCategory();
            Dictionary<string, int> localTokens = new Dictionary<string, int>();
            EbayServiceBL service = new EbayServiceBL(UserKey);
            foreach (KeyValuePair<int, string> pair in service.UserTokens)
            {
                string result = service.GetUser(pair.Value);

                localTokens.Add(result, pair.Key);
            }

            string tokenJSON = Common.Serialize(localTokens);
            hfTokenJSON.Value = tokenJSON;

           
        }
        
    }

    public void BindEBayAccount()
    {
        DataModelEntities context = new DataModelEntities();
        var ebayAccount = context.UserAccounts.Where(w => w.Is_Active == true && w.User_Code == UserKey && w.Account_Code == (int)Constant.Accounts.Ebay)
                                 .AsEnumerable()
                                 .Select(s => new { s.User_Account_Code, Application_Name = "Ebay Account : " + s.User_Account_Code.ToString() });

        ddlEbayAccount.DataValueField = "User_Account_Code";
        ddlEbayAccount.DataTextField = "Application_Name";
        ddlEbayAccount.DataSource = ebayAccount.ToList();
        ddlEbayAccount.DataBind();
        ddlEbayAccount.Items.Add(new ListItem("All", "0"));
        ddlEbayAccount.SelectedValue = "0";

    }

    public void BindCategory()
    {
        DataModelEntities context = new DataModelEntities();
        var category = context.SellerItems.Where(w => w.Is_Active == true && w.User_Code == UserKey && w.Item_Category_ID != null)
                                 .AsEnumerable()
                                 .Select(s => new { s.Item_Category_ID, s.Item_Category_Name }).Distinct();

        ddlEbayCategory.DataValueField = "Item_Category_ID";
        ddlEbayCategory.DataTextField = "Item_Category_Name";
        ddlEbayCategory.DataSource = category.ToList();
        ddlEbayCategory.DataBind();
        ddlEbayCategory.Items.Add(new ListItem("All", "0"));
        ddlEbayCategory.SelectedValue = "0";

    }

    protected void btnUpdateSettings_Click(object sender, EventArgs e)
    {
        try
        {
            DataModelEntities context = new DataModelEntities();
            var user = context.Users.FirstOrDefault(w => w.User_Code == UserKey);
            if (user != null)
            {
                user.Floor_Limit_Alert = chkFloorLimitNotification.Checked;
                user.Automation_Time_Delay = int.Parse(ddlTimeDelay.SelectedValue);
                user.Automation_Include_Shipping = chkIncludeShipping.Checked;
                user.Search_Only_Top_Items = int.Parse(ddlSeachOnlyTop.SelectedValue);
                context.SaveChanges();
            }
        }
        catch (Exception ex)
        {
        }

    }

    public void AutomatePricingSettings()
    {
        try
        {
            DataModelEntities context = new DataModelEntities();
            var user = context.Users.FirstOrDefault(w => w.User_Code == UserKey);
            if (user != null)
            {
                chkFloorLimitNotification.Checked = user.Floor_Limit_Alert != null ? (bool)user.Floor_Limit_Alert : false;
                ddlTimeDelay.SelectedValue = user.Automation_Time_Delay != null ? user.Automation_Time_Delay.ToString() : "1";
                chkIncludeShipping.Checked = user.Automation_Include_Shipping != null ? (bool)user.Automation_Include_Shipping : false;
                ddlSeachOnlyTop.SelectedValue = user.Search_Only_Top_Items != null ? user.Search_Only_Top_Items.ToString() : "1";
                
            }
        }
        catch (Exception ex)
        {
        }
                                 

    }
}