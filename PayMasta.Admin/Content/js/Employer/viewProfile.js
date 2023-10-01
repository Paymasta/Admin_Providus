var UserGuid = "";
var pageNumber = 1;
var PageSize = 10;
var totalPages = 1;
var searchText = '';
var AdminUserGuid = "";

$(document).ready(function () {
    $("#unblock_user").hide();
    $("#block_user").hide();

    AdminUserGuid = sessionStorage.getItem("User1");
    getUrlVars();
    // Read a page's GET URL variables and return them as an associative array.
    function getUrlVars() {
        var vars = [], hash;
        var hashes = window.location.href.slice(window.location.href.indexOf('?') + 1).split('&');
        for (var i = 0; i < hashes.length; i++) {
            hash = hashes[i].split('=');
            vars.push(hash[0]);
            vars[hash[0]] = hash[1];
        }
        UserGuid = hash[1];
        getEmployeeProfile(UserGuid)
        return vars;
    }
    $('body').on('click', '#delete_user', function (e) {
        e.preventDefault;

        swal({
            title: "Are you sure you want to remove this employer?",
            
            icon: "warnning",
            buttons: false,
            showConfirmButton: true,
            showCancelButton: true,
            dangerMode: true,
        }, function succes(isDone) {
            var formData = new FormData();
            formData.append("EmployeeUserGuid", UserGuid);
            formData.append("AdminUserGuid", AdminUserGuid);
            formData.append("DeleteOrBlock", 2);

            $.ajax({
                type: "POST",
                cache: false,
                url: "/Employers/BlockUnBlockEmployees",
                contentType: false,
                processData: false,
                data: formData,
                success: function (response) {
                    debugger
                    if (response.RstKey == 1) {
                        //getEmployerList();
                        //getEmployerList(UserGuid, searchParam, pageNumber, pageSize, FromDate, ToDate)
                        swal({
                            title: "Good job!",
                            text: response.Message,
                            icon: "success",
                            button: "Aww yiss!"
                        }, function succes(isDone) { window.location.href = "/Employers"; });
                    } else {
                        swal({
                            title: "oops!",
                            text: "Employer not deleted !",
                            icon: "error",
                            button: "ohh no!"
                        });
                    }
                }
            });
        })

    });

});
$('body').on('click', '.removedata', function (e) {
    e.preventDefault;
    var msg = $(this).attr('data-msg');
    swal({
        title: "Are you sure you want to "+ msg +" this user?",
        icon: "warnning",
        buttons: false,
        showConfirmButton: true,
        showCancelButton: true,
        dangerMode: true,
    },
        function succes(isDone) {
            var formData = new FormData();
            formData.append("EmployeeUserGuid", UserGuid);
            formData.append("AdminUserGuid", AdminUserGuid);
            formData.append("DeleteOrBlock", 1);

            $.ajax({
                type: "POST",
                cache: false,
                url: "/Employers/BlockUnBlockEmployees",
                contentType: false,
                processData: false,
                data: formData,
                success: function (response) {
                    if (response.RstKey == 1) {
                        //getEmployerList();
                        //getEmployerList(UserGuid, searchParam, pageNumber, pageSize, FromDate, ToDate)
                        swal({
                            title: "Good job!",
                            text: response.Message,
                            icon: "success",
                            button: "Aww yiss!"
                        }, function succes(isDone) { location.reload(); });
                    } else {
                        swal({
                            title: "oops!",
                            text: "User not blocked !",
                            icon: "error",
                            button: "ohh no!"
                        });
                    }
                }
            });
        })

});
$("#view_employer_employees_list").on('click', function (e) {
    window.location.href = "/Employers/GetEmployeesList/id=" + UserGuid;
});
function getEmployeeProfile(guid) {



    var formData = new FormData();
    /*formData.append("EmployerGuid", guid);*/
    formData.append("id", guid);
    // formData.append("EmployeerUserGuid", UserGuid);

    $.ajax({
        type: "POST",
        cache: false,
        url: "",
        contentType: false,
        processData: false,
        data: formData,
        success: function (response) {
            console.log(response);
            if (response.RstKey == 1) {
                let isblock = "";
                var value = response.employerResponses;
                if (value.ProfileImage === null || value.ProfileImage == "") {
                    $("#profileImg").attr('src', "/Content/img/images/man-placeholder.png");   
                }
                else {
                    $("#profileImg").attr('src', value.ProfileImage);
                }
                if (value.Status == 1) { $("#unblock_user").hide(); $("#block_user").show(); $("#block_user").attr('data-msg', 'block'); }
                else if (value.Status == 0) { $("#block_user").hide(); $("#unblock_user").show(); $("#unblock_user").attr('data-msg', 'unblock'); }
                $("#organisationName").text(value.OrganisationName);
                $("#email").text(value.Email);
                $("#phoneNumber").text(value.CountryCode + ' ' + value.PhoneNumber);
                $("#state").text(value.State);
                $("#office").text(value.Address);
                $("#office").attr('title', value.Address);
                $("#countryCode").text(value.CountryName);
                $("#postalCode").text(value.PostalCode);

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