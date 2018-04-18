<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SignOut.aspx.cs" Inherits="SignOut" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
   <title>Logout</title>
    <script type="text/javascript">
        function changeHashOnLoad() {
            window.location.href += "#";
            setTimeout("changeHashAgain()", "50");
        }

        function changeHashAgain() {
            window.location.href += "1";
        }

        var storedHash = window.location.hash;
        window.setInterval(function () {
            if (window.location.hash != storedHash) {
                window.location.hash = storedHash;
            }
        }, 50);


    </script>
</head>
<body onload="changeHashOnLoad(); ">
    <form id="form1" runat="server">
    <div>
        <label>
            You are successfully logged out from the website.</label>
        <br />
        <a href="Login.aspx" target="_parent">Click here to login again</a>
        <br />
    </div>
    </form>
</body></html>
