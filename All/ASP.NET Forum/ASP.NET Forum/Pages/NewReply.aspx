<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="NewReply.aspx.cs" Inherits="ASP.NET_Forum.Pages.NewReply" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Безымянная страница</title>
    <link rel="Stylesheet" href="../CSS/forum.css" />
</head>
<body>
    <form id="form1" runat="server">
    <div id ="page" runat="server">
    
    </div>
    <div id ="err" class="error" runat="server"></div>
    Новое сообщение:<br />
        <asp:TextBox ID="tbText" runat="server" Rows="15" TextMode="MultiLine" 
            Width="100%"></asp:TextBox>
    <br />
    <asp:Button ID="Button1" runat="server" onclick="Button1_Click" 
        Text="Отправаить" />
    </form>
</body>
</html>
