<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Register_okno.aspx.cs" Inherits="RESTService.Register_okno" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
        Name :
        <asp:TextBox ID="ime" runat="server"></asp:TextBox>
    
    </div>
        <p>
            Surname :
            <asp:TextBox ID="priimk" runat="server"></asp:TextBox>
        </p>
        Email :<asp:TextBox ID="mail" runat="server"></asp:TextBox>
        <p>
            Username :
            <asp:TextBox ID="uporabnisko" runat="server"></asp:TextBox>
        </p>
        Password :
        <asp:TextBox ID="geslo" runat="server"></asp:TextBox>
        <p>
            Phone Number :
            <asp:TextBox ID="stevilka" runat="server"></asp:TextBox>
        </p>
        <asp:Label ID="error" runat="server" Visible="False"></asp:Label>
        <p>
            <asp:Button ID="Button1" runat="server" OnClick="Register_clicked" Text="Register" />
        </p>
        <p>
            <asp:Button ID="Button2" runat="server" OnClick="nazaj_prijava" Text="Nazaj na prijavo" />
        </p>
    </form>
</body>
</html>
