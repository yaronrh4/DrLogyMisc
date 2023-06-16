<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Encrypt.aspx.cs" Inherits="CommitmentLettersApp.Encrypt" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <ol>
            <li>copy this aspx file to the server 
            </li> 
            <li>download the web.config from the server
            </li>
            <li>encrypt / decrypt the file
            </li>
            <li>upload / the new web.config
            </li>
            <li>delete the web.config from the temp folder
            </li>
            </ol>

            <br />
            <asp:FileUpload ID="fuConfig" runat="server" />
            <br />
            web.config folder<asp:TextBox runat="server" ID="txtFolder" Text="Temp" />
            <br />
            <br />

            <asp:Button id="btnRun" runat="server" Text="Encrypt" OnClick="btnRun_Click" />
            <asp:Button id="btnDecrypt" runat="server" Text="Decrypt" OnClick="btnDecrypt_Click" />
        </div>
    </form>
</body>
</html>
