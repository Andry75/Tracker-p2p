<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Add_Server.aspx.cs" Inherits="Tracker.Admintools.Add_Server" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Forums" runat="server">
    <asp:Label ID="Label1" runat="server" Text="IP:" CssClass="bold"></asp:Label>
    <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
    <br />
    <asp:Label ID="Label2" runat="server" Text="Port:" CssClass="bold"></asp:Label>
    <asp:TextBox ID="TextBox2" runat="server"></asp:TextBox>
    <br />
    <asp:Button ID="Button1"
        runat="server" Text="Button" onclick="Button1_Click" />

</asp:Content>
