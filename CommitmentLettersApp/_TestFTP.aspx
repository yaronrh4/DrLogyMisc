<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="_TestFTP.aspx.cs" Inherits="DrLogyApp._TestFTP" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            url<asp:TextBox runat="server" ID="txtURL" Text="ftp://node617.namehero.net/yaron/" ></asp:TextBox> <br /><br />
            username<asp:TextBox runat="server" ID="txtUserName" Text="test@dr-logyvault.com" ></asp:TextBox> <br /><br />
            password<asp:TextBox runat="server" ID="txtPassword" Text="MaccabiZona!123" ></asp:TextBox> <br /><br />
            <asp:FileUpload runat="server" ID="fu1" /> <br /><br />

            <asp:Button runat="server" ID="btnGo" Text="Go" OnClick="btnGo_Click" /> <br /><br />
            
            <asp:Button runat="server" ID="btnArchive" Text="Archive" OnClick="btnArchive_Click" />
        </div>
    </form>
</body>
</html>
