using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PriceManagerDAL;
using System.Data;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.UI.HtmlControls;

public partial class pages_masters_PartMaster : Base
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindItemss();
            if (int.Parse(RoleCode) != 1)
            {
                btnDownloadItems.Visible = false;
            }
            btnDownloadItems.Visible = true;
        }
    }


    private void BindItemss()
    {
        DataModelEntities entities = new DataModelEntities();

        List<Item> Items = entities.Items.Where(i => (i.UserCode == UserKey) && (txtCustomLabelSearch.Value == string.Empty || i.CustomLabel.Contains(txtCustomLabelSearch.Value)) && (txtDescSearch.Value == string.Empty || i.Description.Contains(txtDescSearch.Value))).ToList();

        rptItems.DataSource = Items;
        rptItems.DataBind();
        entities = null;
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {

        if (hfItemID.Value == string.Empty) // new Items
            AddNewItems();
        else
            UpdateItemsDetails();
    }

    protected void btnAdjust_Click(object sender, EventArgs e)
    {
        if (IsValid)
        {
            int selectedItemsCode = int.Parse(hfAdjustItemID.Value);
            int adjustmentQty = int.Parse(txtAdjustQty.Value);
            
            if (rdbRemoval.Checked)
                adjustmentQty = adjustmentQty * -1;
           

            DataModelEntities entities = new DataModelEntities();
            Item item = entities.Items.First(u => u.ID == selectedItemsCode);
            item.Balance_Quantity = item.Balance_Quantity != null ? item.Balance_Quantity + (adjustmentQty) : adjustmentQty;

            PriceManagerDAL.StockLedger dborder = new PriceManagerDAL.StockLedger();
            dborder.User_Code = item.UserCode;
            dborder.AccountID = null;
            dborder.Type = null;
            dborder.TransactionID = null;
            dborder.ItemID = null;
            dborder.CustomLabel = item.CustomLabel;
            dborder.Quantity = adjustmentQty;
            dborder.Narration = txtAdjustNarration.Value;
            dborder.Stock_Ledger_Type = (int)Common.StockLegerType.Addjustment;
            dborder.Created_Date = System.DateTime.Now;
            dborder.ID = item.ID;
            entities.AddToStockLedgers(dborder);
            entities.SaveChanges();

            BindItemss();
        }
    }

    private void AddNewItems()
    {
        Item item = new Item();
        item.CustomLabel = txtCustomLabel.Value;
        item.Description = txtDesc.Value;
        item.Length = txtLength.Value;
        item.Height = txtHeight.Value;
        item.Weight = txtWeight.Value;
        item.Width = txtWidth.Value;
        item.UserCode = UserKey;

        /*Updated by javed*/
        if (!string.IsNullOrEmpty(txtInitialQty.Value))
            item.Opening_Quantity = int.Parse(txtInitialQty.Value);
        
        item.Balance_Quantity = item.Opening_Quantity != null ? item.Opening_Quantity : 0;
        
        if (!string.IsNullOrEmpty(txtMinimumThreshold.Value))
            item.Minimum_Threshold = int.Parse(txtMinimumThreshold.Value);
        
        DataModelEntities entities = new DataModelEntities();
        entities.AddToItems(item);
        entities.SaveChanges();

        if (item.Opening_Quantity != null)
        {
            PriceManagerDAL.StockLedger dborder = new PriceManagerDAL.StockLedger();
            dborder.User_Code = item.UserCode;
            dborder.AccountID = null;
            dborder.Type = null;
            dborder.TransactionID = null;
            dborder.ItemID = null;
            dborder.CustomLabel = item.CustomLabel;
            dborder.Quantity = item.Opening_Quantity;
            dborder.Narration = "Item Added To Stock";
            dborder.Stock_Ledger_Type = (int)Common.StockLegerType.Opening;
            dborder.Created_Date = System.DateTime.Now;
            dborder.ID = item.ID;
            entities.AddToStockLedgers(dborder);
            entities.SaveChanges();
        }

        
        BindItemss();
    }

    private void UpdateItemsDetails()
    {
        
        int selectedItemsCode = int.Parse(hfItemID.Value);
        DataModelEntities entities = new DataModelEntities();
        Item item = entities.Items.First(u => u.ID == selectedItemsCode);
        item.CustomLabel = txtCustomLabel.Value;
        item.Description = txtDesc.Value;
        item.Length = txtLength.Value;
        item.Height = txtHeight.Value;
        item.Weight = txtWeight.Value;
        item.Width = txtWidth.Value;
        item.UserCode = UserKey;
        
        if (!string.IsNullOrEmpty(txtMinimumThreshold.Value))
            item.Minimum_Threshold = int.Parse(txtMinimumThreshold.Value);

        entities.SaveChanges();
        entities = null;
        BindItemss();
    }

    protected void btnItemsSearch_Click(object sender, EventArgs e)
    {
        if (IsValid)
        {
            BindItemss();
        }
    }


    protected void btnDeleteItems_Click(object sender, ImageClickEventArgs e)
    {
        ImageButton btnDelete = (ImageButton)sender;
        RepeaterItem rptItem = (RepeaterItem)btnDelete.NamingContainer;

        int selectedItemsCode = int.Parse(((HtmlInputHidden)rptItem.FindControl("hfListItemID")).Value);

        DataModelEntities entities = new DataModelEntities();
        Item item = entities.Items.First(u => u.ID == selectedItemsCode);
        entities.Items.DeleteObject(item);
        entities.SaveChanges();
        entities = null;

        BindItemss();

    }

    protected void btnProcessSelectedItems_Click(object sender, EventArgs e)
    {
        if (IsValid)
        {
            DataModelEntities entities = new DataModelEntities();

            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment;filename=PartMaster .csv");
            Response.Charset = "";
            Response.ContentType = "application/text";

            StringBuilder sb = new StringBuilder();
            //add separator
            sb.Append("Custom Label,");
            sb.Append("Weight,");
            sb.Append("Length,");
            sb.Append("Width,");
            sb.Append("Height,");
            sb.Append("Description");
            sb.Append("\r\n");


            string lastBuyerID = string.Empty;
            foreach (RepeaterItem item in rptItems.Items)
            {

                if (((CheckBox)item.FindControl("chkSelect")).Checked == true)
                {   
                    string customLabel = ((Label)item.FindControl("lblCustomLabel")).Text;
                    string length = ((Label)item.FindControl("lblLength")).Text;
                    string height = ((Label)item.FindControl("lblHeight")).Text;
                    string weight = ((Label)item.FindControl("lblWeight")).Text;
                    string width = ((Label)item.FindControl("lblWidth")).Text;
                    string desc = ((Label)item.FindControl("lblDesc")).Text;
                    sb.Append(customLabel + ",");
                    sb.Append(weight + ",");
                    sb.Append(length + ",");
                    sb.Append(width + ",");
                    //sb.Append("0,");
                    sb.Append(height + ",");
                    sb.Append(desc + ",");
                    sb.Append("\r\n");

                }
            }

            Response.Output.Write(sb.ToString());
            Response.Flush();
            Response.End();
        }
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
        DataModelEntities entities = new DataModelEntities();
        List<Item> partMasterItems = entities.Items.ToList();
        try
        {
            lblSkippedItems.Text = string.Empty;

            List<Item> manifestList = File.ReadLines(filePath).Select(line => LineToArray(line)).ToList().Select(m => new Item { CustomLabel = m[0], Weight = m[1], Length = m[2], Width= m[3], Height = m[4], Description = m[5] }).Skip(1).ToList();
            if (manifestList.Count > 0)
            {
                foreach (Item item in manifestList)
                {
                    item.UserCode = UserKey;
                    item.Balance_Quantity = 0;
                    if (!partMasterItems.Any(i => i.CustomLabel == item.CustomLabel && i.UserCode == UserKey))
                    {
                        entities.Items.AddObject(item);
                    }
                    else
                    {
                        Item matchedItem = item;

                    }

                }
                entities.SaveChanges();
                lblUploadStatus.Text = "Success. Ebay Part Master Information updated";
                BindItemss();
            }

        }
        catch (InvalidEbayCredentialsException ex)
        {
            lblAPIIssuesUpload.Visible = true;
            lblAPIIssuesUpload.Text = "The Ebay Part Master settings are invalid. Please check back with your developer account or contact Ebay Support";
        }
        catch (Exception ex)
        {
            lblAPIIssuesUpload.Visible = true;
            lblAPIIssuesUpload.Text = "Invalid Part Master File.";
        }
    }


    protected void btnDeleteBulkItems_Click(object sender, EventArgs e)
    {
        if (IsValid)
        {
            DataModelEntities entities = new DataModelEntities();

            foreach (RepeaterItem rptItem in rptItems.Items)
            {
                if (rptItem.ItemType == ListItemType.Item || rptItem.ItemType == ListItemType.AlternatingItem)
                {
                    CheckBox chkSelect = (CheckBox)rptItem.FindControl("chkSelect");
                    if (chkSelect.Checked == true)
                    {
                        HtmlInputHidden hfListItemID = (HtmlInputHidden)rptItem.FindControl("hfListItemID");
                        int id = int.Parse(hfListItemID.Value);

                        Item item = entities.Items.First(u => u.ID == id);
                        entities.Items.DeleteObject(item);
                    }
                }
            }

            entities.SaveChanges();
            entities = null;
        }

        BindItemss();
    }
    protected void btnDownloadItems_Click(object sender, EventArgs e)
    {
        try
        {
            ParcelBL objParcelBL = new ParcelBL();
            objParcelBL.SaveEbayUserItems(UserKey);
            BindItemss();
        }
        catch (Exception ex)
        {
        }

    }
}