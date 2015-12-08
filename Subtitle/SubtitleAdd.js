"use strict";

//今天日期
var Today = new Date();
var yyyymmdd = Today.getFullYear() + '/' + padLeft(Today.getMonth() + 1, 2) + '/' + padLeft(Today.getDate(), 2);
//字幕申請累積筆數
var RecordCount = 0;

$(document).ready(function () {

    //可移動視窗
    $("#ProgramForm").draggable({ handle: ".title", opacity: 0.5 });

    // 取得資料表格高度
    $("#divTable").css('height', $(window).height() - (Math.round(($("#divTable").position().top) + 20)));
    $("#tablestyle").css('height', $(window).height() - (Math.round(($("#tablestyle").position().top) + 20)));
    FormPosition("#ProgramForm");

    $(window).resize(function () {
        $("#divTable").css('height', $(window).height() - (Math.round(($("#divTable").position().top) + 20)));
        $("#tablestyle").css('height', $(window).height() - (Math.round(($("#tablestyle").position().top) + 20)));
        FormPosition("#ProgramForm");
    });

    $("#tablestyle").mousedown(function () {
        $("#ClassificationForm").hide();
    });

    //各欄位觸發
    //同工編號
    $("#txApplier").blur(function () {

        if ($("#txApplier").val() == "") {
            $("#txApplier").addClass("z-textbox-text-invalid");
            alert('申請人不可空白！');
            return false;
        }
        var ApplierName = getApplier_ajax($("#txApplier").val());
        if (ApplierName == "") {
            $("#txApplierName").val("");
            $("#txApplier").addClass("z-textbox-text-invalid");
            alert('申請人代號錯誤！');
            return false;
        }
        else {
            $("#txApplierName").val(ApplierName);
            $("#txApplier").removeClass("z-textbox-text-invalid");
        }

    });

    //分機
    $("#txCallNo").blur(function () {

        //檢查數字欄位值樣本
        var checkNumber = /^\d+$/;
        if ($("#txCallNo").val() != "") {
            if ($("#txCallNo").val().length != 4 || !checkNumber.test($("#txCallNo").val())) {
                alert('分機必需為4碼數字！');
                return false;
            }
        }

    });

    //手機號碼
    $("#txMobile").blur(function () {

        if ($("#txMobile").val() == "" || $("#txMobile").val().substring(0, 2) != "09" || $("#txMobile").val().length != 10) {
            alert('手機格式錯誤!  請以09開頭，總共10位數字！');
            return false;
        }

    });

    //節目代號
    $("#txProgramID").blur(function () {

        if ($("#txProgramID").val() == "") {
            alert('節目代號不可空白！');
            return false;
        }
        var ProgramName = getProgramName_ajax($("#txProgramID").val());
        if (ProgramName == "err") {
            $("#txProgramAbbrev").val("");
            alert('無法連接資料庫！');
            return false;
        }
        else if (ProgramName == "no")
        {
            $("#txProgramAbbrev").val("");
            alert('節目代號錯誤！');
            return false;
        }

    });

    //查詢字幕人員按鈕
    $("#btProgramQueryShow").click(function () {

        //alert("QueryEditor");
        $("#ProgramFormMark").show();
        $("#ProgramForm").show();

        $("#queryProgramID").focus();
        
    });

    //製作類別
    $("#txClassification").click(function () {
        $("#btnclassClassification").click();
    });

    //製作類別按鈕
    $("#btnclassClassification").click(function () {

        //alert("Classification");

        var ClassificationForm = document.getElementById("ClassificationForm");

        if (ClassificationForm.style.display == 'none') {
            var Classificationok = GetClassification_ajax();
            if (Classificationok) {
                //alert("GetClassification_ajax ok");
            }
            else {
                alert("無法連到資料庫！");
            }
        }
        else {

            $("#ClassificationForm").hide();
        }

    });

    //節目資料查詢 取消按鈕
    $("#ProgramCancel").click(function () {

        //alert("CancelButton");
        $("#ProgramFormMark").hide();
        $("#ProgramForm").hide();
        $("#queryProgramID").val("");
        $("#queryProgramAbbrev").val("")
        $("#ProgramTable").children("tbody").html("");

    });

    //節目資料查詢 查詢
    $("#btProgramQuery").click(function () {

        var GetProgram_ok = getProgram_ajax();

    });

    //直接在節目集數欄位按Enter鍵
    $("#txEpisode").bind("keypress", function (e) {

        if (e.which == 13) {
            $("#btEpisodeAdd").click();
        }

    });

    //加入按鈕
    $("#btEpisodeAdd").click(function () {

        //檢查數字欄位值樣本
        var checkNumber = /^\d+$/;

        //同工編號
        if ($("#txApplier").val() == "" || $("#txApplierName").val() == "") {
            alert('申請人不可空白！');
            return false;
        }

        //分機
        if ($("#txCallNo").val() != "") {
            if ($("#txCallNo").val().length != 4 || !checkNumber.test($("#txCallNo").val())) {
                alert('分機必需為4碼數字！');
                return false;
            }
        }

        //手機號碼
        if ($("#txMobile").val() == "" || $("#txMobile").val().substring(0, 2) != "09" || $("#txMobile").val().length != 10) {
            alert('手機格式錯誤!  請以09開頭，總共10位數字！');
            return false;
        }

        //節目代號
        if ($("#txProgramID").val() == "" || $("#txProgramAbbrev").val() == "") {
            alert('節目代號不可空白！');
            return false;
        }

        //製作類別
        if ($("#txClassification").val() == "" || $("#txClassificationCode").val() == "") {
            alert('製作類別不可空白！');
            return false;
        }

        //製作類別
        if ($("#txClassification").val() == "" || $("#txClassificationCode").val() == "") {
            alert('製作類別不可空白！');
            return false;
        }

        //集數
        if ($("#txEpisode").val() == "") {
            alert('節目集數必須填入！');
            return false;
        }
        if (!checkNumber.test($("#txEpisode").val())) {
            alert('節目集數必需為數字！');
            return false;
        }

        //檢查申請資料是否重覆輸入資料
        var checkAdding_ok = checkAdding($("#txEpisode").val());
        if (checkAdding_ok) {
            alert("重覆輸入資料！");
            return false;
        }

        //檢查申請資料是否已經存在
        var checkSubtitle_ajax_ok = checkSubtitle_ajax();
        if (checkSubtitle_ajax_ok == "err") {
            alert("無法連到資料庫！");
            return false;
        }
        else if (checkSubtitle_ajax_ok != "") {
            alert("申請資料已經存在！");
            return false;
        }

        InsertSubtitleRow();

    });

    //儲存按鈕
    $("#btSave").click(function () {
        //alert("btSave 儲存");
        //檢查是否有加入節目資料(右邊的表格)

        //請至少申請一筆字幕才能儲存
        if (RecordCount <= 0) {
            alert("請至少申請一筆字幕才能儲存！");
            return false;
        }
        var table = html2json();
        //var table = $('#tbLv').tableToJSON();
        //alert(JSON.stringify(table));

        //儲存字幕資料
        //字幕申請完發送email
        var AddSubtitle_ajax_count = AddSubtitle_ajax(table);
   
        //儲存字幕資料訊息
        if (AddSubtitle_ajax_count > 0) {

            alert("儲存成功！");
        
            //畫面重置
            location.reload();
        }
        else {
            alert("儲存失敗！");
        }
        return false;
    });

    //取消按鈕
    $("#btCancel").click(function () {

        $("#ClassificationForm").hide();

        //alert("CancelButton");
        //if (UpdateFlag) {

            if (!confirm("確定要放棄編輯的資料？"))
            {
                return false;
            }

        //}
  
        //畫面重置
        location.reload();
      
    });

});

