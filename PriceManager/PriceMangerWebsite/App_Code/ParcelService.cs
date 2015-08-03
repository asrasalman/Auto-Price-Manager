using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using PriceManagerDAL;
using System.Data.Objects;

/// <summary>
/// Summary description for ParcelService
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
// [System.Web.Script.Services.ScriptService]
public class ParcelService : System.Web.Services.WebService {

    public ParcelService () {

        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }

    [WebMethod]
    public bool SendEmailNotifications()
    {
        try
        {
            string link = @"http://autopricemanager.com";//HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority);
            DataModelEntities context = new DataModelEntities();
            DateTime date = System.DateTime.Now.AddDays(-2);
            List<User> users3 = context.Users.Where(u => u.Is_Active == true  && u.Is_Subscribed == true && EntityFunctions.TruncateTime(u.Created_Date) == date.Date ).ToList();
            date = System.DateTime.Now.AddDays(-6);
            List<User> users7 = context.Users.Where(u => u.Is_Active == true && u.Is_Subscribed == true && EntityFunctions.TruncateTime(u.Created_Date) == date.Date).ToList();
            date = System.DateTime.Now.AddDays(-12);
            List<User> users13 = context.Users.Where(u => u.Is_Active == true && u.Is_Subscribed == true && EntityFunctions.TruncateTime(u.Created_Date) == date.Date).ToList();
            date = System.DateTime.Now.AddDays(-19);
            List<User> users20 = context.Users.Where(u => u.Is_Active == true && u.Is_Subscribed == true && EntityFunctions.TruncateTime(u.Created_Date) == date.Date).ToList();

            if (users3.Count > 0)
            {
                System.Threading.Thread t = new System.Threading.Thread(() => SendEmailNotifications(users3, (int)Common.EmailTemplates.Day3, link, "Auto Price Manager - Hands FREE AUTO Pricing!"));
                t.Start();
            }

            if (users7.Count > 0)
            {
                System.Threading.Thread t = new System.Threading.Thread(() => SendEmailNotifications(users7, (int)Common.EmailTemplates.Day7, link, "Auto Price Manager - Take Back Control of your Business!"));
                t.Start();
            }

            if (users13.Count > 0)
            {
                System.Threading.Thread t = new System.Threading.Thread(() => SendEmailNotifications(users13, (int)Common.EmailTemplates.Day13, link, "Auto Price Manager - Your Account will close tomorrow!"));
                t.Start();
            }

            if (users20.Count > 0)
            {
                System.Threading.Thread t = new System.Threading.Thread(() => SendEmailNotifications(users13, (int)Common.EmailTemplates.Day20, link, "Auto Price Manager - Be Great to have you back!"));
                t.Start();
            }

            /*Lock/Suspend user after trial 14days if they dont pay */
            date = System.DateTime.Now.AddDays(-13);
            List<User> usersTrailEnded = context.Users.Where(u => u.Role_Code == 2 && u.Is_Active == true && EntityFunctions.TruncateTime(u.Created_Date) == date.Date && (u.Is_Paypal_Paid != true || u.Is_Paypal_Expired == true)).ToList();
            foreach (var user in usersTrailEnded)
            {
                user.Is_Locked = true;
            }
            context.SaveChanges();

            return true;
        }
        catch (Exception ex)
        {
            return false;
        }
    }

   

    public void SendEmailNotifications(List<User> users, int NotificationType, string link, string subject)
    {
        string emailMessage = Email.GetTemplateString(NotificationType);
        foreach (var user in users)
        {
            string smsg = emailMessage;
            string verificionLink =  link + @"/pages/Activation.aspx?vc=" + user.Confirmation_Code;
            string unsubscribeList =  link + @"/pages/Unsubscribe.aspx?vc=" + user.Confirmation_Code;
            smsg = smsg.Replace("{User_Name}", user.Full_Name);
            smsg = smsg.Replace("{Verification_Link}", verificionLink);
            smsg = smsg.Replace("{First_Name}", user.First_Name);
            smsg = smsg.Replace("{Unsubscribe_Link}", unsubscribeList);
            Email.SendMail(user.Email_Address, subject, smsg, null);    
        }
    }

