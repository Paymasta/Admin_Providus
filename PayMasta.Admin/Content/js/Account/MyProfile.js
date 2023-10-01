$(document).ready(function () {
    $('.profile-pic').attr('src', '/Content/img/images/man-placeholder.png');

    //upload profile pic
    var readURL = function (input) {
        if (input.files && input.files[0]) {
            var reader = new FileReader();

            reader.onload = function (e) {
                $('.profile-pic').attr('src', e.target.result);
            }

            reader.readAsDataURL(input.files[0]);
        }
    }
    $(".file-upload").on('change', function () {

        var ext = $('#profile-pic').val().split('.').pop().toLowerCase();
        if ($.inArray(ext, ['gif', 'png', 'jpg', 'jpeg']) == -1) {
            alert('invalid extension!,You can upload image in these formats(gif,png,jpg,jpeg)');
            return false;
        } else {
            readURL(this);
        }


    });
    $(".upload-button").on('click', function () {

        $(".file-upload").click();


    });

    $("#btnSave").prop("disabled", true);
    getProfileData();

    function getProfileData() {
        var id = sessionStorage.getItem("User1");
        var formData = new FormData();
        formData.append("guid", id);
        $.ajax({
            type: "POST",
            cache: false,
            url: "/Account/MyProfile/",
            contentType: false,
            processData: false,
            data: formData,
            success: function (response) {
                console.log(response);
                if (response != null) {
                    if (response.ProfileImage == "") {
                        $('.profile-pic').attr('src', '/Content/img/images/man-placeholder.png');
                    } else {
                        $('.profile-pic').attr('src', response.ProfileImage);
                    }
                    $("#proName").val(response.FirstName);
                    $("#profile_old_name").val(response.FirstName);
                    $("#proEmail").text(response.Email);
                    $("#proPhoneNumber").text(response.CountryCode + ' ' + response.PhoneNumber);
                    $("#proCountry").text(response.Country);
                    $("#proWalletBalance").text(response.CurrentBalance);
                }
                else {
                    swal({
                        title: "oops!",
                        text: "Data Not Found !",
                        icon: "error",
                        button: "ohh no!"
                    });
                }
                
                    
            }
        });
        
    }
    $(document).on('keyup', "#proName", function () {
        var name = $("#proName").val();
        var old_name = $("#profile_old_name").val();
        if (name != "" && old_name != name) {
            $("#btnSave").prop("disabled", false);
        }
        else {
            $("#btnSave").prop("disabled", true);
        }
    });
    $(document).on('click', '#btnSave', function () {
        var id = sessionStorage.getItem("User1");
        var name = $("#proName").val();
        var old_name = $("#profile_old_name").val();
        var regex = /^[A-Za-z0-9 ]+$/;
        if (name != "" && name != old_name && regex.test(name)) {
            var formData = new FormData();
            formData.append("AdminGuid", id);
            formData.append("Name", name);
            $.ajax({
                type: "POST",
                cache: false,
                url: "/Account/UpdateAdminProfile/",
                contentType: false,
                processData: false,
                data: formData,
                success: function (response) {
                    if (response != null) {
                        //swal({
                        //    title: "Success",
                        //    text: "Your profile has been updated successfully!",
                        //    icon: "success",
                        //    button: "Aww yiss!"
                        //});

                        swal({
                            title: "",
                            text: "Your profile has been updated successfully!",
                            icon: "Success",
                            buttons: true,
                            dangerMode: true,
                        }, function succes(isDone) {
                            window.location.reload();
                        })

                       
                    }
                    else {
                        swal({
                            title: "oops!",
                            text: "Something went wrong !",
                            icon: "error",
                            button: "ohh no!"
                        });
                    }
                    


                }
            });
        }
        else {
            swal({
                title: "oops!",
                text: "Name can not be updated !",
                icon: "error",
                button: "ohh no!"
            });
        }
        
        

    });
    $(document).on('click', '#btnCancel', function () {
        window.href = window.history.back();
    });

    $("#btnSaveImage").click(function () {

        var UserGuid = sessionStorage.getItem("User1");
        var fi = $('#profile-pic')[0].files[0];
        var formData = new FormData();
        formData.append('file', $('#profile-pic')[0].files[0]);
        formData.append('guid', UserGuid);
        $.ajax({
            url: '/Account/UploadFiles',
            type: 'POST',
            data: formData,
            processData: false,  // tell jQuery not to process the data
            contentType: false,  // tell jQuery not to set contentType
            success: function (response) {
                if (response.RstKey == 1) {
                    $('#uploadProfileModal').modal('hide')
                    swal(
                        '',
                        'Your profile has been updated successfully',
                        ''
                    ).catch(swal.noop);
                }
                else if (response.RstKey == 3) {
                    swal(
                        'Error!',
                        response.Message,
                        'error'
                    ).catch(swal.noop);
                }
                else {
                    swal(
                        'Error!',
                        response.Message,
                        'error'
                    ).catch(swal.noop);
                }
            }
        });
    })
});