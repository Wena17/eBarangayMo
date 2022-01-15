$(document).ready(function () {
    $('#btnCreateAcc').click(function () {
        var lname = $('#txtLname').val();
        var fname = $('#txtfname').val();
        var mname = $('#txtmname').val();
        var age = $('#txtage').val();
        var birthdate = $('#txtbirthdate').val();
        var role = $('#drpRole').val();
        var civilStat = $('#drpCivilStat').val();
        var email = $('#txtemail').val();
        var pass = $('#txtpass').val();
        var famCode = Math.floor((Math.random() * 99999) + 1);
        var officialNotifID = Math.floor((Math.random() * 99999) + 1);
        var residentNotifID = Math.floor((Math.random() * 99999) + 1);

        $.post('../Home/Account', {
            lastname: lname,
            firstname: fname,
            middlename: mname,
            age: age,
            bdate: birthdate,
            role: role,
            civilStat: civilStat,
            email: email,
            password: pass,
            famCode: famCode,
            officialNotifID: officialNotifID,
            residentNotifID: residentNotifID
        }, function (data) {
            if (data[0].mess == 1) {
                alert('Family Code: ' + famCode + '\nOfficial Notif ID: ' + officialNotifID + '\nResident Notif ID: ' + residentNotifID);
            }
            else {
                alert('Failed to Create');
            }

        });
    });
});