    [WebMethod]
    public bool RefreshTransactions() {
        try
        {
            DataModelEntities context = new DataModelEntities();
            List<User> users = context.Users
                .Where(u => 
                    u.Is_Active == true &&
                    u.Is_Locked != true &&
                    u.Is_Verified == true &&
                    u.Is_Paypal_Paid == true //&&
                   // (u.Is_Paypal_Expired == null || u.Is_Paypal_Expired != true)
                    ).ToList();
            ParcelBL parcelBL = new ParcelBL();
            foreach (PriceManagerDAL.User user in users)
            {
                int UserCode = user.User_Code;
                ParcelBL objParcelBL = new ParcelBL();
                /*Save Ebay's All Account's items to seller item table in order to automate pricing*/
                objParcelBL.SaveEbayUserItems(UserCode);
            }
            return true;
        }
        catch (Exception ex)
        {
            return false;
        }
    }

    [WebMethod]
    public bool UpdateProductPrice(int timeDelay)
    {

        try
        {
            int pageSize = 20;
            DateTime today = System.DateTime.Today;
            DataModelEntities context = new DataModelEntities();
            var sellerItems = context.SellerItems.Where(w => w.User.Is_Active == true && w.User.Is_Locked != true && w.User.Is_Verified == true && ((timeDelay == (int)Constant.PriceAutomationTimeDelay.Hrs12 && (w.User.Automation_Time_Delay == null || w.User.Automation_Time_Delay == (int)Constant.PriceAutomationTimeDelay.NotSpecified)) || (w.User.Automation_Time_Delay == timeDelay)) && w.Is_Active == true && w.Is_Automated == true && w.Is_Promo_Item != true && w.End_Date > today)
                                                 .AsEnumerable()
                                                 .Select(a => new
                                                 {
                                                     a.Algo,
                                                     a.BIN_Price,
                                                     a.Ceiling_Price,
                                                     a.Current_Price,
                                                     a.End_Date,
                                                     a.Floor_Price,
                                                     a.Item_Code,
                                                     a.Item_ID,
                                                     a.Item_Name,
                                                     a.Item_SKU,
                                                     a.Start_Date,
                                                     a.Keywords,
                                                     a.Is_Automated,
                                                     a.User_Code,
                                                     a.User_Account_Code,
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
                                                     a.Item_Category_ID,
                                                     a.Item_Category_Name,
                                                     a.Less_To_Lowest_Price,
                                                     a.Item_Rank,
                                                     a.Ignore_Words,
                                                     a.Is_Round_To_Nearest,
                                                     a.Country_Code,
                                                     a.LocatedIn
                                                 }).ToList();
            
            if (sellerItems.Count > 0)
            {
                foreach (var sellerItem in sellerItems)
                {
                    try
                    {
                        string TokenJSON = GetTokenJSON((int)sellerItem.User_Code);
                        GeneralSvc genSvc = new GeneralSvc();
                        string filterJSON = Common.Serialize(sellerItem);
                        string response = genSvc.GetProductRank(filterJSON, pageSize, TokenJSON, sellerItem.User_Code);
                        if (!string.IsNullOrEmpty(response))
                        {
                            SellerItem si = context.SellerItems.First(f => f.Item_Code == sellerItem.Item_Code);
                            UpdateProductPrice(response, si);
                        }
                    }
                    catch (Exception ex) 
                    {
                        Logging.WriteLog(LogType.Error, ex.ToString());
                    }
                }
                context.SaveChanges();
            }
            return true;
        }
        catch (Exception ex)
        {
            Logging.WriteLog(LogType.Error, ex.ToString());
            return false;
        }
    }

