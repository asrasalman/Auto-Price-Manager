using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using com.ebay.developer;
using System.Configuration;
using PriceManagerDAL;
using System.Data;


/// <summary>
/// Summary description for ServiceBL
/// </summary>
public class EbayServiceBL
{
    private string AppID = ConfigurationManager.AppSettings["AppID"];
    private string DevID = ConfigurationManager.AppSettings["DevID"];
    private string CertID = ConfigurationManager.AppSettings["CertID"];
    public Dictionary<int, string> UserTokens;
    string version = "813";
    public string siteID { get; set; }
    public int countryID { get; set; }
    public string countryShortID { get; set; }

    public EbayServiceBL(int userCode)
    {
        DataModelEntities context = new DataModelEntities();
        int ebayAccountCode = (int)Constant.Accounts.Ebay;

        List<UserAccount> userAccounts = context.UserAccounts.Where(u => u.User_Code == userCode && u.Is_Active == true && u.Account_Code == ebayAccountCode).ToList();
        User user = context.Users.FirstOrDefault(f => f.User_Code == userCode);
        if (userAccounts.Count > 0)
        {
            
            UserTokens = userAccounts.ToDictionary(u => u.User_Account_Code, u => u.Config_Value1);
            if (user != null)
            {
                siteID = user.Country1.Ebay_Site_ID.ToString();
                countryID = (int)user.Country_Code;
                countryShortID = user.Country1.Country_Abbr;
            }
            else
            {
                siteID = "15";
                countryID = 15;
                countryShortID = "AU";
            }
        }
        context = null;
    }

    public EbayServiceBL(int userCode, int countryCode)
    {
        DataModelEntities context = new DataModelEntities();
        int ebayAccountCode = (int)Constant.Accounts.Ebay;


        List<UserAccount> userAccounts = context.UserAccounts.Where(u => u.User_Code == userCode && u.Is_Active == true && u.Account_Code == ebayAccountCode).ToList();

        if (userAccounts.Count > 0)
        {
            var country = context.Countries.FirstOrDefault(w => w.Country_Code == countryCode);
            UserTokens = userAccounts.ToDictionary(u => u.User_Account_Code, u => u.Config_Value1);
            siteID = country != null ? country.Ebay_Site_ID.ToString() : "15";
            countryID = countryCode;
            countryShortID = country.Country_Abbr;
        }
        context = null;
    }

    public SellingManagerSoldOrderType[] GetPendingShipmentItems(string userToken)
    {
        string callname = "GetSellingManagerSoldListings";

        #region Initialise Needed Variables

        //Get the Server to use (Sandbox or Production)
        string serverUrl = ConfigurationManager.AppSettings["TradingService"];

        //SiteID = 0  (US) - UK = 3, Canada = 2, Australia = 15, ....
        //string siteID = "0";

        eBayAPIInterfaceService service = new eBayAPIInterfaceService();
        string requestURL = serverUrl + "?callname=" + callname + "&siteid=" + siteID
                            + "&appid=" + AppID + "&version=" + version + "&routing=default";
        service.Url = requestURL;

        // Set credentials
        service.RequesterCredentials = new CustomSecurityHeaderType();
        service.RequesterCredentials.Credentials = new UserIdPasswordType();
        service.RequesterCredentials.Credentials.AppId = AppID;
        service.RequesterCredentials.Credentials.DevId = DevID;
        service.RequesterCredentials.Credentials.AuthCert = CertID;
        service.RequesterCredentials.eBayAuthToken = userToken;
        #endregion

        GetSellingManagerSoldListingsRequestType request = new GetSellingManagerSoldListingsRequestType();
        request.Filter = new SellingManagerSoldListingsPropertyTypeCodeType[] { SellingManagerSoldListingsPropertyTypeCodeType.PaidNotShipped };
        request.Version = version;

        PaginationType pagination = new PaginationType();
        pagination.EntriesPerPage = 500;
        pagination.EntriesPerPageSpecified = true;

        request.Pagination = pagination;


        try
        {
            GetSellingManagerSoldListingsResponseType response = service.GetSellingManagerSoldListings(request);
            return response.SaleRecord;
        }
        catch (Exception ex)
        {
            if (ex.Message.ToLower().Contains("auth token"))
                throw new InvalidEbayCredentialsException();
            else
                throw ex;
        }
    }

    public SellingManagerSoldOrderType GetSaleRecordDetails(string itemID, string transactionID, string userToken)
    {
        string callname = "GetSellingManagerSaleRecord";

        #region Initialise Needed Variables

        //Get the Server to use (Sandbox or Production)
        string serverUrl = ConfigurationManager.AppSettings["TradingService"];

        //SiteID = 0  (US) - UK = 3, Canada = 2, Australia = 15, ....
        //string siteID = "0";

        eBayAPIInterfaceService service = new eBayAPIInterfaceService();
        string requestURL = serverUrl + "?callname=" + callname + "&siteid=" + siteID
                            + "&appid=" + AppID + "&version=" + version + "&routing=default";
        service.Url = requestURL;

        // Set credentials
        service.RequesterCredentials = new CustomSecurityHeaderType();
        service.RequesterCredentials.Credentials = new UserIdPasswordType();
        service.RequesterCredentials.Credentials.AppId = AppID;
        service.RequesterCredentials.Credentials.DevId = DevID;
        service.RequesterCredentials.Credentials.AuthCert = CertID;
        service.RequesterCredentials.eBayAuthToken = userToken;
        #endregion

        GetSellingManagerSaleRecordRequestType request = new GetSellingManagerSaleRecordRequestType();
        request.ItemID = itemID;
        request.TransactionID = transactionID;
        request.Version = version;
        GetSellingManagerSaleRecordResponseType response = service.GetSellingManagerSaleRecord(request);
        return response.SellingManagerSoldOrder;

    }

