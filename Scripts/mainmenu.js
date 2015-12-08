"use strict";

$(document).ready(function () {

    //$("#menu").css('width', $(window).width());
    $("#content").css('width', $(window).width());
    $("#content").css('height', $(window).height() - ($("#nav-table").height() + 2));

    $(window).resize(function () {
        //$("#menu").css('width', $(window).width());
        $("#content").css('width', $(window).width());
        $("#content").css('height', $(window).height() - ($("#nav-table").height() + 2));
    });

    //直接做在Server端
    //$("ul.menu > li:has(ul) > a").append('<div class="arrow-bottom"></div>');

    $("#home").click(function (e) {
        $("#nav-title").html("[首頁]");
    });

    $("ul.menu > li > ul > li > a").click(function (e) {

        var strTitle = this.innerText;
        if (strTitle.trim() != "") {
            $("#nav-title").html("[" + strTitle.trim() + "]");
        }

        $(this).parent('li').parent('ul').css('display', 'none');

        //$("ul.menu li ul").css('display', 'none');

    });

    $("ul.menu > li > a").click(function (e) {
        $(this).parent('li').children('ul').css('display', 'block');
    });

    $("ul.menu li ul").mouseover(function (e) {

        $(this).css('display', 'block');

    });

    /*
    $("ul.menu li").mouseover(function (e) {

        $(this).children('ul').css('display', 'block');

    });
    */

    $("ul.menu li").mouseout(function (e) {

        $(this).children('ul').css('display', 'none');

    });

    $("ul.menu li ul").mouseout(function (e) {

        $(this).css('display', 'none');

    });


});
