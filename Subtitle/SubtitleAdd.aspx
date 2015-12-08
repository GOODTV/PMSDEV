<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SubtitleAdd.aspx.cs" Inherits="Subtitle_SubtitleAdd" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">    
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <link href="../css/jquery-ui.min.css" rel="stylesheet" />
    <script type="text/javascript" src="../Scripts/jquery.min.js"></script>
    <script type="text/javascript" src="../Scripts/jquery-ui.min.js"></script>
    <link href="../css/pms-ui.css" rel="stylesheet" />
    <script type="text/javascript" src="../Scripts/pms-script.js"></script>
    <script type="text/javascript" src="../Subtitle/SubtitleAdd.js"></script>
    <style type="text/css">

        .title {
            border-top-left-radius: 4px;
            border-top-right-radius: 4px;
        }

        .title:hover {
            cursor: move;
        }

        .footer {
            border-bottom-left-radius: 4px;
            border-bottom-right-radius: 4px;
        }

        .table-detail {
            border: none;
        }

        .table-detail tr td {
            height: 30px;
        }

        #tablestyle {
            width: 100%;
            height: 100%;
            border: 0px;
            padding: 0px;
            margin: 0px;
            border-spacing: 0px;
            table-layout: fixed;
        }

        td.tabletdstyle {
            border: 1px solid #9ECAD8;
            padding: 0px;
            margin: 0px;
            border-spacing: 0px;
        }

        #ProgramTable {
            width: 100%;
        }

        .z-textbox-text-invalid {
            background: #FFF repeat-x 0 0;
            background-image: url(/img/misc/text-bg-invalid.gif);
            border-color: #DD7870;
        }

    </style>
