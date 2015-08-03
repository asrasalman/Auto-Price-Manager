<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PaymentFailure.aspx.cs"  MasterPageFile="~/master/SiteMain.master" Inherits="paypal_PaymentFailure" %>

<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="rt-containerInner" style="height:300px;">
        <div class="rt-grid-12 ">
            <div class="rt-block">
                <div class="componentheading">
                    <h2>
                        Payment Failure</h2>
                </div>
                
                <div class="searchContainer22">
                    <p style="color: Red">
                        <asp:HiddenField ID="hfSignedCode" runat="server" />
                        Your paypal payment is not verified, Please click below to make your payment.<br />
                    </p>
                    <p>
                        <asp:Button ID="btnMakePayment" CssClass="button" runat="server" OnClick="btnMakePayment_Click" Text="Make Payment" />
                    </p>
                </div>
            </div>
        </div>
    </div>
</asp:Content>


