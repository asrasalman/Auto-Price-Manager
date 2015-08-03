<%@ Page Title="" Language="C#" MasterPageFile="~/master/Main.master" AutoEventWireup="true"
    CodeFile="PricingHistory.aspx.cs" Inherits="pages_seller_PricingHistory" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="/css/select2.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="rt-containerInner">
        <div class="rt-grid-12 ">
            <div class="rt-block">
                <input type="hidden" id="hfItemCode" runat="server" class="hfItemCode" clientidmode="Static" />
                <div class="downloadFile">
                    <div class="componentheading">
                      <h2> Pricing History </h2>
                    </div>
                    <p>
                       Here you can see price revised history.
                    </p>
                    <div style="clear: both">
                    </div>
                    <div class="searchContainer22">
                    <div id="chartHitRateDateWise">
                    </div>
                    </div>
                     <div style="clear: both">
                    </div>
                    <br />
                    <div id="divPricingItems">
                        <h3>Pricing History</h3>
                        <table id="tblPricingItems" class="list tablesorter" cellpadding="0" cellspacing="0"
                            style="width: 100%">
                            <thead>
                                <tr>
                                    <th>
                                        Date/Time
                                    </th>
                                    <th>
                                        Algo
                                    </th>
                                    <th>
                                        Keywords
                                    </th>
                                    <th>
                                        Old Pricie
                                    </th>
                                    <th>
                                        New Price
                                    </th>

                                </tr>
                            </thead>
                            <tbody>
                            </tbody>
                        </table>
                    </div>
                    <br />
                    <div id="divTitleItems">
                        <h3>Title History</h3>
                        <table id="tblTitlesItems" class="list tablesorter" cellpadding="0" cellspacing="0"
                            style="width: 100%">
                            <thead>
                                <tr>
                                    <th>
                                        Date/Time
                                    </th>
                                    <th>
                                        New Title
                                    </th>
                                    <th>
                                        Old Title
                                    </th>
                                    <th>
                                        Total Sales
                                    </th>
                                    
                                </tr>
                            </thead>
                            <tbody>
                            </tbody>
                        </table>
                    </div>
                </div>
                <br />
                <br />
                <br />
                <div class="clear">
                </div>
            </div>
        </div>
    </div>
    <script src="/Scripts/lib/jquery-1.7.1.min.js" type="text/javascript"></script>
    <script src="/Scripts/lib/jquery-ui-1.8.22.custom.min.js" type="text/javascript"></script>
    <script src="/scripts/lib/colResizable-1.3.min.js" type="text/javascript"></script>
    <script src="/scripts/lib/jquery.tablesorter.js" type="text/javascript"></script>
    <script src="/scripts/lib/jquery.tmpl.min.js" type="text/javascript"></script>
    <script src="/scripts/lib/jquery.numeric.js" type="text/javascript"></script>
    <script src="/scripts/lib/jquery.qtip.min.js" type="text/javascript"></script>
   
    <script src="/scripts/lib/select2.min.js" type="text/javascript"></script>
    <script src="/scripts/lib/oneSimpleTablePaging-1.0.js" type="text/javascript"></script>
    <script src="/scripts/lib/jquery.switchButton.js" type="text/javascript"></script>

    <script type="text/javascript" src="/scripts/lib/jqplot/jquery.jqplot.min.js"></script>
    <script type="text/javascript" src="/scripts/lib/jqplot/plugins/jqplot.dateAxisRenderer.min.js"></script>
    <script type="text/javascript" src="/scripts/lib/jqplot/plugins/jqplot.highlighter.min.js"></script>
    <script type="text/javascript" src="/scripts/lib/jqplot/plugins/jqplot.cursor.min.js"></script>
    <script type="text/javascript" src="/scripts/lib/jqplot/plugins/jqplot.categoryAxisRenderer.min.js"></script>
    <script type="text/javascript" src="/scripts/lib/jqplot/plugins/jqplot.pointLabels.min.js"></script>
    

    <script src="/scripts/custom/PricingHistory.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            InitializePricingHistory();
        });
    </script>
    <script id="pricingItemTemplate" type="text/x-jQuery-tmpl">
        <tr>
            <td>${Created_Date}</td>
            <td>${Algo}</td>
            <td>${Keyword}</td>
            <td>${Old_Price}</td>
            <td>${New_Price}</td>
            
        </tr>
    </script>
</asp:Content>
