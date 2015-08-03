<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ErrorPage.aspx.cs" Inherits="site_Activation" %>


<%@ Register src="../control/Login.ascx" tagname="Login" tagprefix="uc1" %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>Parcel Solutions</title>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="description" content="" />
    <link rel="stylesheet" type="text/css" href="/css/styles.css" />
    <link rel="stylesheet" type="text/css" href="/css/blue-green.css" media="screen" />
    <link rel="stylesheet" type="text/css" href="/css/loginStyle.css" />
    <script type="text/javascript" src="/scripts/jquery.js"></script>
    <script type="text/javascript" src="/scripts/jquery.validate.js"></script>
    <script type="text/javascript" src="/scripts/site.js"></script>
    <script type="text/javascript" src="/scripts/demo.js"></script>
    <script type="text/javascript" src="/scripts/login.js"></script>
    <script type="text/javascript" charset="utf-8">
        $(document).ready(function () {
            $("#signup_form").validate();
        });
        function f_name_onclick() {

        }

    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div id="header">
        <div class="page">
            <a href="/default.aspx" class="logo">Parcel Solutions</a>
            <ul>
                <li><a href="/default.aspx">Home</a></li>
                <li><a href="about.aspx">About</a></li>
                <li><a href="tour.aspx">Tour</a></li>
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
                    Error Page</h1>
            </div>
            <div id="signup" class="padding">
                <div class="left">
                    An unexpected error occured on the website. The website administrator is notified.
                    <br />               
                    <br />       
                    <br />       
                    <a href="../Default.aspx"> Return to Homepage</a>
                </div>
                <div class="right">
                    <div class="sidebar_box">
                        
                    </div>
                    <div class="sidebar_box">
                        <h4>
                            Compatible Browsers</h4>
                        <ul>
                            <li>Firefox 2 or later</li>
                            <li>IE 7 or later</li>
                            <li>Safari 3 or later</li>
                            <li>Chrome 2 or later</li>
                            <li>Opera 8 or later</li>
                        </ul>
                    </div>
                    <div class="sidebar_box">
                        <h4>
                            Contact Us</h4>
                        <p>
                            Still have questions? <a href="contact.aspx">Contact us</a>.</p>
                    </div>
                </div>
                <div class="clear">
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
