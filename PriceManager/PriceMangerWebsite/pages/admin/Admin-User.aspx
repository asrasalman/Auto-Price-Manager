<%@ Page Title="" Language="C#" MasterPageFile="~/master/Main.master" AutoEventWireup="true"
    CodeFile="Admin-User.aspx.cs" Inherits="pages_admin_Admin_User" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="rt-containerInner">
        <div class="rt-grid-12 ">
            <div class="rt-block">
                <div class="container generalContainer">
                    <div class="componentheading">
                        <h2>
                            Search Users</h2>
                    </div>
                    <p>
                        Here you can create and manage users for the system.
                    </p>
                    <div class='searchContainer22'>
                        <div style="width:50%;float:left">
                            Search by User Name:
                            <input type="text" id="txtUserNameSearch" style="width:200px" class="inputbox" runat="server" />
                        </div>
                        <div style="width:50%;float:left">
                            Search by Role:
                            <asp:DropDownList ID="ddlRoleSearch" Width="200px" runat="server">
                            </asp:DropDownList>
                            <asp:Button ID="btnUserSearch" runat="server" CssClass="button" Text="Search" OnClick="btnUserSearch_Click" />
                        </div>
                        
                        <div class="clear"></div>
                    </div>
                    <%--<div style='float: right'>
                        <a class='addUser addSmall addRight editFunction'>Add User</a>
                    </div>--%>
                    <div style="clear: both">
                    </div>
                    <asp:UpdatePanel ID="upData" runat="server">
                        <ContentTemplate>
                            <asp:Repeater ID="rptUser" runat="server">
                                <HeaderTemplate>
                                    <table cellpadding="0" cellspacing="0" class="list artistList">
                                        <thead>
                                            <tr>
                                                <th>
                                                    User Name
                                                </th>
                                                <th>
                                                    Role
                                                </th>
                                                <th>
                                                    Package
                                                </th>
                                                <th>
                                                    Email Address
                                                </th>
                                                <th style="width: 70px; text-align: center;" class='editFunction'>
                                                    Edit
                                                </th>
                                                <th style="width: 70px; text-align: center;" class='editFunction'>
                                                    Delete
                                                </th>
                                                <th style="width: 70px; text-align: center;" class='editFunction'>
                                                    Suspend
                                                </th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr class="searchItem">
                                        <td>
                                            <span class='fullName'>
                                                <%# Eval("Full_Name") %></span>
                                            <input type="hidden" class="hfListUserCode" runat="server" id="hfListUserCode" value='<%# Eval("User_Code") %>' />
                                        </td>
                                        <td>
                                            <span class='roleName'>
                                                <%# Eval("Role_Name") %></span>
                                        </td>
                                        <td>
                                            <span class='packageName'>
                                                <%# Eval("Package_Name") %></span>
                                            <input type="hidden" class="hfPackageId" runat="server" id="hfPackageId" value='<%# Eval("Package_Id") %>' />
                                        </td>
                                        <td>
                                            <span class='emailAddress'>
                                                <%# Eval("Email_Address") %></span>
                                        </td>
                                        <td style="text-align: center;" class='editFunction'>
                                            <span class='edit editUser'></span>
                                        </td>
                                        <td style="text-align: center;" class='editFunction'>
                                            <asp:ImageButton ID="btnDeleteUser" runat="server" ImageUrl="~/images/delete2.png"
                                                OnClick="btnDeleteUser_Click" CssClass="deleteUser" />
                                        </td>
                                        <td>
                                            <asp:ImageButton ID="btnLockedUser" runat="server" ImageUrl='<%# Eval("Image_Url")  %>'
                                                OnClick="btnLockedUser_Click" OnClientClick='<%# String.Format("javascript:return confirmation(\"{0}\")", Eval("Is_Locked").ToString()) %>'
                                                CssClass="lockedUser" />
                                            <input type="hidden" class="hfLockedStatus" runat="server" id="hfLockedStatus" value='<%# Eval("Is_Locked") %>' />
                                        </td>
                                    </tr>
                                </ItemTemplate>
                                <FooterTemplate>
                                    </tbody> </table>
                                </FooterTemplate>
                            </asp:Repeater>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnUserSearch" EventName="Click" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
                <br />
                <br />
                <span class="manageUserReference"></span>
                <div class='manageUser' style="display:none">
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <table>
                                <tr>
                                    <td>
                                        Role
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlRole" runat="server" CssClass="ddlRole" Width="250px">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Package
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlPackage" runat="server" CssClass="ddlPackage" Width="250px">
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="rfvPackage" runat="server" ValidationGroup="addUser"
                                            ErrorMessage="*" ControlToValidate="ddlPackage"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 110px;">
                                        User Full Name
                                    </td>
                                    <td>
                                        <input type="text" id='txtUserName' runat="server" class="txtUserName inputbox" style="width: 250px;" />
                                        <asp:RequiredFieldValidator ID="rfvUserName" runat="server" ValidationGroup="addUser"
                                            ErrorMessage="*" ControlToValidate="txtUserName"></asp:RequiredFieldValidator>
                                        <input type="hidden" id="hfUserCode" runat="server" class="hfUserCode" />
                                    </td>
                                </tr>
                                <tr>
                        
                                        Email Address
                                    </td>
                                    <td>
                                        <input type="text" id='txtEmailAddress' runat="server" class="txtEmailAddress inputbox" style="width: 250px;" />
                                        <asp:RequiredFieldValidator ID="rfvEmailAddress" runat="server" ValidationGroup="addUser"
                                            ErrorMessage="*" ControlToValidate="txtEmailAddress"></asp:RequiredFieldValidator>
                                        <asp:RegularExpressionValidator ID="regvEmail" runat="server" ControlToValidate="txtEmailAddress"
                                            ErrorMessage="Invalid Email" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                                            ValidationGroup="addUser"></asp:RegularExpressionValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="vertical-align: top; padding-top: 7px;">
                                        <asp:CheckBox ID="chkPassword" runat="server" CssClass="chkPassword inputbox" Text="Password"
                                            TextAlign="Left" Style="display: none" />
                                        <span class="lblPassword">Password</span>
                                    </td>
                                    <td>
                                        <div class='passwordContainer'>
                                            <input type="password" id='txtPassword' runat="server" class="txtPassword inputbox" style="width:100px;" />
                                            <asp:RequiredFieldValidator ID="rfvPassword" runat="server" CssClass="rfvPassword"
                                                ValidationGroup="addUser" ErrorMessage="*" ControlToValidate="txtPassword"></asp:RequiredFieldValidator>
                                            Confirm
                                            <input type="password" id='txtPasswordConfirm' runat="server" class="txtPasswordConfirm inputbox" style="width:100px;" />
                                            <asp:RequiredFieldValidator ID="rfvPasswordConfirm" CssClass="rfvPasswordConfirm"
                                                runat="server" ValidationGroup="addUser" ErrorMessage="*" ControlToValidate="txtPasswordConfirm"></asp:RequiredFieldValidator>
                                            <asp:CompareValidator ID="cmpPassword" runat="server" ErrorMessage="Passwords not matching"
                                                ValueToCompare="Value" ValidationGroup="addUser" ControlToCompare="txtPassword"
                                                ControlToValidate="txtPasswordConfirm"></asp:CompareValidator>
                                        </div>
                                    </td>

                                </tr>
                                <tr>
                                    <td>
                                    </td>
                                    <td>
                                        <asp:Button ID="btnSave" Text="Save User" runat="server" CssClass="button saveUser"
                                            ValidationGroup="addUser" OnClick="btnSave_Click" />
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>
     <script src="/Scripts/lib/jquery-1.7.1.min.js" type="text/javascript"></script>
    <script src="/Scripts/lib/jquery-ui-1.8.22.custom.min.js" type="text/javascript"></script>
    <script src="/Scripts/custom/Admin.js" type="text/javascript"></script>
    <script type="text/javascript">
        function pageLoad() {
            SetTriggersUser();
        }
        function confirmation(status) {
            var text = '';
            if (status == 'True') {
                text = 'unsuspend';
            }

            else { text = 'suspend' }
            return confirm('Are you sure you want to ' + text + ' user?');
        }
    </script>
</asp:Content>
