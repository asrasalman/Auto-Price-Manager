using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;
using System.Text;
using System.IO;
using OAuth;
using com.magento.www;
using PriceManagerDAL;

public partial class pages_demo_TestMagento : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
       // new EbayServiceBL().GetTransactionMessages("320918552579", "williamgerardit", "AgAAAA**AQAAAA**aAAAAA**F1jIUA**nY+sHZ2PrBmdj6wVnY+sEZ2PrA2dj6wFl4SjDZGDpgmdj6x9nY+seQ**zpgBAA**AAMAAA**wDYvZvwpMsm7skOXpYD65cZtehbwOefjW4b5fMz4o3zzIqA1XP//fxobf/mUGHoC6Lp/isDdqajC+sQhOjOYFBXn8EWgadiSpsAhwrr62bRXTxT7ZOFJLkfDFPiXdsqsQtqsAYdyIal2GzjgyGP+nD9DwUSVCIXjxhimMjeJpK3tOe9It4GPeH6iNJK8qH32xCyMlwIlbyN/fQU4BxRhIeOJdxVZIki9eIZqkGQAhSHhaZR/3HteQQ8iWth/h1o677JAmB8iEDMAWKHCSuqp9cVF2oIUQXv1OnTQKnYpxrW1LRnoKS9J8x/mJMs6wsnGmwYBZgNJJg18noQrY5+rAft6Mracba191jcNYLncFJnQALHbU1Is/9eSPUQ+v0FU4b5YAEOuAORdOzBnM28TiJdq0disi9W+sz60EWWroaURs2OE5ZtR4zr5ZJS0ozdpW6c3lqt3P0jSIz6dEqgwvjPumHokx697KSS+5To4nX6GO7pUDiq2OlHuJtBLjuUTcFQJ7iJCPw6EUfq0zebfPDp4sNO74cQmDoR9NOfNj0QKzoFA/UjDGMzY+heQD69DC5GO2oQVla1Y0g3xMuQk4v4bJySSHroBlHqDhYi9wFqUI8VwDLO13SLpLkPlfbT/SWMxFNsIQuu2FVGxNHmfni4GJ6kQIVDDgvezfxmEeFxLNdfdzdbOBYkYxPxFBdY4rACM3a2nBkbfnMKhLkVuJxq7TvyLWxndQf4Cegpp9rm37dlxiIGmJiDbrADJSFCA");
        TestOAuth();
    }

    private void TestOAuth()
    {
        var consumerKey = "3t82hBH8dF2ymlW";
        var consumerSecret = "79acbe81-7a9b-404f-94aa-7356c641e2e5";

        //var uri = new Uri("http://www.wgit-tech.com/shop/index.php/oauth/initiate");

        var uri = new Uri("https://testing.crunch.co.uk/crunch-core/oauth/request_token");
        Dictionary<string, string> extraParams = new Dictionary<string, string>();
        extraParams.Add("oauth_callback", "oob");


        // Generate a signature 
        OAuthBase oAuth = new OAuthBase();
        string nonce = oAuth.GenerateNonce();
        string timeStamp = oAuth.GenerateTimeStamp();
        string parameters;
        string normalizedUrl;

        string signature = oAuth.GenerateSignature(uri, consumerKey, consumerSecret,
        String.Empty, String.Empty, "POST", timeStamp, nonce, OAuthBase.SignatureTypes.HMACSHA1,
        out normalizedUrl, out parameters, extraParams);

        signature = HttpUtility.UrlEncode(signature);

        StringBuilder requestUri = new StringBuilder(normalizedUrl);
        requestUri.AppendFormat("?oauth_consumer_key={0}&", consumerKey);
        requestUri.AppendFormat("oauth_nonce={0}&", nonce);
        requestUri.AppendFormat("oauth_timestamp={0}&", timeStamp);
        requestUri.AppendFormat("oauth_signature_method={0}&", "HMAC-SHA1");
        requestUri.AppendFormat("oauth_version={0}&", "1.0");
        requestUri.AppendFormat("oauth_signature={0}&", signature);
        requestUri.AppendFormat("oauth_callback={0}", "oob");

        var request = (HttpWebRequest)WebRequest.Create(new Uri(requestUri.ToString()));
        request.Method = WebRequestMethods.Http.Post;
        try
        {
            string result = Process(requestUri.ToString(), string.Empty);

            var parts = result.Split('&');
            var token = parts[0].Substring(parts[0].IndexOf('=') + 1);
            var tokenSecret = parts[1].Substring(parts[1].IndexOf('=') + 1);

            result = String.Format("oauth_token={0}", token);
            var authorizeUrl = "http://www.wgit-tech.com/shop/index.php/admin/oauth_authorize?" + result;

            Response.Write("<script>window.open('" + authorizeUrl + "');</script>");

            string verifier = "u1ly8ykpthwqxexvej0oqew8kw4z3iiw";

            nonce = oAuth.GenerateNonce();
            timeStamp = oAuth.GenerateTimeStamp();

            uri = new Uri("http://www.wgit-tech.com/shop/index.php/oauth/token");

            extraParams = new Dictionary<string, string>();
            extraParams.Add("oauth_token", token);
            extraParams.Add("oauth_verifier", verifier);

            signature = oAuth.GenerateSignature(uri, consumerKey, consumerSecret,
            String.Empty, tokenSecret, "POST", timeStamp, nonce, OAuthBase.SignatureTypes.HMACSHA1,
            out normalizedUrl, out parameters, extraParams);

            signature = HttpUtility.UrlEncode(signature);

            StringBuilder tokenUri = new StringBuilder(normalizedUrl);
            tokenUri.AppendFormat("?oauth_consumer_key={0}&", consumerKey);
            tokenUri.AppendFormat("oauth_nonce={0}&", nonce);
            tokenUri.AppendFormat("oauth_timestamp={0}&", timeStamp);
            tokenUri.AppendFormat("oauth_signature_method={0}&", "HMAC-SHA1");
            tokenUri.AppendFormat("oauth_version={0}&", "1.0");
            tokenUri.AppendFormat("oauth_signature={0}&", signature);
            tokenUri.AppendFormat("oauth_token={0}&", token);
            tokenUri.AppendFormat("oauth_verifier={0}", verifier);

            string finalResult = Process(tokenUri.ToString(), string.Empty);

            parts = result.Split('&');
            token = parts[0].Substring(parts[0].IndexOf('=') + 1);
            tokenSecret = parts[1].Substring(parts[1].IndexOf('=') + 1);
        }
        catch (WebException ex)
        {
            var response = (HttpWebResponse)ex.Response;
            var queryString = new StreamReader(response.GetResponseStream()).ReadToEnd();
        }

        /*
        var oauth = new OAuth.Manager();
        // the URL to obtain a temporary "request token"
        var rtUrl = "http://www.wgit-tech.com/shop/index.php/oauth/initiate?oauth_callback=http://www.wgit-tech.com/shop/oauth_admin.php";
        oauth["consumer_key"] = "mw2jxs7rn059wza9q7a9kcd39uup9bze";
        oauth["consumer_secret"] = "448svpz44w3js7x9d9pg4my254g6osdl";
        oauth.AcquireRequestToken(rtUrl, "POST");
        */

    }

    public static string Process(string url)
    {
        var request = (HttpWebRequest)WebRequest.Create(url);
        request.Timeout = 200000;
        request.Method = "GET";

        HttpWebResponse HttpWResp = (HttpWebResponse)request.GetResponse();
        Stream streamResponse = HttpWResp.GetResponseStream();

        // And read it out
        StreamReader reader = new StreamReader(streamResponse);
        string response = reader.ReadToEnd();

        reader.Close();
        reader.Dispose();

        return response;
    }

    public static string Process(string url, string postData)
    {
        var request = (HttpWebRequest)WebRequest.Create(url);
        request.Timeout = 200000;
        request.ContentType = "application/json";
        request.Method = "POST";

        using (var streamWriter = new StreamWriter(request.GetRequestStream()))
        {
            string json = postData;

            streamWriter.Write(json);
        }
        var httpResponse = (HttpWebResponse)request.GetResponse();
        using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
        {
            var responseText = streamReader.ReadToEnd();
            //Now you have your response.
            //or false depending on information in the response
            return responseText;
        }
    }
}
