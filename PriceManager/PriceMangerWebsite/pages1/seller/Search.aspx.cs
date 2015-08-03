using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class pages_seller_Search : Base
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Dictionary<string,int> localTokens = new Dictionary<string,int>();
        EbayServiceBL service = new EbayServiceBL(UserKey);
        foreach (KeyValuePair<int, string> pair in service.UserTokens)
        {
            string result = service.GetUser(pair.Value);

            localTokens.Add(result, pair.Key);
        }

        string tokenJSON = Common.Serialize(localTokens);
        hfTokenJSON.Value = tokenJSON;
    }
}