//設定視窗的顯示位置
function FormPosition(FormName) {

    var EditFormleft = (parseInt($(window).width()) / 2) - (parseInt($(FormName).width()) / 2);
    var EditFormtop = (parseInt($(window).height()) / 2) - (parseInt($(FormName).height()) / 2);
    if (EditFormleft < 0) {
        EditFormleft = 0;
    }
    if (EditFormtop < 0) {
        EditFormtop = 0;
    }
    $(FormName).css('left', EditFormleft);
    $(FormName).css('top', EditFormtop);

}

//取得申請人
function getApplier_ajax(ApplierID) {

    var strData = "{'EMPLOYEE_NO':'" + ApplierID + "'}";
    var strApplierName = "";

    $.ajax({
        type: "POST",
        url: "../HRService.asmx/GetEmployee",
        data: strData,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (json) {
            var mydata = $.parseJSON(json.d);
            if (mydata.length > 0) {
                strApplierName = mydata[0].EMPLOYEE_CNAME;
            }
        },
        failure: function (ex) {
            alert(ex.get_message);
        }
    });

    return strApplierName;

}

//取得節目名稱
function getProgramName_ajax(ProgramID) {

    var strData = "{'ProgramID':'" + ProgramID + "'}";
    var ok = "err";

    $.ajax({
        type: "POST",
        url: "../SubtitleWebService.asmx/GetProgramName",
        data: strData,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (json) {
            var mydata = $.parseJSON(json.d);
            if (mydata.length > 0) {
                $("#txProgramID").val(mydata[0].ProgramID);
                $("#txProgramAbbrev").val(mydata[0].ProgramAbbrev);
                $("#txProgramLength").val(mydata[0].ProgramLength);
                ok = "yes";
            }
            else {
                ok = "no";
            }
        },
        failure: function (ex) {
            alert(ex.get_message);
        }
    });
    return ok;
}

