using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PriceManagerDAL;
using System.Data;

public partial class pages_admin_Admin_Role : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindRoles();
        }
    }

    private void BindRoles()
    {
        DataModelEntities entities = new DataModelEntities();

        List<Role> roles = entities.Roles.Where(m => (txtRoleNameSearch.Value == string.Empty || m.Role_Name.Contains(txtRoleNameSearch.Value)) && m.Is_Active == true).ToList();

        rptRole.DataSource = roles;
        rptRole.DataBind();
        entities = null;
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (hfRoleCode.Value == string.Empty) // new role
            AddNewRole();
        else
            UpdateRoleDetails();
    }

    private void AddNewRole()
    {
        Role role = new Role();
        role.Role_Name = txtRoleName.Value;
        role.Created_Date = DateTime.Now;
        role.Is_Active = true;

        DataModelEntities entities = new DataModelEntities();
        entities.AddToRoles(role);
        entities.SaveChanges();

        BindRoles();
    }

    private void UpdateRoleDetails()
    {
        int selectedRoleCode = int.Parse(hfRoleCode.Value);
        DataModelEntities entities = new DataModelEntities();
        Role role = entities.Roles.First(u => u.Role_Code == selectedRoleCode);

        role.Role_Name = txtRoleName.Value;
        entities.SaveChanges();
        entities = null;

        BindRoles();
    }

    protected void btnRoleSearch_Click(object sender, EventArgs e)
    {
        if (IsValid)
        {
            BindRoles();
        }
    }

    protected void btnDeleteRole_Click(object sender, ImageClickEventArgs e)
    {
        ImageButton btnDelete = (ImageButton)sender;
        RepeaterItem rptItem = (RepeaterItem)btnDelete.NamingContainer;

        int selectedRoleCode = int.Parse(((System.Web.UI.HtmlControls.HtmlInputHidden)rptItem.FindControl("hfListRoleCode")).Value);

        DataModelEntities entities = new DataModelEntities();
        Role role = entities.Roles.First(u => u.Role_Code == selectedRoleCode);

        role.Is_Active = false;
        entities.SaveChanges();
        entities = null;

        BindRoles();

    }
}