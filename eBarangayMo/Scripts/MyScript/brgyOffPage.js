$(document).ready(function () {
    $('#btnPost').click(function () {
        var post = $('#txtPostNews').val();

        $.post('../Home/Post', {
            post: post
        }, function (data) {
            if (data[0].mess == 1) {
                alert('Successfully Posted');
                location.reload();
            }
            else {
                alert('Failed to Post');
            }
        });
    });
});
