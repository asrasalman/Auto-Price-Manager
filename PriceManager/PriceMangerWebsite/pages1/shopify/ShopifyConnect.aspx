<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ShopifyConnect.aspx.cs" Inherits="pages_shopify_ShopifyConnect" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="/scripts/lib/jquery-1.7.1.min.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <input type="hidden" runat="server" id="hfToken" class='hfToken' />
    </div>
    </form>
</body>
</html>
<script type="text/javascript">
    $(document).ready(function () {
        if ($('.hfToken').val() != '') {
            $(window.opener.document).find('.shopifyConnect').hide();
            $(window.opener.document).find('.shopifyConnectedBox').show();
            window.close();
        }
    });

</script>
