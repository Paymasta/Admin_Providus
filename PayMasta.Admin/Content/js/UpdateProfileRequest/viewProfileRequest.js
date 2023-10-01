var UserGuid = "";
var AdminGuid = "";
$(document).ready(function () {
    //$("#new_profile_image").hide();
    //$("#old_profile_image").hide();
    AdminGuid = sessionStorage.getItem("User1");
    getUrlVars();
    function getUrlVars() {
        var vars = [], hash;
        var hashes = window.location.href.slice(window.location.href.indexOf('?') + 1).split('&');
        for (var i = 0; i < hashes.length; i++) {
            hash = hashes[i].split('=');
            vars.push(hash[0]);
            vars[hash[0]] = hash[1];
        }
        UserGuid = hash[1];
        getEmployerOldProfile(UserGuid);
        getEmployerNewProfile(UserGuid);
        return vars;
    }
    $(document).on('click', '#updateRequest', function () {
        var requestId = $("#updateRequest").data('val');
        var formData = new FormData();
        formData.append("userGuid", UserGuid);
        formData.append("AdminGuid", AdminGuid);
        formData.append("UpdateUserProfileRequestId", requestId);
        $.ajax({
            type: "POST",
            cache: false,
            url: "/UpdateProfileRequest/UpdateEmployeeProfileByAdmin",
            contentType: false,
            processData: false,
            data: formData,
            success: function (response) {
                console.log(response);
                if (response.RstKey == 1) {
                    swal({
                        title: "Yess!",
                        text: response.Message,
                        icon: "error",
                        //button: "ohh no!"
                    });
                    window.location.href = '/UpdateProfileRequest/Index';

                } else {
                    swal({
                        title: "oops!",
                        text: response.Message,
                        icon: "error",
                        button: "ohh no!"
                    });
                }
            }
        });

    });
});
function getEmployerOldProfile(guid) {

    var formData = new FormData();
    formData.append("userGuid", guid);

    $.ajax({
        type: "POST",
        cache: false,
        url: "/UpdateProfileRequest/GetOldProfileDetail",
        contentType: false,
        processData: false,
        data: formData,
        success: function (response) {
            console.log(response);
            if (response.RstKey == 1) {
                var value = response.updateProfileRequestResponse;
             
                if (value.ProfileImage === null || value.ProfileImage == "") {
                    $("#old_profile_image").attr('src', "/Content/img/images/man-placeholder.png");
                    $("#old_profile_image1").attr('src', "/Content/img/images/man-placeholder.png");
                }
                else {
                    $("#old_profile_image").attr('src', value.ProfileImage);
                    $("#old_profile_image1").attr('src', value.ProfileImage);
                  
                }
                $("#old_first_name").text(value.FirstName);
                $("#old_middle_name").text(value.MiddleName);
                $("#old_last_name").text(value.LastName);
                $("#old_email").text(value.Email);
                $("#old_email").attr('title', value.Email);
                $("#old_phoneNumber").text(value.CountryCode + ' ' + value.PhoneNumber);
                $("#old_address").text(value.Address);
                $("#old_address").attr('title', value.Address);

            } else {
                swal({
                    title: "oops!",
                    text: response.Message,
                    icon: "error",

                    button: "ohh no!"
                });
            }
        }
    });

}
function getEmployerNewProfile(guid) {

    var formData = new FormData();
    formData.append("userGuid", guid);

    $.ajax({
        type: "POST",
        cache: false,
        url: "/UpdateProfileRequest/GetNewProfileDetail",
        contentType: false,
        processData: false,
        data: formData,
        success: function (response) {
            console.log(response);
            if (response.RstKey == 1) {
                var value = response.updateProfileRequestResponse;
                //if (value.ProfileImage === null) {
                //    $("#default_new_profile").show();
                //}
                //else {
                //    $("#default_new_profile").hide();
                //    $("#new_profile_image").show();
                //    $("#new_profile_image").attr('src', value.ProfileImage);

                //}
                $("#new_first_name").text(value.FirstName);
                $("#new_middle_name").text(value.MiddleName);
                $("#new_last_name").text(value.LastName);
                $("#new_email").text(value.Email);
                $("#new_email").attr('title', value.Email);
                $("#new_phoneNumber").text(value.CountryCode + ' ' + value.PhoneNumber);
                $("#new_address").text(value.Address);
                $("#new_Comment").text(value.Comment);
                $("#new_Comment").attr('title', value.Comment);
                $("#updateRequest").attr('data-val', value.RowNumber);
                
            } else {
                swal({
                    title: "oops!",
                    text: response.Message,
                    icon: "error",
                    button: "ohh no!"
                });
            }
        }
    });

}