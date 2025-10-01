<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Debug_.aspx.cs" Inherits="CommitmentLettersApp.Debug" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="aspnetForm" runat="server">
        <asp:Panel runat="server" ID="pnl1" Visible="false">
            <div>
                commitments <br /><br />

                current time
                <asp:Label runat="server" ID="lblDate" />
                <br />
                <br />

                New Debug Time
                <asp:TextBox ID="txtDebugTime" runat="server" Text="" />
                <asp:Button runat="server" ID="btnSetTime" Text="Set" OnClick="btnSetTime_Click" />
                <asp:Button runat="server" ID="btnClearTime" Text="Clear" OnClick="btnClearTime_Click" />

                <br />
                <br />
                <br />
                <br />
                current version:
                <asp:Label runat="server" ID="lblVersion" />

            </div>
        </asp:Panel>
    </form>
</body>
</html>
