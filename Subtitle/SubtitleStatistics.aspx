<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SubtitleStatistics.aspx.cs" Inherits="Subtitle_SubtitleStatistics" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link href="../css/jquery-ui.min.css" rel="stylesheet" />
    <script type="text/javascript" src="../Scripts/jquery.min.js"></script>
    <script type="text/javascript" src="../Scripts/jquery-ui.min.js"></script>
    <link href="../css/pms-ui.css" rel="stylesheet" />
    <script type="text/javascript" src="../Scripts/pms-script.js"></script>
    <script type="text/javascript" src="../Subtitle/SubtitleStatistics.js"></script>
    <style type="text/css">

        .table-detail-img {
            cursor: pointer;
            width: 13px;
            height: 13px;
            padding: 1px;
            background: transparent no-repeat 3px 3px;
            background-image: url(/img/grid/row-expand.gif);
            /*background-position: -12px 3px;*/
        }

        .table-detail-outer {
            background: #C6E8FC repeat-y left;
            vertical-align: top;
            background-image: url(/img/grid/detail-bg.png);
        }

        .table-detail-expd {
            background-position: -13px 3px;
        }

        .table-list2 {
            background: white;
            border-collapse: collapse;
            empty-cells: show;
            border-spacing: 0px;
            border: 1px solid #86A4BE;
            table-layout: fixed;
        }

        .table-list2 {
            border: 1px solid #DDD;
            width: 100%;
        }

            .table-list2 tr:first-child {
                background: #C7E5F1 repeat-x scroll 0 0;
                background-image: url(/img/grid/column-bg.png);
            }

            .table-list2 tr th {
                border-style: solid;
                border-width: 1px;
                border-color: #86A4BE #86A4BE #DAE7F6 #DAE7F6;
                white-space: nowrap;
                margin: 0px;
                padding: 2px;
            }

            .table-list2 tr td {
                padding: 2px;
                border-top: none;
                border-left: 0px solid white;
                border-right: 1px solid #CCC;
                border-bottom: 1px solid #DDD;
                word-break: break-all;
            }

            .table-list2 tr table {
                width: 100%;
                border-spacing: 0px;
            }

            .table-list2 tr table tr:nth-child(odd) {
                background-color: #E6F8FF;
            }

            .table-list2 tr table td:nth-child(1) {
                width: 10%;
            }

            .table-list2 tr table td:nth-child(2) {
                width: 20%;
            }

            .table-list2 tr table td:nth-child(3) {
                width: 10%;
            }

            .table-list2 tr table td:nth-child(n+4):nth-child(-n+7) {
                width: 15%;
            }

    </style>
</head>
<body style="margin: 0px; position: relative; width: 99.9%; height: 100%; overflow: hidden;">
    <form id="form1" runat="server">
        <asp:HiddenField ID="UserID" runat="server" />
        <div>
            <div id="divnorth" style="border: 1px solid #9ECAD8; height: 40px; width: 99.9%; left: 0px; top: 0px;">
                <table style="vertical-align: middle">
                    <tr>
                        <td>
                            <input id="uiQuery" name="uiFunctionGroup" type="radio" checked="checked" />
                            <label for="uiQuery">查詢</label>
                            <input id="uiAccount" name="uiFunctionGroup" type="radio" />
                            <label for="uiAccount">對帳</label>
                            <input id="uiAccounted" name="uiFunctionGroup" type="radio" />
                            <label for="uiAccounted">結帳</label>
                        </td>
                        <td>
                            <div style="width: 3px"></div>
                        </td>
                        <td>
                            <div id="uiDepartBox">
                                <label for="uiDept">部門</label>
                                <select id="uiDept" name="uiDept" style="WIDTH: 80px; font-size: 12px;">
                                    <option value="000">全部</option>
                                    <option value="300">節目部</option>
                                    <option value="400">國外部</option>
                                </select>
                            </div>
                        </td>
                        <td>
                            <div id="uiQueryArea">
                                <input id="uiQueryAreaImport" type="radio" name="uiQueryArea" checked="checked" />
                                <label for="uiQueryAreaImport">依匯入日期</label>
                                <input id="uiQueryAreaAccount" type="radio" name="uiQueryArea" />
                                <label for="uiQueryAreaAccount">依結帳年月</label>
                            </div>
                        </td>
                        <td>
                            <div id="uiQueryBox">
                                <label for="uiSDate">開始日：</label>
                                <input id="uiSDate" name="uiSDate" type="text" style="width: 100px" />
                                <label for="uiEDate">結束日：</label>
                                <input id="uiEDate" name="uiEDate" type="text" style="width: 100px" />
                            </div>
                        </td>
                        <td>
                            <div id="uiAccountBox" style="display: none">
                                <label for="uiYear">結帳年份：</label>
                                <select id="uiYear" name="uiYear" style="width: 80px">
                                    <option>2012</option>
                                    <option>2013</option>
                                    <option>2014</option>
                                    <option selected="selected">2015</option>
                                    <option>2016</option>
                                </select>
                                <label for="uiMonth">結帳月份：</label>
                                <select id="uiMonth" name="uiMonth" style="width: 80px">
                                    <option>1</option>
                                    <option>2</option>
                                    <option>3</option>
                                    <option>4</option>
                                    <option>5</option>
                                    <option>6</option>
                                    <option>7</option>
                                    <option>8</option>
                                    <option>9</option>
                                    <option>10</option>
                                    <option>11</option>
                                    <option>12</option>
                                </select>
                            </div>
                        </td>
                        <td>
                            <input id="uiSearch" class="ui-button ui-corner-all" type="button" value="查詢" />
                        </td>
                    </tr>
                </table>
            </div>
            <div id="divcenter" style="border: 1px solid #9ECAD8; height: 630px; width: 99.9%; left: 0px; top: 42px;">
                <div id="body1" style="border: 1px solid #9ECAD8; border-bottom-style: none; width: 100%; overflow: auto;">
                    <table id="tb1" class="table-list2">
                        <tr>
                            <td style="width: 18px"></td>
                            <td>代號</td>
                            <td>姓名</td>
                            <td>所屬部門</td>
                            <td>筆數</td>
                            <td>總額</td>
                            <td>總評比</td>
                        </tr>
                    </table>
                </div>
                <div id="body2" style="border: 1px solid #9ECAD8; height: 50px; width: 100%; display: none;">
                    <table id="tb2" class="table-list2" style="width: 100%;"></table>
                </div>
            </div>
            <div id="divsouth" style="border: 1px solid #9ECAD8; height: 38px; width: 99.9%; display: none;">
                <div id="uiExportBoard" style="margin-right: 5px; float: right;">
                    <input id="uiExportQuery" class="ui-button ui-corner-all" type="button" value="匯出 Excel" style="display: none;cursor: pointer;" />
                    <input id="uiExportAccounted" class="ui-button ui-corner-all" type="button" value="匯出 Excel" style="display: none;cursor: pointer;" />
                    <input id="uiExportAccount" class="ui-button ui-corner-all" type="button" value="結帳並匯出 Excel" style="display: none;cursor: pointer;" />
                </div>
            </div>
        </div>
    </form>
</body>
</html>
