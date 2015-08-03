using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.IO;
using PriceManagerDAL;
/// <summary>
/// Summary description for PayPal
/// </summary>
public class PayPal
{
    #region PAYPAL_API_SETTINGS

    public static  bool Is_Development = false;

    //IPN URL
    public const string IPNURL = "http://parcelsolutions.com.au:4236/paypal/ipn.aspx";

    //Success URL
    public const string SuccessURL = "http://parcelsolutions.com.au:4236/paypal/PaymentSuccess.aspx";

    //Failure URL
    public const string FailureURL = "http://parcelsolutions.com.au:4236/paypal/PaymentFailure.aspx";

    // THIS IS WHERE THE DATA WILL BE POSTED IF THE LIVE ACCOUNT IS USED
    public const string PAYPAL_LIVE_URL = "https://www.paypal.com/cgi-bin/webscr?"; // NOTE don't forget to put ? at the end of url

    // THIS IS WHERE THE DATA WILL BE POSTED IF THE SANDBOX TEST ACCOUNT IS USED
    public const string PAYPAL_SANDBOX_URL = "https://www.sandbox.paypal.com/cgi-bin/webscr?"; // NOTE don't forget to put ? at the end of url

    // THIS IS WHERE THE USER WILL BE REDIRECT FOR CANCEL SUBSCRIPTION
    public const string PAYPAL_CANCEL_SUBSCRIPTIO_URL = "https://api-3t.sandbox.paypal.com/nvp?"; // NOTE don't forget to put ? at the end of url

    // THIS WILL TELLS THE PAYPAL THAT CLICKED EVENT IS FIRED FOR A  SUBSRIPTION BUTTON 
    public const string PAYPAL_SUBSCRIPTION_BUTTON_CMD = "_s-xclick";

    // THIS IS THE SUBSCRIPTION BUTTON ID , EACH PROCUCT / SUBSCRIPTION BUTTON HAS ITS OWN ID
    public const string PAYPAL_PRODUCT_BUTTON_ID = "A8TT6RCBSY3TU";

    // THIS IS THE VALUE SET TO USED CUSTOM VARIABLE IF WE WANT THAT IPN RETURN THE CUSTOM VARIABLE VALUE TO OUR WEBSITE PAGE AFTER PAYPAL PROCESSING WE SET THE VALUE TO 2
    public const string PAYPAL_API_RM_VARIABLE = "2";

    // THIS IS THE PAYPAL MERCHANT ACCOUNT ID GIVEN BY PAYPAL API
    public const string PAYPAL_API_USER_ID = "APM3193_api1.gmail.com";

    // THIS IS THE PAYPAL MERCHANT ACCOUNT PASSWORD GIVEN BY PAYPAL API
    public const string PAYPAL_API_PASSWORD = "YGF8FUNHHNKDBQ5Z";

    // THIS IS THE PAYPAL MERCHANT ACCOUNT SIGNATURE TO BE USED FOR CANCEL SUBCRIPTION OR OTHER PROCESSES GIVEN BY PAYPAL API
    public const string PAYPAL_API_SIGNATURES = "A812ukn7H6OAq5buFexH3p4DxruaAzBxI8mrv8UtpU0iaZMUPc-xaaBd";

    // THIS IS THE PAYPAL VERSION USED FOR CANCELLATION SUBSCRIPTION  -- DON"T KNOW MUCH ABOUT THIS
    public const string PAYPPAL_VERSION = "56";

    // THIS IS THE METHOD VERSION USED FOR CANCELLATION SUBSCRIPTION
    public const string PAYPAL_API_RECURRING_METHOD = "ManageRecurringPaymentsProfileStatus";

    // THIS IS THE VALUE FOR ACTION USED FOR CANCELLATION SUBSCRIPTION
    public const string PAYPAL_API_CANCEL_ACTION = "Cancel";


    #endregion

    public PayPal()
    {


    }

    #region SubscriptionURL
    // GET PAYPAL SUBSCRIPTION URL BASED ON PAYPAL API VARIBALES
    public static string GetPayPalURL(string CODE)
    {
        string url = GetPaypalServiceURL() + GetSubscriptionVaribalesToPost(CODE);
        return url;
    }

