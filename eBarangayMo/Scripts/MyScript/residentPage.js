$(document).ready(function () {
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
