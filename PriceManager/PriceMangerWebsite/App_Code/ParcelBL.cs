using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PriceManagerDAL;
using com.ebay.developer;

/// <summary>
/// Summary description for ParcelBL
/// </summary>
public class ParcelBL
{

    public void SaveTransactions(List<ParcelItem> allItems, int UserCode)
    {
        DataModelEntities context = new DataModelEntities();
        PriceManagerDAL.User user = context.Users.FirstOrDefault(f => f.User_Code == UserCode);
        List<PriceManagerDAL.ParcelItem> eItems = context.ParcelItems.Where(p => p.User_Code == UserCode && p.Is_Active == true).ToList();
        Dictionary<int, int> itemsToUpdateQty = new Dictionary<int, int>();
        Dictionary<string, int> itemsToSendAlert = new Dictionary<string, int>();
        foreach (ParcelItem item in allItems)
        {
            try
            {
                // check if the item already does not exist in the database and insert it then.
                PriceManagerDAL.ParcelItem dbItem = eItems.FirstOrDefault(p => p.ItemID == item.ItemID && p.TransactionID == item.TransactionID);

                if (dbItem == null)
                {
                    dbItem = new PriceManagerDAL.ParcelItem();
                    dbItem.User_Code = UserCode;
                    dbItem.AccountID = int.Parse(item.AccountID);
                    dbItem.BuyerID = item.BuyerID;
                    dbItem.BuyerName = item.BuyerName;
                    dbItem.City = item.City;
                    dbItem.Country = item.Country;
                    dbItem.Created_Date = DateTime.Now;
                    dbItem.Currency = item.Currency;
                    dbItem.CustomLabel = item.CustomLabel;
                    dbItem.CustomLabelText = item.CustomLabelText;
                    dbItem.EmailAddress = item.EmailAddress;
                    dbItem.HasInsurance = item.HasInsurance;
                    dbItem.Insurance = Convert.ToDecimal(item.Insurance);
                    dbItem.Is_Active = true;
                    dbItem.ItemID = item.ItemID;
                    dbItem.ItemName = item.ItemName;
                    dbItem.Messages = item.Messages;
                    dbItem.Parcel_Status_Code = (int)Constant.ParcelStatusCode.Pending;
                    dbItem.Phone = item.Phone;
                    dbItem.PostalCode = item.PostalCode;
                    dbItem.PostCodeImageURL = item.PostCodeImageURL;
                    dbItem.Price = Convert.ToDecimal(item.Price);
                    dbItem.Quantity = item.Quantity;
                    dbItem.RecordNumber = item.RecordNumber;
                    dbItem.SaleRecordId = item.SaleRecordId;
                    dbItem.ShippingCost = Convert.ToDecimal(item.ShippingCost);
                    dbItem.ShippingMethod = item.ShippingMethod;
                    dbItem.State = item.State;
                    dbItem.Street = item.Street;
                    dbItem.Street2 = item.Street2;
                    dbItem.Street3 = item.Street3;
                    dbItem.TransactionID = item.TransactionID;
                    dbItem.Type = item.Type;
                    dbItem.AddressID = item.AddressID;
                    context.ParcelItems.AddObject(dbItem);

                }
                /*Create Order for inventory tracking*/
                PriceManagerDAL.Item product = context.Items.FirstOrDefault(f => f.CustomLabel == item.CustomLabel && f.UserCode == UserCode);
                if (product != null)
                {
                    /*First Check if item with the same ItemID and TransactionID does not exists in the order table*/
                    PriceManagerDAL.StockLedger checkorder = context.StockLedgers.FirstOrDefault(p => p.ItemID == item.ItemID && p.TransactionID == item.TransactionID && p.Type == item.Type && p.AccountID == item.AccountID);
                    if (checkorder == null)
                    {
                        PriceManagerDAL.StockLedger dborder = new PriceManagerDAL.StockLedger();
                        dborder.User_Code = UserCode;
                        dborder.AccountID = item.AccountID;
                        dborder.Type = item.Type;
                        dborder.TransactionID = item.TransactionID;
                        dborder.ItemID = item.ItemID;
                        dborder.CustomLabel = item.CustomLabel;
                        dborder.Quantity = item.Quantity * -1;
                        dborder.Narration = item.Type + " New Sale";
                        dborder.Stock_Ledger_Type = (int)Common.StockLegerType.NewSale;
                        dborder.Created_Date = System.DateTime.Now;
                        dborder.ID = product.ID;
                        context.AddToStockLedgers(dborder);

                        if (!itemsToUpdateQty.Any(a => a.Key == product.ID))
                            itemsToUpdateQty.Add(product.ID, item.Quantity);
                        else
                            itemsToUpdateQty[product.ID] = itemsToUpdateQty[product.ID] + item.Quantity;

                    }
                    else
                    {
                        /*Check if Qty is different from the existing order item*/
                        if (checkorder.Quantity != item.Quantity * -1)
                        {
                            if (!itemsToUpdateQty.Any(a => a.Key == product.ID))
                                itemsToUpdateQty.Add(product.ID, item.Quantity - (int)checkorder.Quantity);
                            else
                                itemsToUpdateQty[product.ID] = itemsToUpdateQty[product.ID] + (item.Quantity - (int)checkorder.Quantity);

                            checkorder.Quantity = item.Quantity;
                            checkorder.Modifed_Date = System.DateTime.Now;

                        }

                    }
                }
            }
            catch (Exception ex)
            {
                Logging.WriteLog(LogType.Critical, ex.ToString());
            }
        }
        /*Update Items Balance Qty*/
        foreach (var item in itemsToUpdateQty)
        {
            PriceManagerDAL.Item updateItem = context.Items.FirstOrDefault(f => f.ID == item.Key);
            if (updateItem != null && updateItem.Balance_Quantity != null)
            {
                int previousBalance = updateItem.Balance_Quantity != null ? (int)updateItem.Balance_Quantity : 0;
                int currentBalance = previousBalance - item.Value;


                /*Add items if user min threshold alert is on and item is reaching to min threshold level*/
                if (user.Minimum_Threshold_Alert == true && updateItem.Minimum_Threshold != null && previousBalance > updateItem.Minimum_Threshold && currentBalance <= updateItem.Minimum_Threshold)
                    itemsToSendAlert.Add(updateItem.CustomLabel + " - " + updateItem.Description, currentBalance);

                /*Update item's balance quantity*/
                updateItem.Balance_Quantity = currentBalance;

            }
        }
        context.SaveChanges();

        /*Send Email Notification For Minimum Threshold*/
        if (itemsToSendAlert.Count > 0)
        {
            System.Threading.Thread t = new System.Threading.Thread(() => SendMinimumThresholdAlert(itemsToSendAlert, user));
            t.Start();
        }



    }

