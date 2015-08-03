<%@ Page Title="" Language="C#" MasterPageFile="~/master/Main.master" AutoEventWireup="true"
    CodeFile="ShopConnect.aspx.cs" Inherits="pages_ShopConnect" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="/css/styles.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:HiddenField ID="hfSessionID" runat="server" />
    <h1>
        Connect Your E-Commerce Account(s)</h1>
    <br />
    <div class="generalContainer">
        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
            <ContentTemplate>
                <h2>
                    Connect Your Ebay Account</h2>
                <asp:Repeater ID="rptEbayConnectedAccounts" runat="server">
                    <ItemTemplate>
                        <div class='shopItem'>
                            <div id='ebayConnected' class='ebayConnected' runat="server">
                                Connected To Ebay Account
                                <%# Container.ItemIndex + 1 %>
                            </div>
                            <input type="hidden" class='hfEbayAccountID' value='<%# Eval("User_Account_Code") %>' />
                            <a class='refreshConnection openBox'>Create New Token</a>
                            <a class='cancelEbayToken cancelToken'>Cancel Token</a>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
                <div id="ebayAddMore" class="ebayAddMore openBox" runat="server" style="display: none">
                    Add Another Account
                </div>
                <input type="hidden" runat="server" id="hfEbayAccountsRemaining" class='hfEbayAccountsRemaining' />
                <input type="hidden" value="0" id="hdfEbaySettings" runat="server" />
                <input type="hidden" value="0" id="hfSelectedEbayAccountID" runat="server" class='hfSelectedEbayAccountID' />
                <p id='ebayConnect' class='ebayConnect' runat="server">
                    In order to connect and download Ebay transactions, you need to authorize this application
                    to use your Ebay Account. Please press the button below to open Ebay and verify
                    our application.
                    <br />
                    <br />
                    Note: The authorization takes place at Ebay.com and we do not get or use your Ebay
                    UserID / Password.
                    <br />
                    <br />
                    <input type="button" class="button3" id="btnAuthorization" value="Connect To Ebay" />
                    <input type="hidden" id="hfAuthURL" runat="server" class="hfAuthURL" />
                    <br />
                    <br />
                    Once you are done with the authorization process, please close that window and press
                    the following confirmation button.
                    <br />
                    <br />
                    <asp:Button CssClass="button3" ID="btnConfirmAuthorization" Text="Confirm Authorization"
                        runat="server" OnClick="btnConfirmAuthorization_Click" />
                    <asp:Label ID="lblAuthError" runat="server" ForeColor="Maroon"></asp:Label>
                </p>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <br />
    <br />
    <div class="generalContainer">
        <h2>
            Connect Your Shopify Account</h2>
        <div id='shopifyConnected' runat="server" class="shopifyConnectedBox">
            <div class='shopifyConnected'>
                Connected To Shopify
            </div>
            <a class='refreshShopifyConnection'>Create New Token</a>
            <a class='cancelShopifyToken cancelToken'>Cancel Token</a>
        </div>
        <p id='shopifyConnect' class='shopifyConnect' runat="server">
            In order to connect and download Shopify transactions, you need to authorize this
            application to use your Shopify Account. Please press the button below to open Shopify
            and verify our application.
            <br />
            <br />
            Note: The authorization takes place at Shopify.com and we do not get or use your
            Shopify UserID / Password.
            <br />
            <br />
            Your Shopify URL:<br />
            <input type="text" id="txtShopName" class="bigTextBox2" /><span class='bigMessage'>.myshopify.com</span><br />
            <asp:Button ID="btnAuthorizationShopify" runat="server" CssClass="button3 btnAuthorizationShopify"
                Text="Connect To Shopify" OnClick="btnAuthorizationShopify_Click" />
            <input type="hidden" value="0" id="hfSelectedShopifyAccountID" runat="server" class='hfSelectedShopifyAccountID' />
        </p>
    </div>
    <br />
    <br />
    <div class='generalContainer'>
        <h2>
            Connect Your Magento Account</h2>
        <div id='magentoConnected' runat="server" class="magentoConnectedBox">
            <div class='magentoConnected'>
                Connected To Magento
            </div>
            <a class='refreshMagentoConnection'>Change Magento Account</a>
            <a class='cancelMagentoToken cancelToken'>Remove Integration</a>
        </div>
        <p id='magentoConnect' class='magentoConnect' runat="server">
            In order to connect and download Magento transactions, you need to authorize this
            application to use your Magento Account. Please enter the following details 
            to connect your Magento store to our application.
            <br />
            <br />
            Your Magento URL:<br />
            <input type="text" id="txtMagentoStoreURL" class="bigTextBox2" style="width: 550px;" runat="server"/><asp:RequiredFieldValidator ID="rfvMagentoURL" runat="server" ControlToValidate="txtMagentoStoreURL" ErrorMessage="URL Required" ValidationGroup="magento" ></asp:RequiredFieldValidator><br />
            Your API UserID:<br />
            <input type="text" id="txtMagentoUserID" class="bigTextBox2" runat="server"/><asp:RequiredFieldValidator ID="rfvMagentoUserID" runat="server" ControlToValidate="txtMagentoUserID" ErrorMessage="API User ID Required"  ValidationGroup="magento"></asp:RequiredFieldValidator><br />
            Your API Key:<br />
            <input type="text" id="txtMagentoKey" class="bigTextBox2" runat="server"/><asp:RequiredFieldValidator ID="rfvMagentoKey" runat="server" ControlToValidate="txtMagentoKey" ErrorMessage="API Key Required" ValidationGroup="magento" ></asp:RequiredFieldValidator><br />
            <asp:Button ID="btnAuthorizationMagento" runat="server" CssClass="button3 btnAuthorizationMagento"
                Text="Save Magento Credentials" OnClick="btnAuthorizationMagento_Click" ValidationGroup="magento" />
            <input type="hidden" value="0" id="hfSelectedMagentoAccountID" runat="server" class='hfSelectedMagentoAccountID' />
        </p>
        <div class="clear"></div>
        <asp:Label ID="lblMagentoError" runat="server" CssClass="magentoError"></asp:Label>
    </div>
    <br />
    <br />
    <div class='generalContainer'>
        <h2>
            Connect Your Bigcommerce Account</h2>
        <div id='bigcommerceConnected' runat="server" class="bigcommerceConnectedBox">
            <div class='bigcommerceConnected'>
                Connected To Bigcommerce
            </div>
            <a class='refreshBigcommerceConnection'>Change Bigcommerce Account</a>
            <a class='cancelBigcommerceToken cancelToken'>Remove Integration</a>
        </div>
        <p id='bigcommerceConnect' class='bigcommerceConnect' runat="server">
            In order to connect and download Bigcommerce transactions, you need to authorize this
            application to use your Bigcommerce Account. Please enter the following details 
            to connect your Magento store to our application.
            <br />
            <br />
            Your Bigcommerce URL:<br />
            <input type="text" id="txtBigcommerceStoreURL" class="bigTextBox2" style="width: 550px;" runat="server"/><asp:RequiredFieldValidator ID="rfvBigcommerceURL" runat="server" ControlToValidate="txtBigcommerceStoreURL" ErrorMessage="URL Required" ValidationGroup="Bigcommerce" ></asp:RequiredFieldValidator><br />
            Your API UserID:<br />
            <input type="text" id="txtBigcommerceUserID" class="bigTextBox2" runat="server"/><asp:RequiredFieldValidator ID="rfvBigcommerceUserID" runat="server" ControlToValidate="txtBigcommerceUserID" ErrorMessage="API User ID Required"  ValidationGroup="Bigcommerce"></asp:RequiredFieldValidator><br />
            Your API Key:<br />
            <input type="text" id="txtBigcommerceKey" class="bigTextBox2" runat="server"/><asp:RequiredFieldValidator ID="rfvBigcommerceKey" runat="server" ControlToValidate="txtBigcommerceKey" ErrorMessage="API Key Required" ValidationGroup="Bigcommerce" ></asp:RequiredFieldValidator><br />
            <asp:Button ID="btnAuthorizationBigcommerce" runat="server" CssClass="button3 btnAuthorizationBigcommerce"
                Text="Save Bigcommerce Credentials" OnClick="btnAuthorizationBigcommerce_Click" ValidationGroup="Bigcommerce" />
            <input type="hidden" value="0" id="hfSelectedBigcommerceAccountID" runat="server" class='hfSelectedBigcommerceAccountID' />
        </p>
        <div class="clear"></div>
        <asp:Label ID="bigcommerceError" runat="server" CssClass="bigcommerceError"></asp:Label>
    </div>
    <script src="/Scripts/lib/jquery-ui-1.8.22.custom.min.js" type="text/javascript"></script>
    <script src="/scripts/custom/Ebay.js" type="text/javascript"></script>
    <script src="/scripts/lib/colResizable-1.3.min.js" type="text/javascript"></script>
    <script src="/scripts/lib/jquery.tablesorter.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        function pageLoad() {

            if ($('#' + '<%=hdfEbaySettings.ClientID%>').val() == "0") {

                ShowEbayConnect();
            }
            SetConnectTriggers();
            $('#btnAuthorization').click(OpenAuthWindow);
            $('.btnAuthorizationShopify').click(OpenShopifyAuthWindow);
        }

    </script>
</asp:Content>
