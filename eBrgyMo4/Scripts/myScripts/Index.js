﻿$(document).ready(function () {
    $('#btnCreateAcc').click(function () {
        var lname = $('#txtLname').val();
        var fname = $('#txtfname').val();
        var mname = $('#txtmname').val();
        var age = $('#txtage').val();
        var birthdate = $('#txtbirthdate').val();
        var civilStat = $('#drpCivilStat').val();
        var mail = $('#txtEmail').val();
        var pass = $('#txtpassword').val();
        var residentID = Math.floor((Math.random() * 99999) + 1);
        var phonenum = $('#txtphoneNum').val();

        var a = document.getElementById("chckBrgyOff");
        if (a.checked == true) {
            var username = Math.random().toString(36).replace(/[^a-z]+/g, '').substr(0, 5);
            var officialID = Math.floor((Math.random() * 99999) + 1);
            var role = $('#drpRole').val();

            $.post('../Home/User', {
                lastname: lname,
                firstname: fname,
                middlename: mname,
                age: age,
                bdate: birthdate,
                civilStat: civilStat,
                mail: mail,
                pass: pass,
                residentID: residentID,
                phonenum: phonenum,
                username: username,
                role: role,
                officialID: officialID
            }, function (data) {
                if (data[0].mess == 1) {
                    alert('Successfully Sign Up' + '\nUsername: ' + username + '\nOfficial ID: ' + officialID + '\nResident ID: ' + residentID);
                    location.reload();
                }
                else {
                    alert('Failed to Sign Up');
                }
            });
        }
        else {
            var username = ' ';
            var officialID = ' ';
            var role = ' ';

            $.post('../Home/User', {
                lastname: lname,
                firstname: fname,
                middlename: mname,
                age: age,
                bdate: birthdate,
                civilStat: civilStat,
                mail: mail,
                pass: pass,
                residentID: residentID,
                phonenum: phonenum,
                username: username,
                role: role,
                officialID: officialID
            }, function (data) {
                if (data[0].mess == 1) {
                    alert('Successfully Sign Up' + '\nResident ID: ' + residentID);
                    location.reload();
                }
                else {
                    alert('Failed to Sign Up');
                }
            });
        }
    });
});

$(document).ready(function () {
    $('#btnLogin').click(function () {
        var email = $('#txtemail').val();
        var pass = $('#txtpass').val();

        $.post('../Home/Login', {
            email: email,
            password: pass
        }, function (data) {
            if (data[0].mess == 1) {
                if (email > String(5)) {
                    alert('Landing to Barangay Page');
                }
                else {
                    alert('Landing to Resident Page');
                }
            }
            else {
                alert('Invalid Email or Password');
            }
        });
    });
});

$(document).ready(function () {
    $("#btnlogin").click(function () {
        $("#login").show();
        $("#register").hide();
    })

    $("#btnregister").click(function () {
        $("#register").show();
        $("#login").hide();
    })
})

var x = document.getElementById("login");
var y = document.getElementById("register");
var z = document.getElementById("bttn");


function register() {
    z.style.left = "110px";
}

function login() {
    z.style.left = "0";
}

function CheckMe() {
    var a = document.getElementById("chckBrgyOff");
    var b = document.getElementById("OffRole");
    if (a.checked == true) {
        b.style.display = "block";
    }
    else {
        b.style.display = "none";
    }
}