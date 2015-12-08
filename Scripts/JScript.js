function chkData(objClientID) {
    testReg = /[^0-9]/;
    var Data = document.getElementById(objClientID).value;

    if (Data.match(testReg)) {
        alert('請輸入數字');
        document.getElementById(objClientID).value = '';
        return false;
    }
}