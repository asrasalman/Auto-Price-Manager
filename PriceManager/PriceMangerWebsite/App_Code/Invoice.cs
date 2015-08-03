using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for Invoice
/// </summary>
public class Invoice
{
    public string Invoice_Code { get; set; }
    public string From_Name { get; set; }
    public string From_Address { get; set; }
    public string From_Address2 { get; set; }
    public string From_Country { get; set; }
    public string To_Name { get; set; }
    public string To_Address { get; set; }
    public string To_Address2 { get; set; }
    public string To_Country { get; set; }
    public string Currency { get; set; }
    public string Invoice_Date { get; set; }
    public string Invoice_Number { get; set; }
    public string Shipping_Method { get; set; }
    public string From_Suburb { get; set; }
    public string From_State { get; set; }
    public string From_Postcode { get; set; }
    public string From_Phone { get; set; }
    public string From_Email { get; set; }
    public byte[] Company_Logo { get; set; }
    public string ABN_Number { get; set; }
    public string BuyerId { get; set; }
    public decimal Total_Price { get; set; }
    public decimal Shipping_Cost { get; set; }
    public decimal Tax_Price { get; set; }
    public decimal Grand_Total { get; set; }
}

public class InvoiceDetail
{
    public string Invoice_Code { get; set; }
    public string Item_Code { get; set; }
    public string Quantity { get; set; }
    public string Item_Name { get; set; }
    public string Price { get; set; }
    public string BuyerId { get; set; }
}

public class PickingList
{
    public int Quantity { get; set; }
    public string CustomLabel { get; set; }
    public string Description { get; set; }
    public int? QuantitySupplied { get; set; }
    public bool Selected { get; set; }
}

public class ItemLabel
{
    public string Client_Name { get; set; }
    public string Street { get; set; }
    public string Street2 { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public string PostalCode { get; set; }
    public string Record_Number { get; set; }
    public string Custom_Label { get; set; }
    public string BuyerId { get; set; }
}