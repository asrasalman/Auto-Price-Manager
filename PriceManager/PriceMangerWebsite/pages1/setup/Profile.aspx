<%@ Page Title="" Language="C#" MasterPageFile="~/master/Main.master" AutoEventWireup="true"
    CodeFile="Profile.aspx.cs" Inherits="pages_setup_Profile" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="/css/styles.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class='partMasterContainer'>
        <h1>
            Profile</h1>
        <div id="divUserSelect" runat="server" class="userSelectBox" visible="false">
            Select User:
            <asp:DropDownList ID="ddlUserList" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlUserList_SelectedIndexChanged">
            </asp:DropDownList>
        </div>
        <div id="signup" class="padding">
            <div class="left">
                <asp:Label ID="lblMsg" runat="server" CssClass="profileMessage" Visible="false"></asp:Label><br />
                <asp:Panel ID="pnlSignUp" runat="server">
                    <h2>
                        <span>Personal Info</span></h2>
                    <p>
                        <label>
                            First Name</label>
                        <input type="text" id="f_name" name="f_name" runat="server" class="text_field required" />
                        <asp:RequiredFieldValidator ID="rfvFname" runat="server" CssClass="redText" ControlToValidate="f_name"
                            Text="First name is required." ValidationGroup="signup"></asp:RequiredFieldValidator>
                    </p>
                    <p>
                        <label>
                            Last Name</label>
                        <input type="text" id="l_name" name="l_name" runat="server" class="text_field required" />
                        <asp:RequiredFieldValidator ID="rfvLname" runat="server" CssClass="redText" ControlToValidate="l_name"
                            Text="Last name is required." ValidationGroup="signup"></asp:RequiredFieldValidator>
                    </p>
                    <p>
                        <label>
                            Company</label>
                        <input type="text" id="company" name="company" runat="server" class="text_field required" />
                        <asp:RequiredFieldValidator ID="rfvCompany" runat="server" CssClass="redText" ControlToValidate="company"
                            Text="Company name is required." ValidationGroup="signup"></asp:RequiredFieldValidator>
                    </p>
                    <p>
                        <label>
                            Address 1</label>
                        <input type="text" id="txtAddress1" runat="server" class="text_field required" />
                    </p>
                    <p>
                        <label>
                            Address 2</label>
                        <input type="text" id="txtAddress2" name="company" runat="server" class="text_field required" />
                    </p>
                    <p>
                        <label>
                            Suburb</label>
                        <input type="text" id="txtSuburb" name="company" runat="server" class="text_field required" />
                    </p>
                    <p>
                        <label>
                            Postcode</label>
                        <input type="text" id="txtPostcode" name="company" runat="server" class="text_field required" />
                    </p>
                    <p>
                        <label>
                            State</label>
                        <input type="text" id="txtState" name="company" runat="server" class="text_field required" />
                    </p>
                    <p>
                        <label>
                            Phone</label>
                        <input type="text" id="txtPhone" name="company" runat="server" class="text_field required" />
                    </p>
                    <p>
                        <label>
                            Email</label>
                        <input type="text" id="txtEmail" name="company" runat="server" class="text_field required" />
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" CssClass="redText"
                            ControlToValidate="txtEmail" Text="Company name is required." ValidationGroup="signup"></asp:RequiredFieldValidator>
                    </p>
                    <p>
                        <label>
                            ABN Number</label>
                        <input type="text" id="txtABNNumber" name="company" runat="server" class="text_field required" autocomplete="off" />
                    </p>
                    <p>
                        <label>
                            Flat Rate eParcel Client
                        </label>
                        <input type="checkbox" id="chkFlatRate" runat="server" style="margin-top: 11px;" />
                    </p>
                    <div class="clear">
                    </div>
                    <p>
                        <label>
                            Upload Logo</label>
                        <asp:Image ID="imgLogo" runat="server" Width="100px" Height="70px" Style="margin-left: -12px;" />

                        <asp:FileUpload ID="fuLogo" runat="server" />
                    </p>
                    <div class="clear">
                    </div>
                </asp:Panel>
                <asp:Panel ID="Panel1" runat="server">
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
                <asp:Panel ID="Panel3" runat="server" style="margin-top: 20px; display:none;">
                    <h2>
                        <span>Automate Pricing Setting</span></h2>
                    <p>
                         <label>
                            Time Delay</label>
                         <asp:DropDownList ID="ddlTimeDelay" runat="server" ClientIDMode="Static">
                            <asp:ListItem value="1" Selected="True" Text="Not Specified"></asp:ListItem>
                            <asp:ListItem value="2" Text="12hrs"></asp:ListItem>
                            <asp:ListItem value="3" Text="24hrs"></asp:ListItem>
                            <asp:ListItem value="4" Text="48hrs"></asp:ListItem>
                            <asp:ListItem value="5" Text="Weekly"></asp:ListItem>
                         </asp:DropDownList>
                    </p>
                    <p>
                         <label>
                            Search Only Top </label>
                         <asp:DropDownList ID="ddlSeachOnlyTop" runat="server" ClientIDMode="Static">
                            <asp:ListItem value="1" Selected="True" Text="Not Specified"></asp:ListItem>
                            <asp:ListItem value="5" Text="5 Itmes"></asp:ListItem>
                            <asp:ListItem value="10" Text="10 Items"></asp:ListItem>
                            <asp:ListItem value="15" Text="15 Items"></asp:ListItem>
                            <asp:ListItem value="25" Text="25 Items"></asp:ListItem>
                            <asp:ListItem value="50" Text="50 Items"></asp:ListItem>
                         </asp:DropDownList>
                    </p>
                     <p>
                        <input type="checkbox" id="chkIncludeShipping" runat="server" style="margin-top: 11px;" />
                        <span>Include shipping in overall pricing comparison.</span>                        
                    </p>
                </asp:Panel>
                <asp:Panel ID="Panel2" runat="server" style="margin-top: 20px;">
                    <h2>
                        <span>Notifications</span></h2>
                    <p>
                        <input type="checkbox" id="chkMinimumThresholdNotification" runat="server" style="margin-top: 11px;" />
                        <span>Notify me with email when item's quantity is reached to reorder level.</span>                        
                    </p>
                    <p style="display:none">
                        <input type="checkbox" id="chkFloorLimitNotification" runat="server" style="margin-top: 11px;" />
                        <span>Notify me with email when item's floor limit reached.</span>                        
                    </p>
                    <p>
                        <a class="button "><span>
                            <asp:Button CssClass="button" ID="btnSaveProfile" OnClick="btnSaveProfile_Click"
                                Text="Save Changes" ValidationGroup="signup" runat="server" />
                        </span></a>
                    </p>
                    
                </asp:Panel>
                <asp:Panel ID="pnlPass" runat="server">
                    <h2>
                        <span>Change Password</span></h2>
                    <p>
                        <label>
                            Old Password</label>
                        <input type="password" id="oldPassword" name="oldPassword" runat="server" class="text_field required" autocomplete="off" />
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="oldPassword"
                            CssClass="redText" Text="Old Password is required." ValidationGroup="changePass"></asp:RequiredFieldValidator>
                    </p>
                    <p>
                        <label>
                            Password</label>
                        <input type="password" id="password" name="password" runat="server" class="text_field required" autocomplete="off" />
                        <asp:RequiredFieldValidator ID="rfvPassword" runat="server" ControlToValidate="password"
                            CssClass="redText" Text="Password is required." ValidationGroup="changePass"></asp:RequiredFieldValidator>
                        <asp:CompareValidator ID="cvPass" runat="server" ControlToCompare="password" ControlToValidate="confirmPassword"
                            Text="Password Must Be Same" ValidationGroup="changePass">
                        </asp:CompareValidator>
                    </p>
                    <p>
                        <label>
                            Confirm Password</label>
                        <input type="password" id="confirmPassword" name="confirmPassword" runat="server" autocomplete="off"
                            class="text_field required" />
                        <asp:RequiredFieldValidator ID="RerfvConfPass" runat="server" ControlToValidate="confirmPassword"
                            CssClass="redText" Text="Confirm Password is required." ValidationGroup="changePass"></asp:RequiredFieldValidator>
                    </p>
                    <div class="clear">
                    </div>
                    <p>
                        <a class="button "><span>
                            <asp:Button ID="btnChangePass" runat="server" CssClass="button" OnClick="btnChangePass_Click"
                                Text="Change Password" ValidationGroup="changePass" />
                        </span></a>
                    </p>
                    <h2>
                        <span>Close Account</span></h2>
                    <p>
                        If you want to close your account, you can use the button below. Please note that
                        your paypal subscription will be automatically cancelled and your account will be
                        deleted and you'll be logged out of the system.
                        <br />
                        (As a precationary measure, please login your paypal account and confirm the cancellation
                        after your account is deleted').
                        <br />
                    </p>
                    <asp:LinkButton ID="lbCloseAccount" runat="server" OnClientClick="javascript:return confirm('Are you sure you want to close your account?')"
                        OnClick="lbCloseAccount_Click" CssClass='redLink' Text='Close Account'></asp:LinkButton>
                    <br />
                    <br />
                    <br />
                </asp:Panel>
            </div>
            <div class="clear">
            </div>
           
        </div>
    </div>
    <script type="text/javascript">
        $(document).ready(function () {
            $('.text_field').keyup(ValidateSpecialCharacter);
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
