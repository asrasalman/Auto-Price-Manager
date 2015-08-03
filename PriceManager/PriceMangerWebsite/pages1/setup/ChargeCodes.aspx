<%@ Page Title="" Language="C#" MasterPageFile="~/master/Main.master" AutoEventWireup="true"
    CodeFile="ChargeCodes.aspx.cs" Inherits="pages_masters_ChargeCode" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="/css/styles.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="partMasterContainer">
        <h1>
            E-Parcel Charge Codes</h1>
        <p>
            Here you should define all the e-Parcel Charge Codes that corresponds to the ebay codes.
        </p>
        <div class='searchContainer22'>
            <div>
                Search by E-Parcel Charge Code:
                <input type="text" class="txtChargeCodeSearch" id="txtChargeCodeSearch" runat="server" />
            </div>
            <div>
                Search by Ebay Code:
                <input type="text" class="txtEbayCodeSearch" id="txtEbayCodeSearch" runat="server" />
            </div>
            <div>
                <asp:Button ID="btnItemSearch" runat="server" CssClass="button3" Text="Search" OnClick="btnItemsSearch_Click" />
            </div>
        </div>
        <div style='float: right'>
            <a class='addCode addSmall addRight editFunction'>Add Charge Code</a>
        </div>
        <div style="clear: both">
        </div>
        <asp:UpdatePanel ID="upData" runat="server">
            <ContentTemplate>
                <asp:Repeater ID="rptItems" runat="server">
                    <HeaderTemplate>
                        <table cellpadding="0" cellspacing="0" class="list tblItems" style="margin-bottom: 10px;">
                            <thead>
                                <tr>
                                    <th>
                                        User Name
                                    </th>
                                    <th>
                                        Ebay Code
                                    </th>
                                    <th>
                                        E-Parcel Charge Code
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
                                <tr class='emptyRow'>
                                    <td colspan='9'>
                                        List is empty. Please start adding from the option on top right of this box.
                                    </td>
                                </tr>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <tr class="searchItem">
                            <td>
                                <asp:Label ID="lblUserName" runat="server" CssClass="userName" Text='<%# Eval("User.Full_Name") %>'></asp:Label>
                                <input type="hidden" class="hfChargeCode" runat="server" id="hfChargeCode" value='<%# Eval("Charge_Code") %>' />
                                <input type="hidden" class="hfUserCode" runat="server" id="hfUserCode" value='<%# Eval("User_Code") %>' />
                            </td>
                            <td>
                                <asp:Label ID="lblEbayCode" runat="server" CssClass="lblEbayCode" Text='<%# Eval("Ebay_Code") %>'></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblChargeCodeName" runat="server" CssClass="lblChargeCodeName" Text='<%# Eval("Charge_Code_Name")%>'></asp:Label>
                            </td>
                            <td style="text-align: center;" class='editFunction'>
                                <span class='edit editCode'></span>
                            </td>
                            <td style="text-align: center;" class='editFunction'>
                                <asp:ImageButton ID="btnDeleteItems" runat="server" ImageUrl="~/images/cancel.png"
                                    OnClick="btnDeleteItems_Click" CssClass="deleteCode" />
                            </td>
                        </tr>
                    </ItemTemplate>
                    <FooterTemplate>
                        </tbody> </table>
                    </FooterTemplate>
                </asp:Repeater>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnItemSearch" EventName="Click" />
            </Triggers>
        </asp:UpdatePanel>
    </div>
    <span class="manageCodesReference"></span>
    <div class='manageCodes managePopupGeneral'>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <table>
                    <tr>
                        <td style="width: 110px;">
                            User
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlUser" runat="server" CssClass="ddlUser"></asp:DropDownList>
                            <input type="hidden" id="hfSelectedChargeCode" runat="server" class="hfSelectedChargeCode" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Ebay Code
                        </td>
                        <td>
                            <input type="text" id="txtEbayCode" runat="server" class="txtEbayCode" style="width: 250px" />
                            <asp:RequiredFieldValidator ID="rfvEbayCode" runat="server" ValidationGroup="addCode" ControlToValidate="txtEbayCode" ErrorMessage="Required"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 145px;">
                            E-Parcel Charge Code
                        </td>
                        <td>
                            <input type="text" id='txtChargeCode' runat="server" class="txtChargeCode" style="width: 250px;" />
                            <asp:RequiredFieldValidator ID="rfvChargeCode" runat="server" ValidationGroup="addCode" ControlToValidate="txtChargeCode" ErrorMessage="Required"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td>
                            <asp:Button ID="btnSave" Text="Save Code" runat="server" CssClass="button1 saveCode"
                                ValidationGroup="addCode" OnClick="btnSave_Click" />
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <script src="/Scripts/lib/jquery-ui-1.8.22.custom.min.js" type="text/javascript"></script>
    <script src="/scripts/custom/Masters.js" type="text/javascript"></script>
    <script type="text/javascript">
        function pageLoad() {
            SetTriggersCodes();

            if ($('.searchItem').length == 0) {
                $('.emptyRow').show();
            }
            else
                $('.emptyRow').hide();
        }
    </script>
</asp:Content>