//查詢節目名稱
function getProgram_ajax() {

    var strData = "{'ProgramID':'" + $("#queryProgramID").val() + "','ProgramAbbrev':'" + $("#queryProgramAbbrev").val() + "'}";

    $.ajax({
        type: "POST",
        url: "../SubtitleWebService.asmx/GetProgramData",
        data: strData,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: true,
        success: function (json) {
            var mydata = $.parseJSON(json.d);
            InsertProgramRow(mydata);
        },
        beforeSend: function (e) {
            $("#btProgramQuery").css("cursor", "progress");
        },
        complete: function (e) {
            $("#btProgramQuery").css("cursor", "pointer");
        },
        failure: function (ex) {
            alert(ex.get_message);
        }
    });

    return;
}

//顯示節目名稱
function InsertProgramRow(data) {

    var tr = "";
    for (var i = 0; i < data.length; i++) {
        // tr begin
        tr += '<tr style="cursor: pointer;" ondblclick="ProgramEdit(this)">';
        // td begin
        tr += '<td><span>' + data[i].ProgramID + '</span></td>';
        tr += '<td><span>' + data[i].ProgramTitle + '</span></td>';
        tr += '<td><span>' + data[i].ProgramAbbrev + '</span></td>';
        tr += '<td><span>' + data[i].programEnglishTitle + '</span></td>';
        tr += '<td style="WIDTH: 0px; display: none;"><span>' + data[i].ProgramLength + '</span></td>';
        // td end
        tr += '</tr>';
        // tr end
    }
    //alert(tr);

    $("#ProgramTable").children("tbody").html(tr);

    return;
}

//點選節目名稱視窗的清單及帶回父視窗
function ProgramEdit(e) {

    //alert("ClassificationEdit");
    var Cell0 = $(e).find('td:eq(0)').text();
    var Cell2 = $(e).find('td:eq(2)').text();
    var Cell4 = $(e).find('td:eq(4)').text();
    //製作類別
    $("#txProgramID").val(Cell0);
    $("#txProgramAbbrev").val(Cell2);
    $("#txProgramLength").val(Cell4);
    //製作類別視窗隱藏
    $("#ProgramFormMark").hide();
    $("#ProgramForm").hide();

    return;
}

