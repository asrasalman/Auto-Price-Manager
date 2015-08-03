using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PriceManagerDAL;
using System.Data;
using System.Net.Mail;
using System.IO;

public partial class pages_admin_Admin_User : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindRoles();
            BindPackages();
            BindUsers();
        }
    }

    private void BindPackages()
    {
        Common.BindDropDown(ddlPackage, new DataModelEntities().Packages.Where(p => p.Is_Active == true).ToList(), "Package_Name", "Package_ID", false,false);
    }

    private void BindRoles()
    {
        DataModelEntities entities = new DataModelEntities();

        List<Role> roles = entities.Roles.ToList();
        Common.BindDropDown(ddlRole, roles, "Role_Name", "Role_Code", false, false);
        Common.BindDropDown(ddlRoleSearch, roles, "Role_Name", "Role_Code", true, false);
        entities = null;
    }

    private void BindUsers()
    {
        DataModelEntities entities = new DataModelEntities();

        int selectedRole = int.Parse(ddlRoleSearch.SelectedValue);
        //List<User> users = entities.Users.Include("Role").Include("Package").Where(m => (txtUserNameSearch.Value == string.Empty || m.Full_Name.Contains(txtUserNameSearch.Value)) &&
        //                                                             (selectedRole == 0 || m.Role.Role_Code == selectedRole) && m.Is_Active == true).ToList();

        var users = entities.Users.Include("Role").Include("Package").Where(m => (txtUserNameSearch.Value == string.Empty || m.Full_Name.Contains(txtUserNameSearch.Value)) &&
                                                                     (selectedRole == 0 || m.Role.Role_Code == selectedRole) && m.Is_Active == true)
                                                                     .Select(s => new 
                                                                     {
                                                                         User_Code = s.User_Code,
                                                                         Full_Name = s.Full_Name,
                                                                         Role_Code = s.Role.Role_Code,
                                                                         Role_Name = s.Role.Role_Name,
                                                                         Package_Id = s.Package.Package_Id,
                                                                         Package_Name = s.Package.Package_Name,
                                                                         Email_Address = s.Email_Address,
                                                                         Is_Locked = s.Is_Locked == null ? false : true,
                                                                         Image_Url = s.Is_Locked == null ? "~/images/unlock_green.png" : s.Is_Locked == true ? "~/images/lock_red.png" : s.Is_Locked == false ? "~/images/unlock_green.png" : ""

                                                                     }).ToList();


        rptUser.DataSource = users;
        rptUser.DataBind();
        entities = null;
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (hfUserCode.Value == string.Empty) // new user
            AddNewUser();
        else
            UpdateUserDetails();
    }

    private void AddNewUser()
    {
        User user = new User();
        user.Full_Name = txtUserName.Value;
        user.Email_Address = txtEmailAddress.Value;
        user.Password = Common.GetHash(txtPassword.Value);
        user.Role_Code = int.Parse(ddlRole.SelectedValue);
        user.Package_Id = int.Parse(ddlPackage.SelectedValue);
        user.Created_Date = DateTime.Now;

        
        user.Is_Active = true;

        DataModelEntities entities = new DataModelEntities();
        entities.AddToUsers(user);
        entities.SaveChanges();

    

        BindUsers();
    }

    private void UpdateUserDetails()
    {
        int selectedUserCode = int.Parse(hfUserCode.Value);
        DataModelEntities entities = new DataModelEntities();
        User user = entities.Users.First(u => u.User_Code == selectedUserCode);

        user.Full_Name = txtUserName.Value;
        user.Email_Address = txtEmailAddress.Value;
        user.Role_Code = int.Parse(ddlRole.SelectedValue);
        user.Package_Id = int.Parse(ddlPackage.SelectedValue);
        if (chkPassword.Checked == true)
            user.Password = Common.GetHash(txtPassword.Value);
        entities.SaveChanges();
        entities = null;

        BindUsers();
    }

    protected void btnUserSearch_Click(object sender, EventArgs e)
    {
        if (IsValid)
        {
            BindUsers();
        }
    }

    protected void btnDeleteUser_Click(object sender, ImageClickEventArgs e)
    {
        ImageButton btnDelete = (ImageButton)sender;
        RepeaterItem rptItem = (RepeaterItem)btnDelete.NamingContainer;

        int selectedUserCode = int.Parse(((System.Web.UI.HtmlControls.HtmlInputHidden)rptItem.FindControl("hfListUserCode")).Value);

        DataModelEntities entities = new DataModelEntities();
        User user = entities.Users.First(u => u.User_Code == selectedUserCode);

        user.Is_Active = false;
        entities.SaveChanges();
        entities = null;

        BindUsers();

    }

    protected void btnLockedUser_Click(object sender, ImageClickEventArgs e)
    {
        ImageButton btnLocked = (ImageButton)sender;
        RepeaterItem rptItem = (RepeaterItem)btnLocked.NamingContainer;

        int selectedUserCode = int.Parse(((System.Web.UI.HtmlControls.HtmlInputHidden)rptItem.FindControl("hfListUserCode")).Value);
        bool currentUserStatus = Convert.ToBoolean(((System.Web.UI.HtmlControls.HtmlInputHidden)rptItem.FindControl("hfLockedStatus")).Value);

        DataModelEntities entities = new DataModelEntities();
        User user = entities.Users.First(u => u.User_Code == selectedUserCode);
        
        if (currentUserStatus == true)
            user.Is_Locked = false;
        else
            user.Is_Locked = true;

        entities.SaveChanges();
        entities = null;

        BindUsers();

    }



}