    [WebMethod]
    public bool UpdateProductTitle(int timeDelay)
    {
        try
        {
            int pageSize = 20;
            DateTime today = System.DateTime.Today;
            DataModelEntities context = new DataModelEntities();
            var sellerItems = context.SellerItems.Where(w => w.User.Is_Active == true && w.User.Is_Locked != true && w.User.Is_Verified == true && timeDelay == w.Rotate_Days && w.Is_Active == true && w.Is_Title_Automated == true && w.Is_Promo_Item != true && w.End_Date > today)
                                                 .AsEnumerable()
                                                 .Select(a => new
                                                 {
                                                     a.Is_Title_Automated,
                                                     a.Item_Name,
                                                     a.Item_Code,
                                                     a.Item_ID,
                                                     a.ItemTitles,
                                                     a.Rotate_Days,
                                                     a.Rotate_Order,
                                                     a.Rotate_Sales,
                                                     a.Current_Sales,
                                                     a.User_Code
                                                 }).ToList();

            if (sellerItems.Count > 0)
            {
                foreach (var sellerItem in sellerItems)
                {
                    try
                    {
                        //write here logic for the revise title.
                        if (sellerItem.ItemTitles != null && sellerItem.ItemTitles.Count > 0 && sellerItem.ItemTitles.Any(a => a.Is_Locked != true))
                        {
                            ItemTitle currentTitle = sellerItem.ItemTitles.FirstOrDefault(f => f.Is_Current == true);
                            ItemTitle newTitle = null;
                            if (currentTitle != null)
                            {
                                int currentTitleIndex = currentTitle.Title_Index == 5 ? 1 : (int)currentTitle.Title_Index;
                                newTitle = sellerItem.ItemTitles.FirstOrDefault(f => f.Title_Index == currentTitleIndex);
                            }
                            else
                            {
                                newTitle = sellerItem.ItemTitles.FirstOrDefault(f => f.Title_Index == sellerItem.Rotate_Order); 
                            }

                            if (newTitle != null)
                            {
                                UpdateProductTitle(sellerItem.Item_Code, newTitle, currentTitle);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Logging.WriteLog(LogType.Error, ex.ToString());
                    }
                }
                context.SaveChanges();
            }
            return true;
        }
        catch (Exception ex)
        {
            Logging.WriteLog(LogType.Error, ex.ToString());
            return false;
        }
    }

   public void UpdateProductTitle(int sellerItemCode, ItemTitle itemTitle, ItemTitle oldTitle)
   {
       DataModelEntities context = new DataModelEntities();
       SellerItem sellerItem = context.SellerItems.First(f => f.Item_Code == sellerItemCode);
       

       EbayServiceBL service = new EbayServiceBL((int)sellerItem.User_Code, (int)sellerItem.Country_Code);
       string result = service.ReviseEbayItemTitle(sellerItem.Item_ID, itemTitle.Title, service.UserTokens[(int)sellerItem.User_Account_Code]);
      
       /*In case of success service returns null*/
       if (string.IsNullOrEmpty(result))
       {
           TitleHistory ph = new TitleHistory();
           ph.Item_Code = sellerItemCode;
           ph.New_Title = itemTitle.Title;
           ph.Old_Title = oldTitle == null ? sellerItem.Item_Name : oldTitle.Title;
           ph.Total_Sales = oldTitle == null ? sellerItem.Current_Sales : oldTitle.TotalSales; 
           ph.Created_Date = System.DateTime.Now;
           context.TitleHistories.AddObject(ph);
           context.SaveChanges();
           Logging.WriteLog(LogType.Info, sellerItem.Item_ID + " Title revised.");
       }
       else
           Logging.WriteLog(LogType.Error, result);
   }

    public void UpdateProductPrice(string productRankResponse, SellerItem sellerItem)
    {
        EbayServiceBL service = new EbayServiceBL((int)sellerItem.User_Code, (int)sellerItem.Country_Code);
        double? averagePrice = null;
        double newPrice;
        double? AvgORLow = null;
        double floorPrice = Convert.ToDouble(sellerItem.Floor_Price);
        double ceilingPrice = Convert.ToDouble(sellerItem.Ceiling_Price);
        int? rank = null;
        double? shippingCost = null;
        


        List<EbaySearchItem> searchItems = (List<EbaySearchItem>)Common.Deserialize(productRankResponse, typeof(List<EbaySearchItem>));
        string ItemID = sellerItem.Item_ID;

        EbaySearchItem myItem = searchItems.FirstOrDefault(f => f.ItemID == ItemID);
        if (myItem != null)
        {
            shippingCost = myItem.ShippingCost;
            rank = searchItems.IndexOf(myItem);
        }
        else
        {
            var myItemResponse = service.SearchItemsByID(ItemID);
            var myItemresult = myItemResponse.searchResult;
            if (myItemresult != null && myItemresult.count > 0)
            {
                var myItemSearched = myItemresult.item.FirstOrDefault(f => f.itemId == ItemID);
                shippingCost = myItemSearched.shippingInfo.shippingServiceCost != null ? myItemSearched.shippingInfo.shippingServiceCost.Value : 0;
                //shippingCost = 0;
            }                 
        }

        List<EbaySearchItem> excludedMySearchItems = searchItems.Where(w => w.IsMyProduct == false).ToList();
        EbaySearchItem minPriceSearchItem = excludedMySearchItems.OrderBy(o => o.TotalCostIncludingShipping).FirstOrDefault();
        EbaySearchItem maxPriceSearchItem = excludedMySearchItems.OrderByDescending(o => o.TotalCostIncludingShipping).FirstOrDefault();

        /*Get averagePrice Price From Seached Items Excluded User's Items*/

        if (excludedMySearchItems != null)
            averagePrice = excludedMySearchItems.Sum(s => s.TotalCostIncludingShipping) / excludedMySearchItems.Count;

        /*Set New Price Of Item According to Algo which have been selected by user*/

        if (sellerItem.Algo == Convert.ToString((int)Common.Algo.Lowest))
        {
            if (sellerItem.Less_To_Lowest_Price == null)
                AvgORLow = minPriceSearchItem != null ? (double?)(minPriceSearchItem.TotalCostIncludingShipping - 0.1) : null;
            else
                AvgORLow = minPriceSearchItem != null ? (double?)(minPriceSearchItem.TotalCostIncludingShipping - Convert.ToDouble(sellerItem.Less_To_Lowest_Price)) : null;
                //AvgORLow = minPriceSearchItem != null ? (double?)(minPriceSearchItem.TotalCost - (minPriceSearchItem.TotalCost * Convert.ToDouble(sellerItem.Less_To_Lowest_Price))) : null; it was before 9th april 2014
        }
        else if (sellerItem.Algo == Convert.ToString((int)Common.Algo.Average))
            AvgORLow = averagePrice;
        else if (sellerItem.Algo == Convert.ToString((int)Common.Algo.MatchLowest))
            AvgORLow = minPriceSearchItem.TotalCostIncludingShipping;
        else
            AvgORLow = null;

        /*IF AvgORLow is set and not equals to null means price will be updated */

        if (AvgORLow != null)
        {
            newPrice = AvgORLow >= floorPrice &&
                       AvgORLow <= ceilingPrice ?
                       Convert.ToDouble(AvgORLow) :
                       AvgORLow > ceilingPrice ?
                       ceilingPrice :
                       floorPrice;

            if (sellerItem.User.Automation_Include_Shipping == true && sellerItem.User.Country1.Country_Abbr.ToUpper() == sellerItem.LocatedIn.ToUpper())
            {
                if (shippingCost == null)
                    return;

                newPrice = newPrice - Convert.ToDouble(shippingCost);
            }

            if (sellerItem.Is_Round_To_Nearest == true)
                newPrice = Math.Floor(newPrice / 0.10) * 0.10;


            if (sellerItem.Current_Price == Convert.ToDecimal(newPrice))
                return;

            string result = service.ReviseEbayItem(sellerItem.Item_ID, (double)newPrice, service.UserTokens[(int)sellerItem.User_Account_Code]);

            /*In case of success service returns null*/
            if (string.IsNullOrEmpty(result))
            {
                
                DataModelEntities context = new DataModelEntities();
                SellerItem si = context.SellerItems.First(f => f.Item_Code == sellerItem.Item_Code);

                PricingHistory ph = new PricingHistory();
                ph.Algo = si.Algo;
                ph.Keyword = si.Keywords;
                ph.Item_Code = si.Item_Code;
                ph.Old_Price = si.Current_Price;
                ph.New_Price = Convert.ToDecimal(newPrice);
                ph.Created_Date = System.DateTime.Now;
                ph.Currency = sellerItem.Currency;
                context.PricingHistories.AddObject(ph);

                si.Current_Price = Convert.ToDecimal(newPrice);
                si.Item_Rank = rank != null ? rank + 1 : null;

                context.SaveChanges();
                if (sellerItem.Floor_Price == Convert.ToDecimal(newPrice))
                {
                    /*Send Floor limit Reaced Notification*/
                    System.Threading.Thread t = new System.Threading.Thread(() => SendFloorLimitReachedAlert(sellerItem));
                    t.Start();
                    
                }

                if (sellerItem.Ceiling_Price == Convert.ToDecimal(newPrice))
                {
                    /*Send Floor limit Reaced Notification*/
                    System.Threading.Thread t = new System.Threading.Thread(() => SendFloorLimitReachedAlert(sellerItem));
                    t.Start();

                }
                Logging.WriteLog(LogType.Info, sellerItem.Item_ID + " Price revised.");
            }
            else
                Logging.WriteLog(LogType.Error, result);
        }
    }

    [WebMethod]
    public bool UpdateProductRank()
    {
        try
        {
            //string TokenJSON = GetTokenJSON(26);//"{\"brucewl1964\": 62, \"williamgerardit\": 64}";
            int pageSize = 20;
            DateTime today = System.DateTime.Today;
            DataModelEntities context = new DataModelEntities();
            var sellerItems = context.SellerItems.Where(w => w.Is_Active == true && w.Is_Automated == true && w.Is_Promo_Item == false && w.End_Date > today)
                                                 .AsEnumerable()
                                                 .Select(a => new 
                                                 {
                                                        a.Algo,
                                                        a.BIN_Price,
                                                        a.Ceiling_Price,
                                                        a.Current_Price,
                                                        a.End_Date,
                                                        a.Floor_Price,
                                                        a.Item_Code,
                                                        a.Item_ID,
                                                        a.Item_Name,
                                                        a.Item_SKU,
                                                        a.Start_Date,
                                                        a.Keywords,
                                                        a.Is_Automated,
                                                        a.User_Code,
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
                                                        a.Item_Category_ID,
                                                        a.Item_Category_Name,
                                                        a.Less_To_Lowest_Price,
                                                        a.Item_Rank,
                                                        a.Ignore_Words,
                                                        a.Is_Round_To_Nearest,
                                                        a.Country_Code,
                                                        a.LocatedIn

                                                 }).ToList();
                                                    

            if (sellerItems.Count > 0)
            {
                foreach (var sellerItem in sellerItems)
                {
                    try
                    {
                        string TokenJSON = GetTokenJSON((int)sellerItem.User_Code);
                        GeneralSvc genSvc = new GeneralSvc();
                        string filterJSON = Common.Serialize(sellerItem);
                        string response = genSvc.GetProductRank(filterJSON, pageSize, TokenJSON, sellerItem.User_Code);
                        if (!string.IsNullOrEmpty(response))
                        {
                            List<EbaySearchItem> searchItems = (List<EbaySearchItem>)Common.Deserialize(response, typeof(List<EbaySearchItem>));
                            string ItemID = sellerItem.Item_ID;
                            EbaySearchItem myItem = searchItems.FirstOrDefault(f => f.ItemID == ItemID);
                            if (myItem != null)
                            {

                                int rank = searchItems.IndexOf(myItem);
                                SellerItem si = context.SellerItems.First(f => f.Item_Code == sellerItem.Item_Code);
                                si.Item_Rank = rank + 1;
                            }
                            else
                            {
                                SellerItem si = context.SellerItems.First(f => f.Item_Code == sellerItem.Item_Code);
                                si.Item_Rank = null;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Logging.WriteLog(LogType.Error, ex.ToString());
                    }
                }
                context.SaveChanges();
            }
            return true;
        }
        catch (Exception ex)
        {
            Logging.WriteLog(LogType.Error, ex.ToString());
            return false;
        }
    }

    public void SendFloorLimitReachedAlert(PriceManagerDAL.SellerItem item)
    {
        string msg = Email.GetTemplateString((int)Common.EmailTemplates.FloorLimitReachedAlert);
        string alertItems = "";
        alertItems = "<table><thead><tr><th>Item</th><th style='text-align:right'>Current Price</th></tr></thead><tbody>";
        alertItems += "<tr><td>" + item.Item_ID + " - " + item.Item_Name + "</td><td style='text-align:right'>" + item.Floor_Price + "</td></tr>";
        alertItems += "</tbody></table>";
        msg = msg.Replace("{User_Name}", item.User.Full_Name);
        msg = msg.Replace("{Items}", alertItems);
        Email.SendMail(item.User.Email_Address, "Item Has Reached Floor Limit Notification", msg, null);
    }

    public void SendCeilingLimitReachedAlert(PriceManagerDAL.SellerItem item)
    {
        string msg = Email.GetTemplateString((int)Common.EmailTemplates.CeilingLimitReachedAlert);
        string alertItems = "";
        alertItems = "<table><thead><tr><th>Item</th><th style='text-align:right'>Current Price</th></tr></thead><tbody>";
        alertItems += "<tr><td>" + item.Item_ID + " - " + item.Item_Name + "</td><td style='text-align:right'>" + item.Ceiling_Price + "</td></tr>";
        alertItems += "</tbody></table>";
        msg = msg.Replace("{User_Name}", item.User.Full_Name);
        msg = msg.Replace("{Items}", alertItems);
        Email.SendMail(item.User.Email_Address, "Item Has Reached Ceiling Limit Notification", msg, null);
    }

    public string GetTokenJSON(int userCode)
    {
        Dictionary<string, int> localTokens = new Dictionary<string, int>();
        EbayServiceBL service = new EbayServiceBL(userCode);
        foreach (KeyValuePair<int, string> pair in service.UserTokens)
        {
            string result = service.GetUser(pair.Value);

            localTokens.Add(result, pair.Key);
        }
        string tokenJSON = Common.Serialize(localTokens);
        return tokenJSON;
    }
    

    
    
}
