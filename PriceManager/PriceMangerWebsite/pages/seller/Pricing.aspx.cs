using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PriceManagerDAL;
using System.Data;

public partial class pages_seller_Pricing : Base
{
    protected void Page_Load(object sender, EventArgs e)
    {

        if (!IsPostBack)
        {
            
            Dictionary<string, int> localTokens = new Dictionary<string, int>();
            EbayServiceBL service = new EbayServiceBL(UserKey);
            if (service.UserTokens != null)
            {
                string userNames = "";
                foreach (KeyValuePair<int, string> pair in service.UserTokens)
                {
                    string result = service.GetUser(pair.Value);
                    localTokens.Add(result, pair.Key);
                    userNames = result + ",";
                }

                hfSeller.Value = "";//userNames.Substring(0, userNames.Length - 1);
                string tokenJSON = Common.Serialize(localTokens);
                hfTokenJSON.Value = tokenJSON;
                AutomatePricingSettings();
                BindEBayAccount();
                BindCategory();
                BindSettings();
                BindCountry();
            }
            else
            {
                Response.Redirect("~/pages/shopconnect.aspx", true);
            }

            

           
        }
        
    }

    protected void BindCountry()
    {
        DataModelEntities enities = new DataModelEntities();
        var countries = enities.Countries.Where(w => w.Is_Ebay_Site == true).ToList();
        ddlCountry.DataValueField = "Country_Abbr";
        ddlCountry.DataTextField = "Country_Name";
        ddlCountry.DataSource = countries;

        ddlCountrySettings.DataValueField = "Country_Code";
        ddlCountrySettings.DataTextField = "Country_Name";
        ddlCountrySettings.DataSource = countries;

        ddlCountrySearch.DataValueField = "Country_Code";
        ddlCountrySearch.DataTextField = "Country_Name";
        ddlCountrySearch.DataSource = countries;

        ddlCountryDefault.DataValueField = "Country_Abbr";
        ddlCountryDefault.DataTextField = "Country_Name";
        ddlCountryDefault.DataSource = countries;

        ddlCountry.DataBind();
        ddlCountrySettings.DataBind();
        ddlCountrySearch.DataBind();
        ddlCountryDefault.DataBind();
        
    }

