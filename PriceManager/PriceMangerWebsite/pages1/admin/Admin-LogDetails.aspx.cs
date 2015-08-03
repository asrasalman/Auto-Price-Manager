using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PriceManagerDAL;

public partial class pages_admin_Admin_LogDetails : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            GetLogDetails();
        }
    }

    public void GetLogDetails()
    {
        DataModelEntities entities = new DataModelEntities();

     
        DateTime _FirstDate = new DateTime();
        DateTime? _FromDate = null;
        DateTime? _ToDate = null;

        

        if (ddlDates.Value == "Today") { _FromDate = DateTime.Today; _ToDate = DateTime.Today; }
        else if (ddlDates.Value == "All_Time") { _FromDate = _FirstDate.Date; _ToDate = DateTime.Today; }
        else if (ddlDates.Value == "Custom")
        {
            if (txtFromDate.Value == "" || txtToDate.Value == "") { _FromDate = DateTime.Today; _ToDate = DateTime.Today; }
            else { _FromDate = new Base().ConvertDate(txtFromDate.Value); _ToDate = new Base().ConvertDate(txtToDate.Value); }
        }
        else { _FromDate = Convert.ToDateTime(DateTime.Today.AddDays(-int.Parse(ddlDates.Value))); _ToDate = DateTime.Today; }

        var logdetails = entities.LoginDetails.Include("User").AsEnumerable()
                .Where(ld => 
                    (ld.Created_Date >= _FromDate && ld.Created_Date <= _ToDate.Value.Add(new TimeSpan(23, 59, 59)))

                    && (
                    string.IsNullOrEmpty(txtDescSearch.Value) 
                    
                    || (ddlSearchType.Value == "username" && ld.User.Full_Name.ToLower().Contains(txtDescSearch.Value.ToLower()))

                    || (ddlSearchType.Value == "address" && ld.User.Address1.ToLower().Contains(txtDescSearch.Value.ToLower()))

                    || (ddlSearchType.Value == "browser" && ld.Browser.ToLower().Contains(txtDescSearch.Value.ToLower()))

                    || (ddlSearchType.Value == "operatingsystem" && ld.Operating_System.ToLower().Contains(txtDescSearch.Value.ToLower()))
                    
                    ))
                        .Select(de => new
                        {
                            de.User.Full_Name,
                            de.User.Email_Address,
                            de.Login_Date_Time,
                            de.User_IP,
                            de.Browser,
                            de.Operating_System,
                            de.Logout_Date_Time

                        }).ToList();

        rptItems.DataSource = logdetails;
        rptItems.DataBind();

    }
    public string FormatDate(object date)
    {
        if(date == null)
            return "";
        else
            return Convert.ToDateTime(date).ToString("MMM dd, yyyy hh:mm tt");
    }
    protected void btnItemsSearch_Click(object sender, EventArgs e)
    {
        GetLogDetails();
    }
}


public class TblLogDetails
{
    public int Login { get; set; }
}