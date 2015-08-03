<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Header.ascx.cs" Inherits="control_Header" %>
<div id="rt-top">
    <div class="rt-container">
        <div class="rt-grid-3 rt-alpha">
            <div class="rt-block">
                <a href="/" id="rt-logo"></a>
            </div>
        </div>
        <div class="rt-grid-9 rt-omega">
            <ul class="sf-menu  sf-js-enabled sf-shadow">
                <%= menu.ToString() %>
                <li><a href="#">
                    <asp:Literal ID="lblLoginStatus" runat="server"></asp:Literal></a>
                    <ul>
                        <li>
                            <asp:LinkButton ID="lnkSignout" runat="server" Text="Logout" OnClick="lnkSignout_Click"></asp:LinkButton>
                        </li>
                    </ul>
                </li>
                
            </ul>
        </div>
        <div class="clear">
        </div>
    </div>
    <input type="hidden" id="hfIsAdmin" value='<%= IsAdmin %>' />
    <input type="hidden" id="hfCanAddUpdate" value='<%= CanAddUpdate %>' />
    <%--<input type="hidden" id="hfCanView" value='<%= CanView %>' />--%>
</div>
<%--<link href="/css/jqueryslidemenu.css" rel="stylesheet" type="text/css" />
<div class="topHeader">
    <div class='logo'>
    </div>
    <div id="myslidemenu" class="jqueryslidemenu">
        <%= menu.ToString() %>
    </div>
    <div class="userInfoSection">
        <asp:Label ID="lblLoginStatus" CssClass="paddingRight" runat="server"></asp:Label><br />
        <asp:LinkButton ID="lnkSignout" runat="server" Text="Logout" OnClick="lnkSignout_Click"></asp:LinkButton>
    </div>
    
</div>
<script src="/Scripts/lib/jqueryslidemenu.js" type="text/javascript"></script>--%>
