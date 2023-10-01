var UserGuid = "";
var pageNumber = 1;
var PageSize = 10;
var totalPages = 1;
var searchText = '';
var AdminUserGuid = "";

$(document).ready(function () {
    $("#unblock_user").hide();
    $("#block_user").hide();
    /*$("#resolve_pop_btn").prop("disabled", true);
    $("#reject_pop_btn").prop("disabled", true);
    $("#hold_pop_btn").prop("disabled", true);*/
    /*$("#resolve_pop_btn").hide();
   $("#reject_pop_btn").hide();
   $("#hold_pop_btn").hide();*/
    AdminUserGuid = sessionStorage.getItem("User1");
    getUrlVars();
    // Read a page's GET URL variables and return them as an associative array.
    function getUrlVars() {
        var vars = [], hash;
        var ticketId, uId;
        var hashes = window.location.href.slice(window.location.href.indexOf('?') + 1).split('&');
        for (var i = 0; i < hashes.length; i++) {
            hash = hashes[i].split('=');
            var ids = hash[1].split(',');
            uId = ids[0];
            ticketId = ids[1];
            vars.push(hash[0]);
            vars[hash[0]] = hash[1];
        }
        UserGuid = hash[1];
        getEmployeeProfile(uId, ticketId)
        return vars;
    }
    $('body').on('click', '#delete_user', function (e) {
        e.preventDefault;

        swal({
            title: "Are you sure you want to delete this user?",
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
                        });
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

    $(document).on('click', '#backBtn', function () {
        window.href = window.history.back();
    });

});
$(document).on('click', '#btn_resolve', function (e) {
    e.preventDefault;
    debugger
    var TicketId = $('#btn_resolve').attr('data-ticket');
    var Status = '3';

    var formData = new FormData();
    formData.append("AdminUserGuid", AdminUserGuid);
    formData.append("TicketId", TicketId);
    formData.append("Status", Status);

    $.ajax({
        type: "POST",
        cache: false,
        url: "/Support/UpdateSupportTicketStatus",
        contentType: false,
        processData: false,
        data: formData,
        success: function (response) {
            if (response.RstKey == 1) {
                swal({
                    title: "Good job!",
                    text: response.Message,
                    icon: "success",
                    button: "Aww yiss!"
                }, function succes(isDone) { location.reload(); });
            } else {
                swal({
                    title: "oops!",
                    text: "Request not resolved !",
                    icon: "error",
                    button: "ohh no!"
                });
            }
        }
    });
});

$(document).on('click', '#btn_delete', function (e) {
    e.preventDefault;
    var TicketId = $('#btn_delete').attr('data-ticket');
    var Status = '5';

    var formData = new FormData();
    formData.append("AdminUserGuid", AdminUserGuid);
    formData.append("TicketId", TicketId);
    formData.append("Status", Status);

    $.ajax({
        type: "POST",
        cache: false,
        url: "/Support/UpdateSupportTicketStatus",
        contentType: false,
        processData: false,
        data: formData,
        success: function (response) {
            if (response.RstKey == 1) {
                swal({
                    title: "Good job!",
                    text: response.Message,
                    icon: "success",
                    button: "Aww yiss!"
                }, function succes(isDone) { location.reload(); });
            } else {
                swal({
                    title: "oops!",
                    text: "Request not rejected !",
                    icon: "error",
                    button: "ohh no!"
                });
            }
        }
    });

});

$(document).on('click', '#btn_hold', function (e) {
    e.preventDefault;
    var TicketId = $('#btn_hold').attr('data-ticket');
    var Status = '4';

    var formData = new FormData();
    formData.append("AdminUserGuid", AdminUserGuid);
    formData.append("TicketId", TicketId);
    formData.append("Status", Status);

    $.ajax({
        type: "POST",
        cache: false,
        url: "/Support/UpdateSupportTicketStatus",
        contentType: false,
        processData: false,
        data: formData,
        success: function (response) {
            if (response.RstKey == 1) {
                swal({
                    title: "Good job!",
                    text: response.Message,
                    icon: "success",
                    button: "Aww yiss!"
                }, function succes(isDone) { location.reload(); });
            } else {
                swal({
                    title: "oops!",
                    text: "Request not hold !",
                    icon: "error",
                    button: "ohh no!"
                });
            }
        }
    });

});

function getEmployeeProfile(guid,tId) {
    var formData = new FormData();
    formData.append("UserGuid", guid);
    formData.append("SupportTicketId", tId);
    $.ajax({
        type: "POST",
        cache: false,
        url: "/Support/ViewSupportTicketDetail",
        contentType: false,
        processData: false,
        data: formData,
        success: function (response) {
            console.log(response);
            if (response.RstKey == 1) {
                var value = response.supportViewModel;
                let created_date = new Date(value.CreatedAt);
                let dateFormat = moment(created_date).format('MMMM/DD/YYYY');

                $("#ticket_number").text(value.TicketNumber);
                $("#username").text(value.FirstName + ' ' + value.LastName);
                $("#email").text(value.Email);
                $("#email").attr('title', value.Email);
                $("#date").text(dateFormat);
                $("#title").text(value.Title);
                
                if (value.StatusId ==3) {
                    $("#status").addClass('closed');
                    //$("#resolve_pop_btn").prop("disabled", true);
                    //$("#reject_pop_btn").prop("disabled", true);
                    //$("#hold_pop_btn").prop("disabled", true);
                    $("#resolve_pop_btn").hide();
                    $("#reject_pop_btn").hide();
                    $("#hold_pop_btn").hide();
                }
                else if (value.StatusId == 0) {
                    $("#status").addClass('pending');
                    //$("#resolve_pop_btn").prop("disabled", false);
                    //$("#reject_pop_btn").prop("disabled", false);
                    //$("#hold_pop_btn").prop("disabled", true);
                   
                    $("#resolve_pop_btn").removeClass('d-none');
                    $("#reject_pop_btn").removeClass('d-none');
                    $("#hold_pop_btn").removeClass('d-none');
                }
                else if (value.StatusId == 5) {
                    $("#status").addClass('reject');
                    //$("#resolve_pop_btn").prop("disabled", true);
                    //$("#reject_pop_btn").prop("disabled", true);
                    //$("#hold_pop_btn").prop("disabled", true);
                    $("#resolve_pop_btn").hide();
                    $("#reject_pop_btn").hide();
                    $("#hold_pop_btn").hide();
                }
                else if (value.StatusId == 4) {
                    $("#status").addClass('hold');
                    //$("#resolve_pop_btn").prop("disabled", false);
                    //$("#reject_pop_btn").prop("disabled", false);
                    //$("#hold_pop_btn").prop("disabled", true);

                    $("#resolve_pop_btn").removeClass('d-none');
                    $("#reject_pop_btn").removeClass('d-none');
                    //$("#hold_pop_btn").hide();
                }
                
                $("#status").text(value.Status);
                $("#description").text(value.DescriptionText);
                $("#description").attr('title', value.DescriptionText);
                $("#btn_resolve").attr('data-ticket', value.Id);
                $("#btn_delete").attr('data-ticket', value.Id);
                $("#btn_hold").attr('data-ticket', value.Id);

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