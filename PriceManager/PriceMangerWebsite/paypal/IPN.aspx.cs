using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net.Mail;
using PriceManagerDAL;
using System.IO;
using System.Net;
using System.Text;
using System.Globalization;
using System.Text.RegularExpressions;

public partial class paypal_IPN : Base
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //StreamWriter stw = new StreamWriter(Server.MapPath("/Log/IPNstatus.txt"), true);
        //stw.WriteLine("INSTANT PAYPAL NOTIFICATION STARTED " + DateTime.Now.ToString());
        try
        {


            //Post back to either sandbox or live

            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(PayPal.GetPaypalServiceURL());

            //Set values for the request back
            req.Method = "POST";
            req.ContentType = "application/x-www-form-urlencoded";
            byte[] param = Request.BinaryRead(HttpContext.Current.Request.ContentLength);
            string strRequest = Encoding.ASCII.GetString(param);
            strRequest += "&cmd=_notify-validate";
            req.ContentLength = strRequest.Length;

            //for proxy
            //WebProxy proxy = new WebProxy(new Uri("http://url:port#"));
            //req.Proxy = proxy;

            //Send the request to PayPal and get the response
            StreamWriter streamOut = new StreamWriter(req.GetRequestStream(), System.Text.Encoding.ASCII);
            streamOut.Write(strRequest);
            streamOut.Close();
            StreamReader streamIn = new StreamReader(req.GetResponse().GetResponseStream());
            string strResponse = streamIn.ReadToEnd();
            streamIn.Close();
            //*******************************************

            #region PAYPAL VARIBALES
            //stw.WriteLine("GETTING VARIABLES");
            //stw.WriteLine(Request.Params.ToString());
            string Paypal_txn_type = string.Empty;
            string Paypal_payment_status = string.Empty;
            string Paypal_txn_id = string.Empty;
            string Paypal_Subscription_Id = string.Empty;
            string Paypal_receiver_email = string.Empty;
            string PayPal_buyer_email = string.Empty;
            string PayPal_payer_id = string.Empty;
            decimal Paypal_mc_gross = 0;
            decimal Paypal_trial_amount = 0;
            string Paypal_payment_type = string.Empty;
            string Paypal_custom = string.Empty;
            string Paypal_next_payment_date = string.Empty;
            string Paypal_subscr_date = string.Empty;
            string paystat = string.Empty;
            string paypal_item_id = string.Empty;

            // Paypal_txn_id = Request["txn_id"].ToString();
            //stw.WriteLine("receiver_email");
            Paypal_receiver_email = Request["receiver_email"].ToString();
            //stw.WriteLine("payer_email");
            PayPal_buyer_email = Request["payer_email"].ToString();
            //stw.WriteLine("payer_id");
            PayPal_payer_id = Request["payer_id"].ToString();
            //stw.WriteLine("receiver_email");
            //stw.WriteLine("subscr_id");
            Paypal_Subscription_Id = Request["subscr_id"];
            //stw.WriteLine("txn_type");
            //Paypal_payment_type = Request["payment_type"].ToString();
            Paypal_txn_type = Request["txn_type"].ToString().ToLower();
            //stw.WriteLine("custom");
            Paypal_custom = Request["custom"].ToString();
            //stw.Write("itemNumber");
            paypal_item_id = Request["item_number"].ToString();
            //stw.WriteLine("SUCCESS VARIABLES");
            #endregion


            if (Request["payment_status"] != null)
            {
                Paypal_payment_status = Request["payment_status"].ToString();
            }
            //stw.WriteLine(Paypal_payment_status);


            DataModelEntities entites = new DataModelEntities();
            List<User> Users = entites.Users.ToList();



            // THIS MEANS THAT THE PAY PAL PAYMENT PROCESS IS VERIFED
            if (strResponse == "VERIFIED")
            {
                UTF8Encoding en = new UTF8Encoding();
                string input = en.GetString(param);

                try
                {
                    //stw.WriteLine("============================================");
                    //stw.WriteLine("PAYMENT STATUS= " + Paypal_payment_status);
                    //stw.WriteLine("VERIFIED " + DateTime.Now.ToString());
                    if (Paypal_txn_type == "subscr_signup" || Paypal_txn_type == "subscr_payment")
                    {
                        //stw.WriteLine("SIGNUP OR RECURING PAYMENT - " + DateTime.Now.ToString());
                        //if (Paypal_payment_status == 1)
                        //{

                        //=== FIRST TIME SUBSCRIPTION MEANS COMES FROM SINGUP PROCESS  =====
                        
                        if (Paypal_custom != string.Empty && Users.Any(u => u.Confirmation_Code == Paypal_custom && u.Paypal_Buyer_ID == null && u.Is_Paypal_Paid == false && u.Is_Active == true))
                        {
                            
                            // THIS BLOCK OF CODE WILL BE HIT WHEN THE USER SIGN UP

                            //stw.WriteLine("NEW SUBSCRIPTION " + DateTime.Now.ToString());
                            User user = Users.Single(u => u.Confirmation_Code == Paypal_custom);
                            int userCode = user.User_Code;
                            user.Paypal_Email_Address = PayPal_buyer_email;
                            user.Paypal_Buyer_ID = PayPal_payer_id;
                            user.Modified_Date = DateTime.Now;
                            user.Is_Paypal_Paid = true;
                            user.Is_Locked = false;
                            user.Is_Active = true;
                            user.Package_Id = int.Parse(paypal_item_id);
                            user.Paypal_Subscription_ID = Paypal_Subscription_Id;
                            entites.SaveChanges();

                            //==== INSERT SUBSCRIPTION DETAIL
                            Subscription sub = new Subscription();
                            sub.User_Code = userCode;
                            sub.PayPal_Email_Address = PayPal_buyer_email;
                            //// FOR TRIAL PERIOD GET THE TRIAL AMOUNT 
                            //// FOR TRIAL PERIOD GET THE TRIAL AMOUNT 

                            bool isTrial = false;

                            try
                            {
                                // IF NO TRIAL PERIOD IS GIVEN ON THE PRODUCT THIS VARIBALE WILL BE EMPTY
                                Paypal_trial_amount = decimal.Parse(Request["mc_amount1"].ToString());
                                isTrial = true;
                                //stw.WriteLine("TRIAL PERIOD");
                            }
                            catch
                            {
                                //stw.WriteLine("NO TRIAL PERIOD");
                            }
                            try
                            {

                                // IF TRIAL PERIOD IS GIVEN ON THE PRODUCT THIS VARIBALE WILL BE EMPTY
                                Paypal_mc_gross = decimal.Parse(Request["mc_amount3"].ToString());
                                //stw.WriteLine("Sub Amount: " + Request["mc_amount3"].ToString());
                            }
                            catch
                            {
                            }

                            //stw.WriteLine("TRIAL / NORMAL");

                            // IF TRIAL PERIOD IS GIVEN ON THE PRODUCT THEN GET ASSIGN THE PAYPAL TRIAL AMOUNT ON AMOUNT VARIABLE
                            sub.Amount = isTrial == true ? Paypal_trial_amount : Paypal_mc_gross;
                            sub.Transaction_Id = Paypal_txn_id.ToString();
                            sub.Subscription_Date = DateTime.Now;
                            sub.Paypal_Buyer_ID = PayPal_payer_id;
                            sub.Package_Id = int.Parse(paypal_item_id);
                            //  ConvertFromPayPalDate(Paypal_subscr_date, 0);
                            sub.Expiry_Date = DateTime.Now.AddMonths(1);
                            entites.Subscriptions.AddObject(sub);
                            entites.SaveChanges();

                            //==== SEND ACTIVATION EMAIL TO THE USER AFTER PAYMENT PROCESSING
                            if (user.Package_Id != (int)Common.Package.Trial)
                            {
                                string link = Request.Url.GetLeftPart(UriPartial.Authority) + @"/site/Activation.aspx?vc=" + user.Confirmation_Code;
                                sendmail(user, link);
                            }
                            //stw.WriteLine("END OF SIGNUP");


                        } // FOR PAYMENT SECOND TIME
                        //else if (Users.Any(u => u.Paypal_Buyer_ID == PayPal_payer_id))//
                        else if (Users.Any(u => u.Paypal_Email_Address == PayPal_buyer_email))
                        {
                            // THIS BLOCK OF CODE WILL BE HIT WHEN THE USER'S RECURRING PAYMEN OCCURS OR THE USER RE SUBSCRIBES 
                            
                            //stw.WriteLine("RECURING SUBSCRIPTION " + DateTime.Now.ToString());
                            //stw.WriteLine(Request.Params.ToString());
                            //User user = Users.Single(u => u.Paypal_Buyer_ID == PayPal_payer_id);
                            User user = Users.Single(u => u.Paypal_Email_Address == PayPal_buyer_email);
                            user.Is_Paypal_Paid = true;
                            user.Is_Active = true;
                            user.Is_Locked = false;
                            //==== INSERT SUBSCRIPTION DETAIL
                            Subscription sub = new Subscription();
                            sub.User_Code = user.User_Code;
                            sub.PayPal_Email_Address = PayPal_buyer_email;
                            //stw.WriteLine("RECURRING PAYMENT");
                            try
                            {
                                //Paypal_mc_gross = decimal.Parse(Request["mc_amount3"].ToString());

                                Paypal_mc_gross = decimal.Parse(Request["mc_gross"].ToString());  
                            }
                            catch
                            {
                            }
                            // PACKAGE UPDATION 

                            sub.Package_Id = int.Parse(paypal_item_id);
                            sub.Amount = Paypal_mc_gross;
                            sub.Transaction_Id = Paypal_txn_id.ToString();
                            sub.Subscription_Date = DateTime.Now;
                            sub.Paypal_Buyer_ID = PayPal_payer_id;
                            //  ConvertFromPayPalDate(Paypal_subscr_date, 0);
                            sub.Expiry_Date = DateTime.Now.AddMonths(1);
                            entites.Subscriptions.AddObject(sub);
                            entites.SaveChanges();

                        }
                    }
                    else if (Paypal_txn_type == "subscr-failed")
                    {
                        // THIS BLOCK OF CODE WILL BE HIT WHEN THE USER'S RECURRING PAYMENT WILL BE FAILED DUE TO INSUFFICIENT BALANCE
                        //stw.WriteLine("RECURRING FAILED SUBSCRIPTION " + DateTime.Now.ToString());
                        //User user = Users.Single(u => u.Paypal_Buyer_ID == PayPal_payer_id);
                        User user = Users.Single(u => u.Paypal_Email_Address == PayPal_buyer_email);
                        user.Is_Paypal_Paid = false;
                        user.Is_Locked = true;
                        ////==== DELETE SUBSCRIPTION DETAILsu
                        //List<Subscription> subs = entites.Subscriptions.ToList();
                        //entites.Subscriptions.Where(w => w.Paypal_Buyer_ID == PayPal_payer_id)
                        //.ToList().ForEach(entites.Subscriptions.DeleteObject);
                        entites.SaveChanges();
                        //stw.WriteLine("CANCEL FAILED CoMNPLETED" + DateTime.Now.ToString());
                        //==== SEND ACTIVATION EMAIL TO THE USER AFTER PAYMENT PROCESSING
                        sendmailFailure(user.Full_Name, user.Email_Address, user.Full_Name, user.Password, user.Confirmation_Code);

                    }
                    else if (Paypal_txn_type == "subscr_cancel")
                    {
                        // THIS BLOCK OF CODE WILL BE HIT WHEN THE USER'S CANCEL THE SUBSCRIPTION
                        //stw.WriteLine("CANCEL SUBSCRIPTION " + DateTime.Now.ToString());
                        //User user = Users.Single(u => u.Paypal_Buyer_ID == PayPal_payer_id);
                        User user = Users.Single(u => u.Paypal_Email_Address == PayPal_buyer_email);
                        user.Is_Paypal_Paid = false;
                        user.Is_Active = false;
                        entites.SaveChanges();
                        sendmailCancelSubscription(user.Full_Name, user.Email_Address, user.Full_Name, user.Password, user.Confirmation_Code);

                        //stw.WriteLine("CANCEL SUBSCRIPTION CoMNPLETED" + DateTime.Now.ToString());

                    }
                    //}
                    //else
                    //{

                    //}

                }
                catch (Exception ex)
                {
                    //stw.WriteLine(ex.Message);
                    //stw.Dispose();
                    Email.SendMail("jawaid@aimviz.com", "Test IPN", ex.ToString(), null);
                }

                //stw.Dispose();

            }
            else if (strResponse == "INVALID")
            {
                // THIS BLOCK OF CODE WILL BE HIT WHEN THE USER'S PAYPAL PROCESS IS INVALID
                UTF8Encoding en = new UTF8Encoding();
                string input = en.GetString(param);
                //StreamWriter stw = new StreamWriter(Server.MapPath("status.txt"), true);

                //stw.WriteLine("Invalid " + DateTime.Now.ToString());
                // CANCEL SUBSCTIPION AS USER DONT HAVE FUNDS
                if (Users.Any(u => u.Paypal_Buyer_ID == PayPal_payer_id && u.Is_Active == true))
                {
                    //stw.WriteLine("invalid SUBSCRIPTION " + DateTime.Now.ToString());
                    User user = Users.Single(u => u.Paypal_Email_Address == PayPal_buyer_email);
                    user.Is_Paypal_Paid = false;
                    user.Is_Locked = true;
                    entites.SaveChanges();
                    //==== SEND ACTIVATION EMAIL TO THE USER AFTER PAYMENT PROCESSING
          
                }



                //stw.Dispose();
            }
            else
            {
                //log response/ipn data for manual investigation
                
            }
            //stw.Dispose();
        }
        catch (Exception ee)
        {
            ////stw.WriteLine(DateTime.Now.ToString());
            ////stw.WriteLine(ee.ToString());
            //stw.Dispose();
            //Logging.WriteLog(LogType.Critical, ee.ToString()); 
            Email.SendMail("jawaid@aimviz.com", "Test IPN", ee.ToString(), null);
        }

    }

    private void sendmail(PriceManagerDAL.User user, string verificionLink)
    {
        string CC = System.Configuration.ConfigurationManager.AppSettings["CCEmailAddress"];
        string smsg = Email.GetTemplateString((int)Common.EmailTemplates.NewRegistration);
        smsg = smsg.Replace("{User_Name}", user.Full_Name);
        smsg = smsg.Replace("{Verification_Link}", verificionLink);
        MailMessage message = new MailMessage();
        Email.SendMail(user.Email_Address, "Account Verification Email", smsg, CC);
    }

    


    public void sendmailFailure(string fullname, string emailid, string username, string password, string verfication)
    {
        string siteurl = Request.Url.GetLeftPart(UriPartial.Authority) + @"/site/Activation.aspx";

        string smsg = "Dear " + fullname + "<br/> you have insufficient funds in your account. Your subscription for next month is not activated";
        smsg += "<br>";
        smsg += "Warm regards, <br> Parcel Solutions";
        MailMessage message = new MailMessage();


        Email.SendMail(emailid, "Paypal Recurring Payment Failed", smsg, null);
    }


    public void sendmailCancelSubscription(string fullname, string emailid, string username, string password, string verfication)
    {
        string siteurl = PayPal.GetPayPalURL(verfication);// Request.Url.GetLeftPart(UriPartial.Authority) + @"/site/Activation.aspx";

        string smsg = "Dear " + fullname + "<br/> you have cancelled the subscription of Eparcel Solutions. Your account is de-actived now.";
        smsg += "<br>";
        smsg += "<br>";
        smsg += "Thank you, <br> Parcel Solutions";
        MailMessage message = new MailMessage();

        Email.SendMail(emailid, "Paypal Subscription Cancelled", smsg, null);
    }




}


