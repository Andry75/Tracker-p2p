<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="ASP.NET_Forum.Pages.Login" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Безымянная страница</title>
    <link rel="Stylesheet" href="../CSS/forum.css" />
</head>
<body>
    
    <form id="form1" runat="server">
    <div>
     
        
   <h3 align="center">Форма входа</h3> 
   <div id="err" runat="server" class="error"></div>
        <table style="width: 40%;" class="login" align="center">
            <tr>
          <td width="50%">Логин</td>
                <td><asp:TextBox ID="tbLogin" runat="server"></asp:TextBox></td>
         </tr>
         <tr><td></td></tr>
          <tr>
                <td>Пароль</td>
                <td><asp:TextBox ID="tbPass" runat="server" TextMode="Password"></asp:TextBox></td>
          </tr>
          <tr><td></td></tr>
           <tr>
                <td>
                    <asp:CheckBox ID="cbRem" runat="server" Text="Запомнить?" /></td>
                <td>
                    <asp:Button ID="btnLogin" runat="server" Text="Войти" 
                        onclick="btnLogin_Click" /></td>
          </tr> 
        </table>
    </div>
    </form>
</body>
</html>
