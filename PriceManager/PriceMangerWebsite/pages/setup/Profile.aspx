<%@ Page Title="" Language="C#" MasterPageFile="~/master/Main.master" AutoEventWireup="true"
    CodeFile="Profile.aspx.cs" Inherits="pages_setup_Profile" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="rt-containerInner">
        <div class="rt-grid-12 ">
            <div class="rt-block">
                <div class='partMasterContainer'>
                    <div class="componentheading">
                        <h2>
                            Profile</h2>
                    </div>
                    <div id="divUserSelect" runat="server" class="userSelectBox" visible="false">
                        Select User:
                        <asp:DropDownList ID="ddlUserList" runat="server" AutoPostBack="true" Width="200px"
                            OnSelectedIndexChanged="ddlUserList_SelectedIndexChanged">
                        </asp:DropDownList>
                        <br />
                    </div>
                    <div id="divPackage">
                        <div class="componentheading">
                            <h3>
                                Plan Details
                            </h3>
                        </div>
                        <br />
                        <p>
                            <label>
                                Current Plan</label>
                            <span class="spnPackage">
                                <asp:Literal ID="txtPackageName" runat="server"></asp:Literal></span>
                        </p>
                        <p>
                            <label>
                            </label>
                            <asp:Button ID="btnUpgradePaypalBussiness" class="button" ClientIDMode="Static" Text="Upgrade To Business"
                                OnClick="btnUpgradePaypalBussiness_Click" Visible="false" runat="server" />

                            <asp:Button ID="btnUpgradePaypal" class="button" ClientIDMode="Static" Text="Upgrade To Pro"
                                OnClick="btnUpgradePaypal_Click" Visible="false" runat="server" />
                            

                             <asp:Button ID="btnUpgradePaypalProPlus" class="button" ClientIDMode="Static" Text="Upgrade To Pro +" OnClick="btnUpgradePaypalProPlus_Click"
                                Visible="false" runat="server" />
                        </p>
                    </div>
                    <div id="signup" class="padding">
                        <div class="left">
                            <asp:Label ID="lblMsg" runat="server" CssClass="profileMessage" Visible="false"></asp:Label><br />
                            <asp:Panel ID="pnlSignUp" runat="server" ClientIDMode="Static">
                                <div class="componentheading">
                                    <h3>
                                        Personal Info</h3>
                                </div>
                                <br />
                                <p>
                                    <label>
                                        First Name</label>
                                    <input type="text" id="f_name" name="f_name" runat="server" class="inputbox required" />
                                    <asp:RequiredFieldValidator ID="rfvFname" runat="server" CssClass="redText" ControlToValidate="f_name"
                                        Text="First name is required." ValidationGroup="signup" Display="Dynamic"></asp:RequiredFieldValidator>
                                </p>
                                <p>
                                    <label>
                                        Last Name</label>
                                    <input type="text" id="l_name" name="l_name" runat="server" class="inputbox required" />
                                    <asp:RequiredFieldValidator ID="rfvLname" runat="server" CssClass="redText" ControlToValidate="l_name"
                                        Text="Last name is required." ValidationGroup="signup" Display="Dynamic"></asp:RequiredFieldValidator>
                                </p>
                                <p>
                                    <label>
                                        Company</label>
                                    <input type="text" id="company" name="company" runat="server" class="inputbox required" />
                                    <asp:RequiredFieldValidator ID="rfvCompany" runat="server" CssClass="redText" ControlToValidate="company"
                                        Text="Company name is required." ValidationGroup="signup" Display="Dynamic"></asp:RequiredFieldValidator>
                                </p>
                                <p style="display: none">
                                    <label>
                                    </label>
                                    <asp:DropDownList ID="ddlCurrency" runat="server" Style="width: 250px;" Visible="false">
                                    </asp:DropDownList>
                                   <%-- <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" CssClass="redText"
                                        ControlToValidate="ddlCurrency" Text="Currency is required." ValidationGroup="signup"
                                        InitialValue="0" Display="Dynamic"></asp:RequiredFieldValidator>--%>
                                </p>
                                <p>
                                    <label>
                                        Address 1</label>
                                    <input type="text" id="txtAddress1" runat="server" class="inputbox required" />
                                </p>
                                <p>
                                    <label>
                                        Address 2</label>
                                    <input type="text" id="txtAddress2" name="company" runat="server" class="inputbox required" />
                                </p>
                                <p style="display: none">
                                    <label>
                                        Suburb</label>
                                    <input type="text" id="txtSuburb" name="company" runat="server" class="inputbox required" />
                                </p>
                                <p>
                                    <label>
                                        Postcode</label>
                                    <input type="text" id="txtPostcode" name="company" runat="server" class="inputbox required" />
                                </p>
                                <p>
                                    <label>
                                        State</label>
                                    <input type="text" id="txtState" name="company" runat="server" class="inputbox required" />
                                </p>
                                <p>
                                    <label>
                                        Country</label>
                                    <asp:DropDownList ID="ddlCountry" runat="server" Style="width: 250px;">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" CssClass="redText"
                                        ControlToValidate="ddlCountry" Text="Country is required." ValidationGroup="signup"
                                        InitialValue="0" Display="Dynamic"></asp:RequiredFieldValidator>
                                </p>
                                <p>
                                    <label>
                                        Phone</label>
                                    <input type="text" id="txtPhone" name="company" runat="server" class="inputbox required" />
                                </p>
                                <p>
                                    <label>
                                        Email</label>
                                    <input type="text" id="txtEmail" name="company" runat="server" class="inputbox required"
                                        disabled="disabled" />
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" CssClass="redText"
                                        ControlToValidate="txtEmail" Text="Company name is required." ValidationGroup="signup"
                                        Display="Dynamic"></asp:RequiredFieldValidator>
                                </p>
                                <p style="display: none">
                                    <label>
                                        ABN Number</label>
                                    <input type="text" id="txtABNNumber" name="company" runat="server" class="inputbox required"
                                        autocomplete="off" />
                                </p>
                                <p style="display: none">
                                    <label>
                                        Flat Rate eParcel Client
                                    </label>
                                    <input type="checkbox" id="chkFlatRate" runat="server" style="margin-top: 11px;" />
                                </p>
                                <div class="clear">
                                </div>
                                <p style="display: none">
                                    <label>
                                        Upload Logo</label>
                                    <asp:Image ID="imgLogo" runat="server" Width="100px" Height="70px" Style="margin-left: -12px;" />
                                    <asp:FileUpload ID="fuLogo" runat="server" />
                                </p>
                            </asp:Panel>
                            <asp:Panel ID="Panel1" runat="server" Visible="false">
                                <h2>
                                    <span>E-Parcel Charge Codes</span></h2>
                                <table cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td colspan="8" class='codeGroup'>
                                            Domestic Codes:
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 144px;">
                                            Standard/Regular:
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlStandardCode" runat="server" CssClass='codeList'>
                                            </asp:DropDownList>
                                        </td>
                                        <td style="width: 149px;">
                                            Express:
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlExpressCode" runat="server" CssClass='codeList'>
                                            </asp:DropDownList>
                                        </td>
                                        <td style="width: 135px;">
                                        </td>
                                        <td>
                                        </td>
                                        <td style="width: 159px;">
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="8" class='codeGroup'>
                                            International Codes:
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Express International:
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlExpressIntCode" runat="server" CssClass='codeList'>
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                            Standard International:
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlStandardIntCode" runat="server" CssClass='codeList'>
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                            Air Mail International:
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlAirMailIntCode" runat="server" CssClass='codeList'>
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                            AusPost Registered Post International Parcel:
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlAusPostRegIntCode" runat="server" CssClass='codeList'>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                            <asp:Panel ID="divPriceSettings" ClientIDMode="Static" runat="server" Style="margin-top: 20px;"
                                Visible="true">
                                <div class="componentheading">
                                    <h3>
                                        <span>Automate Pricing Setting</span></h3>
                                </div>
                                <br />
                                <p>
                                    <label>
                                        Time Delay</label>
                                    <asp:DropDownList ID="ddlTimeDelay" runat="server" ClientIDMode="Static" Style="width: 250px;">
                                        <asp:ListItem Value="1" Selected="True" Text="Not Specified"></asp:ListItem>
                                        <asp:ListItem Value="6" Text="6hrs"></asp:ListItem>
                                        <asp:ListItem Value="2" Text="12hrs"></asp:ListItem>
                                        <asp:ListItem Value="3" Text="24hrs"></asp:ListItem>
                                        <asp:ListItem Value="4" Text="48hrs"></asp:ListItem>
                                        <asp:ListItem Value="5" Text="Weekly"></asp:ListItem>
                                    </asp:DropDownList>
                                </p>
                                <p>
                                    <label>
                                        Search Only Top
                                    </label>
                                    <asp:DropDownList ID="ddlSeachOnlyTop" runat="server" ClientIDMode="Static" Style="width: 250px;">
                                        <asp:ListItem Value="0" Selected="True" Text="Not Specified"></asp:ListItem>
                                        <asp:ListItem Value="1" Text="1 Item"></asp:ListItem>
                                        <asp:ListItem Value="2" Text="2 Items"></asp:ListItem>
                                        <asp:ListItem Value="3" Text="3 Items"></asp:ListItem>
                                        <asp:ListItem Value="4" Text="4 Items"></asp:ListItem>
                                        <asp:ListItem Value="5" Text="5 Items"></asp:ListItem>
                                        <asp:ListItem Value="10" Text="10 Items"></asp:ListItem>
                                        <asp:ListItem Value="15" Text="15 Items"></asp:ListItem>
                                        <asp:ListItem Value="25" Text="25 Items"></asp:ListItem>
                                        <asp:ListItem Value="50" Text="50 Items"></asp:ListItem>
                                    </asp:DropDownList>
                                </p>
                                <p>
                                    <label>
                                    </label>
                                    <input type="checkbox" id="chkIncludeShipping" runat="server" style="margin-top: 11px;" />
                                    <span>Include shipping in overall pricing comparison.</span>
                                </p>
                                <p>
                                    <label>
                                    </label>
                                    <input type="checkbox" id="chkFloorLimitNotification" runat="server" style="margin-top: 11px;" />
                                    <span>Notify me with email when item's floor/Ceiling limit reached.</span>
                                </p>
                            </asp:Panel>
                            <asp:Panel ClientIDMode="Static" runat="server" Style="margin-top: 20px;" Visible="true"
                                ID="PnlAutomateTitle">
                                <div class="componentheading">
                                    <h3>
                                        <span>Automate Title Setting</span></h3>
                                </div>
                                <br />
                                <p>
                                    <label>
                                        Titles Displayed</label>
                                    <asp:DropDownList ID="ddlTitleDisplayed" runat="server" ClientIDMode="Static" Style="width: 250px;">
                                        <asp:ListItem Value="1" Text="Show 2 Title Fields"></asp:ListItem>
                                        <asp:ListItem Value="2" Text="Show 3 Title Fields"></asp:ListItem>
                                        <asp:ListItem Value="3" Text="Show 4 Title Fields"></asp:ListItem>
                                        <asp:ListItem Value="4" Selected="True" Text="Show 5 Title Fields"></asp:ListItem>
                                    </asp:DropDownList>
                                </p>
                                <p>
                                    <label>
                                    </label>
                                    <input type="checkbox" id="chkNotifyTitleLocked" runat="server" style="margin-top: 11px;" />
                                    <span>Notify me with email when Title is locked.</span>
                                </p>
                                <div class="clear">
                                </div>
                                <p>
                                    <label>
                                    </label>
                                    <asp:Button CssClass="button" ID="btnSaveProfile" OnClick="btnSaveProfile_Click"
                                        Text="Save Changes" ValidationGroup="signup" runat="server" />
                                </p>

                            </asp:Panel>
                            <asp:Panel ID="Panel2" runat="server" Style="margin-top: 20px;" Visible="false">
                                <h2>
                                    <span>Notifications</span></h2>
                                <p>
                                    <input type="checkbox" id="chkMinimumThresholdNotification" runat="server" style="margin-top: 11px;" />
                                    <span>Notify me with email when item's quantity is reached to reorder level.</span>
                                </p>
                                <p>
                                    <a class="button "><span></span></a>
                                </p>
                            </asp:Panel>
                            <asp:Panel ID="pnlResetPass" ClientIDMode="Static" runat="server">
                                <div class="componentheading">
                                    <h3>
                                        Change Password</h3>
                                </div>
                                <br />
                                <p>
                                    <label>
                                        Old Password</label>
                                    <input type="password" id="oldPassword" name="oldPassword" runat="server" class="inputbox required"
                                        autocomplete="off" />
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="oldPassword"
                                        CssClass="redText" Text="Old Password is required." ValidationGroup="changePass"
                                        Display="Dynamic"></asp:RequiredFieldValidator>
                                </p>
                                <p>
                                    <label>
                                        Password</label>
                                    <input type="password" id="password" name="password" runat="server" class="inputbox required"
                                        autocomplete="off" />
                                    <asp:RequiredFieldValidator ID="rfvPassword" runat="server" ControlToValidate="password"
                                        CssClass="redText" Text="Password is required." ValidationGroup="changePass"></asp:RequiredFieldValidator>
                                    <asp:CompareValidator ID="cvPass" runat="server" ControlToCompare="password" ControlToValidate="confirmPassword"
                                        Text="Password Must Be Same" ValidationGroup="changePass" Display="Dynamic">
                                    </asp:CompareValidator>
                                </p>
                                <p>
                                    <label>
                                        Confirm Password</label>
                                    <input type="password" id="confirmPassword" name="confirmPassword" runat="server"
                                        autocomplete="off" class="inputbox required" />
                                    <asp:RequiredFieldValidator ID="RerfvConfPass" runat="server" ControlToValidate="confirmPassword"
                                        CssClass="redText" Text="Confirm Password is required." ValidationGroup="changePass"
                                        Display="Dynamic"></asp:RequiredFieldValidator>
                                </p>
                                <div class="clear">
                                </div>
                                <p>
                                    <label>
                                    </label>
                                    <asp:Button ID="btnChangePass" runat="server" CssClass="button" OnClick="btnChangePass_Click"
                                        Text="Change Password" ValidationGroup="changePass" ClientIDMode="Static" />
                                </p>
                                <div class="searchContainer22" runat="server" visible="false">
                                    <div class="componentheading">
                                        <h3>
                                            Close Account
                                        </h3>
                                    </div>
                                    <p>
                                        If you want to close your account, you can use the button below. Please note that
                                        your paypal subscription will be automatically cancelled and your account will be
                                        deleted and you'll be logged out of the system.
                                        <br />
                                        (As a precationary measure, please login your paypal account and confirm the cancellation
                                        after your account is deleted').
                                        <br />
                                        <asp:LinkButton ID="lbCloseAccount" runat="server" OnClientClick="javascript:return confirm('Are you sure you want to close your account?')"
                                            OnClick="lbCloseAccount_Click" CssClass='button' Text='Close My Account' Style="float: right"></asp:LinkButton>
                                        <br />
                                    </p>
                                </div>
                                <br />
                                <br />
                            </asp:Panel>
                        </div>
                        <div class="clear">
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <script src="/Scripts/lib/jquery-1.7.1.min.js" type="text/javascript"></script>
    <script src="/Scripts/lib/jquery-ui-1.8.22.custom.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $('.inputbox').keyup(ValidateSpecialCharacter);
            $('img.what-is-this').mouseover(function () {
                $("div.callout").show();
            });
            $('img.what-is-this').mouseleave(function () {
                $("div.callout").hide();
            });
        });

        function ValidateSpecialCharacter() {
            var text = $(this).val();
            text = text.replace(/\//g, '-');
            text = text.replace(/\\/g, '-');
            text = text.replace(/\,/g, '-');
            text = text.replace(/\"/g, '');
            text = text.replace(/\'/g, '');
            $(this).val(text);
        }
    </script>
</asp:Content>
