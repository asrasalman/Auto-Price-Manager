using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.Entity;
using System.Data.EntityClient;
//using SmartSend;
using PriceManagerDAL;

public partial class pages_ServiceTest : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string UserName = "testing.emu.com.au";
        string Password = "gW465KMI";
        if (!IsPostBack)
        {
            /*
            SmartSend.ServiceSoapClient service = new ServiceSoapClient();

            SmartSend.Request smRequest = new SmartSend.Request();
            smRequest.VIPUsername = UserName;
            smRequest.VIPPassword = Password;
            smRequest.PostcodeFrom = "2046";
            smRequest.SuburbFrom = "ABBOTSFORD";
            smRequest.StateFrom = "NSW";
            smRequest.PostcodeTo = "872";
            smRequest.SuburbTo = "ALICE SPRINGS";
            smRequest.StateTo = "NT";

            SmartSend.Item item = new SmartSend.Item();
            item.Description = "Test";
            item.Weight = 1;
            item.Height = 1;
            item.Depth = 1;
            item.Length = 1;

            smRequest.Items = new SmartSend.Item[1];
            smRequest.Items[0] = item;

            Result result = service.ObtainQuote(smRequest);
            */


            /*
            Location[] locations = service.GetLocations();

            DataModelEntities context = new DataModelEntities();
            List<PostCode> postCodes = context.PostCodes.ToList();
            foreach (Location location in locations)
            {
                string postCode = location.Postcode.TrimStart('0');
                postCodes.First(p => p.Post_Code == postCode && p.Location == location.Suburb).State = location.State;
            }
            context.SaveChanges();
            */
        }
    }
}