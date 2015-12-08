<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AdminRight.aspx.cs" Inherits="SysMgr_AdminRight" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>使用權限設定</title>
    <link href="../css/jquery-ui.min.css" rel="stylesheet" />
    <script type="text/javascript" src="../Scripts/jquery.min.js"></script>
    <script type="text/javascript" src="../Scripts/jquery-ui.min.js"></script>
    <link href="../css/pms-ui.css" rel="stylesheet" />
    <script type="text/javascript" src="../Scripts/pms-script.js"></script>
    <script type="text/javascript" src="../include/common.js"></script>
    <script type="text/javascript">


        $(document).ready(function (e) {

            // 取得資料表格高度
            $("#mainTable").css('height', $(window).height() - (Math.round(($("#mainTable").position().top) + 17)));

            $(window).resize(function () {
                $("#mainTable").css('height', $(window).height() - (Math.round(($("#mainTable").position().top) + 17)));
            });

            if ($('#HFD_UpdateRight').val() == 'False') {
                $('#btnSave').hide();
            }

            $('#CB_AddColAll').bind("click", function (e) {
                SetCol('Add', $(this).prop('checked'));
            });
            $('#CB_UpdateColAll').bind("click", function (e) {
                SetCol('Update', $(this).prop('checked'));
            });
            $('#CB_DeleteColAll').bind("click", function (e) {
                SetCol('Delete', $(this).prop('checked'));
            });
            $('#CB_QueryColAll').bind("click", function (e) {
                SetCol('Query', $(this).prop('checked'));
            });
            $('#CB_PrintColAll').bind("click", function (e) {
                SetCol('Print', $(this).prop('checked'));
            });
            $('#CB_FocusColAll').bind("click", function (e) {
                SetCol('Focus', $(this).prop('checked'));
            });
            $('#CB_Examine1ColAll').bind("click", function (e) {
                SetCol('Examine1', $(this).prop('checked'));
            });
            $('#CB_Examine2ColAll').bind("click", function (e) {
                SetCol('Examine2', $(this).prop('checked'));
            });
            $('#CB_Examine3ColAll').bind("click", function (e) {
                SetCol('Examine3', $(this).prop('checked'));
            });
            $('#CB_Examine4ColAll').bind("click", function (e) {
                SetCol('Examine4', $(this).prop('checked'));
            });

            $('#CB_SelectAll').bind("click", function (e) {
                $(this).prop('checked', true)
                $('#CB_UnSelectAll').prop('checked', false)
                $('#CB_AddColAll').prop('checked', true)
                $('#CB_UpdateColAll').prop('checked', true);
                $('#CB_DeleteColAll').prop('checked', true);
                $('#CB_QueryColAll').prop('checked', true);
                $('#CB_PrintColAll').prop('checked', true);
                $('#CB_FocusColAll').prop('checked', true);
                $('#CB_Examine1ColAll').prop('checked', true);
                $('#CB_Examine2ColAll').prop('checked', true);
                $('#CB_Examine3ColAll').prop('checked', true);
                $('#CB_Examine4ColAll').prop('checked', true);

                SetAll(true);
            });
            $('#CB_UnSelectAll').bind("click", function (e) {
                $(this).prop('checked', true)
                $('#CB_SelectAll').prop('checked', false)
                $('#CB_AddColAll').prop('checked', false)
                $('#CB_UpdateColAll').prop('checked', false);
                $('#CB_DeleteColAll').prop('checked', false);
                $('#CB_QueryColAll').prop('checked', false);
                $('#CB_PrintColAll').prop('checked', false);
                $('#CB_FocusColAll').prop('checked', false);
                $('#CB_Examine1ColAll').prop('checked', false);
                $('#CB_Examine2ColAll').prop('checked', false);
                $('#CB_Examine3ColAll').prop('checked', false);
                $('#CB_Examine4ColAll').prop('checked', false);

                SetAll(false);
            });

            $('input:checkbox[id*=CB_RowSelectAll]').bind("click", function (e) {
                var s = 'input:checkbox[value=' + $(this).val() + ']';
                $(s).prop('checked', true);
                s = '#CB_RowUnSelectAll' + $(this).val();
                $(s).prop('checked', false);
            });

            $('input:checkbox[id*=CB_RowUnSelectAll]').bind("click", function (e) {
                var s = 'input:checkbox[value=' + $(this).val() + ']';
                $(s).prop('checked', false);
                s = '#CB_RowSelectAll' + $(this).val();
                $(s).prop('checked', false);
            });
       });

        function SetCol(name, checked) {
            var s = 'input:checkbox[id*=CB_Col' + name + ']';
            $(s).prop('checked', checked);
        }

        function SetAll(checked) {
            SetCol('Add', checked);
            SetCol('Update', checked);
            SetCol('Delete', checked);
            SetCol('Query', checked);
            SetCol('Print', checked);
            SetCol('Focus', checked);
            SetCol('Examine1', checked);
            SetCol('Examine2', checked);
            SetCol('Examine3', checked);
            SetCol('Examine4', checked);
        }

        function SaveAllValue() {
            var SumCnt = $('#HFD_TotalMenuItem').val();
            var SetupValue = '';
            for (var i = 0; i < SumCnt; i++) {
                var MenuID = $('#MenuID' + i).attr('value');
                SetupValue += MenuID + ",";
                SetupValue += GetValue('#CB_ColAdd' + i) + ',';
                SetupValue += GetValue('#CB_ColUpdate' + i) + ',';
                SetupValue += GetValue('#CB_ColDelete' + i) + ',';
                SetupValue += GetValue('#CB_ColQuery' + i) + ',';
                SetupValue += GetValue('#CB_ColPrint' + i) + ',';
                SetupValue += GetValue('#CB_ColFocus' + i) + ',';
                SetupValue += GetValue('#CB_ColExamine1' + i) + ',';
                SetupValue += GetValue('#CB_ColExamine2' + i) + ',';
                SetupValue += GetValue('#CB_ColExamine3' + i) + ',';
                SetupValue += GetValue('#CB_ColExamine4' + i);

                if (i < SumCnt - 1) {
                    SetupValue += '|';
                }
            }
            $('#HFD_SetupValue').val(SetupValue);
            document.getElementById("KeyFocus").click();

        }

        function GetValue(selector) {
            if ($(selector).length != 1) {
                return '';
            }
            if ($(selector).prop('checked') == true) {
                return '1';
            }
            else {
                return '0';
            }
        }
    </script>
    <style type="text/css">

        #LB_Title {
            border: 0px;
        }

        #LB_Grid {
            border: 0px;
        }

        #Title table tr:nth-child(3) {
            background-image: url(/img/grid/column-bg.png);
        }

        #Title .table-list tr:nth-child(even) {
            background-color: white;
        }

        #Title .table-list tr:nth-child(odd) {
            background-color: #E6F8FF;
        }

        #mainTable .table-list tr:nth-child(even) {
            background-color: #E6F8FF;
        }

        #mainTable .table-list tr:nth-child(odd) {
            background-color: white;
        }

        #mainTable .table-list tr:first-child td {
            border-top-width: 0px;
        }

        #mainTable .table-list tr:hover {
            background-color: #dae7f6;
        }

        .table-list tr:first-child {
            background-image: none;
        }

        .table-list tr td {
            border: 1px solid #CCC;
            min-width: 24px;
        }

    </style>
