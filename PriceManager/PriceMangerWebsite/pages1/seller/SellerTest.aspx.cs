using com.ebay.developer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PriceManagerDAL;

public partial class pages_seller_SellerTest : Base
{
    protected void Page_Load(object sender, EventArgs e)
    {
        EbayServiceBL service = new EbayServiceBL(UserKey);
        List<ParcelItem> allEbayItems = new List<ParcelItem>();

        DataModelEntities context = new DataModelEntities();

        if (service.UserTokens != null)
        {
            foreach (KeyValuePair<int, string> account in service.UserTokens)
            {
               List<Seller_Item> items =  service.GetUserItems(account.Value);


               foreach (Seller_Item item in items)
               {
                   SellerItem dbItem = new SellerItem();
                   dbItem.BIN_Price = (decimal)item.BinPrice;
                   dbItem.Created_Date = DateTime.Now;
                   dbItem.Current_Price = (decimal)item.CurrentPrice;
                   dbItem.End_Date = item.EndDate;
                   dbItem.Is_Active = true;
                   dbItem.Item_ID = item.ItemID;
                   dbItem.Item_Name = item.ItemName;
                   dbItem.Start_Date = item.StartDate;

                   context.SellerItems.AddObject(dbItem);
               }

               context.SaveChanges();
            }

        }

    }
}