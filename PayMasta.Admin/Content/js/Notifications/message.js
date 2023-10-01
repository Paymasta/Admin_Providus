$(document).ready(function () {

    $(function () {
        $('#notifyMessage').on('keypress', function (e) {
            if (event.target.value.substr(-1) === ' ' && event.code === 'Space') {
                return false;
            }
        });
    });


    $(document).on('click', '#sendBtn', function () {
        flag = false;
        var msg = $('#notifyMessage').val().trim();
        var regex = /^[A-Za-z0-9 ]+$/;
        
        if (msg != null && msg != '' && msg != "") {
            sessionStorage.setItem("notifyMessage", msg);
            window.location.href = "/ManageNotifications/ManageNotification";
        } else {
            swal(
                'Error!',
                'Please enter description',
                'error'
            ).catch(swal.noop);
        }
    });

    //$(function () {
    //    $('#notifyMessage').on('keypress', function (e) {
    //        if (event.target.value.substr(-1) === ' ' && event.code === 'Space') {
    //            return false;
    //        }
    //    });
    //});

});