using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using PriceManagerDAL;
using Api = BigCommerce4Net.Api;
using Domain = BigCommerce4Net.Domain;

public partial class pages_ShopConnect : Base
{

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            SetAccounts();
        }
    }

    private void SetAccounts()
    {
        PriceManagerDAL.DataModelEntities context = new PriceManagerDAL.DataModelEntities();

        List<UserAccount> accounts = context.UserAccounts.Where(u => u.User_Code == UserKey && u.Is_Active == true).ToList();

        List<UserAccount> ebayAccounts = accounts.Where(u => u.Account_Code == (int)Constant.Accounts.Ebay).ToList();

        Package package = context.Users.Include("Package").First(u => u.User_Code == UserKey).Package;

        // check if atleast one ebay account is configured for the current user.
        if (ebayAccounts.Count > 0)
        {
            rptEbayConnectedAccounts.DataSource = ebayAccounts;
            rptEbayConnectedAccounts.DataBind();

            ebayConnect.Style["display"] = "none";

            // check if all ebay accounts are used.
            if (package.Ebay_Accounts <= ebayAccounts.Count)
            {
                hdfEbaySettings.Value = "1";
                ebayAddMore.Visible = ebayConnect.Visible = false;
            }
            else
            {
                ebayAddMore.Style["display"] = "block";
                ebayAddMore.InnerText += " (" + Convert.ToString(package.Ebay_Accounts - ebayAccounts.Count) + " Remaining)";
                hfEbayAccountsRemaining.Value = Convert.ToString(package.Ebay_Accounts - ebayAccounts.Count);
            }
        }
        else
        {

        }

        SetAuthURL();

        // check for shopify account
        int shopifyAccounts = accounts.Count(u => u.Account_Code == (int)Constant.Accounts.Shopify);
        if (shopifyAccounts > 0)
        {
            shopifyConnect.Style["display"] = "none";
            shopifyConnected.Style["display"] = "block";
        }

        // check for magento account
        int magentoAccounts = accounts.Count(u => u.Account_Code == (int)Constant.Accounts.Magento);
        if (magentoAccounts > 0)
        {
            magentoConnect.Style["display"] = "none";
            magentoConnected.Style["display"] = "block";
        }

        // check for bigcommerce account
        int bigcommerceAccounts = accounts.Count(u => u.Account_Code == (int)Constant.Accounts.Bigcommerce);
        if (bigcommerceAccounts > 0)
        {
            bigcommerceConnect.Style["display"] = "none";
            bigcommerceConnected.Style["display"] = "block";
        }
    }

    private void SetAuthURL()
    {
        // create the authentication url for ebay oAuth connectivity

        hfSessionID.Value = new EbayServiceBL(UserKey).GetSessionID();
        Session["EbaySessionID"] = hfSessionID.Value;
        string authURL = ConfigurationManager.AppSettings["EbaySignInLink"];
        string ruName = ConfigurationManager.AppSettings["RuName"];
        authURL = authURL.Replace("AND", @"&");
        authURL = authURL.Replace("[RuName]", ruName);
        authURL = authURL.Replace("[SessionID]", hfSessionID.Value);
        hfAuthURL.Value = authURL;
    }

    protected void btnConfirmAuthorization_Click(object sender, EventArgs e)
    {
        if (IsValid)
        {
            string sessionID = hfSessionID.Value;

            try
            {
                // fetch a new token using SessionID
                string token = new EbayServiceBL(UserKey).FetchToken(sessionID);
                if (string.IsNullOrEmpty(token) == false)
                {
                    // save entry in database.
                    DataModelEntities context = new DataModelEntities();

                    // check if user is creating a new account, or updating existing one 
                    if (hfSelectedEbayAccountID.Value == "0")
                    {
                        UserAccount userAccount = new UserAccount();
                        userAccount.User_Code = UserKey;
                        userAccount.Account_Code = (int)Constant.Accounts.Ebay;
                        userAccount.Application_Name = string.Empty;
                        userAccount.Config_Value1 = token;
                        userAccount.Created_Date = DateTime.Now;
                        userAccount.Is_Active = true;
                        userAccount.User_IP = Request.UserHostAddress;

                        context.UserAccounts.AddObject(userAccount);
                    }
                    else
                    {
                        int userAccountCode = Convert.ToInt32(hfSelectedEbayAccountID.Value);
                        UserAccount userAccount = context.UserAccounts.First(u => u.User_Account_Code == userAccountCode);
                        userAccount.Config_Value1 = token;
                        userAccount.Modified_Date = DateTime.Now;
                        userAccount.User_IP = Request.UserHostAddress;
                    }
                    context.SaveChanges();

                    ScriptManager.RegisterClientScriptBlock(this, GetType(), "1", "window.location.reload()", true);
                    lblAuthError.Text = string.Empty;
                }
                else
                {
                    lblAuthError.Text = "Either you have not authorized properly, or the Ebay servers are down for the moment. Please try agian in a while.";
                }
            }
            catch (Exception ex)
            {
                lblAuthError.Text = "Either you have not authorized properly, or the Ebay servers are down for the moment. Please try agian in a while.";
            }

        }
    }
    protected void btnAuthorizationShopify_Click(object sender, EventArgs e)
    {
        if (IsValid)
        {

        }
    }

    protected void btnAuthorizationMagento_Click(object sender, EventArgs e)
    {
        if (IsValid)
        {
            string url = txtMagentoStoreURL.Value.Trim();
            string startSlash = string.Empty;

            if (url.EndsWith("/") == false)
                startSlash = "/";

            if (url.StartsWith("http://") == false)
                url = "http://" + url;
            if (url.ToLower().Contains("index.php") == false)
                url += startSlash + "index.php/api/v2_soap/index/";
            else
                url += startSlash + "api/v2_soap/index/";

            string apiUserID = txtMagentoUserID.Value.Trim();
            string apiKey = txtMagentoKey.Value.Trim();

            bool isValid = new MagentoHelper(UserKey).TestMagentoCredentials(url, apiUserID, apiKey);

            if (isValid == true)
            {

                // save entry in database.
                DataModelEntities context = new DataModelEntities();


                // check if user is creating a new account, or updating existing one 
                if (hfSelectedMagentoAccountID.Value == "0")
                {
                    UserAccount userAccount = new UserAccount();
                    userAccount.User_Code = UserKey;
                    userAccount.Account_Code = (int)Constant.Accounts.Magento;
                    userAccount.Application_Name = url;
                    userAccount.Config_Value1 = apiUserID;
                    userAccount.Config_Value2 = apiKey;
                    userAccount.Created_Date = DateTime.Now;
                    userAccount.Is_Active = true;
                    userAccount.User_IP = Request.UserHostAddress;

                    context.UserAccounts.AddObject(userAccount);
                }
                else
                {
                    int userAccountCode = Convert.ToInt32(hfSelectedMagentoAccountID.Value);
                    UserAccount userAccount = context.UserAccounts.First(u => u.Is_Active == true && u.Account_Code == (int)Constant.Accounts.Magento);
                    userAccount.Application_Name = url;
                    userAccount.Config_Value1 = apiUserID;
                    userAccount.Config_Value2 = apiKey;
                    userAccount.Modified_Date = DateTime.Now;
                    userAccount.User_IP = Request.UserHostAddress;

                }
                context.SaveChanges();

                magentoConnect.Style["display"] = "none";
                magentoConnected.Style["display"] = "block";
                lblMagentoError.Text = string.Empty;
            }
            else
            {
                lblMagentoError.Text = "URL or API credentials are not valid. Please check and try again";
            }
        }
    }

    protected void btnAuthorizationBigcommerce_Click(object sender, EventArgs e)
    {
        if (IsValid)
        {
            string url = txtBigcommerceStoreURL.Value.Trim();
            string startSlash = string.Empty;
            
            if (url.EndsWith("/") == false)
                startSlash = "/";

            if (url.StartsWith("https://") == false)
                url = "https://" + url;
            
            url += startSlash + "api/v2";

            string apiUserID = txtBigcommerceUserID.Value.Trim();
            string apiKey = txtBigcommerceKey.Value.Trim();

            // test provided credentials
            bool isValid = new BigcommerceHelper(UserKey).TestBigcommerceCredential(url, apiUserID, apiKey);

            if (isValid == true)
            {
                // save entry in database.
                DataModelEntities context = new DataModelEntities();

                // check if user is creating a new account, or updating existing one 
                if (hfSelectedBigcommerceAccountID.Value == "0")
                {
                    UserAccount userAccount = new UserAccount();
                    userAccount.User_Code = UserKey;
                    userAccount.Account_Code = (int)Constant.Accounts.Bigcommerce;
                    userAccount.Application_Name = url;
                    userAccount.Config_Value1 = apiUserID;
                    userAccount.Config_Value2 = apiKey;
                    userAccount.Created_Date = DateTime.Now;
                    userAccount.Is_Active = true;
                    userAccount.User_IP = Request.UserHostAddress;
                    context.UserAccounts.AddObject(userAccount);
                }
                else
                {
                    int userAccountCode = Convert.ToInt32(hfSelectedBigcommerceAccountID.Value);
                    UserAccount userAccount = context.UserAccounts.First(u => u.Is_Active == true && u.Account_Code == (int)Constant.Accounts.Bigcommerce);
                    userAccount.Application_Name = url;
                    userAccount.Config_Value1 = apiUserID;
                    userAccount.Config_Value2 = apiKey;
                    userAccount.Modified_Date = DateTime.Now;
                    userAccount.User_IP = Request.UserHostAddress;

                }
                context.SaveChanges();

                bigcommerceConnect.Style["display"] = "none";
                bigcommerceConnected.Style["display"] = "block";
                bigcommerceError.Text = string.Empty;
                txtBigcommerceStoreURL.Value = "";
                txtBigcommerceUserID.Value = "";
                txtBigcommerceKey.Value = "";
            }
            else
            {
                bigcommerceError.Text = "URL or API credentials are not valid. Please check and try again";
            }
        }
    }

    
}