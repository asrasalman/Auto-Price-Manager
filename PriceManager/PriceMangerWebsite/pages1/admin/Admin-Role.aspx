<%@ Page Title="" Language="C#" MasterPageFile="~/master/Main.master" AutoEventWireup="true"
    CodeFile="Admin-Role.aspx.cs" Inherits="pages_admin_Admin_Role" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="/css/styles.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="container generalContainer">
        <h1>Search Roles</h1>
        <p>
            Here you can create all the roles associated to the system.
        </p>

        <div class='searchContainer22'>
            <div>
                Search by Role Name:
                <input type="text" id="txtRoleNameSearch" runat="server" />
            </div>
            <div>
                <asp:Button ID="btnRoleSearch" runat="server" CssClass="button3" Text="Search" OnClick="btnRoleSearch_Click" />
            </div>
        </div>
        <div style='float: right'>
            <a class='addRole addSmall addRight editFunction'>Add Role</a>
        </div>
        <div style="clear: both">
        </div>
        <asp:UpdatePanel ID="upData" runat="server">
            <ContentTemplate>
                <asp:Repeater ID="rptRole" runat="server">
                    <HeaderTemplate>
                        <table cellpadding="0" cellspacing="0" class="list artistList">
                            <thead>
                                <tr>
                                    <th>
                                        Role Name
                                    </th>
                                    <th style="width: 70px; text-align: center;" class='editFunction'>
                                        Edit
                                    </th>
                                    <th style="width: 70px; text-align: center;" class='editFunction'>
                                        Delete
                                    </th>
                                </tr>
                            </thead>
                            <tbody>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <tr class="searchItem">
                            <td>
                                <span class='roleName'>
                                    <%# Eval("Role_Name") %></span>
                                <input type="hidden" class="hfListRoleCode" runat="server" id="hfListRoleCode" value='<%# Eval("Role_Code") %>' />
                            </td>
                            <td style="text-align: center;" class='editFunction'>
                                <span class='edit editRole'></span>
                            </td>
                            <td style="text-align: center;" class='editFunction'>
                                <asp:ImageButton ID="btnDeleteRole" runat="server" ImageUrl="~/images/delete2.png"
                                    OnClick="btnDeleteRole_Click" CssClass="deleteRole" />
                            </td>
                        </tr>
                    </ItemTemplate>
                    <FooterTemplate>
                        </tbody> </table>
                    </FooterTemplate>
                </asp:Repeater>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnRoleSearch" EventName="Click" />
            </Triggers>
        </asp:UpdatePanel>
    </div>
    <span class="manageRoleReference"></span>
    <div class='manageRole managePopupGeneral'>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <table>
                    <tr>
                        <td style="width: 110px;">
                            Role Name
                        </td>
                        <td>
                            <input type="text" id='txtRoleName' runat="server" class="txtRoleName" style="width: 250px;" />
                            <asp:RequiredFieldValidator ID="rfvRoleName" runat="server" ValidationGroup="addRole"
                                ErrorMessage="*" ControlToValidate="txtRoleName"></asp:RequiredFieldValidator>
                            <input type="hidden" id="hfRoleCode" runat="server" class="hfRoleCode" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td>
                            <asp:Button ID="btnSave" Text="Save Role" runat="server" CssClass="button1 saveRole"
                                ValidationGroup="addRole" OnClick="btnSave_Click" />
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <script src="/Scripts/lib/jquery-ui-1.8.22.custom.min.js" type="text/javascript"></script>
    <script src="/Scripts/custom/Admin.js" type="text/javascript"></script>
    <script type="text/javascript">
        function pageLoad() {
            SetTriggersRole();
        }
    </script>
</asp:Content>