    public void SendMinimumThresholdAlert(Dictionary<string, int> items, PriceManagerDAL.User user)
    {
        string msg = Email.GetTemplateString((int)Common.EmailTemplates.MinimumThresholdAlert);
        string alertItems = "";
        alertItems = "<table><thead><tr><th>Item</th><th style='text-align:right'>Qty</th></tr></thead><tbody>";
        foreach (var item in items)
        {
            alertItems += "<tr><td>" + item.Key + "</td><td style='text-align:right'>" + item.Value + "</td></tr>";
        }
        alertItems += "</tbody></table>";
        msg = msg.Replace("{User_Name}", user.Full_Name);
        msg = msg.Replace("{Items}", alertItems);
        Email.SendMail(user.Email_Address, "Item(s) Reorder Level Notification", msg, null);
    }

    public List<ParcelItem> GetEbayTransactions(int UserCode)
    {
        EbayServiceBL service = new EbayServiceBL(UserCode);
        List<ParcelItem> allEbayItems = new List<ParcelItem>();
        if (service.UserTokens != null)
        {
            foreach (KeyValuePair<int, string> account in service.UserTokens)
            {
                List<ParcelItem> ebayItems = GetEbayAccountTransactions(service, account,UserCode);
                if (ebayItems != null)
                    allEbayItems.AddRange(ebayItems);
            }
            return allEbayItems.OrderBy(a => a.Type).ThenBy(a => a.AccountID).ThenBy(a => a.BuyerID).ToList();
        }
        else
            return null;
    }

