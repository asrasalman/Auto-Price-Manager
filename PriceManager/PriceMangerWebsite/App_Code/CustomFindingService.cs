using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using com.ebay.developer;
using System.Net;
using System.Configuration;

/// <summary>
/// Summary description for FindingService
/// </summary>
public class CustomFindingService : FindingService
{
    public CustomFindingService()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    protected override System.Net.WebRequest GetWebRequest(Uri uri)
    {
        try
        {
            HttpWebRequest request = (HttpWebRequest)base.GetWebRequest(uri);
            request.Headers.Add("X-EBAY-SOA-SECURITY-APPNAME", ConfigurationManager.AppSettings["AppId"]);
            request.Headers.Add("X-EBAY-SOA-OPERATION-NAME", "findItemsByKeywords");
            request.Headers.Add("X-EBAY-SOA-SERVICE-NAME", "FindingService");
            request.Headers.Add("X-EBAY-SOA-MESSAGE-PROTOCOL", "SOAP11");
            request.Headers.Add("X-EBAY-SOA-SERVICE-VERSION", "1.0.0");
            request.Headers.Add("X-EBAY-SOA-GLOBAL-ID", "EBAY-AU");
            return request;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
}