using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PriceManagerDAL;
using ShopifyAPIAdapterLibrary;
using System.Web.Script.Serialization;
using System.Configuration;

/// <summary>
/// Summary description for ShopifyHelper
/// </summary>
public class ShopifyHelper
{
    string ConsumerKey = ConfigurationManager.AppSettings["ShopifyConsumerKey"];
    string ConsumerSecret = ConfigurationManager.AppSettings["ShopifyConsumerSecret"];
    string ShopifyScope = "read_products,read_orders,write_orders";
    public List<int> UserAccountCodes;

    public ShopifyHelper(int UserCode)
    {
        
        DataModelEntities context = new DataModelEntities();
        int spAccountCode = (int)Constant.Accounts.Shopify;

        List<UserAccount> userAccounts = context.UserAccounts.Where(u => u.User_Code == UserCode && u.Is_Active == true && u.Account_Code == spAccountCode).ToList();

        if (userAccounts.Count > 0)
        {
            UserAccountCodes = userAccounts.Select(u => u.User_Account_Code).ToList();
        }
        else
        {
            UserAccountCodes = new List<int>();
        }
        context = null;
    }

    public string GetAuthURL(string shopName, string currentUri)
    {
        var authorizer = new ShopifyAPIAuthorizer(shopName, ConsumerKey, ConsumerSecret);

        // get the Authorization URL and redirect the user
        var authUrl = authorizer.GetAuthorizationURL(new string[] { ShopifyScope }, currentUri);
        return authUrl;
    }

    public string ExchangeToken(string tempToken, string shopName)
    {
        var authorizer = new ShopifyAPIAuthorizer(shopName, ConsumerKey, ConsumerSecret);

        // get the authorization state
        ShopifyAuthorizationState authState = authorizer.AuthorizeClient(tempToken);

        if (authState != null && authState.AccessToken != null)
        {
            return authState.AccessToken;
        }
        else
            return null;
    }

    public List<ParcelItem> GetPendingOrders(int userCode, int userAccountCode)
    {
        DataModelEntities context = new DataModelEntities();
        
        UserAccount userAccount = context.UserAccounts.First(u => u.User_Account_Code == userAccountCode);
        List<ChargeCode> chargeCodes = context.ChargeCodes.Where(u => u.Is_Active == true && u.User_Code == userCode).ToList();


        ShopifyAuthorizationState authState = new ShopifyAuthorizationState();
        authState.AccessToken = userAccount.Config_Value1;
        authState.ShopName = userAccount.Application_Name;

        ShopifyAPIClient api = new ShopifyAPIClient(authState);

        try
        {
            // by default JSON string is returned
            dynamic data = new JavaScriptSerializer().DeserializeObject(api.Get("/admin/orders.json?fulfillment_status=unshipped").ToString());

            List<PriceManagerDAL.ParcelItem> parcelItems = context.ParcelItems.Where(f => f.User_Code == userCode && f.UserAccount.Account_Code == (int)Constant.Accounts.Shopify && f.Is_Active == true).ToList();

            // delete all database entries
            foreach (PriceManagerDAL.ParcelItem existingItem in parcelItems)
            {
                context.ParcelItems.DeleteObject(existingItem);
            }
            context.SaveChanges();


            List<ParcelItem> items = new List<ParcelItem>();

            foreach (dynamic order in data["orders"])
            {
                foreach (dynamic lineItem in order["line_items"])
                {
                    ParcelItem item = new ParcelItem();

                    item.Type = "SHOPIFY";
                    item.AccountID = userAccount.User_Account_Code.ToString();
                    item.RecordNumber = order["order_number"].ToString();
                    item.ItemID = lineItem["id"].ToString();
                    item.TransactionID = order["id"].ToString();
                    item.ItemName = lineItem["title"];
                    item.CustomLabel = lineItem["sku"].ToString();
                    item.CustomLabelText = lineItem["sku"].ToString();

                    string state = order["shipping_address"]["province"];
                    string stateCode = order["shipping_address"]["province_code"];

                    if (StateHelper.States.Where(s => s.Key == stateCode).Count() > 0)
                        item.State = StateHelper.States[stateCode];
                    else
                        item.State = state;

                    item.BuyerName = order["customer"]["first_name"] + " " + order["customer"]["last_name"];
                    item.EmailAddress = order["email"];
                    item.BuyerID = order["customer"]["id"].ToString();

                    item.Street2 = order["shipping_address"]["address1"];
                    item.Street3 = order["shipping_address"]["address2"];
                    item.City = order["shipping_address"]["city"];
                    item.PostalCode = order["shipping_address"]["zip"].TrimStart('0');
                    item.Country = order["shipping_address"]["country_code"];
                    item.Phone = order["shipping_address"]["phone"];

                    item.Quantity = lineItem["quantity"];
                    if (order["shipping_lines"] != null)
                        item.ShippingCost = double.Parse(order["shipping_lines"][0]["price"]);
                    item.SaleRecordId = order["order_number"].ToString();

                    // insurance details //


                    item.Currency = order["currency"];
                    item.Price = double.Parse(lineItem["price"]);

                    item.Messages = GetMessageText(order["note"],order["created_at"]);

                    item.ShippingMethod = order["shipping_lines"][0]["title"];

                    string shippingCode = order["shipping_lines"][0]["code"];
                    ChargeCode code = chargeCodes.FirstOrDefault(u => shippingCode.ToLower().Contains(u.Ebay_Code.ToLower()) == true);
                    if (code != null && code.Charge_Code_Name.ToLower() == "ignore")
                    {
                        continue; // ignore the item
                    }

                    bool IspostCodeOK = Common.VerifyPostCode(order["shipping_address"]["zip"], order["shipping_address"]["city"]);
                    if (IspostCodeOK)
                        item.PostCodeImageURL = Constant.tickURL;
                    else
                        item.PostCodeImageURL = Constant.crossURL;

                    items.Add(item);
                }
            }

            return items;
        }
        catch (Exception ex)
        {
            return null;
        }
    }

    private string GetMessageText(string note, string date)
    {
        if (string.IsNullOrEmpty(note) == false)
        {
            List<ParcelMessage> messages = new List<ParcelMessage>();
            ParcelMessage message = new ParcelMessage();
            message.Subject = "-";
            message.Date = DateTime.Parse(date);
            message.Text = note;

            messages.Add(message);
            return Common.Serialize(messages);
        }
        else
            return null;
    }

    public void FulfillOrder(int UserKey, UserAccount userAccount, string orderID, string trackingNumber, string itemID)
    {
        ShopifyAuthorizationState authState = new ShopifyAuthorizationState();
        authState.AccessToken = userAccount.Config_Value1;
        authState.ShopName = userAccount.Application_Name;

        ShopifyAPIClient api = new ShopifyAPIClient(authState, new JsonDataTranslator());

        string inputData = @"{""fulfillment"": {""tracking_number"": """ + trackingNumber + @""",""line_items"": [{id: """ + itemID + @"""}]}}";

        dynamic _inputData = new JavaScriptSerializer().DeserializeObject(inputData);

        // by default JSON string is returned
        dynamic data = new JavaScriptSerializer().DeserializeObject(api.Post("/admin/orders/" + orderID + "/fulfillments.json", _inputData).ToString());

    }
}