    public CompleteSaleResponseType UpdateShippingInfo(string itemID, string transactionID, string trackingNumber, string userToken)
    {
        string callname = "CompleteSale";

        #region Initialise Needed Variables

        //Get the Server to use (Sandbox or Production)
        string serverUrl = ConfigurationManager.AppSettings["TradingService"];

        //SiteID = 0  (US) - UK = 3, Canada = 2, Australia = 15, ....
        string siteID = "0";

        eBayAPIInterfaceService service = new eBayAPIInterfaceService();
        string requestURL = serverUrl + "?callname=" + callname + "&siteid=" + siteID
                            + "&appid=" + AppID + "&version=" + version + "&routing=default";
        service.Url = requestURL;

        // Set credentials
        service.RequesterCredentials = new CustomSecurityHeaderType();
        service.RequesterCredentials.Credentials = new UserIdPasswordType();
        service.RequesterCredentials.Credentials.AppId = AppID;
        service.RequesterCredentials.Credentials.DevId = DevID;
        service.RequesterCredentials.Credentials.AuthCert = CertID;
        service.RequesterCredentials.eBayAuthToken = userToken;
        #endregion

        CompleteSaleRequestType request = new CompleteSaleRequestType();
        request.WarningLevel = WarningLevelCodeType.High;
        request.ItemID = itemID;
        request.TransactionID = transactionID;

        ShipmentType shipment = new ShipmentType();
        ShipmentTrackingDetailsType shipmentDetails = new ShipmentTrackingDetailsType();
        shipmentDetails.ShipmentTrackingNumber = trackingNumber;
        shipmentDetails.ShippingCarrierUsed = ConfigurationManager.AppSettings["ShippingCarrier"];
        shipment.ShipmentTrackingDetails = new ShipmentTrackingDetailsType[] { shipmentDetails };
        request.Shipment = shipment;
        request.Version = version;

        try
        {
            CompleteSaleResponseType response = service.CompleteSale(request);
            return response;
        }
        catch (Exception ex)
        {
            if (ex.Message.ToLower().Contains("auth token"))
                throw new InvalidEbayCredentialsException();
            else
                throw ex;
        }
    }

    public GetMemberMessagesResponseType GetTransactionMessages(string itemID, string BuyerID, string userToken)
    {
        string callname = "GetMemberMessages";

        #region Initialise Needed Variables

        //Get the Server to use (Sandbox or Production)
        string serverUrl = ConfigurationManager.AppSettings["TradingService"];

        //SiteID = 0  (US) - UK = 3, Canada = 2, Australia = 15, ....
        //string siteID = "0";

        eBayAPIInterfaceService service = new eBayAPIInterfaceService();
        string requestURL = serverUrl + "?callname=" + callname + "&siteid=" + siteID
                            + "&appid=" + AppID + "&version=" + version + "&routing=default";
        service.Url = requestURL;

        // Set credentials
        service.RequesterCredentials = new CustomSecurityHeaderType();
        service.RequesterCredentials.Credentials = new UserIdPasswordType();
        service.RequesterCredentials.Credentials.AppId = AppID;
        service.RequesterCredentials.Credentials.DevId = DevID;
        service.RequesterCredentials.Credentials.AuthCert = CertID;
        service.RequesterCredentials.eBayAuthToken = userToken;
        #endregion

        GetMemberMessagesRequestType request = new GetMemberMessagesRequestType();
        request.MailMessageType = MessageTypeCodeType.All;
        request.MailMessageTypeSpecified = true;
        request.WarningLevel = WarningLevelCodeType.High;
        request.MessageStatus = MessageStatusTypeCodeType.Unanswered;
        request.MessageStatusSpecified = true;
        request.ItemID = itemID;
        if (BuyerID != string.Empty)
            request.SenderID = BuyerID;
        request.Version = version;

        try
        {
            GetMemberMessagesResponseType response = service.GetMemberMessages(request);
            return response;
        }
        catch (Exception ex)
        {
            if (ex.Message.ToLower().Contains("auth token"))
                throw new InvalidEbayCredentialsException();
            else
                throw ex;
        }
    }

    public string GetSessionID()
    {
        string callname = "GetSessionID";

        #region Initialise Needed Variables

        //Get the Server to use (Sandbox or Production)
        string serverUrl = ConfigurationManager.AppSettings["TradingService"];

        //SiteID = 0  (US) - UK = 3, Canada = 2, Australia = 15, ....
        //string siteID = "0";

        eBayAPIInterfaceService service = new eBayAPIInterfaceService();
        string requestURL = serverUrl + "?callname=" + callname + "&siteid=" + siteID
                            + "&appid=" + AppID + "&version=" + version + "&routing=default";
        service.Url = requestURL;

        // Set credentials
        service.RequesterCredentials = new CustomSecurityHeaderType();
        service.RequesterCredentials.Credentials = new UserIdPasswordType();
        service.RequesterCredentials.Credentials.AppId = AppID;
        service.RequesterCredentials.Credentials.DevId = DevID;
        service.RequesterCredentials.Credentials.AuthCert = CertID;
        #endregion

        GetSessionIDRequestType request = new GetSessionIDRequestType();
        request.RuName = ConfigurationManager.AppSettings["RuName"];
        request.Version = version;
        GetSessionIDResponseType response = service.GetSessionID(request);
        return response.SessionID;
    }

    public string FetchToken(string SessionID)
    {
        string callname = "FetchToken";

        #region Initialise Needed Variables

        //Get the Server to use (Sandbox or Production)
        string serverUrl = ConfigurationManager.AppSettings["TradingService"];

        //SiteID = 0  (US) - UK = 3, Canada = 2, Australia = 15, ....
        //string siteID = "0";

        eBayAPIInterfaceService service = new eBayAPIInterfaceService();
        string requestURL = serverUrl + "?callname=" + callname + "&siteid=" + siteID
                            + "&appid=" + AppID + "&version=" + version + "&routing=default";
        service.Url = requestURL;

        // Set credentials
        service.RequesterCredentials = new CustomSecurityHeaderType();
        service.RequesterCredentials.Credentials = new UserIdPasswordType();
        service.RequesterCredentials.Credentials.AppId = AppID;
        service.RequesterCredentials.Credentials.DevId = DevID;
        service.RequesterCredentials.Credentials.AuthCert = CertID;
        #endregion

        FetchTokenRequestType request = new FetchTokenRequestType();
        request.SessionID = SessionID;
        request.Version = version;
        FetchTokenResponseType response = service.FetchToken(request);
        return response.eBayAuthToken;
    }

