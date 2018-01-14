<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login_okno.aspx.cs" Inherits="RESTService.Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
        <asp:TextBox ID="uporabnisko" runat="server" Width="1315px"></asp:TextBox>
    
    </div>
        <p>
            <asp:TextBox ID="geslo" runat="server" Width="1319px"></asp:TextBox>
        </p>
        <p>
            <asp:Button ID="Vpis" runat="server" OnClick="login_clicked" Text="LOGIN" Width="1332px" />
        </p>
        <asp:Button ID="registracija" runat="server" OnClick="Register_click" Text="Register Here" Width="1334px" />
        <p>
            <asp:Label ID="test" runat="server" Text="Label"></asp:Label>
        </p>
    </form>
</body>
</html>
