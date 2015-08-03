using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PriceManagerDAL;
using System.Data;

public partial class pages_admin_Admin_Product_Feed : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindUsers();
        }
    }

    

    private void BindUsers()
    {
        List<User> users = new DataModelEntities().Users.Where(u => u.Is_Active == true).OrderBy(u => u.Full_Name).ToList();
        Common.BindDropDown(ddlUserList, users, "Full_Name", "User_Code", false, false);
    }

    protected void btnExport_Click(object sender, EventArgs e)
    {
        try
        {
            int userCode = int.Parse(ddlUserList.SelectedValue);
            ExportProductFeed(userCode);
        }
        catch(Exception ex)
        {
        }
    }

    protected void ExportProductFeed(int userCode)
    {
        hprExport.Visible = false;
        DataModelEntities context = new DataModelEntities();
        var user = context.Users.FirstOrDefault(f => f.User_Code == userCode);
        var items = context.Items.Where(c => c.UserCode == userCode).AsEnumerable()                                
            .Select(a => new
            {
                ItemID = a.Item_ID == null ? string.Empty : a.Item_ID,
                ItemName = a.Description == null ? string.Empty : a.Description,
                ItemViewURL = a.Item_View_URL == null ? string.Empty : a.Item_View_URL,
                PictureURL = a.Picture_URL == null ? string.Empty : a.Picture_URL,
                Category = a.Item_Category_Name == null ? string.Empty : a.Item_Category_Name,
                CategoryID = a.Item_Category_ID == null ? string.Empty : a.Item_Category_ID,
                Decription = "",//a.Detail_Description == null ? string.Empty : a.Detail_Description,
                SKU = a.CustomLabel == null ? string.Empty : a.CustomLabel,
                StartDate = a.Start_Date == null ? string.Empty : Convert.ToDateTime(a.Start_Date).ToString("dd-MMM-yyyy"),
                EndDate = a.End_Date == null ? string.Empty : Convert.ToDateTime(a.End_Date).ToString("dd-MMM-yyyy"),
                Price = a.BIN_Price == null ? string.Empty : Convert.ToDecimal(a.BIN_Price).ToString("0.00"),
                Length = a.Length == null ? string.Empty : a.Length,
                Width = a.Width == null ? string.Empty : a.Width,
                Height = a.Height == null ? string.Empty : a.Height,
                WeightUnit = "Kgs",
            })
            .ToList();
        DataTable finalList;

        if (user.Package_Id == (int)Common.Package.Trial)
            finalList = items.Take(100).ToList().ToDataTable();
        else if (user.Package_Id == (int)Common.Package.Business)
            finalList = items.Take(200).ToList().ToDataTable();
        else
            finalList = items.ToDataTable();

        string file = Export.CreateCsv(finalList, "PF_" + user.First_Name + "_" + DateTime.Now.ToString("ddmmyyyyhhmmss") + ".csv");
        string link = Common.MapPathReverse(file).Replace("\\", "/");
        hprExport.Text = link;
        hprExport.NavigateUrl = link;
        hprExport.Visible = true;
       
    }

    
}