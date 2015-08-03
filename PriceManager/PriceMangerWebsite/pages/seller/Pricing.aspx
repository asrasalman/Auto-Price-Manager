<%@ Page Title="" Language="C#" MasterPageFile="~/master/Main.master" AutoEventWireup="true"
    CodeFile="Pricing.aspx.cs" Inherits="pages_seller_Pricing" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="/css/select2.css" rel="stylesheet" type="text/css" />

    <style>
    .spnNoStock
    {
        width: 54px;
        display: inline-block;
        float: left;
        padding: 6px;
        text-decoration: none;
        margin: 2px;
        color: rgb(242, 26, 26);
        font-size: 14px;
        vertical-align: top;
        font-weight: bold !important;
        text-align: center;
}
    }
    
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="rt-containerInner">
        <div class="rt-grid-12 ">
            <div class="rt-block">
                <input type="hidden" id="hfTokenJSON" runat="server" class="hfTokenJSON" clientidmode="Static" />
                <input type="hidden" id="hfSeller" runat="server" class="hfSeller" clientidmode="Static" />
                <input type="hidden" id="hfCountryCode" runat="server" class="hfCountryCode" clientidmode="Static" />
                <div class="downloadFile">
                    <div class="componentheading">
                        <h2>
                            Pricing Automation</h2>
                    </div>
                    <p>
                     You can use the available settings on each Listing to enable automatic pricing adjustments on your selected Listings. Using the Filters,
                        Exceptions and Hard Floor / Ceiling Prices you are able to comfortably set and forget your Listings knowing that you will be well priced
                         to suit your market needs.  See the Tutorials pages for Video training.
                    </p>
                    <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                        <ContentTemplate>
                            <div class='searchContainer22' runat="server" visible="false">
                                <div class="componentheading">
                                    <h3>
                                        Automate Pricing Settings
                                        <asp:Button ID="btnUpdateSettings" runat="server" Text="Update" CssClass="button"
                                            Style="float: right" OnClick="btnUpdateSettings_Click" />
                                    </h3>
                                    <br />
                                </div>
                                <div style="width: 33%; float: left">
                                    Search Only Top:
                                    <asp:DropDownList ID="ddlSeachOnlyTop" runat="server" ClientIDMode="Static" Style="width: 150px">
                                        <asp:ListItem Value="1" Selected="True" Text="Not Specified"></asp:ListItem>
                                        <asp:ListItem Value="5" Text="5 Itmes"></asp:ListItem>
                                        <asp:ListItem Value="10" Text="10 Items"></asp:ListItem>
                                        <asp:ListItem Value="15" Text="15 Items"></asp:ListItem>
                                        <asp:ListItem Value="25" Text="25 Items"></asp:ListItem>
                                        <asp:ListItem Value="50" Text="50 Items"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                                <div style="width: 33%; float: left">
                                    Country:
                                    <asp:DropDownList ID="ddlCountrySettings" runat="server" ClientIDMode="Static" Style="width: 150px">
                                    </asp:DropDownList>
                                </div>
                                <div style="width: 33%; float: left">
                                    Time Delay:
                                    <asp:DropDownList ID="ddlTimeDelay" runat="server" ClientIDMode="Static" Style="width: 150px">
                                        <asp:ListItem Value="1" Selected="True" Text="Not Specified"></asp:ListItem>
                                        <asp:ListItem Value="2" Text="12hrs"></asp:ListItem>
                                        <asp:ListItem Value="3" Text="24hrs"></asp:ListItem>
                                        <asp:ListItem Value="4" Text="48hrs"></asp:ListItem>
                                        <asp:ListItem Value="5" Text="Weekly"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                                <div style="width: 17%; float: left">
                                    <input type="checkbox" id="chkIncludeShipping" runat="server" style="margin-top: 11px;" />
                                    <span>Include shipping in overall pricing comparison.</span>
                                </div>
                                <div style="width: 17%; float: left">
                                    <input type="checkbox" id="chkFloorLimitNotification" runat="server" style="margin-top: 11px;" />
                                    <span>Notify me with email when item's floor limit reached.</span>
                                </div>
                                <div class="clear">
                                </div>
                            </div>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnUpdateSettings" EventName="Click" />
                        </Triggers>
                    </asp:UpdatePanel>
                    <div class='searchContainer22' id="divPricingMainSearch">
                        <div class="componentheading">
                            <h3>
                            Search
                            <input type="button" id="btnReloadListing" class="button" value="Reload Listing" style="float: right; margin-right: 2px" />
                            <input type="button" id="btnDefaults" class="button" style="float: right; margin-right: 2px;" value="Default Setup" />
                            <input type="button" id="btnDeactivateAll" class="button" value="Deactivate All"  style="float:right; margin-right:2px;" />
                            <asp:Button runat="server" ID="btnExport" class="button" Text="Export All" Style="float: right;margin-right: 2px;" OnClick="btnExport_Click"></asp:Button>
                            
                            
                            </h3>
                            <br />
                        </div>
                        <div style="width: 140px; float: left">
                            <label>
                                eBay Account:</label>
                            <asp:DropDownList ID="ddlEbayAccount" runat="server" ClientIDMode="Static" Style="width: 135px">
                            </asp:DropDownList>
                        </div>
                        <div style="width: 246px; float: left">
                            <label>
                                Category:</label>
                            <asp:DropDownList ID="ddlEbayCategory" runat="server" ClientIDMode="Static" Style="width: 240px">
                            </asp:DropDownList>
                        </div>
                        <div style="width: 140px; float: left">
                            <label>
                                Country:</label>
                            <asp:DropDownList ID="ddlCountrySearch" runat="server" ClientIDMode="Static" Style="width: 135px;">
                            </asp:DropDownList>
                        </div>
                        <div style="width: 140px; float: left">
                            <label>
                                Search Text:</label>
                            <input type="text" id="txtSearchValue" class="inputbox" style="width: 135px" />
                        </div>
                        <div style="width: 250px; float: left">
                            <label style="float:left">
                                Search In:</label>
                            <select id="ddlSearchFeild" name="ddlSearchFeild" style="width: 90px; margin-left: 5px;float: left;clear:left;margin-right:5px;">
                                <option value="Item_Name">In Title</option>
                                <option value="Item_ID">In Item ID</option>
                            </select>
                            <input type="button" id="btnMainSearch" class="button" value="Search"  style="float:left"/>
                            
                        </div>
                        <div class="clear">
                        </div>
                    </div>
                    <div style="clear: both">
                    </div>
                    <div id="divPricingItems">
                        <table id="tblPricingItems" class="list tablesorter" cellpadding="0" cellspacing="0"
                            style="width: 100%; font-size: smaller;">
                            <thead>
                                <tr>
                                    <th class="nosort" style="width: 256px;">
                                    </th>
                                    <th style="width: 30px;">
                                        Rank
                                    </th>
                                    <th style="width: 110px;">
                                        ItemID
                                    </th>
                                    <th>
                                        Title
                                    </th>
                                    <th>
                                        Category
                                    </th>
                                    <th style="width: 90px;">
                                        Current Price<br />
                                        Bin Price
                                    </th>
                                    <th style="width: 100px;">
                                        Start<br />
                                        End
                                    </th>
                                    <th class="" style="width: 150px;">
                                        Automate
                                    </th>
                                    <th class="" style="width: 100px;">
                                        Floor<br />
                                        Ceiling
                                    </th>
                                    <th class="" style="width: 120px;">
                                        Algorithm
                                    </th>
                                    <th class="" style="width: 150px;">
                                        Search Query
                                    </th>
                                    <th class="nosort" style="width: 50px;">
                                    </th>
                                </tr>
                            </thead>
                            <tbody>
                            </tbody>
                        </table>
                        
                    </div>
                    <select id="ddlAlgorithm1" class="algorithmMaster">
                        <option value="" selected="selected">Select Algorithm</option>
                        <option value="1">Lowest</option>
                        <option value="2">Average</option>
                    </select>
                </div>
                <div class="clear">
                </div>
                <div class="divFiltersModal validationGroup" style="display: none">
                     <div class="divCurrentItemDetails">
                                    <a id="hrefitemURL" data-column="Item_View_URL" class="anchor" href='' target='_blank'>
                                        <img id="itemImageURL" data-column="Picture_URL" src='' alt='Ebay' class='searchImg image' /></a>
                                    <span class='resultText'><span class='searchTitle span' data-column="Item_Name"></span>
                                        <a id="hrefRefresh" href="#">
                                            <img src='/images/refresh.png' alt='Refresh' />
                                        </a>
                                        <br />
                                        <span class='searchSellerID'>Rank:</span> <span class='searchSellerScore span' data-column="Item_Rank">
                                        </span><span class='searchSellerID'>Current Price:</span> <span data-column="Current_Price"
                                            class="searchSellerScore span"></span><span class="searchSellerScore span" data-column="Currency">
                                            </span><a id="linkPricingHistory" target="_blank" href="/pages/seller/pricinghistory.aspx?itemcode={itemcode}"
                                                style="float: right;">Pricing History </a></span>
                                </div>
                    <div id="tabs">
                        <ul>
                            <li><a href="#tabs-pa">Price Automation</a></li>
                            <li><a href="#tabs-ta">Title Automation</a></li>
                        </ul>
                        <div id="tabs-pa">
                            <div class="divFilterModalBody">
                               
                                <div class="divPricingInputs">
                                    <input type="hidden" id="hfItemID" data-column="Item_ID" data-filter="Item_ID" />
                                    <input type="hidden" id="hfItemCode" data-column="Item_Code" />
                                    <input type="hidden" id="hfUserAccountCode" data-column="User_Account_Code" data-filter="User_Account_Code" />
                                    <div class="errorbox" style="display: none">
                                        <div class="errorboxinnerbox">
                                            <strong>There was a problem with the details you entered, please see below for more
                                                information:</strong>
                                            <ul id="ulErrors">
                                            </ul>
                                        </div>
                                    </div>
                                    <div class="divAutomation">
                                        <div style="float: right; margin-right: 11px;">
                                            <p>
                                                <label for="ddlSavedFile">
                                                    Saved Settings</label>
                                                <asp:DropDownList ID="ddlSavedFile" runat="server" ClientIDMode="Static">
                                                </asp:DropDownList>
                                            </p>
                                            <p>
                                                <a href="#" id="btnClearForm" style="float: right; margin-left: 15px;">Clear Form</a>
                                                <a href="#" id="btnUseDefaults" style="float: right">Apply Defaults</a>
                                            </p>
                                        </div>
                                        <div class="clear">
                                        </div>
                                        <h4>
                                            Pricing</h4>
                                        <div class="columns2">
                                            <p>
                                                <label for="chkAutomate">
                                                    Automate</label>
                                                <input type="checkbox" id="chkAutomate" class="chkAutomate" data-column="Is_Automated"
                                                    data-file="Is_Automated" />
                                            </p>
                                            <p>
                                                <label for="txtFloorPrice">
                                                    Floor / Ceiling Price</label>
                                                <input type="text" id="txtFloorPrice" name="txtFloorPrice" title="Floor Price" class='txtFloorPrice numeric inputbox'
                                                    data-column="Floor_Price" data-file="Floor_Price" />
                                                <input type="text" id="txtCeilingPrice" name="txtCeilingPrice" title="Ceiling Price"
                                                    class='txtCeilingPrice numeric inputbox' data-column="Ceiling_Price" data-file="Ceiling_Price" />
                                            </p>
                                        </div>
                                        <div class="columns2">
                                            <p>
                                                <label for="chkAutomate">
                                                    Algorithm</label>
                                                <select id="ddlAlgorithm" name="ddlAlgorithm" class="ddlAlgorithm select-normal"
                                                    data-column="Algo" data-file="Algo">
                                                    <option value="" selected="selected">Select Algorithm</option>
                                                    <option value="3">Match Lowest</option>
                                                    <option value="1">Lowest</option>
                                                    <option value="2">Average</option>
                                                </select>
                                            </p>
                                            <p>
                                                <label for="chkAutomate">
                                                    Less Than Lowest Price</label>
                                                <select id="txtLessToLowest" name="txtLessToLowest" class="txtLessToLowest select-normal"
                                                    data-column="Less_To_Lowest_Price" data-file="Less_To_Lowest_Price">
                                                    <option value="" selected="selected">Select</option>
                                                    <option value="0.01">0.01</option>
                                                    <option value="0.05">0.05</option>
                                                    <option value="0.10">0.10</option>
                                                    <option value="0.20">0.20</option>
                                                    <option value="0.30">0.30</option>
                                                    <option value="0.40">0.40</option>
                                                    <option value="0.50">0.50</option>
                                                    <option value="0.60">0.60</option>
                                                    <option value="0.70">0.70</option>
                                                    <option value="0.80">0.80</option>
                                                    <option value="0.90">0.90</option>
                                                    <option value="1.00">1.00</option>
                                                </select>
                                                <%--<input type="text" id="txtLessToLowest" name="txtLessToLowest" title="Less To Lowest Price(i.e: 0.01% of lowest price) "
                                            class='txtLessToLowest numeric inputbox' data-column="Less_To_Lowest_Price" />--%>
                                            </p>
                                        </div>
                                        <div class="clear">
                                        </div>
                                    </div>
                                    <div class="divFilters">
                                        <h4>
                                            Filters</h4>
                                        <div class="columns2">
                                            <p>
                                                <input type="hidden" id="hfItemRank" data-column="Item_Rank" />
                                                <label for="txtKeywords">
                                                    Search Query</label>
                                                <input type="text" id="txtKeywords" name="txtKeywords" title="Search Query" class='txtKeywords required inputbox'
                                                    data-column="Keywords" data-filter="Keywords" data-file="Keywords" />
                                            </p>
                                            <p>
                                                <label for="txtIgnoreWords">
                                                    Ignore Words</label>
                                                <input type="text" id="txtIgnoreWords" name="txtIgnoreWords" title="Search Query"
                                                    class='txtIgnoreWords inputbox' data-column="Ignore_Words" data-filter="Ignore_Words"
                                                    data-file="Ignore_Words" /><br />
                                                <label>
                                                </label>
                                                <span style="font-size: x-small">(Comma separated)</span>
                                            </p>
                                            <p>
                                                <label for="txtMinFeedBack">
                                                    Min/Max Feedback</label>
                                                <input type="text" id="txtMinFeedBack" name="txtMinFeedBack" title="Min. Feedback"
                                                    class='txtMinFeedBack numeric inputbox' data-column="Minimum_Feedback" data-filter="Minimum_Feedback"
                                                    data-file="Minimum_Feedback" />
                                                <input type="text" id="txtMaxFeedBack" name="txtMaxFeedBack" title="Max. Feedback"
                                                    class='txtMaxFeedBack numeric inputbox' data-column="Maximum_Feedback" data-filter="Maximum_Feedback"
                                                    data-file="Maximum_Feedback" />
                                            </p>
                                            <p>
                                                <label for="txtMinPrice">
                                                    Min/Max Price</label>
                                                <input type="text" id="txtMinPrice" name="txtMinPrice" title="Min. Price" class='txtMinPrice numeric inputbox'
                                                    data-column="Minimum_Price" data-filter="Minimum_Price" data-file="Minimum_Price" />
                                                <input type="text" name="txtMinPrice" title="Max. Price" class='txtMinPrice numeric inputbox'
                                                    data-column="Maximum_Price" data-filter="Maximum_Price" data-file="Maximum_Price" />
                                            </p>
                                            <p>
                                                <label for="txtMinQuantity">
                                                    Min/Max Quantity</label>
                                                <input type="text" id="txtMinQuantity" name="txtMinQuantity" title="Min. Quantity"
                                                    class='txtMinQuantity numeric inputbox' data-column="Minimum_Quantity" data-filter="Maximum_Price"
                                                    data-file="Maximum_Price" />
                                                <input type="text" id="txtMaxQuantity" name="txtMaxQuantity" title="Max. Quantity"
                                                    class='txtMaxQuantity numeric inputbox' data-column="Maximum_Quantity" data-filter="Maximum_Quantity"
                                                    data-file="Maximum_Quantity" />
                                            </p>
                                            <p>
                                                <label for="txtIncludeSellers">
                                                    Inclued / Exclude Sellers</label>
                                                <input type="text" id="txtIncludeSellers" name="txtIncludeSellers" title="Include Sellers"
                                                    class='txtIncludeSellers inputbox' data-column="Inclued_Sellers" data-filter="Inclued_Sellers"
                                                    data-file="Inclued_Sellers" />
                                                <input type="text" id="txtExcludeSellers" name="txtExcludeSellers" title="Exclude Sellers"
                                                    class='txtExcludeSellers inputbox' data-column="Exclude_Sellers" data-filter="Exclude_Sellers"
                                                    data-file="Exclude_Sellers" />
                                                <label>
                                                </label>
                                                <span style="font-size: x-small">(Comma separated)</span>
                                            </p>
                                        </div>
                                        <div class="columns2">
                                            <p>
                                                <label for="txtMaxHandlingTime">
                                                    Max Handling Time</label>
                                                <input type="text" id="txtMaxHandlingTime" name="txtMaxHandlingTime" title="Max. Handling Time"
                                                    class='txtMaxHandlingTime numeric inputbox' data-column="Maximum_Handling_Time"
                                                    data-filter="Maximum_Handling_Time" data-file="Maximum_Handling_Time" />
                                            </p>
                                            <p>
                                                <label for="ddlCondition">
                                                    Condition</label>
                                                <select id="ddlCondtion" name="ddlCondition" multiple="" title="Condition" class="ddlCondition select-select2"
                                                    style="width: 200px" data-column="Include_Condtion_Codes" data-filter="Include_Condtion_Codes"
                                                    data-file="Include_Condtion_Codes">
                                                    <option value="1000">Brand New</option>
                                                    <option value="1500">New: Never Used</option>
                                                    <option value="2000">Manufacturer refurbished</option>
                                                    <option value="2500">Seller refurbished</option>
                                                    <option value="3000">Used</option>
                                                    <option value="7000">For parts or not working</option>
                                                    <option value="-10">Not Specified</option>
                                                </select>
                                            </p>
                                            <p>
                                                <label for="chkCategory">
                                                    Exclude Category</label>
                                                <%--<input type="checkbox" id="chkCategory" class="chkCategory" />--%>
                                                <select id="ddlCategory" name="ddlCategory" multiple="" title="Exclude category"
                                                    class="ddlCategory select-select2" style="width: 200px" data-column="Exclude_Category_Codes"
                                                    data-filter="Exclude_Category_Codes" data-file="Exclude_Category_Codes">
                                                    <option value="14308">Alcohol &amp; Food</option>
                                                    <option value="20081">Antiques</option>
                                                    <option value="550">Art</option>
                                                    <option value="2984">Baby</option>
                                                    <option value="267">Books, Magazines</option>
                                                    <option value="170638">Business</option>
                                                    <option value="625">Cameras</option>
                                                    <option value="9800">Cars, Bikes, Boats</option>
                                                    <option value="11450">Clothing, Shoes, Accessories</option>
                                                    <option value="11116">Coins</option>
                                                    <option value="1">Collectables</option>
                                                    <option value="58058">Computers</option>
                                                    <option value="14339">Crafts</option>
                                                    <option value="237">Dolls, Bears</option>
                                                    <option value="293">Electronics</option>
                                                    <option value="172009">Gift Cards</option>
                                                    <option value="26395">Health &amp; Beauty</option>
                                                    <option value="20710">Home Appliances</option>
                                                    <option value="172176">Home Entertainment</option>
                                                    <option value="11700">Home &amp; Garden</option>
                                                    <option value="170769">Industrial</option>
                                                    <option value="281">Jewellery &amp; Watches</option>
                                                    <option value="11232">Movies</option>
                                                    <option value="11233">Music</option>
                                                    <option value="619">Musical Instruments</option>
                                                    <option value="15032">Phones</option>
                                                    <option value="870">Pottery, Glass</option>
                                                    <option value="316">Services</option>
                                                    <option value="888">Sporting Goods</option>
                                                    <option value="260">Stamps</option>
                                                    <option value="11730">Tickets, Travel</option>
                                                    <option value="220">Toys, Hobbies</option>
                                                    <option value="131090">Vehicle Parts &amp; Accessories</option>
                                                    <option value="1249">Video Games &amp; Consoles</option>
                                                    <option value="99">Lots More...</option>
                                                </select>
                                            </p>
                                            <p>
                                                <label for="ddlCountry">
                                                    Country</label>
                                                <asp:DropDownList ID="ddlCountry" ClientIDMode="Static" runat="server" data-column="LocatedIn"
                                                    data-filter="LocatedIn" data-file="LocatedIn">
                                                </asp:DropDownList>
                                                <input type="hidden" data-column="Country_Code" data-filter="Country_Code" />
                                            </p>
                                            <p>
                                                <label for="chkFixedPrice">
                                                    Buy It Now</label>
                                                <input type="checkbox" id="chkFixedPrice" class="chkFixedPrice" data-column="Is_Fixed_Price"
                                                    data-filter="Is_Fixed_Price" data-file="Is_Fixed_Price" />
                                                <label for="chkLocationAU" style="display: none">
                                                    Location AU</label>
                                                <input type="checkbox" id="chkLocationAU" class="chkLocationAU" data-column="Is_Location_AU"
                                                    data-filter="Is_Location_AU" data-file="Is_Location_AU" style="display: none" />
                                                <label for="chkAuctions">
                                                    Auctions w/ BIN</label>
                                                <input type="checkbox" id="chkAuctions" class="chkAuctions" data-column="Is_Auctions"
                                                    data-filter="Is_Auctions" data-file="Is_Auctions" />
                                            </p>
                                            <p>
                                                <label for="chkHideDuplicates">
                                                    Hide Duplicates</label>
                                                <input type="checkbox" id="chkHideDuplicates" class="chkHideDuplicates" data-column="Is_Hide_Duplicates"
                                                    data-filter="Is_Hide_Duplicates" data-file="Is_Hide_Duplicates" />
                                                <label for="chkRoundToNearest">
                                                    Round Down Nearest 0.10</label>
                                                <input type="checkbox" id="chkRoundToNearest" class="chkRoundToNearest" data-column="Is_Round_To_Nearest"
                                                    data-filter="Is_Round_To_Nearest" data-file="Is_Round_To_Nearest" />
                                            </p>
                                            <p>
                                                <label for="chkReturnsAccepted">
                                                    Returns Accepted</label>
                                                <input type="checkbox" id="chkReturnsAccepted" class="chkReturnsAccepted" data-column="Is_Returns_Accepted"
                                                    data-filter="Is_Returns_Accepted" data-file="Is_Returns_Accepted" />
                                                <label for="chkHideDuplicates">
                                                    Top Rated Only</label>
                                                <input type="checkbox" id="chkTopRatedOnly" class="chkTopRatedOnly" data-column="Is_Top_Rated_Only"
                                                    data-filter="Is_Top_Rated_Only" data-file="Is_Top_Rated_Only" />
                                            </p>
                                        </div>
                                        <div class="clear">
                                        </div>
                                    </div>
                                </div>
                                <div class="divSearchResults" style="display: none">
                                    <ul id="ulResults">
                                    </ul>
                                </div>
                            </div>
                            <div class="divFilterModalFooter">
                                <input type="button" id="btnSearch" value="Search" class="button" />
                                <input type="button" id="btnRevise" value="Revise" class="button" style="display: none" />
                                <input type="button" id="btnSaveFile" value="Activate and Save to File" class="button"
                                    style="float: right; margin-right: 24px" />
                                <input type="button" id="btnSave" value="Activate" class="button" style="float: right;
                                    margin-right: 24px" />
                            </div>
                        </div>
                        <div id="tabs-ta">
                          <%--  <div class="divCurrentItemDetails">
                                <a id="A1" data-column="Item_View_URL" class="anchor" href='' target='_blank'>
                                    <img id="Img1" data-column="Picture_URL" src='' alt='Ebay' class='searchImg image' /></a>
                                <span class='resultText'><span class='searchTitle span' data-column="Item_Name"></span>
                                    <a id="A2" href="#">
                                        <img src='/images/refresh.png' alt='Refresh' />
                                    </a>
                                    <br />
                                    <span class='searchSellerID'>Rank:</span> <span class='searchSellerScore span' data-column="Item_Rank">
                                    </span><span class='searchSellerID'>Current Price:</span> <span data-column="Current_Price"
                                        class="searchSellerScore span"></span><span class="searchSellerScore span" data-column="Currency">
                                        </span><a id="A3" target="_blank" href="/pages/seller/pricinghistory.aspx?itemcode={itemcode}"
                                            style="float: right;">Title History </a></span>
                            </div>--%>
                            <div id="divTopTitles">
                                <div class="columns2">
                                    <p>
                                        <label for="chkAutomate">
                                            Automate</label>
                                        <input type="checkbox" id="chkTitleAutomate" class="chkAutomate" data-column="Is_Title_Automated"
                                            data-file="Is_Title_Automated" />
                                    </p>
                                </div>
                                <div class="clear">
                                </div>
                                <h4>
                                    Top Ranking Titles - Please edit and save</h4>
                                <input type="button" id="btnLoadTitle" value="Load Titles" class="button" style="  margin-top: 0%;float: right;margin-right: 3%;" />
                                <div class="clear">
                                </div>
                                <div class="column2 divLoadTitle">
                                    <p>
                                        
                                        <label for="txtTitle1" style="margin-left: -150px">
                                            1</label>
                                        <img src="" alt="Ebay" class="searchImg" style="display:none" />
                                        <input type="text" id="txtTitle1" name="txtTitle1" title="Top Title" class='inputbox'
                                            style="width: 550px" onblur="CheckValues('txtTitle1',0);" onkeyup="CountCharacters('txtTitle1','spnCount1');" />
                                        <input type="hidden" value="" id="hdntitleId1" />
                                        <span id="spnCount1"></span><%--<a class="btnEditTitle" onclick="editTitle('#txtTitle1')" href="javascript:void(0)">Edit</a>--%>
                                        <img src="../../Images/reject.png" class="titlereject" style="width: 25px; display: none" />
                                        <img src="../../Images/accept.png" class="titleaccept" style="width: 25px; display: none" />
                                        <br />
                                        <span class="searchSellerID"></span>
                                        <img src="/images/topratedseller.gif" class="topRatedSeller" style="display:none" />
                                        <span class="searchTimeRemaining"></span>
                                        <span class="totalSales" style="display:none">Total Sales : <span id="spnTotalSales1" class="salesCount"></span></span>
                                    </p>
                                    <p>
                                        
                                        <label for="txtTitle2" style="margin-left: -150px">
                                            2</label>
                                        <img src="" alt="Ebay" class="searchImg" style="display:none" />
                                        <input type="text" id="txtTitle2" name="txtTitle2" title="Top Title" class='inputbox'
                                            style="width: 550px" onblur="CheckValues('txtTitle2',1);" onkeyup="CountCharacters('txtTitle2','spnCount2');" />
                                        <input type="hidden" value="" id="hdntitleId2" />
                                        <span id="spnCount2"></span><%--<a class="btnEditTitle" onclick="editTitle('#txtTitle2')" href="javascript:void(0)">Edit</a>--%>
                                        <img src="../../Images/reject.png" class="titlereject" style="width: 25px; display: none" />
                                        <img src="../../Images/accept.png" class="titleaccept" style="width: 25px; display: none" />
                                        <br />
                                        <span class="searchSellerID"></span>
                                        <img src="/images/topratedseller.gif" class="topRatedSeller" style="display:none" />
                                        <span class="searchTimeRemaining"></span>
                                        <span class="totalSales" style="display:none">Total Sales : <span id="spnTotalSales2" class="salesCount"></span></span>
                                    </p>
                                    <p>
                                        <label for="txtTitle3" style="margin-left: -150px">
                                            3</label>
                                        <img src="" alt="Ebay" class="searchImg" style="display:none" />
                                        <input type="text" id="txtTitle3" name="txtTitle3" title="Top Title" class='inputbox'
                                            style="width: 550px" onblur="CheckValues('txtTitle3',2);" onkeyup="CountCharacters('txtTitle3','spnCount3');" />
                                        <input type="hidden" value="" id="hdntitleId3" />
                                        <span id="spnCount3"></span><%--<a class="btnEditTitle" onclick="editTitle('#txtTitle3')" href="javascript:void(0)">Edit</a>--%>
                                        <img src="../../Images/reject.png" class="titlereject" style="width: 25px; display: none" />
                                        <img src="../../Images/accept.png" class="titleaccept" style="width: 25px; display: none" />
                                        <br />
                                        <span class="searchSellerID"></span>
                                        <img src="/images/topratedseller.gif" class="topRatedSeller" style="display:none" />
                                        <span class="searchTimeRemaining"></span>
                                        <span class="totalSales" style="display:none">Total Sales : <span id="spnTotalSales3" class="salesCount"></span></span>
                                    </p>
                                    <p>
                                        <label for="txtTitle4" style="margin-left: -150px">
                                            4</label>
                                        <img src="" alt="Ebay" class="searchImg" style="display:none" />
                                        <input type="text" id="txtTitle4" name="txtTitle4" title="Top Title" class='inputbox'
                                            style="width: 550px" onblur="CheckValues('txtTitle4',3);" onkeyup="CountCharacters('txtTitle4','spnCount4');" />
                                        <input type="hidden" value="" id="hdntitleId4" />
                                        <span id="spnCount4"></span><%--<a class="btnEditTitle" onclick="editTitle('#txtTitle4')" href="javascript:void(0)">Edit</a>--%>
                                        <img src="../../Images/reject.png" class="titlereject" style="width: 25px; display: none" />
                                        <img src="../../Images/accept.png" class="titleaccept" style="width: 25px; display: none" />
                                        <br />
                                        <span class="searchSellerID"></span>
                                        <img src="/images/topratedseller.gif" class="topRatedSeller" style="display:none" />
                                        <span class="searchTimeRemaining"></span>
                                        <span class="totalSales" style="display:none">Total Sales : <span id="spnTotalSales4" class="salesCount"></span></span>
                                    </p>
                                    <p>
                                        <label for="txtTitle5" style="margin-left: -150px">
                                            5</label>
                                        <img src="" alt="Ebay" class="searchImg" style="display:none" />
                                        <input type="text" id="txtTitle5" name="txtTitle5" title="Top Title" class='inputbox'
                                            style="width: 550px" onblur="CheckValues('txtTitle5',4);" onkeyup="CountCharacters('txtTitle5','spnCount5');" />
                                        <input type="hidden" value="" id="hdntitleId5" />
                                        <span id="spnCount5"></span><%--<a class="btnEditTitle" onclick="editTitle('#txtTitle5')" href="javascript:void(0)">Edit</a>--%>
                                        <img src="../../Images/reject.png" class="titlereject" style="width: 25px; display: none" />
                                        <img src="../../Images/accept.png" class="titleaccept" style="width: 25px; display: none" />
                                        <br />
                                        <span class="searchSellerID"></span>
                                        <img src="/images/topratedseller.gif" class="topRatedSeller" style="display:none" />
                                        <span class="totalSales" style="display:none">Total Sales : <span id="spnTotalSales5" class="salesCount"></span></span>
                                    </p>
                                </div>
                                <div class="column2">
                                    <p>
                                        Title Rotate by
                                        <select id="ddlRotate" style="width: 80px" data-column="Rotate_Order">
                                            <option value="1">1</option>
                                            <option value="2">2</option>
                                            <option value="3">3</option>
                                            <option value="4">4</option>
                                            <option value="5">5</option>
                                        </select>
                                        every
                                        <select id="ddlDays" style="width: 80px" data-column="Rotate_Days">
                                            <option value="1">1 day</option>
                                            <option value="2">2 days</option>
                                            <option value="3">3 days</option>
                                            <option value="4">4 days</option>
                                            <option value="5">5 days</option>
                                            <option value="7">7 days</option>
                                            <option value="10">10 days</option>
                                            <option value="14">14 days</option>
                                            <option value="21">21 days</option>
                                            <option value="30">30 days</option>
                                        </select>
                                        lock Title when sales =
                                        <select id="ddlSales" style="width: 80px" data-column="Rotate_Sales">
                                            <option value="1">1</option>
                                            <option value="2">2</option>
                                            <option value="3">3</option>
                                            <option value="4">4</option>
                                            <option value="5">5</option>
                                            <option value="10">10</option>
                                            <option value="15">15</option>
                                            <option value="25">25</option>
                                            <option value="35">35</option>
                                            <option value="50">50</option>
                                            <option value="75">75</option>
                                            <option value="100">100</option>
                                        </select>
                                    </p>
                                </div>
                                <div class="clear">
                                </div>
                            </div>
                            <div class="divPricingInputsTitle">
                                <div class="errorbox" style="display: none">
                                    <div class="errorboxinnerbox">
                                        <strong>There was a problem with the details you entered, please see below for more
                                            information:</strong>
                                        <ul id="ul1">
                                        </ul>
                                    </div>
                                </div>
                                
                            </div>
                            <div class="divSearchResultsTitle">
                                <ul id="ulResultTitle">
                                </ul>
                            </div>
                            <div class="divFilterModalFooter">
                                <input type="button" id="btnSearchTitle" value="Search" class="button" />
                                <input type="button" id="btnReviseTitle" value="Revise" class="button" style="display: none" />
                                <input type="button" id="btnCancelTitle" value="Cancel" class="button" style="float: right;
                                    margin-right: 24px" />
                                <input type="button" id="btnSaveTitle" value="Save" class="button" style="float: right;
                                    margin-right: 24px" />
                            </div>
                        </div>
                    </div>
                    <div class="clear">
                    </div>
                    <div class="divDefaultsModal validationGroup" style="display: none">
                        <div class="divFilterModalBody">
                            <div class="divPricingInputs">
                                <div class="divAutomation">
                                    <h4>
                                        Pricing</h4>
                                    <div class="columns2">
                                        <p>
                                            <label for="txtFloorPriceDefault">
                                                Floor / Ceiling Price</label>
                                            <input type="text" id="txtFloorPriceDefault" name="txtFloorPriceDefault" title="Floor Price"
                                                class='txtFloorPriceDefault numeric inputbox' data-default="Floor_Price" />
                                            <input type="text" id="txtCeilingPriceDefault" name="txtCeilingPrice" title="Ceiling Price"
                                                class='txtCeilingPriceDefault numeric inputbox' data-default="Ceiling_Price" />
                                        </p>
                                    </div>
                                    <div class="columns2">
                                        <p>
                                            <label for="ddlAlgorithmDefault">
                                                Algorithm</label>
                                            <select id="ddlAlgorithmDefault" name="ddlAlgorithmDefault" class="ddlAlgorithmDefault select-normal"
                                                data-default="Algo">
                                                <option value="" selected="selected">Select Algorithm</option>
                                                <option value="3">Match Lowest</option>
                                                <option value="1">Lowest</option>
                                                <option value="2">Average</option>
                                            </select>
                                        </p>
                                        <p>
                                            <label for="txtLessToLowestDefault">
                                                Less Than Lowest Price</label>
                                            <select id="txtLessToLowestDefault" name="txtLessToLowestDefault" class="txtLessToLowestDefault select-normal"
                                                data-file="Less_To_Lowest_Price">
                                                <option value="" selected="selected">Select</option>
                                                <option value="0.01">0.01</option>
                                                <option value="0.05">0.05</option>
                                                <option value="0.10">0.10</option>
                                                <option value="0.20">0.20</option>
                                                <option value="0.30">0.30</option>
                                                <option value="0.40">0.40</option>
                                                <option value="0.50">0.50</option>
                                                <option value="0.60">0.60</option>
                                                <option value="0.70">0.70</option>
                                                <option value="0.80">0.80</option>
                                                <option value="0.90">0.90</option>
                                                <option value="1.00">1.00</option>
                                            </select>
                                        </p>
                                    </div>
                                    <div class="clear">
                                    </div>
                                </div>
                                <div class="divFilters">
                                    <h4>
                                        Filters</h4>
                                    <div class="columns2">
                                        <p>
                                            <label for="txtMinFeedBackDefault">
                                                Min/Max Feedback</label>
                                            <input type="text" id="txtMinFeedBackDefault" name="txtMinFeedBackDefault" title="Min. Feedback"
                                                class='txtMinFeedBackDefault numeric inputbox' data-default="Minimum_Feedback" />
                                            <input type="text" id="txtMaxFeedBackDefault" name="txtMaxFeedBack" title="Max. Feedback"
                                                class='txtMaxFeedBackDefault numeric inputbox' data-default="Maximum_Feedback" />
                                        </p>
                                        <p>
                                            <label for="txtMinPriceDefault">
                                                Min/Max Price</label>
                                            <input type="text" id="txtMinPriceDefault" name="txtMinPriceDefault" title="Min. Price"
                                                class='txtMinPriceDefault numeric inputbox' data-default="Minimum_Price" />
                                            <input type="text" id="txtMaxPriceDefault" name="txtMaxPriceDefault" title="Max. Price" class='txtMaxPriceDefault numeric inputbox'
                                                data-default="Maximum_Price" />
                                        </p>
                                        <p>
                                            <label for="txtMinQuantityDefault">
                                                Min/Max Quantity</label>
                                            <input type="text" id="txtMinQuantityDefault" name="txtMinQuantity" title="Min. Quantity"
                                                class='txtMinQuantityDefault numeric inputbox' data-default="Minimum_Quantity" />

                                            <input type="text" id="txtMaxQuantityDefault" name="txtMaxQuantity" title="Max. Quantity"
                                                class='txtMaxQuantityDefault numeric inputbox' data-default="Maximum_Quantity" />
                                        </p>
                                        <p>
                                            <label for="txtMaxHandlingTimeDefault">
                                                Max Handling Time</label>
                                            <input type="text" id="txtMaxHandlingTimeDefault" name="txtMaxHandlingTimeDefault"
                                                title="Max. Handling Time" class='txtMaxHandlingTimeDefault numeric inputbox'
                                                data-default="Maximum_Handling_Time" />
                                        </p>
                                        <p>
                                            <label for="txtIncludeSellersDefault">
                                                Inclued / Exclude Sellers</label>
                                            <input type="text" id="txtIncludeSellersDefault" name="txtIncludeSellersDefault"
                                                title="Include Sellers" class='txtIncludeSellersDefault inputbox' data-file="Inclued_Sellers" />
                                            <input type="text" id="txtExcludeSellersDefault" name="txtExcludeSellersDefault"
                                                title="Exclude Sellers" class='txtExcludeSellersDefault inputbox' data-default="Exclude_Sellers" />
                                        </p>
                                    </div>
                                    <div class="columns2">
                                        <p>
                                            <label for="ddlConditionDefault">
                                                Condition</label>
                                            <select id="ddlConditionDefault" name="ddlConditionDefault" multiple="" title="Condition"
                                                class="ddlConditionDefault select-select2" style="width: 200px" data-default="Include_Condtion_Codes">
                                                <option value="1000">Brand New</option>
                                                <option value="1500">New: Never Used</option>
                                                <option value="2000">Manufacturer refurbished</option>
                                                <option value="2500">Seller refurbished</option>
                                                <option value="3000">Used</option>
                                                <option value="7000">For parts or not working</option>
                                                <option value="-10">Not Specified</option>
                                            </select>
                                        </p>
                                        <p>
                                            <label for="ddlCategoryDefault">
                                                Exclude Category</label>
                                            <select id="ddlCategoryDefault" name="ddlCategoryDefault" multiple="" title="Exclude category"
                                                class="ddlCategoryDefault select-select2" style="width: 200px" data-default="Exclude_Category_Codes">
                                                <option value="14308">Alcohol &amp; Food</option>
                                                <option value="20081">Antiques</option>
                                                <option value="550">Art</option>
                                                <option value="2984">Baby</option>
                                                <option value="267">Books, Magazines</option>
                                                <option value="170638">Business</option>
                                                <option value="625">Cameras</option>
                                                <option value="9800">Cars, Bikes, Boats</option>
                                                <option value="11450">Clothing, Shoes, Accessories</option>
                                                <option value="11116">Coins</option>
                                                <option value="1">Collectables</option>
                                                <option value="58058">Computers</option>
                                                <option value="14339">Crafts</option>
                                                <option value="237">Dolls, Bears</option>
                                                <option value="293">Electronics</option>
                                                <option value="172009">Gift Cards</option>
                                                <option value="26395">Health &amp; Beauty</option>
                                                <option value="20710">Home Appliances</option>
                                                <option value="172176">Home Entertainment</option>
                                                <option value="11700">Home &amp; Garden</option>
                                                <option value="170769">Industrial</option>
                                                <option value="281">Jewellery &amp; Watches</option>
                                                <option value="11232">Movies</option>
                                                <option value="11233">Music</option>
                                                <option value="619">Musical Instruments</option>
                                                <option value="15032">Phones</option>
                                                <option value="870">Pottery, Glass</option>
                                                <option value="316">Services</option>
                                                <option value="888">Sporting Goods</option>
                                                <option value="260">Stamps</option>
                                                <option value="11730">Tickets, Travel</option>
                                                <option value="220">Toys, Hobbies</option>
                                                <option value="131090">Vehicle Parts &amp; Accessories</option>
                                                <option value="1249">Video Games &amp; Consoles</option>
                                                <option value="99">Lots More...</option>
                                            </select>
                                        </p>
                                        <p>
                                            <label for="ddlCountryDefault">
                                                Country</label>
                                            <asp:DropDownList ID="ddlCountryDefault" ClientIDMode="Static" runat="server" data-default="LocatedIn">
                                            </asp:DropDownList>
                                        </p>
                                        <p>
                                            <label for="chkFixedPriceDefault">
                                                Buy It Now</label>
                                            <input type="checkbox" id="chkFixedPriceDefault" class="chkFixedPriceDefault" data-default="Is_Fixed_Price" />
                                            <label for="chkLocationAUDefault" style="display: none">
                                                Location AU</label>
                                            <input type="checkbox" id="chkLocationAUDefault" class="chkLocationAUDefault" data-default="Is_Location_AU"
                                                style="display: none" />
                                            <label for="chkAuctionsDefault">
                                                Auctions w/ BIN</label>
                                            <input type="checkbox" id="chkAuctionsDefault" class="chkAuctionsDefault" data-default="Is_Auctions" />
                                        </p>
                                        <p>
                                            <label for="chkHideDuplicatesDefault">
                                                Hide Duplicates</label>
                                            <input type="checkbox" id="chkHideDuplicatesDefault" class="chkHideDuplicatesDefault"
                                                data-default="Is_Hide_Duplicates" />
                                            <label for="chkRoundToNearestDefault">
                                                Round Down Nearest 0.10</label>
                                            <input type="checkbox" id="chkRoundToNearestDefault" class="chkRoundToNearestDefault"
                                                data-default="Is_Round_To_Nearest" />
                                        </p>
                                        <p>
                                            <label for="chkReturnsAcceptedDefault">
                                                Returns Accepted</label>
                                            <input type="checkbox" id="chkReturnsAcceptedDefault" class="chkReturnsAcceptedDefault"
                                                data-default="Is_Returns_Accepted" />
                                            <label for="chkTopRatedOnlyDefault">
                                                Top Rated Only</label>
                                            <input type="checkbox" id="chkTopRatedOnlyDefault" class="chkTopRatedOnlyDefault"
                                                data-default="Is_Top_Rated_Only" />
                                        </p>
                                    </div>
                                    <div class="clear">
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="divDefaultsModalFooter">
                            <input type="button" id="btnSaveDefaults" value="Save Default Setup" class="button"
                                style="float: right; margin-right: 24px" />
                        </div>
                    </div>
                    <div class="clear">
                    </div>
                </div>
            </div>
        </div>
        <br />
        <br />
        </div>
        <script src="/Scripts/lib/jquery-1.7.1.min.js" type="text/javascript"></script>
        <script src="/Scripts/lib/jquery-ui-1.8.22.custom.min.js" type="text/javascript"></script>
        <script src="/scripts/lib/colResizable-1.3.min.js" type="text/javascript"></script>
        <script src="/scripts/lib/jquery.tablesorter.js" type="text/javascript"></script>
        <script src="/scripts/lib/jquery.tmpl.min.js" type="text/javascript"></script>
        <script src="/scripts/lib/jquery.numeric.js" type="text/javascript"></script>
        <script src="/scripts/lib/jquery.qtip.min.js" type="text/javascript"></script>
        <script src="/scripts/lib/select2.min.js" type="text/javascript"></script>
        <script src="/scripts/lib/oneSimpleTablePaging-1.0.js" type="text/javascript"></script>
        <script src="/scripts/lib/jquery.switchButton.js" type="text/javascript"></script>
        <script src="/scripts/custom/Pricing.js" type="text/javascript"></script>




        <script type="text/javascript">
            $(document).ready(function () {
                InitializePricing();
            });
        </script>

        <script id="pricingItemTemplate" type="text/x-jQuery-tmpl">
        <tr title="{{if Is_Promo_Item == true }} Promo Item {{/if}}" class='pricingItem {{if Is_Promo_Item == true }} promoItem {{/if}} {{if Is_Automated == true }} pricingItemSelected {{/if}}'>
            
            <td>
                <a href='${Item_View_URL}' title="${Item_Name}" target="_blank"><img class="thumbnail" src="${Picture_URL}" width="90px" height="70px" /></a>
            </td>
            <td>
                {{if Is_Automated == true }}
                    <span class='spnItemRank'>${Item_Rank}</span>
                {{/if}}
            </td>
            <td>
                <span class='spnItemID'>${Item_ID}</span>
                <input type='hidden' class='hfItemCode' value='${Item_Code}' />
            </td>
            <td>
                <span class='spnItemName'>${Item_Name}</span>
            </td>
            <td>
                <span class='spnCategoryName'>${Item_Category_Name}</span>
            </td>
            <td>
                <span class='spnCurrentPrice'>${formatPrice(Current_Price)}</span><br/>
                <span class='spnBinPrice'>${formatPrice(BIN_Price)}</span>
            </td>
            <td>
                <span class='spnStartDate'>${GetDate(Start_Date,'MMM dd')}</span><br />
                 <span class='spnEndDate'>${GetDate(End_Date,'MMM dd')}</span>
            </td>
            <td>
                 <img src="{{if Is_Automated == true }} /images/tick-medium2.png {{else}} /images/close.png {{/if}}" width="26px" height="26px"/>
               
            </td>
            <td>
                <span class='spnFloorPrice'>${formatPrice(Floor_Price)}</span>
                <br />
                <span class='spnCeilingPrice'>${formatPrice(Ceiling_Price)}</span>
            </td>
            <td>
                <input type="hidden" class="hfAlgoCode" value="${Algo}">
                <span class='spnAlgo'>${Algo_Name}</span>
            </td>
            <td>
                <span class='spnKeywords'>${Keywords}</span>
            </td>
            <td>
                {{if QuantityAvailable == null || QuantityAvailable == 0 }} 

                            <span class="spnNoStock" title="No Stock">No Stock</span>
               
                    {{/if}}

                {{if  Is_Promo_Item != true }}

                        <a class="btnEdit" href="#" title="Edit pricing and filters"> <img src="/images/pencil.png" /> Edit </a>
                       
                {{else}}
                        <span class="spnPromo" title="Promo item (Price can not be revised)">Promo Item</spna>
               
                {{/if}}


              
            </td>
        </tr>
        </script>

        <script id="searchTemplate" type="text/x-jQuery-tmpl">
        <li class='resultItem'>
            <a href='${ViewURL}' target='_blank'><img src='${ImageURL}' alt='Ebay' class='searchImg' /></a>
            <span class='resultText'>
                <span class='searchTitle'>${Title}</span>
                <br>
                <span class='searchSellerID'>${SellerID}</span>
                (<span class='searchSellerScore'>${SellerScore}</span>)
                <img src='/images/topratedseller.gif' class='topRatedSeller' style='${GetSellerType(TopRatedSeller)}' />
                <span style='padding: 0px 0px 0px 10px;'>|</span>
                <span class='searchTimeRemaining'>Time: ${TimeRemaining}</span>
            </span>
            <div class='resultRight'>
                <span class='searchPrice'>Price: <span>${parseFloat(TotalCost).toFixed(2)}</span></span><br/>
                <span class='searchShippingCost'>Shipping: ${ShippingCost}</span><br/>
            </div>
            <input type="hidden" class='hfMyProduct' value='${IsMyProduct}' />
            <input type="hidden" class='hfItemID' value='${ItemID}' />
        </tr>
        </script>

        <script id="searchTemplateTitle" type="text/x-jQuery-tmpl">
        <li class='resultItem'>
            <a href='${ViewURL}' target='_blank'><img src='${ImageURL}' alt='Ebay' class='searchImg' /></a>
            <span class='resultText'>
                <span class='searchTitle'>${Title}</span>
                <br>
                <span class='searchSellerID'>${SellerID}</span>
                (<span class='searchSellerScore'>${SellerScore}</span>)
                <img src='/images/topratedseller.gif' class='topRatedSeller' style='${GetSellerType(TopRatedSeller)}' />
                <span style='padding: 0px 0px 0px 10px;'>|</span>
                <span class='searchTimeRemaining'>Time: ${TimeRemaining}</span>
            </span>
            <div class='resultRight'>
                <span class='searchPrice'>Price: <span>${parseFloat(TotalCost).toFixed(2)}</span></span><br/>
                <span class='searchShippingCost'>Shipping: ${ShippingCost}</span><br/>
            </div>
            <input type="hidden" class='hfMyProductTitle' value='${IsMyProduct}' />
            <input type="hidden" class='hfItemIDTitle' value='${ItemID}' />
        </tr>
        </script>
        <script id="selectTemplate" type="text/x-jQuery-tmpl">
        <option value="${File_Item_Code}">${Keywords}</option>
        </script>
        <%-- <script src="/scripts/lib/jquery.validate.js" type="text/javascript"></script>
    <script src="/scripts/custom/validate.js" type="text/javascript"></script>--%>
</asp:Content>
