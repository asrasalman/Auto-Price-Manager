using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ShopifyAPIAdapterLibrary;
using System.Data.Entity;
using System.Data.EntityClient;
using PriceManagerDAL;

public partial class pages_shopify_ShopifyConnect : Base
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string shopName = Request.QueryString["inputShopName"];
        // check if 'code' is already in the url , i.e. returning after auth phase 1
        if (Request["code"] != null)
        {
            string token = new ShopifyHelper(UserKey).ExchangeToken(Request["code"], shopName);
            hfToken.Value = token;

            SaveToken(token, shopName);
        }
        else
        {
            string authURL = new ShopifyHelper(UserKey).GetAuthURL(shopName, Request.Url.AbsoluteUri);
            Response.Redirect(authURL);
        }
    }

    private void SaveToken(string token, string shopName)
    {
        string isExistingShop = Request.QueryString["isES"];

        // save token in database
        DataModelEntities context = new DataModelEntities();

        if (isExistingShop == "0") // new user
        {
            UserAccount userAccount = new UserAccount();

            userAccount.User_Code = UserKey;
            userAccount.Account_Code = (int)Constant.Accounts.Shopify;
            userAccount.Application_Name = shopName;
            userAccount.Config_Value1 = token;
            userAccount.Created_Date = DateTime.Now;
            userAccount.Is_Active = true;

            context.UserAccounts.AddObject(userAccount);
        }
        else
        {
            UserAccount userAccount = context.UserAccounts.First(u => u.User_Code == UserKey && u.Is_Active == true && u.Account_Code == (int)Constant.Accounts.Shopify);
            userAccount.Application_Name = shopName; 
            userAccount.Config_Value1 = token;
            userAccount.Modified_Date = DateTime.Now;
            userAccount.User_IP = Request.UserHostAddress;
        }

        context.SaveChanges();
        context = null;
    }
}