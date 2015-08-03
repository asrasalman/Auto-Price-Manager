<%@ Page Title="Login | autopricemanger.com" Language="C#" AutoEventWireup="true"
    MasterPageFile="~/master/SiteMain.master" CodeFile="Login.aspx.cs" Inherits="pages_Login" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
<style type="text/css">
      #pnlLogin input.inputbox
      {
          width:250px !important;
      }
      
      #pnlLogin Label
      {
          width:100px; display:inline-block; margin-top:5px;
      }
      #LoginButton
      {
        float: right;
        margin-right: 96px;
      }
      p.FailureText
      {
          color:Red;
      }
      
</style>

    <div class="rt-containerInner" style="min-height:460px;">
        <div class="rt-grid-12 ">
            <div class="rt-block">
                <div class="componentheading">
                    <h2>
                        Enter Your Credentials</h2>
                </div>
                <asp:Panel ID="pnlLogin" runat="server" ClientIDMode="Static">
                    <asp:Login ID="loginUser" runat="server" TitleText="" OnAuthenticate="login1_Authenticate" Width="450"
                        DisplayRememberMe="false" DestinationPageUrl="~/pages/setup/profile.aspx">
                        <LayoutTemplate>
                            <p>
                                <asp:Label ID="UserNameLabel" runat="server" AssociatedControlID="UserName">User Name:</asp:Label>
                                <asp:TextBox ID="UserName" runat="server" CssClass="inputbox"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="UserNameRequired" runat="server" CssClass="redText"
                                                        ControlToValidate="UserName" ErrorMessage="User Name is required." ToolTip="User Name is required."
                                                        ValidationGroup="login1">*</asp:RequiredFieldValidator>
                            </p>
                            <p>
                                <asp:Label ID="PasswordLabel" runat="server" AssociatedControlID="Password">Password:</asp:Label>
                                <asp:TextBox ID="Password" runat="server" TextMode="Password" CssClass="inputbox"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="PasswordRequired" runat="server" CssClass="redText"
                                                        ControlToValidate="Password" ErrorMessage="Password is required." ToolTip="Password is required."
                                                        ValidationGroup="login1">*</asp:RequiredFieldValidator>

                            </p>
                            <p class="FailureText">
                                <asp:Literal ID="FailureText" runat="server" EnableViewState="False"></asp:Literal>
                            </p>
                            <p>
                                <a href="/site/forgotpassword.aspx"><sapan>Forgot Password?</sapan></a>
                                <asp:Button ID="LoginButton" ClientIDMode="Static" runat="server" CommandName="Login" Text="Log In" class="button" ValidationGroup="login1" />
                            </p>
               
                        </LayoutTemplate>
                    </asp:Login>
                </asp:Panel>
                
            </div>
        </div>
    </div>
</asp:Content>
