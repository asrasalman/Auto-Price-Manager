using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for ParcelItem
/// </summary>
public class ParcelItem
{
    public string Type { get; set; }
    public string ItemID { get; set; }
    public string TransactionID { get; set; }
    public string RecordNumber { get; set; }
    public string CustomLabel { get; set; }
    public string CustomLabelText { get; set; }
    public string ItemName { get; set; }
    public string State { get; set; }
    public string BuyerID { get; set; }
    public string BuyerName { get; set; }
    public string EmailAddress { get; set; }
    public string Street { get; set; }
    public string Street2 { get; set; }
    public string Street3 { get; set; }
    public string City { get; set; }
    public string PostalCode { get; set; }
    public string Country { get; set; }
    public string Phone { get; set; }
    public string ShippingMethod { get; set; }
    public int Quantity { get; set; }
    public double Price { get; set; }
    public string Currency { get; set; }
    public double ShippingCost { get; set; }
    public string SaleRecordId { get; set; }
    public double Insurance { get; set; }
    public bool HasInsurance { get; set; }
    public string PostCodeImageURL { get; set; }
    public string AccountID { get; set; }
    public string Messages { get; set; }
    public int? AddressID { get; set; }
}

public class PartMaster
{
    public string CustomLabel { get; set; }
    public string Weight { get; set; }
    public string Length { get; set; }
    public string Width { get; set; }
    public string Height { get; set; }
    public string ItemName { get; set; }
}

public class Manifest
{
    public string AccountID { get; set; }
    public string AccountType { get; set; }
    public string LineType { get; set; }
    public string ItemID { get; set; }
    public string TransactionID { get; set; }
    public string TrackingNumber { get; set; }
    public string Item_TransactionID { get; set; }
}

public class ParcelMessage
{
    public string Subject { get; set; }
    public string Text { get; set; }
    public DateTime Date { get; set; }
    public string Status { get; set; }
}

public class Seller_Item
{
    public string Type { get; set; }
    public string PictureURL { get; set; }
    public string ItemViewURL { get; set; }
    public string ItemID { get; set; }
    public string CustomLabel { get; set; }
    public string CustomLabelText { get; set; }
    public string ItemName { get; set; }
    public double CurrentPrice { get; set; }
    public double BinPrice { get; set; }
    public string Currency { get; set; }
    public double ShippingCost { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public bool IsPromoItem { get; set; }
    public decimal? Height { get; set; }
    public decimal? Length { get; set; }
    public decimal? Width { get; set; }
    public decimal? Weight { get; set; }
    public decimal? WeightMinor { get; set; }
    public string CategoryID { get; set; }
    public string CategoryName { get; set; }
    public int Quantity { get; set; }
    public string Discription { get; set; }
    public string SiteID { get; set; }
    public int CountryCode { get; set; }
    public string CountryShortCode { get; set; }
    public int? CurrentSales { get; set; }
    public int QuantityAvailable { get; set; }
    
}