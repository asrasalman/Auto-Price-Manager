using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for Constant
/// </summary>
public class Constant
{
  /*  public const string UserID = "Seller_1350575118_biz_api1.hotmail.com";
    public const string Password = "1350575136";
    public const string Signature = "AFcWxV21C7fd0v3bYYYRCpSSRl31AJ6lloaCpVNGBwIR0rRS4ZnjIW0P";*/

    public const string UserID = "mgtoeasy_api1.gmail.com";
    public const string Password = "VDSTHJGVBPF8GMA4";
    public const string Signature = "AFcWxV21C7fd0v3bYYYRCpSSRl31AaVr2WirOMg8W7pqV2pDcAipnR-v";

    public const string tickURL = "/images/tick.png";
    public const string crossURL = "/images/delete2.png";

	public Constant()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    public enum enumAccessFunction
    {
        Admin = 1,
        View = 2,
        AddUpdate =3
    }

    public enum Accounts
    {
        Ebay = 1,
        Shopify = 2,
        Magento = 3,
        Bigcommerce = 4
    }

    public enum ParcelStatusCode
    {
        Pending = 1,
        Completed = 2
    }

    public enum PriceAutomationTimeDelay
    {
        NotSpecified = 1,
        Hrs12 = 2,
        Hrs24 = 3,
        Hrs48 = 4,
        Weekly = 5,
        Hrs6 = 6
    }
}