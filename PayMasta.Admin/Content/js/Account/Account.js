let usernameError = true;
let emailFormateError = true;
let mobileFormateError = true;
let passwordError = true;
let mobileNumberError = true;
let ConpasswordError = true;
let ConpasswordError1 = true
var mobileVerify = "";
let logintype = 0;
var OtpPass = "";
var passwordStrengh = false;
let timerOn = false;
var forgetOtpEmailOrMobile = "";
let IsTermConditionChecked = false;
var UserGuid = "";
$(document).ready(function () {

    document.onkeypress = function (event) {
        event = (event || window.event);
        if (event.keyCode == 123) {
            return false;
        }
    }
    document.onmousedown = function (event) {
        event = (event || window.event);
        if (event.keyCode == 123) {
            return false;
        }
    }
    document.onkeydown = function (event) {
        event = (event || window.event);
        if (event.keyCode == 123) {
            return false;
        }
    }

    $(document).bind('contextmenu.namespace', function () {
        return false;
    });

    let ipAddress = "";
    UserGuid = sessionStorage.getItem("User1");
    $.getJSON("https://api.ipify.org/?format=json", function (e) {
        ipAddress = e.ip;
        console.log(e.ip);
    });
    $('#usercheck').hide();
    $('#mobilechk').hide();
    $('#passcheck').hide();
    $('#txtForgetEmailError').hide();
    $('#txtForgetMobileError').hide();
    $('#spntxtForgeContPasswordError').hide();
    const $elemMobile = $("#divMobile");
    $elemMobile[0].style.setProperty('display', 'none', 'important');

    let logintype = 0;
    $('input:radio[name="userLogin"]').change(function () {
        debugger
        $("#txtEmail").val('');
        $("#txtPassword").val('');
        $("#txtMobile").val('');
        if ($(this).val() == 0) {
            const $elem = $("#divMobile");
            $elem[0].style.setProperty('display', 'none', 'important');
            //$('#divMobile').hide();
            $('#divEmail').show();
            //    $('#usercheck').hide();
            logintype = 0
        }
        else if ($(this).val() == 1) {
            // $('#divEmail').hide();
            const $elemEmail = $("#divEmail");
            $elemEmail[0].style.setProperty('display', 'none', 'important');
            $('#divMobile').show();
            $('#passcheck').hide();
            // $('#usercheck').hide();
            logintype = 1;
        }
    });

    // Submit button
    $('#btnLogin').click(function () {

        //  $('#loader1').show();
        // $('#btnLogin').prop('disabled', true);
        var EmailOrMobile = "";
        var email = $('#txtEmail').val();
        var txtMobile = $('#txtMobile').val();
        var password = $('#txtPassword').val();

        if (logintype == 0) {
            if (email == "") {
                $('#usercheck').show();
                usernameError = false;
                $('#btnLogin').prop('disabled', false);
                // return false;
            }
            else {
                $('#usercheck').hide();
                usernameError = true;
                // return true;

            }
            if (password == '' || password == "" || password == null) {
                $('#passcheck').show();
                passwordError = false;
                $('#btnLogin').prop('disabled', false);
                // return false;
            } else {
                $('#passcheck').hide();
                passwordError = true;
                // return true;
            }

            validateEmail();
        }
        else if (logintype == 1) {
            if (txtMobile == "") {
                $('#mobilechk').show();
                usernameError = false;
                $('#btnLogin').prop('disabled', false);
            }
            else {
                $('#mobilechk').hide();
                usernameError = true;
                // return true;

            }
            if (password == '' || password == "" || password == null) {
                $('#passcheck').show();
                passwordError = false;
                $('#btnLogin').prop('disabled', false);
                // return false;
            } else {
                $('#passcheck').hide();
                passwordError = true;
                //validateMobile();
                // return true;
            }

        }
        if ((usernameError == true) && (passwordError == true) && emailFormateError == true && mobileFormateError == true) {


            if (email != "") {
                EmailOrMobile = email;
            } else if (txtMobile != "") {
                EmailOrMobile = txtMobile;

            }

            /*var req = { Email: email, Password: password }*/
            var formData = new FormData();
            formData.append("Email", EmailOrMobile);
            formData.append("Password", password);
            formData.append("DeviceType", 3);
            formData.append("DeviceId", ipAddress);
            formData.append("DeviceToken", ipAddress);

            $.ajax({
                type: "POST",
                cache: false,
                url: "Account/Login",
                contentType: false,
                processData: false,
                data: formData,
                success: function (response) {
                    console.log(JSON.stringify(response));
                    $('#btnLogin').prop('disabled', false);
                    $('#loader').hide();
                    if (response.RstKey == 1 && response.RoleId == 1) {
                        $('#loader1').hide();
                        var encrypted = CryptoJS.AES.encrypt(JSON.stringify(response), response.UserGuid);
                        //alert(encrypted.toString());
                        //var decrypted = CryptoJS.AES.decrypt(encrypted, "Secret Passphrase");
                        sessionStorage.setItem("User", encrypted.toString());
                        sessionStorage.setItem("User1", response.UserGuid);
                        if (response.ProfileImage == "" || response.ProfileImage == null) {
                            sessionStorage.setItem("ProfileImage", "/Content/images/avatar.jpg");
                        } else {
                            sessionStorage.setItem("ProfileImage", response.ProfileImage);
                        }
                        sessionStorage.setItem("UserName", response.FirstName + " " + response.LastName);
                        // $("#spnUserName").text(response.FirstName + " " + response.LastName);
                        $("#loginModal").modal('hide');

                        window.location.href = "Home/Index";
                        swal(
                            'Success!',
                            'Logged in.',
                            'success'
                        ).catch(swal.noop);

                    }
                    else if (response.RstKey == 2) {

                        swal(
                            'Error!',
                            'Email or Password is not correct,Please Try Again.',
                            'error'
                        ).catch(swal.noop);
                        window.location.href = "Account/Index";
                    }
                    else if (response.RstKey == 3) {

                        swal(
                            'Error!',
                            'You can not login,you are blocked by admin.',
                            'error'
                        ).catch(swal.noop);

                    }
                    else if (response.RstKey == 4) {

                        swal(
                            'Error!',
                            'You can not login,your account is deleted by admin.',
                            'error'
                        ).catch(swal.noop);

                    }
                    else {

                        swal(
                            'Error!',
                            'Please Try Again.',
                            'error'
                        ).catch(swal.noop);

                    }

                }
            });

        } else {
            return false;
        }
    });



    $(function () {
        $('#txtEmail').on('keypress', function (e) {
            if (e.which == 32) {
                // alert('Space not allowed');
                return false;
            }
        });
    });

    $(function () {
        $('#txtMobile').on('keypress', function (e) {
            if (e.which == 32) {
                //  alert('Space not allowed');
                return false;
            }
        });
    });

    $(function () {
        $('#txtPassword').on('keypress', function (e) {
            if (e.which == 32) {
                // alert('Space not allowed');
                return false;
            }
        });
    });

    $(function () {
        $('#txtForgetEmail').on('keypress', function (e) {
            if (e.which == 32) {
                // alert('Space not allowed');
                return false;
            }
        });
    });
    $("#txtPasswordtoggle").click(function () {

        $(this).toggleClass("fa-eye fa-eye-slash");
        var input = $("#txtPassword");
        if (input.attr("type") == "password") {
            input.attr("type", "text");
        } else {
            input.attr("type", "password");
        }
    });
    $("#forgetPasswordAnchor").click(function () {
        $('#forgetPasswordModal').modal('show');
        $("#txtEmail").val('');
        $("#txtPassword").val('');
        $("#txtMobile").val('');
    });

    $("#btnRegister1").click(function () {
        $('#forgetPasswordModal').modal('hide');
    });

    //------------Auto switch
    $(".otp").keyup(function () {

        if (this.value.length == this.maxLength) {
            $(this).next('.otp').select();
        }
    });


    $('#btnForgetPassword').click(function () {
        debugger
        var txtForgetEmail = $('#txtForgetEmail').val();
        var txtForgetMobile = $('#txtForgetMobile').val();

        if (txtForgetEmail != "" && txtForgetMobile != "") {
            alert("Please enter email or mobile");
            return false;
        }

        var EmailOrPhone = "";
        var type = 1;
        if ((txtForgetEmail == '' || txtForgetEmail == "" || txtForgetEmail == null) && (txtForgetMobile == '' || txtForgetMobile == "" || txtForgetMobile == null)) {

            $('#txtForgetMobileError').show();
            $('#txtForgetEmailError').show();
            return false;
        }
        else {
            // $('#usercheck').hide();
            usernameError = true;
            $('#txtForgetMobileError').hide();
            $('#txtForgetEmailError').hide();
            // return true;

        }
        if (txtForgetEmail != "") {
            mobileVerify = txtForgetEmail;
            EmailOrPhone = txtForgetEmail;
            forgetOtpEmailOrMobile = txtForgetEmail;
            type = 1;
            validateEmailForgetPassword(txtForgetEmail);
        } else if (txtForgetMobile != "") {
            mobileVerify = txtForgetMobile;
            EmailOrPhone = txtForgetMobile;
            forgetOtpEmailOrMobile = txtForgetMobile;
            type = 2;
            validateForgetMobile(txtForgetMobile);
            //mobileFormateError
        }


        if (usernameError == true && emailFormateError == true && mobileFormateError == true) {
            /*var req = { Email: email, Password: password }*/
            var formData = new FormData();
            formData.append("EmailorPhone", EmailOrPhone);
            formData.append("Type", type);


            $.ajax({
                type: "POST",
                cache: false,
                url: "/Account/ForgotPassword/",
                contentType: false,
                processData: false,
                data: formData,
                success: function (response) {
                    debugger
                    console.log(response);
                    if (response != null && response != "" && response != '') {
                        $('#forgetPasswordModalloader1').hide();
                        swal({
                            title: "Success",
                            text: "Verification OTP sent to your email or mobile",
                            icon: "Success",
                            buttons: true,
                            dangerMode: true,
                        }, function succes(isDone) {
                            timerOn = true;
                            $('#btnResend').prop('disabled', true);
                            $('#btnResend').css("color", "#D3D3D3");
                            timer(59);
                            $("#forgetPasswordOtpverificationmodal").modal('show');
                            $("#forgetPasswordModal").modal('hide');
                            $("#verificationmodal").modal('show');

                        })
                        //swal(
                        //    'Success!',
                        //    'Verification OTP sent to your email or mobile',
                        //    'success'
                        //).catch(swal.noop);

                    }
                    else {

                        swal(
                            'Error!',
                            'User not registered with this credentials',
                            'error'
                        ).catch(swal.noop);
                        // window.location.href = "Account/Index";
                    }

                }
            });

        } else {
            return false;
        }
    });


    //$('#btnOtpVerify').click(function () {
    //    $("#verificationmodal").modal('hide');
    //    $("#createpasswordmodal").modal('show');
    //});

    $('#btnResend').click(function () {
        // $('#loader1').show();
        var txtForgetEmail = forgetOtpEmailOrMobile;

        //empty after verified
        var FirstDigit = $('#txtForgetPasswordFirstDigit').val('');
        var SecondDigit = $('#txtForgetPasswordSecondDigit').val('');
        var ThirdDigit = $('#txtForgetPasswordThirdDigit').val('');
        var FourthDigit = $('#txtForgetPasswordFourthDigit').val('');
        // txtForgetEmail

        if (txtForgetEmail != "") {
            /*var req = { Email: email, Password: password }*/
            var formData = new FormData();
            formData.append("EmailorPhone", txtForgetEmail);
            formData.append("Type", 2);


            $.ajax({
                type: "POST",
                cache: false,
                url: "Account/ForgotPassword",
                contentType: false,
                processData: false,
                data: formData,
                success: function (response) {
                    console.log(response);
                    if (response != null) {
                        $('#loader1').hide();
                        swal({
                            title: "Success",
                            text: "Verification OTP sent to your email or mobile",
                            icon: "Success",
                            buttons: true,
                            dangerMode: true,
                        }, function succes(isDone) {
                            timerOn = true;
                            $('#btnResend').prop('disabled', true);
                            $('#btnResend').css("color", "#D3D3D3");
                            timer(59);
                            $("#verificationmodal").modal('show');
                            $("#forgetPasswordModal").modal('hide');
                        })
                        //swal(
                        //    'Success!',
                        //    'Verification OTP sent to your email or mobile',
                        //    'success'
                        //).catch(swal.noop);

                    }
                    else {

                        swal(
                            'Error!',
                            ' OTP not sent',
                            'error'
                        ).catch(swal.noop);
                        // window.location.href = "Account/Index";
                    }

                }
            });

        } else {
            return false;
        }
    });

    $('#btnOtpVerify').click(function () {
        $('#loader1').show();
        $('#btnOtpVerify').prop('disabled', true);
        var FirstDigit = $('#txtFirstDigit').val();
        var SecondDigit = $('#txtSecondDigit').val();
        var ThirdDigit = $('#txtThirdDigit').val();
        var FourthDigit = $('#txtFourthDigit').val();


        if (FirstDigit == '' || FirstDigit == "" || FirstDigit == null) {

            alert("FirstDigit can not empty")
            return false;
        }
        else {
            // $('#usercheck').hide();
            usernameError = true;
            // return true;

        }
        if (SecondDigit == '' || SecondDigit == "" || SecondDigit == null) {
            //$('#passcheck').show();
            alert("SecondDigit can not empty")
            return false;
        }
        if (ThirdDigit == '' || ThirdDigit == "" || ThirdDigit == null) {
            //$('#passcheck').show();
            alert("ThirdDigit can not empty")
            return false;
        }
        if (FourthDigit == '' || FourthDigit == "" || FourthDigit == null) {
            //$('#passcheck').show();
            alert("FourthDigit can not empty")
            return false;
        }
        var otp = FirstDigit.concat(SecondDigit, ThirdDigit, FourthDigit);
        OtpPass = otp;
        if (otp.length >= 4) {
            /*var req = { Email: email, Password: password }*/
            var formData = new FormData();


            formData.append("EmailorPhone", mobileVerify);
            formData.append("OtpCode", otp);
            formData.append("Type", 1);


            $.ajax({
                type: "POST",
                cache: false,
                url: "Account/VerifyForgetPasswordOTP",
                contentType: false,
                processData: false,
                data: formData,
                success: function (response) {
                    $('#loader1').hide();

                    $('#btnOtpVerify').prop('disabled', false);
                    if (response.RstKey == 6) {
                        $("#verificationmodal").modal('hide');
                        $(".modal-body input").val("");
                        swal({
                            title: "Success",
                            text: "OTP verified",
                            icon: "Success",
                            buttons: true,
                            dangerMode: true,
                        }, function succes(isDone) {
                            $("#verificationmodal").modal('hide');
                            $("#createpasswordmodal").modal('show');

                        })
                        //swal(
                        //    'Success!',
                        //    'Verifucation OTP sent to your email or mobile',
                        //    'success'
                        //).catch(swal.noop);

                    }
                    else {
                        $('#btnOtpVerify').prop('disabled', false);
                        swal(
                            'Error!',
                            'Invalid OTP',
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
            //$.ajax({
            //    type: "POST",
            //    url: "AccountController/Login",
            //    data: req,
            //    success: function (data) {
            //        console.log(data)
            //    }
            //});
            //return true;
        } else {
            return false;
        }
    });

    $('#btnSavePassword').click(function () {
        //$('#loader1').show();
        var txtForgetPassword = $('#txtForgetPassword').val();
        var txtForgetConfirmPassword = $('#txtForgetConfirmPassword').val();
        let txtForgetPasswordpasswordError = true

        if (txtForgetPassword == '' || txtForgetPassword == "" || txtForgetPassword == null) {

            $('#spntxtForgetPasswordError').show();
            $('#spntxtForgetPasswordError').text("Please enter your password.");
            txtForgetPasswordpasswordError = false;
            // return false;
        } /*else {
            txtForgetPasswordpasswordError = true;
            $('#spntxtForgetPasswordError').hide();
        }*/
        else {
            if (txtForgetPassword.length < 8) {
                $('#spntxtForgetPasswordError').empty();
                $('#txtPasswordChangeError').empty();
                $('#txtPasswordChangePassError').empty();
                $("#strengthMessage1").empty();

                $('#spntxtForgetPasswordError').show();

                $('#spntxtForgetPasswordError').text("Password should be 8 characters");
                txtForgetPasswordpasswordError = false;
            } else {
                $('#spntxtForgetPasswordError').hide();
                txtForgetPasswordpasswordError = true;
            }

            // return true;
        }
        if (txtForgetConfirmPassword == '' || txtForgetConfirmPassword == "" || txtForgetConfirmPassword == null) {
            //$('#passcheck').show();
            // alert("Password miss match!")
            $('#spntxtForgeContPasswordError').show();
            $('#spntxtForgeContPasswordError').text("Please enter your confirm password.");

            ConpasswordError = false;
            // return false;
        }
        else {
            if (txtForgetConfirmPassword.length < 8) {
                $('#spntxtForgetPasswordError').empty();
                $('#txtPasswordChangeError').empty();
                $('#txtPasswordChangePassError').empty();
                $("#strengthMessage1").empty();

                $('#spntxtForgetPasswordError').show();

                $('#spntxtForgetPasswordError').text("Password should be 8 characters");
                txtForgetPasswordpasswordError = false;
            } else {
                $('#spntxtForgetPasswordError').hide();
                txtForgetPasswordpasswordError = true;
            }

            // return true;
        }
        /*if (txtForgetPassword != txtForgetConfirmPassword) {
            $('#spntxtForgeContPasswordError').show();
            $('#spntxtForgeContPasswordError').text("New password and confirm password miss match!");

            ConpasswordError = false;
            // return false;
        } else {

            ConpasswordError = true;
            $('#spntxtForgeContPasswordError').hide();
            // return true;
        }*/

        /*else {
            // $('#passcheck').hide();
            ConpasswordError = true;
            // return true;
        }*/
        if (txtForgetPassword != "" && txtForgetPassword != txtForgetConfirmPassword && passwordStrengh == true) {
            //$('#passcheck').show();
            // alert("Password miss match!")
            $('#spntxtForgeContPasswordError').show();
            $('#spntxtForgeContPasswordError').text("Password and confirm password not match!");

            ConpasswordError = false;
            // return false;
        } else {
            // $('#passcheck').hide();
            ConpasswordError = true;
            $('#spntxtForgeContPasswordError').hide();
            // return true;
        }

        if (ConpasswordError == true && txtForgetPasswordpasswordError == true && passwordStrengh == true) {
            /*var req = { Email: email, Password: password }*/
            var formData = new FormData();
            formData.append("EmailorPhone", mobileVerify);
            formData.append("OtpCode", OtpPass);
            formData.append("Password", txtForgetPassword);


            $.ajax({
                type: "POST",
                cache: false,
                url: "Account/ResetPassword",
                contentType: false,
                processData: false,
                data: formData,
                success: function (response) {
                    debugger
                    console.log(response);
                    if (response.RstKey == 1) {
                        $('#loader1').hide();
                        swal({
                            title: "Success",
                            text: "Your password has been updated successfully",
                            icon: "Success",
                            buttons: true,
                            dangerMode: true,
                        }, function succes(isDone) {
                            $("#createpasswordmodal").modal('hide');
                            $("#loginModal").modal('show');

                        })
                        //swal(
                        //    'Success!',
                        //    'Verifucation OTP sent to your email or mobile',
                        //    'success'
                        //).catch(swal.noop);

                    }
                    else if (response.RstKey == 3) {
                        swal(
                            'Error!',
                            ' The old password and new password are the same, please enter a different password.',
                            'error'
                        ).catch(swal.noop);
                       
                    }
                    else {

                        swal(
                            'Error!',
                            'Failed',
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
            //$.ajax({
            //    type: "POST",
            //    url: "AccountController/Login",
            //    data: req,
            //    success: function (data) {
            //        console.log(data)
            //    }
            //});
            //return true;
        } else {
            return false;
        }
    });

    //password hide and show
    $("body").on('click', '#txtForgetResetPassword', function () {

        $(this).toggleClass("toggle-password fa-eye fa-eye-slash");
        var input = $("#txtForgetPassword");
        if (input.attr("type") === "password") {
            input.attr("type", "text");
        } else {
            input.attr("type", "password");
        }

    });
    $("body").on('click', '#txtForgetResetConfirmPassword', function () {

        $(this).toggleClass("fa-eye fa-eye-slash");
        var input = $("#txtForgetConfirmPassword");
        if (input.attr("type") === "password") {
            input.attr("type", "text");
        } else {
            input.attr("type", "password");
        }

    });

    $('#txtForgetPassword').keyup(function () {
        $('#strengthMessage').show();
        $('#strengthMessage1').html(checkStrength1($('#txtForgetPassword').val()))
    })

    function checkStrength1(password) {

        var strength = 0
        if (password.length < 7) {
            $('#strengthMessage1').removeClass()
            // $('#strengthMessage1').addClass('Short')
            //return false;//'Too short'
            return 'The password is too short'
        }

        var regex = new RegExp("^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[!@#\$%\^&\*_])(?=.{8,30}$)");

        if (regex.test(password)) {
            $('#strengthMessage1').hide()
            //$('#strengthMessage1').addClass('Strong')
            passwordStrengh = true;
            return 'Strong'
        } else {
            $('#strengthMessage1').show()
            $('#strengthMessage1').text('Password must contain at least one uppercase,one lowercase,one special character and one number.')
            passwordStrengh = false;
        }

    }

    $(document).on('click', '#btnResetForgetPassValues', function () {
        $("#txtForgetEmail").val('');
        $("#txtForgetMobile").val('');
    });

    $(function () {
        $('#txtForgetConfirmPassword').on('keypress', function (e) {
            if (e.which == 32) {
                //  alert('Space not allowed');
                return false;
            }
        });
    });
});

// Validate Confirm Password
function validateEmail() {
    var userinput = $('#txtEmail').val();



    if (userinput.length != '') {

        var pattern = /^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$/;

        if (!pattern.test(userinput)) {
            emailFormateError = false;
            swal({
                title: "Email format is not valid",
                text: "Try again",
                type: "warning",
                //showCancelButton: true,
                confirmButtonColor: "#DD6B55",
                //confirmButtonText: "Delete",
                //closeOnConfirm: false
            }, function succes(isDone) {
                emailFormateError = false;
            })
        }
        else {
            emailFormateError = true
                ;
        }
    }


}

function validateEmailForgetPassword(email) {

    if (email.length != '') {

        var pattern = /^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$/;// /^\b[A-Z0-9._%-]+@[A-Z0-9.-]+\.[A-Z]{2,4}\b$/i

        if (!pattern.test(email)) {
            emailFormateError = false;
            swal({
                title: "Email format is not valid",
                text: "Try again",
                type: "warning",
                //showCancelButton: true,
                confirmButtonColor: "#DD6B55",
                //confirmButtonText: "Delete",
                //closeOnConfirm: false
            }, function succes(isDone) {
                emailFormateError = false;
                //$('#btnRegister').prop('disabled', false);
            })
        }
        else {
            emailFormateError = true;
        }
    }


}


function validateMobile() {
    var txtMobile = $('#txtMobile').val();
    if (txtMobile.length >= 9 && txtMobile.length <= 10) {
        mobileFormateError = true;
    } else {
        alert('Please put  11  digit mobile number');
        mobileFormateError = false;
    }
}
function validateForgetMobile(txtMobile) {
   
    if (txtMobile.length >= 9 && txtMobile.length <= 10) {
        mobileFormateError = true;
    } else {
        alert('Please put  11  digit mobile number');
        mobileFormateError = false;
    }
}

function timer(remaining) {

    var m = Math.floor(remaining / 60);
    var s = remaining % 60;

    m = m < 10 ? '0' + m : m;
    s = s < 10 ? '0' + s : s;
    document.getElementById('timer').innerHTML = m + ':' + s;
    remaining -= 1;
    if (remaining == 0) {
        // IsOtpResent = true;
        $('#btnResend').css("color", "#32A4FF");
        $('#btnResend').prop('disabled', false);
    }

    if (remaining >= 0 && timerOn) {
        setTimeout(function () {
            timer(remaining);
        }, 1000);
        return;
    }

    if (!timerOn) {

        return;
    }

    // Do timeout stuff here

}

