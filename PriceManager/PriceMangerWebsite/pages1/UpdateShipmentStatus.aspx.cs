using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using com.ebay.developer;
using System.IO;
using System.Configuration;
using System.Text;
using System.Text.RegularExpressions;
using PriceManagerDAL;


public partial class pages_UpdateShipmentStatus : Base
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            PriceManagerDAL.DataModelEntities context = new PriceManagerDAL.DataModelEntities();

            // check if atleast one account is configured for the current user.
            if (context.UserAccounts.Count(u => u.User_Code == UserKey && u.Is_Active == true) > 0)
            {
                hdfSettings.Value = "1";
            }
        }
    }

    protected void btnUploadManifest_Click(object sender, EventArgs e)
    {
        if (IsValid)
        {
            string filePath = UploadFile();
            if (filePath != null)
            {
                UpdateParcelItems(filePath);
            }
        }
    }

    private void UpdateParcelItems(string filePath)
    {
        lblAPIIssuesUpload.Visible = false;
        try
        {
            lblSkippedItems.Text = string.Empty;
            List<Manifest> manifestList = File.ReadLines(filePath).Select(line => LineToArray(line)).Select(m => new Manifest { TrackingNumber = m[10], Item_TransactionID = m[28] }).Skip(1).ToList();
            foreach (Manifest item in manifestList)
            {
                if (item.Item_TransactionID != string.Empty)
                {
                    string[] vals = item.Item_TransactionID.Split(':');

                    if (vals.Length == 4) // includes account ID , account Type , Item ID && Transaction ID
                    {
                        item.AccountID = vals[0];
                        item.AccountType = vals[1];
                        item.ItemID = vals[2];
                        item.TransactionID = vals[3];

                        if (item.AccountType == "EBAY")
                            UpdateEbaySale(item); // replace null with correct token
                        else if (item.AccountType == "SHOPIFY")
                            UpdateShopifySale(item);
                        else if (item.AccountType == "MAGENTO")
                            UpdateMagentoSale(item);
                        else if (item.AccountType == "BIGCOMMERCE")
                            UpdateBigcommerceSale(item);
                    }
                    else if (vals.Length == 3) // includes account ID, Item ID && Transaction ID
                    {
                        item.AccountID = string.Empty;
                        item.AccountType = vals[0];
                        item.ItemID = vals[1];
                        item.TransactionID = vals[2];

                        if (item.AccountType == "EBAY")
                            UpdateEbaySale(item); // replace null with correct token
                        else if (item.AccountType == "SHOPIFY")
                            UpdateShopifySale(item);
                        else if (item.AccountType == "MAGENTO")
                            UpdateMagentoSale(item);
                        else if (item.AccountType == "BIGCOMMERCE")
                            UpdateBigcommerceSale(item);

                    }
                    else // includes Item ID && Transaction ID
                    {
                        item.AccountID = string.Empty;
                        item.AccountType = "EBAY";
                        item.ItemID = vals[0];
                        item.TransactionID = vals[1];

                        UpdateEbaySale(item); // replace null with correct token

                    }

                }
                else
                {
                    lblSkippedItems.Text += item.TrackingNumber + ", ";
                }
            }
            if (lblSkippedItems.Text != string.Empty)
            {
                lblSkippedItems.Text = "Following tracking numbers were not updated because ItemID/TransactionID pair was not found: " + lblSkippedItems.Text.Substring(0, lblSkippedItems.Text.Length - 3);
            }
        }
        catch (InvalidEbayCredentialsException ex)
        {
            lblAPIIssuesUpload.Visible = true;
            lblAPIIssuesUpload.Text = "The Ebay API settings are invalid. Please check back with your account or contact Ebay Support";
        }
        catch (Exception ex)
        {
            lblAPIIssuesUpload.Visible = true;
            lblAPIIssuesUpload.Text = "There is some issue connecting to the Ebay server. Please check back in a while.";
        }
    }

    private void UpdateBigcommerceSale(Manifest item)
    {
        DataModelEntities context = new DataModelEntities();
        UserAccount account;
        if (item.AccountID == string.Empty)
        {
            account = context.UserAccounts.Where(u => u.User_Code == UserKey & u.Account_Code == (int)Constant.Accounts.Bigcommerce && u.Is_Active == true).OrderBy(u => u.User_Account_Code).First();
        }
        else
        {
            int userAccountCode = int.Parse(item.AccountID);
            account = context.UserAccounts.First(u => u.User_Account_Code == userAccountCode);

        }

        if(!string.IsNullOrEmpty(item.TransactionID) &&  !string.IsNullOrEmpty(item.ItemID))
        {
            var parcelItem = context.ParcelItems.FirstOrDefault(f => f.TransactionID == item.TransactionID && f.ItemID == item.ItemID);
            new BigcommerceHelper(UserKey).FulfillOrder(account, item.TransactionID, item.ItemID, (int)parcelItem.Quantity, (int)parcelItem.AddressID, item.TrackingNumber); // need to change the carrier accordingly.    
        }
        
        
    }

    private void UpdateMagentoSale(Manifest item)
    {
        DataModelEntities context = new DataModelEntities();
        UserAccount account;
        if (item.AccountID == string.Empty)
        {
            account = context.UserAccounts.Where(u => u.User_Code == UserKey & u.Account_Code == (int)Constant.Accounts.Shopify && u.Is_Active == true).OrderBy(u => u.User_Account_Code).First();
        }
        else
        {
            int userAccountCode = int.Parse(item.AccountID);
            account = context.UserAccounts.First(u => u.User_Account_Code == userAccountCode);
        }

        new MagentoHelper(UserKey).FulfillOrder(account, item.TransactionID, item.TrackingNumber, "ups"); // need to change the carrier accordingly.
    }

    private void UpdateShopifySale(Manifest item)
    {
        DataModelEntities context = new DataModelEntities();
        UserAccount account;
        if (item.AccountID == string.Empty)
        {
            account = context.UserAccounts.Where(u => u.User_Code == UserKey & u.Account_Code == (int)Constant.Accounts.Shopify && u.Is_Active == true).OrderBy(u => u.User_Account_Code).First();
        }
        else
        {
            int userAccountCode = int.Parse(item.AccountID);
            account = context.UserAccounts.First(u => u.User_Account_Code == userAccountCode);
        }
        new ShopifyHelper(UserKey).FulfillOrder(UserKey, account, item.TransactionID, item.TrackingNumber, item.ItemID);
    }

    private void UpdateEbaySale(Manifest item)
    {
        DataModelEntities context = new DataModelEntities();
        UserAccount account;
        if (item.AccountID == string.Empty)
        {
            account = context.UserAccounts.Where(u => u.User_Code == UserKey & u.Account_Code == (int)Constant.Accounts.Ebay && u.Is_Active == true).OrderBy(u => u.User_Account_Code).First();
        }
        else
        {
            int userAccountCode = int.Parse(item.AccountID);
            account = context.UserAccounts.First(u => u.User_Account_Code == userAccountCode);
        }

        if (item.ItemID != string.Empty && item.TransactionID != string.Empty) // check if both itemID and transactionID are valid.
        {
            CompleteSaleResponseType response = new EbayServiceBL(UserKey).UpdateShippingInfo(item.ItemID, item.TransactionID, item.TrackingNumber, account.Config_Value1);
            if (response.Ack == AckCodeType.Success || response.Ack == AckCodeType.Warning)
            {
                // status updated
                lblUploadStatus.Text = "Success. Ebay Shipping Information updated";

                // delete from database too
                PriceManagerDAL.ParcelItem itemToDelete = context.ParcelItems.FirstOrDefault(f => f.User_Code == UserKey && f.TransactionID == item.TransactionID && f.ItemID == item.ItemID);
                if (itemToDelete != null)
                {
                    context.ParcelItems.DeleteObject(itemToDelete);
                    context.SaveChanges();
                }
            }
            else
            {
                // some error
                lblUploadStatus.Text = "Error!! " + response.Errors[0].LongMessage;
            }
        }
        else
        {
            lblSkippedItems.Text += item.TrackingNumber + ", ";
        }

        context = null;
    }

    private String[] LineToArray(String line)
    {
        String pattern = ",(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))";
        Regex r = new Regex(pattern);
        return r.Split(line);
    }

    private string UploadFile()
    {
        if (FileUpload1.HasFile)
        {
            string filePath = Server.MapPath("~/files/" + FileUpload1.FileName);
            if (File.Exists(filePath))
                filePath = Server.MapPath("~/files/" + DateTime.Now.ToString("MMddyy-hhmm_") + FileUpload1.FileName);
            FileUpload1.SaveAs(filePath);
            return filePath;
        }
        else
            return null;
    }
}