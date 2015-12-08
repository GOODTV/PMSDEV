<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SubtitleHandle.aspx.cs" Inherits="Subtitle_SubtitleHandle" %>

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
    <script type="text/javascript" src="../Subtitle/SubtitleHandle.js"></script>
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

        #tbLv tbody td:first-child {
            text-align: center;
        }

    </style>
</head>
<body>
    <form id="form1" name="form" runat="server">

        <input id="Requester" type="hidden" name="Requester" />
        <asp:HiddenField ID="UserID" runat="server" />
        <section>
            <table id="tbQuery">
                <tr>
                    <td>
                        <label>節目代號</label>
                        <input id="tbProgramID" type="text" style="WIDTH: 100px" value="G002001" />
                    </td>
                    <td>
                        <label>集數</label>
                        <input id="Episode1" type="text" style="WIDTH: 40px" value="1" />
                        <label>~ </label>
                        <input id="Episode2" type="text" style="WIDTH: 40px" value="200" />
                    </td>
                    <td>
                        <input id="DataScope1" name="DataScope" type="radio" checked="checked" value="全部" />
                        <label for="DataScope1">全部</label>
                        <input id="DataScope2" name="DataScope" type="radio" value="近五日" />
                        <label for="DataScope2">近五日</label>
                    </td>
                    <td>
                        <input id="Query" class="ui-button ui-corner-all" type="button" value="搜尋" />
                    </td>
                </tr>
            </table>
        </section>
        <section>
            <p></p>
        </section>
        <section>
            <div id="divTable" style="width: 100%; height: 100%; overflow-x:hidden; overflow-y:auto;">
            <table id="tbLv" class="table" style="width: 100%; ">
                <thead>
                    <tr class="headerRow">
                        <th style="width:15px;"></th>
                        <th>節目代號</th>
                        <th>節目簡稱</th>
                        <th>集數</th>
                        <th>節目長度</th>
                        <th>字幕人員代號</th>
                        <th>字幕人員姓名</th>
                        <th>製作類別</th>
                        <th>單集成本</th>
                        <th>單集評比</th>
                        <th>申請日期</th>
                        <th>匯入日期</th>
                        <th>鎖定日期</th>
                        <th>結帳年月</th>
                    </tr>
                </thead>
                <tbody></tbody>
            </table>
            </div>
        </section>

        <div id="ShadowMark" style="background-color: #E0E1E3; display: none; width: 100%; height: 100%; position: absolute; left: 1px; top: 1px; right: 0px; bottom: 0px;filter:alpha(opacity=60);opacity:0.6;" ></div>
        <div id="EditForm" style="WIDTH: 850px; display: none; position: absolute; left: 250px; top: 80px;">
            <table style="border: 1px solid #9ECAD8; empty-cells: show; background-color: white; border-radius: 6px; table-layout: fixed; position:relative;">
                <tr style="HEIGHT: 25px">
                    <td class="title" style="text-align: left; padding:3px; background-color: #5EABDB; color: #FFFFFF;" colspan="2">字幕編輯</td>
                </tr>
                <tr>
                    <td class="z-border-layout" rowspan="2" style="width: 500px">
                        <table class="z-border">
                            <tr class="bodyRow" style="height: 45px;">
                                <td class="bodyRow" style="width: 150px; text-align:right">
                                    <span>節目代號</span>
                                </td>
                                <td class="bodyRow" style="width: 410px">
                                    <input id="txProgramID" type="text" disabled="disabled" style="background: #ECEAE4; color: black; width: 80px;" />
                                    <input id="txProgramAbbrev" type="text" disabled="disabled" style="background: #ECEAE4; color: black; width: 300px;" />
                                </td>
                            </tr>
                            <tr class="bodyRow" style="height: 45px;" >
                                <td class="bodyRow" style="text-align:right">
                                    <span>集數</span>
                                </td>
                                <td class="bodyRow">
                                    <input id="txEpisode" type="text" disabled="disabled" style="background: #ECEAE4; color: black; width: 80px;" />
                                </td>
                            </tr>
                            <tr class="bodyRow" style="height: 45px;" >
                                <td class="bodyRow" style="text-align:right">
                                    <span style="color: red">節目長度</span>
                                </td>
                                <td class="bodyRow">
                                    <input id="txProgramLength" type="text" disabled="disabled" style="background: #ECEAE4; color: black; width: 80px;" />
                                    <span> 分鐘</span>
                                </td>
                            </tr>
                            <tr class="bodyRow" style="height: 45px;" >
                                <td class="bodyRow" style="text-align:right">
                                    <span style="color: red; white-space: nowrap;">字幕人員代號</span>
                                </td>
                                <td class="bodyRow" style="vertical-align: middle; white-space: nowrap;">
                                    <input id="txEditorID" type="text" style="width: 80px;" />
                                    <input id="btQueryEditor" class="ui-button ui-corner-all" type="button" value="查詢字幕人員" />
                                    <input id="txEditorName" type="text" disabled="disabled" style="background: #ECEAE4; color: black; width: 200px;" />
                                </td>
                            </tr>
                            <tr class="bodyRow" style="height: 45px;" >
                                <td class="bodyRow" style="text-align:right">
                                    <span style="color: red">製作類別</span>
                                </td>
                                <td class="bodyRow">
                                    <span>
                                    <input id="txClassification" type="text" style="width: 310px; color: black;" disabled="disabled" />
                                    <input id="txClassificationCode" type="text" style="display: none" />
                                    <img id="btnclassClassification" src="../img/spacer.gif" style="width: 17px; border-width: 0px 0px 1px 1px; 
                                            border-bottom-style: solid; border-bottom-color: #86A4BE; 
                                            background-image: url('/img/button/bandbtn.gif'); 
                                            background-repeat: no-repeat; vertical-align: top; height: 18px; 
                                            border-left-style: solid; border-left-color: #86A4BE;" class="ui-button" />
                                        </span>
                                </td>
                            </tr>
                            <tr class="bodyRow" style="height: 45px;" >
                                <td class="bodyRow" style="text-align:right">
                                    <span style="color: red">單集字幕成本</span>
                                </td>
                                <td class="bodyRow">
                                    <input id="txSubtitleCostPerEpisode" type="text" style="width: 80px;" />
                                    <span> 元</span>
                                </td>
                            </tr>
                            <tr class="bodyRow" style="height: 45px;" >
                                <td class="bodyRow" style="text-align:right">
                                    <span>單集字幕評比</span>
                                </td>
                                <td class="bodyRow">
                                    <input id="txSubtitleComparePerEpisode" type="text" style="width: 80px;" />
                                </td>
                            </tr>
                            <tr class="bodyRow" style="height: 45px;" >
                                <td class="bodyRow" style="text-align:right">
                                    <span>說明</span>
                                </td>
                                <td class="bodyRow">
                                    <textarea id="ttDescription" style="WIDTH: 380px" rows="4">Description</textarea>
                                </td>
                            </tr>

                            <tr style="padding: 0px; margin: 0px; width:100%;height: 45px;" >
                                <td colspan="2" style="padding: 0px; margin: 0px; width:100%; height: 45px;">
                                    <table style="padding: 0px; margin: 0px; width:100%; height: 45px; empty-cells: show; " >
                                        <tr>
                                            <td class="z-border-layout2" style="width: 16%; text-align:right;" >
                                                <span>申請日期</span>
                                            </td>
                                            <td class="z-border-layout2" style="width: 16%;" >
                                                <span id="sRequestDate">RequestDate</span>
                                            </td>
                                            <td class="z-border-layout2" style="width: 16%; text-align:right;">
                                                <span>完成日期<br />(匯入日期)</span>
                                            </td>
                                            <td class="z-border-layout2" style="width: 20%">
                                               <input id="txImportDate" type="text" disabled="disabled" style="width: 90px; background: #ECEAE4; color: black;" />
                                            </td>
                                            <td class="z-border-layout2" style="width: 16%; text-align:right">
                                                <span>鎖定日期</span>
                                            </td>
                                            <td style="width: 16%">
                                                <span id="sLockedDate">LockedDate</span>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>

                        </table>

                    </td>
                    <td class="z-border-layout" style="text-align:right;">
                        <input id="ViewSubtitleButton" class="ui-button ui-corner-all" type="button" value="檢視字幕內容" />
                        <input id="SubtitleFileButton" class="ui-button ui-corner-all" type="button" value="字幕檔案上傳" />
                    </td>
                </tr>
                <tr>
                    <td class="z-border-layout" style="HEIGHT: 400px; WIDTH: 340px; vertical-align: top; table-layout:fixed;">
                        <textarea id="ttSubtitleDescription" style="HEIGHT: 400px; WIDTH: 340px; background: #ECEAE4; display: none; overflow: auto;" readonly="readonly">ttSubtitleDescription</textarea>
                    </td>
                </tr>
                <tr>
                    <td class="z-border-layout footer" style="text-align: right;" colspan="2">
                        <input id="DeleteButton" class="ui-button ui-corner-all" type="button" value="刪除" />
                        <input id="UnLockButton" class="ui-button ui-corner-all" type="button" value="解鎖" />
                        <input id="LockedButton" class="ui-button ui-corner-all" type="button" value="鎖定" />
                        <input id="ExportButton" class="ui-button ui-corner-all" type="button" value="匯出" />
                        <input id="UpdateButton" class="ui-button ui-corner-all ui-state-disabled" type="button" value="儲存" disabled="disabled" />
                        <input id="CancelButton" class="ui-button ui-corner-all" type="button" value="取消" />
                    </td>
                </tr>
            </table>

        </div>


        <div id="UpLoadFormMark" style="background-color: #E0E1E3; display: none; width: 100%; height: 100%; position: absolute; left: 1px; top: 1px; right: 0px; bottom: 0px;filter:alpha(opacity=60);opacity:0.6;" ></div>
        <div id="UpLoadForm" style="WIDTH: 360px; POSITION: absolute; LEFT: 506px; TOP: 150px; display: none;">

            <table style="border: 1px solid #0B5CA0; width: 100%; empty-cells: show; background-color: #5EABDB; border-radius: 6px;">
                <tr>
                    <td class="title" style="height: 16px; color: white;">字幕檔案資料編輯</td>
                </tr>
                <tr>
                    <td class="footer">
                        <table style="border-color: #0B5CA0; border-style: solid; border-width: 1px; background-color: white; width: 100%;">
                            <tr>
                                <td>
                                    <span>請選擇要上傳的字幕檔案</span>
                                </td>

                            </tr>
                            <tr>
                                <td>
                                    <input id="upFile" type="file" style="width: 100%" />
                                </td>

                            </tr>
                            <tr>
                                <td>
                                    <input id="uploadfile" class="ui-button ui-corner-all" type="button" value="上傳" />
                                    <input id="uploadcancel" class="ui-button ui-corner-all" type="button" value="取消" />
                                </td>

                            </tr>

                        </table>
                    </td>
                </tr>
            </table>

    </div>


    <div id="EditorFormMark" style="background-color: #E0E1E3; display: none; width: 100%; height: 100%; position: absolute; left: 1px; top: 1px; right: 0px; bottom: 0px;filter:alpha(opacity=60);opacity:0.6;" ></div>
    <div id="EditorForm" style="WIDTH: 900px; POSITION: absolute; LEFT: 200px; TOP: 150px; display: none; height: 500px;">

            <table style="border: 1px solid #0B5CA0; width: 100%; empty-cells: show; background-color: #5EABDB; height: 100%; border-radius: 6px;" >
                <tr>
                    <td class="title" style="height: 16px; color: white;">字幕人員查詢</td>
                </tr>
                <tr>
                    <td class="footer">
                        <table style="border-color: #0B5CA0; border-style: solid; border-width: 1px; background-color: white; width: 100%; height: 100%;">
                            <tr>
                                <td>
                                    <span>字幕人員姓名</span>
                                    <input id="EditorQueryInput" type="text" />
                                    <input id="EditorQuery" class="ui-button ui-corner-all" type="button" value="查詢" />
                                </td>

                            </tr>
                            <tr style="height: 412px">
                                <td style="border: 2px solid #86A4BE; vertical-align: top">
                                    <div style="overflow: auto; width: 100%; height: 412px;">
                                        <table id="EditorTable" class="table" style="border: 1px solid #86A4BE; padding: 0px; margin: 0px; width: 100%;" border="0" cellSpacing="0" cellPadding="0">
                                            <thead>
                                                <tr class="headerRow">
                                                    <th style="WIDTH: 10%"><span>字幕人員代號</span></th>
                                                    <th style="WIDTH: 35%"><span>字幕人員姓名</span></th>
                                                    <th style="WIDTH: 15%"><span>字幕人員部門</span></th>
                                                    <th style="WIDTH: 30%"><span>連絡電話</span></th>
                                                    <th style="WIDTH: 10%"><span>評比</span></th>
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
                                    <input id="EditorOk" class="ui-button ui-corner-all" type="button" value="確定" style="visibility: hidden" />
                                    <input id="EditorCancel" class="ui-button ui-corner-all" type="button" value="取消" />

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

        <div id="export-dialog-confirm"></div>

    </form>
</body>
</html>

