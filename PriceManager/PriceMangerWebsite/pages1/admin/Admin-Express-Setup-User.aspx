<%@ Page Title="" Language="C#" MasterPageFile="~/master/Main.master" AutoEventWireup="true"
    CodeFile="Admin-Express-Setup-User.aspx.cs" Inherits="pages_admin_Admin_Express_Setup_User" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="/css/styles.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="container generalContainer" style="width: 96%">
        <h1>
            Express Setup Users</h1>
        <p>
            Here you can find those users who has registered with Express Setup to get assistance.
        </p>
        <div class='searchContainer22'>
            <div>
                Search by Name:
                <input type="text" id="txtUserNameSearch" runat="server" clientidmode="Static" />
            </div>
            <div>
                Search by Business Name:
                <input type="text" id="txtUserBusinessName" runat="server" clientidmode="Static" />
            </div>
            <div>
                Search by Status:
                <select id="ddlStatus" name="ddlStatus" style="width:130px">
                    <option value='0'>All</option>
                    <option value='1' selected="selected">Pending</option>
                    <option value='2'>Setup Completed</option>
                    <option value='3'>Cancel</option>

                </select>
            </div>
            <div>
                <input type="button" id="btnSearch" class="button3" value="Search"/>
            </div>
        </div>
        <div style="clear: both">
        </div>
        <div style="margin-bottom: 50px;">
            <a href="#" id="btnComplete" class="btnEdit" style="width:auto;float:right"><img src="/images/round_delete.png" width="16px" height="16px"/>Cancel</a>
            <a href="#" id="btnCancel" class="btnEdit" style="width:auto;float:right"><img src="/images/round_checkmark.png" width="16px" height="16px"/>Setup Completed</a>
            <%--<input type="button" id="btnComplete" class="button3" value="Setup Completed" style="float:right"/>
            <input type="button" id="btnCancel" class="button3 right" value="Cancel" style="float:right"/>--%>
        </div>
        <div>
            <table id="tblUsers" cellpadding="0" cellspacing="0" class="list artistList tablesorter">
            <thead>
                <tr>
                    <th style="text-align:center" class="nosort">
                        <input type="checkbox" class="chkAll" />
                    </th>
                    <th style="text-align:center" class="nosort">
                        Status
                    </th>
                    <th>
                        Package
                    </th>
                    <th>
                        Name
                    </th>
                    <th>
                        Business Name
                    </th>
                    <th>
                        eBay Store URL
                    </th>
                    <th>
                        Daily Orders
                    </th>
                    <th class="nosort">
                        Use PS
                    </th>
                    <th>
                        Login Email PS
                    </th>
                    <th>
                        Time Using eParcel
                    </th>
                    <th>
                        Phone
                    </th>
                    <th>
                        Best Time Call
                    </th>
                    <th>
                        Email
                    </th>
                </tr>
            </thead>
            <tbody>
            </tbody>
        </table>
        </div>
    </div>
    <script src="/Scripts/lib/jquery-ui-1.8.22.custom.min.js" type="text/javascript"></script>
    <script src="/scripts/lib/jquery.tmpl.min.js" type="text/javascript"></script>
    <script src="/scripts/lib/jquery.tablesorter.js" type="text/javascript"></script>
    <script src="/scripts/lib/oneSimpleTablePaging-1.0.js" type="text/javascript"></script>
    <script src="/scripts/custom/ExpressSetupUsers.js" type="text/javascript"></script>
    <script id="tblUsersTemplate" type="text/x-jQuery-tmpl">
        <tr class="searchItem">
            <td>
                <input type="checkbox" class="chkOne" />
            </td>
            <td style="text-align:center;">
                <img alt="" class="status" src="${GetStatusImage(Status_Code)}" width="24" height="24" />
            </td>
            <td>
                <span class='packageName'>
                    ${Package_Name}</span>
                    <input type="hidden" class="hfPackageId" value='${Package_Id}' />
            </td>
            <td>
                <span class='fullName'>
                    ${Full_Name}</span>
                <input type="hidden" class="hfExpressSetupUserCode" value='${Express_Setup_User_Code}' />
            </td>
            <td>
                <span class='businessName'>
                    ${Business_Name}</span>
            </td>
            <td>
                <a class="ebayStoreURL" href='${Ebay_Store_URL}' target="_blank">${Ebay_Store_URL}</a>
            </td>
            <td>
                <span class='dailyOrders'>
                    ${Daily_Orders}</span>
            </td>
            <td style="text-align:center">
                <img alt="" class="isRegisteredPS" src="{{if Is_Registered_PS == true }} /images/tick-medium2.png {{else}} /images/close.png {{/if}}" width="24" height="24" />
            </td>
            <td>
                <span class='loginEmailPS'>
                    ${Login_Email_PS}</span>
            </td>
            <td>
                <span class='timeUsingEParcel'>
                    ${Time_Using_EParcel}</span>
            </td>
            <td>
                <span class='phoneNumber'>
                    ${Phone_Number}</span>
            </td>
            <td>
                <span class='bestTimeCall'>
                    ${Best_Time_Call}</span>
            </td>
            <td>
                <span class='emailAddress'>
                    ${Email_Address}</span>
            </td>
        </tr>
    </script>
    <script type="text/javascript">
        $(document).ready(function () {
            InitializeExpressSetupUsers();
        });
    </script>
</asp:Content>