    public void BindEBayAccount()
    {
        DataModelEntities context = new DataModelEntities();
        var ebayAccount = context.UserAccounts.Where(w => w.Is_Active == true && w.User_Code == UserKey && w.Account_Code == (int)Constant.Accounts.Ebay)
                                 .AsEnumerable()
                                 .Select(s => new { s.User_Account_Code, Application_Name = s.Ebay_User_Name });

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

    public void BindSettings()
    {
        DataModelEntities context = new DataModelEntities();
        var category = context.SellerItemFiles.Where(w => w.Is_Active == true && w.SellerItem.User_Code == UserKey)
                                 .AsEnumerable()
                                 .Select(s => new { s.Keywords, s.File_Item_Code,s.Created_Date }).OrderByDescending(s => s.Created_Date).Take(15);

        ddlSavedFile.DataValueField = "File_Item_Code";
        ddlSavedFile.DataTextField = "Keywords";
        ddlSavedFile.DataSource = category.ToList();
        ddlSavedFile.DataBind();
        ddlSavedFile.Items.Add(new ListItem("None", "0"));
        ddlSavedFile.SelectedValue = "0";

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
                int countryCode = int.Parse(ddlCountrySettings.SelectedValue);
                if (countryCode != user.Country_Code)
                {
                    user.Country_Code = int.Parse(ddlCountrySettings.SelectedValue);
                    hfCountryCode.Value = ddlCountrySettings.SelectedValue;//int.Parse(ddlCountrySettings.SelectedValue);
                    ParcelBL objParcelBL = new ParcelBL();
                    objParcelBL.SaveEbayUserItems(UserKey);
                    //ddlCountrySearch.SelectedValue = hfCountryCode.Value;
 
                }
                
                
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
                ddlCountrySettings.SelectedValue = user.Country_Code.ToString();
                ddlCountryDefault.SelectedValue = user.LocatedIn.ToString();
                ddlCountrySearch.SelectedValue = user.Country_Code.ToString();
                hfCountryCode.Value = user.Country_Code.ToString();
            }
        }
        catch (Exception ex)
        {
        }
                                 

    }

    public void btnExport_Click(object sender, EventArgs e)
    {
        DataModelEntities context = new DataModelEntities();
        var user = context.Users.FirstOrDefault(f => f.User_Code == UserKey);
        var items = context.SellerItems.Where(c => c.Is_Active == true
                                                && c.User_Code == UserKey


                                             ).AsEnumerable()
            .Select(a => new
            {
                ItemViewURL = a.Item_View_URL == null ? string.Empty : a.Item_View_URL,
                PictureURL = a.Picture_URL == null ? string.Empty : a.Picture_URL,
                ItemRank = a.Item_Rank == null ? string.Empty : a.Item_Rank.ToString(),
                ItemID = a.Item_ID == null ? string.Empty : a.Item_ID,
                ItemName = a.Item_Name == null ? string.Empty : a.Item_Category_Name,
                Category = a.Item_Category_Name == null ? string.Empty : a.Item_Category_Name,
                SKU = a.Item_SKU == null ? string.Empty : a.Item_SKU,
                StartDate = a.Start_Date == null ? string.Empty : Convert.ToDateTime(a.Start_Date).ToString("dd-MMM-yyyy"),
                EndDate = a.End_Date == null ? string.Empty : Convert.ToDateTime(a.End_Date).ToString("dd-MMM-yyyy"),
                IsPromoItem = a.Is_Promo_Item == true ? "Yes" : "No",
                BINPrice = a.BIN_Price == null ? string.Empty : Convert.ToDecimal(a.BIN_Price).ToString("0.00"),
                FloorPrice = a.Floor_Price == null ? string.Empty : Convert.ToDecimal(a.Floor_Price).ToString("0.00"),
                IsAutomated = a.Is_Automated == true ? "Yes" : "No",
                CeillingPrice = a.Ceiling_Price == null ? string.Empty : Convert.ToDecimal(a.Ceiling_Price).ToString("0.00"),
                CurrentPrice = a.Current_Price == null ? string.Empty : Convert.ToDecimal(a.Current_Price).ToString("0.00"),
                Keywords = a.Keywords == null ? string.Empty : a.Keywords,
                AlgoName = Common.GetAlgoName(a.Algo) == null ? string.Empty : Common.GetAlgoName(a.Algo),
                MinimumFeedback = a.Minimum_Feedback == null ? string.Empty : Convert.ToDecimal(a.Minimum_Feedback).ToString("0.00"),
                MaximumFeedback = a.Maximum_Feedback == null ? string.Empty : Convert.ToDecimal(a.Maximum_Feedback).ToString("0.00"),
                MinimumPrice = a.Minimum_Price == null ? string.Empty : Convert.ToDecimal(a.Minimum_Price).ToString("0.00"),
                MaximumPrice = a.Maximum_Price == null ? string.Empty : Convert.ToDecimal(a.Maximum_Price).ToString("0.00"),
                LessToLowestPrice = a.Less_To_Lowest_Price == null ? string.Empty : Convert.ToDecimal(a.Less_To_Lowest_Price).ToString("0.00"),
                MinimumQuantity = a.Minimum_Quantity == null ? string.Empty : Convert.ToDecimal(a.Minimum_Quantity).ToString("0.00"),
                MaximumQuantity = a.Maximum_Quantity == null ? string.Empty : Convert.ToDecimal(a.Minimum_Quantity).ToString("0.00"),
                IncluedSellers = a.Inclued_Sellers == null ? string.Empty : a.Inclued_Sellers,
                ExcludeSellers = a.Exclude_Sellers == null ? string.Empty : a.Exclude_Sellers,
                MaximumHandlingTime = a.Maximum_Handling_Time == null ? string.Empty : a.Maximum_Handling_Time.ToString(),
                IsFixedPrice = a.Is_Fixed_Price == true ? "Yes" : "No",
                IsAuctions = a.Is_Auctions == true ? "Yes" : "No",
                IsReturnsAccepted = a.Is_Returns_Accepted == true ? "Yes" : "No",
                IsLocationAU = a.Is_Location_AU == true ? "Yes" : "No",
                Is_Hide_Duplicates = a.Is_Hide_Duplicates == true ? "Yes" : "No",
                Is_Top_Rated_Only = a.Is_Top_Rated_Only == true ? "Yes" : "No",
                ExcludeCategoryCodes = a.Exclude_Category_Codes == null ? string.Empty : a.Exclude_Category_Codes,
                IncludeCondtionCodes = a.Include_Condtion_Codes == null ? string.Empty : a.Include_Condtion_Codes

            })
            .ToList();
        DataTable finalList;

        if (user.Package_Id == (int)Common.Package.Trial)
            finalList = items.Take(100).ToList().ToDataTable();
        else if (user.Package_Id == (int)Common.Package.Business)
            finalList = items.Take(200).ToList().ToDataTable();
        else
            finalList = items.ToDataTable();

        Export.ExportToExcel(finalList, "PM_" + DateTime.Now.ToString("ddmmyyyyhhmmss") + ".xls");

        
    }
}