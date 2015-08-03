<%@ Page Title="" Language="C#" MasterPageFile="~/master/Main.master" AutoEventWireup="true"
    CodeFile="Search.aspx.cs" Inherits="pages_seller_Search" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div>
        Search for Items
        <input type="text" style='width: 426px' id='txtSearch' />
        <select id="ddlSearchEbay">
            <option value="10">10 Items</option>
            <option value="15">15 Items</option>
            <option value="20">20 Items</option>
        </select>
        <input type="submit" id="btnSearch" value="Search" class='button3' /><a id='aAdvancedSearch'>Show
            Filters</a>
        <div id='divAdvancedSearchBox'>
            <div class='item'>
                Min Quantity:
                <input type="text" id="txtMinQuantity" value='2' /></div>
            <div class='item'>
                <input type="checkbox" id="chkNew" name='chkNew' checked="checked" /><label for='chkUsed'>New?</label></div>
            <div class='item'>
                Min Price:
                <input type="text" id="txtMinPrice" value='1' />AUD</div>
            <div class='clear'>
            </div>
        </div>
        <br />
        <ul id="ulResults">
        </ul>
        <div class='rightContainer'>
            <div id='divSearchSummary' class='summaryText'>
                <h2>
                    Summary</h2>
                <p>
                    You have <span class='summaryItems'></span>in the list, with price <span class='summaryYourPrice bold'>
                    </span>
                    <br />
                    The average price (excluding your items) is <span class='summaryAveragePrice bold'>
                    </span>
                    <br />
                    The highest price is <span class='summaryHighestPrice bold'></span> and lowest is
                    <span class='summaryLowestPrice bold'></span>
                </p>
            </div>
            <div class='summaryText searchNote'>
                Note: The price shown includes the shipping cost already. Shipping cost is shown
                for information.
            </div>
            <br />
            <div class='summaryText searchNote orange'>
                Note: The price is immediately updated on Ebay, however API takes a few seconds
                to update. So you may get old prices using this page if you search the same query
                quickly after updating prices.
            </div>
        </div>
        <div id="changePrice">
            <h3 id="hProductTitle">
            </h3>
            Change Price to
            <input type="text" id="txtNewPrice" />
            <input type="hidden" id="hfSelectedProductID" />
            <input type="submit" id="btnSavePrice" class="button3" value="Save" />
        </div>
    </div>
    <input type="hidden" id="hfTokenJSON" class="hfTokenJSON" runat="server" />
    <script src="/scripts/lib/jquery.tmpl.min.js" type="text/javascript"></script>
    <script src="/scripts/lib/jquery-ui-1.8.22.custom.min.js" type="text/javascript"></script>
    <script src="/scripts/custom/EbaySearch.js" type="text/javascript"></script>
    <script type="text/javascript">
        function pageLoad() {
            SetSearchTriggers();
        }

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
