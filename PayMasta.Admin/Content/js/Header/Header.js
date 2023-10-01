$(document).ready(function () {
    var UserGuid = "";
    let ipAddress = "";
     UserGuid = sessionStorage.getItem("User1");
    $('.img-circle').attr('src', sessionStorage.getItem("ProfileImage"));
    $('.user-image').attr('src', sessionStorage.getItem("ProfileImage"));
    $('#btnAdminLogout').click(function () {
    $("#logoutModel").modal('show');
    });
   
    $.getJSON("https://api.ipify.org/?format=json", function (e) {
        ipAddress = e.ip;
     
    });

    $('#btnLogout').click(function () {
        debugger
        var formData = new FormData();
        formData.append("UserGuid", UserGuid);
        formData.append("DeviceId", ipAddress);

        $.ajax({
            type: "POST",
            cache: false,
            url: "/Account/Logout",
            contentType: false,
            processData: false,
            data: formData,
            success: function (response) {
                console.log('log out');
                if (response == 1) {
                    console.log(response);
                    sessionStorage.clear();
                    var base_url = window.location.origin;
                    window.location.href = base_url + "/Landing";
                   // window.location.reload();
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
    });



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
});