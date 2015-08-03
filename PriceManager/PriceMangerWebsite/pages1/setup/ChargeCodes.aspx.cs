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

public partial class pages_masters_ChargeCode : Base
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindCodes();
            BindUsers();
        }
    }

    private void BindUsers()
    {
        Common.BindDropDown(ddlUser, new DataModelEntities().Users.Where(u => u.Is_Active == true), "Full_Name", "User_Code", false, false);
    }


    private void BindCodes()
    {
        DataModelEntities entities = new DataModelEntities();

        List<ChargeCode> codes = entities.ChargeCodes.Include("User").Where(c => c.Is_Active == true && c.Charge_Code_Name.ToLower().Contains(txtChargeCodeSearch.Value) && c.Ebay_Code.ToLower().Contains(txtEbayCodeSearch.Value)).OrderBy(c => c.Ebay_Code).ToList();

        rptItems.DataSource = codes;
        rptItems.DataBind();
        entities = null;
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {

        if (hfSelectedChargeCode.Value == string.Empty) // new Items
            AddNewItems();
        else
            UpdateItemsDetails();
    }

    private void AddNewItems()
    {
        ChargeCode code = new ChargeCode();
        code.Ebay_Code = txtEbayCode.Value;
        code.Charge_Code_Name = txtChargeCode.Value;
        code.User_Code = int.Parse(ddlUser.SelectedValue);
        code.Created_By = UserKey;
        code.Created_Date = DateTime.Now;
        code.Is_Active = true;
        code.User_IP = Request.UserHostAddress;

        DataModelEntities entities = new DataModelEntities();
        entities.ChargeCodes.AddObject(code);
        entities.SaveChanges();

        BindCodes();
    }

    private void UpdateItemsDetails()
    {
        int selectedItemsCode = int.Parse(hfSelectedChargeCode.Value);
        DataModelEntities entities = new DataModelEntities();
        ChargeCode code = entities.ChargeCodes.First(u => u.Charge_Code == selectedItemsCode);

        code.Ebay_Code = txtEbayCode.Value;
        code.Charge_Code_Name = txtChargeCode.Value;
        code.User_Code = int.Parse(ddlUser.SelectedValue);
        code.Modified_By = UserKey;
        code.Modified_Date = DateTime.Now;
        code.User_IP = Request.UserHostAddress;
 
        entities.SaveChanges();
        entities = null;

        BindCodes();
    }

    protected void btnItemsSearch_Click(object sender, EventArgs e)
    {
        if (IsValid)
        {
            BindCodes();
        }
    }


    protected void btnDeleteItems_Click(object sender, ImageClickEventArgs e)
    {
        ImageButton btnDelete = (ImageButton)sender;
        RepeaterItem rptItem = (RepeaterItem)btnDelete.NamingContainer;

        int selectedChargeCode = int.Parse(((System.Web.UI.HtmlControls.HtmlInputHidden)rptItem.FindControl("hfChargeCode")).Value);

        DataModelEntities entities = new DataModelEntities();
        ChargeCode code = entities.ChargeCodes.First(u => u.Charge_Code == selectedChargeCode);
        entities.ChargeCodes.DeleteObject(code);
        entities.SaveChanges();
        entities = null;

        BindCodes();

    }


}