<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Login.ascx.cs" Inherits="control_login" %>
<div id="loginContainer">
<a href="#" class="loginButton btnLogin" id="btnLogin" style="text-decoration: none" runat="server"><span id="spnLoginText" runat="server">Login</span></a>
    <div style="clear: both">
    </div>
    <div id="loginBox">
        <asp:Login ID="login1" runat="server" TitleText="" OnAuthenticate="login1_Authenticate"
            DisplayRememberMe="false" DestinationPageUrl="~/pages/GettingStarted.aspx">
            <LayoutTemplate>
                <table cellpadding="1" cellspacing="0" style="border-collapse: collapse;">
                    <tr>
                        <td>
                            <table cellpadding="0">
                                <tr style="height: 70px;">
                                    <td align="right" style="min-width: 85px;">
                                        <asp:Label ID="UserNameLabel" runat="server" AssociatedControlID="UserName">User Name:</asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="UserName" runat="server" CssClass="text_field"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="UserNameRequired" runat="server" CssClass="redText"
                                            ControlToValidate="UserName" ErrorMessage="User Name is required." ToolTip="User Name is required."
                                            ValidationGroup="popupLogin">*</asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr style="height: 30px;">
                                    <td align="right">
                                        <asp:Label ID="PasswordLabel" runat="server" AssociatedControlID="Password">Password:</asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="Password" runat="server" TextMode="Password" CssClass="text_field"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="PasswordRequired" runat="server" CssClass="redText"
                                            ControlToValidate="Password" ErrorMessage="Password is required." ToolTip="Password is required."
                                            ValidationGroup="popupLogin">*</asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" colspan="2" style="color: Red;">
                                        <asp:Literal ID="FailureText" runat="server" EnableViewState="False"></asp:Literal>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right" colspan="2">
                                        <a href="/site/ForgotPassword.aspx" class="forgotPassword">Forgot Password? </a>
                                        <a
                                            class="button" style="opacity: 0.8;"><span style="text-shadow: rgb(0, 0, 0) 0px 0px 2px;">
                                                <asp:Button ID="LoginButton" runat="server" CommandName="Login" Text="Log In" CssClass="login clean-gray-big"
                                                    Style="margin-top: 10px" ValidationGroup="popupLogin" />
                                            </span></a>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </LayoutTemplate>
        </asp:Login>
    </div>
</div>
<!-- Login Ends Here -->