    public List<ParcelMessage> ConvertEbayMessages(GetMemberMessagesResponseType messages)
    {
        List<ParcelMessage> newMessages = new List<ParcelMessage>();
        foreach (MemberMessageExchangeType message in messages.MemberMessage)
        {
            ParcelMessage newMessage = new ParcelMessage();
            newMessage.Subject = message.Question.Subject;
            newMessage.Text = message.Question.Body;
            newMessage.Date = message.CreationDate;
            newMessage.Status = message.MessageStatus == MessageStatusTypeCodeType.Answered ? "Answered" : "Pending";

            newMessages.Add(newMessage);
        }
        return newMessages;
    }

    public FindItemsByKeywordsResponse SearchItemsByID(string ItemID)
    {
        // Creating an object to the BestMatchService class
        CustomFindingService service = new CustomFindingService();
        service.Url = ConfigurationManager.AppSettings["FindingService"];

        // Creating request object for FindBestMatchItemDetailsByKeywords API
        FindItemsByKeywordsRequest request = new FindItemsByKeywordsRequest();

        // Setting the required property values
        request.keywords = ItemID;

        // Setting the pagination
        PaginationInput pagination = new PaginationInput();
        pagination.entriesPerPageSpecified = true;
        pagination.entriesPerPage = 10;
        pagination.pageNumberSpecified = true;
        pagination.pageNumber = 1;
        request.paginationInput = pagination;

        List<ItemFilter> filters = new List<ItemFilter>();

        ItemFilter filter = new ItemFilter();
        filter.name = ItemFilterType.ListingType;
        filter.value = new string[] { "FixedPrice", "AuctionWithBIN" };
        filters.Add(filter);
        
        request.itemFilter = filters.ToArray();

        request.sortOrder = SortOrderType.BestMatch;
        request.sortOrderSpecified = true;

        // Creating response object
        FindItemsByKeywordsResponse response = service.findItemsByKeywords(request);
        return response;
    }

    public FindItemsByKeywordsResponse SearchItems(string query, int pageSize, int pageNumber, dynamic filterList)
    {
        // Creating an object to the BestMatchService class
        CustomFindingService service = new CustomFindingService();
        service.Url = ConfigurationManager.AppSettings["FindingService"];

        // Creating request object for FindBestMatchItemDetailsByKeywords API
        FindItemsByKeywordsRequest request = new FindItemsByKeywordsRequest();

        // Setting the required property values
        request.keywords = query;

        // Setting the pagination
        PaginationInput pagination = new PaginationInput();
        pagination.entriesPerPageSpecified = true;
        pagination.entriesPerPage = pageSize;
        pagination.pageNumberSpecified = true;
        pagination.pageNumber = pageNumber;
        request.paginationInput = pagination;

        ItemFilter[] filters = new ItemFilter[4];

        ItemFilter filter = new ItemFilter();
        filter.name = ItemFilterType.ListingType;
        filter.value = new string[] { "FixedPrice", "StoreInventory" };

        ItemFilter filter3 = new ItemFilter();
        filter3.name = ItemFilterType.MinPrice;
        filter3.value = new string[] { filterList["MinPrice"] };

        filters[0] = filter;
        filters[1] = filter3;

        if (int.Parse(filterList["MinQuantity"]) > 0)
        {
            ItemFilter filter2 = new ItemFilter();
            filter2.name = ItemFilterType.MinQuantity;
            filter2.value = new string[] { filterList["MinQuantity"] };
            filters[2] = filter2;
        }


        if (filterList["IsNew"] == true)
        {
            ItemFilter filter4 = new ItemFilter();
            filter4.name = ItemFilterType.Condition;
            filter4.value = new string[] { "New" };
            filters[3] = filter4;
        }

        request.itemFilter = filters;

        request.sortOrder = SortOrderType.BestMatch;
        request.sortOrderSpecified = true;

        // Creating response object
        FindItemsByKeywordsResponse response = service.findItemsByKeywords(request);
        return response;
    }

