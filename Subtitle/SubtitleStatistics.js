"use strict";

//功能按鈕群組高度
var divsouth_height = 0;
//狀態類別(查詢=query,對帳=account,結帳=accounted)
var mode = "query";
//查詢類別(依匯入日期=import,依結帳年月=bill)
var query_type = "import";

$(document).ready(function () {

    // 取得資料表格高度
    CenterFormPosition();

    $(window).resize(function () {
        CenterFormPosition();
    });

    $("#uiSDate").datepicker();
    $("#uiEDate").datepicker();

    //匯出 Excel
    $("#uiExportQuery").click(function () {

        if (query_type == "import") {
            //依匯入日期
            location.href = "../Subtitle/ExportExcel.aspx?type=1&SDate=" + $("#uiSDate").val() + "&EDate=" + $("#uiEDate").val() +
                "&DeptID=" + $("#uiDept").val();
        }
        else {
            //依結帳年月
            location.href = "../Subtitle/ExportExcel.aspx?type=3&Year=" + $("#uiYear").val() + "&Month=" + $("#uiMonth").val() +
                "&DeptID=" + $("#uiDept").val();
        }

    });

    //對帳 結帳並匯出 Excel
    $("#uiExportAccount").click(function () {

        if (confirm("每月只能結帳一次，本動作無法復原，確定結帳？")) {
            location.href = "../Subtitle/ExportExcel.aspx?type=2&Year=" + $("#uiYear").val() + "&Month=" + $("#uiMonth").val() +
                "&DeptID=" + $("#uiDept").val() + "&UserID=" + $("#UserID").val();
            ClearMode();
        }
        
    });

    //結帳 匯出 Excel
    $("#uiExportAccounted").click(function () {

        location.href = "../Subtitle/ExportExcel.aspx?type=4&Year=" + $("#uiYear").val() + "&Month=" + $("#uiMonth").val() +
            "&DeptID=" + $("#uiDept").val();
    });

    //radio 查詢
    $("#uiQuery").click(function () {

        if (ExchangeMode(mode, "query")) {
            $("#uiQueryArea").show();
            $("#uiAccountBox").hide();
            $("#uiQueryBox").show();
            $("#uiQueryAreaImport").prop("checked", true);
        }

    });

    //radio 對帳
    $("#uiAccount").click(function () {

        if (ExchangeMode(mode, "account")) {
            $("#uiQueryArea").hide();
            $("#uiAccountBox").show();
            $("#uiQueryBox").hide();
        }

    });

    //radio 結帳
    $("#uiAccounted").click(function () {

        if (ExchangeMode(mode, "accounted")) {
            $("#uiQueryArea").hide();
            $("#uiAccountBox").show();
            $("#uiQueryBox").hide();
        }

    });

    //radio 依匯入日期
    $("#uiQueryAreaImport").click(function () {

        query_type = "import";
        $("#uiAccountBox").hide();
        $("#uiQueryBox").show();

    });

    //radio 依結帳年月
    $("#uiQueryAreaAccount").click(function () {

        query_type = "bill";
        $("#uiAccountBox").show();
        $("#uiQueryBox").hide();

    });

    //button 查詢
    $("#uiSearch").click(function () {

        var strSDate = "";
        var strEDate = "";
        var intYear = 0;
        var intMonth = 0;
        var strDept = $("#uiDept").val();

        if (mode == "query") {

            if (query_type == "import") {

                //radio 依匯入日期
                if ($("#uiSDate").val() == "") {
                    alert("未輸入開始日！");
                    $("#uiSDate").focus();
                    return false;
                }

                if ($("#uiEDate").val() == "") {
                    alert("未輸入結束日！");
                    $("#uiEDate").focus();
                    return false;
                }

                var uiSDate = new Date($("#uiSDate").val());
                var uiEDate = new Date($("#uiEDate").val());
                var v1 = uiSDate - uiEDate;
                if (uiSDate - uiEDate > 0) {
                    alert("開始日至結束日之間範圍有誤！");
                    $("#uiSDate").focus();
                    return false;
                }
                // 2764800000毫秒 = 32天, 2678400000毫秒 = 31天, 2592000000毫秒 = 30天
                var v2 = uiEDate - uiSDate;
                if (uiEDate - uiSDate > 2678400000) {
                    alert("開始日至結束日之間範圍太大，已超過31天！");
                    $("#uiSDate").focus();
                    return false;
                }

                strSDate = $("#uiSDate").val();
                strEDate = $("#uiEDate").val();

            }
            else if (query_type == "bill") {

                //radio 依結帳年月
                intYear = $("#uiYear").val();
                intMonth = $("#uiMonth").val();

            }

            $("#uiExportQuery").show();
            $("#uiExportAccount").hide();
            $("#uiExportAccounted").hide();

        }
        else if (mode == "account") {

            //radio 對帳 狀態
            intYear = $("#uiYear").val();
            intMonth = $("#uiMonth").val();
            $("#uiExportQuery").hide();
            $("#uiExportAccount").show();
            $("#uiExportAccounted").hide();

        }
        else if (mode == "accounted") {

            //radio 結帳 狀態
            intYear = $("#uiYear").val();
            intMonth = $("#uiMonth").val();
            $("#uiExportQuery").hide();
            $("#uiExportAccount").hide();
            $("#uiExportAccounted").show();

        }
        else {
            return false;
        }

        var GetStatistics_ajax_ok = GetStatistics_ajax(strSDate, strEDate, intYear, intMonth, strDept, mode);

    });

});

