$(document).ready(function () {
    $('#btnPost').click(function () {
        var post = $('#txtPostNews').val();
        var tDate = new Date();
        var date = tDate.getFullYear() + '-' + (tDate.getMonth() + 1) + '-' + tDate.getDate();
        var tTime = new Date();
        var time = tTime.getHours() + ":" + tTime.getMinutes() + ":" + tTime.getSeconds();
        var dateTime = date + ' ' + time;

        $.post('../Home/Post', {
            post: post,
            dateTime: dateTime
            //date: date,
            //time: time
        }, function (data) {
            if (data[0].mess == 1) {
                location.reload();
            }
            else {
                alert('Failed to Post');
            }
        });
    });

    $.post('../Home/displayNaU', {
    }, function (data) {
        var tbl = "";
        $('#dsplyNaU tr').remove();
        for (var view in data) {
            tbl += "<tr >";
            tbl += "<td>";
            tbl += "<b style='font-size: 16px; color: aqua;'>";
            tbl += data[view].name;
            tbl += "</b>";
            tbl += "<br />";
            tbl += "<b style='font-size: 10px; color: blue;'>";
            tbl += data[view].datetime;
            tbl += "</b>";
            tbl += "<br />";
            tbl += data[view].post;
            tbl += "</td>";
            tbl += "</tr>";
        }
        $("#dsplyNaU").html(tbl);
    });

});

