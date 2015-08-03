using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using com.ebay.developer;
using PriceManagerDAL;

public partial class pages_EbayDownloadCategories : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void DownloadCategories(object sender, EventArgs e)
    {
        DataModelEntities context = new DataModelEntities();
       
        ParcelBL parcelBL = new ParcelBL();
        List<CategoryType> catogories = parcelBL.GetEbayCategories(26);
        
        foreach (CategoryType item in catogories)
        {
            ProductCategory objProductCategory = new ProductCategory();
            objProductCategory.CategoryId = Convert.ToInt32(item.CategoryID);
            objProductCategory.CategoryName = item.CategoryName;
            objProductCategory.ParentId = item.CategoryParentID.Length > 0 ? Convert.ToInt32(item.CategoryParentID[0]) : 0;
            objProductCategory.CategoryLevel = item.CategoryLevel;
            context.AddToProductCategories(objProductCategory);
        }
        context.SaveChanges();
    } 
}