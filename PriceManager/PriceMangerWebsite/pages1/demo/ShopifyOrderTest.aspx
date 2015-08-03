<%@ Page Title="" Language="C#" MasterPageFile="~/master/Main.master" AutoEventWireup="true" CodeFile="ShopifyOrderTest.aspx.cs" Inherits="pages_ShopifyOrderTest" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<asp:Button ID="Button1" runat="server" Text="Button" onclick="Button1_Click" />
&nbsp;
    <asp:Button ID="Button2" runat="server" onclick="Button2_Click" Text="Button" />
</asp:Content>