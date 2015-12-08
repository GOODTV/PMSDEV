    
"use strict";

//從SRTQC C#的JScript.js copy來的
function chkData(objClientID) {
    testReg = /[^0-9]/;
    var Data = document.getElementById(objClientID).value;

    if (Data.match(testReg)) {
        alert('請輸入數字');
        document.getElementById(objClientID).value = '';
        return false;
    }
}

/* 左邊補0 */
function padLeft(str, len) {
    str = '' + str;
    if (str.length >= len) {
        return str;
    } else {
        return padLeft("0" + str, len);
    }
}

//檢查日期的正確
//判斷使用者輸入日期格式是否為 YYYY/MM/DD
function dateValidationCheck(str) {

    var re = new RegExp("^([0-9]{4})[./]{1}([0-9]{2})[./]{1}([0-9]{2})$");
    var strDataValue;
    var infoValidation = true;

    if ((strDataValue = re.exec(str)) != null) {
        var i = Date.parse(str);
        if (isNaN(i)) {
            infoValidation = false;
        }
        else {
            infoValidation = validDate(strDataValue[1], strDataValue[2], strDataValue[3]);
        }
    } else {
        infoValidation = false;
    }
    return infoValidation;
}

//額外檢查日期的正確(針對javascript，遇到2/30會自動轉換成3/2，讓日期不會錯誤)
function validDate(_year, _month, _day) {
    var Day = new Date(_year + '/' + _month + '/' + _day);
    var chk = new Date(Day.valueOf());
    return (chk.getFullYear() == _year && (chk.getMonth() + 1) == _month && chk.getDate() == _day);
}

//檢查日期範圍的正確
function checkDateStartEnd(StartDate, EndDate) {

    var dt1 = StartDate;
    var dt2 = EndDate;

    if (dt1 == "") {
        return false;
    }
    if (dt2 == "") {
        return false;
    }

    if (!dateValidationCheck(dt1)) {
        return false;
    }
    if (!dateValidationCheck(dt2)) {
        return false;
    }

    var dt1 = new Date(dt1);
    var dt2 = new Date(dt2);

    if ((dt2 - dt1) >= 0) {
        return true;
    }
    else {
        return false;
    }
    return false;

}

//檢查開始時間和結束時間的正確與範圍
function chkCalendarTimeStartEnd(str1, strfieldName1, str2, strfieldName2) {

    if (str1 == "" || str2 == "") {
        return false;
    }

    var chkCalendarTime_ok = chkCalendarTime(str1, strfieldName1);
    if (!chkCalendarTime_ok) return false;
    var chkCalendarTime_ok = chkCalendarTime(str2, strfieldName2);
    if (!chkCalendarTime_ok) return false;

    if (str2 > str1) {
        return true;
    }
    else {
        return false;
    }

}

//檢查時間
function chkCalendarTime(str, strfieldName) {

    testReg = /[^0-9]/;
    var Data = str;

    if (Data.match(testReg)) {
        alert(strfieldName + '欄位請輸入數字');
        return false;
    }

    if (Data.length != 4) {
        alert(strfieldName + '欄位必須輸入4位數字的時分(hhmm)');
        return false;
    }

    var ck = parseInt(Data.substr(0, 2), 10);
    if (ck > 24) {
        alert(strfieldName + '欄位的小時數字不能超過24小時');
        return false;
    }

    var ck = parseInt(Data.substr(2, 2), 10);
    if (ck > 60) {
        alert(strfieldName + '欄位的分鐘數字不能超過60分鐘');
        return false;
    }
    return true;
}

//計算有幾個全型字、中文字...  
function countLength(stringToCount) {
    var c = stringToCount.match(/[^ -~]/g);
    return stringToCount.length + (c ? c.length : 0);
}

function getCookie(name) {
    var nameEQ = name + "=";
    //alert(document.cookie);
    var ca = document.cookie.split(';');
    for (var i = 0; i < ca.length; i++) {
        var c = ca[i];
        while (c.charAt(0) == ' ') c = c.substring(1);
        if (c.indexOf(nameEQ) != -1) return c.substring(nameEQ.length, c.length);
    }
    return null;
}

//設定 JQuery Datepicker 預設值
/*==================================================================================================================
 d	 每月的第幾日, 個位數時前面不補 0
dd	 每月的第幾日, 兩位數, 個位數時前面補 0
o	 每年的第幾日, 小於 100 時前面不補 0
oo	 每年的第幾日, 三位數, 小於 100 時前面補 0 為 3 位數
D	 星期的簡稱, 如 Fri
DD	 星期的全名, 如 Friday
m	 每年的第幾月, 個位數時前面不補 0
mm	 每年的第幾月, 兩位數, 個位數時前面補 0
M	 月份簡稱, 例如 Jan
MM	 月份全明, 例如 January
y	 兩位數的年, 個位數時前面補 0, 例如 05
yy	 四位數的年, 例如 2005
 @	 自 1970/1/1 至今之毫秒數
!	 自 0001/1/1 至今之奈秒數 (tick)
''	 跳脫上述之模式字串
==================================================================================================================*/
$.datepicker.regional['zh-TW']={
    dayNames: ["星期日","星期一","星期二","星期三","星期四","星期五","星期六"],
    dayNamesMin: ["日","一","二","三","四","五","六"],
    monthNames: ["一月","二月","三月","四月","五月","六月","七月","八月","九月","十月","十一月","十二月"],
    monthNamesShort:["一月","二月","三月","四月","五月","六月","七月","八月","九月","十月","十一月","十二月"],
    prevText: "上月",
    nextText: "次月",
    showMonthAfterYear: true,
    dateFormat: "yy/mm/dd",
    weekHeader: "週"
};
$.datepicker.setDefaults($.datepicker.regional["zh-TW"]);

