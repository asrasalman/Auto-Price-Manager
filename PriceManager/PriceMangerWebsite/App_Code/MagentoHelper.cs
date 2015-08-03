using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PriceManagerDAL;
using com.magento.www;

/// <summary>
/// Summary description for MagentoHelper
/// </summary>
public class MagentoHelper
{
    int UserAccountCode;
    int UserCode;

    public MagentoHelper(int UserKey)
    {
        UserCode = UserKey;
        UserAccount account = new DataModelEntities().UserAccounts.FirstOrDefault(u => u.User_Code == UserKey && u.Account_Code == (int)Constant.Accounts.Magento && u.Is_Active == true);
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


                MagentoService service = new MagentoService();
                service.Url = userAccount.Application_Name;

                string sessionID = service.login(userAccount.Config_Value1, userAccount.Config_Value2);
                List<associativeEntity> listAssociatedEntity = new List<associativeEntity>();
                listAssociatedEntity.Add(new associativeEntity() { key = "status", value = "pending" });

                filters filters = new filters();
                filters.filter = listAssociatedEntity.ToArray();
                

                salesOrderListEntity[] orders = service.salesOrderList(sessionID, filters);



                List<ChargeCode> chargeCodes = context.ChargeCodes.Where(u => u.Is_Active == true && u.User_Code == UserCode).ToList();

                foreach (salesOrderListEntity order in orders)
                {
                    salesOrderEntity orderDetail = service.salesOrderInfo(sessionID, order.increment_id);

                    foreach (salesOrderItemEntity lineItem in orderDetail.items)
                    {

                        ParcelItem item = new ParcelItem();

                        item.Type = "MAGENTO";
                        item.AccountID = userAccount.User_Account_Code.ToString();
                        item.RecordNumber = orderDetail.order_id;
                        item.ItemID = lineItem.item_id;
                        item.TransactionID = orderDetail.increment_id;
                        item.ItemName = lineItem.name;
                        item.CustomLabel = lineItem.sku;
                        item.CustomLabelText = lineItem.sku;

                        string state = orderDetail.shipping_address.region;
                        string stateCode = orderDetail.shipping_address.region_id;

                        if (StateHelper.States.Where(s => s.Key == stateCode).Count() > 0)
                            item.State = StateHelper.States[stateCode];
                        else
                            item.State = state;

                        item.BuyerName = orderDetail.customer_firstname + " " + orderDetail.customer_lastname;
                        item.EmailAddress = orderDetail.customer_email;
                        item.BuyerID = orderDetail.customer_id;

                        item.Street2 = orderDetail.shipping_address.street;
                        item.Street3 = string.Empty;
                        item.City = orderDetail.shipping_address.city;
                        item.PostalCode = orderDetail.shipping_address.postcode;
                        item.Country = orderDetail.shipping_address.country_id;
                        item.Phone = orderDetail.shipping_address.telephone;

                        item.Quantity = lineItem.qty_ordered == string.Empty ? 0 : (int)decimal.Parse(lineItem.qty_ordered);
                        if (orderDetail.shipping_amount != string.Empty)
                            item.ShippingCost = double.Parse(orderDetail.shipping_amount);
                        item.SaleRecordId = orderDetail.increment_id;

                        // insurance details //


                        item.Currency = orderDetail.order_currency_code;
                        item.Price = double.Parse(lineItem.price);



                        item.ShippingMethod = orderDetail.shipping_description;

                        string shippingCode = orderDetail.shipping_description;
                        ChargeCode code = chargeCodes.FirstOrDefault(u => shippingCode.ToLower().Contains(u.Ebay_Code.ToLower()) == true);
                        if (code != null && code.Charge_Code_Name.ToLower() == "ignore")
                        {
                            continue; // ignore the item
                        }

                        bool IspostCodeOK = Common.VerifyPostCode(orderDetail.shipping_address.postcode, orderDetail.shipping_address.city);
                        if (IspostCodeOK)
                            item.PostCodeImageURL = Constant.tickURL;
                        else
                            item.PostCodeImageURL = Constant.crossURL;

                        items.Add(item);
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


    public bool FulfillOrder(UserAccount userAccount, string orderID, string trackingNumber, string carrier)
    {
        try
        {
            DataModelEntities context = new DataModelEntities();

            MagentoService service = new MagentoService();
            service.Url = userAccount.Application_Name;

            string sessionID = service.login(userAccount.Config_Value1, userAccount.Config_Value2);

            string result = service.salesOrderShipmentCreate(sessionID, orderID, null, null, 0, 0);
            int trackingResult = service.salesOrderShipmentAddTrack(sessionID, orderID, carrier, "Tracking", trackingNumber);

            return true;
        }
        catch (Exception ex)
        {
            Logging.WriteLog(LogType.Error, "Magento Fulfill Order - OrderID : " + orderID + ": " + ex.Message);
            return false;
        }
    }


    public bool TestMagentoCredentials(string url, string apiUserID, string apiKey)
    {
        try
        {
            MagentoService service = new MagentoService();
            service.Url = url;

            string sessionID = service.login(apiUserID, apiKey);

            salesOrderListEntity[] orders = service.salesOrderList(sessionID, null);
            return true;
        }
        catch (Exception)
        {
            return false;
        }

    }
}