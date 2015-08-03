using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Dynamic;
using ShopifyAPIAdapterLibrary;
using System.Configuration;
using System.Web.Script.Serialization;

public partial class pages_ShopifyOrderTest : System.Web.UI.Page
{
    string ConsumerKey = "2a1076aa2b95a03268bea0d7ca667503";
    string ConsumerSecret = "c0b81a311142d05a3150558b836d5a24";
    string ShopifyScope = "read_products,read_orders,write_orders";
    string shopName = "wgit";

    string accessToken = "b27437ffd9d5baf802f637ad0e90b2d8";


    protected void Page_Load(object sender, EventArgs e)
    {
        // check if 'code' is already in the url , i.e. returning after auth phase 1
        if (Request["code"] != null)
        {
            string permanentToken = ExchangeToken(Request["code"]);
        }
    }

    protected void Button1_Click(object sender, EventArgs e)
    {

        var authorizer = new ShopifyAPIAuthorizer(shopName, ConsumerKey, ConsumerSecret);

        // get the Authorization URL and redirect the user
        var authUrl = authorizer.GetAuthorizationURL(new string[] { ShopifyScope }, Request.Url.AbsoluteUri);
        Response.Redirect(authUrl);

    }

    private string ExchangeToken(string tempToken)
    {
        var authorizer = new ShopifyAPIAuthorizer(shopName, ConsumerKey, ConsumerSecret);

        // get the authorization state
        ShopifyAuthorizationState authState = authorizer.AuthorizeClient(tempToken);

        if (authState != null && authState.AccessToken != null)
        {
            return authState.AccessToken;
        }
        else
            return null;
    }

    protected void Button2_Click(object sender, EventArgs e)
    {
        ShopifyAuthorizationState authState = new ShopifyAuthorizationState();
        authState.AccessToken = accessToken;
        authState.ShopName = shopName;

        ShopifyAPIClient api = new ShopifyAPIClient(authState);

        // by default JSON string is returned
        dynamic data = new JavaScriptSerializer().DeserializeObject(api.Get("/admin/orders.json").ToString());

        //dynamic orders = new JavaScriptSerializer().DeserializeObject(data);
    }
}