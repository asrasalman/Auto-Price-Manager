using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using PriceManagerDAL;
using System.Data;
using System.ComponentModel;
public partial class pages_admin_Admin_RoleAccess : System.Web.UI.Page
{

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindRoles();
            BindAccess();

        }
    }

    private void BindRoles()
    {
        DataModelEntities entities = new DataModelEntities();

        List<Role> roles = entities.Roles.Where(r => r.Is_Active == true).ToList();
        Common.BindDropDown(ddlRole, roles, "Role_Name", "Role_Code", false, false);
        entities = null;
    }

    private void BindAccess()
    {
        DataModelEntities entities = new DataModelEntities();
        int role = int.Parse(ddlRole.SelectedValue);
        List<GetRoleAccess_Result> menuList = entities.GetRoleAccess(role).ToList();

        tvMenu.SetDataSource(GenerateDataSet(menuList));

        List<AccessFunction> accessFunctions = entities.AccessFunctions.ToList();
        rptAccess.DataSource = accessFunctions;
        rptAccess.DataBind();
    }

    private DataSet GenerateDataSet(List<GetRoleAccess_Result> list)
    {
        DataSet dSet = new DataSet();
        DataTable dTable = new DataTable();

        DataColumn dcId = new DataColumn("Id");
        DataColumn dcText = new DataColumn("Text");
        DataColumn dcLink = new DataColumn("NavigateUrl");
        DataColumn dcParentId = new DataColumn("ParentId");
        DataColumn dcChecked = new DataColumn("Checked");
        DataColumn dcCustom1 = new DataColumn("Custom1");
        DataColumn dcCustom2 = new DataColumn("Custom2");

        dTable.Columns.Add(dcId);
        dTable.Columns.Add(dcText);
        dTable.Columns.Add(dcLink);
        dTable.Columns.Add(dcParentId);
        dTable.Columns.Add(dcChecked);
        dTable.Columns.Add(dcCustom1);
        dTable.Columns.Add(dcCustom2);

        for (int i = 0; i < list.Count; i++)
        {
            DataRow dRow = dTable.NewRow();
            dRow[dcId] = list[i].Menu_Item_Code.ToString();
            dRow[dcText] = list[i].Menu_Item_Name;
            dRow[dcLink] = "#";
            dRow[dcParentId] = list[i].Parent_Menu_Item_Code;
            dRow[dcChecked] = list[i].Has_Access.ToString().ToLower();
            dRow[dcCustom1] = list[i].Menu_URL == "#" ? null : "true";
            dTable.Rows.Add(dRow);
        }

        // generate relation in table
        dTable.TableName = "Tree";
        dSet.Tables.Add(dTable);
        dSet.DataSetName = "Trees";

        //create Relation Parent and Child
        DataRelation relation = new DataRelation("ParentChild",
            dSet.Tables["Tree"].Columns["Id"],
            dSet.Tables["Tree"].Columns["ParentId"],
            false);

        relation.Nested = true;

        dSet.Relations.Add(relation);
        return dSet;
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        DataModelEntities entities = new DataModelEntities();
        foreach (TreeNode treeNode in tvMenu.TreeNodes)
        {
            int id = int.Parse(treeNode.Id);
            string name = treeNode.Name;
            string parentId = treeNode.ParentId;
            bool isChecked = treeNode.IsChecked;
            int role = int.Parse(ddlRole.SelectedValue);

            RoleAccess roleAccess = entities.RoleAccesses.FirstOrDefault(r => r.MenuItem.Menu_Item_Code == id && r.Role.Role_Code == role);

            if (roleAccess == null)
            {
                if (isChecked == true)
                {
                    RoleAccess roleAccessNew = new RoleAccess();
                    roleAccessNew.Menu_Item_Code = id;
                    roleAccessNew.Role_Code = role;
                    roleAccessNew.Has_Access = true;
                    roleAccessNew.Created_Date = DateTime.Now;
                    roleAccessNew.Is_Active = true;

                    entities.AddToRoleAccesses(roleAccessNew);

                    entities.SaveChanges();
                }
            }
            else
            {
                roleAccess.Menu_Item_Code = id;
                roleAccess.Role_Code = role;
                roleAccess.Has_Access = isChecked;

                entities.SaveChanges();
            }

        }

    }
    protected void ddlRole_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindAccess();
    }
}