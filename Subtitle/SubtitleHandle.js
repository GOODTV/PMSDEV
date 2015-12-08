"use strict";

// 用來判斷是否有上傳字幕檔案
var doUploading = false;
//用來判斷是否要發送字幕有轉拋信件
var sendSrtUpdateEmail = false;
//今天日期
var Today = new Date();
var yyyymmdd = Today.getFullYear() + '/' + padLeft(Today.getMonth() + 1, 2) + '/' + padLeft(Today.getDate(), 2);
//有SRT字幕欄位 = 表示不是第一次上傳字幕檔(不是第一次上傳字幕檔，又轉拋字幕 => 發送Email 通知主控人員有轉拋字幕)
var hasSRT = false;
//有TC字幕欄位
var hasTXT = false;
//檢查SRT字幕的時間格式
var checkSRT = false;
//無時間格式的字幕
var checkTXT = false;
//進度狀態
var Status = "";
//檢查瀏覽器是否支援File API
if (window.File && window.FileReader && window.FileList && window.Blob) {
    //alert("support");
} else {
    alert('該瀏覽器無法支援上傳檔案作業！');
}

$(document).ready(function () {

    // 從Web Service回傳節目代碼給節目代碼輸入欄位的AutoComplete
    //SubtitleWebService.GetProgramID(GetProgramIDonSuccess, onFailed);
    var ok = GetProgramID_ajax();

    if (ok) {
        //alert("節目代碼 AutoComplete ok");
    }
    else {
        alert("節目代碼 AutoComplete err");
    }

    // 取得資料表格高度
    $("#divTable").css('height', $(window).height() - (Math.round(($("#divTable").position().top) + 20)));
    FormPosition("#EditForm");

    //可移動視窗
    $("#EditForm").draggable({ handle: ".title", opacity: 0.5 });

    $("#UpLoadForm").draggable({ handle: ".title", opacity: 0.5 });

    $("#EditorForm").draggable({ handle: ".title", opacity: 0.5 });

    $(window).resize(function () {
        $("#divTable").css('height', $(window).height() - (Math.round(($("#divTable").position().top) + 20)));
        FormPosition("#EditForm");
    });

    /*
    $("#EditForm .title").mousedown(function () {
        $("#ClassificationForm").hide();
    });
    */
    $("#EditForm").mousedown(function () {
        $("#ClassificationForm").hide();
    });

    $("#txImportDate").datepicker();

    //搜尋按鈕
    $("#Query").click(function () {

        var intEpisode1 = 0;
        var intEpisode2 = 0;

        if ($("#Episode1").val() != "") {
            var p = parseInt($("#Episode1").val());
            if (isNaN(p) || p <= 0) {
                alert("請輸入正整數");
                $("#Episode1").focus();
                return false;
            }
            else {
                intEpisode1 = parseInt($("#Episode1").val());
            }
        }

        if ($("#Episode2").val() != "") {
            var p = parseInt($("#Episode2").val());
            if (isNaN(p) || p <= 0) {
                alert("請輸入正整數");
                $("#Episode2").focus();
                return false;
            }
            else {
                intEpisode2 = parseInt($("#Episode2").val());
            }
        }

        //var DataScope = $("input[type=radio][checked=checked]").val();
        var DataScope = $("input[name=DataScope]:checked").val();
        if (DataScope == "全部" && $("#tbProgramID").val() == "") {

            alert("請輸入節目代碼");
            $("#tbProgramID").focus();
            return false;
        }

        var intDataScope = DataScope == "全部" ? 0 : 5;
        var strProgramID = $("#tbProgramID").val();

        GetSubtitle_ajax(strProgramID, intEpisode1, intEpisode2, intDataScope);

    });

    //直接在節目代號欄位按Enter鍵搜尋
    $("#tbProgramID").bind("keypress", function (e) {

        if (e.which == 13) {
            //alert(e.which);
            $("#Query").click();
        }


    });

    //集數欄位按Enter鍵搜尋
    $("#Episode1").bind("keypress", function (e) {

        if (e.which == 13) {
            //alert(e.which);
            $("#Query").click();
        }


    });
    $("#Episode2").bind("keypress", function (e) {

        if (e.which == 13) {
            //alert(e.which);
            $("#Query").click();
        }


    });

    //刪除按鈕
    $("#DeleteButton").click(function () {

        //alert("LockedButton");
        if (confirm("確定要刪除？")) {

            var ok = SubtitleDelete_ajax($("#txProgramID").val(), $("#txEpisode").val());
            if (ok) {
                //alert("ajax ok");
                alert("刪除成功！");
            }
            else {
                alert("刪除失敗！");
            }

            $("#CancelButton").click();
            //查詢畫面更新
            $("#Query").click();


        }

    });

    //鎖定按鈕
    $("#LockedButton").click(function () {

        //alert("LockedButton");
        if (confirm("確定要鎖定？")) {

            var ok = UpdateLocked_ajax($("#txProgramID").val(), $("#txEpisode").val());
            if (ok) {
                //alert("ajax ok");
                $("#sLockedDate").text(yyyymmdd);
                LockedHide();
                //alert("鎖定成功！");
            }


            //查詢畫面更新
            $("#Query").click();


        }

    });


    //解鎖按鈕
    $("#UnLockButton").click(function () {

        //alert("LockedButton");
        if (confirm("確定要解鎖？")) {

            var ok = UpdateUnLock_ajax($("#txProgramID").val(), $("#txEpisode").val());
            if (ok) {
                //alert("ajax ok");
                $("#sLockedDate").text('');
                LockedShow();
                //alert("解鎖成功！");
            }


            //查詢畫面更新
            $("#Query").click();

        }

    });

    //匯出按鈕
    $("#ExportButton").click(function () {

        //alert("ExportButton");
        //確認
        var msg = "<p>如果要下載TXT格式檔名(節目+集數.txt)請選擇'TXT'，<br />";
        msg += "如果要下載TXT格式檔名(節目+_+集數+HDN+.TC)請選擇'TC'，<br />";
        msg += "如果要下載SRT格式請選擇'SRT'：系統會檢查CUE表內，<br />";
        msg += "有HoursNo則檔名(hoursNo.srt)；無則檔名(節目+集數.srt)，<br />";
        msg += "如果要離開請選擇'取消'。</p>";
        $("#export-dialog-confirm").html(msg);
        //$("#export-dialog-confirm").dialog("open");

        //匯出訊息視窗
        $("#export-dialog-confirm").dialog({
            autoOpen: true,
            title: "下載字幕提示",
            //設定dialog box開啟效果，預設無(null)。
            show: { effect: "blind", duration: 0 },
            //設定dialog box縮放功能(滑鼠按住視窗右下角拖曳)，預設關閉(false)。
            resizable: false,
            //height: 140,
            width: 400,
            modal: true,
            buttons: {
                "TXT": function () {
                    $(this).dialog("close");
                    //alert("TXT");
                    location.href = "../Subtitle/Download.aspx?ProgramID=" + $("#txProgramID").val() + "&Episode=" + $("#txEpisode").val() + "&SubtitleType=" + "TXT";
                },
                "TC": function () {
                    $(this).dialog("close");
                    //alert("TC");
                    location.href = "../Subtitle/Download.aspx?ProgramID=" + $("#txProgramID").val() + "&Episode=" + $("#txEpisode").val() + "&SubtitleType=" + "TC";
                },
                "SRT": function () {
                    $(this).dialog("close");
                    //alert("SRT");
                    if (hasSRT) {
                        location.href = "../Subtitle/Download.aspx?ProgramID=" + $("#txProgramID").val() + "&Episode=" + $("#txEpisode").val() + "&SubtitleType=" + "SRT";
                    }
                    else {
                        alert("該字幕目前沒有SRT檔案，請確定是否上傳TXT檔\n或是請檢查上傳的TXT檔案有時間格式。");
                    }
                },
                "取消": function () {
                    $(this).dialog("close");
                }
            }
        });

        //alert("ExportButton return");

    });

    //儲存按鈕
    $("#UpdateButton").click(function () {

        //alert("UpdateButton 儲存");
        //return;
        var checkNumber = /^\d+$/;

        //檢查欄位值

        //字幕人員

        if (checkTXT && ($("#txEditorName").val() == "" || $("#txEditorID").val() == "")) {
            alert('字幕人員不可空白！');
            $("#txEditorID").focus();
            return false;
        }

        //製作類別
        /*
        if (!(CheckClassification_ajax($("#txClassificationCode").val(), $("#txProgramLength").val())))
        {
            alert('查無標準製作費用！');
            return false;
        }
        */
        if (!(CheckClassification_ajax($("#txClassificationCode").val()))) {
            alert('查無標準製作費用！');
            return false;
        }

        //單集字幕成本
        if ($("#txSubtitleCostPerEpisode").val()=="") {
            alert('單集字幕成本不可空白！');
            $("#txSubtitleCostPerEpisode").focus();
            return false;
        } else if (!checkNumber.test($("#txSubtitleCostPerEpisode").val())) {
            alert('單集字幕成本不可為非數字！');
            $("#txSubtitleCostPerEpisode").focus();
            return false;
        }

        //單集字幕評比
        if ($("#txSubtitleComparePerEpisode").val() == "") {
            $("#txSubtitleComparePerEpisode").val(0);
        } else if (!checkNumber.test($("#txSubtitleComparePerEpisode").val())) {
            alert('單集字幕評比不可為非數字！');
            $("#txSubtitleComparePerEpisode").focus();
            return false;
        }

        //完成日期(匯入日期)
        if (hasTXT && !dateValidationCheck($("#txImportDate").val())) {
            alert('完成日期(匯入日期)不可是無效日期。\n請輸入 YYYY/MM/DD 日期格式！');
            $("#txImportDate").focus();
            return false;
        }

        //如有上傳字幕
        if (doUploading) {

            //字幕內容判斷
            if (!checkTXT && !checkSRT) {
                //轉換TXT字幕不成功
                alert("您所上傳的檔案TXT內容有問題，沒有轉為txt檔！");
                return false;
            }
            else if (checkTXT && !checkSRT) {
                //轉換SRT字幕不成功
                alert("您所上傳的檔案TXT時間有問題，沒有轉為srt檔。");
            }

            //確認字幕是否覆蓋
            if (hasSRT || hasTXT) {
                if (!confirm("確定要覆蓋上次匯入的字幕檔？")) {
                    return false;
                }
            }

        }

        //儲存字幕資料
        var UpdateSubtitle_ok = UpdateSubtitle_ajax();

        //如有上傳字幕，儲存字幕檔內容
        if (doUploading) {

            //顯示其他按鈕
            $("#ViewSubtitleButton").show();
            $("#LockedButton").show();
            $("#ExportButton").show();
            $("#DeleteButton").hide();

            doUploading = false;

            //儲存匯入字幕檔
            //var uf = 
            UploadFile_ajax();

            if (checkTXT || checkSRT) {
                //不是第一次上傳字幕檔(不是第一次上傳字幕檔，又轉拋字幕 => 發送Email 通知主控人員有轉拋字幕)
                //字幕處理作業完發送email
                //不是第一次上傳字幕，且有HouseNo的話發送Email通知
                //var usc = 
                UploadSubtitleContent_ajax();
            }

        }

        //儲存字幕資料訊息
        if (UpdateSubtitle_ok) {
            //儲存按鈕失效
            $("#UpdateButton").addClass("ui-state-disabled");
            $("#UpdateButton").attr('disabled', 'disabled');
            alert("儲存成功！");

            //查詢畫面更新
            $("#Query").click();
        }
        else {
            alert("儲存失敗！");
        }
        return false;
    });


    //取消按鈕
    $("#CancelButton").click(function () {

        $("#ClassificationForm").hide();

        //alert("CancelButton");
        if ($("#UpdateButton").attr("disabled") != "disabled") {

            if (!confirm("確定要放棄編輯的資料？"))
            {
                checkTXT = false;
                checkSRT = false;
                return false;
            }

        }
  
        $("#ShadowMark").hide();
        $("#EditForm").hide();
        $("#ttSubtitleDescription").hide();
        //$("#ttSubtitleDescription").text("");
        $("#ttSubtitleDescription").val("");

        //儲存按鈕失效
        $("#UpdateButton").addClass("ui-state-disabled");
        $("#UpdateButton").attr('disabled', 'disabled');
      
    });

    //查詢字幕人員按鈕
    $("#btQueryEditor").click(function () {

        //alert("QueryEditor");

        $("#ClassificationForm").hide();

        var position = $("#EditForm").offset();
        var x = position.left;
        var y = position.top;
        var position2 = document.getElementById("EditorForm")
        position2.style.left = x + "px";
        position2.style.top = y + "px";
        $("#EditorFormMark").show();
        $("#EditorForm").show();


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

    /*
    $("#txClassification").click(function () {
        $("#btnclassClassification").click();
    });
    */

    //檢視字幕內容按鈕
    $("#ViewSubtitleButton").click(function () {

        $("#ClassificationForm").hide();

        //alert("ViewSubtitleButton");
        ViewSubtitle_ajax($("#txProgramID").val(), $("#txEpisode").val());

    });


    //字幕人員查詢 取消按鈕
    $("#EditorCancel").click(function () {

        //alert("CancelButton");
        $("#EditorFormMark").hide();
        $("#EditorForm").hide();

    });


    //字幕檔案上傳按鈕 顯示視窗
    $("#SubtitleFileButton").click(function () {

        $("#ClassificationForm").hide();

        //alert("SubtitleFileButton");
        //字幕檔案資料編輯視窗
        $("#UpLoadFormMark").show();
        $("#UpLoadForm").show();


    });

    //字幕檔案上傳視窗 取消按鈕
    $("#uploadcancel").click(function () {

        //alert("CancelButton");
        //字幕檔案資料編輯視窗
        $("#UpLoadFormMark").hide();
        $("#UpLoadForm").hide();
        $("#upFile").val("");

    });

    //字幕檔案上傳視窗 上傳按鈕
    $("#uploadfile").click(function () {

        //檢查是否有檔案
        if ($('#upFile').val() == "") {
            alert("未選擇檔案！");
            return;
        }

        //檢查副檔名
        var ext = $('#upFile').val().split('.').pop().toLowerCase();
        if ($.inArray(ext, ['txt']) == -1) {
            alert('檔案只支援txt副檔名！');
            return;
        }

        //檢查檔案格式
        //錯誤:上傳的檔案格式不為UTF-16 LE相容格式
        //var filename = document.getElementById("upFile").value;
        //var mystr = checkTXT_ajax(filename);

        //return;
        var upfile = document.getElementById("upFile").files[0];
        var fr = new FileReader();

        fr.onload = function () {

            //隱藏上傳字幕檔案視窗
            $("#UpLoadFormMark").hide();
            $("#UpLoadForm").hide();
            $("#upFile").val("");

            //上傳註記
            doUploading = true;

            //檢查機制

            //檢查TC的時間格式或間距不對 (與原系統增加上傳不能轉SRT提示訊息)
            var strSubtitle = fr.result;
            if (strSubtitle == "") {
                checkTXT = false;
                checkSRT = false;
                alert("上傳的檔案內容空白！");
                return false;
            }
            //var arryTC = strSubtitle.substring(0, 12).split(':');
            //if (arryTC.length >= 4) {
                var SRTStatus = checkSRT_ajax(strSubtitle);
                if (SRTStatus == "ok") {
                    checkTXT = true;
                    checkSRT = true;
                }
                else if (SRTStatus.substring(0, 6) == "update") {
                    checkTXT = true;
                    checkSRT = true;
                    strSubtitle = SRTStatus.substring(6);
                }
                else if (SRTStatus == "") {
                    //不是TC格式
                    checkTXT = true;
                    checkSRT = false;
                    //return false;
                }
                else if (SRTStatus.substring(0, 5) == "ERROR") {
                    //執行ajax時因檔案內容亂碼而錯誤跳出，所以以此條件判斷非UTF檔案格式
                    //alert("上傳的檔案格式不為UTF-16 LE相容格式！");
                    doUploading = false;
                    checkTXT = false;
                    checkSRT = false;
                    alert("上傳的檔案格式不為UTF相容格式！");
                    return false;
                }
                else {
                    //TC時間或間距不對
                    checkTXT = false;
                    checkSRT = false;
                    //與原系統增加上傳不能轉SRT提示訊息
                    alert("TC的時間格式或間距不對，如下：\n" + SRTStatus.substring(5));
                    return false;
                }
            //}

            //顯示字幕內容
            $("#ttSubtitleDescription").show();
            $("#ttSubtitleDescription").val(strSubtitle);
            if (checkSRT || checkTXT) {
                //可使用儲存按鈕
                $("#UpdateButton").removeAttr('disabled');
                $("#UpdateButton").removeClass("ui-state-disabled");
                //完成日期(匯入日期)
                $("#txImportDate").val(yyyymmdd);
            }
        };

        fr.readAsText(upfile);

    });

    //字幕人員查詢按鈕 查詢
    $("#EditorQuery").click(function () {

        var GetEditor_ok = GetEditor_ajax();
        if (GetEditor_ok) {
            //alert("GetEditor_ajax ok");
        }
        else {
            alert("GetEditor_ajax err");
        }


    });


    //直接在字幕人員代號欄位按Enter鍵 txEditorID
    $("#txEditorID").bind("keypress", function (e) {

        if (e.which == 13) {
            //alert(e.which);
            $("#txEditorID").blur();
            //$("#txEditorID").change();
        }

        /*
        if (e.which == 27) {
            $("#CancelButton").click();
        }
        */

    });

    //字幕人員代號欄位變更
    $("#txEditorID").change(function () {

        //alert("字幕人員");
        if ($("#txEditorID").val() == "") {
            alert('字幕人員不可空白！');
            //document.getElementById("txEditorID").focus();
            $("#txEditorID").focus();
            return false;
        }

        //檢查字幕人員
        var strEditorName = checkEditor_ajax($("#txEditorID").val());
        if (strEditorName != "") {

            $("#UpdateButton").removeAttr('disabled');
            $("#UpdateButton").removeClass("ui-state-disabled");

            //儲存按鈕
            $("#UpdateButton").removeAttr('disabled');
            $("#UpdateButton").removeClass("ui-state-disabled");
            //字幕人員
            $("#txEditorName").val(strEditorName);

        }
        else {
            //字幕人員
            alert("查無資料！");
            $("#txEditorName").val("");
            $("#txEditorID").focus();
            //document.getElementById("txEditorID").focus();
            return false;
        }

    });

    //單集字幕成本欄位變更
    $("#txSubtitleCostPerEpisode").change(function () {

        //alert("單集字幕成本");
        $("#UpdateButton").removeAttr('disabled');
        $("#UpdateButton").removeClass("ui-state-disabled");

    });

    //單集字幕評比欄位變更
    $("#txSubtitleComparePerEpisode").change(function () {

        //alert("單集字幕評比");
        $("#UpdateButton").removeAttr('disabled');
        $("#UpdateButton").removeClass("ui-state-disabled");

    });

    //說明欄位變更 ttDescription
    $("#ttDescription").change(function () {

        //alert("說明");
        $("#UpdateButton").removeAttr('disabled');
        $("#UpdateButton").removeClass("ui-state-disabled");

    });

    //完成日期 (匯入日期)欄位變更 txImportDate
    $("#txImportDate").change(function () {

        //alert("匯入日期");
        $("#UpdateButton").removeAttr('disabled');
        $("#UpdateButton").removeClass("ui-state-disabled");

    });

    //製作類別代碼欄位變更 txClassificationCode
    $("#txClassificationCode").change(function () {

        //alert("匯入日期");
        $("#UpdateButton").removeAttr('disabled');
        $("#UpdateButton").removeClass("ui-state-disabled");

    });

});


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

//取得字幕人員
function checkEditor_ajax(EditorID) {

    var strdata = "{'EditorID':'" + EditorID + "'}";
    var strEditorName = "";

    $.ajax({
        type: "POST",
        url: "../SubtitleWebService.asmx/GetEditorName",
        data: strdata,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (json) {

            var mydata = $.parseJSON(json.d);
            if (mydata.length > 0) {
                strEditorName = mydata[0].EditorName;
            }
            else {
                //字幕人員
                //alert("查無資料！");
                //document.getElementById("txEditorID").focus();
                //return false;
            }

        },

        failure: function (ex) {
            alert(ex.get_message);
            //return false;
        }
    });
    return strEditorName;
}

//檢查SRT格式及過濾掉不正常檔頭
function checkSRT_ajax(textContent) {

    //var strData = "{'textContent':'" + $("#ttSubtitleDescription").val() + "'}";
    var strData = "{'textContent':'" + textContent + "'}";
    var ok = "err"; //false;
    $.ajax({
        type: "post",
        url: "../SubtitleWebService.asmx/CheckSubtitle",
        data: strData,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (data) {
            /*
            if (data.d == "ok") {
                ok = true;
            }
            else {
                //alert("upload file err!");
            }
            */
            ok = data.d;
        },
        failure: function (ex) {
            alert(ex.get_message);
        }
    });

    return ok;

}

//儲存字幕內容
function UploadSubtitleContent_ajax() {

    var strData = "{'ProgramID':'" + $("#txProgramID").val() + "','Episode':" + $("#txEpisode").val() + ",'textContent':'" +
        $("#ttSubtitleDescription").val() + "','ProgramName':'" + $("#txProgramAbbrev").val() + "','Requester':'" +
        $("#Requester").val() + "','sendSrtUpdateEmail':" + sendSrtUpdateEmail + ",'UserID':'" + $("#UserID").val() + "'}";
    var ok = false;
    $.ajax({
        type: "post",
        url: "../SubtitleWebService.asmx/UploadSubtitleContent",
        data: strData,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: true,
        success: function (data) {
            if (data.d == "ok") {
                ok = true;
            }
            else {
                //alert("字幕儲存錯誤!");
                SendErrorEmail("字幕匯入確認系統", "字幕儲存錯誤",
                    "錯誤敘述：SRT字幕轉換錯誤或儲存資料庫錯誤<br/>程式：SubtitleHandle.aspx<br/>錯誤點：UploadSubtitleContent_ajax()<br/>節目代號：" +
                    $("#txProgramID").val() + "<br/>集數：" + $("#txEpisode").val() + "<br/>錯誤原因：" + data.d);
            }
        },
        failure: function (ex) {
            //alert(ex.get_message);
            SendErrorEmail("字幕匯入確認系統", "字幕儲存錯誤",
                "錯誤敘述：SRT字幕轉換錯誤或儲存資料庫錯誤<br/>程式：SubtitleHandle.aspx<br/>錯誤點：UploadSubtitleContent_ajax()<br/>節目代號：" +
                $("#txProgramID").val() + "<br/>集數：" + $("#txEpisode").val() + "<br/>錯誤原因：" + ex.get_message);
        }
    });
    return ok;
}

//儲存匯入字幕檔
function UploadFile_ajax() {

    var filename = $("#txProgramID").val() + "_" + padLeft("0" + $("#txEpisode").val(), 4);
    var strData = "{'fileName':'" + filename + "','fileContent':'" + $("#ttSubtitleDescription").val() + "'}";
    //var strData = "{'fileName':'" + filename + "','fileContent':'" + $("#ttSubtitleDescription").text() + "'}";
    var ok = false;
    $.ajax({
        type: "post",
        url: "../SubtitleWebService.asmx/UploadFile",
        data: strData,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (data) {
            ok = true;
        },
        failure: function (ex) {
            alert(ex.get_message);
        }
    });

    return ok;

}

//取得節目代碼給節目代碼輸入欄位的AutoComplete
function GetProgramID_ajax() {

    var ok = false;
    $.ajax({
        type: "POST",
        url: "../SubtitleWebService.asmx/GetProgramID",
        data: {},
        contentType: "application/json; charset=utf-8",
        dataType: "json", //"json",
        async: false,
        success: function (response) {
            var availableTags = response.d;
            $("#tbProgramID").autocomplete({ source: availableTags });
            ok = true;
        },

        failure: function (ex) {
            alert(ex.get_message);
        }
    });
    return ok;
}

//搜尋取得節目及字幕相關資料
function GetSubtitle_ajax(strProgramID, intEpisode1, intEpisode2, intDataScope) {

    //var arForm = $("#form1").serializeArray();
    var strdata = "{'ProgramID':'" + strProgramID + "', 'Episode1':" + intEpisode1 + ", 'Episode2':" + intEpisode2
        + ", 'GetDataDay':" + intDataScope + "}";

    $.ajax({
        type: "POST",
        url: "../SubtitleWebService.asmx/GetSubtitle",
        data: strdata,
        contentType: "application/json; charset=utf-8",
        //contentType:"text/xml; charset=utf-8",
        dataType: "json",
        async: true,

        success: function (json) {
            var mytable = $.parseJSON(json.d);
            //依照原片庫程式，在查無資料時把原資料清除
            InsertNewRow(mytable, "#tbLv");
            if (mytable.length > 0) {
                //InsertNewRow(mytable, "#tbLv");
            }
            else {
                alert("查無資料！");
            }

        },
        beforeSend: function (e) {
            $("#Query").css("cursor", "progress");
        },
        complete: function (e) {
            $("#Query").css("cursor", "pointer");
        },
        failure: function (ex) {
            alert(ex.get_message);
        }
    });

    //return;

}

//產出Table多筆欄位格式
function InsertNewRow(data, tableID) {

    var tr = "";
    for (var i = 0; i < data.length; i++) {

        // tr begin
        tr += '<tr id="row_' + i + '" style="cursor: pointer;" class="bodyRow" ondblclick="openEdit(this);">';
        /*
        if (i % 2 == 0) {
            tr += '<tr id="row_' + i + '" style="cursor: pointer;" class="bodyRow" onclick="do_click(this,\'tbLv_tbody_tr_selected\');" ondblclick="do_dblclick(this,\'openEdit\');">';
        }
        else {
            tr += '<tr id="row_' + i + '" style="cursor: pointer;" class="bodyRow z-table-tr-even" onclick="do_click(this,\'tbLv_tbody_tr_selected\');" ondblclick="do_dblclick(this,\'openEdit\');">';
        }
        */
        // td begin
        tr += '<td><img alt="" src="../img/' + data[i].Image + '" title="' + data[i].Tooltiptext + '" /></td>';
        tr += '<td><span id="ProgramID_' + i + '">' + data[i].ProgramID + '</span></td>';
        tr += '<td><span id="ProgramAbbrev_' + i + '">' + data[i].ProgramAbbrev + '</span></td>';
        tr += '<td><span id="Episode_' + i + '">' + data[i].Episode + '</span></td>';
        tr += '<td><span id="ProgramLength_' + i + '">' + data[i].ProgramLength + '</span></td>';
        tr += '<td><span id="EditorID_' + i + '">' + data[i].EditorID + '</span></td>';
        tr += '<td><span id="EditorName_' + i + '">' + data[i].EditorName + '</span></td>';
        tr += '<td><span id="Classification_' + i + '">' + data[i].Classification + '</span></td>';
        tr += '<td><span id="SubtitleCostPerEpisode_' + i + '">' + data[i].SubtitleCostPerEpisode + '</span></td>';
        tr += '<td><span id="SubtitleComparePerEpisode_' + i + '">' + data[i].SubtitleComparePerEpisode + '</span></td>';
        tr += '<td><span id="RequestDate_' + i + '">' + data[i].RequestDate + '</span></td>';
        tr += '<td><span id="ImportDate_' + i + '">' + GetImportDate(data[i].ImportDate) + '</span></td>';
        tr += '<td><span id="LockedDate_' + i + '">' + GetLockedDate(data[i].LockedDate) + '</span></td>';
        tr += '<td><span id="BillDate_' + i + '">' + GetBillDate(data[i].BillDate) + '</span></td>';
        // td end
        tr += '</tr>';
        // tr end
    }
    //alert(tr);
    $(tableID).children("tbody").html(tr);
    return;
}

//點選節目取得相關字幕資料及開啟編輯視窗
function openEdit(e) {

    var secondCell = $(e).find('td:eq(1)').text();
    var fourthCell = $(e).find('td:eq(3)').text();
    //alert(secondCell + ',' + fourthCell);
    var strdata = "{'ProgramID':'" + secondCell + "','Episode1':" + fourthCell + ",'Episode2':" + fourthCell + ",'GetDataDay':0}";

    $.ajax({
        type: "POST",
        url: "../SubtitleWebService.asmx/GetSubtitle",
        data: strdata,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (json) {
            var mytdata = $.parseJSON(json.d);
            if (mytdata.length > 0) {
                //開啟編輯視窗
                EditForm(mytdata);
                $("#ShadowMark").show();
                $("#EditForm").show();
                $("#txEditorID").focus();
            }
            else {
                alert("無此資料！");
            }

        },
        failure: function (ex) {
            alert(ex.get_message);
        }

    });

}

function GetImportDate(ImportDate) {

    if (ImportDate == null) { return ""; }
    return ImportDate;

}

function GetLockedDate(LockedDate) {

    if (LockedDate == null) { return ""; }
    return LockedDate;

}

function GetBillDate(intBillDate) {

    //var intBillDate = Convert.ToInt32(BillDate);
    if (intBillDate == -1) { return ""; }

    var month = (0 == intBillDate % 12) ? 12 : (intBillDate % 12);
    var year = ((intBillDate - month) / 12);

    return year + "-" + month;

}

//開啟節目的字幕相關編輯視窗
function EditForm(data) {

    //字幕編輯UI
    //節目代號
    $("#txProgramID").val(data[0].ProgramID);
    //節目名稱
    $("#txProgramAbbrev").val(data[0].ProgramAbbrev);
    //集數
    $("#txEpisode").val(data[0].Episode);
    //節目長度
    $("#txProgramLength").val(data[0].ProgramLength);

    //字幕人員
    $("#txEditorID").val(data[0].EditorID);
    $("#txEditorName").val(data[0].EditorName);

    //製作類別
    $("#txClassification").val(data[0].Classification);
    $("#txClassificationCode").val(data[0].ClassificationCode);

    //單集字幕成本
    $("#txSubtitleCostPerEpisode").val(data[0].SubtitleCostPerEpisode);
    //單集字幕評比
    $("#txSubtitleComparePerEpisode").val(data[0].SubtitleComparePerEpisode);
    //說明
    $("#ttDescription").val(data[0].Description);

    //申請日期
    $("#sRequestDate").text(data[0].RequestDate);
    //完成日期 (匯入日期)
    $("#txImportDate").val(data[0].ImportDate);
    //鎖定日期
    $("#sLockedDate").text(data[0].LockedDate);

    //申請人同工
    $("#Requester").val(data[0].Requester);
    //var ss = $("#Requester").val();

    //儲存狀態
    hasTXT = data[0].TXT == "1" ? true : false;
    hasSRT = data[0].SRT == "1" ? true : false;
    sendSrtUpdateEmail = data[0].SubtitleFilename.search(".srt") > -1 ? true : false;

    Status = data[0].Tooltiptext;

    if (Status == "已鎖定") {
        LockedHide();
    }
    else {
        LockedShow();
    }

}

//取得字幕內容及顯示字幕內容欄位
function ViewSubtitle_ajax(strProgramID, intEpisode) {

    var strdata = "{'ProgramID':'" + strProgramID + "', 'Episode':" + intEpisode + "}";
    //var ok = false;

    $.ajax({
        type: "POST",
        url: "../SubtitleWebService.asmx/GetSubtitle2",
        data: strdata,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (json) {
            //var mydata = $.parseJSON(json.d);
            var mydata = json.d;

            if (mydata.length > 0) {
                $("#ttSubtitleDescription").show();
                $("#ttSubtitleDescription").val(mydata);
                //$("#ttSubtitleDescription").text(mydata[0].Subtitle);
                //$("#ttSubtitleDescription").val(mydata[0].Subtitle);
            }
            else {
                alert("無此字幕！");
            }

            //ok = true;
        },

        failure: function (ex) {
            alert(ex.get_message);
        }
    });

    //return ok;
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

//檢查製作類別
function CheckClassification_ajax(Classification) {

    var ok = false;
    var strData = "{ 'Classification':'" + Classification + "'}";

    $.ajax({
        type: "POST",
        url: "../SubtitleWebService.asmx/CheckClassification",
        data: strData,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (json) {
            var mydata = $.parseJSON(json.d);
            if (mydata.length > 0) {
                ok = true;
            }
            else {
            }

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
    //var Cell1 = $(e).find('td:eq(1)').text();
    var Cell2 = $(e).find('td:eq(2)').text();
    var Cell3 = $(e).find('td:eq(3)').text();
    //alert(Cell1 + ',' + Cell2 + ',' + Cell3 + ',' + Cell4);
    //製作類別
    $("#txClassification").val(Cell0);
    $("#txSubtitleCostPerEpisode").val(Cell2);
    $("#txClassificationCode").val(Cell3);  //.trigger('change');
    //製作類別視窗隱藏
    $("#ClassificationForm").hide();

    return;
}

//儲存字幕_ST03P0
function UpdateSubtitle_ajax() {

    //暫存欄位
    //節目代號
    var ProgramID = $("#txProgramID").val();
    //集數
    var Episode = $("#txEpisode").val();
    //字幕人員代號
    var EditorID = $("#txEditorID").val();
    //製作類別
    var Classification = $("#txClassificationCode").val();
    //單集字幕成本
    var SubtitleCostPerEpisode = $("#txSubtitleCostPerEpisode").val();
    //單集字幕評比
    var SubtitleComparePerEpisode = $("#txSubtitleComparePerEpisode").val();
    //說明
    var Description = $("#ttDescription").val();
    //完成日期 (匯入日期)
    var ImportDate = $("#txImportDate").val();

    //儲存傳送欄位值
    var strdata = "{'ProgramID':'" + ProgramID + "','Episode':" + Episode + ",'EditorID':'" + EditorID + "','Classification':'" + 
        Classification + "','SubtitleCostPerEpisode':" + SubtitleCostPerEpisode +",'SubtitleComparePerEpisode':" + 
        SubtitleComparePerEpisode + ",'Description':'" + Description + "','ImportDate':'" + ImportDate + "','checkSRT':" + 
        checkSRT + ",'UserID':'" + $("#UserID").val() + "'}";
    //alert(strdata);

    var ok = false;
    $.ajax({
        type: "post",
        url: "../SubtitleWebService.asmx/UpdateSubtitle",
        data: strdata,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (data) {

            if (data.d > 0) {
                //alert("儲存成功！");
                ok = true;
            }
            else {
                //alert("儲存不成功！");
            }

        },
        failure: function (ex) {
            alert(ex.get_message);
        }
    });

    return ok;

}

//取得字幕人員
function GetEditor_ajax() {

    var strdata = "{'queryInput':'" + $("#EditorQueryInput").val() + "'}";

    var ok = false;

    $.ajax({
        type: "POST",
        url: "../SubtitleWebService.asmx/GetEditor",
        data: strdata,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (json) {
            var mydata = $.parseJSON(json.d);
            //產出字幕人員查詢視窗
            InsertEditorRow(mydata);
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

//產出字幕人員查詢視窗
function InsertEditorRow(data) {

    var tr = "";
    for (var i = 0; i < data.length; i++) {
        // tr begin
        tr += '<tr style="cursor: pointer;" class="bodyRow" ondblclick="EditorEdit(this)" title="行動電話：' + data[i].MobilePhone + '">';
        // td begin
        tr += '<td><span>' + data[i].EditorID + '</span></td>';
        tr += '<td><span>' + data[i].EditorName + '</span></td>';
        tr += '<td><span>' + data[i].EditorDept + '</span></td>';
        tr += '<td><span>' + data[i].LandPhone + '</span></td>';
        tr += '<td><span>' + data[i].Comparison + '</span></td>';
        // td end
        tr += '</tr>';
        // tr end
    }
    //alert(tr);

    $("#EditorTable").children("tbody").html(tr);
    $("#EditorForm").show();

    return;
}

//點選字幕人員查詢視窗的清單及帶回父視窗
function EditorEdit(e) {

    //alert("EditorEdit");
    var Cell0 = $(e).find('td:eq(0)').text();
    var Cell1 = $(e).find('td:eq(1)').text();

    if ($("#txEditorID").val() != Cell0) {

        //儲存按鈕
        $("#UpdateButton").removeAttr('disabled');
        $("#UpdateButton").removeClass("ui-state-disabled");

        //字幕人員
        $("#txEditorID").val(Cell0);
        $("#txEditorName").val(Cell1);

    }

    $("#EditorFormMark").hide();
    $("#EditorForm").hide();

    return;
}

//刪除紀錄
function SubtitleDelete_ajax(strProgramID, intEpisode) {

    var strdata = "{'ProgramID':'" + strProgramID + "', 'Episode':" + intEpisode + "}";
    var ok = false;

    $.ajax({
        type: "POST",
        url: "../SubtitleWebService.asmx/SubtitleDelete",
        data: strdata,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (data) {

            if (data.d > 0) {
                ok = true;
            }
            else {
                alert("無法刪除！");
            }

        },

        failure: function (ex) {
            alert(ex.get_message);
        }
    });

    return ok;
}

//更新鎖定紀錄
function UpdateLocked_ajax(strProgramID, intEpisode) {

    var strdata = "{'ProgramID':'" + strProgramID + "', 'Episode':" + intEpisode + ",'UserID':'" + $("#UserID").val() + "'}";
    var ok = false;

    $.ajax({
        type: "POST",
        url: "../SubtitleWebService.asmx/UpdateLocked",
        data: strdata,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (data) {

            if (data.d > 0) {
                ok = true;
            }
            else {
                alert("無法鎖定！");
            }

        },

        failure: function (ex) {
            alert(ex.get_message);
        }
    });

    return ok;
}

//更新解鎖紀錄
function UpdateUnLock_ajax(strProgramID, intEpisode) {

    var strdata = "{'ProgramID':'" + strProgramID + "', 'Episode':" + intEpisode + ",'UserID':'" + $("#UserID").val() + "'}";
    var ok = false;

    $.ajax({
        type: "POST",
        url: "../SubtitleWebService.asmx/UpdateUnLock",
        data: strdata,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (data) {

            if (data.d > 0) {
                ok = true;
            }
            else {
                alert("無法解鎖！");
            }

        },

        failure: function (ex) {
            alert(ex.get_message);
        }
    });

    return ok;
}

//鎖定狀態
function LockedHide() {

    $("#btQueryEditor").addClass("ui-state-disabled");
    $("#btQueryEditor").attr('disabled', 'disabled');

    $("#txEditorID").attr('disabled', 'disabled');
    //$("#txEditorID").css("background-color", "#ECEAE4");
    $("#txEditorID").css("background", "#ECEAE4");
    $("#txEditorID").css("color", "black");

    $("#txClassification").attr('disabled', 'disabled');
    $("#txClassification").css("background", "#ECEAE4");
    $("#txClassification").css("color", "black");

    $("#btnclassClassification").attr('disabled', 'disabled');
    $("#btnclassClassification").removeClass("ui-button");

    $("#txSubtitleCostPerEpisode").attr('disabled', 'disabled');
    //$("#txSubtitleCostPerEpisode").css("background-color", "#ECEAE4");
    $("#txSubtitleCostPerEpisode").css("background", "#ECEAE4");
    $("#txSubtitleCostPerEpisode").css("color", "black");

    $("#txSubtitleComparePerEpisode").attr('disabled', 'disabled');
    //$("#txSubtitleComparePerEpisode").css("background-color", "#ECEAE4");
    $("#txSubtitleComparePerEpisode").css("background", "#ECEAE4");
    $("#txSubtitleComparePerEpisode").css("color", "black");

    $("#ttDescription").attr('disabled', 'disabled');
    //$("#ttDescription").css("background-color", "#ECEAE4");
    $("#ttDescription").css("background", "#ECEAE4");
    $("#ttDescription").css("color", "black");

    $("#txImportDate").attr('disabled', 'disabled');
    //$("#txImportDate").css("background-color", "#ECEAE4");
    $("#txImportDate").css("background", "#ECEAE4");
    $("#txImportDate").css("color", "black");

    $("#ViewSubtitleButton").show();
    $("#SubtitleFileButton").hide();

    $("#UnLockButton").show();
    $("#LockedButton").hide();
    //$("#UpdateButton").show();
    $("#ExportButton").show();

}

//未鎖定狀態
function LockedShow() {

    $("#btQueryEditor").removeAttr('disabled');
    $("#btQueryEditor").removeClass("ui-state-disabled");

    $("#txEditorID").removeAttr('disabled');
    $("#txEditorID").css("background-color", "white");

    $("#txClassification").removeAttr('disabled');
    $("#txClassification").css("background-color", "white");

    $("#btnclassClassification").removeAttr('disabled');
    $("#btnclassClassification").addClass("ui-button");

    $("#txSubtitleCostPerEpisode").removeAttr('disabled');
    $("#txSubtitleCostPerEpisode").css("background-color", "white");

    $("#txSubtitleComparePerEpisode").removeAttr('disabled');
    $("#txSubtitleComparePerEpisode").css("background-color", "white");

    $("#ttDescription").removeAttr('disabled');
    $("#ttDescription").css("background-color", "white");

    if (Status == "申請中") {

        $("#txImportDate").attr('disabled', 'disabled');
        //$("#txImportDate").css("background-color", "#ECEAE4");
        $("#txImportDate").css("background", "#ECEAE4");
        $("#txImportDate").css("color", "black");

        $("#ViewSubtitleButton").hide();
        $("#SubtitleFileButton").show();

        $("#DeleteButton").show();
        $("#UnLockButton").hide();
        $("#LockedButton").hide();
        //$("#UpdateButton").hide();
        $("#ExportButton").hide();
    }
    else {

        if ($("#UserID").val() == "admin") {
            $("#txImportDate").removeAttr('disabled');
            $("#txImportDate").css("background-color", "white");
        }
        else {
            $("#txImportDate").attr('disabled', 'disabled');
            //$("#txImportDate").css("background-color", "#ECEAE4");
            $("#txImportDate").css("background", "#ECEAE4");
            $("#txImportDate").css("color", "black");
        }

        $("#ViewSubtitleButton").show();
        $("#SubtitleFileButton").show();

        $("#DeleteButton").hide();
        $("#UnLockButton").hide();
        $("#LockedButton").show();
        //$("#UpdateButton").hide();
        $("#ExportButton").show();
    }

}

//發送內部錯誤Email(非同步傳送)
function SendErrorEmail(EmailFromName, Subject, Body) {

    var strdata = "{'EmailFromName':'" + EmailFromName + "', 'Subject':'" + Subject
    + "', 'Body':'" + Body + "'}";

    $.ajax({
        type: "POST",
        url: "../SubtitleWebService.asmx/SendErrorEmail",
        data: strdata,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: true,
        success: function (json) {
        }
    });

}

