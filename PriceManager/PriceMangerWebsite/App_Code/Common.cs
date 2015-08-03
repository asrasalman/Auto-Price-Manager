using System;
using System.Collections.Generic;
using System.Data.Objects.DataClasses;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.IO.Compression;
using System.Web.UI.WebControls;
using Newtonsoft.Json;
using System.Web.UI;
using System.IO;
using PriceManagerDAL;

/// <summary>
/// Summary description for Common
/// </summary>
public class Common
{
    public Common()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    public static string Serialize(object obj)
    {
        string result = JsonConvert.SerializeObject(obj, Formatting.Indented, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
        return result;
    }

    public static object Deserialize(string json, Type type)
    {
        return JsonConvert.DeserializeObject(json, type);
    }

    public static string GetHash(string value)
    {
        return System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(value, "md5");

    }

    public static void BindDropDown(DropDownList ddlGeneral, Object dataSource, string dataTextField, string dataValueField, bool hasSelectItem, bool hasOtherItem)
    {
        ddlGeneral.DataSource = dataSource;
        ddlGeneral.DataTextField = dataTextField;
        ddlGeneral.DataValueField = dataValueField;
        ddlGeneral.DataBind();

        if (hasSelectItem == true)
        {
            ddlGeneral.Items.Insert(0, new ListItem("-Select-", "0"));
        }

        if (hasOtherItem == true)
        {
            ddlGeneral.Items.Add(new ListItem("-OTHER-", "-100"));
        }
    }

    public static bool EmptyDirectory(string path)
    {
        try
        {
            DirectoryInfo dir = new DirectoryInfo(path);

            List<FileInfo> files = dir.EnumerateFiles().ToList();

            foreach (FileInfo file in files)
            {
                file.Delete();
            }

            return true;
        }
        catch (Exception ex)
        {
            return false;
        }
    }

    public static string GetAlgoName(string algo)
    {
        string algoName;

        if (string.IsNullOrEmpty(algo))
            algoName = "";
        else if (algo == Convert.ToString((int)Common.Algo.Lowest))
            algoName = "Lowest";
        else if (algo == Convert.ToString((int)Common.Algo.Average))
            algoName = "Average";
        else
            algoName = "Match Lowest";
        
        return algoName;
    }

    public static List<ItemTitleModels> GetTitles(EntityCollection<ItemTitle> lst)
    {
        List<ItemTitleModels> returnList = new List<ItemTitleModels>();

        foreach (var itemTitle in lst)
        {
            returnList.Add(new ItemTitleModels {  ItemTitleId= itemTitle.ItemTitleId, Title = itemTitle.Title });
        }

        return returnList;
    }


    public enum EmailTemplates
    {
        ForgotPassword = 1,
        MinimumThresholdAlert = 2,
        NewExpressSetupRequest = 3, 
        FloorLimitReachedAlert = 4,
        NewRegistration = 5,
        Day3 = 6,
        Day7 = 7,
        Day13 = 8,
        Day20 = 9,
        CeilingLimitReachedAlert = 10,

    }

    public enum StockLegerType
    {
        Opening = 1,
        Addjustment = 2,
        NewSale = 3
    }

   
    public enum Algo
    {
        Lowest = 1,
        Average = 2,
        MatchLowest = 3
    }

    public enum Package
    {
        Trial = 10001,
        Business = 10002,
        Pro = 10003,
        ProPlus =10004
    }

    public enum Status
    {
        Pending = 1,
        SetupCompleted = 2,
        Cancel = 3
    }

    public static string GetStatus(int? statusCode)
    {
        if (statusCode == null || statusCode == 1)
            return "Pending";
        else if (statusCode == 2)
            return "SetupCompleted";
        else
            return "Cancel";
    }


    public string getRedirectionURL()
    {
        return "~/pages/login.aspx";
    }

    public static bool VerifyPostCode(string postCode, string location)
    {
        postCode = postCode.TrimStart('0');
        if (new DataModelEntities().PostCodes.Count(c => c.Post_Code == postCode && c.Location == location) > 0)
            return true;
        else
            return false;
    }

    public static string MapPathReverse(string fullServerPath)
    {
        return @"\" + fullServerPath.Replace(HttpContext.Current.Request.PhysicalApplicationPath, String.Empty);
    }

}

public class SellerItemModel
{
    public string Item_View_URL { get; set; }
    public string Picture_URL { get; set; }
    public bool? Is_Promo_Item { get; set; }
    public string Algo { get; set; }
    public decimal? BIN_Price { get; set; }
    public decimal? Ceiling_Price { get; set; }
    public decimal? Current_Price { get; set; }
    public DateTime? End_Date { get; set; }
    public decimal? Floor_Price { get; set; }
    public int Item_Code { get; set; }
    public string Item_ID { get; set; }
    public string Item_Name { get; set; }
    public string Item_SKU { get; set; }
    public DateTime? Start_Date { get; set; }
    public string Keywords { get; set; }
    public bool? Is_Automated { get; set; }
    public int? User_Code { get; set; }
    public string Algo_Name { get; set; }
    public decimal? Minimum_Feedback { get; set; }
    public decimal? Maximum_Feedback { get; set; }
    public decimal? Minimum_Price { get; set; }
    public decimal? Maximum_Price { get; set; }
    public int? Minimum_Quantity { get; set; }
    public int? Maximum_Quantity { get; set; }
    public string Inclued_Sellers { get; set; }
    public string Exclued_Sellers { get; set; }
    public int? Maximum_Handling_Time { get; set; }
    public bool? Is_Fixed_Price { get; set; }
    public bool? Is_Auctions { get; set; }
    public bool? Is_Returns_Accepted { get; set; }
    public bool? Is_Location_AU { get; set; }
    public bool? Is_Hide_Duplicates { get; set; }
    public bool? Is_Top_Rated_Only { get; set; }
    public string Exclude_Category_Codes { get; set; }
    public string Include_Condtion_Codes { get; set; }
    public string Item_Category_ID { get; set; }
    public string Item_Category_Name { get; set; }
    public decimal? Less_To_Lowest_Price { get; set; }
    public int? Item_Rank { get; set; }
    public string Currency { get; set; }
    public string Ignore_Words { get; set; }
    public int? Country_Code { get; set; }
    public string LocatedIn { get; set; }
    public int? Rotate_Order { get; set; }
    public int? Rotate_Days { get; set; }
    public int? Rotate_Sales { get; set; }
    public List<ItemTitleModels> ItemTitles { get; set; }
    public int? User_Account_Code { get; set; }
    public int? QuantityAvailable { get; set; }
}

public  class ItemTitleModels
{
    public int ItemTitleId { get; set; }
    public string Title { get; set; }
    public int TotalSales { get; set; }
}