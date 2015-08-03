<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="pages_Login" %>

<%@ Register Src="../control/Login.ascx" TagName="Login" TagPrefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>EbayShipping - Login</title>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="description" content="" />
    <link rel="stylesheet" type="text/css" href="/css/styles.css" />
    <link rel="stylesheet" type="text/css" href="/css/blue-green.css" media="screen" />
    <link rel="stylesheet" type="text/css" href="/css/loginStyle.css" />
    <script type="text/javascript" src="/scripts/jquery.js"></script>
    <script type="text/javascript" src="/scripts/site.js"></script>
    <script type="text/javascript" src="/scripts/demo.js"></script>
    <script type="text/javascript" src="/scripts/login.js"></script>
</head>
<body>
    <form id="form1" runat="server">
    <div id="header">
        <div class="page">
            <a href="/default.aspx" class="logo">Parcel Solutions</a>
            <ul>
                <li><a href="/default.aspx">Home</a></li>
                <li><a href="about.aspx">About</a></li>
                <li class="active"><a href="tour.aspx">Tour</a></li>
                <li><a href="pricing.aspx">Pricing</a></li>
                <li><a href="blog.aspx">Blog</a></li>
                <li><a href="contact.aspx">Contact</a></li>
                <uc1:Login ID="Login1" runat="server" />
            </ul>
            <div class="clear">
            </div>
        </div>
    </div>
    <div id="page">
        <div class="top">
        </div>
        <div class="content">
            <div class="header page">
                <h1>
                    Login</h1>
            </div>
            <div id="tour" class="padding">
                <div class="loginContainer">
                    <div class="section_title">
                        <h3 style="text-shadow: rgb(62, 40, 40) 0px 0px 2px;">
                            Enter Your Credentials</h3>
                    </div>
                    <div class="content contentPadding">
                        <asp:Login ID="loginUser" runat="server" TitleText="" OnAuthenticate="login1_Authenticate"
                            DisplayRememberMe="false" DestinationPageUrl="~/pages/GettingStarted.aspx">
                            <LayoutTemplate>
                                <table cellpadding="1" cellspacing="0" style="border-collapse: collapse;" class="loginContainer">
                                    <tr>
                                        <td>
                                            <table cellpadding="0">
                                                <tr style="height: 70px;">
                                                    <td align="right">
                                                        <asp:Label ID="UserNameLabel" runat="server" AssociatedControlID="UserName">User Name:</asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="UserName" runat="server" CssClass="text_field"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="UserNameRequired" runat="server" CssClass="redText"
                                                            ControlToValidate="UserName" ErrorMessage="User Name is required." ToolTip="User Name is required."
                                                            ValidationGroup="login1">*</asp:RequiredFieldValidator>
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
                                                            ValidationGroup="login1">*</asp:RequiredFieldValidator>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="center" colspan="2" style="color: Red;">
                                                        <asp:Literal ID="FailureText" runat="server" EnableViewState="False"></asp:Literal>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="right" colspan="2">
                                                        <a class="button" style="opacity: 1;"><span style="text-shadow: rgb(0, 0, 0) 0px 0px 2px;">
                                                            <asp:Button ID="LoginButton" runat="server" CommandName="Login" Text="LogIn" Style="margin-top: 10px" ValidationGroup="login1" />
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
            </div>
        </div>
        <div class="bottom">
        </div>
    </div>
    <div id="footer" class="page">
        <p>
            Copyright Parcel Solutions.</p>
        <ul>
            <li><a href="/default.aspx">Home</a></li>
            <li><a href="about.aspx">About</a></li>
            <li><a href="tour.aspx">Tour</a></li>
            <li><a href="pricing.aspx">Pricing</a></li>
            <li><a href="blog.aspx">Blog</a></li>
            <li><a href="contact.aspx">Contact</a></li>
        </ul>
        <div class="clear">
        </div>
    </div>
    </form>
</body>
</html>