    private List<ParcelItem> GetEbayAccountTransactions(EbayServiceBL service, KeyValuePair<int, string> account, int UserCode)
    {
        int accountID = account.Key;
        DataModelEntities context = new DataModelEntities();
        List<PriceManagerDAL.ParcelItem> parcelItems = context.ParcelItems.Where(f => f.AccountID == accountID && f.User_Code == UserCode && f.Is_Active == true).ToList();

        string currentItemID = string.Empty, currentTransactionID = string.Empty, currentRecordNo = string.Empty;

        SellingManagerSoldOrderType[] results = service.GetPendingShipmentItems(account.Value);

        if(results == null)
            results = new SellingManagerSoldOrderType[0];

        // delete all database entries that does not exist in the API results.
        foreach (PriceManagerDAL.ParcelItem existingItem in parcelItems)
        {
            long transactionID = long.Parse(existingItem.TransactionID);
            SellingManagerSoldOrderType checkResult = results.FirstOrDefault(r => r.SellingManagerSoldTransaction.Count(c => c.ItemID == existingItem.ItemID && c.TransactionID == transactionID) > 0);
            if (checkResult == null)
            {
                context.ParcelItems.DeleteObject(existingItem);
            }
        }

        // delete entries which do not have correct shipping details
        foreach (PriceManagerDAL.ParcelItem existingItem in parcelItems.Where(p => string.IsNullOrEmpty(p.BuyerName) == true))
        {
            context.ParcelItems.DeleteObject(existingItem);
        }

        // now traverse through API results and save only those which are new
        List<ParcelItem> items = new List<ParcelItem>();
        if (results != null && results.Length > 0)
        {
            List<ChargeCode> chargeCodes = new DataModelEntities().ChargeCodes.Where(u => u.Is_Active == true && u.User_Code == UserCode).ToList();
            foreach (SellingManagerSoldOrderType result in results)
            {
                foreach (SellingManagerSoldTransactionType transaction in result.SellingManagerSoldTransaction)
                {
                    try
                    {

                        currentItemID = transaction.ItemID;
                        currentTransactionID = transaction.TransactionID.ToString();

                        // ignore if the item already exists in our database 
                        if (context.ParcelItems.FirstOrDefault(f => f.AccountID == accountID && f.ItemID == currentItemID && f.TransactionID == currentTransactionID) != null)
                        {
                            continue;
                        }

                        ParcelItem item = new ParcelItem();
                        item.Type = "EBAY";

                        SellingManagerSoldOrderType itemDetails = service.GetSaleRecordDetails(transaction.ItemID, transaction.TransactionID.ToString(), account.Value);

                        currentRecordNo = itemDetails.SaleRecordID.ToString();

                        item.AccountID = account.Key.ToString();
                        item.ItemID = transaction.ItemID;
                        item.TransactionID = transaction.TransactionID.ToString();
                        item.ItemName = transaction.ItemTitle;
                        item.CustomLabel = transaction.CustomLabel;
                        item.CustomLabelText = transaction.SaleRecordID.ToString() + ":" + transaction.CustomLabel;
                        if (itemDetails.ShippingAddress != null)
                        {
                            if (StateHelper.States.Where(s => s.Key == itemDetails.ShippingAddress.StateOrProvince.ToLower()).Count() > 0)
                                item.State = StateHelper.States[itemDetails.ShippingAddress.StateOrProvince.ToLower()];
                            else
                                item.State = itemDetails.ShippingAddress.StateOrProvince.ToLower();
                            item.BuyerName = itemDetails.ShippingAddress.Name;
                            item.Street = itemDetails.ShippingAddress.Street;
                            item.Street2 = itemDetails.ShippingAddress.Street1;
                            item.Street3 = itemDetails.ShippingAddress.Street2;
                            item.City = itemDetails.ShippingAddress.CityName;
                            item.PostalCode = itemDetails.ShippingAddress.PostalCode.TrimStart('0'); ;
                            item.Country = itemDetails.ShippingAddress.Country.ToString();
                            item.Phone = itemDetails.ShippingAddress.Phone;
                        }
                        else
                        {

                        }
                        item.EmailAddress = itemDetails.BuyerEmail;
                        item.BuyerID = itemDetails.BuyerID;
                        item.Quantity = transaction.QuantitySold;
                        if (itemDetails.ActualShippingCost != null)
                            item.ShippingCost = itemDetails.ActualShippingCost.Value;
                        item.SaleRecordId = itemDetails.SaleRecordID.ToString();

                        bool IspostCodeOK = true;
                        if (itemDetails.ShippingDetails != null)
                        {
                            if (itemDetails.ShippingDetails.InsuranceFee == null)
                                item.HasInsurance = false;
                            else
                            {
                                item.HasInsurance = true;
                                item.Insurance = itemDetails.ShippingDetails.InsuranceFee.Value;
                            }
                            if (itemDetails.ShippingDetails != null && itemDetails.ShippingDetails.ShippingServiceOptions != null)
                                item.ShippingMethod = itemDetails.ShippingDetails.ShippingServiceOptions[0].ShippingService;
                            else
                                item.ShippingMethod = "N/A";

                            IspostCodeOK = Common.VerifyPostCode(itemDetails.ShippingAddress.PostalCode, itemDetails.ShippingAddress.CityName);
                            if (IspostCodeOK)
                                item.PostCodeImageURL = Constant.tickURL;
                            else
                                item.PostCodeImageURL = Constant.crossURL;
                        }

                        if (itemDetails.SellingManagerSoldTransaction != null)
                        {
                            item.Currency = itemDetails.SellingManagerSoldTransaction[0].ItemPrice.currencyID.ToString();
                            item.Price = itemDetails.SellingManagerSoldTransaction[0].ItemPrice.Value;
                        }


                        item.RecordNumber = itemDetails.SaleRecordID.ToString();

                        ChargeCode code = chargeCodes.FirstOrDefault(u => item.ShippingMethod.ToLower().Contains(u.Ebay_Code.ToLower()) == true);
                        if (code != null && code.Charge_Code_Name.ToLower() == "ignore")
                        {
                            continue; // ignore the item
                        }

                        GetMemberMessagesResponseType messages = service.GetTransactionMessages(item.ItemID, item.BuyerID, account.Value);
                        if (messages.MemberMessage != null)
                        {
                            List<ParcelMessage> ebayMessages = service.ConvertEbayMessages(messages);
                            item.Messages = Common.Serialize(ebayMessages);
                        }

                        items.Add(item);

                        currentItemID = string.Empty;
                        currentRecordNo = string.Empty;
                        currentTransactionID = string.Empty;
                    }
                    catch (Exception ex)
                    {
                        Logging.WriteLog(LogType.Critical, ex.ToString());
                    }
                }
            }
            context.SaveChanges();
            return items.OrderBy(i => i.BuyerID).ToList();
        }
        else
        {
            context.SaveChanges();
            return null;
        }

    }

