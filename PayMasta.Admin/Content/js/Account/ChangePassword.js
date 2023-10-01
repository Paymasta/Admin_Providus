$(document).ready(function () {
    $('#txtPasswordChangeError').hide();
    $('#txtPasswordChangePassError').hide();
    $('#txtPasswordChangeOldError').hide();
    $('#txtConPasswordChangePassError').hide();
    $('#txtConPasswordChangePassError').hide();
    $.getJSON("https://api.ipify.org/?format=json", function (e) {
        ipAddress = e.ip;
        console.log(e.ip);
    });
    var UserGuid = "";
    let passwordError = true;
    let newpasswordError = true;
    let ConpasswordError = true;
    let passwordStrengh = false;
    // var encrypted = CryptoJS.AES.encrypt(JSON.stringify(response), response.UserGuid);
    //alert(encrypted.toString());
    //var decrypted = CryptoJS.AES.decrypt(encrypted, "Secret Passphrase");

    UserGuid = sessionStorage.getItem("User1");
    //var data = sessionStorage.getItem("User");
    //var decrypted = CryptoJS.AES.decrypt(data, UserGuid);
    //var resData = decrypted.toString(CryptoJS.enc.Utf8);
    //var guid = JSON.stringify(resData);
    //var userData = JSON.parse(guid);

    $('#btnChangePassword').click(function () {
        
        var txtOldPassword = $('#txtOldPassword').val();
        var txtNewPassword = $('#txtNewPassword').val();
        var txtconfirmPassword = $('#txtconfirmPassword').val();

        if (txtOldPassword == '' || txtOldPassword == "" || txtOldPassword == null) {

            $('#txtPasswordChangeOldError').show();
            $('#txtPasswordChangeOldError').text("Please enter old password");
            passwordError = false;
        }
        else {
            passwordError = true;
            $('#txtPasswordChangeOldError').hide();
        }
        if (txtNewPassword == '' || txtNewPassword == "" || txtNewPassword == null) {
            $('#txtPasswordChangePassError').show();
            $('#txtPasswordChangePassError').text("Please enter new password.");
            newpasswordError = false;
            // return false;
        } else {
            if (txtNewPassword.length < 8) {
                $('#txtPasswordChangeError').empty();
                $('#txtPasswordChangePassError').empty();
                $('#txtPasswordChangePassError').show();
                $('#txtPasswordChangePassError').text("Password should be 8 characters");
                newpasswordError = false;
            } else {
                $('#txtPasswordChangePassError').hide();
                newpasswordError = true;
            }

            // return true;
        }
        if (txtNewPassword != txtconfirmPassword) {
            $('#txtConPasswordChangePassError').show();
            $('#txtConPasswordChangePassError').text("New password and confirm password miss match!");

            ConpasswordError = false;
            // return false;
        } else {

            ConpasswordError = true;
            $('#txtConPasswordChangePassError').hide();
            // return true;
        }
        if (passwordError == true && newpasswordError == true && ConpasswordError == true && passwordStrengh == true) {

            /*var req = { Email: email, Password: password }*/
            var formData = new FormData();
            formData.append("OldPassword", txtOldPassword);
            formData.append("Password", txtNewPassword);
            formData.append("UserGuid", UserGuid);


            $.ajax({
                type: "POST",
                cache: false,
                url: "ChangePassword",
                contentType: false,
                processData: false,
                data: formData,
                success: function (response) {
                    console.log(response);

                    if (response == 1) {

                        swal({
                            title: "Success",
                            text: "Your password changed successfully.",
                            icon: "Success",
                            buttons: true,
                            dangerMode: true,
                        }, function succes(isDone) {
                            //var baseUrl = window.location.g
                            var base_url = window.location.origin;
                            window.location.href = base_url + "/Landing";
                        })
                        //swal(
                        //    'Success!',
                        //    'Your password changed successfully.',
                        //    'success'
                        //).catch(swal.noop);

                    }
                    else if (response == 2) {

                        swal(
                            'Error!',
                            'Old Password is not correct.',
                            'error'
                        ).catch(swal.noop);
                        window.location.href = "Account/Index";
                    }
                    else if (response == 3) {

                        swal(
                            'Error!',
                            'Old password and new password can not be same.',
                            'error'
                        ).catch(swal.noop);
                        // window.location.href = "Account/Index";
                    }
                    //if (response == "success") {
                    //    ClearControls();
                    //    swal(
                    //        'Success!',
                    //        'Ticket Created.',
                    //        'success'
                    //    ).catch(swal.noop);
                    //}
                    //else if (response == "fail") {
                    //    swal(
                    //        'Error!',
                    //        'Please Try Again.',
                    //        'error'
                    //    ).catch(swal.noop);
                    //}
                    //else {
                    //    swal(
                    //        'Error!',
                    //        response,
                    //        'error'
                    //    ).catch(swal.noop);
                    //}
                }
            });

        } else {
            return false;
        }
    });


    $('#txtNewPassword').keyup(function () {
        $('#txtPasswordChangeError').show();
        $('#txtPasswordChangeError').html(checkStrength($('#txtNewPassword').val()))
    })


    function checkStrength(password) {

        var strength = 0
        if (password.length < 7) {
            $('#spntxtForgetPasswordError').empty();
            $('#txtPasswordChangeError').empty();
            $('#txtPasswordChangePassError').empty();
            $('#txtPasswordChangeError').removeClass()
            //$('#strengthMessage').addClass('Short')
            return 'The password is too short'
        }

        var regex = new RegExp("^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[!@#\$%\^&\*_])(?=.{8,30}$)");

        if (regex.test(password)) {
            // $('#txtPasswordRegisterError').removeClass()
            $('#txtPasswordChangeError').hide()
            passwordStrengh = true;
            return 'Strong'
        } else {
            $('#txtPasswordChangeError').show();
            $('#txtPasswordChangeError').empty();
            $('#txtPasswordChangePassError').empty();
            $('#txtPasswordChangeError').text("Password must contain at least one uppercase,one lowercase,one special character and one number.")
            // $('#strengthMessage').addClass('Weak')
            passwordStrengh = false;
            // return false;

        }

    }

    $("body").on('click', '#txtPasswordRegister1', function () {

        $(this).toggleClass("toggle-password fa-eye fa-eye-slash");
        var input = $("#txtOldPassword");
        if (input.attr("type") === "password") {
            input.attr("type", "text");
        } else {
            input.attr("type", "password");
        }

    });

    $("body").on('click', '#txtNewPasswordIcon', function () {

        $(this).toggleClass("toggle-password fa-eye fa-eye-slash");
        var input = $("#txtNewPassword");
        if (input.attr("type") === "password") {
            input.attr("type", "text");
        } else {
            input.attr("type", "password");
        }

    });

    $("body").on('click', '#txtconfirmPasswordIcon', function () {

        $(this).toggleClass("toggle-password fa-eye fa-eye-slash");
        var input = $("#txtconfirmPassword");
        if (input.attr("type") === "password") {
            input.attr("type", "text");
        } else {
            input.attr("type", "password");
        }

    });


    $('#divLogout').click(function () {

        var UserGuid = sessionStorage.getItem("User1");

        swal({
            title: "Are you sure you want to logout?",
            // text: "Logout",
            icon: "warnning",
            buttons: true,
            dangerMode: true,
            showCancelButton: true,
        }, function succes(isDone) {
            logout(UserGuid);

        })

    });


    function logout(UserGuid) {
        var formData = new FormData();
        formData.append("UserGuid", UserGuid);
        formData.append("DeviceId", ipAddress);
        $.ajax({
            type: "POST",
            //cache: false,
            url: "/Account/Logout/",
            //contentType: false,
            processData: false,
            data: formData,
            timeout: 40000,

            success: function (response) {

                if (response == 1) {
                    console.log(response);
                    sessionStorage.clear();
                    var base_url = window.location.origin;
                    //window.location.href = base_url + "/Account/Index";
                    //window.location.reload();
                    //window.location = $('#next').attr('href');
                    setTimeout(function () { document.location.href = base_url }, 500);
                }
                else {

                    swal(
                        'Error!',
                        'Please Try again.',
                        'error'
                    ).catch(swal.noop);
                    // window.location.href = "Account/Index";
                }

            }
        });
    }


    $(function () {
        $('#txtOldPassword').on('keypress', function (e) {
            if (e.which == 32) {
                //alert('Space not allowed');
                return false;
            }
        });
    });


    $(function () {
        $('#txtNewPassword').on('keypress', function (e) {
            if (e.which == 32) {
               // alert('Space not allowed');
                return false;
            }
        });
    });


    $(function () {
        $('#txtconfirmPassword').on('keypress', function (e) {
            if (e.which == 32) {
              //  alert('Space not allowed');
                return false;
            }
        });
    });
    $(document).on('click', '#btnCancel', function () {
        window.href = window.history.back();
    });
});

