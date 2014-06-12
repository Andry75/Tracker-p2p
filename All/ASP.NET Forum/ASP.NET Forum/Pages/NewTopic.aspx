<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="NewTopic.aspx.cs" Inherits="ASP.NET_Forum.Pages.NewTopic" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Безымянная страница</title>
    <link rel="Stylesheet" href="../CSS/forum.css" />
</head>
<body>
    <form id="form1" runat="server">
    <div id="page" runat="server"></div>
    <div id="err" runat="server" class="error"></div>
       Название темы <asp:TextBox ID="tbTopicName" runat="server" Width="50%"></asp:TextBox><br />
       Текст сообщения<br />
        <asp:TextBox ID="tbMessText" runat="server" Width="90%" Rows="15" TextMode="MultiLine"></asp:TextBox>
    
    <asp:Button ID="btCreate" runat="server" onclick="btCreate_Click" 
        Text="Создать" />
    </form>
</body>
</html>