    public List<ParcelItem> GetShopifyTransactions(int UserCode)
    {
        ShopifyHelper shopify = new ShopifyHelper(UserCode);

        List<ParcelItem> allShopifyItems = new List<ParcelItem>();

        foreach (int userAccountCode in shopify.UserAccountCodes)
        {
            List<ParcelItem> shopifyOders = new ShopifyHelper(UserCode).GetPendingOrders(UserCode, userAccountCode);
            if (shopifyOders != null)
                allShopifyItems.AddRange(shopifyOders);
        }
        return allShopifyItems;
    }

    public List<ParcelItem> GetMagentoTransactions(int UserCode)
    {
        MagentoHelper magentoService = new MagentoHelper(UserCode);
        return magentoService.GetPendingOrders();
    }

    public List<ParcelItem> GetBigcommerceTransactions(int UserCode)
    {
        BigcommerceHelper bigcommerceService = new BigcommerceHelper(UserCode);
        return bigcommerceService.GetPendingOrders();
    }

    public void SaveEbayUserItems(int UserCode)
    {
        EbayServiceBL service = new EbayServiceBL(UserCode);
        DataModelEntities context = new DataModelEntities();
        DateTime today = DateTime.Today;
        if (service.UserTokens != null)
        {
            foreach (KeyValuePair<int, string> account in service.UserTokens)
            {
                try
                {
                   // List<Seller_Item> items = service.GetUserItems(account.Value);
                    List<Seller_Item> items = service.GetUserItemsList(account.Value);
                    foreach (Seller_Item item in items)
                    {
                        SellerItem sellerItem = context.SellerItems.FirstOrDefault(a => a.Is_Active == true && a.User_Account_Code == account.Key && a.Item_ID == item.ItemID && a.Country_Code == service.countryID);
                        if (sellerItem == null)
                        { 
                            if (item.EndDate.Date > today.Date)
                            {
                                /*Save to seller item*/
                                SellerItem dbItem = new SellerItem();
                                dbItem.BIN_Price = (decimal)item.BinPrice;
                                dbItem.Created_Date = DateTime.Now;
                                dbItem.Current_Price = (decimal)item.CurrentPrice;
                                dbItem.End_Date = item.EndDate;
                                dbItem.Is_Active = true;
                                dbItem.Item_ID = item.ItemID;
                                dbItem.Item_Name = item.ItemName;
                                dbItem.Start_Date = item.StartDate;
                                dbItem.Is_Automated = false;
                                dbItem.User_Code = UserCode;
                                dbItem.User_Account_Code = account.Key;
                                dbItem.Picture_URL = item.PictureURL;
                                dbItem.Item_View_URL = item.ItemViewURL;
                                dbItem.Is_Promo_Item = item.IsPromoItem;
                                dbItem.Item_Category_ID = item.CategoryID;
                                dbItem.Item_Category_Name = item.CategoryName;
                                dbItem.Currency = item.Currency;
                                dbItem.Country_Code = item.CountryCode;
                                dbItem.LocatedIn = item.CountryShortCode;
                                dbItem.Current_Sales = item.CurrentSales;
                                dbItem.QuantityAvailable = item.QuantityAvailable;
                                context.SellerItems.AddObject(dbItem);
                            }
                        }
                        else
                        {
                            
                            if (item.EndDate.Date > today.Date)
                            {
                                sellerItem.User_Account_Code = account.Key;
                                sellerItem.BIN_Price = (decimal)item.BinPrice;
                                sellerItem.Current_Price = (decimal)item.CurrentPrice;
                                sellerItem.End_Date = item.EndDate;
                                sellerItem.Item_Name = item.ItemName;
                                sellerItem.Start_Date = item.StartDate;
                                sellerItem.Is_Promo_Item = item.IsPromoItem;
                                sellerItem.Picture_URL = item.PictureURL;
                                sellerItem.Item_View_URL = item.ItemViewURL;
                                sellerItem.Item_Category_ID = item.CategoryID;
                                sellerItem.Item_Category_Name = item.CategoryName;
                                if (item.IsPromoItem == true && sellerItem.Is_Automated == true)
                                    sellerItem.Is_Automated = false;
                                sellerItem.Currency = item.Currency;
                                sellerItem.Country_Code = item.CountryCode;
                                sellerItem.Current_Sales = item.CurrentSales;
                                sellerItem.QuantityAvailable = item.QuantityAvailable;

                                ItemTitle currentTitle = context.ItemTitles.FirstOrDefault(f => f.ItemId == sellerItem.Item_Code && (f.Is_Current == true || f.Title.ToLower() == sellerItem.Item_Name.ToLower()));
                                
                                if (currentTitle != null)
                                {
                                    if(context.TitleHistories.Any(a => a.Item_Code == sellerItem.Item_Code))
                                    {
                                       var totalSales = context.GetSalesForOldTitles(sellerItem.Item_Code, sellerItem.Item_Name).FirstOrDefault();
                                       currentTitle.TotalSales = totalSales.TotalSales;
                                       
                                    }
                                }


                            }
                            else
                            {
                                sellerItem.Is_Automated = false;
                                sellerItem.Is_Active = false;
                            }
                            
                        }

                        if (1 == 1)
                        {
                            Decimal weightMajor = item.Weight != null ? Convert.ToDecimal(item.Weight) * 1000 : 0; //Weight Major comes in Kgs so converted in to gms
                            Decimal weightMinor = item.Weight != null ? Convert.ToDecimal(item.WeightMinor) : 0; //Weight Minor comes in gms
                            Decimal weight = (weightMajor + weightMinor) / 1000; // Added both major and minor and converted into kgs

                            Item partmaster = context.Items.FirstOrDefault(a => a.Item_ID == item.ItemID && a.UserCode == UserCode && a.Country_Code == service.countryID);
                            if (partmaster == null)
                            {
                                partmaster = new Item();
                                partmaster.Item_ID = item.ItemID;
                                partmaster.CustomLabel = item.CustomLabel;
                                partmaster.Description = item.ItemName;
                                partmaster.UserCode = UserCode;
                                partmaster.User_Account_Code = account.Key;
                                partmaster.Height = item.Height != null ? item.Height.ToString() : string.Empty;
                                partmaster.Length = item.Length != null ? item.Length.ToString() : string.Empty;
                                partmaster.Width = item.Width != null ? item.Width.ToString() : string.Empty;
                                partmaster.Weight = item.Weight != null || item.WeightMinor != null ? weight.ToString("0.00") : string.Empty;
                                partmaster.Balance_Quantity = item.Quantity;
                                partmaster.Current_Price = Convert.ToDecimal(item.CurrentPrice);
                                partmaster.BIN_Price = Convert.ToDecimal(item.BinPrice);
                                partmaster.Picture_URL = item.PictureURL;
                                partmaster.Item_View_URL = item.ItemViewURL;
                                partmaster.Item_Category_ID = item.CategoryID;
                                partmaster.Item_Category_Name = item.CategoryName;
                                partmaster.Start_Date = item.StartDate;
                                partmaster.End_Date = item.EndDate;
                                partmaster.User_Account_Code = account.Key;
                                partmaster.Detail_Description = item.Discription;
                                partmaster.Country_Code = item.CountryCode;
                                context.AddToItems(partmaster);
                            }
                            else
                            {
                                partmaster.CustomLabel = item.CustomLabel;
                                partmaster.Description = item.ItemName;
                                partmaster.UserCode = UserCode;
                                partmaster.User_Account_Code = account.Key;

                                if (string.IsNullOrEmpty(partmaster.Height))
                                    partmaster.Height = item.Height != null ? item.Height.ToString() : string.Empty;

                                if (string.IsNullOrEmpty(partmaster.Length))
                                    partmaster.Length = item.Length != null ? item.Length.ToString() : string.Empty;

                                if (string.IsNullOrEmpty(partmaster.Width))
                                    partmaster.Width = item.Width != null ? item.Width.ToString() : string.Empty;

                                if (string.IsNullOrEmpty(partmaster.Weight))
                                    partmaster.Weight = item.Weight != null || item.WeightMinor != null ? weight.ToString("0.00") : string.Empty;

                                partmaster.Height = item.Height != null ? item.Height.ToString() : string.Empty;
                                partmaster.Length = item.Length != null ? item.Length.ToString() : string.Empty;
                                partmaster.Width = item.Width != null ? item.Width.ToString() : string.Empty;
                                partmaster.Weight = item.Weight != null || item.WeightMinor != null ? weight.ToString("0.00") : string.Empty;
                                partmaster.Current_Price = Convert.ToDecimal(item.CurrentPrice);
                                partmaster.BIN_Price = Convert.ToDecimal(item.BinPrice);
                                partmaster.Picture_URL = item.PictureURL;
                                partmaster.Item_View_URL = item.ItemViewURL;
                                partmaster.Item_Category_ID = item.CategoryID;
                                partmaster.Item_Category_Name = item.CategoryName;
                                partmaster.Start_Date = item.StartDate;
                                partmaster.User_Account_Code = account.Key;
                                partmaster.End_Date = item.EndDate;
                                partmaster.Balance_Quantity = item.Quantity;
                                partmaster.Country_Code = item.CountryCode;
                                
                            }
                        }
                        context.SaveChanges();
                    }
                    //context.SaveChanges();
                }
                catch (Exception ex) 
                {
                    Logging.WriteLog(LogType.Critical, ex.ToString());
                }
                
            }

        }
    }


    public List<CategoryType> GetEbayCategories(int UserCode)
    {
        EbayServiceBL service = new EbayServiceBL(UserCode);
        List<CategoryType> allEbayCategories = new List<CategoryType>();
        if (service.UserTokens != null)
        {
            
            foreach (KeyValuePair<int, string> account in service.UserTokens)
            {
                CategoryType[] Categories = service.GetCategories(account.Value.ToString());
                if (Categories != null)
                    allEbayCategories.AddRange(Categories);
            }
            return allEbayCategories.ToList();
        }
        else
            return null;
    }

}