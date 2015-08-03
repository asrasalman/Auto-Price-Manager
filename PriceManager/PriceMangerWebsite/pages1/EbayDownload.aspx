<%@ Page Title="" Language="C#" MasterPageFile="~/master/Main.master" AutoEventWireup="true"
    CodeFile="EbayDownload.aspx.cs" Inherits="pages_EbayDownload" ValidateRequest="false" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="/css/styles.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div runat="server" id="divMain">
        <div class='downloadFile'>
            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                <ContentTemplate>
                    <asp:Button ID="btnRefreshTransactions" runat="server" Text="Refresh Transactions from Live Server"
                        CssClass="button3" Style="float: right" 
                        onclick="btnRefreshTransactions_Click" />
                </ContentTemplate>
            </asp:UpdatePanel>
            <h1>
                Download your Pending Shipments:
            </h1>
            <p>
                Press the "Download Now" button below, and download all the 'Pending Shipment' transactions
                from all your connected e-commerce accounts that you've setup. Once Downloaded,
                you can process the transactions with the available options in your account.
            </p>
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <asp:Button ID="btnProcessAPI" runat="server" OnClick="btnProcessAPI_Click" Text="Download Now"
                        CssClass="button3" />
                    <asp:Label ID="lblAPIIssues" runat="server" ForeColor="Maroon" Visible="false"></asp:Label>
                    <asp:HyperLink ID="aConnectAgain" CssClass="aConnectAgain" runat="server" Text="Connect Again"
                        Visible="false"></asp:HyperLink>
                    <asp:Label ID="lblPendingShipmentCount" runat="server" CssClass='pendingShipmentCount'
                        Visible="false"></asp:Label>
                    <asp:Image ID="imgEbay" runat="server" CssClass="imgEbay" ImageUrl='/images/ebay-rightnow.gif'
                        alt="Right now on Ebay" Visible="false" />
                    <asp:Panel ID="pnlItems" runat="server" Visible="false" Style="line-height: 20px">
                        <asp:Repeater ID="rptParcelItems" runat="server">
                            <HeaderTemplate>
                                <table id='tblParcelItems' class='list tablesorter' cellspacing="0">
                                    <thead>
                                        <tr>
                                            <th class='nowidth'>
                                            </th>
                                            <th style="width: 20px">
                                                <input type="checkbox" id="chkSelectAll" />
                                            </th>
                                            <th style="width: 50px">
                                                Record#
                                            </th>
                                            <th style="width: 110px">
                                                Transaction ID
                                            </th>
                                            <th>
                                                Item Name
                                            </th>
                                            <th style="width: 100px">
                                                Buyer ID
                                            </th>
                                            <th style="width: 130px">
                                                Buyer Name
                                            </th>
                                            <th style="width: 30px">
                                                Qty
                                            </th>
                                            <th style="width: 100px">
                                                Custom Label
                                            </th>
                                            <th style="width: 135px">
                                                Shipping Method
                                            </th>
                                            <th style="width: 100px">
                                                Suburb
                                            </th>
                                            <th style="width: 60px">
                                                State
                                            </th>
                                            <th style="width: 60px">
                                                Insurance
                                            </th>
                                            <th style="width: 50px">
                                                Mail
                                            </th>
                                            <th style="width: 60px">
                                                Status
                                            </th>
                                        </tr>
                                    </thead>
                                    <tbody>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <tr class='parcelItem'>
                                    <td class='nowidth'>
                                        <%# Eval("Type") %><%# Eval("AccountID") %>
                                    </td>
                                    <td>
                                        <asp:CheckBox ID="chkSelect" runat="server" CssClass="chkSelect" />
                                        <input type="hidden" id="hfAccountID" runat="server" value='<%# Eval("AccountID") %>'
                                            class='hfAccountID' />
                                        <asp:HiddenField ID="hfState" runat="server" Value='<%# Eval("State") %>'></asp:HiddenField>
                                        <input type="hidden" id="hfAccountType" runat="server" value='<%# Eval("Type") %>'
                                            class='hfAccountType' />
                                        <input type="hidden" id="hfStreet" runat="server" value='<%# Eval("Street") %>' class='hfStreet' />
                                        <input type="hidden" id="hfStreet2" runat="server" value='<%# Eval("Street2") %>'
                                            class='hfStreet2' />
                                        <input type="hidden" id="hfStreet3" runat="server" value='<%# Eval("Street3") %>'
                                            class='hfStreet3' />
                                        <input type="hidden" id="hfCity" runat="server" value='<%# Eval("City") %>' class='hfCity' />
                                        <input type="hidden" id="hfPostalCode" runat="server" value='<%# Eval("PostalCode") %>'
                                            class='hfPostalCode' />
                                        <asp:HiddenField ID="hfCountry" runat="server" Value='<%# Eval("Country") %>'></asp:HiddenField>
                                        <asp:HiddenField ID="hfPhone" runat="server" Value='<%# Eval("Phone") %>'></asp:HiddenField>
                                        <asp:HiddenField ID="hfPrice" runat="server" Value='<%# Eval("Price") %>'></asp:HiddenField>
                                        <asp:HiddenField ID="hfCurrency" runat="server" Value='<%# Eval("Currency") %>'>
                                        </asp:HiddenField>
                                        <asp:HiddenField ID="hfShippingCost" runat="server" Value='<%# Eval("ShippingCost") %>'>
                                        </asp:HiddenField>
                                        <asp:HiddenField ID="hfSaleRecordId" runat="server" Value='<%# Eval("SaleRecordId") %>'>
                                        </asp:HiddenField>
                                        <asp:HiddenField ID="hfEmail" runat="server" Value='<%# Eval("EmailAddress") %>'>
                                        </asp:HiddenField>
                                        <asp:HiddenField ID="hfTransactionID" runat="server" Value='<%# Eval("TransactionID") %>'>
                                        </asp:HiddenField>
                                        <asp:HiddenField ID="hfItemID" runat="server" Value='<%# Eval("ItemID") %>'></asp:HiddenField>
                                        <asp:HiddenField ID="hfCustomLabel" runat="server" Value='<%# Eval("CustomLabel") %>'>
                                        </asp:HiddenField>
                                        <input type="hidden" id="hfIsIncomplete" class="hfIsIncomplete" runat="server" value='<%# Eval("IsIncomplete") %>' />
                                        <input type="hidden" id="hfIsPCFixed" runat="server" value='0' class='hfIsPCFixed' />
                                        <input type="hidden" id="hfMessages" runat="server" class='hfMessages' value='<%# Eval("Messages") %>' />
                                    </td>
                                    <td>
                                        <asp:Label ID="lblRecordNumber" runat="server" Text='<%# Eval("RecordNumber") %>'></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblTransactionID" runat="server" Text='<%# Eval("TransactionID") %>'></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblItemName" runat="server" Text='<%# Eval("ItemName") %>'></asp:Label><span class="spnIncomplete">Incomplete Details</span>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblBuyerID" runat="server" Text='<%# Eval("BuyerID") %>'></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblBuyerName" runat="server" Text='<%# Eval("BuyerName") %>'></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblQuantity" runat="server" Text='<%# Eval("Quantity") %>'></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblCustomLabel" runat="server" Text='<%#  Eval("CustomLabelText") %>'></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblShippingMethod" runat="server" Text='<%# Eval("ShippingMethod") %>'
                                            CssClass='shippingMethod'></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblSuburb" runat="server" Text='<%# Eval("City") %>' CssClass='suburbText'></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblState" runat="server" Text='<%# Eval("State") %>' CssClass='stateText'></asp:Label>
                                    </td>
                                    <td style="text-align: center">
                                        <asp:CheckBox ID="chkInsurance" runat="server" Checked='<%# Eval("HasInsurance") %>'
                                            CssClass="chkInsurance" />
                                        <asp:TextBox ID="txtInsurance" runat="server" CssClass="insuranceText" Text='<%# Eval("Insurance") %>'></asp:TextBox>
                                    </td>
                                    <td style="text-align: center">
                                        <span class='messageIcon messageRead'></span>
                                    </td>
                                    <td style="text-align: center">
                                        <asp:Image ID="imgPostCode" runat="server" CssClass="imgPostCode" ImageUrl='<%# Eval("PostCodeImageURL") %>' />
                                    </td>
                                </tr>
                            </ItemTemplate>
                            <FooterTemplate>
                                </tbody> </table>
                            </FooterTemplate>
                        </asp:Repeater>
                        <br />
                        <asp:Button ID="btnProcessSelectedItems" CssClass="btnProcessSelectedItems button3"
                            runat="server" Text="Generate E-Parcel CSV" OnClick="btnProcessSelectedItems_Click" />
                        <asp:Button ID="btnGenerateClicknSend" CssClass="btnGenerateClicknSend button3" runat="server"
                            Text="Generate Click&Send CSV" OnClick="btnGenerateClicknSend_Click" />
                        <asp:Button ID="btnGenerateInvoice" CssClass="btnGenerateInvoice button3" runat="server"
                            Text="Generate Invoice(s)" OnClick="btnGenerateInvoice_Click" />
                        <asp:Button ID="btnGeneratePickingList" CssClass="btnGeneratePickingList button3"
                            runat="server" Text="Generate Picking List" OnClick="btnGeneratePickingList_Click" />
                        <asp:Button ID="btnGenerateLabels" CssClass="btnGenerateLabels button3" runat="server"
                            Text="Generate Label(s)" OnClick="btnGenerateLabels_Click" />
                    </asp:Panel>
                    <asp:Panel runat="server" ID="pnlOutput" Visible="false" Style="line-height: 20px;">
                        <asp:HyperLink ID="hplProcessedFile" runat="server" Text="E-Parcel File Link" Target="_blank"></asp:HyperLink>
                        <br />
                        <br />
                        <asp:Label ID="lblIssuesHeading" runat="server" Font-Underline="true"></asp:Label>
                        <br />
                        <asp:Label ID="lblResult" runat="server"></asp:Label>
                    </asp:Panel>
                    <rsweb:ReportViewer ID="ReportViewer1" runat="server" Font-Names="Verdana" Font-Size="8pt"
                        InteractiveDeviceInfos="(Collection)" Visible="false" WaitMessageFont-Names="Verdana"
                        WaitMessageFont-Size="14pt" Width="988px">
                        <LocalReport ReportPath="report\Invoice.rdlc" EnableExternalImages="true" EnableHyperlinks="true">
                        </LocalReport>
                    </rsweb:ReportViewer>
                    <rsweb:ReportViewer ID="ReportViewer2" runat="server" Font-Names="Verdana" Font-Size="8pt"
                        InteractiveDeviceInfos="(Collection)" Visible="false" WaitMessageFont-Names="Verdana"
                        WaitMessageFont-Size="14pt" Width="988px">
                        <LocalReport ReportPath="report\PickingList.rdlc" EnableExternalImages="true" EnableHyperlinks="true">
                        </LocalReport>
                    </rsweb:ReportViewer>
                    <rsweb:ReportViewer ID="rvLabels" runat="server" Font-Names="Verdana" Font-Size="8pt"
                        InteractiveDeviceInfos="(Collection)" Visible="false" WaitMessageFont-Names="Verdana"
                        WaitMessageFont-Size="14pt" Width="988px">
                        <LocalReport ReportPath="report\Labels.rdlc" EnableExternalImages="true" EnableHyperlinks="true">
                        </LocalReport>
                    </rsweb:ReportViewer>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <br />
        <div class="divNoSettingFound" style="display: none">
            <h2>
                Connect Your E-Commerce Account(s)</h2>
            <p>
                You need atleast one E-Commerce account connected to Parcel Solutions before you
                can use the features.
                <br />
                <br />
                <input type="hidden" value="0" id="hdfSettings" runat="server" />
                <a href="ShopConnect.aspx" class='button3'>Take me there</a>
            </p>
        </div>
        <div id="selectSuburb">
            Address Line1:
            <br />
            <input id="txtAddress1" />
            <br />
            <br />
            Address Line2:
            <br />
            <input id="txtAddress2" />
            <br />
            <br />
            PostCode:
            <br />
            <span id='noPostCode'></span>
            <input id="txtPostCode" />
            <br />
            <br />
            Suburb:
            <br />
            <select id="ddlSuburb">
            </select>
            <input type="text" id="txtSuburb" style="display: none" />
            <br />
            <br />
            <input type="submit" class="button1" value="Change Suburb" id="btnChangeSuburb" />
            <input type="hidden" id="hfSelectedRow" />
        </div>
        <div class='messageListContainer'>
            <table id="messageList" width="100%" class='list' cellspacing="0">
                <thead>
                    <tr>
                        <th scope="col">
                            Date
                        </th>
                        <th scope="col">
                            Subject
                        </th>
                        <th scope="col">
                            Message
                        </th>
                    </tr>
                </thead>
                <tbody>
                </tbody>
            </table>
        </div>
        <asp:Image ID="Image1" runat="server" />
    </div>
    <span class='fileName'></span>
    <script src="/Scripts/lib/jquery-ui-1.8.22.custom.min.js" type="text/javascript"></script>
    <script src="/scripts/custom/Ebay.js" type="text/javascript"></script>
    <script src="/scripts/lib/colResizable-1.3.min.js" type="text/javascript"></script>
    <script src="/scripts/lib/jquery.tablesorter.js" type="text/javascript"></script>
    <script src="/scripts/lib/jquery.tmpl.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        function pageLoad() {

            if ($('#' + '<%=hdfSettings.ClientID%>').val() == "0") {

                ShowEbayConnect();
            }
            SetTriggers();
        }

    </script>
    <script id="messageTemplate" type="text/x-jQuery-tmpl">
        <tr class='messageItem'>
            <td>
                ${GetDate(Date)}
            </td>
            <td>
                ${Subject}
            </td>
            <td>
                ${Text}
            </td>
        </tr>
    </script>
</asp:Content>
