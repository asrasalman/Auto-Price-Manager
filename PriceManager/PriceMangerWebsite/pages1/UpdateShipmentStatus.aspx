<%@ Page Title="" Language="C#" MasterPageFile="~/master/Main.master" AutoEventWireup="true"
    CodeFile="UpdateShipmentStatus.aspx.cs" Inherits="pages_UpdateShipmentStatus" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="/css/styles.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:HiddenField ID="hfSessionID" runat="server" />
    <div class='uploadFile'>
        <h1>
            Update Shipment Status :
        </h1>
        <p>
            Upload the E-Parcel output manifest using the file uploader below, and then click
            on "Upload & Process" button to update all your transactions with "Shipped" / "Fulfilled" 
            status in their respective E-Commerce accounts, along with the Tracking number passed by E-Parcel.
        </p>
        <div class='uploadManifestContainer'>
            <span class='uploadFileHeading'>Upload Manifest File (csv)</span><br />
            <asp:FileUpload ID="FileUpload1" runat="server" />
        </div>
        <a class="button" style="opacity: 0.8;">
            <asp:Button ID="btnUploadManifest" CssClass="uploadManifest" runat="server" Text="Upload Manifest & Update Shipment Status"
                OnClick="btnUploadManifest_Click" /></a>
        <asp:Label ID="lblAPIIssuesUpload" runat="server" ForeColor="Maroon" Visible="false"></asp:Label>
        <br />
        <asp:Label ID="lblUploadStatus" runat="server"></asp:Label>
        <br />
        <asp:Label ID="lblSkippedItems" runat="server"></asp:Label>
    </div>
    <br />
    <div class="divNoSettingFound" style="display: none">
        <h2>
            Connect Your E-Commerce Account(s)</h2>
        <p>
            You need atleast one E-Commerce account connected to Parcel Solutions before you
            can use the features.
            <br />
            <br />
            <input type="hidden" value="0" id="hdfSettings" runat="server" />
            <a href="ShopConnect.aspx" class='button3'>Take me there</a>
        </p>
    </div>
    <span class='fileName'></span>
    <script src="/Scripts/lib/jquery-ui-1.8.22.custom.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        function pageLoad() {

            if ($('#' + '<%=hdfSettings.ClientID%>').val() == "0") {

                $('.divNoSettingFound').dialog({ width: '500px', modal: true, title: 'EBay Settings' }).parent().find('.ui-dialog-titlebar-close').hide();
                $('.ui-dialog').appendTo('form:first');
                $('.ui-dialog-titlebar').remove();
            }
            SetTriggers();
        }

        function SetTriggers() {
            $("input:file").change(function () {
                var fileName = $(this).val();
                $(".fileName").text(fileName);
            });

            $('.uploadManifest').click(ValidateFile);
            $('#chkSelectAll').change(SetCheckboxes);
            $('.btnProcessSelectedItems').click(ValidateSelection);
            SetColors();
        }

        function ValidateFile(event) {
            var fileName = $('.fileName').text();
            if (fileName == '') {
                alert('No File Selected');
                event.preventDefault();
                return false;
            }
        }

        function SetCheckboxes() {
            $('#tblParcelItems tbody').find('input[type="checkbox"]').prop('checked', $(this).prop('checked'));
        }

        function ValidateSelection() {
            var count = $('#tblParcelItems tbody').find('input[type="checkbox"]:checked').length;

            if (count == 0) {
                alert('Please select at-least one item to process for e-parcel file');
                return false;
            }
        }

        function SetColors() {
            $('.shippingMethod:contains("Express")').closest('tr').addClass('backred');
            $('.shippingMethod:contains("AU_Freight")').closest('tr').addClass('backblue');
        }

    </script>
</asp:Content>
