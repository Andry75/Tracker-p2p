<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Create_topic.aspx.cs" Inherits="Tracker.Forum.Create_topic" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Forums" runat="server">
    <div Align = "left">
        <asp:Label ID="Label1" runat="server" Text="Форум"></asp:Label>
        <br />
        <asp:DropDownList ID="DropDownList1" runat="server" 
            onselectedindexchanged="DropDownList1_SelectedIndexChanged" AutoPostBack ="true">
        
        </asp:DropDownList>
        <br />
         <asp:Label ID="Label2" runat="server" Text="Подфорум"></asp:Label>
         <br />
        <asp:DropDownList ID="DropDownList2" runat="server">
        
        </asp:DropDownList>
        <br />
        <asp:Label ID="Label3" runat="server" Text="Название темы"></asp:Label>
        <br />
        <asp:TextBox ID="TextBox1" runat="server" Width="173px"></asp:TextBox>
        <br />
        
        <asp:Label ID="Label4" runat="server" Text="Пост"></asp:Label>
        <br />
        <asp:TextBox ID="TextBox2" runat="server" Height="99px" TextMode="MultiLine" 
            Width="397px"></asp:TextBox>
          <br />
          <asp:Label ID="Label5" runat="server" Text="Мета-файл"></asp:Label>
        <br />
        <asp:FileUpload ID="FileUpload1" runat="server" />
        <br />
        <asp:Button ID="Button1"
            runat="server" Text="Создать" onclick="Button1_Click" />


    </div>
</asp:Content>
