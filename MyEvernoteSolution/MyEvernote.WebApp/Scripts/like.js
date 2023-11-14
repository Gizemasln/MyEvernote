$(function () {

    var noteids = [];
    $("div[data-note-id]").each(function (i, e) {
        /*  elementlerin hepsini getir each fonk ile dön*/

        noteids.push($(e).data("note-id"));/* BİR DİZİYE ELEMAN EKLERKEN PUSH*/
    });


    $.ajax({
        method: "POST",
        url: "/Note/GetLiked",
        data: { ids: noteids }

    }).done(function (data) {

        if (data.result != null && data.result.length > 0) {

            for (var i = 0; i < data.result.length; i++) {
                var id = data.result[i];
                var likedNote = $("div[data-note-id=" + id + "]");
                //datanoteid si gelen id ye eşit olan div ibul
                var btn = likedNote.find("button[data-liked]")//buton olup dataliked olan buton bizim butonumuzdur
                var span = btn.find("span.like-star");
                btn.data("liked", true); //liked özelliğni true ya çekiyorum
                span.removeClass("glyphicon-star-empty");
                span.addClass("glyphicon-star");
            }
        }

    }).fail(function () {


    });

    $("button[data-liked]").click(function () {

        var btn = $(this);
        var liked = btn.data("liked");
        var noteid = btn.data("note-id");
        var spanStar = btn.find("span.like-star");
        var spanCount = btn.find("span.like-count");


        $.ajax({
            method: "POST",
            url: "/Note/SetLikeState",
            data: { "noteid": noteid, "liked": !liked }
        }).done(function (data) {

            if (data.hasError) {
                //hata varsa
                alert(data.errorMessage);
            }
            else {
                liked = !liked;
                btn.data("liked", liked);//yeni liked değerini vermek için butona
                spanCount.text(data.result);//sayısını tutan metni data.result dan gelen sayı ile güncelle

                spanStar.removeClass("glyphicon-star-empty");
                spanStar.removeClass("glyphicon-star");
                if (liked) {
                    spanStar.addClass("glyphicon-star")//like olmnuşssa  yıldız  ekle
                }
                else {
                    spanStar.addClass("glyphicon-star-empty")//like olmamışsa boş yıldız ekle
                }

            }

        }).fail(function () {

            alert("sunucu ile bağlantı kurulmadı");
        })

    });


});
