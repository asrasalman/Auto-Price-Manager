<%@ Page Title="" Language="C#" MasterPageFile="~/master/Main.master" AutoEventWireup="true"
    CodeFile="Pricing.aspx.cs" Inherits="pages_seller_Pricing" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="/css/styles.css" rel="stylesheet" />
    <link href="/css/select2.css" rel="stylesheet" type="text/css" />
    <link href="/css/jquery.switchButton.css" rel="stylesheet" type="text/css" />
    <link href="/css/jquery.qtip.min.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <input type="hidden" id="hfTokenJSON" runat="server" class="hfTokenJSON" runat="server" />
    <div class="downloadFile">
        <h1>
            Pricing Adjustment Automation</h1>
        <p>
            You can use the following tool to enable automatic pricing adjustments on your selected
            products, using a few algorithms to choose from. You need to see the ceiling and
            floor values for each enabled product, so that our automated system does not go
            beyond your decided limits
        </p>
        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
            <ContentTemplate>
                <div class='searchContainer22'>
                    <h5 style="margin-left: 20px">
                        Automate Pricing Settings</h5>
                    <div style="width:25%">
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
                    <div style="width:25%">
                        Time Delay:
                        <asp:DropDownList ID="ddlTimeDelay" runat="server" ClientIDMode="Static" Style="width: 150px">
                            <asp:ListItem Value="1" Selected="True" Text="Not Specified"></asp:ListItem>
                            <asp:ListItem Value="2" Text="12hrs"></asp:ListItem>
                            <asp:ListItem Value="3" Text="24hrs"></asp:ListItem>
                            <asp:ListItem Value="4" Text="48hrs"></asp:ListItem>
                            <asp:ListItem Value="5" Text="Weekly"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div style="width:15%">
                        <input type="checkbox" id="chkIncludeShipping" runat="server" style="margin-top: 11px;" />
                        <span>Include shipping in overall pricing comparison.</span>
                    </div>
                    <div style="width:15%">
                        <input type="checkbox" id="chkFloorLimitNotification" runat="server" style="margin-top: 11px;" />
                        <span>Notify me with email when item's floor limit reached.</span>
                        
                    </div>
                    <asp:Button ID="btnUpdateSettings" runat="server" Text="Update" CssClass="button3"
                        Style="float: right" OnClick="btnUpdateSettings_Click" />
                    
                    
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        <div class='searchContainer22'>
            <h5 style="margin-left: 20px">
                Search</h5>
            <div>
                eBay Account:
                <asp:DropDownList ID="ddlEbayAccount" runat="server" ClientIDMode="Static" Style="width: 150px">
                </asp:DropDownList>
            </div>
            <div>
                Category:
                <asp:DropDownList ID="ddlEbayCategory" runat="server" ClientIDMode="Static" Style="width: 150px">
                </asp:DropDownList>
            </div>
            <div>
                Search:
                <input type="text" id="txtSearchValue" style="width: 150px" />
                <select id="ddlSearchFeild" name="ddlSearchFeild" style="width: 150px; margin-left: 5px;">
                    <option value="Item_Name">In Title</option>
                    <option value="Item_ID">In Item ID</option>
                </select>
            </div>
            <div>
                <input type="button" id="btnMainSearch" class="button3" value="Search" />
                <input type="button" id="btnReloadListing" class="button3" value="Reload Listing" />
            </div>
        </div>
        <div style="clear: both">
        </div>
        <div id="divPricingItems">
            <%--<input type="button" id="btnSavePricingDetails" class="button3 causesValidation"
                value="Save Details" />--%>
            <table id="tblPricingItems" class="list tablesorter" cellpadding="0" cellspacing="0"
                style="width: 100%">
                <thead>
                    <tr>
                        <th class="nosort" style="width: 110px;">
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
                        <th class="nosort" style="width: 86px; text-align: center;">
                            Automate
                        </th>
                        <th class="nosort" style="width: 100px;">
                            Floor<br />
                            Ceiling
                        </th>
                        <th class="nosort" style="width: 120px;">
                            Algorithm
                        </th>
                        <th class="nosort" style="width: 150px;">
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
        <div class="divFilterModalBody">
            <div class="divCurrentItemDetails">
                <a id="hrefitemURL" data-column="Item_View_URL" class="anchor" href='' target='_blank'>
                    <img id="itemImageURL" data-column="Picture_URL" src='' alt='Ebay' class='searchImg image' /></a>
                <span class='resultText'><span class='searchTitle span' data-column="Item_Name"></span>
                    <br />
                    <span class='searchSellerID'>Rank:</span> <span class='searchSellerScore span' data-column="Item_Rank">
                    </span><span class='searchSellerID'>Current Price:</span> <span data-column="Current_Price"
                        class="searchSellerScore span"></span></span>
            </div>
            <div class="divPricingInputs">
                <input type="hidden" id="hfItemID" data-column="Item_ID" data-filter="Item_ID" />
                <input type="hidden" id="hfItemCode" data-column="Item_Code" />
                <div class="errorbox" style="display: none">
                    <div class="errorboxinnerbox">
                        <strong>There was a problem with the details you entered, please see below for more
                            information:</strong>
                        <ul id="ulErrors">
                        </ul>
                    </div>
                </div>
                <div class="divAutomation">
                    <h4>
                        Pricing</h4>
                    <div class="columns2">
                        <p>
                            <label for="chkAutomate">
                                Automate</label>
                            <input type="checkbox" id="chkAutomate" class="chkAutomate" data-column="Is_Automated" />
                        </p>
                        <p>
                            <label for="txtFloorPrice">
                                Floor / Ceiling Price</label>
                            <input type="text" id="txtFloorPrice" name="txtFloorPrice" title="Floor Price" class='txtFloorPrice numeric'
                                data-column="Floor_Price" />
                            <input type="text" id="txtCeilingPrice" name="txtCeilingPrice" title="Ceiling Price"
                                class='txtCeilingPrice numeric' data-column="Ceiling_Price" />
                        </p>
                    </div>
                    <div class="columns2">
                        <p>
                            <label for="chkAutomate">
                                Algorithm</label>
                            <select id="ddlAlgorithm" name="ddlAlgorithm" class="ddlAlgorithm select-normal"
                                data-column="Algo">
                                <option value="" selected="selected">Select Algorithm</option>
                                <option value="1">Lowest</option>
                                <option value="2">Average</option>
                            </select>
                        </p>
                        <p>
                            <label for="chkAutomate">
                                Less To Lowest Price</label>
                            <input type="text" id="txtLessToLowest" name="txtLessToLowest" title="Less To Lowest Price(i.e: 0.01% of lowest price) "
                                class='txtLessToLowest numeric' data-column="Less_To_Lowest_Price" />
                            %
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
                            <input type="text" id="txtKeywords" name="txtKeywords" title="Search Query" class='txtKeywords required'
                                data-column="Keywords" data-filter="Keywords" />
                        </p>
                        <p>
                            <label for="txtIgnoreWords">
                                Ignore Words</label>
                            <input type="text" id="txtIgnoreWords" name="txtIgnoreWords" title="Search Query" class='txtKeywords required'
                                data-column="Ignore_Words" data-filter="Ignore_Words" /><br />
                            <label>
                            </label>    
                            <span style="font-size:x-small">(Comma sepreated)</span>
                            
                        </p>
                        <p>
                            <label for="txtMinFeedBack">
                                Min/Max Feedback</label>
                            <input type="text" id="txtMinFeedBack" name="txtMinFeedBack" title="Min. Feedback"
                                class='txtMinFeedBack numeric' data-column="Minimum_Feedback" data-filter="Minimum_Feedback" />
                            <input type="text" id="txtMaxFeedBack" name="txtMaxFeedBack" title="Max. Feedback"
                                class='txtMaxFeedBack numeric' data-column="Maximum_Feedback" data-filter="Maximum_Feedback" />
                        </p>
                        <p>
                            <label for="txtMinPrice">
                                Min/Max Price</label>
                            <input type="text" id="txtMinPrice" name="txtMinPrice" title="Min. Price" class='txtMinPrice numeric'
                                data-column="Minimum_Price" data-filter="Minimum_Price" />
                            <input type="text" name="txtMinPrice" title="Max. Price" class='txtMinPrice numeric'
                                data-column="Maximum_Price" data-filter="Maximum_Price" />
                        </p>
                        <p>
                            <label for="txtMinQuantity">
                                Min/Max Quantity</label>
                            <input type="text" id="txtMinQuantity" name="txtMinQuantity" title="Min. Quantity"
                                class='txtMinQuantity numeric' data-column="Minimum_Quantity" data-filter="Maximum_Price" />
                            <input type="text" id="txtMaxQuantity" name="txtMaxQuantity" title="Max. Quantity"
                                class='txtMaxQuantity numeric' data-column="Maximum_Quantity" data-filter="Maximum_Quantity" />
                        </p>
                        <p>
                            <label for="txtMaxHandlingTime">
                                Max Handling Time</label>
                            <input type="text" id="txtMaxHandlingTime" name="txtMaxHandlingTime" title="Max. Handling Time"
                                class='txtMaxHandlingTime numeric' data-column="Maximum_Handling_Time" data-filter="Maximum_Handling_Time" />
                        </p>
                        <p>
                            <label for="txtIncludeSellers">
                                Inclued / Exclude Sellers</label>
                            <input type="text" id="txtIncludeSellers" name="txtIncludeSellers" title="Include Sellers"
                                class='txtIncludeSellers' data-column="Inclued_Sellers" data-filter="Inclued_Sellers" />
                            <input type="text" id="txtExcludeSellers" name="txtExcludeSellers" title="Exclude Sellers"
                                class='txtExcludeSellers' data-column="Exclude_Sellers" data-filter="Exclude_Sellers" />
                        </p>
                        <p>
                            <label for="chkFixedPrice">
                                Buy It Now</label>
                            <input type="checkbox" id="chkFixedPrice" class="chkFixedPrice" data-column="Is_Fixed_Price"
                                data-filter="Is_Fixed_Price" />
                            <label for="chkLocationAU">
                                Location AU</label>
                            <input type="checkbox" id="chkLocationAU" class="chkLocationAU" data-column="Is_Location_AU"
                                data-filter="Is_Location_AU" />
                        </p>
                        <p>
                            <label for="chkAuctions">
                                Auctions w/ BIN</label>
                            <input type="checkbox" id="chkAuctions" class="chkAuctions" data-column="Is_Auctions"
                                data-filter="Is_Auctions" />
                            <label for="chkHideDuplicates">
                                Hide Duplicates</label>
                            <input type="checkbox" id="chkHideDuplicates" class="chkHideDuplicates" data-column="Is_Hide_Duplicates"
                                data-filter="Is_Hide_Duplicates" />
                            <label for="chkRoundToNearest">
                                Round Down To Nearest 0.10</label>
                            <input type="checkbox" id="chkRoundToNearest" class="chkRoundToNearest" data-column="Is_Round_To_Nearest"
                                data-filter="Is_Round_To_Nearest" />
                        </p>
                        <p>
                            <label for="chkReturnsAccepted">
                                Returns Accepted</label>
                            <input type="checkbox" id="chkReturnsAccepted" class="chkReturnsAccepted" data-column="Is_Returns_Accepted"
                                data-filter="Is_Returns_Accepted" />
                            <label for="chkHideDuplicates">
                                Top Rated Only</label>
                            <input type="checkbox" id="chkTopRatedOnly" class="chkTopRatedOnly" data-column="Is_Top_Rated_Only"
                                data-filter="Is_Top_Rated_Only" />
                        </p>
                    </div>
                    <div class="columns2">
                        <p>
                            <label for="ddlCondition">
                                Condition</label>
                            <select id="ddlCondtion" name="ddlCondition" multiple="" title="Condition" class="ddlCondition select-select2"
                                style="width: 200px" data-column="Include_Condtion_Codes" data-filter="Include_Condtion_Codes">
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
                                data-filter="Exclude_Category_Codes">
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
            <input type="button" id="btnSearch" value="Search" class="button1" />
            <input type="button" id="btnRevise" value="Revise" class="button1" style="display: none" />
            <input type="button" id="btnSave" value="Save" class="button1" style="float: right;
                margin-right: 24px" />
        </div>
    </div>
    <div class="clear">
    </div>
    <script src="/Scripts/lib/jquery-ui-1.8.22.custom.min.js" type="text/javascript"></script>
    <script src="/scripts/lib/colResizable-1.3.min.js" type="text/javascript"></script>
    <script src="/scripts/lib/jquery.tablesorter.js" type="text/javascript"></script>
    <script src="/scripts/lib/jquery.tmpl.min.js" type="text/javascript"></script>
    <script src="/scripts/lib/jquery.numeric.js" type="text/javascript"></script>
    <script src="/scripts/lib/jquery.qtip.min.js" type="text/javascript"></script>
    <script src="/scripts/lib/jquery.validate.js" type="text/javascript"></script>
    <script src="/scripts/custom/validate.js" type="text/javascript"></script>
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
            <td style="text-align:center;">
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
                {{if Is_Promo_Item != true }} 
                    <a class="btnEdit" href="#" title="Edit pricing and filters">
                        <img src="/images/pencil.png" /> Edit
                    </a>
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
</asp:Content>
