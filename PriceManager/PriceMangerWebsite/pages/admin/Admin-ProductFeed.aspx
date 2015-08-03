<%@ Page Title="" Language="C#" MasterPageFile="~/master/Main.master" AutoEventWireup="true"
    CodeFile="Admin-ProductFeed.aspx.cs" Inherits="pages_admin_Admin_Product_Feed" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="rt-containerInner">
        <div class="rt-grid-12 ">
            <div class="rt-block">
                <div class="container generalContainer">
                    <div class="componentheading">
                        <h2>
                            Product Feed</h2>
                    </div>
                    <p>
                        Here you can create and export products in csv format.
                    </p>
                    <div id="divUserSelect" runat="server" class="userSelectBox">
                        <p>
                            
                            Select User:
                            <asp:DropDownList ID="ddlUserList" runat="server" AutoPostBack="true" Width="200px">
                            
                            </asp:DropDownList>
                            <asp:Button ID="btnExport" runat="server" Text="Export" OnClick="btnExport_Click" class="button"/>
                        
                        </p>

                        <p>
                            <asp:HyperLink ID="hprExport" runat="server" Text="" NavigateUrl="#" Visible ="false" />
                            
                        
                        </p>

                    </div>
                    <div style="clear: both">
                        
                    </div>
                </div>
            </div>
        </div>
    </div>
     <script src="/Scripts/lib/jquery-1.7.1.min.js" type="text/javascript"></script>
    <script src="/Scripts/lib/jquery-ui-1.8.22.custom.min.js" type="text/javascript"></script>
    
</asp:Content>
