<%@ Page Title="" Language="C#" MasterPageFile="~/master/Main.master" AutoEventWireup="true"
    CodeFile="PartMaster.aspx.cs" Inherits="pages_masters_PartMaster" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="/css/styles.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="partMasterContainer">
        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                <ContentTemplate>
                    <asp:Button ID="btnDownloadItems" runat="server" Text="Download Active Ebay Listings"
                        CssClass="button3" Style="float: right" 
                        onclick="btnDownloadItems_Click" />
                </ContentTemplate>
        </asp:UpdatePanel>
        <h1>
            Part Master</h1>
        <p>
            Here you should define all the parts that you are selling with their dimensions,
            as these values are required in E-Parcel file. Please use the same Custom Label
            as you have defined in Ebay / Shopify.
        </p>
        <div class='searchContainer22'>
            <div>
                Search by Custom Label:
                <input type="text" class="txtCustomLabelSearch" id="txtCustomLabelSearch" runat="server" />
            </div>
            <div>
                Search by Description:
                <input type="text" class="txtDescSearch" id="txtDescSearch" runat="server" />
            </div>
            <div>
                <asp:Button ID="btnItemsSearch" runat="server" CssClass="button3" Text="Search" OnClick="btnItemsSearch_Click" />
            </div>
        </div>
        <div style='float: right'>
            <a class='addItems addSmall addRight editFunction'>Add Items</a> &nbsp;
            <asp:LinkButton ID="btnDeleteBulkItems" runat="server" Text="Delete Items" OnClick="btnDeleteBulkItems_Click"
                CssClass="deleteItemsBulk deleteSmall deleteRight editFunction" />
            
        </div>
        <div style="clear: both">
        </div>
        <asp:UpdatePanel ID="upData" runat="server">
            <ContentTemplate>
                <asp:Repeater ID="rptItems" runat="server">
                    <HeaderTemplate>
                        <table cellpadding="0" cellspacing="0" class="list artistList tblItems" style="margin-bottom: 10px;">
                            <thead>
                                <tr>
                                    <th>
                                        <input type="checkbox" id="chkSelectAll" />
                                    </th>
                                    <th>
                                        Custom Label
                                    </th>
                                    <th>
                                        Description
                                    </th>
                                    <th>
                                        Weight
                                    </th>
                                    <th>
                                        Length
                                    </th>
                                    <th>
                                        Width
                                    </th>
                                    <th>
                                        Height
                                    </th>
                                    <th style="display:none">
                                        InitialQty
                                    </th>
                                    <th>
                                        Total Qty
                                    </th>
                                    <th>
                                        Reorder Level
                                    </th>
                                    <th style="width: 70px; text-align: center;" class='editFunction'>
                                        Edit
                                    </th>
                                    <th style="width: 70px; text-align: center;" class='editFunction'>
                                        Delete
                                    </th>
                                    <th style="width: 70px; text-align: center;" class='adjustFunction'>
                                        Adjustment
                                    </th>
                                </tr>
                            </thead>
                            <tbody>
                                <tr class='emptyRow'>
                                    <td colspan='9'>
                                        List is empty. Please start adding from importing / manually entering part items.
                                    </td>
                                </tr>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <tr class="searchItem">
                            <td>
                                <asp:CheckBox ID="chkSelect" runat="server" CssClass="chkSelect" />
                            </td>
                            <td>
                                <asp:Label ID="lblCustomLabel" runat="server" CssClass="customLabel" Text='<%# Eval("customLabel") %>'></asp:Label>
                                <input type="hidden" class="hfListItemID" runat="server" id="hfListItemID" value='<%# Eval("ID") %>' />
                            </td>
                            <td>
                                <asp:Label ID="lblDesc" runat="server" CssClass="description" Text='<%# Eval("description") %>'></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblWeight" runat="server" CssClass="weight" Text='<%# Eval("weight", "{00:f2}")%>'></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblLength" runat="server" CssClass="length" Text='<%# Eval("length", "{00:f2}")%>'></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblWidth" runat="server" CssClass="width" Text='<%# Eval("width", "{00:f2}")%>'></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblHeight" runat="server" CssClass="height" Text='<%# Eval("height", "{00:f2}")%>'></asp:Label>
                            </td>
                            <td style="display:none">
                                <asp:Label ID="lblInitialQty" runat="server" CssClass="initialQty" Text='<%# Eval("Opening_Quantity")%>'></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblBalanceQty" runat="server" CssClass="balanceQty" Text='<%# Eval("Balance_Quantity")%>'></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblMinThreshold" runat="server" CssClass="minimumThreshold" Text='<%# Eval("Minimum_Threshold")%>'></asp:Label>
                            </td>
                            <td style="text-align: center;" class='editFunction'>
                                <span class='edit editItems'></span>
                            </td>
                            <td style="text-align: center;" class='editFunction'>
                                <asp:ImageButton ID="btnDeleteItems" runat="server" ImageUrl="~/images/cancel.png"
                                    OnClick="btnDeleteItems_Click" CssClass="deleteItems" />
                            </td>
                            <td style="text-align: center;" class='adjustFunction'>
                                <span class='adjust adjustItems'></span>
                            </td>
                        </tr>
                    </ItemTemplate>
                    <FooterTemplate>
                        </tbody> </table>
                    </FooterTemplate>
                </asp:Repeater>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnItemsSearch" EventName="Click" />
            </Triggers>
        </asp:UpdatePanel>
        <a class="button" style="opacity: 0.8;">
            <asp:Button ID="btnProcessSelectedItems" CssClass="btnProcessSelectedItems" runat="server"
                Text="Download Selected Items" OnClick="btnProcessSelectedItems_Click" /></a>
        <span class='fileName'></span>
        <div style="margin-top: 20px;">
            <h1>
                Update your Part Master:
            </h1>
            <p>
                Upload the Part Master using the file uploader below, and then click on "Upload".
            </p>
            <div class='uploadManifestContainer'>
                <span class='uploadFileHeading'>Upload Manifest File (csv)</span><br />
                <asp:FileUpload ID="FileUpload1" runat="server" />
            </div>
            <a class="button" style="opacity: 0.8;">
                <asp:Button ID="btnUploadManifest" CssClass="uploadManifest" runat="server" Text="Upload Part Master"
                    OnClick="btnUploadManifest_Click" /></a>
            <asp:Label ID="lblAPIIssuesUpload" runat="server" ForeColor="Maroon" Visible="false"></asp:Label>
            <br />
            <asp:Label ID="lblUploadStatus" runat="server"></asp:Label>
            <br />
            <asp:Label ID="lblSkippedItems" runat="server"></asp:Label>
        </div>
    </div>
    <span class="manageItemsReference"></span>
    <div class='manageItems managePopupGeneral'>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <table>
                    <tr>
                        <td style="width: 110px;">
                            Custom Label
                        </td>
                        <td>
                            <input type="text" id='txtCustomLabel' runat="server" class="txtCustomLabel" style="width: 250px;" />
                            <asp:RequiredFieldValidator ID="rfvcustomLabel" runat="server" ValidationGroup="addItems"
                                ErrorMessage="*" ControlToValidate="txtCustomLabel"></asp:RequiredFieldValidator>
                            <input type="hidden" id="hfItemID" runat="server" class="hfItemID" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Description
                        </td>
                        <td>
                            <input type="text" id="txtDesc" runat="server" class="txtDesc" style="width: 250px" />
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 110px;">
                            Weight
                        </td>
                        <td>
                            <input type="text" id='txtWeight' runat="server" class="txtWeight numeric" style="width: 250px;" />
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 110px;">
                            Length
                        </td>
                        <td>
                            <input type="text" id='txtLength' runat="server" class="txtLength numeric" style="width: 250px;" />
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 110px;">
                            Width
                        </td>
                        <td>
                            <input type="text" id='txtWidth' runat="server" class="txtWidth numeric" style="width: 250px;" />
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 110px;">
                            Height
                        </td>
                        <td>
                            <input type="text" id='txtHeight' runat="server" class="txtHeight numeric" style="width: 250px;" />
                        </td>
                    </tr>
                    <tr class="trInitialQty">
                        <td style="width: 110px;">
                            Initial Qty
                        </td>
                        <td>
                            <input type="text" id='txtInitialQty' runat="server" class="txtInitialQty numeric" onchange="makeInteger(this);"
                                style="width: 250px;" />
                            <asp:RegularExpressionValidator runat="server" ID="revtxtInitialQty" ControlToValidate="txtInitialQty"
                                ValidationExpression="\d+" ErrorMessage="Enter in number" Display="Dynamic"></asp:RegularExpressionValidator>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 110px;">
                            Reorder Level
                        </td>
                        <td>
                            <input type="text" id='txtMinimumThreshold' runat="server" class="txtMinimumThreshold numeric"  onchange="makeInteger(this);"
                                style="width: 250px;" />
                            <asp:RegularExpressionValidator runat="server" ID="RegularExpressionValidator1" ControlToValidate="txtMinimumThreshold"
                                ValidationExpression="\d+" ErrorMessage="Enter in number" Display="Dynamic"></asp:RegularExpressionValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td>
                            <asp:Button ID="btnSave" Text="Save Items" runat="server" CssClass="button1 saveItems"
                                ValidationGroup="addItems" OnClick="btnSave_Click" />
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <div class='adjustItemsmodal managePopupGeneral'>
        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
            <ContentTemplate>
                <table>
                    <tr>
                        <td colspan="2">
                            <span id="spanCurrentQty" class="pendingShipmentCount"></span>
                            <span id="spanNewQty" class="pendingShipmentCount" style="background-color:Green"></span>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 110px;">
                            Adjustment Type
                        </td>
                        <td>
                            <input type="radio" id="rdbAddition" runat="server" clientidmode="Static" value="addition" onclick="calculate($('#txtAdjustQty'));" name="adjustmentType" checked="true"/> Additon
                            <input type="radio" id="rdbRemoval" runat="server" clientidmode="Static" value="removal" onclick="calculate($('#txtAdjustQty'));" name="adjustmentType" style="margin-left:10px"/>Removal
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 110px;">
                            Adjustment Qty
                        </td>
                        <td>
                            <input type="text" id='txtAdjustQty' clientidmode="Static" onchange="calculate(this);" runat="server" class="txtAdjustQty numeric"
                                style="width: 250px;" />
                            <span id="errortxtAdjustQty" class="redText red" style="color:Red; font-size:smaller"></span>
                            <input type="hidden" id="hfAdjustItemID" runat="server" clientidmode="Static" class="hfAdjustItemID" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Reason For Adjustment (optional)
                        </td>
                        <td>
                            <textarea id="txtAdjustNarration" runat="server" class="txtAdjustNarration" clientidmode="Static"
                                cols="5" rows="5" style="width: 250px; height: 100px"></textarea>
                            <span id="errortxtAdjustNarration" class="redText red" style="color:Red; font-size:smaller"></span>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td>
                            <asp:Button ID="btnSubmit" runat="server"  Text="Save" OnClientClick="forceClick();" CssClass="button1 btnAdjust"/>
                            
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
        <asp:Button ID="btnAdjust" ClientIDMode="Static" Text="Adjust" runat="server" OnClick="btnAdjust_Click" style="display:none"
                                CssClass="button1 btnAdjust"/>
    </div>
    <script src="/Scripts/lib/jquery-ui-1.8.22.custom.min.js" type="text/javascript"></script>
    <script src="/scripts/custom/Masters.js" type="text/javascript"></script>
    <script src="/scripts/lib/jquery.numeric.js" type="text/javascript"></script>
    <script type="text/javascript">
        function pageLoad() {
            SetTriggersItems();

            if ($('.searchItem').length == 0) {
                $('.emptyRow').show();
            }
            else
                $('.emptyRow').hide();
        }
        function makeInteger(obj){
            var value = $(obj).val();
            if(value){
                if(value == '-')
                    $(obj).val("");
                else if(value.indexOf(".") > 0){
                    $(obj).val(value.substring(0, value.indexOf(".")));
                }
            }
        }
        function calculate(obj){
            makeInteger(obj);
            if($(obj).val())
            {
                $(obj).val($(obj).val().replace("-", ""));
                var total = 0;
                if($("#rdbAddition").is(":checked"))
                    total = Number($("#spanCurrentQty").text().split(":")[1]) + Number($(obj).val());
                else
                    total = Number($("#spanCurrentQty").text().split(":")[1]) - Number($(obj).val());
                
                $("#spanNewQty").text("New Qty : " + total).show();
            }
            else{
                $("#spanNewQty").text("").hide();
            }
            
        }
        function forceClick() {
           
            var status = true;
            if(!$("#txtAdjustQty").val())
            {
                $("#errortxtAdjustQty").text("*");
                status = false;
            }
            else 
            {
                if(isNaN($("#txtAdjustQty").val()) || Number($("#txtAdjustQty").val()) == 0 || Number($("#txtAdjustQty").val()) == 'undefined'){
                    $("#errortxtAdjustQty").text("Number requried");
                    status = false;
                }
                else
                   $("#errortxtAdjustQty").text("");
            }
            
            
            if(status)
                <%= ClientScript.GetPostBackEventReference(btnAdjust, string.Empty) %>;
        }
    </script>
</asp:Content>
