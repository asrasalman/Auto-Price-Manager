using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for EbaySearchItem
/// </summary>
public class EbaySearchItem
{
    public string ItemID { get; set; }
    public string Title { get; set; }
    public string ViewURL { get; set; }
    public string ImageURL { get; set; }
    public double Price { get; set; }
    public string TimeRemaining { get; set; }
    public string SellerID { get; set; }
    public float SellerScore { get; set; }
    public bool TopRatedSeller { get; set; }
    public bool IsMyProduct { get; set; }
    public double ShippingCost { get; set; }
    public double TotalCost { get; set; }
    public double ConvertedPrice { get; set; }
    public double TotalCostIncludingShipping { get; set; }

}