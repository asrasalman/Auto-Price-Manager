<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PaymentSuccess.aspx.cs"  MasterPageFile="~/master/SiteMain.master" Inherits="paypal_PaymentSuccess" %>



<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="rt-containerInner" style="height:300px;">
        <div class="rt-grid-12 ">
            <div class="rt-block">
                <div class="componentheading">
                    <h2>
                        Payment Success</h2>
                </div>
                
                <div class="searchContainer22">
                    <p style="color: Green">
                        Your paypal payment is verified successfully. Please click <a href="/pages/login.aspx">here</a> login. 
                    </p>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