</head>
<body style="margin: 0px;">
    <form id="form1" name="form" runat="server">

        <asp:HiddenField ID="UserID" runat="server" />
        <table id="tablestyle">
            <tr>
                <td class="tabletdstyle" style="width: 58%; vertical-align: top;">
                    <div style="min-width:400px;">
                        <table class="table-detail" style="width: 100%;table-layout: fixed;">
                            <tr>
                                <td style="text-align: right;">
                                    <span style="color: #FF0000">同工編號</span>
                                </td>
                                <td style="width: 530px;">
                                    <input id="txApplier" type="text" style="width: 150px;" />
                                    <input id="txApplierName" type="text" disabled="disabled" style="background: #ECEAE4; color: black; width: 300px;" />
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right;">
                                    <span>分機</span>
                                </td>
                                <td>
                                    <input id="txCallNo" type="text" style="width: 150px;" />
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right;">
                                    <span style="color: #FF0000">手機號碼</span><br />
                                    <span>格式: 0935123456</span>
                                </td>
                                <td>
                                    <input id="txMobile" type="text" style="width: 150px;" />
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right;">
                                    <span style="color: #FF0000">節目代號</span>
                                </td>
                                <td style="vertical-align: middle; white-space: nowrap;">
                                    <input id="txProgramID" type="text" style="width: 150px;" />
                                    <input id="btProgramQueryShow" class="ui-button ui-corner-all" type="button" value="查詢節目" />
                                    <input id="txProgramAbbrev" type="text" disabled="disabled" style="background: #ECEAE4; color: black; width: 210px;" />
                                    <input id="txProgramLength" type="text" style="display: none" />
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right;">
                                    <span style="color: red">製作類別</span>
                                </td>
                                <td>
                                    <span>
                                        <input id="txClassification" type="text" style="width: 310px; color: black;" readonly="readonly" />
                                        <input id="txClassificationCode" type="text" style="display: none" />
                                        <img id="btnclassClassification" src="../img/spacer.gif" style="width: 17px; border-width: 0px 0px 1px 1px; border-bottom-style: solid; border-bottom-color: #86A4BE; background-image: url('/img/button/bandbtn.gif'); background-repeat: no-repeat; vertical-align: top; height: 18px; border-left-style: solid; border-left-color: #86A4BE;"
                                            class="ui-button" />
                                    </span>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right;">
                                    <span>說明</span>
                                </td>
                                <td>
                                    <textarea id="ttDescription" style="WIDTH: 98%" rows="8"></textarea>
                                </td>
                            </tr>
                        </table>
                    </div>
                </td>
                <td class="tabletdstyle" style="width: 38%; vertical-align: top;">
                    <table style="table-layout: fixed;">
                        <tr>
                            <td style="padding-left: 5px;">
                                <label style="color: red;">集數</label>
                                <input id="txEpisode" type="text" style="WIDTH: 40px" />
                                <input id="btEpisodeAdd" class="ui-button ui-corner-all" type="button" value="加入" />
                                <label>(雙擊可刪除)</label>
                            </td>
                            <td style="text-align: right; padding-right: 5px;">
                                <input id="btSave" class="ui-button ui-corner-all" type="button" value="確定" />
                                <input id="btCancel" class="ui-button ui-corner-all" type="button" value="取消" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <div id="divTable" style="width: 100%; overflow-x: hidden; overflow-y: auto;">
                                    <table id="tbLv" class="table-list" style="width: 100%; table-layout: fixed;">
                                        <tr>
                                            <th style="width: 100px">節目代號</th>
                                            <th>節目簡稱</th>
                                            <th>製作類別</th>
                                            <th style="width: 10%">集數</th>
                                            <th style="WIDTH: 0px; display: none;">ClassificationCode</th>
                                        </tr>
                                    </table>
                                </div>

                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>

    <div id="ProgramFormMark" style="background-color: #E0E1E3; display: none; width: 100%; height: 100%; position: absolute; left: 1px; top: 1px; right: 0px; bottom: 0px;filter:alpha(opacity=60);opacity:0.6;" ></div>
    <div id="ProgramForm" style="WIDTH: 850px; POSITION: absolute; LEFT: 200px; TOP: 150px; display: none; height: 475px;">

            <table style="border: 1px solid #0B5CA0; width: 100%; empty-cells: show; background-color: #5EABDB; height: 100%; border-radius: 6px;" >
                <tr>
                    <td class="title" style="height: 16px; color: white;">節目資料查詢</td>
                </tr>
                <tr>
                    <td class="footer">
                        <table style="border-color: #0B5CA0; border-style: solid; border-width: 1px; background-color: white; width: 100%; height: 100%;">
                            <tr>
                                <td>
                                    <span>節目代號</span>
                                    <input id="queryProgramID" type="text" style="width: 150px;" />
                                    <span>節目名稱</span>
                                    <input id="queryProgramAbbrev" type="text" style="width: 150px;" />
                                    <input id="btProgramQuery" class="ui-button ui-corner-all" type="button" value="查詢" />
                                </td>

                            </tr>
                            <tr style="height: 412px">
                                <td style="border: 2px solid #86A4BE; vertical-align: top">
                                    <div style="overflow: auto; width: 100%; height: 412px;">
                                        <table id="ProgramTable" class="table">
                                            <thead>
                                                <tr class="headerRow">
                                                    <th style="WIDTH: 10%"><span>節目代號</span></th>
                                                    <th style="WIDTH: 35%"><span>節目名稱</span></th>
                                                    <th style="WIDTH: 20%"><span>節目簡稱</span></th>
                                                    <th style="WIDTH: 35%"><span>節目英文名稱</span></th>
                                                </tr>
                                            </thead>
                                            <tbody>

                                            </tbody>
                                        </table>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right">
                                    <input id="ProgramOk" class="ui-button ui-corner-all" type="button" value="確定" style="visibility: hidden" />
                                    <input id="ProgramCancel" class="ui-button ui-corner-all" type="button" value="取消" />

                                </td>
                            </tr>

                        </table>
                    </td>
                </tr>
            </table>

        </div>

        <div id="ClassificationForm" style="border: 1px solid #86A4BE; background-color: white; padding: 2px; margin: 0px; WIDTH: 330px; position: absolute; top: 376px; left: 350px; display: none;">
            <table id="tbClassification" class="table" style="border: 1px solid #86A4BE; padding: 0px; margin: 0px; width: 100%;" border="0" cellSpacing="0" cellPadding="0">
                <thead>
                <tr class="headerRow">
                    <th style="WIDTH: 144px"><span>製作類別</span></th>
                    <th style="WIDTH: 84px"><span>標準時間</span></th>
                    <th style="WIDTH: 84px"><span>基本費用</span></th>
                    <th style="WIDTH: 0px; display: none;"></th>
                </tr>
                </thead>
                <tbody></tbody>
            </table>
        </div>

    </form>
</body>
</html>