//取得製作類別
function GetClassification_ajax() {

    var ok = false;

    $.ajax({
        type: "POST",
        url: "../SubtitleWebService.asmx/GetClassification",
        data: {},
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (json) {
            var mydata = $.parseJSON(json.d);
            InsertClassificationRow(mydata);
            if (mydata.length > 0) {
            }
            else {
                alert("查無資料！");
            }

            ok = true;
        },

        failure: function (ex) {
            alert(ex.get_message);
        }
    });


    return ok;
}

//取得製作類別後顯示視窗選項
function InsertClassificationRow(data) {

    var position = $("#txClassification").offset();
    var x = position.left;
    var y = position.top;
    //var position2 = $("#ClassificationForm");
    var position2 = document.getElementById("ClassificationForm")
    position2.style.left = x + "px";
    position2.style.top = (y + 20) + "px";

    var tr = "";
    for (var i = 0; i < data.length; i++) {
        // tr begin
        tr += '<tr style="cursor: pointer;" class="bodyRow" onclick="ClassificationEdit(this)" title="' + data[i].Description + '">';
        // td begin
        tr += '<td><span>' + data[i].ClassificationName + '</span></td>';
        tr += '<td><span>' + data[i].Length + '</span></td>';
        tr += '<td><span>' + data[i].Amount + '</span></td>';
        tr += '<td style="WIDTH: 0px; display: none;"><span>' + data[i].Classification + '</span></td>';
        // td end
        tr += '</tr>';
        // tr end
    }
    //alert(tr);

    $("#tbClassification").children("tbody").html(tr);
    $("#ClassificationForm").show();

    return;
}

//點選製作類別視窗的清單及帶回父視窗
function ClassificationEdit(e) {

    //alert("ClassificationEdit");
    var Cell0 = $(e).find('td:eq(0)').text();
    var Cell3 = $(e).find('td:eq(3)').text();
    //製作類別
    $("#txClassification").val(Cell0);
    $("#txClassificationCode").val(Cell3);
    //製作類別視窗隱藏
    $("#ClassificationForm").hide();

    return;
}

//檢查是否有字幕申請
function checkSubtitle_ajax() {

    var result = "err";
    var strData = "{'ProgramID':'" + $("#txProgramID").val() + "', 'Episode':" + $("#txEpisode").val() + "}";

    $.ajax({
        type: "POST",
        url: "../SubtitleWebService.asmx/CheckExistSubtitle",
        data: strData,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (res) {
            //var mydata = $.parseJSON(json.d);
            var mydata = res.d;
            if (mydata.length > 0) {
                result = mydata;
            }
            else {
                result = "";
            }
        },

        failure: function (ex) {
            alert(ex.get_message);
        }
    });
    return result;

}

//加入字幕申請
function InsertSubtitleRow() {

    var tr = "";
    // tr begin
    tr += '<tr style="cursor: pointer;" ondblclick="ProgramDelete(this);">';
    // td begin
    tr += '<td><span>' + $("#txProgramID").val() + '</span></td>';
    tr += '<td><span>' + $("#txProgramAbbrev").val() + '</span></td>';
    tr += '<td><span>' + $("#txClassification").val() + '</span></td>';
    tr += '<td><span>' + $("#txEpisode").val() + '</span></td>';
    tr += '<td style="WIDTH: 0px; display: none;"><span>' + $("#txClassificationCode").val() + '</span></td>';
    // td end
    tr += '</tr>';
    // tr end
    //alert(tr);
    $('#tbLv tr:last').after(tr);

    RecordCount++;
    CheckLocked();
    $("#txEpisode").val("");
    $("#txEpisode").focus();

    return;
}

