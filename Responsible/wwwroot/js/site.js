// Write your JavaScript code.
$(document).ready(function () {
    if (isVisitor()) {
        //note:必须使用removeClass("hide")，不要使用show()
        //bootstrap的.hide 标注了!important
        $("[zyf-not-logon]").removeClass("hide");
    } else {
        $("[zyf-logon]").removeClass("hide");
        var $currentCredit = $("[zyf-nav-current-credit]").text();
        $("[zyf-current-credit]").text($currentCredit);
        $("[zyf-hide-current-credit]").val($currentCredit);
    }

    $("[zyf-show-contact]").click(function () {
        var $modal = $("[zyf-global-modal]");
        $modal.find(".modal-title").html("联系方式");
        var $userId = $(this).attr("zyf-show-contact");
        $.get("/Contact/_Show?userId=" + $userId, function (data) {
            $modal.find(".modal-body").html(data);
        })
        $("[zyf-global-modal]").modal('show');
    })
})

function isVisitor() {
    return $.cookie().USER_ID == undefined;
}

function isCurrent(userId) {
    if (isVisitor()) {
        return false;
    }
    return $.cookie().USER_ID.split("&")[0] == userId;
}

function JqueryAjaxError(jqXHR, textStatus, errorThrown) {
    //TODO: 此处应收集一些用户本地信息，如时间/浏览器等……
    showModal("错误信息",
        "<span class='glyphicon glyphicon-warning-sign text-warning'></span> JQuery Ajax请求发生错误，请稍后再试或联系管理员，以下为错误信息：\n" +
        jqXHR.responseText + "\n" +
        "status:" + textStatus + "\n" +
        "error:" + errorThrown);
}

function showRemind(content) {
    showModal("<span class='glyphicon glyphicon-info-sign descend-2px'></span> 友情提示", content);
}

function showModal(title, body) {
    $modal = $("[zyf-global-modal]");
    $modal.find(".modal-title").html(title);
    $modal.find(".modal-body").html(body);
    $modal.modal("show");
}