    public FindItemsByKeywordsResponse SearchItems(SellerItem sellerItem, int pageSize, int pageNumber)
    {
        // Creating an object to the BestMatchService class
        CustomFindingService service = new CustomFindingService();
        service.Url = ConfigurationManager.AppSettings["FindingService"];

        // Creating request object for FindBestMatchItemDetailsByKeywords API
        FindItemsByKeywordsRequest request = new FindItemsByKeywordsRequest();

        // Setting the required property values
        request.keywords = sellerItem.Keywords;

        // Setting the pagination
        PaginationInput pagination = new PaginationInput();
        pagination.entriesPerPageSpecified = true;
        pagination.entriesPerPage = pageSize;
        pagination.pageNumberSpecified = true;
        pagination.pageNumber = pageNumber;
        request.paginationInput = pagination;
        ItemFilter filter;
        List<ItemFilter> filters = new List<ItemFilter>();

        /*ListingType*/
        string ListingTypeFilter = null;
        if (sellerItem.Is_Fixed_Price == true)
            ListingTypeFilter = "FixedPrice";
        
        if (sellerItem.Is_Auctions == true)
            ListingTypeFilter += ",AuctionWithBIN";

        if (ListingTypeFilter != null)
        {
            filter = new ItemFilter();
            filter.name = ItemFilterType.ListingType;
            filter.value = ListingTypeFilter.Split(',');
            filters.Add(filter);
        }

        /*ReturnsAcceptedOnly*/
        if (sellerItem.Is_Returns_Accepted == true)
        {
            filter = new ItemFilter();
            filter.name = ItemFilterType.ReturnsAcceptedOnly;
            filter.value = new string[] { "true" };
            filters.Add(filter);
        }

        /*LocatedIn*/
        //if (sellerItem.Is_Location_AU == true)
        //{
        //    filter = new ItemFilter();
        //    filter.name = ItemFilterType.LocatedIn;
        //    filter.value = new string[] { "AU" };
        //    filters.Add(filter);
        //}
        if (!string.IsNullOrEmpty(sellerItem.LocatedIn))
        {
            filter = new ItemFilter();
            filter.name = ItemFilterType.LocatedIn;
            filter.value = new string[] { sellerItem.LocatedIn.ToUpper() };
            filters.Add(filter);
        }
        /*HideDuplicateItems*/
        if (sellerItem.Is_Hide_Duplicates == true)
        {
            filter = new ItemFilter();
            filter.name = ItemFilterType.HideDuplicateItems;
            filter.value = new string[] { "true" };
            filters.Add(filter);
        }
        /*TopRatedSellerOnly*/
        if (sellerItem.Is_Hide_Duplicates == true)
        {
            filter = new ItemFilter();
            filter.name = ItemFilterType.TopRatedSellerOnly;
            filter.value = new string[] {"true"};
            filters.Add(filter);
        }
        else
        {
            /*Specify one or more seller names. Search results will 
             include items from the specified sellers only. 
             The Seller item filter cannot be used together with either the 
             ExcludeSeller or TopRatedSellerOnly item filters. */
            if (!string.IsNullOrEmpty(sellerItem.Inclued_Sellers))
            {
                filter = new ItemFilter();
                filter.name = ItemFilterType.Seller;
                filter.value = sellerItem.Inclued_Sellers.Split(',');
                filters.Add(filter);
            }
            else if (!string.IsNullOrEmpty(sellerItem.Exclude_Sellers))
            {
                filter = new ItemFilter();
                filter.name = ItemFilterType.ExcludeSeller;
                filter.value = sellerItem.Exclude_Sellers.Split(',');
                filters.Add(filter);
            }
        }
        /*MinQuantity*/
        if (sellerItem.Minimum_Quantity > 0)
        {
            filter = new ItemFilter();
            filter.name = ItemFilterType.MinQuantity;
            filter.value = new string[] { sellerItem.Minimum_Quantity.ToString() };
            filters.Add(filter);
        }
        /*MaxQuantity*/
        if (sellerItem.Maximum_Quantity > 0)
        {
            filter = new ItemFilter();
            filter.name = ItemFilterType.MaxQuantity;
            filter.value = new string[] { sellerItem.Maximum_Quantity.ToString() };
            filters.Add(filter);
        }
        /*MinPrice*/
        if (sellerItem.Minimum_Price >= 0)
        {
            filter = new ItemFilter();
            filter.name = ItemFilterType.MinPrice;
            filter.value = new string[] { sellerItem.Minimum_Price.ToString() };
            filters.Add(filter);
        }
        /*MaxPrice*/
        if (sellerItem.Maximum_Price > 0)
        {
            filter = new ItemFilter();
            filter.name = ItemFilterType.MaxPrice;
            filter.value = new string[] { sellerItem.Maximum_Price.ToString() };
            filters.Add(filter);
        }
        /*FeedbackScoreMin*/
        if (sellerItem.Minimum_Feedback >= 0)
        {
            filter = new ItemFilter();
            filter.name = ItemFilterType.FeedbackScoreMin;
            filter.value = new string[] {sellerItem.Minimum_Feedback.ToString()};
            filters.Add(filter);
        }
        /*FeedbackScoreMax*/
        if (sellerItem.Maximum_Feedback > 0)
        {
            filter = new ItemFilter();
            filter.name = ItemFilterType.FeedbackScoreMax;
            filter.value = new string[] { sellerItem.Maximum_Feedback.ToString() };
            filters.Add(filter);
        }
        /*Condition*/
        if (!string.IsNullOrEmpty(sellerItem.Include_Condtion_Codes))
        {
            filter = new ItemFilter();
            filter.name = ItemFilterType.Condition;
            filter.value = sellerItem.Include_Condtion_Codes.Split(',');
            filters.Add(filter);
        }
        /*Cateogry*/
        if (!string.IsNullOrEmpty(sellerItem.Exclude_Category_Codes))
        {
            filter = new ItemFilter();
            filter.name = ItemFilterType.ExcludeCategory;
            filter.value = sellerItem.Exclude_Category_Codes.Split(',');
            filters.Add(filter);
        }
        
        if(filters.Count > 0)
            request.itemFilter = filters.ToArray();
        request.sortOrder = SortOrderType.BestMatch;
        request.sortOrderSpecified = true;
        // Creating response object
        FindItemsByKeywordsResponse response = service.findItemsByKeywords(request);
        return response;
    }

    public SimpleItemType[] GetMultipleItemsDetails(SearchResult result)
    {
        List<SimpleItemType> listSimpleItemType = new List<SimpleItemType>(); 
        Shopping service = new Shopping();
        service.Url = ConfigurationManager.AppSettings["ShoppingService"] + "?appid=" + ConfigurationManager.AppSettings["AppId"] + "&version=523&siteid=" + siteID + "&callname=GetMultipleItems&responseencoding=SOAP&requestencoding=SOAP";
        int page = 0;
        int perPage = 20;
        int skip = 0;
        if(result.count > 40)
        {
            page = 3;
        }
        else if (result.count > 20 && result.count <= 40)
        {
            page = 2;
        }
        else
        {
            page = 1;
        }

        for (int i = 0; i < page; i++)
        {
            skip = i * perPage;
            var resultItems = result.item.Skip(skip).Take(perPage);
            List<String> items = new List<String>();
            foreach (SearchItem item in resultItems)
            {
                items.Add(item.itemId);
            }
            System.Net.ServicePointManager.Expect100Continue = false;
            GetMultipleItemsRequestType multipleRequest = new GetMultipleItemsRequestType();
            multipleRequest.IncludeSelector = "Details,ItemSpecifics,Variations";
            multipleRequest.ItemID = items.ToArray();
            GetMultipleItemsResponseType multipleResponse = service.GetMultipleItems(multipleRequest);
            listSimpleItemType.AddRange(multipleResponse.Item.ToList());
        }
        return listSimpleItemType.ToArray();

        
    }

    public SimpleItemType GetSingleItem(string itemID)
    {
        Shopping service = new Shopping();
        service.Url = ConfigurationManager.AppSettings["ShoppingService"] + "?appid=" + ConfigurationManager.AppSettings["AppId"] + "&version=523&siteid=" + siteID + "&callname=GetSingleItem&responseencoding=SOAP&requestencoding=SOAP";
        GetSingleItemRequestType multipleRequest = new GetSingleItemRequestType();
        multipleRequest.IncludeSelector = "Details,ItemSpecifics,Variations";
        multipleRequest.ItemID = itemID;
        GetSingleItemResponseType multipleResponse = service.GetSingleItem(multipleRequest);
        return multipleResponse.Item;
    }

