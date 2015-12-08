<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=11" />
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>節目管理系統</title>
    <script type="text/javascript" src="../Scripts/jquery.min.js"></script>
    <script type="text/javascript" src="../Scripts/jquery-ui.min.js"></script>
    <link href="../css/jquery-ui.min.css" rel="stylesheet" />
    <link href="css/pms-ui.css" rel="stylesheet" />
    <script type="text/javascript">

        $(document).ready(function () {

            login_window();

            $("#login").draggable({ handle: ".title", opacity: 0.5 });

            $(window).resize(function () {

                login_window();
            });

        });

        function login_window() {

            $("#login").css('left', parseInt($(window).width()) / 2 - 200);
            if (parseInt($(window).height()) > 500) {
                $("#login").css('top', parseInt($(window).height()) / 4);
            }
            else {
                $("#login").css('top', parseInt($(window).height()) / 2 - 115);
            }

        }

    </script>
    <style type="text/css">

        #ShadowMark {
            background-color: #E0E1E3;
            width: 100%;
            height: 100%;
            position: absolute;
            left: 1px;
            top: 1px;
            right: 0px;
            bottom: 0px;
            filter: alpha(opacity=60);
            opacity: 0.6;
        }

        #login {
            height: 230px;
            width: 400px;
            border: 0px;
            position: absolute;
        }

        #login-table {
            border: 1px solid #86A4BE;
            border-radius: 4px;
            box-shadow: 0px 2px 10px #AAAAAA;
        }

        .title {
            background-color: rgb(107, 171, 215);
            height: 25px;
            color: #FFFFFF;
        }

            .title:hover {
                cursor: move;
            }

    </style>
</head>
<body style="overflow: hidden;">
    <form id="Form1" runat="server" method="post" name="form">
    <!-- 機 構-->
    <asp:HiddenField ID="ddlOrg" runat="server" />
    <div id="container">
      <div id="ShadowMark"></div>
        <div id="login">
            <table id="login-table" class="z-border">
                <tr class="title">
                    <td colspan="2"">
                        <span>由此登入</span>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" style="background-image: url('img/profile_header.jpg')">
                        <div style="height: 90px; width: 396px; top: 0px; left: 0px;"></div>
                    </td>
                </tr>
                <tr class="bodyRow">
                    <td colspan="2" style="text-align: right">
                       <div><span style="COLOR: red; font-size: xx-large;">TMSDEV C#改版測試機</span></div>
                       <div><a href="http://srv-tmsdev-ap.goodtv.tv:8080/"><span style="font-size: medium;">如果要連結到原Java片庫請點這裡</span></a></div>
                    </td>
                </tr>
                <tr class="bodyRow">
                    <td class="bodyRow" style="text-align: right; width: 100px;">
                        <span style="width: 100px;">請輸入帳號</span>
                    </td>
                    <td class="bodyRow" style="vertical-align: middle; text-align: left; width: 100%;">
                        <label>
                            <asp:TextBox ID="txtUserID" runat="server" Width="150px"></asp:TextBox>
                        </label>
                    </td>
                </tr>
                <tr class="bodyRow">
                    <td class="bodyRow" style="text-align: right; width: 100px;">
                        <span style="width: 100px;">請輸入密碼</span>
                    </td>
                    <td class="bodyRow" style="vertical-align: middle; text-align: left; width: 100%;">
                        <label>
                            <asp:TextBox ID="txtPassword" runat="server" Width="150px" TextMode="Password"></asp:TextBox>
                        </label>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" style="height: 20px;">
                        <table style="width: 100%">
                            <tr>
                                <td>
                                    <div style="width: 80px"></div>
                                </td>
                                <td style="text-align: center; vertical-align: top">
                                    <div style="text-align: center">version 2.00</div>
                                </td>
                                <td style="text-align: right; vertical-align: middle">
                                    <asp:Button ID="btLogin" runat="server" Text="使用者登入" class="ui-button ui-corner-all" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </div>
    </div>
    </form>
</body>
</html>
