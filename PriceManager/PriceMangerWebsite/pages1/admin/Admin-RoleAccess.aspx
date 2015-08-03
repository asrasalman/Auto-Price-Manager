<%@ Page Title="" Language="C#" MasterPageFile="~/master/Main.master" AutoEventWireup="true"
    CodeFile="Admin-RoleAccess.aspx.cs" Inherits="pages_admin_Admin_RoleAccess" %>

<%@ Register Src="~/control/TreeView.ascx" TagName="TreeView" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="/css/styles.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <link href="/scripts/lib/treeview/themes/classic/style.css" rel="stylesheet" type="text/css" />
    <div class="container generalContainer">
        <span class='pageHeading'>Setup Role Access</span>
        <p>
            Here you can provide the page level access for the roles you created.
        </p>
        <table>
            <tr>
                <td>
                    Select Role:
                </td>
                <td>
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td>
                    <asp:DropDownList ID="ddlRole" runat="server" CssClass="dropdownBig ddlRole" Width="216px"
                        AutoPostBack="true" OnSelectedIndexChanged="ddlRole_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
                <td>
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td style="padding-top: 10px;">
                    Select Pages to provide access:
                </td>
                <td style="padding-top: 10px;">
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td>
                    <uc1:TreeView ID="tvMenu" runat="server" Theme="Classic" EnableCheckBox="true" EnableDragDrop="false"
                        EnableContextMenu="false" AdminMode="false" />
                </td>
                <td>
                    <asp:Repeater ID="rptAccess" runat="server">
                        <HeaderTemplate>
                            <table id="tblAccess" cellpadding="2" cellspacing="0">
                                <thead>
                                    <tr>
                                        <th>
                                        </th>
                                        <th>
                                            Access
                                        </th>
                                    </tr>
                                </thead>
                                <tbody>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <tr>
                                <td>
                                    <input type="radio" name="rbAccess1" class="chkSelect" />
                                </td>
                                <td>
                                    <%# Eval("Access_Function_Name") %>
                                    <input type="hidden" runat="server" id="hfAccessFunctionCode" class="hfAccessFunctionCode"
                                        value='<%# Eval("Access_Function_Code") %>' />
                                </td>
                            </tr>
                        </ItemTemplate>
                        <FooterTemplate>
                            <tr>
                                <td colspan="2">
                                    <input type="submit" class="button1 btnSaveAccessPage editFunction" value="Save Access" />
                                </td>
                            </tr>
                            </tbody> </table>
                        </FooterTemplate>
                    </asp:Repeater>
                </td>
            </tr>
            <tr>
                <td>
                        <asp:Button ID="btnSave" runat="server" Text="Save Changes" OnClick="btnSave_Click" CssClass="button3 editFunction" style="margin-top:10px;" />
                    
                </td>
                <td>
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td>
                </td>
                <td>
                    &nbsp;
                </td>
            </tr>
        </table>
    </div>
    <script src="/scripts/custom/Admin.js" type="text/javascript"></script>
    <script type="text/javascript">
        function pageLoad() {
            SetAccessRights();
            $('.btnSaveAccessPage').live('click', SaveAccessDetailsForPage);

            var t = setTimeout(function () {
                $(document).on('click', '.jstree li', GetAccessDetailsForPage);
                $('.jstree li[custom1="true"]').addClass('textBlue');
            }, 2000)
        }

        function SetAccessRights() {
            if ($('#hfIsAdmin').val() == 'false' && $('#hfCanAddUpdate').val() == 'false') {
                $('.editFunction').remove();
            }
        }

    </script>
</asp:Content>