    public string ReviseEbayItemTitle(string ItemId, string Title, string userToken)
    {
        string callname = "ReviseItem";

        #region Initialise Needed Variables

        //Get the Server to use (Sandbox or Production)
        string serverUrl = ConfigurationManager.AppSettings["TradingService"];

        //SiteID = 0  (US) - UK = 3, Canada = 2, Australia = 15, ....
        //string siteID = "0";

        eBayAPIInterfaceService service = new eBayAPIInterfaceService();
        string requestURL = serverUrl + "?callname=" + callname + "&siteid=" + siteID
                            + "&appid=" + AppID + "&version=" + version + "&routing=default";
        service.Url = requestURL;

        // Set credentials
        service.RequesterCredentials = new CustomSecurityHeaderType();
        service.RequesterCredentials.Credentials = new UserIdPasswordType();
        service.RequesterCredentials.Credentials.AppId = AppID;
        service.RequesterCredentials.Credentials.DevId = DevID;
        service.RequesterCredentials.Credentials.AuthCert = CertID;
        service.RequesterCredentials.eBayAuthToken = userToken;
        #endregion

        ReviseItemRequestType requestType = new ReviseItemRequestType();
        ItemType itemType = new ItemType();
        itemType.ItemID = ItemId;

        itemType.Title = Title;

        requestType.Item = itemType;
        requestType.Version = "833";

        try
        {
            ReviseItemResponseType response = service.ReviseItem(requestType);
            if (response.Ack == AckCodeType.Success || response.Ack == AckCodeType.Warning)
                return null;
            else
                return response.Errors.First().ShortMessage;
        }
        catch (Exception ex)
        {
            if (ex.Message.ToLower().Contains("auth token"))
                throw new InvalidEbayCredentialsException();
            else
                throw ex;
        }

        return "";
    }
    
    public string ReviseEbayItem(string ItemID, double Price, string userToken)
    {
        string callname = "ReviseItem";

        #region Initialise Needed Variables

        //Get the Server to use (Sandbox or Production)
        string serverUrl = ConfigurationManager.AppSettings["TradingService"];

        //SiteID = 0  (US) - UK = 3, Canada = 2, Australia = 15, ....
        //string siteID = "0";

        eBayAPIInterfaceService service = new eBayAPIInterfaceService();
        string requestURL = serverUrl + "?callname=" + callname + "&siteid=" + siteID
                            + "&appid=" + AppID + "&version=833&routing=default";
        service.Url = requestURL;

        // Set credentials
        service.RequesterCredentials = new CustomSecurityHeaderType();
        service.RequesterCredentials.Credentials = new UserIdPasswordType();
        service.RequesterCredentials.Credentials.AppId = AppID;
        service.RequesterCredentials.Credentials.DevId = DevID;
        service.RequesterCredentials.Credentials.AuthCert = CertID;
        service.RequesterCredentials.eBayAuthToken = userToken;
        #endregion

        ReviseItemRequestType requestType = new ReviseItemRequestType();
        ItemType itemType = new ItemType();
        itemType.ItemID = ItemID;

        AmountType price = new AmountType();
        price.Value = Price;
        price.currencyID = CurrencyCodeType.AUD;

        itemType.StartPrice = price;
        requestType.Item = itemType;
        requestType.Version = "833";

        try
        {
            ReviseItemResponseType response = service.ReviseItem(requestType);
            if (response.Ack == AckCodeType.Success)
                return null;
            else
                return response.Errors.First().ShortMessage;
        }
        catch (Exception ex)
        {
            if (ex.Message.ToLower().Contains("auth token"))
                throw new InvalidEbayCredentialsException();
            else
                throw ex;
        }
    }

    public string GetUser(string userToken)
    {
        string callname = "GetUser";

        #region Initialise Needed Variables

        //Get the Server to use (Sandbox or Production)
        string serverUrl = ConfigurationManager.AppSettings["TradingService"];

        //SiteID = 0  (US) - UK = 3, Canada = 2, Australia = 15, ....
        //string siteID = "0";

        eBayAPIInterfaceService service = new eBayAPIInterfaceService();
        string requestURL = serverUrl + "?callname=" + callname + "&siteid=" + siteID
                            + "&appid=" + AppID + "&version=833&routing=default";
        service.Url = requestURL;

        // Set credentials
        service.RequesterCredentials = new CustomSecurityHeaderType();
        service.RequesterCredentials.Credentials = new UserIdPasswordType();
        service.RequesterCredentials.Credentials.AppId = AppID;
        service.RequesterCredentials.Credentials.DevId = DevID;
        service.RequesterCredentials.Credentials.AuthCert = CertID;
        service.RequesterCredentials.eBayAuthToken = userToken;
        #endregion

        GetUserRequestType request = new GetUserRequestType();
        request.Version = "833";


        try
        {
            GetUserResponseType response = service.GetUser(request);
            if (response.Ack == AckCodeType.Success)
                return response.User.UserID;
            else
                return null;
        }
        catch (Exception ex)
        {
            if (ex.Message.ToLower().Contains("auth token"))
                throw new InvalidEbayCredentialsException();
            else
                throw ex;
        }
    }

