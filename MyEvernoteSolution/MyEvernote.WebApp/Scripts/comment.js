var noteid = -1;
var modalCommentBodyId = "#modal_comment_body";

$(function () {

    $('#modal_comment').on('show.bs.modal', function (e) {

        var btn = $(e.relatedTarget);
        noteid = btn.data("note-id");
        $(modalCommentBodyId).load("/Comment/ShowNoteComments/" + noteid);
    })


});
function doComment(btn, e, commentid, spanid) {

    var button = $(btn);
    var mode = button.data("edit-mode");


    if (e == "edit_clicked") {

        if (!mode) {
            //İLK TIKLANDIĞINDA EDİTLMEK İÇİN TRUE ÇEVİRİP YAZILABİLİR HALE GETİRİYOR
            button.data("edit-mode", true); //edit modu true yap
            button.removeClass("btn-warning");//warning kaldır
            button.addClass("btn-success"); //success e dönüştür
            var btnSpan = button.find("span");//span ara
            btnSpan.removeClass("glyphicon-edit");//editi kaldır
            btnSpan.addClass("glyphicon-ok");//ok yap
            $(spanid).addClass("editable");

            $(spanid).attr("contenteditable", true);
            $(spanid).focus();

        }
        else {
            //İKİNCİ TIKLANDIĞINDA FALSE ÇEVİREREK TKERAR ESKİ HALİNE YAZILAMAZ HALE GETİRİYOR
            button.data("edit-mode", false); //edit modu true yap
            button.addClass("btn-warning");//warning kaldır
            button.removeClass("btn-success"); //success e dönüştür
            var btnSpan = button.find("span");//span ara
            btnSpan.addClass("glyphicon-edit");//editi kaldır
            btnSpan.removeClass("glyphicon-ok");//ok yap
            $(spanid).removeClass("editable");

            $(spanid).attr("contenteditable", false);
            var txt = $(spanid).text();
            $.ajax({
                method: "POST",
                url: "/Comment/Edit/" + commentid,
                data: { text: txt }
            }).done(function (data) {

                if (data.result) {

                    //yorumlar partial tekrar yüklenir..
                    $(modalCommentBodyId).load("/Comment/ShowNoteComments/" + noteid);

                }
                else {
                    alert("Yorum Güncellenemedi.");

                }
            }).fail(function () {

                alert("sunucu ile bağlantı kurulmadı")

            });
        }

    }
    else if (e == "delete_clicked") {

        var dialog_res = confirm("Yorum Silinsin Mi?");
        if (!dialog_res) return false;
        $.ajax({
            method: "GET",
            url: "/Comment/Delete/" + commentid

        }).done(function (data) {

            if (data.result) {
                //yorumlar partial tekrar yüklenir..
                $(modalCommentBodyId).load("/Comment/ShowNoteComments/" + noteid);
            }
            else {
                alert("Yorum silinemedi.")
            }

        }).fail(function () {
            alert("Sunucu ile bağlantı kurulamadı");

        });
    }
    else if (e == "new_clicked") {

        var txt = $("#new_comment_text").val();
        $.ajax({
            method: "POST",
            url: "/Comment/Create/",
            data: { "text": txt, "noteid": noteid }

        }).done(function (data) {

            if (data.result) {
                //yorumlar partial tekrar yüklenir..
                $(modalCommentBodyId).load("/Comment/ShowNoteComments/" + noteid);
            }
            else {
                alert("Yorum Eklenemedi.")
            }

        }).fail(function () {
            alert("Sunucu ile bağlantı kurulamadı");

        });
    }
}