</head>
<body style="width: 99%; position: relative" >
    <form id="Form1" runat="server">
    <asp:HiddenField runat="server" ID="HFD_GroupUid" />
    <asp:HiddenField runat="server" ID="HFD_TotalMenuItem" />
    <asp:HiddenField runat="server" ID="HFD_SetupValue" />
    <asp:HiddenField runat="server" ID="HFD_UpdateRight" />
    <div id="container" align="center">
    <table>
        <tr>
            <th>
                群組：
            </th>
            <td>
                <asp:DropDownList ID="ddlGroup" DataTextField="GroupName" DataValueField="uid" Width="180px"
                    runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlGroup_SelectedIndexChanged">
                </asp:DropDownList>
            </td>   
         <!--   <td width="30%" align="left">
            </td>
            <td width="40%" align="left"> -->
                <!--<asp:ImageButton ID="btnQuery"  Style="font-size: 9pt" CssClass="button3-bg" ImageUrl="~/images/search.jpg" runat="server" OnClick="btnQuery_Click"></asp:ImageButton>
            </td> -->
        </tr>
    </table>
    <div>
        <asp:Button ID="KeyFocus" Text="" BackColor="#ffffff" BorderColor="#cff6d8" OnClick="KeyFocus_OnClick"
            BorderStyle="None" runat="server" />
    </div>
        <table cellpadding="0" cellspacing="0">
            <tr>
                <td>
                    <div id="Title">
                        <asp:Label ID="LB_Title" runat="server" CssClass="table-list"></asp:Label>
                    </div>
                </td>
                <td>
                    <div style="width: 17px;"></div>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <div id="mainTable" style="overflow-y: scroll; height: 400px;">
                        <asp:Label ID="LB_Grid" runat="server" CssClass="table-list"></asp:Label>
                    </div>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
