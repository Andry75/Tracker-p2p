<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Topic.aspx.cs" Inherits="Tracker.Forum.Topic" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Forums" runat="server">
    <asp:Panel ID="First_post" runat = "server">

    </asp:Panel>
    <div align = "right">
        
    
    <asp:Button ID="Button1" runat="server" Text="Скачать мета-файл" 
        onclick="Button1_Click" Height="21px" Width="116px" />
</div>
<asp:Panel ID="Posts" runat = "server">

    </asp:Panel>
    <div>
<asp:TextBox ID="TextBox1" runat="server" Height="120px" Width="767px"></asp:TextBox>
</div>
<div Align = "right">
    <asp:Button ID="Button2" runat="server" Text="Ответить" Width="108px" 
        onclick="Button2_Click" />
    </div>
</asp:Content>