    public List<Seller_Item> GetUserItems(string userToken)
    {
        string callname = "GetSellerList";

        #region Initialise Needed Variables

        //Get the Server to use (Sandbox or Production)
        string serverUrl = ConfigurationManager.AppSettings["TradingService"];

        //SiteID = 0  (US) - UK = 3, Canada = 2, Australia = 15, ....
        string siteID = "15";/*Updated by javed 27-11-2013 --- Previous value was 0*/

        eBayAPIInterfaceService service = new eBayAPIInterfaceService();
        string requestURL = serverUrl + "?callname=" + callname + "&siteid=" + siteID
                            + "&appid=" + AppID + "&version=" + version + "&routing=default";
        service.Url = requestURL;

        // Set credentials
        service.RequesterCredentials = new CustomSecurityHeaderType();
        service.RequesterCredentials.Credentials = new UserIdPasswordType();
        service.RequesterCredentials.Credentials.AppId = AppID;
        service.RequesterCredentials.Credentials.DevId = DevID;
        service.RequesterCredentials.Credentials.AuthCert = CertID;
        service.RequesterCredentials.eBayAuthToken = userToken;
        #endregion

        GetSellerListRequestType request = new GetSellerListRequestType();
        request.WarningLevel = WarningLevelCodeType.High;
        request.EndTimeFrom = DateTime.Today;
        request.EndTimeFromSpecified = true;
        request.EndTimeTo = DateTime.Today.AddDays(120);
        request.EndTimeToSpecified = true;
        request.Version = version;
        request.DetailLevel = new DetailLevelCodeType[] { DetailLevelCodeType.ItemReturnDescription, DetailLevelCodeType.ItemReturnAttributes};
        request.Pagination = new PaginationType();
        request.Pagination.PageNumber = 1;
        request.Pagination.PageNumberSpecified = true;
        request.Pagination.EntriesPerPage = 200;
        request.Pagination.EntriesPerPageSpecified = true;
        
        try
        {
            List<Seller_Item> items = new List<Seller_Item>();
            bool moreItem = true;
            while (moreItem == true)
            {
                GetSellerListResponseType response = service.GetSellerList(request);
                

                foreach (var s in response.ItemArray)
                {
                    try
                    {
                        Seller_Item si = new Seller_Item()
                        {
                            CustomLabel = s.SKU,
                            PictureURL = s.PictureDetails != null && s.PictureDetails.PictureURL != null ? s.PictureDetails.PictureURL[0] : string.Empty,
                            ItemViewURL = s.ListingDetails.ViewItemURL,
                            BinPrice = s.ListingDetails.ConvertedBuyItNowPrice.Value,
                            CurrentPrice = s.ListingDetails.ConvertedStartPrice.Value,
                            Currency = s.ListingDetails.ConvertedStartPrice.currencyID != null ? s.ListingDetails.ConvertedStartPrice.currencyID.ToString() : string.Empty,
                            EndDate = s.ListingDetails.EndTime,
                            StartDate = s.ListingDetails.StartTime,
                            ItemID = s.ItemID,
                            ItemName = s.Title,
                            IsPromoItem = s.SellingStatus.PromotionalSaleDetails != null && s.SellingStatus.PromotionalSaleDetails.StartTime.Date <= System.DateTime.Today.Date && s.SellingStatus.PromotionalSaleDetails.EndTime.Date >= System.DateTime.Today.Date ? true : false,
                            Height = s.ShippingPackageDetails.PackageDepth != null ? (decimal?)s.ShippingPackageDetails.PackageDepth.Value : null,
                            Length = s.ShippingPackageDetails.PackageLength != null ? (decimal?)s.ShippingPackageDetails.PackageLength.Value : null,
                            Width = s.ShippingPackageDetails.PackageWidth != null ? (decimal?)s.ShippingPackageDetails.PackageWidth.Value : null,
                            Weight = s.ShippingPackageDetails.WeightMajor != null ? (decimal?)s.ShippingPackageDetails.WeightMajor.Value : null,
                            WeightMinor = s.ShippingPackageDetails.WeightMinor != null ? (decimal?)s.ShippingPackageDetails.WeightMinor.Value : null,
                            CategoryID = s.PrimaryCategory.CategoryID,
                            CategoryName = s.PrimaryCategory.CategoryName,
                            Quantity = s.Quantity,
                            Discription = s.Description,
                            CountryCode = countryID,
                            CountryShortCode = countryShortID,
                            CurrentSales = s.SellingStatus.QuantitySold,
                            QuantityAvailable = s.QuantityAvailable

                        };
                        items.Add(si);

                    }
                    catch (Exception ex)
                    {
                        Logging.WriteLog(LogType.Critical, ex.ToString());
                    }
                }

                if (response.ItemArray.Count() == 200)
                    request.Pagination.PageNumber += 1;
                else
                    moreItem = false;

            }
            

            return items;

            //List<Seller_Item> items = response.ItemArray.Select(s => new Seller_Item
            //{
            //    CustomLabel = s.SKU,
            //    //PictureURL = s.PictureDetails != null && s.PictureDetails.PictureURL != null ? s.PictureDetails.PictureURL[0] : string.Empty,
            //    //ItemViewURL = s.ListingDetails.ViewItemURL,
            //    BinPrice = s.ListingDetails.ConvertedBuyItNowPrice.Value,
            //    CurrentPrice = s.ListingDetails.ConvertedStartPrice.Value,
            //    Currency = s.ListingDetails.ConvertedStartPrice.currencyID != null ? s.ListingDetails.ConvertedStartPrice.currencyID.ToString() : string.Empty,
            //    EndDate = s.ListingDetails.EndTime,
            //    StartDate = s.ListingDetails.StartTime,
            //    ItemID = s.ItemID,
            //    ItemName = s.Title,
            //    IsPromoItem = s.SellingStatus.PromotionalSaleDetails != null && s.SellingStatus.PromotionalSaleDetails.StartTime.Date <= System.DateTime.Today.Date && s.SellingStatus.PromotionalSaleDetails.EndTime.Date >= System.DateTime.Today.Date ? true : false,
            //    Height = s.ShippingPackageDetails.PackageDepth != null ? (decimal?)s.ShippingPackageDetails.PackageDepth.Value : null,
            //    Length = s.ShippingPackageDetails.PackageLength != null ? (decimal?)s.ShippingPackageDetails.PackageLength.Value : null,
            //    Width = s.ShippingPackageDetails.PackageWidth != null ? (decimal?)s.ShippingPackageDetails.PackageWidth.Value : null,
            //    //Weight = s.ShippingPackageDetails.WeightMajor != null ? (decimal?)s.ShippingPackageDetails.WeightMajor.Value : null,
            //    //WeightMinor = s.ShippingPackageDetails.WeightMinor != null ? (decimal?)s.ShippingPackageDetails.WeightMinor.Value : null,
            //    CategoryID = s.PrimaryCategory.CategoryID,
            //    CategoryName = s.PrimaryCategory.CategoryName
            
            //}).ToList();
           
        }
        catch (Exception ex)
        {
            if (ex.Message.ToLower().Contains("auth token"))
                throw new InvalidEbayCredentialsException();
            else
                throw ex;
        }
    }

