<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Unsubscribe.aspx.cs"  MasterPageFile="~/master/SiteMain.master" Inherits="paypal_Unsubscribe" %>

<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="rt-containerInner" style="height:300px;">
        <div class="rt-grid-12 ">
            <div class="rt-block">
                <div class="componentheading">
                    <h2>
                        Unsubscribe</h2>
                </div>
                
                <div class="searchContainer22">
                    <p >
                        <asp:HiddenField ID="hfSignedCode" runat="server" />
                        Please click below to unsubscribe from our mailing list.<br />
                    </p>
                    <p>
                        <asp:Button ID="btnUnsubscribe" CssClass="button" runat="server" OnClientClick="javascript:return confirm('Are you sure you want to unsubscribe?')" OnClick="btnUnsubscribe_Click" Text="Unsubscribe Me" />
                    </p>
                </div>
            </div>
        </div>
    </div>
</asp:Content>


