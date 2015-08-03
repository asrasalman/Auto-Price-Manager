<%@ Page Title="" Language="C#" MasterPageFile="~/master/Main.master" AutoEventWireup="true"
    CodeFile="GettingStarted.aspx.cs" Inherits="pages_GettingStarted" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="/css/styles.css" rel="stylesheet" type="text/css" />
    <link href="/scripts/videojs/video-js.min.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class='partMasterContainer'>
        <h1>
            Hello and Welcome to Parcel Solutions.</h1>
        <br />
        <iframe src="http://player.vimeo.com/video/56684599" width="500" height="375" frameborder="0" webkitAllowFullScreen mozallowfullscreen allowFullScreen></iframe>
        <p id='gettingStarted'>
            <p>
                <h2>
                    1. Set up Profile</h2>
                Let’s get started on setting up your Parcel Solutions Account. Firstly you need
                to setup your user profile. These details will appear on the invoices you send out
                to your customers. Any details you do not want to appear on the invoice can simply
                be left blank. Choosing the “Profile Menu” item at top of the page, fill in the
                fields as required and click save changes at the bottom of the page.
            </p>
            <p>
                <h2>
                    2. Part Master</h2>
                The part master menu enables you to automatically add your weights and measures
                to your ebay parts list without any further manual input. You can add your items
                one at a time or simply add your entire parts database in one go. The part master
                list contains your part numbers, descriptions and the weight, length, height and
                width dimensions for your entire inventory, if you choose to use it. It can be a
                huge time saver if you are sending volumes of parcels daily. The parts list needs
                to be uploaded in a comma delimited or CSV format. In order to get the full use
                out of the part master function it is necessary to use the “custom label” function
                in the “ebay Selling Manager”. Each of your ebay items needs a unique ID or “custom
                label”. In your “ebay selling manager pro” you need to go to “Active listings”.
                You will see a small link to “Customise” make sure that the check box for “custom
                label” is checked so it appears on the active listings page. Once it appears you
                can add your unique ID number to each of your ebay listings.
            </p>
            <p>
                <h2>
                    3. Pending Shipments</h2>
                The pending shipments page allows you to download pending Shipments from ebay, create
                invoices as required and also create the eparcel file to upload to the eparcel website.
                Start by clicking the “Download Now” button. This will then display all the current
                pending ebay shipments. Select the shipments you wish to send and click “Process
                Selected Items”. This will create your eparcel CSV file. Once it has finished processing
                a link will appear below the “Process Selected Item” button. Click on the “Eparcel
                File Link” to download the file, and save it to a known location on your hard drive.
                To upload the eparcel file to the eparcel website, log into the eparcel site and
                choose from the reference data menu, “Import Reference File”. Select the radio button
                “Import consignment File”, then click “Choose File”. Select the eparcel file, and
                then click “import”. Make certain that “Assign all consignment to the next manifest”
                is checked. Once the file is uploaded check for any errors with the consignment
                menu, under “view consignment – Not Despatched”. Errors will appear as a red crossed
                icon in the “invalid column”. Click on the “Consignment Number” to correct the error.
                The error is usually related to either the suburb or town or the postcode. Se the
                “Locality Search” button to check they are valid. Also make certain they are “assigned”
                to the manifest, under the “View Consignment – Not Despatched”.
            </p>
            <p>
                <h2>
                    4. Update Shipment Status</h2>
                The “Update shipment Status” menu, lets you update the shipment status, and tracking
                information of your ebay shipments automatically. If you use this function and send
                large volumes of shipments it can save hours. To use this function you need to retrieve
                the completed manifest file from the eparcel website. There are several forms of
                manifest formats, but the one we use is from the eparcel search menu, under “Search
                for Manifest”. Change the created date to the current date in both fields and click
                on export data. This will download the manifest file to your local machine. Save
                it in a known directory or folder. The next part of the process is to upload the
                file to your Parcel Solution account to update the shipment status and add the tracking
                numbers. In the “Update Shipment Status” click on “choose file” and select the manifest
                file, then click on Upload Manifest & Update shipment Status. Check for any errors.
            </p>
            <p>
                If you have any issues please contact support.
            </p>
        </p>
    </div>
    <script src="/scripts/videojs/video.min.js" type="text/javascript"></script>
</asp:Content>