    public List<Seller_Item> GetUserItemsList(string userToken)
    {
        string callname = "GetMyeBaySelling";

        #region Initialise Needed Variables

        string version1 = "929";
        //Get the Server to use (Sandbox or Production)
        string serverUrl = ConfigurationManager.AppSettings["TradingService"];

        //SiteID = 0  (US) - UK = 3, Canada = 2, Australia = 15, ....
        string siteID = "15";/*Updated by javed 27-11-2013 --- Previous value was 0*/

        eBayAPIInterfaceService service = new eBayAPIInterfaceService();
        string requestURL = serverUrl + "?callname=" + callname + "&siteid=" + siteID
                            + "&appid=" + AppID + "&version=" + version1 + "&routing=default";

        service.Url = requestURL;

        // Set credentials
        service.RequesterCredentials = new CustomSecurityHeaderType();
        service.RequesterCredentials.Credentials = new UserIdPasswordType();
        service.RequesterCredentials.Credentials.AppId = AppID;
        service.RequesterCredentials.Credentials.DevId = DevID;
        service.RequesterCredentials.Credentials.AuthCert = CertID;
        service.RequesterCredentials.eBayAuthToken = userToken;
        #endregion

        
        GetMyeBaySellingRequestType request = new GetMyeBaySellingRequestType();
        request.WarningLevel = WarningLevelCodeType.High;
        //request.EndTimeFrom = DateTime.Today;
        //request.EndTimeFromSpecified = true;
        //request.EndTimeTo = DateTime.Today.AddDays(120);
        //request.EndTimeToSpecified = true;
        request.Version = version1;
        request.DetailLevel = new DetailLevelCodeType[] { DetailLevelCodeType.ItemReturnDescription, DetailLevelCodeType.ItemReturnAttributes };
        //request.ActiveList.Pagination = new PaginationType();
        //request.ActiveList.Pagination.PageNumber = 1;
       // request.ActiveList.Pagination.PageNumberSpecified = true;
        //request.ActiveList.Pagination.EntriesPerPage = 200;
       // request.ActiveList.Pagination.EntriesPerPageSpecified = true;
        request.ActiveList.Include = true;
        request.ActiveList.DurationInDays = 1;
        request.ActiveList.Include = true;
        request.ActiveList.IncludeNotes = true;
       

        //request.ActiveList.Pagination.PageNumber = 1;
        //request.ActiveList.Pagination.EntriesPerPage = 200;
        //request.ActiveList.Pagination.EntriesPerPageSpecified = true;
        //request.ActiveList.Pagination.PageNumberSpecified = true;


        try
        {
            List<Seller_Item> items = new List<Seller_Item>();
            bool moreItem = true;
            while (moreItem == true)
            {
                GetMyeBaySellingResponseType response = service.GetMyeBaySelling(request);


                foreach (var s in response.ActiveList.ItemArray)
                {
                    try
                    {
                        Seller_Item si = new Seller_Item()
                        {
                            CustomLabel = s.SKU,
                            PictureURL = s.PictureDetails != null && s.PictureDetails.PictureURL != null ? s.PictureDetails.PictureURL[0] : string.Empty,
                            ItemViewURL = s.ListingDetails.ViewItemURL,
                            BinPrice = s.ListingDetails.ConvertedBuyItNowPrice.Value,
                            CurrentPrice = s.ListingDetails.ConvertedStartPrice.Value,
                            Currency = s.ListingDetails.ConvertedStartPrice.currencyID != null ? s.ListingDetails.ConvertedStartPrice.currencyID.ToString() : string.Empty,
                            EndDate = s.ListingDetails.EndTime,
                            StartDate = s.ListingDetails.StartTime,
                            ItemID = s.ItemID,
                            ItemName = s.Title,
                            IsPromoItem = s.SellingStatus.PromotionalSaleDetails != null && s.SellingStatus.PromotionalSaleDetails.StartTime.Date <= System.DateTime.Today.Date && s.SellingStatus.PromotionalSaleDetails.EndTime.Date >= System.DateTime.Today.Date ? true : false,
                            Height = s.ShippingPackageDetails.PackageDepth != null ? (decimal?)s.ShippingPackageDetails.PackageDepth.Value : null,
                            Length = s.ShippingPackageDetails.PackageLength != null ? (decimal?)s.ShippingPackageDetails.PackageLength.Value : null,
                            Width = s.ShippingPackageDetails.PackageWidth != null ? (decimal?)s.ShippingPackageDetails.PackageWidth.Value : null,
                            Weight = s.ShippingPackageDetails.WeightMajor != null ? (decimal?)s.ShippingPackageDetails.WeightMajor.Value : null,
                            WeightMinor = s.ShippingPackageDetails.WeightMinor != null ? (decimal?)s.ShippingPackageDetails.WeightMinor.Value : null,
                            CategoryID = s.PrimaryCategory.CategoryID,
                            CategoryName = s.PrimaryCategory.CategoryName,
                            Quantity = s.Quantity,
                            Discription = s.Description,
                            CountryCode = countryID,
                            CountryShortCode = countryShortID,
                            CurrentSales = s.SellingStatus.QuantitySold,
                            QuantityAvailable = s.QuantityAvailable

                        };
                        items.Add(si);

                    }
                    catch (Exception ex)
                    {
                        Logging.WriteLog(LogType.Critical, ex.ToString());
                    }
                }

                if (response.ActiveList.ItemArray.Count() == 200)
                    response.ActiveList.PaginationResult.TotalNumberOfPages += 1;
                else
                    moreItem = false;

            }


            return items;

            //List<Seller_Item> items = response.ItemArray.Select(s => new Seller_Item
            //{
            //    CustomLabel = s.SKU,
            //    //PictureURL = s.PictureDetails != null && s.PictureDetails.PictureURL != null ? s.PictureDetails.PictureURL[0] : string.Empty,
            //    //ItemViewURL = s.ListingDetails.ViewItemURL,
            //    BinPrice = s.ListingDetails.ConvertedBuyItNowPrice.Value,
            //    CurrentPrice = s.ListingDetails.ConvertedStartPrice.Value,
            //    Currency = s.ListingDetails.ConvertedStartPrice.currencyID != null ? s.ListingDetails.ConvertedStartPrice.currencyID.ToString() : string.Empty,
            //    EndDate = s.ListingDetails.EndTime,
            //    StartDate = s.ListingDetails.StartTime,
            //    ItemID = s.ItemID,
            //    ItemName = s.Title,
            //    IsPromoItem = s.SellingStatus.PromotionalSaleDetails != null && s.SellingStatus.PromotionalSaleDetails.StartTime.Date <= System.DateTime.Today.Date && s.SellingStatus.PromotionalSaleDetails.EndTime.Date >= System.DateTime.Today.Date ? true : false,
            //    Height = s.ShippingPackageDetails.PackageDepth != null ? (decimal?)s.ShippingPackageDetails.PackageDepth.Value : null,
            //    Length = s.ShippingPackageDetails.PackageLength != null ? (decimal?)s.ShippingPackageDetails.PackageLength.Value : null,
            //    Width = s.ShippingPackageDetails.PackageWidth != null ? (decimal?)s.ShippingPackageDetails.PackageWidth.Value : null,
            //    //Weight = s.ShippingPackageDetails.WeightMajor != null ? (decimal?)s.ShippingPackageDetails.WeightMajor.Value : null,
            //    //WeightMinor = s.ShippingPackageDetails.WeightMinor != null ? (decimal?)s.ShippingPackageDetails.WeightMinor.Value : null,
            //    CategoryID = s.PrimaryCategory.CategoryID,
            //    CategoryName = s.PrimaryCategory.CategoryName

            //}).ToList();

        }
        catch (Exception ex)
        {
            if (ex.Message.ToLower().Contains("auth token"))
                throw new InvalidEbayCredentialsException();
            else
                throw ex;
        }
    }