function table_detail_expd() {

    //點選細項資料表格+-符號
    $("div.table-detail-img").click(function () {

        if ($(this).attr('class').indexOf("table-detail-expd") != -1) {
            $(this).removeClass("table-detail-expd")
        }
        else
        {
            $(this).addClass("table-detail-expd")
        }

        if ($(this).parent().parent().next('tr').css('display') == "none") {
            $(this).parent().attr('rowspan', '2');
            $(this).parent().parent().next('tr').show();
        }
        else {
            $(this).parent().attr('rowspan', '1');
            $(this).parent().parent().next('tr').hide();
        }

    });
    /*
    //點選細項資料表格+-符號 後續動作
    $("td.table-detail-outer").click(function () {
        
        if ($(this).parent().next('tr').css('display') == "none") {
            $(this).attr('rowspan', '2');
            $(this).parent().next('tr').show();
        }
        else {
            $(this).attr('rowspan', '1');
            $(this).parent().next('tr').hide();
        }

    });
    */

}

function CenterFormPosition() {

    $("#divcenter").css('height', $(window).height() - (Math.round(($("#divcenter").position().top) + divsouth_height + 2)));
    $("#body1").css('height', $("#divcenter").height() - $("#body2").height() - 2);

}

function ExchangeMode(oldMode,newMode) {

    if (oldMode != newMode && divsouth_height > 0) {

        if (!confirm("目前查詢的結果要放棄嗎？")) {
            if (oldMode == "query") {
                $("#uiQuery").prop("checked", true);
            }
            else if (oldMode == "account") {
                $("#uiAccount").prop("checked", true);
            }
            else if (oldMode == "accounted") {
                $("#uiAccounted").prop("checked", true);
            }
            return false;
        }
        //清除查詢結果
        ClearMode();
    }
    mode = newMode;
    return true;

}

function ClearMode() {

    $("#tb1").html('<tr><td style="width: 18px"></td><td>代號</td><td>姓名</td><td>所屬部門</td><td>筆數</td><td>總額</td><td>總評比</td></tr>');
    $("#tb2").html('');
    $("#body2").hide();
    $("#divsouth").hide();
    divsouth_height = 0;
    CenterFormPosition();

}

//查詢結果
function GetStatistics_ajax(strSDate, strEDate, intYear, intMonth, strDept, strMode) {

    //var arForm = $("#form1").serializeArray();
    if (intYear == null || intYear < 0) intYear = 0;
    if (intMonth == null || intMonth < 0) intMonth = 0;
    var strdata = "{'SDate':'" + strSDate + "', 'EDate':'" + strEDate + "', 'Year':" + intYear
        + ", 'Month':" + intMonth + ", 'DeptID':'" + strDept + "', 'Mode':'" + strMode + "'}";

    $.ajax({
        type: "POST",
        url: "../SubtitleWebService.asmx/GetStatistics",
        data: strdata,
        contentType: "application/json; charset=utf-8",
        //contentType:"text/xml; charset=utf-8",
        dataType: "json",
        async: true,

        success: function (json) {
            var mytable = $.parseJSON(json.d);
            if (mytable.length > 0) {
                InsertNewRow(mytable, "#tb1", "#tb2", intYear, intMonth);
                $("#divsouth").show();
                $("#body2").show();
                divsouth_height = 40;
                CenterFormPosition();
                table_detail_expd();
            }
            else {
                alert("查無資料！");
            }

        },
        beforeSend: function (e) {
            $("#uiSearch").css("cursor", "progress");
        },
        complete: function (e) {
            $("#uiSearch").css("cursor", "pointer");
        },
        failure: function (ex) {
            alert(ex.get_message);
        }
    });

    //return;

}

