$(document).ready(function () {
    $('#btnLogin').click(function () {
        var email = $('#txtemail').val();
        var pass = $('#txtpass').val();

        $.post('../Home/Login', {
            email: email,
            password: pass
        }, function (data) {
            if (data[0].mess == 1) {
                alert('Successfully Login');
            }
            else {
                alert('Invalid Email or Password');
            }
        });
    });
});



