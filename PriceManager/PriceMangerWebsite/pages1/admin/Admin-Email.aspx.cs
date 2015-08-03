using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PriceManagerDAL;

public partial class pages_admin_Admin_Email : System.Web.UI.Page
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
        List<User> activeUsers = new DataModelEntities().Users.Where(i => i.Is_Active == true).ToList();

        foreach (User user in activeUsers)
        {
            user.Full_Name = user.Full_Name + " (" + user.Email_Address + ")";
        }

        chklActiveUsers.DataSource = activeUsers;
        chklActiveUsers.DataTextField = "Full_Name";
        chklActiveUsers.DataValueField = "Email_Address";
        chklActiveUsers.DataBind();

        List<User> inactiveUsers = new DataModelEntities().Users.Where(i => i.Is_Active == false).ToList();

        foreach (User user in inactiveUsers)
        {
            user.Full_Name = user.Full_Name + " (" + user.Email_Address + ")";
        }


        chklInactiveUsers.DataSource = inactiveUsers;
        chklInactiveUsers.DataTextField = "Full_Name";
        chklInactiveUsers.DataValueField = "Email_Address";
        chklInactiveUsers.DataBind();

    }
    protected void btnSendEmail_Click(object sender, EventArgs e)
    {
        if (IsValid)
        {

            try
            {
                List<string> to = new List<string>();

                for (int i = 0; i < chklActiveUsers.Items.Count; i++)
                {
                    if (chklActiveUsers.Items[i].Selected == true)
                        to.Add(chklActiveUsers.Items[i].Value);
                }

                for (int i = 0; i < chklInactiveUsers.Items.Count; i++)
                {
                    if (chklInactiveUsers.Items[i].Selected == true)
                        to.Add(chklInactiveUsers.Items[i].Value);
                }


                string htmlBody = "<html><body>" + txtEmailBody.InnerHtml + "</body></html>";

                Email.SendBulkMail(to, txtSubject.Text, Server.HtmlDecode(htmlBody), null);
                lblMessage.Text = "Email sent successfully to all selected users.";
                lblMessage.ForeColor = System.Drawing.Color.Navy;
                lblMessage.Visible = true;
            }
            catch (Exception ex)
            {
                lblMessage.Text = "There is a problem sending email. Please try again in a while or contact system support";
                lblMessage.ForeColor = System.Drawing.Color.Red;
                lblMessage.Visible = true;
            }
        }
    }
}