//產出Table多筆欄位格式
function InsertNewRow(data, tableID, tableID2, intYear, intMonth) {

    //字幕筆數
    var intSum = 0;
    //人員筆數
    var intEditor = 0;
    //總成本
    var intCost = 0;

    //var tr = '<table id="tb1" class="table-list2">';
    var tr = '<tr><td style="width: 18px"></td><td>代號</td><td>姓名</td><td>所屬部門</td><td>筆數</td><td>總額</td><td>總評比</td></tr>';
    for (var i = 0; i < data.length; i++) {

        // tr begin
        //tr += '<tr>';

        intEditor++;

        if (i % 2 == 0) {
            tr += '<tr>';
        }
        else {
            tr += '<tr style="background-color: #E6F8FF;">';
        }

        // td begin
        tr += '<td class="table-detail-outer" rowspan="2"><div class="table-detail-img table-detail-expd"></div></td>';
        tr += '<td>' + data[i].EditorID + '</td>';
        tr += '<td>' + data[i].EditorName + '</td>';
        tr += '<td>' + data[i].DeptName + '</td>';
        tr += '<td>' + data[i].SubtitleCostCount + '</td>';
        tr += '<td>' + data[i].SubtitleCostSum + '</td>';
        tr += '<td>' + data[i].SubtitleCompare + '</td>';
        // td end
        tr += '</tr>';
        // tr end

        //計算
        intCost += data[i].SubtitleCostSum;

        if (data[i].table.length > 0) {
            tr += '<tr><td colspan="6" style="padding: 0px;"><table><tr><td>節目代號</td><td>節目簡稱</td><td>集數</td>'+
                        '<td>單集費用</td><td>申請日期</td><td>匯入日期</td><td>結帳年月</td></tr>';
            for (var j = 0; j < data[i].table.length; j++) {
                // tr begin
                tr += '<tr>';
                // td begin
                tr += '<td>' + data[i].table[j].ProgramID + '</td>';
                tr += '<td>' + data[i].table[j].ProgramAbbrev + '</td>';
                tr += '<td>' + data[i].table[j].Episode + '</td>';
                tr += '<td>' + data[i].table[j].SubtitleCostPerEpisode + '</td>';
                tr += '<td>' + data[i].table[j].RequestDate + '</td>';
                tr += '<td>' + data[i].table[j].ImportDate + '</td>';
                tr += '<td>' + data[i].table[j].Bill_Date + '</td>';
                // td end 
                tr += '</tr>';
                // tr end

                //計算
                intSum++;
                //CompareSum += data[i].table[j].SubtitleComparePerEpisode;
            }
            //CompareAvg = CompareAvg / data[i].tables.length;
            tr += '</table></td></tr>';
        }
    }
    //tr += '</table>';
    //alert(tr);

    //總計表格
    var tr2 = '<tr>';
    if (mode == "account" || mode == "accounted") tr2 += '<td>計帳年月</td>'
    tr2 += '<td>字幕筆數</td><td>人員筆數</td><td>總成本</td>';
    if (mode == "account") tr2 += '<td>結帳日期</td>';
    tr2 += '</tr><tr>';
    if (mode == "account" || mode == "accounted") tr2 += '<td>' + intYear + '-' + intMonth + '</td>';
    tr2 += '<td>' + intSum + '</td><td>' + intEditor + '</td><td>' + intCost + '</td>';
    if (mode == "account") {
        var Today = new Date();
        var year = Today.getFullYear().toString();
        var intMonth = Today.getMonth() + 1;
        var month = intMonth > 10 ? intMonth.toString() : '0' + intMonth.toString();
        var day = Today.getDate() > 10 ? Today.getDate().toString() : '0' + Today.getDate().toString();
        tr2 += '<td>' + year + "/" + month + "/" + day + '</td>';
    }
    tr2 += '</tr>';

    $(tableID).html(tr);
    $(tableID2).html(tr2);
    return;
}

