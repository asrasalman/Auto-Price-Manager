using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Text;
using System.Web.Script.Serialization;
using PriceManagerDAL;
using System.Collections;
using System.Linq.Expressions;
using com.ebay.developer;
using System.Configuration;
using System.Data.Objects;


[ServiceContract(Namespace = "GeneralServices")]
[AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
public class GeneralSvc
{
    [OperationContract]
    public string GetItemDetail(string inventoryID)
    {
        return null;
        // Inventory inventory = new DataModelEntities().Inventories.First(i => i.INVENTORYID == inventoryID);
        //  return Common.Serialize(inventory);
    }


    [OperationContract]
    public string GetSuburbList(string postCode, string currentSuburb)
    {
        postCode = postCode.TrimStart('0');
        List<string> postCodes = new DataModelEntities().PostCodes.Where(p => p.Post_Code == postCode).Select(s => s.Location).ToList();
        return Common.Serialize(postCodes);
        //return null;
    }

    [OperationContract]
    public string RemoveAccountToken(string type, int userAccountCode)
    {


        try
        {
            DataModelEntities context = new DataModelEntities();
            UserAccount userAccount = null;
            int UserCode = new Base().UserKey;
            if (type == "EBAY")
            {
                userAccount = context.UserAccounts.First(f => f.User_Account_Code == userAccountCode);
                userAccount.Is_Active = false;
            }
            else if (type == "SHOPIFY")
            {
                userAccount = context.UserAccounts.First(f => f.User_Code == UserCode && f.Is_Active == true && f.Account_Code == (int)Constant.Accounts.Shopify);
                userAccount.Is_Active = false;
            }
            else if (type == "MAGENTO")
            {
                userAccount = context.UserAccounts.First(f => f.User_Code == UserCode && f.Is_Active == true && f.Account_Code == (int)Constant.Accounts.Magento);
                userAccount.Is_Active = false;
            }
            else if (type == "BIGCOMMERCE")
            {
                userAccount = context.UserAccounts.First(f => f.User_Code == UserCode && f.Is_Active == true && f.Account_Code == (int)Constant.Accounts.Bigcommerce);
                userAccount.Is_Active = false;
            }

            if (userAccount != null)
            {
                userAccountCode = userAccount.User_Account_Code;
                context.ExecuteStoreCommand("DELETE FROM ParcelItem WHERE AccountID = " + userAccountCode);
                context.ExecuteStoreCommand("DELETE FROM StockLedger WHERE AccountID = " + userAccountCode);
                context.ExecuteStoreCommand("DELETE FROM Items WHERE User_Account_Code = '" + userAccountCode + "'");
                context.ExecuteStoreCommand("DELETE FROM SellerItemFile WHERE Item_Code IN (SELECT Item_Code FROM SellerItem Where User_Account_Code = " + userAccountCode + ")");
                context.ExecuteStoreCommand("DELETE FROM PricingHistory WHERE Item_Code IN (SELECT Item_Code FROM SellerItem Where User_Account_Code = " + userAccountCode + ")");
                context.ExecuteStoreCommand("DELETE FROM SellerItem WHERE User_Account_Code = " + userAccountCode);
                //context.SaveChanges();

                //    var parcelItems = context.ParcelItems.Where(w => w.AccountID == userAccountCode);
                //    foreach (var item in parcelItems)
                //    {
                //        context.ParcelItems.DeleteObject(item);
                //    }

                //    string AccountID = userAccountCode.ToString();
                //    var legerItems = context.StockLedgers.Where(w => w.AccountID == AccountID);
                //    foreach (var item in legerItems)
                //    {
                //        context.StockLedgers.DeleteObject(item);
                //    }

                //    var items = context.Items.Where(w => w.User_Account_Code == userAccountCode);
                //    foreach (var item in items)
                //    {
                //        context.Items.DeleteObject(item);
                //    }



                //    var sellerItems = context.SellerItems.Where(w => w.User_Account_Code == userAccountCode);
                //    foreach (var item in sellerItems)
                //    {
                //        var sellerItemsFiles = context.SellerItemFiles.Where(w => w.Item_Code == item.Item_Code);
                //        foreach (var itemfiles in sellerItemsFiles)
                //        {
                //            context.SellerItemFiles.DeleteObject(itemfiles);

                //        }
                //        context.SellerItems.DeleteObject(item);
                //    }
            }

            context.SaveChanges();
            context = null;
            return "true";
        }
        catch (Exception ex)
        {
            Logging.WriteLog(LogType.Critical, ex.ToString());
            return null;
        }





    }

    [OperationContract]
    public string GetSearchByKeyword(string keyword, int pageSize, string TokenJSON, string filters, int? userCode)
    {

        int UserCode = userCode == null ? new Base().UserKey : (int)userCode;
        User user = new DataModelEntities().Users.FirstOrDefault(f => f.User_Code == userCode);
        dynamic filterObject = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<dynamic>(filters);

        EbayServiceBL serviceBL = new EbayServiceBL(UserCode);

        FindItemsByKeywordsResponse response = serviceBL.SearchItems(keyword, pageSize, 1, filterObject);
        SearchResult result = response.searchResult;

        if (result == null)
            return null;

        // get all Items Details
        SimpleItemType[] simpleItems = serviceBL.GetMultipleItemsDetails(result);
        List<EbaySearchItem> searchItems = new List<EbaySearchItem>();

        Dictionary<string, int> tokens = (Dictionary<string, int>)Common.Deserialize(TokenJSON, typeof(Dictionary<string, int>));

        string[] mySellerIDs = tokens.Select(t => t.Key).ToArray();

        // Looping through response object for result
        foreach (SearchItem item in result.item)
        {
            SimpleItemType simpleItem = simpleItems.FirstOrDefault(i => i.ItemID == item.itemId);
            if (simpleItem.ReserveMetSpecified == false || simpleItem.ReserveMet == true)
            {
                EbaySearchItem searchItem = new EbaySearchItem();
                string timeLeft = item.sellingStatus.timeLeft, days = string.Empty, temp = string.Empty;

                if (timeLeft.IndexOf('D') != -1)
                    days = timeLeft.Substring(timeLeft.IndexOf('P') + 1, timeLeft.IndexOf('D') - 1) + "d ";
                if (days == "0d ")
                    days = "";

                temp = days + timeLeft.Substring(timeLeft.IndexOf('T') + 1, timeLeft.IndexOf('H') - timeLeft.IndexOf('T') - 1) + "h ";

                if (days == "")
                    timeLeft = temp + timeLeft.Substring(timeLeft.IndexOf('H') + 1, timeLeft.IndexOf('M') - timeLeft.IndexOf('H') - 1) + "m";
                else
                    timeLeft = temp;

                searchItem.ItemID = item.itemId;
                searchItem.Title = item.title;
                searchItem.Price = item.sellingStatus.currentPrice.Value;
                searchItem.TimeRemaining = timeLeft;
                searchItem.ViewURL = item.viewItemURL;
                searchItem.ImageURL = item.galleryURL;
                searchItem.SellerID = simpleItem.Seller.UserID;
                searchItem.TopRatedSeller = simpleItem.Seller.TopRatedSeller;
                searchItem.SellerScore = simpleItem.Seller.PositiveFeedbackPercent;
                searchItem.ConvertedPrice = item.sellingStatus.convertedCurrentPrice.Value;
                searchItem.TotalCost = searchItem.ConvertedPrice;//will be shown on search result


                //We are ignoring Shipping cost for the time being because we dont have solution to get converted shipping cost
                if (item.shippingInfo.shippingServiceCost != null)
                    searchItem.ShippingCost = item.shippingInfo.shippingServiceCost.Value;
                else
                    searchItem.ShippingCost = 0;

                /*If user have selected include shipping in settings shippingcost + price*/
                if (user.Automation_Include_Shipping != null && user.Automation_Include_Shipping == true)
                    searchItem.TotalCostIncludingShipping = searchItem.ShippingCost + searchItem.ConvertedPrice;
                else
                    searchItem.TotalCostIncludingShipping = searchItem.ConvertedPrice;



                if (mySellerIDs.Contains(searchItem.SellerID))
                    searchItem.IsMyProduct = true;
                else
                    searchItem.IsMyProduct = false;

                searchItems.Add(searchItem);
            }
        }

        var data = searchItems.OrderBy(o => o.TotalCostIncludingShipping).ToList();
        return Common.Serialize(data);
    }

    [OperationContract]
    public string GetProductRank(string filterJSON, int pageSize, string TokenJSON, int? userCode)
    {
        int UserCode = userCode == null ? new Base().UserKey : (int)userCode;
        User user = new DataModelEntities().Users.FirstOrDefault(f => f.User_Code == UserCode);
        pageSize = user.Search_Only_Top_Items == null || user.Search_Only_Top_Items == 0 ? 25 : (int)user.Search_Only_Top_Items;

        SellerItem sellerItem = null;
        if (!string.IsNullOrEmpty(filterJSON))
        {
            sellerItem = (SellerItem)Common.Deserialize(filterJSON, typeof(SellerItem));
        }
        if (sellerItem != null)
        {
            //EbayServiceBL serviceBL = new EbayServiceBL(UserCode);
            EbayServiceBL serviceBL = new EbayServiceBL(UserCode, (int)sellerItem.Country_Code);


            FindItemsByKeywordsResponse response = null;
            SearchResult result = null;

            //There is no filter for ignored words 
            //I am requesting api to give 60 items so that i can ignore items based on ingored words

            if (!string.IsNullOrEmpty(sellerItem.Ignore_Words))
            {
                response = serviceBL.SearchItems(sellerItem, 60, 1);
                result = response.searchResult;

                if (result == null || result.count == 0)
                    return null;

                string[] ignoredWords = sellerItem.Ignore_Words.Split(',');
                SearchItem[] shortResult = result.item.AsEnumerable().Where(w => !ignoredWords.Any(a => !string.IsNullOrEmpty(a) && w.title.ToLower().Contains(a.ToLower()))).Take(pageSize).ToArray();
                if (shortResult.Length > 0)
                {
                    result.item = shortResult;
                    result.count = shortResult.Length;
                }
                else
                    return null;
            }
            else
            {
                response = serviceBL.SearchItems(sellerItem, pageSize, 1);
                result = response.searchResult;

                if (result == null || result.count == 0)
                    return null;

            }


            //FindItemsByKeywordsResponse response = serviceBL.SearchItems(sellerItem, pageSize, 1);
            //SearchResult result = response.searchResult;

            //if (result == null || result.count == 0)
            //    return null;



            // get all Items Details
            SimpleItemType[] simpleItems = serviceBL.GetMultipleItemsDetails(result);
            List<EbaySearchItem> searchItems = new List<EbaySearchItem>();

            Dictionary<string, int> tokens = (Dictionary<string, int>)Common.Deserialize(TokenJSON, typeof(Dictionary<string, int>));

            string[] mySellerIDs = tokens.Select(t => t.Key).ToArray();

            // Looping through response object for result
            foreach (SearchItem item in result.item)
            {
                SimpleItemType simpleItem = simpleItems.FirstOrDefault(i => i.ItemID == item.itemId);
                if (simpleItem.ReserveMetSpecified == false || simpleItem.ReserveMet == true)
                {
                    EbaySearchItem searchItem = new EbaySearchItem();
                    string timeLeft = item.sellingStatus.timeLeft, days = string.Empty, temp = string.Empty;

                    if (timeLeft.IndexOf('D') != -1)
                        days = timeLeft.Substring(timeLeft.IndexOf('P') + 1, timeLeft.IndexOf('D') - 1) + "d ";
                    if (days == "0d ")
                        days = "";

                    temp = days + timeLeft.Substring(timeLeft.IndexOf('T') + 1, timeLeft.IndexOf('H') - timeLeft.IndexOf('T') - 1) + "h ";

                    if (days == "")
                        timeLeft = temp + timeLeft.Substring(timeLeft.IndexOf('H') + 1, timeLeft.IndexOf('M') - timeLeft.IndexOf('H') - 1) + "m";
                    else
                        timeLeft = temp;

                    searchItem.ItemID = item.itemId;
                    searchItem.Title = item.title;
                    searchItem.Price = item.sellingStatus.currentPrice.Value;
                    searchItem.TimeRemaining = timeLeft;
                    searchItem.ViewURL = item.viewItemURL;
                    searchItem.ImageURL = item.galleryURL;
                    searchItem.SellerID = simpleItem.Seller.UserID;
                    searchItem.TopRatedSeller = simpleItem.Seller.TopRatedSeller;
                    searchItem.SellerScore = simpleItem.Seller.PositiveFeedbackPercent;
                    searchItem.ConvertedPrice = item.sellingStatus.convertedCurrentPrice.Value;
                    searchItem.TotalCost = searchItem.ConvertedPrice;//will be shown on search result

                    //We are ignoring Shipping cost for the time being because we dont have solution to get converted shipping cost
                    //if (item.shippingInfo.shippingServiceCost != null && user.Country1.Country_Abbr.ToUpper() == sellerItem.LocatedIn.ToUpper())
                    if (1 == 1)

                       
                   // searchItem.ShippingCost = item.shippingInfo.shippingServiceCost.;
                    {
                        //double test = item.shippingInfo.shippingServiceCost.Value;

                        var shipping = serviceBL.GetShippingCost(serviceBL.UserTokens[(int)sellerItem.User_Account_Code], searchItem.ItemID);
                        if (shipping.ShippingDetails.InternationalShippingServiceOption != null)
                        {
                            foreach (var items in shipping.ShippingDetails.InternationalShippingServiceOption)
                            {
                                string cost = items.ShippingServiceCost == null ? "0.0" : items.ShippingServiceCost.ToString();
                                searchItem.ShippingCost = Double.Parse(cost);
                            }
                            
                        }
                        else
                        {
                            searchItem.ShippingCost = 0;
                        }
                        //var shipping = serviceBL.GetShippingCost(serviceBL.UserTokens[(int)sellerItem.User_Account_Code], searchItem.ItemID);
                        //if (shipping.ShippingDetails.DefaultShippingCost != null)
                        //{
                        //    searchItem.ShippingCost = shipping.ShippingDetails.DefaultShippingCost.Value;
                        //}
                        //else
                        //{
                        //    searchItem.ShippingCost = 0;
                        //}
                    }
                    else
                        searchItem.ShippingCost = 0;

                    /*If user have selected include shipping in settings shippingcost + price*/
                    if (user.Automation_Include_Shipping != null && user.Automation_Include_Shipping == true)
                        searchItem.TotalCostIncludingShipping = searchItem.ShippingCost + searchItem.ConvertedPrice;
                    else
                        searchItem.TotalCostIncludingShipping = searchItem.ConvertedPrice;



                    if (mySellerIDs.Contains(searchItem.SellerID))
                        searchItem.IsMyProduct = true;
                    else
                        searchItem.IsMyProduct = false;

                    searchItems.Add(searchItem);
                }
            }

            var data = searchItems.Where(w => w.ItemID == sellerItem.Item_ID || w.IsMyProduct == false).ToList().OrderBy(o => o.TotalCost);
            return Common.Serialize(data);
        }
        else
            return null;
    }

    [OperationContract]
    public string GetProductRankTitle(string filterJSON, int pageSize, string TokenJSON, int? userCode)
    {
        int UserCode = userCode == null ? new Base().UserKey : (int)userCode;
        User user = new DataModelEntities().Users.FirstOrDefault(f => f.User_Code == UserCode);
        pageSize = user.Search_Only_Top_Items == null || user.Search_Only_Top_Items == 0 ? 25 : (int)user.Search_Only_Top_Items;

        SellerItem sellerItem = null;
        if (!string.IsNullOrEmpty(filterJSON))
        {
            sellerItem = (SellerItem)Common.Deserialize(filterJSON, typeof(SellerItem));
        }
        if (sellerItem != null)
        {
            //EbayServiceBL serviceBL = new EbayServiceBL(UserCode);
            EbayServiceBL serviceBL = new EbayServiceBL(UserCode, (int)sellerItem.Country_Code);


            FindItemsByKeywordsResponse response = null;
            SearchResult result = null;

            //There is no filter for ignored words 
            //I am requesting api to give 60 items so that i can ignore items based on ingored words

            if (!string.IsNullOrEmpty(sellerItem.Ignore_Words))
            {
                response = serviceBL.SearchItems(sellerItem, 60, 1);
                result = response.searchResult;

                if (result == null || result.count == 0)
                    return null;

                string[] ignoredWords = sellerItem.Ignore_Words.Split(',');
                SearchItem[] shortResult = result.item.AsEnumerable().Where(w => !ignoredWords.Any(a => !string.IsNullOrEmpty(a) && w.title.ToLower().Contains(a.ToLower()))).Take(pageSize).ToArray();
                if (shortResult.Length > 0)
                {
                    result.item = shortResult;
                    result.count = shortResult.Length;
                }
                else
                    return null;
            }
            else
            {
                response = serviceBL.SearchItems(sellerItem, pageSize, 1);
                result = response.searchResult;

                if (result == null || result.count == 0)
                    return null;

            }


            //FindItemsByKeywordsResponse response = serviceBL.SearchItems(sellerItem, pageSize, 1);
            //SearchResult result = response.searchResult;

            //if (result == null || result.count == 0)
            //    return null;



            // get all Items Details
            SimpleItemType[] simpleItems = serviceBL.GetMultipleItemsDetails(result);
            List<EbaySearchItem> searchItems = new List<EbaySearchItem>();

            Dictionary<string, int> tokens = (Dictionary<string, int>)Common.Deserialize(TokenJSON, typeof(Dictionary<string, int>));

            string[] mySellerIDs = tokens.Select(t => t.Key).ToArray();

            // Looping through response object for result
            foreach (SearchItem item in result.item)
            {
                SimpleItemType simpleItem = simpleItems.FirstOrDefault(i => i.ItemID == item.itemId);
                if (simpleItem.ReserveMetSpecified == false || simpleItem.ReserveMet == true)
                {
                    EbaySearchItem searchItem = new EbaySearchItem();
                    string timeLeft = item.sellingStatus.timeLeft, days = string.Empty, temp = string.Empty;

                    if (timeLeft.IndexOf('D') != -1)
                        days = timeLeft.Substring(timeLeft.IndexOf('P') + 1, timeLeft.IndexOf('D') - 1) + "d ";
                    if (days == "0d ")
                        days = "";

                    temp = days + timeLeft.Substring(timeLeft.IndexOf('T') + 1, timeLeft.IndexOf('H') - timeLeft.IndexOf('T') - 1) + "h ";

                    if (days == "")
                        timeLeft = temp + timeLeft.Substring(timeLeft.IndexOf('H') + 1, timeLeft.IndexOf('M') - timeLeft.IndexOf('H') - 1) + "m";
                    else
                        timeLeft = temp;

                    searchItem.ItemID = item.itemId;
                    searchItem.Title = item.title;
                    searchItem.Price = item.sellingStatus.currentPrice.Value;
                    searchItem.TimeRemaining = timeLeft;
                    searchItem.ViewURL = item.viewItemURL;
                    searchItem.ImageURL = item.galleryURL;
                    searchItem.SellerID = simpleItem.Seller.UserID;
                    searchItem.TopRatedSeller = simpleItem.Seller.TopRatedSeller;
                    searchItem.SellerScore = simpleItem.Seller.PositiveFeedbackPercent;
                    searchItem.ConvertedPrice = item.sellingStatus.convertedCurrentPrice.Value;
                    searchItem.TotalCost = searchItem.ConvertedPrice;//will be shown on search result

                    //We are ignoring Shipping cost for the time being because we dont have solution to get converted shipping cost
                    //if (item.shippingInfo.shippingServiceCost != null && user.Country1.Country_Abbr.ToUpper() == sellerItem.LocatedIn.ToUpper())
                    //    searchItem.ShippingCost = item.shippingInfo.shippingServiceCost.Value;
                    //else
                    //    searchItem.ShippingCost = 0;

                    /*If user have selected include shipping in settings shippingcost + price*/
                    if (user.Automation_Include_Shipping != null && user.Automation_Include_Shipping == true)
                        searchItem.TotalCostIncludingShipping = searchItem.ShippingCost + searchItem.ConvertedPrice;
                    else
                        searchItem.TotalCostIncludingShipping = searchItem.ConvertedPrice;



                    if (mySellerIDs.Contains(searchItem.SellerID))
                        searchItem.IsMyProduct = true;
                    else
                        searchItem.IsMyProduct = false;

                    searchItems.Add(searchItem);
                }
            }

            var data = searchItems.Where(w => w.ItemID == sellerItem.Item_ID || w.IsMyProduct == false).ToList().OrderBy(o => o.TotalCost);
            return Common.Serialize(data);
        }
        else
            return null;
    }

    [OperationContract]
    public string UpdateProductPrice(string ProductCode, double Price, string TokenJSON, string userID)
    {
        Dictionary<string, int> tokens = (Dictionary<string, int>)Common.Deserialize(TokenJSON, typeof(Dictionary<string, int>));

        int userAccountCode = tokens[userID];

        int UserCode = new Base().UserKey;
        EbayServiceBL service = new EbayServiceBL(UserCode);

        string result = service.ReviseEbayItem(ProductCode, Price, service.UserTokens[userAccountCode]);

        return result;
    }

    public string UpdateProductTitle(string ItemId, string Title, string TokenJSON, string userID)
    {
        Dictionary<string, int> tokens = (Dictionary<string, int>)Common.Deserialize(TokenJSON, typeof(Dictionary<string, int>));

        int userAccountCode = tokens[userID];

        int UserCode = new Base().UserKey;
        EbayServiceBL service = new EbayServiceBL(UserCode);

        string result = service.ReviseEbayItemTitle(ItemId, Title, service.UserTokens[userAccountCode]);

        return result;
    }

    [OperationContract]
    public string LoadTitle(string ProductCode)
    {
        int userKey = new Base().UserKey;

        if (userKey == 0)
        {
            return "{\"login\": false}";
        }

        //EbayServiceBL serviceBL = new EbayServiceBL(userKey);

        //FindItemsByKeywordsResponse response = serviceBL.SearchItems;
        //SearchResult result = response.searchResult;

        //if (result == null)
        //    return null;

        return Common.Serialize("1");
    }

    [OperationContract]
    public string GetPricingItems(int? itemCode, int? userAccountCode, string categoryID, string searchColumn, string searchValue, bool? reload, int? country)
    {
        int userKey = new Base().UserKey;
        if (userKey == 0)
        {
            return "{\"login\": false}";
        }

        if (reload == true)
        {
            ParcelBL objParcelBL = new ParcelBL();
            objParcelBL.SaveEbayUserItems(userKey);
        }

        //string[] searchitems = null;
        
        //if (searchValue.Contains(""))
        //{
        //    searchitems = searchValue.Split(' ');

        //}

        DataModelEntities context = new DataModelEntities();
        var user = context.Users.FirstOrDefault(f => f.User_Code == userKey);

        DateTime currentDate = DateTime.Today.Date;

        var items = context.SellerItems.Where(c => c.Is_Active == true
                                                && c.User_Code == userKey
                                                && (
                                                (
                                                    itemCode == null
                                        //            && (country == null || c.Country_Code == country)
                                                    && (userAccountCode == null || userAccountCode == 0 || c.User_Account_Code == userAccountCode)
                                                    && (string.IsNullOrEmpty(categoryID) || categoryID == "0" || c.Item_Category_ID == categoryID)

                                                    && (searchColumn != "Item_Name" || string.IsNullOrEmpty(searchValue) || c.Item_Name.Contains(searchValue))
                                                    && (searchColumn != "Item_ID" || string.IsNullOrEmpty(searchValue) || c.Item_ID.Contains(searchValue))

                                                   //// && (searchColumn == "Item_Name" && (string.IsNullOrEmpty(searchValue) == false || c.Item_Name.Any(p => searchitems.Any())) )
                                                    //&& (searchColumn == "Item_ID" && (string.IsNullOrEmpty(searchValue) == false  || c.Item_Name.Any(p => searchitems.Any()))   )
                                                    && (EntityFunctions.TruncateTime(c.End_Date) > currentDate)
                                                )
                                                    || (c.Item_Code == itemCode)
                                                    )

                                             ).AsEnumerable()
            .Select(a => new SellerItemModel()
            {
                Item_View_URL = a.Item_View_URL,
                Picture_URL = a.Picture_URL,
                Is_Promo_Item = a.Is_Promo_Item,
                Algo = a.Algo,
                BIN_Price = a.BIN_Price,
                Ceiling_Price = a.Ceiling_Price,
                Current_Price = a.Current_Price,
                End_Date = a.End_Date,
                Floor_Price = a.Floor_Price,
                Item_Code = a.Item_Code,
                Item_ID = a.Item_ID,
                Item_Name = a.Item_Name,
                Item_SKU = a.Item_SKU,
                Start_Date = a.Start_Date,
                Keywords = a.Keywords,
                Is_Automated = a.Is_Automated,
                User_Code = a.User_Code,
                Algo_Name = Common.GetAlgoName(a.Algo),
                Minimum_Feedback = a.Minimum_Feedback,
                Maximum_Feedback = a.Maximum_Feedback,
                Minimum_Price = a.Minimum_Price,
                Maximum_Price = a.Maximum_Price,
                Minimum_Quantity = a.Minimum_Quantity,
                Maximum_Quantity = a.Maximum_Quantity,
                Inclued_Sellers = a.Inclued_Sellers,
                Exclued_Sellers = a.Exclude_Sellers,
                Maximum_Handling_Time = a.Maximum_Handling_Time,
                Is_Fixed_Price = a.Is_Fixed_Price,
                Is_Auctions = a.Is_Auctions,
                Is_Returns_Accepted = a.Is_Returns_Accepted,
                Is_Location_AU = a.Is_Location_AU,
                Is_Hide_Duplicates = a.Is_Hide_Duplicates,
                Is_Top_Rated_Only = a.Is_Top_Rated_Only,
                Exclude_Category_Codes = a.Exclude_Category_Codes,
                Include_Condtion_Codes = a.Include_Condtion_Codes,
                Item_Category_ID = a.Item_Category_ID,
                Item_Category_Name = a.Item_Category_Name,
                Less_To_Lowest_Price = a.Less_To_Lowest_Price,
                Item_Rank = a.Item_Rank,
                Currency = a.Currency,
                Ignore_Words = a.Ignore_Words,
                Country_Code = a.Country_Code,
                LocatedIn = a.LocatedIn,
                Rotate_Order = a.Rotate_Order,
                Rotate_Days = a.Rotate_Days,
                Rotate_Sales = a.Rotate_Sales,
                User_Account_Code = a.User_Account_Code,
                QuantityAvailable = a.QuantityAvailable
            })
            .ToList();

        if (itemCode.HasValue)
        {
            items[0].ItemTitles = context.ItemTitles.Where(a => a.ItemId == itemCode).Select(
                b => new ItemTitleModels
                    {
                        Title = b.Title,
                        ItemTitleId = b.ItemTitleId,
                        TotalSales = b.TotalSales == null ? 0 : (int)b.TotalSales
                    }
                ).ToList();
        }


        if (user.Package_Id == (int)Common.Package.Trial)
            return Common.Serialize(items.Take(100));
        else if (user.Package_Id == (int)Common.Package.Business)
            return Common.Serialize(items.ToList().Take(200));
        else
            return Common.Serialize(items);
    }

    //[OperationContract]
    //public string GetPricingItems(int? itemCode, int? userAccountCode, string categoryID, string searchColumn, string searchValue, bool? reload, int? country)
    //{
    //    int userKey = new Base().UserKey;
    //    if (userKey == 0)
    //    {
    //        return "{\"login\": false}";
    //    }

    //    if (reload == true)
    //    {
    //        ParcelBL objParcelBL = new ParcelBL();
    //        objParcelBL.SaveEbayUserItems(userKey);
    //    }

    //    string[] searchitems;

    //    if (searchValue.Contains(""))
    //    {
    //        searchitems = searchValue.Split("");

    //    }

    //    DataModelEntities context = new DataModelEntities();
    //    var user = context.Users.FirstOrDefault(f => f.User_Code == userKey);

    //    DateTime currentDate = DateTime.Today.Date;

    //    var items = context.SellerItems.Where(c => c.Is_Active == true
    //                                            && c.User_Code == userKey
    //                                            && (
    //                                                (
    //                                                    itemCode == null
    //        //            && (country == null || c.Country_Code == country)
    //                                                    && (userAccountCode == null || userAccountCode == 0 || c.User_Account_Code == userAccountCode)
    //                                                    && (string.IsNullOrEmpty(categoryID) || categoryID == "0" || c.Item_Category_ID == categoryID)

    //                                                    && (searchColumn != "Item_Name" || string.IsNullOrEmpty(searchValue) || c.Item_Name.Contains(searchValue))
    //                                                    && (searchColumn != "Item_ID" || string.IsNullOrEmpty(searchValue) || c.Item_ID.Contains(searchValue))
    //                                                    && (EntityFunctions.TruncateTime(c.End_Date) > currentDate)
    //                                                )
    //                                                || (c.Item_Code == itemCode)
    //                                                )

    //                                         ).AsEnumerable()
    //        .Select(a => new SellerItemModel()
    //        {
    //            Item_View_URL = a.Item_View_URL,
    //            Picture_URL = a.Picture_URL,
    //            Is_Promo_Item = a.Is_Promo_Item,
    //            Algo = a.Algo,
    //            BIN_Price = a.BIN_Price,
    //            Ceiling_Price = a.Ceiling_Price,
    //            Current_Price = a.Current_Price,
    //            End_Date = a.End_Date,
    //            Floor_Price = a.Floor_Price,
    //            Item_Code = a.Item_Code,
    //            Item_ID = a.Item_ID,
    //            Item_Name = a.Item_Name,
    //            Item_SKU = a.Item_SKU,
    //            Start_Date = a.Start_Date,
    //            Keywords = a.Keywords,
    //            Is_Automated = a.Is_Automated,
    //            User_Code = a.User_Code,
    //            Algo_Name = Common.GetAlgoName(a.Algo),
    //            Minimum_Feedback = a.Minimum_Feedback,
    //            Maximum_Feedback = a.Maximum_Feedback,
    //            Minimum_Price = a.Minimum_Price,
    //            Maximum_Price = a.Maximum_Price,
    //            Minimum_Quantity = a.Minimum_Quantity,
    //            Maximum_Quantity = a.Maximum_Quantity,
    //            Inclued_Sellers = a.Inclued_Sellers,
    //            Exclued_Sellers = a.Exclude_Sellers,
    //            Maximum_Handling_Time = a.Maximum_Handling_Time,
    //            Is_Fixed_Price = a.Is_Fixed_Price,
    //            Is_Auctions = a.Is_Auctions,
    //            Is_Returns_Accepted = a.Is_Returns_Accepted,
    //            Is_Location_AU = a.Is_Location_AU,
    //            Is_Hide_Duplicates = a.Is_Hide_Duplicates,
    //            Is_Top_Rated_Only = a.Is_Top_Rated_Only,
    //            Exclude_Category_Codes = a.Exclude_Category_Codes,
    //            Include_Condtion_Codes = a.Include_Condtion_Codes,
    //            Item_Category_ID = a.Item_Category_ID,
    //            Item_Category_Name = a.Item_Category_Name,
    //            Less_To_Lowest_Price = a.Less_To_Lowest_Price,
    //            Item_Rank = a.Item_Rank,
    //            Currency = a.Currency,
    //            Ignore_Words = a.Ignore_Words,
    //            Country_Code = a.Country_Code,
    //            LocatedIn = a.LocatedIn,
    //            Rotate_Order = a.Rotate_Order,
    //            Rotate_Days = a.Rotate_Days,
    //            Rotate_Sales = a.Rotate_Sales,
    //            User_Account_Code = a.User_Account_Code
    //        })
    //        .ToList();

    //    if (itemCode.HasValue)
    //    {
    //        items[0].ItemTitles = context.ItemTitles.Where(a => a.ItemId == itemCode).Select(
    //            b => new ItemTitleModels
    //            {
    //                Title = b.Title,
    //                ItemTitleId = b.ItemTitleId,
    //                TotalSales = b.TotalSales == null ? 0 : (int)b.TotalSales
    //            }
    //            ).ToList();
    //    }


    //    if (user.Package_Id == (int)Common.Package.Trial)
    //        return Common.Serialize(items.Take(100));
    //    else if (user.Package_Id == (int)Common.Package.Business)
    //        return Common.Serialize(items.ToList().Take(200));
    //    else
    //        return Common.Serialize(items);
    //}

    
    
    
    [OperationContract]
    public string GetPricingItemsFile(int? itemCode, int? itemCodeFile)
    {
        int userKey = new Base().UserKey;
        if (userKey == 0)
        {
            return "{\"login\": false}";
        }

        DataModelEntities context = new DataModelEntities();
        var user = context.Users.FirstOrDefault(f => f.User_Code == userKey);



        var items = context.SellerItemFiles.Where(c => c.Is_Active == true
                                                   && (itemCode == null || c.Item_Code == itemCode)
                                                   && (itemCodeFile == null || c.File_Item_Code == itemCodeFile)


                                             ).AsEnumerable()
            .Select(a => new
            {
                a.File_Item_Code,
                a.Algo,
                a.Ceiling_Price,
                a.Floor_Price,
                a.Item_Code,
                a.Keywords,
                a.Is_Automated,
                Algo_Name = Common.GetAlgoName(a.Algo),
                a.Minimum_Feedback,
                a.Maximum_Feedback,
                a.Minimum_Price,
                a.Maximum_Price,
                a.Minimum_Quantity,
                a.Maximum_Quantity,
                a.Inclued_Sellers,
                a.Exclude_Sellers,
                a.Maximum_Handling_Time,
                a.Is_Fixed_Price,
                a.Is_Auctions,
                a.Is_Returns_Accepted,
                a.Is_Location_AU,
                a.Is_Hide_Duplicates,
                a.Is_Top_Rated_Only,
                a.Exclude_Category_Codes,
                a.Include_Condtion_Codes,
                a.Less_To_Lowest_Price,
                a.Country_Code,
                a.LocatedIn,
                a.Created_Date


            })
            .ToList();

        return Common.Serialize(items);
    }

    [OperationContract]
    public string UpdatePricingTitle(string editedTitles)
    {
        //Saad
        int userKey = new Base().UserKey;
        if (userKey == 0)
        {
            return "{\"login\": false}";
        }

        try
        {
            dynamic item = new JavaScriptSerializer().Deserialize<dynamic>(editedTitles);

            DataModelEntities context = new DataModelEntities();

            int itemCode = Convert.ToInt32(item["ItemCode"]);
            SellerItem sellerItem = context.SellerItems.SingleOrDefault(a => a.Item_Code == itemCode);

            if (sellerItem != null)
            {
                sellerItem.Rotate_Order = Convert.ToInt32(item["Rotate"]);
                sellerItem.Rotate_Days = Convert.ToInt32(item["Days"]);
                sellerItem.Rotate_Sales = Convert.ToInt32(item["Sales"]);
                sellerItem.Is_Title_Automated = Convert.ToBoolean(item["IsAutomate"]);
                ItemTitle itemTitle;

                if (item.ContainsKey("Title1") && !string.IsNullOrEmpty(item["Title1"].ToString()))
                {
                    if (item.ContainsKey("ItemTitleId1") && !string.IsNullOrEmpty(item["ItemTitleId1"]))
                    {
                        int itemTitleId1 = Convert.ToInt32(item["ItemTitleId1"]);
                        itemTitle = context.ItemTitles.SingleOrDefault(a => a.ItemTitleId == itemTitleId1);
                        if (itemTitle != null)
                        {
                            if (!string.IsNullOrEmpty(itemTitle.Title) && itemTitle.Title.ToString().ToLower() != item["Title1"].ToString().ToLower())
                                itemTitle.TotalSales = 0;

                            itemTitle.ItemId = itemCode;
                            itemTitle.Title = item["Title1"].ToString();
                            itemTitle.Updated_Date = DateTime.Now;
                            itemTitle.Updated_By = userKey;
                            
                        }

                    }
                    else
                    {
                        itemTitle = new ItemTitle();
                        itemTitle.ItemId = itemCode;
                        itemTitle.Title = item["Title1"].ToString();
                        itemTitle.Added_Date = DateTime.Now;
                        itemTitle.Added_By = userKey;
                        itemTitle.TotalSales = 0;
                        itemTitle.Is_Locked = false;
                        itemTitle.Is_Current = false;
                        itemTitle.Title_Index = 1;
                        sellerItem.ItemTitles.Add(itemTitle);
                        
                    }
                }

                if (item.ContainsKey("Title2") && !string.IsNullOrEmpty(item["Title2"].ToString()))
                {
                    if (item.ContainsKey("ItemTitleId2") && !string.IsNullOrEmpty(item["ItemTitleId2"]))
                    {
                        int itemTitleId1 = Convert.ToInt32(item["ItemTitleId2"]);
                        itemTitle = context.ItemTitles.SingleOrDefault(a => a.ItemTitleId == itemTitleId1);
                        if (itemTitle != null)
                        {
                            if (!string.IsNullOrEmpty(itemTitle.Title) && itemTitle.Title.ToString().ToLower() != item["Title2"].ToString().ToLower())
                                itemTitle.TotalSales = 0;
                            itemTitle.ItemId = itemCode;
                            itemTitle.Title = item["Title2"].ToString();
                            itemTitle.Updated_Date = DateTime.Now;
                            itemTitle.Updated_By = userKey;
                        }
                    }
                    else
                    {
                        itemTitle = new ItemTitle();
                        itemTitle.ItemId = itemCode;
                        itemTitle.Title = item["Title2"].ToString();
                        itemTitle.Added_Date = DateTime.Now;
                        itemTitle.Added_By = userKey;
                        itemTitle.TotalSales = 0;
                        itemTitle.Is_Locked = false;
                        itemTitle.Is_Current = false;
                        itemTitle.Title_Index = 2;
                        sellerItem.ItemTitles.Add(itemTitle);
                    }
                }

                if (item.ContainsKey("Title3") && !string.IsNullOrEmpty(item["Title3"].ToString()))
                {
                    if (item.ContainsKey("ItemTitleId3") && !string.IsNullOrEmpty(item["ItemTitleId3"]))
                    {
                        int itemTitleId1 = Convert.ToInt32(item["ItemTitleId3"]);
                        itemTitle = context.ItemTitles.SingleOrDefault(a => a.ItemTitleId == itemTitleId1);
                        if (itemTitle != null)
                        {
                            if (!string.IsNullOrEmpty(itemTitle.Title) && itemTitle.Title.ToString().ToLower() != item["Title3"].ToString().ToLower())
                                itemTitle.TotalSales = 0;
                            itemTitle.ItemId = itemCode;
                            itemTitle.Title = item["Title3"].ToString();
                            itemTitle.Updated_Date = DateTime.Now;
                            itemTitle.Updated_By = userKey;
                        }
                    }
                    else
                    {
                        itemTitle = new ItemTitle();
                        itemTitle.ItemId = itemCode;
                        itemTitle.Title = item["Title3"].ToString();
                        itemTitle.Added_Date = DateTime.Now;
                        itemTitle.Added_By = userKey;
                        itemTitle.TotalSales = 0;
                        itemTitle.Is_Locked = false;
                        itemTitle.Is_Current = false;
                        itemTitle.Title_Index = 3;
                        sellerItem.ItemTitles.Add(itemTitle);
                    }
                }

                if (item.ContainsKey("Title4") && !string.IsNullOrEmpty(item["Title4"].ToString()))
                {
                    if (item.ContainsKey("ItemTitleId4") && !string.IsNullOrEmpty(item["ItemTitleId4"]))
                    {
                        int itemTitleId1 = Convert.ToInt32(item["ItemTitleId4"]);
                        itemTitle = context.ItemTitles.SingleOrDefault(a => a.ItemTitleId == itemTitleId1);
                        if (itemTitle != null)
                        {
                            if (!string.IsNullOrEmpty(itemTitle.Title) && itemTitle.Title.ToString().ToLower() != item["Title4"].ToString().ToLower())
                                itemTitle.TotalSales = 0;
                            itemTitle.ItemId = itemCode;
                            itemTitle.Title = item["Title4"].ToString();
                            itemTitle.Updated_Date = DateTime.Now;
                            itemTitle.Updated_By = userKey;
                        }
                    }
                    else
                    {
                        itemTitle = new ItemTitle();
                        itemTitle.ItemId = itemCode;
                        itemTitle.Title = item["Title4"].ToString();
                        itemTitle.Added_Date = DateTime.Now;
                        itemTitle.Added_By = userKey;
                        itemTitle.TotalSales = 0;
                        itemTitle.Is_Locked = false;
                        itemTitle.Is_Current = false;
                        itemTitle.Title_Index = 4;
                        sellerItem.ItemTitles.Add(itemTitle);
                    }
                }

                if (item.ContainsKey("Title5") && !string.IsNullOrEmpty(item["Title5"].ToString()))
                {
                    if (item.ContainsKey("ItemTitleId5") && !string.IsNullOrEmpty(item["ItemTitleId5"]))
                    {
                        int itemTitleId1 = Convert.ToInt32(item["ItemTitleId5"]);
                        itemTitle = context.ItemTitles.SingleOrDefault(a => a.ItemTitleId == itemTitleId1);
                        if (itemTitle != null)
                        {
                            if (!string.IsNullOrEmpty(itemTitle.Title) && itemTitle.Title.ToString().ToLower() != item["Title5"].ToString().ToLower())
                                itemTitle.TotalSales = 0;
                            itemTitle.ItemId = itemCode;
                            itemTitle.Title = item["Title5"].ToString();
                            itemTitle.Updated_Date = DateTime.Now;
                            itemTitle.Updated_By = userKey;
                        }
                    }
                    else
                    {
                        itemTitle = new ItemTitle();
                        itemTitle.ItemId = itemCode;
                        itemTitle.Title = item["Title5"].ToString();
                        itemTitle.Added_Date = DateTime.Now;
                        itemTitle.Added_By = userKey;
                        itemTitle.TotalSales = 0;
                        itemTitle.Is_Locked = false;
                        itemTitle.Is_Current = false;
                        itemTitle.Title_Index = 5;
                        sellerItem.ItemTitles.Add(itemTitle);
                    }
                }

                context.SaveChanges();

                if (sellerItem.Is_Title_Automated == true)
                {
                    string title = sellerItem.ItemTitles.FirstOrDefault().Title;
                    if (!string.IsNullOrEmpty(title))
                    {
                        //Saad
                        
                    }
                }

               
                
            }

            return "1";
        }
        catch (Exception ex)
        {
            Logging.WriteLog(LogType.Error, ex.ToString());
            return "0";
        }


    }

    [OperationContract]
    public string UpdatePricingItems(string editedSellerItems, bool isSavetoFile, int itemCodeFile)
    {
        try
        {
            int userKey = new Base().UserKey;
            if (userKey == 0)
            {
                return "{\"login\": false}";
            }
            SellerItem item = (SellerItem)Common.Deserialize(editedSellerItems, typeof(SellerItem));
            DataModelEntities context = new DataModelEntities();
            if (item != null)
            {
                SellerItem sellerItem = context.SellerItems.Single(s => s.Item_Code == item.Item_Code);
                sellerItem.Is_Automated = item.Is_Automated;
                sellerItem.Ceiling_Price = item.Ceiling_Price;
                sellerItem.Floor_Price = item.Floor_Price;
                sellerItem.Algo = item.Algo;
                sellerItem.Keywords = item.Keywords;
                sellerItem.Keywords = item.Keywords;
                sellerItem.Minimum_Feedback = item.Minimum_Feedback;
                sellerItem.Maximum_Feedback = item.Maximum_Feedback;
                sellerItem.Minimum_Price = item.Minimum_Price;
                sellerItem.Maximum_Price = item.Maximum_Price;
                sellerItem.Minimum_Quantity = item.Minimum_Quantity;
                sellerItem.Maximum_Quantity = item.Maximum_Quantity;
                sellerItem.Inclued_Sellers = item.Inclued_Sellers;
                sellerItem.Exclude_Sellers = item.Exclude_Sellers;
                sellerItem.Maximum_Handling_Time = item.Maximum_Handling_Time;
                sellerItem.Is_Fixed_Price = item.Is_Fixed_Price;
                sellerItem.Is_Auctions = item.Is_Auctions;
                sellerItem.Is_Returns_Accepted = item.Is_Returns_Accepted;
                sellerItem.Is_Location_AU = item.Is_Location_AU;
                sellerItem.Is_Hide_Duplicates = item.Is_Hide_Duplicates;
                sellerItem.Is_Top_Rated_Only = item.Is_Top_Rated_Only;
                sellerItem.Exclude_Category_Codes = item.Exclude_Category_Codes;
                sellerItem.Include_Condtion_Codes = item.Include_Condtion_Codes;
                sellerItem.Less_To_Lowest_Price = item.Less_To_Lowest_Price;
                sellerItem.Ignore_Words = item.Ignore_Words;
                sellerItem.Is_Round_To_Nearest = item.Is_Round_To_Nearest;
                sellerItem.File_Item_Code = itemCodeFile;
                sellerItem.Country_Code = item.Country_Code;
                sellerItem.LocatedIn = item.LocatedIn;

                if (isSavetoFile == true)
                {
                    SellerItemFile sellerItemFile = null;
                    //if (itemCodeFile == 0)
                    //    sellerItemFile = new SellerItemFile();
                    //else
                    //    sellerItemFile = context.SellerItemFiles.Single(w => w.File_Item_Code == itemCodeFile);

                    sellerItemFile = new SellerItemFile();
                    if (sellerItemFile != null)
                    {

                        sellerItemFile.Item_Code = sellerItem.Item_Code;
                        sellerItemFile.Is_Automated = item.Is_Automated;
                        sellerItemFile.Ceiling_Price = item.Ceiling_Price;
                        sellerItemFile.Floor_Price = item.Floor_Price;
                        sellerItemFile.Algo = item.Algo;
                        sellerItemFile.Keywords = item.Keywords;
                        sellerItemFile.Keywords = item.Keywords;
                        sellerItemFile.Minimum_Feedback = item.Minimum_Feedback;
                        sellerItemFile.Maximum_Feedback = item.Maximum_Feedback;
                        sellerItemFile.Minimum_Price = item.Minimum_Price;
                        sellerItemFile.Maximum_Price = item.Maximum_Price;
                        sellerItemFile.Minimum_Quantity = item.Minimum_Quantity;
                        sellerItemFile.Maximum_Quantity = item.Maximum_Quantity;
                        sellerItemFile.Inclued_Sellers = item.Inclued_Sellers;
                        sellerItemFile.Exclude_Sellers = item.Exclude_Sellers;
                        sellerItemFile.Maximum_Handling_Time = item.Maximum_Handling_Time;
                        sellerItemFile.Is_Fixed_Price = item.Is_Fixed_Price;
                        sellerItemFile.Is_Auctions = item.Is_Auctions;
                        sellerItemFile.Is_Returns_Accepted = item.Is_Returns_Accepted;
                        sellerItemFile.Is_Location_AU = item.Is_Location_AU;
                        sellerItemFile.Is_Hide_Duplicates = item.Is_Hide_Duplicates;
                        sellerItemFile.Is_Top_Rated_Only = item.Is_Top_Rated_Only;
                        sellerItemFile.Exclude_Category_Codes = item.Exclude_Category_Codes;
                        sellerItemFile.Include_Condtion_Codes = item.Include_Condtion_Codes;
                        sellerItemFile.Less_To_Lowest_Price = item.Less_To_Lowest_Price;
                        sellerItemFile.Ignore_Words = item.Ignore_Words;
                        sellerItemFile.Is_Round_To_Nearest = item.Is_Round_To_Nearest;
                        sellerItemFile.Is_Active = true;
                        sellerItemFile.Created_Date = System.DateTime.Now;
                        sellerItemFile.Country_Code = item.Country_Code;
                        sellerItemFile.LocatedIn = item.LocatedIn;
                        //if (itemCodeFile == 0)
                        //    context.SellerItemFiles.AddObject(sellerItemFile);

                        context.SellerItemFiles.AddObject(sellerItemFile);
                        context.SaveChanges();

                        sellerItem.File_Item_Code = sellerItemFile.File_Item_Code;
                    }

                }




                context.SaveChanges();
                string sellerItemJson = GetPricingItems(item.Item_Code, null, null, null, null, null, null);
                if (sellerItem.Is_Automated == true)
                {
                    System.Threading.Thread t = new System.Threading.Thread(() => UpdateProductPrice(editedSellerItems, sellerItem, userKey));
                    t.Start();
                }
                return sellerItemJson;
            }
            else
                return null;

        }
        catch (Exception ex)
        {
            Logging.WriteLog(LogType.Error, ex.ToString());
            return null;
        }
    }

    private void UpdateProductPrice(string sellerItemJson, SellerItem sellerItem, int userCode)
    {
        ParcelService ps = new ParcelService();
        string TokenJSON = ps.GetTokenJSON(userCode);//"{\"brucewl1964\": 62, \"williamgerardit\": 64}";
        int pageSize = 20;
        string response = GetProductRank(sellerItemJson, pageSize, TokenJSON, userCode);
        if (!string.IsNullOrEmpty(response))
        {
            ps.UpdateProductPrice(response, sellerItem);
        }
    }

    [OperationContract]
    public string GetExpressSetupUsers(string Name, string BusinessName, int StatusCode)
    {
        try
        {
            DataModelEntities entities = new DataModelEntities();
            var users = entities.ExpressSetupUsers.Include("Package").Where(m =>
                                                              (string.IsNullOrEmpty(Name) == true || m.Full_Name.Contains(Name))
                                                              &&
                                                              (string.IsNullOrEmpty(BusinessName) == true || m.Business_Name.Contains(BusinessName))
                                                              &&
                                                              (StatusCode == 0 || m.Status_Code == StatusCode)
                                                        )
                                                  .AsEnumerable()
                                                  .Select(s => new
                                                  {
                                                      s.Package.Package_Name,
                                                      s.Package_Id,
                                                      s.Express_Setup_User_Code,
                                                      s.Full_Name,
                                                      s.Business_Name,
                                                      s.Ebay_Store_URL,
                                                      s.Daily_Orders,
                                                      s.Is_Registered_PS,
                                                      s.Login_Email_PS,
                                                      s.Time_Using_EParcel,
                                                      s.Phone_Number,
                                                      s.Best_Time_Call,
                                                      s.Email_Address,
                                                      s.Is_Active,
                                                      s.Status_Code,
                                                      Status = Common.GetStatus(s.Status_Code)
                                                  }).ToList();
            return Common.Serialize(users);
        }
        catch (Exception ex)
        {
            Logging.WriteLog(LogType.Error, ex.ToString());
            return null;
        }

    }

    [OperationContract]
    public bool UpdateStatusExpressSetupUsers(int[] userCodes, int statusCode)
    {
        try
        {
            int userCode = new Base().UserKey;
            DataModelEntities entities = new DataModelEntities();
            for (int i = 0; i < userCodes.Length; i++)
            {
                int setupUserCode = userCodes[i];
                var user = entities.ExpressSetupUsers.FirstOrDefault(f => f.Express_Setup_User_Code == setupUserCode);
                if (user != null)
                {
                    user.Status_Code = statusCode;
                    user.Modified_By = userCode;
                    user.Modified_Date = System.DateTime.Now;
                    entities.SaveChanges();
                }
            }
            return true;
        }
        catch (Exception ex)
        {
            Logging.WriteLog(LogType.Error, ex.ToString());
            return false;
        }
    }

    [OperationContract]
    public bool? SaveDefaultSettings(string editedSellerItems)
    {
        try
        {

            SellerItem item = (SellerItem)Common.Deserialize(editedSellerItems, typeof(SellerItem));
            int userCode = new Base().UserKey;
            DataModelEntities entities = new DataModelEntities();
            var user = entities.Users.FirstOrDefault(f => f.User_Code == userCode);
            if (user != null)
            {
                user.Ceiling_Price = item.Ceiling_Price;
                user.Floor_Price = item.Floor_Price;
                user.Algo = item.Algo;
                user.Minimum_Feedback = item.Minimum_Feedback;
                user.Maximum_Feedback = item.Maximum_Feedback;
                user.Minimum_Price = item.Minimum_Price;
                user.Maximum_Price = item.Maximum_Price;
                user.Minimum_Quantity = item.Minimum_Quantity;
                user.Maximum_Quantity = item.Maximum_Quantity;
                user.Inclued_Sellers = item.Inclued_Sellers;
                user.Exclude_Sellers = item.Exclude_Sellers;
                user.Maximum_Handling_Time = item.Maximum_Handling_Time;
                user.Is_Fixed_Price = item.Is_Fixed_Price;
                user.Is_Auctions = item.Is_Auctions;
                user.Is_Returns_Accepted = item.Is_Returns_Accepted;
                user.Is_Location_AU = item.Is_Location_AU;
                user.Is_Hide_Duplicates = item.Is_Hide_Duplicates;
                user.Is_Top_Rated_Only = item.Is_Top_Rated_Only;
                user.Exclude_Category_Codes = item.Exclude_Category_Codes;
                user.Include_Condtion_Codes = item.Include_Condtion_Codes;
                user.Less_To_Lowest_Price = item.Less_To_Lowest_Price;
                user.Is_Round_To_Nearest = item.Is_Round_To_Nearest;
                //user.Country_Code = item.Country_Code;
                user.LocatedIn = item.LocatedIn;
                entities.SaveChanges();
                return true;
            }
            return false;

        }
        catch (Exception ex)
        {
            Logging.WriteLog(LogType.Error, ex.ToString());
            return false;
        }
    }

    [OperationContract]
    public string GetDefaultSettings()
    {
        try
        {
            int userCode = new Base().UserKey;
            DataModelEntities entities = new DataModelEntities();
            var user = entities.Users.Where(u => u.User_Code == userCode).Select(s => new
            {
                s.Ceiling_Price,
                s.Floor_Price,
                s.Algo,
                s.Minimum_Feedback,
                s.Maximum_Feedback,
                s.Minimum_Price,
                s.Maximum_Price,
                s.Minimum_Quantity,
                s.Maximum_Quantity,
                s.Inclued_Sellers,
                s.Exclude_Sellers,
                s.Maximum_Handling_Time,
                s.Is_Fixed_Price,
                s.Is_Auctions,
                s.Is_Returns_Accepted,
                s.Is_Location_AU,
                s.Is_Hide_Duplicates,
                s.Is_Top_Rated_Only,
                s.Exclude_Category_Codes,
                s.Include_Condtion_Codes,
                s.Less_To_Lowest_Price,
                s.Is_Round_To_Nearest,
                s.LocatedIn
            });

            if (user != null)
            {
                return Common.Serialize(user);
            }
            else
                return null;

        }
        catch (Exception ex)
        {
            Logging.WriteLog(LogType.Error, ex.ToString());
            return null;
        }
    }

    [OperationContract]
    public string PriceHistoryDatewise(int itemCode, object fromDate, object toDate)
    {
        //DateTime tdate = System.DateTime.Today.Date;//Convert.ToDateTime(toDate).Date;
        //DateTime fdate = tdate.AddDays(-10).Date;//Convert.ToDateTime(fromDate).Date;

        DateTime tdate = System.DateTime.Today.Date;//Convert.ToDateTime(toDate).Date;
        DateTime fdate = tdate.AddDays(-35);//Convert.ToDateTime(fromDate).Date;

        List<ArrayList> chartdata = new List<ArrayList>();

        DataModelEntities entities = new DataModelEntities();
        IList<HitRate> hitrate = entities.PricingHistories.Where(w =>
                                                                 w.Item_Code == itemCode  &&
            EntityFunctions.TruncateTime(w.Created_Date) >= fdate &&
            EntityFunctions.TruncateTime(w.Created_Date) <= tdate 
                                                                 ).AsEnumerable().Select(s => new HitRate { price = s.New_Price, date = s.Created_Date }).OrderByDescending(o => o.date).Take(15).ToList();

            int days = tdate.Subtract(fdate.Date).Days + 1;

            
            //for (int i = 0; i < days; i++)
            //{
            //    ArrayList hitArray = new ArrayList();
            //    hitArray.Add(fdate.AddDays(i).Date.ToString("MMM dd yyyy"));
            //    hitArray.Add(-0);
            //    chartdata.Add(hitArray);
            //}
            hitrate.Reverse();
            foreach (var item in hitrate)
            {
                //foreach (var hit in chartdata)
                //{
                //    //if (hit[0].ToString() == Convert.ToDateTime(item.date).ToString("MMM dd yyyy"))
                //    //{
                //    //    hit[1] = item.price;
                //    //}

                //    hit[0] = Convert.ToDateTime(item.date).ToString("MMM dd yyyy");
                //    hit[1] = item.price;
                //}
                ArrayList hit = new ArrayList();
                hit.Add(Convert.ToDateTime(item.date).ToString("MMM dd yyyy"));
                hit.Add(item.price);
                chartdata.Add(hit);

            }

            List<List<ArrayList>> l = new List<List<ArrayList>>();
            l.Add(chartdata);
            if (l.Count > 0)
                return Common.Serialize(l);
            else
            {
                return null;
            }
      
    }

    [OperationContract]
    public string GetPricingHistory(int itemCode, object fromDate, object toDate)
    {
        //DateTime tdate = System.DateTime.Today.Date;//Convert.ToDateTime(toDate).Date;
        //DateTime fdate = tdate.AddDays(-10).Date;//Convert.ToDateTime(fromDate).Date;

        //DateTime tdate = System.DateTime.Today.Date;//Convert.ToDateTime(toDate).Date;
        //DateTime fdate = tdate.AddDays(-60).Date;//Convert.ToDateTime(fromDate).Date;


        DataModelEntities entities = new DataModelEntities();
        var history = entities.PricingHistories.Where(w => w.Item_Code == itemCode  
                                                                 
                                                                 ).AsEnumerable().Select(s => new
                                                                 {
                                                                     Created_Date = Convert.ToDateTime(s.Created_Date).ToString("dd-MMM-yyyy"),
                                                                     Algo = Common.GetAlgoName(s.Algo),
                                                                     Keyword = s.Keyword,
                                                                     Old_Price = s.Old_Price,
                                                                     New_Price = s.New_Price
                                                                 }).ToList();
        //EntityFunctions.TruncateTime(w.Created_Date) >= fdate &&
        //EntityFunctions.TruncateTime(w.Created_Date) <= tdate).AsEnumerable().Select(s => new HitRate { price = s.New_Price, date = s.Created_Date }).ToList();

        return Common.Serialize(history);
    }

    [OperationContract]
    public string GetTitleHistory(int itemCode, object fromDate, object toDate)
    {
        DateTime tdate = System.DateTime.Today.Date;//Convert.ToDateTime(toDate).Date;
        DateTime fdate = tdate.AddDays(-30).Date;//Convert.ToDateTime(fromDate).Date;


        DataModelEntities entities = new DataModelEntities();
        var history = entities.TitleHistories.Where(w =>
                                                                 w.Item_Code == itemCode).AsEnumerable().Select(s => new
                                                                 {
                                                                     Created_Date = Convert.ToDateTime(s.Created_Date).ToString("dd-MMM-yyyy"),
                                                                     New_Title = s.New_Title,
                                                                     Old_Title = s.Old_Title,
                                                                     Total_Sales = s.Total_Sales
                                                                     
                                                                 }).ToList();
        //EntityFunctions.TruncateTime(w.Created_Date) >= fdate &&
        //EntityFunctions.TruncateTime(w.Created_Date) <= tdate).AsEnumerable().Select(s => new HitRate { price = s.New_Price, date = s.Created_Date }).ToList();

        return Common.Serialize(history);
    }

    [OperationContract]
    public string DeactivateAll(int? userAccountCode, string categoryID, string searchColumn, string searchValue, int? country)
    {
        int userKey = new Base().UserKey;
        string query = "UPDATE SellerItem SET Is_Automated = 0 WHERE User_Code = " + userKey;
        DataModelEntities entities = new DataModelEntities();
        entities.ExecuteStoreCommand(query);
        return GetPricingItems(null, userAccountCode, categoryID, searchColumn, searchValue, false, country);
    }



}
