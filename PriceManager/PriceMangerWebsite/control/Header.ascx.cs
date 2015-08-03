using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PriceManagerDAL;


public partial class control_Header : System.Web.UI.UserControl
{
    List<GetRoleAccess_Result> menuList;
    public string IsAdmin, CanAddUpdate, CanView;
    public System.Text.StringBuilder menu = new System.Text.StringBuilder();

    protected void Page_Init(object sender, EventArgs e)
    {
        // check if the current user is logged in , and has permissions.
        AuthenticateUser();
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        // create Menu
        int role = int.Parse(new Base().RoleCode);

        DataModelEntities entities = new DataModelEntities();
        menuList = entities.GetRoleAccess(role).ToList();

        MakeMenuHTML();
        entities = null;

        lblLoginStatus.Text = new Base().FullName;

    }

    private void AuthenticateUser()
    {
        // check if the user is logged in, and if Yes then set the access functions on the page according to database, for the javascript to work
        if (new Base().UserKey == 0 || string.IsNullOrEmpty(new Base().RoleCode) == true)
            Response.Redirect("~/pages/login.aspx?returnurl=" + Request.Url.PathAndQuery, true);
        else
        {
            int roleCode = int.Parse(new Base().RoleCode);
            string pageUrl = this.Request.Url.LocalPath;
            RoleAccessFunction accessFunction = new DataModelEntities().RoleAccessFunctions.OrderByDescending(f => f.Menu_Item_Code).FirstOrDefault(r => r.Role_Code == roleCode && (r.Menu_Item_Code == null || r.MenuItem.Menu_URL == pageUrl));
            if (accessFunction == null)
            {
                IsAdmin = CanAddUpdate = "false";
                CanView = "true";
            }
            else
            {
                IsAdmin = accessFunction.Access_Function_Code == (int)Constant.enumAccessFunction.Admin ? "true" : "false";
                CanAddUpdate = accessFunction.Access_Function_Code == (int)Constant.enumAccessFunction.AddUpdate ? "true" : "false";
                CanView = accessFunction.Access_Function_Code == (int)Constant.enumAccessFunction.View ? "true" : "false";
            }
        }
    }

    private void MakeMenuHTML()
    {
        // write html for the menu
        //menu.Append(@"<ul>");

        List<GetRoleAccess_Result> menuTop = menuList.Where(m => m.Parent_Menu_Item_Code.ToString() == string.Empty).OrderBy(m=> m.Order).ToList();

        for (int i = 0; i < menuTop.Count; i++)
        {
            if (menuTop[i].Has_Access == true)
            {
                string url = menuTop[i].Menu_URL == string.Empty ? "#" : menuTop[i].Menu_URL;
                menu.Append(@"<li><a href='" + url + "'><span>" + menuTop[i].Menu_Item_Name + "</span></a>");
                List<GetRoleAccess_Result> menuChild = menuList.Where(m => m.Parent_Menu_Item_Code == menuTop[i].Menu_Item_Code).ToList();
                if (menuChild.Count > 0)
                    MakeSubMenuHTML(menuChild);
                menu.Append(@"</li>");
            }
        } 
        //menu.Append(@"</ul>");

    }

    private void MakeSubMenuHTML(List<GetRoleAccess_Result> menuChild)
    {
        menu.Append(@"<ul>");
        for (int j = 0; j < menuChild.Count; j++)
        {
            if (menuChild[j].Has_Access == true)
            {
                string url = menuChild[j].Menu_URL == string.Empty ? "#" : menuChild[j].Menu_URL;
                menu.Append(@"<li><a href='" + url + "'><span>" + menuChild[j].Menu_Item_Name + "</span></a>");

                List<GetRoleAccess_Result> moreMenuChild = menuList.Where(m => m.Parent_Menu_Item_Code == menuChild[j].Menu_Item_Code).ToList();
                if (moreMenuChild.Count > 0)
                    MakeSubMenuHTML(moreMenuChild);
                menu.Append(@"</li>");
            }
        }
        menu.Append(@"</ul>");
    }

    protected void lnkSignout_Click(object sender, EventArgs e)
    {
        // remove session before sending to login page.
        Base baseClass = new Base();
        if (!string.IsNullOrEmpty(baseClass.LoginDetailCode))
        {
            DataModelEntities context = new DataModelEntities();
            int LoginDetailCode = int.Parse(new Base().LoginDetailCode);
            LoginDetail ld = context.LoginDetails.FirstOrDefault(l => l.Login_Detail_Code == LoginDetailCode);

            ld.User_Code = baseClass.UserKey;
            ld.Browser = Request.Browser.Browser;
            ld.Operating_System = Request.Browser.Platform;
            ld.Logout_Date_Time = System.DateTime.Now;
            ld.Created_By = baseClass.UserKey;
            ld.Created_Date = ld.Login_Date_Time;
            ld.User_IP = Request.UserHostAddress;
            context.SaveChanges();
        }
        
        Session.Abandon();
        new Base().ExpireCookie();
        Response.Redirect("/default.aspx", true);
    }
}