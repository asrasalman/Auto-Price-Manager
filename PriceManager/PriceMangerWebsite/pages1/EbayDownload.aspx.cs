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
using Microsoft.Reporting.WebForms;
using Ionic.Zip;
using System.Data;
using System.Web.UI.HtmlControls;
using ShopifyAPIAdapterLibrary;
using System.Web.Script.Serialization;

public partial class pages_EbayDownload : Base
{
    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager.GetCurrent(this.Page).RegisterPostBackControl(btnGenerateInvoice);
        ScriptManager.GetCurrent(this.Page).RegisterPostBackControl(btnGeneratePickingList);
        //ScriptManager.GetCurrent(this.Page).RegisterPostBackControl(btnGenerateLabels);

        if (!IsPostBack)
        {
            PriceManagerDAL.DataModelEntities context = new PriceManagerDAL.DataModelEntities();

            // check if atleast one account is configured for the current user.
            if (context.UserAccounts.Count(u => u.User_Code == UserKey && u.Is_Active == true) > 0)
            {
                hdfSettings.Value = "1";
            }

            Package package = context.Users.Include("Package").First(u => u.User_Code == UserKey).Package;

            if (package.Is_Eparcel_Active == false)
            {
                btnProcessSelectedItems.Visible = false;
            }
            if (package.Is_ClicknSend_Active == false)
            {
                btnGenerateClicknSend.Visible = false;
            }
        }
    }

    protected void btnProcessAPI_Click(object sender, EventArgs e)
    {
        if (IsValid)
        {
            ShowPendingTransactions();
        }
    }

    private void ShowPendingTransactions()
    {
        DataModelEntities context = new DataModelEntities();
        List<PriceManagerDAL.ParcelItem> items = context.ParcelItems.Where(p => p.Is_Active == true && p.User_Code == UserKey && p.UserAccount.Is_Active == true && p.Parcel_Status_Code == (int)Constant.ParcelStatusCode.Pending).OrderBy(o => o.AccountID).ThenBy(o => o.BuyerName).ToList();

        rptParcelItems.DataSource = items;
        rptParcelItems.DataBind();
        pnlItems.Visible = true;
        lblPendingShipmentCount.Text = items.Count.ToString() + " Shipment(s) Pending";
        lblPendingShipmentCount.Visible = imgEbay.Visible = true;

        if (items.Count == 0)
        {
            lblAPIIssues.Visible = true;
            lblAPIIssues.Text = "No Pending Shipment transactions found.";
        }
    }

    protected void btnProcessSelectedItems_Click(object sender, EventArgs e)
    {
        if (IsValid)
        {
            DataModelEntities context = new DataModelEntities();
            User user = context.Users.First(u => u.User_Code == UserKey);
            List<PartMaster> masters = context.Items.Where(i => i.UserCode == UserKey).Select(m => new PartMaster { CustomLabel = m.CustomLabel, Weight = m.Weight, Length = m.Length, Width = m.Width, Height = m.Height, ItemName = m.Description }).ToList();
            //List<PartMaster> masters = File.ReadLines(Server.MapPath("~/master/partMaster.csv")).Select(line => line.Split(',')).Select(m => new PartMaster { CustomLabel = m[0], Weight = m[1], Length = m[2], Width = m[3], Height = m[4], ItemName = m[5] }).ToList();

            DateTime currentTime = DateTime.UtcNow.AddHours(10);

            string fileName = "~/files/eParcel_" + currentTime.ToString("ddMMyyyy_hhmmtt") + ".csv";
            string file = Server.MapPath(fileName);

            FileStream fStream;
            if (File.Exists(file))
                fStream = File.Open(file, FileMode.Truncate, FileAccess.Write);
            else
                fStream = File.Open(file, FileMode.CreateNew, FileAccess.ReadWrite);

            System.Text.UTF8Encoding enc = new UTF8Encoding(false);
            StreamWriter writer = new StreamWriter(fStream, enc);

            StringBuilder screenOutput = new StringBuilder();

            string lastBuyerID = string.Empty;
            foreach (RepeaterItem item in rptParcelItems.Items)
            {
                if (((CheckBox)item.FindControl("chkSelect")).Checked == true)
                {
                    string accountID = ((HtmlInputHidden)item.FindControl("hfAccountID")).Value;
                    string accountType = ((HtmlInputHidden)item.FindControl("hfAccountType")).Value;
                    string itemID = ((HiddenField)item.FindControl("hfItemID")).Value;
                    string transactionId = ((HiddenField)item.FindControl("hfTransactionID")).Value;
                    string customLabel = ((HiddenField)item.FindControl("hfCustomLabel")).Value;
                    string customLabelText = ((Label)item.FindControl("lblCustomLabel")).Text;
                    string state = ((HiddenField)item.FindControl("hfState")).Value.Replace(",", "-");
                    string street = ((HtmlInputHidden)item.FindControl("hfStreet")).Value.Replace(",", "-");
                    string street2 = ((HtmlInputHidden)item.FindControl("hfStreet2")).Value.Replace(",", "-");
                    string isIncomplete = ((HtmlInputHidden)item.FindControl("hfIsIncomplete")).Value;
                    string street3 = ((HtmlInputHidden)item.FindControl("hfStreet3")).Value.Replace(",", "-");
                    string city = ((HtmlInputHidden)item.FindControl("hfCity")).Value.Replace(",", "-");
                    string postalCode = ((HtmlInputHidden)item.FindControl("hfPostalCode")).Value.Replace(",", "-");
                    string country = ((HiddenField)item.FindControl("hfCountry")).Value;
                    string phone = ((HiddenField)item.FindControl("hfPhone")).Value.Replace(",", "-");
                    string shippingMethod = ((Label)item.FindControl("lblShippingMethod")).Text.Replace(",", "-");
                    string buyerName = ((Label)item.FindControl("lblBuyerName")).Text.Replace(",", "-");
                    string buyerId = ((Label)item.FindControl("lblBuyerID")).Text;
                    string quantity = ((Label)item.FindControl("lblQuantity")).Text;
                    string price = ((HiddenField)item.FindControl("hfPrice")).Value.Replace(",", "");
                    string currency = ((HiddenField)item.FindControl("hfCurrency")).Value;
                    string email = ((HiddenField)item.FindControl("hfEmail")).Value;
                    string insuranceAmount = ((TextBox)item.FindControl("txtInsurance")).Text;
                    string hasInsurance = ((CheckBox)item.FindControl("chkInsurance")).Checked == false ? "N" : "Y";
                    insuranceAmount = hasInsurance == "N" ? "0" : insuranceAmount;


                    // update readonly controls (postback workaround)--------
                    ((Label)item.FindControl("lblSuburb")).Text = city;
                    if (((HtmlInputHidden)item.FindControl("hfIsPCFixed")).Value == "1")
                        ((Image)item.FindControl("imgPostCode")).ImageUrl = Constant.tickURL;
                    //-------------------------------------------------------

                    string orderID = accountID + ":" + accountType + ":" + itemID + ":" + transactionId;


                    if (string.IsNullOrEmpty(customLabel))
                    {
                        screenOutput.Append("Custom Label not defined for this Item. Item ID: " + itemID + ", Transaction ID: " + transactionId);
                        screenOutput.Append("<br/>");
                        continue;
                    }

                    PartMaster master = masters.FirstOrDefault(m => m.CustomLabel == customLabel);
                    string weight = master == null ? string.Empty : master.Weight;
                    string length = master == null ? string.Empty : master.Length;
                    string width = master == null ? string.Empty : master.Width;
                    string height = master == null ? string.Empty : master.Height;

                    // Get charge Code 
                    string colD = GetChargeCode(shippingMethod);
                    if (colD == "??")
                    {
                        screenOutput.Append("Charge Code not found for " + shippingMethod + ". Item ID: " + itemID + ", Transaction ID: " + transactionId);
                        screenOutput.Append("<br/>");
                        continue;
                    }

                    if (isIncomplete.ToLower() == "true")
                    {
                        screenOutput.Append("Incomplete transaction details for Item ID: " + itemID + ", Transaction ID: " + transactionId);
                        screenOutput.Append("<br/>");
                        continue;
                    }

                    if (buyerId != lastBuyerID)
                    {

                        string line1 = "C,,," + colD + ",," + buyerName + ",," + street2 + "," + street3 + "," + street + ",," + city + "," + state + "," + postalCode + "," + country + "," + phone + ",,," + customLabelText + ",Y,," + orderID + ",,," + orderID + ",," + customLabelText + ",Y,,,,,,,,,,,,,,,,,,,,,,," + email + ",TRACKADV";
                        writer.WriteLine(line1);
                    }

                    string line2 = string.Empty;
                    if (user.Flat_Rate_Client == true)
                    {
                        line2 = @"A," + weight + ",nil,nil,nil," + quantity + "," + customLabelText + ",," + hasInsurance + "," + insuranceAmount + ",,,,,,,,,";
                    }
                    else
                    {
                        line2 = @"A," + weight + "," + length + "," + width + "," + height + "," + quantity + "," + customLabelText + ",," + hasInsurance + "," + insuranceAmount + ",,,,,,,,,";
                    }

                    writer.WriteLine(line2);
                    if (country.ToUpper() != "AU" && country.ToUpper() != "AUSTRALIA")
                    {
                        string line3 = @"G,,," + customLabel + ",,,,,," + price;
                        writer.WriteLine(line3);
                    }
                    lastBuyerID = buyerId;

                }
            }
            writer.Close();
            writer.Dispose();

            pnlOutput.Visible = true;
            hplProcessedFile.Text = "E-Parcel File Link";
            hplProcessedFile.NavigateUrl = fileName;

            if (screenOutput.Length == 0)
            {
                lblIssuesHeading.Text = string.Empty;
                lblResult.Text = string.Empty;
            }
            else
            {
                lblIssuesHeading.Text = "Following Issue(s) Found";
                lblResult.Text = screenOutput.ToString();
            }
        }
    }

    private string GetChargeCode(string shippingMethod)
    {
        string colD = "??";

        // first look into user profile, if user has defined it himself, then pick it from there.
        User user = new DataModelEntities().Users.First(u => u.User_Code == UserKey);
        if (shippingMethod.ToLower().Contains("au_expressinternational"))
            colD = user.Charge_Code_ExpressInt;

        else if (shippingMethod.ToLower().Contains("au_standardinternational"))
            colD = user.Charge_Code_StandardInt;

        else if (shippingMethod.ToLower().Contains("au_airmailinternational"))
            colD = user.Charge_Code_AirMailInt;

        else if (shippingMethod.ToLower().Contains("au_auspostregisteredpostinternationalparcel"))
            colD = user.Charge_Code_AusPostRegInt;

        else if (shippingMethod.ToLower().Contains("express"))
            colD = user.Charge_Code_Express;

        else if (shippingMethod.ToLower().Contains("standard") || shippingMethod.ToLower().Contains("regular"))
            colD = user.Charge_Code_Standard;


        // if user has not defined it, or the shipping method is other than Express/Standard/Regular , then pick from the admin spreadsheet.
        if (colD == "??")
        {
            ChargeCode code = new DataModelEntities().ChargeCodes.FirstOrDefault(u => u.Is_Active == true && u.User_Code == UserKey && shippingMethod.ToLower().Contains(u.Ebay_Code.ToLower()) == true);
            if (code != null)
            {
                colD = string.IsNullOrEmpty(code.Charge_Code_Name) ? "??" : code.Charge_Code_Name;
            }
        }
        return colD;
    }

    List<Invoice> invoiceList = new List<Invoice>();
    List<InvoiceDetail> invoiceDetailList = new List<InvoiceDetail>();
    List<PickingList> pickingList = new List<PickingList>();
    protected void btnGenerateInvoice_Click(object sender, EventArgs e)
    {
        if (IsValid)
        {
            DateTime currentTime = DateTime.UtcNow.AddHours(10); // Australian time

            string path = Server.MapPath("~/files/temp/" + DateTime.Now.ToString("MMddyy") + ".pdf");

            User user = new DataModelEntities().Users.First(u => u.User_Code == UserKey);

            string lastBuyerID = string.Empty;
            int count = 1;
            foreach (RepeaterItem item in rptParcelItems.Items)
            {
                if (((CheckBox)item.FindControl("chkSelect")).Checked == true)
                {
                    string isIncomplete = ((HtmlInputHidden)item.FindControl("hfIsIncomplete")).Value;

                    if (isIncomplete.ToLower() == "true")
                    {
                        continue;
                    }

                    string itemID = ((HiddenField)item.FindControl("hfItemID")).Value;
                    string transactionId = ((HiddenField)item.FindControl("hfTransactionID")).Value;
                    string customLabel = ((Label)item.FindControl("lblCustomLabel")).Text;
                    string itemName = ((Label)item.FindControl("lblItemName")).Text;
                    string state = ((HiddenField)item.FindControl("hfState")).Value;
                    string street = ((HtmlInputHidden)item.FindControl("hfStreet")).Value;
                    string street2 = ((HtmlInputHidden)item.FindControl("hfStreet2")).Value;
                    string street3 = ((HtmlInputHidden)item.FindControl("hfStreet3")).Value;
                    string city = ((HtmlInputHidden)item.FindControl("hfCity")).Value;
                    string postalCode = ((HtmlInputHidden)item.FindControl("hfPostalCode")).Value;
                    string country = ((HiddenField)item.FindControl("hfCountry")).Value;
                    string phone = ((HiddenField)item.FindControl("hfPhone")).Value;
                    string shippingMethod = ((Label)item.FindControl("lblShippingMethod")).Text;
                    string buyerName = ((Label)item.FindControl("lblBuyerName")).Text;
                    string buyerId = ((Label)item.FindControl("lblBuyerID")).Text;
                    string quantity = ((Label)item.FindControl("lblQuantity")).Text;
                    string price = ((HiddenField)item.FindControl("hfPrice")).Value;
                    string currency = ((HiddenField)item.FindControl("hfCurrency")).Value;
                    string shippingCost = ((HiddenField)item.FindControl("hfShippingCost")).Value;
                    string saleRecordId = ((HiddenField)item.FindControl("hfSaleRecordId")).Value;


                    Invoice invoice = new Invoice();

                    Invoice sameInvoice = invoiceList.FirstOrDefault(i => i.BuyerId == buyerId);
                    if (sameInvoice == null)
                    {
                        invoice.Invoice_Code = count.ToString();
                        invoice.From_Name = user.Company + " - " + user.Full_Name;
                        invoice.From_Address = user.Address1;
                        invoice.From_Address2 = user.Address2;
                        invoice.From_Country = user.Country;
                        invoice.To_Name = buyerName;
                        invoice.To_Address = street2 + " " + street3;
                        invoice.To_Address2 = city + ' ' + state + " - " + postalCode;
                        invoice.To_Country = country;
                        invoice.Currency = currency;
                        invoice.Total_Price = Math.Round((decimal.Parse(price) * decimal.Parse(quantity)), 2);
                        invoice.Invoice_Date = DateTime.Now.ToString("dd/MM/yyyy");
                        invoice.Invoice_Number = saleRecordId;
                        invoice.Shipping_Cost = shippingCost == string.Empty ? 0 : Math.Round(decimal.Parse(shippingCost), 2);
                        invoice.Grand_Total = Math.Round(invoice.Total_Price + invoice.Shipping_Cost, 2);
                        invoice.Tax_Price = Math.Round(invoice.Grand_Total / 11, 2); // divide by 11
                        invoice.Shipping_Method = shippingMethod;
                        invoice.From_Suburb = user.City;
                        invoice.From_State = user.State;
                        invoice.From_Postcode = user.Zip;
                        invoice.From_Phone = user.Phone_Number;
                        invoice.Company_Logo = string.IsNullOrEmpty(user.Company_Logo) ? null : GetFile(user.Company_Logo);
                        invoice.ABN_Number = user.ABN_Number;
                        invoice.BuyerId = buyerId;

                        invoiceList.Add(invoice);
                    }
                    else
                    {
                        sameInvoice.Total_Price += Math.Round((decimal.Parse(price) * decimal.Parse(quantity)), 2);
                        sameInvoice.Grand_Total = Math.Round(sameInvoice.Total_Price + sameInvoice.Shipping_Cost, 2);
                        sameInvoice.Tax_Price = Math.Round(sameInvoice.Grand_Total / 11, 2); // 10 percent Tax
                    }

                    InvoiceDetail invoiceDetail = new InvoiceDetail();


                    invoiceDetail.Invoice_Code = count.ToString();

                    invoiceDetail.Item_Code = itemID;
                    invoiceDetail.Quantity = quantity;
                    invoiceDetail.Item_Name = itemName;
                    invoiceDetail.Price = price;
                    invoiceDetail.BuyerId = buyerId;

                    invoiceDetailList.Add(invoiceDetail);

                    count++;
                }
            }

            GeneratePDF(path);

            GenerateZip(path, "Ebay-Invoices");
        }
    }

    protected void btnGeneratePickingList_Click(object sender, EventArgs e)
    {
        if (IsValid)
        {
            DateTime currentTime = DateTime.UtcNow.AddHours(10); // Australian time

            string path = Server.MapPath("~/files/temp/PickingList-" + DateTime.Now.ToString("MMddyy") + ".pdf");

            User user = new DataModelEntities().Users.First(u => u.User_Code == UserKey);

            string lastBuyerID = string.Empty;
            int count = 1;
            foreach (RepeaterItem item in rptParcelItems.Items)
            {
                if (((CheckBox)item.FindControl("chkSelect")).Checked == true)
                {
                    string isIncomplete = ((HtmlInputHidden)item.FindControl("hfIsIncomplete")).Value;

                    if (isIncomplete.ToLower() == "true")
                    {
                        continue;
                    }

                    string itemID = ((HiddenField)item.FindControl("hfItemID")).Value;
                    string transactionId = ((HiddenField)item.FindControl("hfTransactionID")).Value;
                    string customLabel = ((HiddenField)item.FindControl("hfCustomLabel")).Value;
                    string customLabelText = ((Label)item.FindControl("lblCustomLabel")).Text;
                    string itemName = ((Label)item.FindControl("lblItemName")).Text;
                    string state = ((HiddenField)item.FindControl("hfState")).Value;
                    string street = ((HtmlInputHidden)item.FindControl("hfStreet")).Value;
                    string street2 = ((HtmlInputHidden)item.FindControl("hfStreet2")).Value;
                    string street3 = ((HtmlInputHidden)item.FindControl("hfStreet3")).Value;
                    string city = ((HtmlInputHidden)item.FindControl("hfCity")).Value;
                    string postalCode = ((HtmlInputHidden)item.FindControl("hfPostalCode")).Value;
                    string country = ((HiddenField)item.FindControl("hfCountry")).Value;
                    string phone = ((HiddenField)item.FindControl("hfPhone")).Value;
                    string shippingMethod = ((Label)item.FindControl("lblShippingMethod")).Text;
                    string buyerName = ((Label)item.FindControl("lblBuyerName")).Text;
                    string buyerId = ((Label)item.FindControl("lblBuyerID")).Text;
                    string quantity = ((Label)item.FindControl("lblQuantity")).Text;
                    string price = ((HiddenField)item.FindControl("hfPrice")).Value;
                    string currency = ((HiddenField)item.FindControl("hfCurrency")).Value;
                    string shippingCost = ((HiddenField)item.FindControl("hfShippingCost")).Value;
                    string saleRecordId = ((HiddenField)item.FindControl("hfSaleRecordId")).Value;

                    PickingList pickingItem = new PickingList();

                    PickingList samePickingItem = pickingList.FirstOrDefault(i => i.CustomLabel == customLabel);
                    if (samePickingItem == null)
                    {
                        pickingItem.CustomLabel = customLabel;
                        pickingItem.Description = itemName;
                        pickingItem.Quantity = int.Parse(quantity);
                        pickingItem.QuantitySupplied = int.Parse(quantity);


                        pickingList.Add(pickingItem);
                    }
                    else
                    {
                        samePickingItem.Quantity += int.Parse(quantity);
                        samePickingItem.QuantitySupplied += int.Parse(quantity);
                    }

                    count++;
                }
            }

            GeneratePL_PDF(path);

            GenerateZip(path, "Ebay-PickingList");
        }
    }

    private byte[] GetFile(string fileName)
    {
        Byte[] file = File.ReadAllBytes(Server.MapPath(fileName));
        return file;
    }

    private void GeneratePDF(string path)
    {
        ReportViewer1.Visible = true;


        ReportDataSource dataSource = new ReportDataSource("DataSet1", invoiceList);

        ReportViewer1.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(ItemsSubreportProcessingEventHandler);
        ReportViewer1.LocalReport.DataSources.Clear();
        ReportViewer1.LocalReport.DataSources.Add(dataSource);
        ReportViewer1.LocalReport.Refresh();

        Microsoft.Reporting.WebForms.Warning[] warnings = null;
        string[] streamids = null;
        String mimeType = null;
        String encoding = null;
        String extension = null;
        Byte[] bytes = null;

        bytes = ReportViewer1.LocalReport.Render("PDF", "", out mimeType, out encoding, out extension, out streamids, out warnings);

        FileStream fs;
        if (File.Exists(path))
            fs = File.Open(path, FileMode.Truncate, FileAccess.Write);
        else
            fs = File.Open(path, FileMode.CreateNew, FileAccess.ReadWrite);

        byte[] data = new byte[fs.Length];
        fs.Write(bytes, 0, bytes.Length);
        fs.Close();

        ReportViewer1.Visible = false;

        //DownloadPDF(dirPath + @"\" + reportName);
    }

    private void GeneratePL_PDF(string path)
    {
        ReportViewer2.Visible = true;

        ReportDataSource dataSource = new ReportDataSource("DataSet1", pickingList);

        ReportViewer2.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(ItemsSubreportProcessingEventHandler);
        ReportViewer2.LocalReport.DataSources.Clear();
        ReportViewer2.LocalReport.DataSources.Add(dataSource);
        ReportViewer2.LocalReport.Refresh();

        Microsoft.Reporting.WebForms.Warning[] warnings = null;
        string[] streamids = null;
        String mimeType = null;
        String encoding = null;
        String extension = null;
        Byte[] bytes = null;

        bytes = ReportViewer2.LocalReport.Render("PDF", "", out mimeType, out encoding, out extension, out streamids, out warnings);

        FileStream fs;
        if (File.Exists(path))
            fs = File.Open(path, FileMode.Truncate, FileAccess.Write);
        else
            fs = File.Open(path, FileMode.CreateNew, FileAccess.ReadWrite);

        byte[] data = new byte[fs.Length];
        fs.Write(bytes, 0, bytes.Length);
        fs.Close();
        ReportViewer2.Visible = false;
    }

    int _SubReportCounter = 1;
    protected void ItemsSubreportProcessingEventHandler(object sender, SubreportProcessingEventArgs e)
    {
        string buyerId = e.Parameters[0].Values[0];

        List<InvoiceDetail> invoiceDetails1 = invoiceDetailList.Where(i => i.BuyerId == buyerId).ToList();
        ReportDataSource dataSource = new ReportDataSource("DataSet1", invoiceDetails1);

        e.DataSources.Clear();
        e.DataSources.Add(dataSource);
        _SubReportCounter++;
    }

    private void DownloadPDF(string path)
    {
        Response.ContentType = "application/pdf";
        Response.AddHeader("Content-Disposition", "filename=" + path);
        Response.WriteFile(path);
        Response.End();
    }

    private void GenerateZip(string path, string name)
    {
        // Tell the browser we're sending a ZIP file!
        var downloadFileName = string.Format(name + "-{0}.zip", DateTime.Now.ToString("yyyy-MM-dd-HH_mm_ss"));
        Response.ContentType = "application/zip";
        Response.AddHeader("Content-Disposition", "filename=" + downloadFileName);

        // Zip the contents of the selected files (STEP 1)
        using (var zip = new ZipFile())
        {
            FileInfo file = new FileInfo(path);
            zip.AddFile(file.FullName, "/");

            zip.Save(Response.OutputStream);
        }
        Response.End();
    }

    protected void btnGenerateClicknSend_Click(object sender, EventArgs e)
    {
        if (IsValid)
        {
            DataModelEntities context = new DataModelEntities();
            User user = context.Users.First(u => u.User_Code == UserKey);
            List<PartMaster> masters = context.Items.Where(i => i.UserCode == UserKey).Select(m => new PartMaster { CustomLabel = m.CustomLabel, Weight = m.Weight, Length = m.Length, Width = m.Width, Height = m.Height, ItemName = m.Description }).ToList();

            DateTime currentTime = DateTime.UtcNow.AddHours(10);

            string fileName = "~/files/ClicknSend_" + currentTime.ToString("ddMMyyyy_hhmmtt") + ".csv";
            string file = Server.MapPath(fileName);

            FileStream fStream;
            if (File.Exists(file))
                fStream = File.Open(file, FileMode.Truncate, FileAccess.Write);
            else
                fStream = File.Open(file, FileMode.CreateNew, FileAccess.ReadWrite);

            System.Text.UTF8Encoding enc = new UTF8Encoding(false);
            StreamWriter writer = new StreamWriter(fStream, enc);

            StringBuilder screenOutput = new StringBuilder();

            string lastBuyerID = string.Empty;
            foreach (RepeaterItem item in rptParcelItems.Items)
            {
                if (((CheckBox)item.FindControl("chkSelect")).Checked == true)
                {
                    string isIncomplete = ((HtmlInputHidden)item.FindControl("hfIsIncomplete")).Value;

                    if (isIncomplete.ToLower() == "true")
                    {
                        continue;
                    }

                    string itemName = ((Label)item.FindControl("lblItemName")).Text.Replace(",", " ");
                    string accountID = ((HtmlInputHidden)item.FindControl("hfAccountID")).Value;
                    string accountType = ((HtmlInputHidden)item.FindControl("hfAccountType")).Value;
                    string itemID = ((HiddenField)item.FindControl("hfItemID")).Value;
                    string transactionId = ((HiddenField)item.FindControl("hfTransactionID")).Value;
                    string customLabel = ((HiddenField)item.FindControl("hfCustomLabel")).Value;
                    string customLabelText = ((Label)item.FindControl("lblCustomLabel")).Text;
                    string state = ((HiddenField)item.FindControl("hfState")).Value.Replace(",", "-");
                    string street = ((HtmlInputHidden)item.FindControl("hfStreet")).Value.Replace(",", "-");
                    string street2 = ((HtmlInputHidden)item.FindControl("hfStreet2")).Value.Replace(",", "-");
                    string street3 = ((HtmlInputHidden)item.FindControl("hfStreet3")).Value.Replace(",", "-");
                    string city = ((HtmlInputHidden)item.FindControl("hfCity")).Value.Replace(",", "-");
                    string postalCode = ((HtmlInputHidden)item.FindControl("hfPostalCode")).Value.Replace(",", "-");
                    string country = ((HiddenField)item.FindControl("hfCountry")).Value;
                    string phone = ((HiddenField)item.FindControl("hfPhone")).Value.Replace(",", "-");
                    string shippingMethod = ((Label)item.FindControl("lblShippingMethod")).Text.Replace(",", "-");
                    string buyerName = ((Label)item.FindControl("lblBuyerName")).Text.Replace(",", "-");
                    string buyerId = ((Label)item.FindControl("lblBuyerID")).Text;
                    string quantity = ((Label)item.FindControl("lblQuantity")).Text;
                    string price = ((HiddenField)item.FindControl("hfPrice")).Value.Replace(",", "");
                    string currency = ((HiddenField)item.FindControl("hfCurrency")).Value;
                    string email = ((HiddenField)item.FindControl("hfEmail")).Value;
                    string insuranceAmount = ((TextBox)item.FindControl("txtInsurance")).Text;
                    string hasInsurance = ((CheckBox)item.FindControl("chkInsurance")).Checked == false ? "N" : "Y";
                    insuranceAmount = hasInsurance == "N" ? "0" : insuranceAmount;


                    // update readonly controls (postback workaround)--------
                    ((Label)item.FindControl("lblSuburb")).Text = city;
                    if (((HtmlInputHidden)item.FindControl("hfIsPCFixed")).Value == "1")
                        ((Image)item.FindControl("imgPostCode")).ImageUrl = Constant.tickURL;
                    //-------------------------------------------------------

                    string orderID = accountID + ":" + accountType + ":" + itemID + ":" + transactionId;


                    if (string.IsNullOrEmpty(customLabel))
                    {
                        screenOutput.Append("Custom Label not defined for this Item. Item ID: " + itemID + ", Transaction ID: " + transactionId);
                        screenOutput.Append("<br/>");
                        continue;
                    }

                    PartMaster master = masters.FirstOrDefault(m => m.CustomLabel == customLabel);
                    string weight = master == null ? string.Empty : master.Weight;
                    string length = master == null ? string.Empty : master.Length;
                    string width = master == null ? string.Empty : master.Width;
                    string height = master == null ? string.Empty : master.Height;

                    // Get charge Code 
                    string colD = GetChargeCode(shippingMethod);
                    if (colD == "??")
                    {
                        screenOutput.Append("Charge Code not found for " + shippingMethod + ". Item ID: " + itemID + ", Transaction ID: " + transactionId);
                        screenOutput.Append("<br/>");
                        continue;
                    }

                    string shippingMethodShort = shippingMethod.ToLower().Contains("express") ? "EP" : "PP";

                    if (buyerId != lastBuyerID)
                    {
                        string line = "," + buyerName + "," + phone + "," + email + "," + street2 + "," + street3 + "," + street + "," + city + "," + state + "," + postalCode + "," + country + "," + shippingMethodShort + "," + "7" + "," + length + "," + width + "," + height + "," + weight + ",0,0," + itemName + "," +
                                      user.Full_Name + "," + user.Company + "," + user.Phone_Number + "," + "," + user.Email_Address + "," + user.ABN_Number.Replace(" ", "") + "," + user.Address1 + "," + user.Address2 + "," + "," + user.City + "," + user.State + "," + user.Zip + "," + user.Country + "," + orderID + ",1,0,0,,,,,,,N,Y,N";
                        writer.WriteLine(line);
                    }
                    lastBuyerID = buyerId;
                }
            }
            writer.Close();
            writer.Dispose();

            pnlOutput.Visible = true;
            hplProcessedFile.Text = "ClickandSend File Link";
            hplProcessedFile.NavigateUrl = fileName;

            if (screenOutput.Length == 0)
            {
                lblIssuesHeading.Text = string.Empty;
                lblResult.Text = string.Empty;
            }
            else
            {
                lblIssuesHeading.Text = "Following Issue(s) Found";
                lblResult.Text = screenOutput.ToString();
            }
        }
    }

    List<ItemLabel> labels = new List<ItemLabel>();
    protected void btnGenerateLabels_Click(object sender, EventArgs e)
    {
        if (IsValid)
        {
            DateTime currentTime = DateTime.UtcNow.AddHours(10); // Australian time

            string relativePath = "~/files/temp/Labels-" + DateTime.Now.ToString("MMddyy") + ".pdf";
            string path = Server.MapPath(relativePath);

            User user = new DataModelEntities().Users.First(u => u.User_Code == UserKey);

            string lastBuyerID = string.Empty;
            int count = 1;
            foreach (RepeaterItem item in rptParcelItems.Items)
            {
                if (((CheckBox)item.FindControl("chkSelect")).Checked == true)
                {
                    string isIncomplete = ((HtmlInputHidden)item.FindControl("hfIsIncomplete")).Value;

                    if (isIncomplete.ToLower() == "true")
                    {
                        continue;
                    }

                    string itemID = ((HiddenField)item.FindControl("hfItemID")).Value;
                    string transactionId = ((HiddenField)item.FindControl("hfTransactionID")).Value;
                    string customLabel = ((HiddenField)item.FindControl("hfCustomLabel")).Value;
                    string customLabelText = ((Label)item.FindControl("lblCustomLabel")).Text;
                    string itemName = ((Label)item.FindControl("lblItemName")).Text;
                    string state = ((HiddenField)item.FindControl("hfState")).Value;
                    string street = ((HtmlInputHidden)item.FindControl("hfStreet")).Value;
                    string street2 = ((HtmlInputHidden)item.FindControl("hfStreet2")).Value;
                    string street3 = ((HtmlInputHidden)item.FindControl("hfStreet3")).Value;
                    string city = ((HtmlInputHidden)item.FindControl("hfCity")).Value;
                    string postalCode = ((HtmlInputHidden)item.FindControl("hfPostalCode")).Value;
                    string country = ((HiddenField)item.FindControl("hfCountry")).Value;
                    string phone = ((HiddenField)item.FindControl("hfPhone")).Value;
                    string shippingMethod = ((Label)item.FindControl("lblShippingMethod")).Text;
                    string buyerName = ((Label)item.FindControl("lblBuyerName")).Text;
                    string buyerId = ((Label)item.FindControl("lblBuyerID")).Text;
                    string quantity = ((Label)item.FindControl("lblQuantity")).Text;
                    string price = ((HiddenField)item.FindControl("hfPrice")).Value;
                    string currency = ((HiddenField)item.FindControl("hfCurrency")).Value;
                    string shippingCost = ((HiddenField)item.FindControl("hfShippingCost")).Value;
                    string saleRecordId = ((HiddenField)item.FindControl("hfSaleRecordId")).Value;

                    ItemLabel label = new ItemLabel();

                    ItemLabel sameLabel = labels.FirstOrDefault(i => i.BuyerId == buyerId);
                    if (sameLabel == null)
                    {
                        label.Client_Name = buyerName;
                        label.BuyerId = buyerId;
                        label.City = city;
                        label.State = state;
                        label.Custom_Label = customLabel;
                        label.PostalCode = postalCode;
                        label.Record_Number = saleRecordId;
                        label.Street = street2;
                        label.Street2 = street3;

                        labels.Add(label);
                    }

                    count++;
                }
            }

            GenerateLabel_Doc(path);

            pnlOutput.Visible = true;
            hplProcessedFile.Text = "Labels File Link";
            hplProcessedFile.NavigateUrl = relativePath;
        }
    }

    private void GenerateLabel_Doc(string path)
    {
        rvLabels.Visible = true;

        ReportDataSource dataSource = new ReportDataSource("DataSet1", labels);

        rvLabels.LocalReport.DataSources.Clear();
        rvLabels.LocalReport.DataSources.Add(dataSource);
        rvLabels.LocalReport.Refresh();

        Byte[] bytes = null;

        bytes = rvLabels.LocalReport.Render("PDF");

        FileStream fs;
        if (File.Exists(path))
            fs = File.Open(path, FileMode.Truncate, FileAccess.Write);
        else
            fs = File.Open(path, FileMode.CreateNew, FileAccess.ReadWrite);

        byte[] data = new byte[fs.Length];
        fs.Write(bytes, 0, bytes.Length);
        fs.Close();
        rvLabels.Visible = false;
    }

    protected void btnRefreshTransactions_Click(object sender, EventArgs e)
    {
        ParcelBL parcelBL = new ParcelBL();
        if (IsValid)
        {
            aConnectAgain.Visible = false;
            lblAPIIssues.Visible = false;
            List<ParcelItem> ebayItems = new List<ParcelItem>();

            try
            {
                ebayItems = parcelBL.GetEbayTransactions(UserKey);
            }
            catch (InvalidEbayCredentialsException ex)
            {
                lblAPIIssues.Visible = true;
                lblAPIIssues.Text = "The Ebay API settings are invalid. Please check back with your developer account or contact Ebay Support";
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Token has been revoked"))
                {
                    lblAPIIssues.Text = "The connectivity with your Ebay account is removed from your Ebay account.";
                    aConnectAgain.Visible = true;
                }
                else
                {
                    lblAPIIssues.Text = "There is some issue connecting to the Ebay server. Please check back in a while.";
                }
                lblAPIIssues.Visible = true;
            }

            List<ParcelItem> shopifyItems = parcelBL.GetShopifyTransactions(UserKey);

            List<ParcelItem> magentoItems = parcelBL.GetMagentoTransactions(UserKey);

            List<ParcelItem> bigcommerceItems = parcelBL.GetBigcommerceTransactions(UserKey);

            if (ebayItems == null)
            {
                ebayItems = new List<ParcelItem>();
                lblPendingShipmentCount.Visible = imgEbay.Visible = false;
            }
            if (shopifyItems == null)
                shopifyItems = new List<ParcelItem>();
            if (magentoItems == null)
                magentoItems = new List<ParcelItem>();
            if (bigcommerceItems == null)
                bigcommerceItems = new List<ParcelItem>();

            List<ParcelItem> allItems = ebayItems.Union(shopifyItems.OrderBy(i => i.BuyerID)).Union(magentoItems.OrderBy(i => i.BuyerID)).Union(bigcommerceItems.OrderBy(i => i.BuyerID)).ToList();

            new ParcelBL().SaveTransactions(allItems, UserKey);
            ShowPendingTransactions();
        }
    }

}