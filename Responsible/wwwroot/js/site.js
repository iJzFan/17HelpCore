// Write your JavaScript code.
$(document).ready(function () {
    if (isVisitor()) {
        //note:必须使用removeClass("hide")，不要使用show()
        //bootstrap的.hide 标注了!important
        $("[not-logon]").removeClass("hide");
    } else {
        $("[logon]").removeClass("hide");
    }
});

function isVisitor() {
    return $.cookie().USER_ID == undefined;
}