//鎖定狀態
function LockedHide() {

    $("#btProgramQueryShow").addClass("ui-state-disabled");
    $("#btProgramQueryShow").attr('disabled', 'disabled');

    $("#txApplier").attr('disabled', 'disabled');
    $("#txApplier").css("background", "#ECEAE4");
    $("#txApplier").css("color", "black");

    $("#txCallNo").attr('disabled', 'disabled');
    $("#txCallNo").css("background", "#ECEAE4");
    $("#txCallNo").css("color", "black");

    $("#txMobile").attr('disabled', 'disabled');
    $("#txMobile").css("background", "#ECEAE4");
    $("#txMobile").css("color", "black");

    $("#txProgramID").attr('disabled', 'disabled');
    $("#txProgramID").css("background", "#ECEAE4");
    $("#txProgramID").css("color", "black");

}

//未鎖定狀態
function LockedShow() {

    $("#btProgramQueryShow").removeAttr('disabled');
    $("#btProgramQueryShow").removeClass("ui-state-disabled");

    $("#txApplier").removeAttr('disabled');
    $("#txApplier").css("background", "white");

    $("#txCallNo").removeAttr('disabled');
    $("#txCallNo").css("background", "white");

    $("#txMobile").removeAttr('disabled');
    $("#txMobile").css("background", "white");

    $("#txProgramID").removeAttr('disabled');
    $("#txProgramID").css("background", "white");

}

function ProgramDelete(e) {
    $(e).remove();
    RecordCount--;
    CheckLocked();
}

//檢查鎖定狀態
function CheckLocked() {

    if (RecordCount > 0) {
        LockedHide();
    }
    else {
        LockedShow();
    }

}

function AddSubtitle_ajax(table) {

    var result = "err";
    var strData = "{'tables':" + JSON.stringify(table) + "}";

    $.ajax({
        type: "POST",
        url: "../SubtitleWebService.asmx/InsertSubtitle",
        data: strData,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (res) {
            result = res.d;
        },

        failure: function (ex) {
            alert(ex.get_message);
        }
    });
    return result;

}

//發送內部錯誤Email(非同步傳送)
function SendErrorEmail(EmailFromName, Subject, Body) {

    var strData = "{'EmailFromName':'" + EmailFromName + "', 'Subject':'" + Subject
    + "', 'Body':'" + Body + "'}";

    $.ajax({
        type: "POST",
        url: "../SubtitleWebService.asmx/SendErrorEmail",
        data: strData,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: true,
        success: function (json) {
        }
    });

}

function html2json() {

    var ProgramID = $("#txProgramID").val();
    var ProgramAbbrev = $("#txProgramAbbrev").val();
    var ProgramLength = $("#txProgramLength").val();
    var CallNo = $("#txCallNo").val();
    var Mobile = $("#txMobile").val();
    var Description = $("#ttDescription").val();
    var UserID = $("#UserID").val();
    var Requester = $("#txApplier").val() + " " + $("#txApplierName").val();
    var json = "{'ProgramID':'" + ProgramID + "','ProgramAbbrev':'" + ProgramAbbrev + "','CallNo':'" + CallNo +
        "','Mobile':'" + Mobile + "','Description':'" + Description + "','ProgramLength':'" + ProgramLength +
        "','UserID':'" + UserID + "','Requester':'" + Requester + "','table':[";
    var otArr = [];
    var tbl2 = $('#tbLv tr:not(:first)').each(function (i) {
        var x = $(this).children();
        var itArr = [];
        x.each(function (j) {
            if (j == 3) {
                itArr.push("'Episode':'" + $(this).text() + "'");
            }
            else if (j == 4) {
                itArr.push("'ClassificationCode':'" + $(this).text() + "'");
            }
        });
        otArr.push("{" + itArr.join(',') + "}");
    })
    json += otArr.join(",") + "]}"

    return json;
}

function checkAdding(Episode) {

    var ok = false;

    var tbl2 = $('#tbLv tr:not(:first)').each(function (i) {
        var x = $(this).children();
        x.each(function (j) {
            if (j == 3) {
                if ($(this).text() == Episode) {
                    ok = true;
                }
            }
        });
    })

    return ok;

}

