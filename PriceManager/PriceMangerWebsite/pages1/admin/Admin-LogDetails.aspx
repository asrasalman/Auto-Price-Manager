<%@ Page Title="" Language="C#" MasterPageFile="~/master/Main.master" AutoEventWireup="true"
    CodeFile="Admin-LogDetails.aspx.cs" Inherits="pages_admin_Admin_LogDetails" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="/css/styles.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="partMasterContainer">
        <h1>
            User's Login Log</h1>
        <p>
            Here you can view User's Login Log. You can search user's which are logged in to the system at specific period of time.</p>
        <div class='searchContainer22'>
            <div>
                Search by Date:
                <select id="ddlDates" runat="server" clientidmode="Static">
                    <option value="Today">Today</option>
                    <option value="2">Last 2 Days</option>
                    <option value="3">Last 3 Days</option>
                    <option value="4">Last 4 Days</option>
                    <option value="5">Last 5 Days</option>
                    <option value="6">Last 6 Days</option>
                    <option value="7">Last 7 Days</option>
                    <option value="Custom">Custom</option>
                    <option value="All_Time" selected="selected">All Time</option>
                </select>
            </div>
            <div>
                <input type="text" style="display: none" class="datepicker" id="txtFromDate" placeholder="From Date" runat="server" clientidmode="Static"/>
            </div>
            <div>
                <input type="text" style="display: none" class="datepicker" id="txtToDate" placeholder="To Date" runat="server" clientidmode="Static"/>
            </div>
            <br />
            <div>
                Search:
                <input type="text" class="txtDescSearch" id="txtDescSearch" runat="server" />
            </div>
            <div>
                Choose:
                <select id="ddlSearchType" runat="server" clientidmode="Static">
                    <option value="username">Username</option>
                    <option value="address">Address</option>
                    <option value="browser">Browser</option>
                    <option value="operatingsystem">Operating System</option>
                </select>
            </div>
            <div>
                <asp:Button ID="btnItemsSearch" runat="server" CssClass="button3" Text="Search" 
                    onclick="btnItemsSearch_Click" />
            </div>
        </div>
        <div style="clear: both">
        </div>
        <asp:UpdatePanel ID="upData" runat="server">
            <ContentTemplate>
                <asp:Repeater ID="rptItems" runat="server">
                    <HeaderTemplate>
                        <table id="tblLoginLog" cellpadding="0" cellspacing="0" class="list tblLoginLog tablesorter" style="margin-bottom: 10px;">
                            <thead>
                                <tr>
                                    <th>
                                        User Name
                                    </th>
                                    <th>
                                        Email
                                    </th>
                                    <th>
                                        Login Datetime
                                    </th>
                                    <th>
                                        IP Address
                                    </th>
                                    <th>
                                        Browser
                                    </th>
                                    <th>
                                        Operating System
                                    </th>
                                    <th>
                                        Logout Datetime
                                    </th>
                                </tr>
                            </thead>
                            <tbody>    
                    </HeaderTemplate>
                    <ItemTemplate>
                        <tr class="searchItem">
                            <td>
                                <%# Eval("Full_Name") %>
                            </td>
                            <td>
                                <%# Eval("Email_Address")%>
                            </td>
                            <td>
                                <%# FormatDate(Eval("Login_Date_Time")) %>
                            </td>
                            <td>
                                <%# Eval("User_IP") %>
                            </td>
                            <td>
                                <%# Eval("Browser")%>
                            </td>
                            <td>
                                <%# Eval("Operating_System")%>
                            </td>
                            <td>
                                <%# FormatDate(Eval("Logout_Date_Time"))%>
                            </td>
                        </tr>
                    </ItemTemplate>
                    <FooterTemplate>
                        </tbody> </table>
                    </FooterTemplate>
                </asp:Repeater>
            </ContentTemplate>
            <Triggers>
                <%--<asp:AsyncPostBackTrigger ControlID="btnItemsSearch" EventName="Click" />--%>
            </Triggers>
        </asp:UpdatePanel>
        
    </div>
    <script type="text/javascript" src="/scripts/lib/jquery-1.7.1.min.js"></script>
    <script type="text/javascript" src="/scripts/lib/jquery-ui-1.8.22.custom.min.js"></script>
    <script src="/scripts/lib/jquery.tablesorter.js" type="text/javascript"></script>
    <script type="text/javascript" src="/scripts/custom/LogDetails.js"></script>
    <script type="text/javascript">
        $("#tblLoginLog").tablesorter();
    </script>
</asp:Content>