    public static string GetPayPalURL(string CODE, int pkgId)
    {
        string url = GetPaypalServiceURL() + GetSubscriptionVaribalesToPost(CODE, pkgId);
        return url;
    }


    // GET PAYPAL SUBSCRIPTION CANCELLATION URL BASED ON PAYPAL API VARIBALES
    public static bool CancelSubscription(string SubscriptionID)
    {

        string requestURL = PAYPAL_CANCEL_SUBSCRIPTIO_URL + GetCancelSubscriptionVariablesToPost(SubscriptionID);

        WebRequest request = WebRequest.Create(requestURL);
        request.Credentials = CredentialCache.DefaultCredentials;
        WebResponse response = request.GetResponse();
        Stream dataStream = response.GetResponseStream();
        StreamReader reader = new StreamReader(dataStream);
        // Read the content.
        string responseFromServer = reader.ReadToEnd();
        string[] resultArray = responseFromServer.Split('&');
        bool result = resultArray.Where(i => i.Contains("ACK")).First().ToLower().Contains("success");
        return result;
    }


    #endregion

    #region PostVariables
    // THIS IS WHERE WE ARE ASSIGING DIFFERENT TYPES OF PARAMETERS TO BE USED BY PAYPAL API FOR SUBCSRIPTION PROCESSS 
    public static string GetSubscriptionVaribalesToPost(string confirmationCode)
    {

        string postData = "cmd=" + HttpUtility.UrlEncode(PAYPAL_SUBSCRIPTION_BUTTON_CMD);
        postData += "&hosted_button_id=" + HttpUtility.UrlEncode(PAYPAL_PRODUCT_BUTTON_ID);
        postData += "&custom=" + HttpUtility.UrlEncode(confirmationCode);
        postData += "&rm=" + HttpUtility.UrlEncode(PAYPAL_API_RM_VARIABLE);
        
        return postData;
    }

    // THIS IS WHERE WE ARE ASSIGING DIFFERENT TYPES OF PARAMETERS TO BE USED BY PAYPAL API FOR SUBCSRIPTION PROCESSS WITH PACKAGE TYPE
    public static string GetSubscriptionVaribalesToPost(string confirmationCode, int pkgId)
    {

        DataModelEntities entites = new DataModelEntities();
        Package pkg = entites.Packages.Single(p => p.Package_Id == pkgId);

        string postData = "cmd=" + HttpUtility.UrlEncode(PAYPAL_SUBSCRIPTION_BUTTON_CMD);
        postData += "&hosted_button_id=" + HttpUtility.UrlEncode(pkg.Paypal_Button_Id);
        postData += "&custom=" + HttpUtility.UrlEncode(confirmationCode);
        postData += "&rm=" + HttpUtility.UrlEncode(PAYPAL_API_RM_VARIABLE);
        postData += "&notify_url=" + HttpUtility.UrlEncode(IPNURL);
        postData += "&cancel_return=" + HttpUtility.UrlEncode(FailureURL + "?custom=" + confirmationCode);
        postData += "&return=" + HttpUtility.UrlEncode(SuccessURL);
        postData += "&modify=1";
        return postData;
    }


    // THIS IS WHERE WE ARE ASSIGING DIFFERENT TYPES OF PARAMETERS TO BE USED BY PAYPAL API FOR CANCELATION SUBSCRIPTION PROCESSS 
    public static string GetCancelSubscriptionVariablesToPost(string subscriptionID)
    {
        string postData = "USER=" + PAYPAL_API_USER_ID;
        postData += "&PWD=" + PAYPAL_API_PASSWORD;
        postData += "&SIGNATURE=" + PAYPAL_API_SIGNATURES;
        postData += "&VERSION=" + PAYPPAL_VERSION;
        postData += "&METHOD=" + PAYPAL_API_RECURRING_METHOD;
        postData += "&ACTION=" + PAYPAL_API_CANCEL_ACTION;
        postData += "&PROFILEID=" + subscriptionID;
        return postData;
    }

    public static string GetPaypalServiceURL()
    {
        if (Is_Development)
        {
            return PAYPAL_SANDBOX_URL;

        }
        return PAYPAL_LIVE_URL;

    }

    #endregion

}