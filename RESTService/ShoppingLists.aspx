<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ShoppingLists.aspx.cs" Inherits="RESTService.ShoppingLists" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
        <asp:Label ID="Label1" runat="server" Text="Shopping Lists"></asp:Label>
        <br />
    
    </div>
        <asp:ListBox ID="ListBox1" runat="server" Width="234px"></asp:ListBox>
        <p>
            <asp:Button ID="Button1" runat="server" Text="Ustvari nov Shopping List" />
        </p>
        <asp:Button ID="Button2" runat="server" Text="Odjava" />
    </form>
</body>
</html>
