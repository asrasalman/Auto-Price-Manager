using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PriceManagerDAL;
using com.magento.www;
using Api = BigCommerce4Net.Api;
using Domain = BigCommerce4Net.Domain;

/// <summary>
/// Summary description for BigcommerceHelper
/// </summary>
public class BigcommerceHelper
{
    int UserAccountCode;
    int UserCode;

    public BigcommerceHelper(int UserKey)
    {
        UserCode = UserKey;
        UserAccount account = new DataModelEntities().UserAccounts.FirstOrDefault(u => u.User_Code == UserKey && u.Account_Code == (int)Constant.Accounts.Bigcommerce && u.Is_Active == true);
        if (account != null)
            UserAccountCode = account.User_Account_Code;
        else
            UserAccountCode = 0;
    }

    public List<ParcelItem> GetPendingOrders()
    {
        try
        {
            if (UserAccountCode != 0)
            {
                List<ParcelItem> items = new List<ParcelItem>();
                DataModelEntities context = new DataModelEntities();
                UserAccount userAccount = context.UserAccounts.First(u => u.User_Account_Code == UserAccountCode);
                List<ChargeCode> chargeCodes = context.ChargeCodes.Where(u => u.Is_Active == true && u.User_Code == UserAccountCode).ToList();

                Api.Configuration Api_Configuration = new Api.Configuration()
                {
                    ServiceURL = userAccount.Application_Name,
                    UserName = userAccount.Config_Value1,
                    UserApiKey = userAccount.Config_Value2
                };
                
                Api.Client Client = new Api.Client(Api_Configuration);
                var filter = new Api.FilterOrders
                {
                    StatusId = 9// awaiting shipment
                };

                var response = Client.Orders.Get(filter);
                
                if (response.Data != null)
                {
                    List<PriceManagerDAL.ParcelItem> parcelItems = context.ParcelItems.Where(f => f.User_Code == userAccount.User_Code && f.UserAccount.User_Account_Code == UserAccountCode && f.Is_Active == true).ToList();

                    // delete all database entries
                    foreach (PriceManagerDAL.ParcelItem existingItem in parcelItems)
                    {
                        context.ParcelItems.DeleteObject(existingItem);
                    }
                    context.SaveChanges();


                    foreach (var order in response.Data)
                    {
                        var Products = Client.OrdersProducts.Get(order.Id);
                        foreach (var product in Products.Data)
	                    {
                            ParcelItem item = new ParcelItem();
                            item.Type = "BIGCOMMERCE";
                            item.AccountID = userAccount.User_Account_Code.ToString();
                            item.RecordNumber = order.Id.ToString();
                            item.ItemID = product.Id.ToString();
                            item.TransactionID = order.Id.ToString();
                            item.ItemName = product.ProductName;
                            item.CustomLabel = product.Sku;
                            item.CustomLabelText = product.Sku;
                            

                            var shippingAddress = Client.OrdersShippingAddresses.Get(order.Id, product.OrderAddressId).Data;//order.ShippingAddresses.SingleOrDefault(s => s.Id == product.OrderAddressId);
                            string state = shippingAddress.State;
                            string stateCode = shippingAddress.State;

                            if (StateHelper.States.Where(s => s.Key == stateCode).Count() > 0)
                                item.State = StateHelper.States[stateCode];
                            else
                                item.State = state;

                            var customer = Client.Customers.Get(order.CustomerId).Data;
                            if (customer != null)
                            {
                                item.BuyerName = customer.FirstName + " " + customer.LastName;
                            }
                            item.EmailAddress = order.BillingAddress.Email;
                            item.BuyerID = order.CustomerId.ToString();
                            item.Street2 = shippingAddress.Street1;
                            item.Street3 = shippingAddress.Street2;
                            item.City = shippingAddress.City;
                            item.PostalCode = shippingAddress.ZipCode;
                            item.Country = shippingAddress.Country;
                            item.Phone = shippingAddress.Phone;
                            item.AddressID = product.OrderAddressId;


                            item.Quantity = product.Quantity;
                            item.ShippingCost = (double)order.ShippingCostIncludingTax;
                            item.SaleRecordId = order.Id.ToString();

                            // insurance details //
                            item.Currency = order.CurrencyCode;
                            item.Price = (double)product.PriceIncludingTax;

                            string shippingCode = null;
                            item.ShippingMethod = shippingAddress.ShippingMethod;
                            shippingCode = shippingAddress.ShippingMethod;
                           
                            
                            ChargeCode code = chargeCodes.FirstOrDefault(u => shippingCode.ToLower().Contains(u.Ebay_Code.ToLower()) == true);
                            if (code != null && code.Charge_Code_Name.ToLower() == "ignore")
                            {
                                continue; // ignore the item
                            }

                            bool IspostCodeOK = Common.VerifyPostCode(item.PostalCode, item.City);
                            if (IspostCodeOK)
                                item.PostCodeImageURL = Constant.tickURL;
                            else
                                item.PostCodeImageURL = Constant.crossURL;

                            items.Add(item);
	                    }
                        
                    }
                }
                return items;
            }
            else
            {
                return null;
            }
        }
        catch (Exception ex)
        {
            return null;
        }
    }

    public bool FulfillOrder(UserAccount userAccount, string orderID, string itemID, int Qty, int OrderAddressID,  string trackingNumber)
    {
        try
        {
            DataModelEntities context = new DataModelEntities();
            
            Api.Configuration Api_Configuration = new Api.Configuration()
            {
                ServiceURL = userAccount.Application_Name,
                UserName = userAccount.Config_Value1,
                UserApiKey = userAccount.Config_Value2
            };

            Api.Client Client = new Api.Client(Api_Configuration);
            
            Dictionary<object, object> createShippment = new Dictionary<object, object>();
            createShippment.Add("tracking_number", trackingNumber);
            createShippment.Add("comments", string.Empty);
            createShippment.Add("order_address_id", OrderAddressID);
            
            List<object> orderItmes = new List<object>();
            Dictionary<object, object> item = new Dictionary<object, object>();
            item.Add("order_product_id", int.Parse(itemID));
            item.Add("quantity", Qty);
            orderItmes.Add(item);
            
            createShippment.Add("items", orderItmes);


            string JSON = Common.Serialize(createShippment);

            var shippment = Client.OrdersShipments.Create(int.Parse(orderID), JSON);
            if (shippment.Data != null)
                return true;
            else
                return false;
        }
        catch (Exception ex)
        {
            Logging.WriteLog(LogType.Error, "Bigcommerce Fulfill Order - OrderID : " + orderID + ": " + ex.Message);
            return false;
        }
    }

    public bool TestBigcommerceCredential(string url, string userId, string apiKey)
    {
        try
        {
            Api.Configuration Api_Configuration = new Api.Configuration()
            {
                ServiceURL = url,
                UserName = userId,
                UserApiKey = apiKey
            };
            
            Api.Client Client = new Api.Client(Api_Configuration);
            var filter = new Api.FilterOrders
            {
                Limit = 1
            };
            var response = Client.Orders.Get(filter);
            if (response.RestResponse.StatusCode == System.Net.HttpStatusCode.OK)
                return true;
            else
                return false;
        }
        catch (Exception ex)
        {
            return false;
        }

    }
   
}