    public CategoryType[] GetCategories(string userToken)
    {
        string callname = "GetCategories";

        #region Initialise Needed Variables

        //Get the Server to use (Sandbox or Production)
        string serverUrl = ConfigurationManager.AppSettings["TradingService"];

        //SiteID = 0  (US) - UK = 3, Canada = 2, Australia = 15, ....
        string siteID = "15";

        eBayAPIInterfaceService service = new eBayAPIInterfaceService();
        string requestURL = serverUrl + "?callname=" + callname + "&siteid=" + siteID
                            + "&appid=" + AppID + "&version=" + version + "&routing=default";
        service.Url = requestURL;

        // Set credentials
        service.RequesterCredentials = new CustomSecurityHeaderType();
        service.RequesterCredentials.Credentials = new UserIdPasswordType();
        service.RequesterCredentials.Credentials.AppId = AppID;
        service.RequesterCredentials.Credentials.DevId = DevID;
        service.RequesterCredentials.Credentials.AuthCert = CertID;
        service.RequesterCredentials.eBayAuthToken = userToken;
        
        #endregion

        GetCategoriesRequestType request = new GetCategoriesRequestType();
        request.CategorySiteID = siteID;
        request.LevelLimit = 4;
        request.Version = version;
        request.ViewAllNodes = true;
        request.DetailLevel = new DetailLevelCodeType[] { DetailLevelCodeType.ReturnAll };
        
        
        try
        {
            GetCategoriesResponseType response = service.GetCategories(request);
            return response.CategoryArray;
        }
        catch (Exception ex)
        {
            if (ex.Message.ToLower().Contains("auth token"))
                throw new InvalidEbayCredentialsException();
            else
                throw ex;
        }
    }

    public GetItemShippingResponseType GetShippingCost(string userToken, string itemID)
    {
        string callname = "GetItemShipping";

        #region Initialise Needed Variables

        //Get the Server to use (Sandbox or Production)
        string serverUrl = ConfigurationManager.AppSettings["TradingService"];

        //SiteID = 0  (US) - UK = 3, Canada = 2, Australia = 15, ....
        string siteID = "15";

        eBayAPIInterfaceService service = new eBayAPIInterfaceService();
        string requestURL = serverUrl + "?callname=" + callname + "&siteid=" + siteID
                            + "&appid=" + AppID + "&version=" + version + "&routing=default";
        service.Url = requestURL;

        // Set credentials
        service.RequesterCredentials = new CustomSecurityHeaderType();
        service.RequesterCredentials.Credentials = new UserIdPasswordType();
        service.RequesterCredentials.Credentials.AppId = AppID;
        service.RequesterCredentials.Credentials.DevId = DevID;
        service.RequesterCredentials.Credentials.AuthCert = CertID;
        service.RequesterCredentials.eBayAuthToken = userToken;

        #endregion

        GetItemShippingRequestType request = new GetItemShippingRequestType();
        request.DestinationCountryCode = CountryCodeType.AU;
        request.DestinationPostalCode = "3025";
        request.ItemID = itemID;
        request.QuantitySold = 1;
        request.Version = "505";
        request.DestinationCountryCodeSpecified = true;
        request.QuantitySoldSpecified = true;
        
        
        
        
        try
        {
            GetItemShippingResponseType response = service.GetItemShipping(request);
            return response;
        }
        catch (Exception ex)
        {
            if (ex.Message.ToLower().Contains("auth token"))
                throw new InvalidEbayCredentialsException();
            else
                throw ex;
        }
    }

    //public GetItemShippingResponseType GetShippingCosts(string userToken, string itemID)
    //{
    //    string callname = "GetShippingCosts";

    //    #region Initialise Needed Variables

    //    //Get the Server to use (Sandbox or Production)
    //    string serverUrl = ConfigurationManager.AppSettings["TradingService"];

    //    //SiteID = 0  (US) - UK = 3, Canada = 2, Australia = 15, ....
    //    string siteID = "15";

    //    eBayAPIInterfaceService service = new eBayAPIInterfaceService();
    //    string requestURL = serverUrl + "?callname=" + callname + "&siteid=" + siteID
    //                        + "&appid=" + AppID + "&version=" + version + "&routing=default";
    //    service.Url = requestURL;

    //    // Set credentials
    //    service.RequesterCredentials = new CustomSecurityHeaderType();
    //    service.RequesterCredentials.Credentials = new UserIdPasswordType();
    //    service.RequesterCredentials.Credentials.AppId = AppID;
    //    service.RequesterCredentials.Credentials.DevId = DevID;
    //    service.RequesterCredentials.Credentials.AuthCert = CertID;
    //    service.RequesterCredentials.eBayAuthToken = userToken;

    //    #endregion

    //    GetShippingCostsRequestType request = new GetShippingCostsRequestType();
    //    request.DestinationCountryCode = CountryCodeType1.AU;
    //    request.DestinationPostalCode = "3035";
    //    request.ItemID = itemID;
    //    request.QuantitySold = 1;

    //    try
    //    {
    //        GetShippingCostsResponseType response = service.getsh(request);
    //        return response;
    //    }
    //    catch (Exception ex)
    //    {
    //        if (ex.Message.ToLower().Contains("auth token"))
    //            throw new InvalidEbayCredentialsException();
    //        else
    //            throw ex;
    //    }
    //}
}