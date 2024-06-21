<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="CommitmentLettersApp.Login" %>


<!DOCTYPE html>
<html lang="iw">
<head>
<meta name="viewport" content="width=device-width, initial-scale=1">
<meta name="theme-color" content="#0093A2">
<title>תבנית כניסה · Bootstrap בעברית</title>
<meta name="description" content="תבנית כניסה · Bootstrap בעברית">
<link rel="stylesheet" href="https://stackpath.bootstrapcdn.com/bootstrap/4.5.0/css/bootstrap.min.css" />
<script src="lib/jquery/jquery.min.js"></script>
<style>
      .bd-placeholder-img {
        font-size: 1.125rem;
        text-anchor: middle;
        -webkit-user-select: none;
        -moz-user-select: none;
        -ms-user-select: none;
        user-select: none;
      }

      @media (min-width: 768px) {
        .bd-placeholder-img-lg {
          font-size: 3.5rem;
        }
      }

      body {
          direction: rtl;
          color: #566787;
          background: #f5f5f5;
          font-family: 'Varela Round', sans-serif;
          font-size: 13px;
      }
    </style>
<link href="signin.css" rel="stylesheet">
<link rel="stylesheet" href="https://stackpath.bootstrapcdn.com/bootstrap/4.5.0/css/bootstrap.min.css" />
</head>
<body             direction: rtl;>
<form runat="server" class="form-signin">
                        <asp:HiddenField runat="server" ID="loginerror" ClientIDMode="Static" Value="" />


                                <div class="d-none text-right alert alert-danger alert-dismissible fade show" role="alert" id="alerterror">
    <div id="alerterrormsg"></div>
  <button type="button" class="close" data-dismiss="alert" aria-label="Close">
    <span aria-hidden="true">&times;</span>
  </button>
</div>


<h1 class="text-right h3 mb-3 font-weight-normal">כניסה למערכת</h1>
<label for="inputEmail" class="sr-only">שם משתמש</label>
<input runat="server" type="text" id="inputUsername" class="form-control" placeholder="שם משתמש/ת.ז" required autofocus>
<label for="inputPassword" class="sr-only">סיסמה</label>
<input runat="server" type="password" id="inputPassword" class="form-control" placeholder="סיסמה" required>
<div class="checkbox mb-3">
<label> <input type="checkbox" id="chkRememberMe" runat="server" value="remember-me">זכור אותי</label>
</div>
<asp:Button runat="server" ID="btnLogin" CssClass="btn btn-lg btn-primary btn-block" OnClick="btnLogin_Click" Text="התחברות"></asp:Button>
</form>
    <script src="https://stackpath.bootstrapcdn.com/bootstrap/4.5.0/js/bootstrap.min.js"></script>

    <script>
      
        function showAlert(msg) {
            $('#alerterrormsg').html(msg);
            $('#alerterror').removeClass("d-none");;
        }

        $(document).ready(function () {
            if ($("#loginerror").val() != "") {
                showAlert($("#loginerror").val());
                $("#loginerror").val("");
            }
        });
    </script>
</body>
</html>
