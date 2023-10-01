var employeeGuid = "";
var UserGuid = "";
var accessRequestId = 0;
$(document).ready(function () {
    $("#loader1").hide();
    // var id = sessionStorage.getItem("User1");
    UserGuid = sessionStorage.getItem("User1");
    getUrlVars();

    // Read a page's GET URL variables and return them as an associative array.


    $("#btnFundTransfer").click(function () {

        swal({
            title: "Are you sure you want to transfer the amount?",
            // text: "Warnning",
            icon: "warnning",
            buttons: false,
            showConfirmButton: true,
            showCancelButton: true,
            dangerMode: true,
        }, function succes(isDone) {
            $("#loader1").show();
            var formData = new FormData();
            formData.append("UserGuid", employeeGuid);
            formData.append("AdminGuid", UserGuid);
            formData.append("AccessAmountId", accessRequestId);
            $.ajax({
                type: "POST",
                cache: false,
                url: "/Withdrawals/ProvidusFundTransfer/",
                contentType: false,
                processData: false,
                data: formData,
                success: function (response) {
                    
                    $("#loader1").hide();
                    if (response.RstKey == 1) {
                        swal({
                            title: "Good job!",
                            text: "Amount has been transferred successfully.",
                            icon: "success",
                            button: "Aww yiss!"
                        });

                        window.location.reload();
                    } else if (response.RstKey == 3) {
                        swal({
                            title: "oops",
                            text: "You already transferd the fund.",
                            icon: "error",
                            button: "Aww yiss!"
                        });
                    } else if (response.RstKey == 31) {
                        swal({
                            title: "oops",
                            text: "There is a problem in receiver's account.May be it not selected as a salary account",
                            icon: "error",
                            button: "Aww yiss!"
                        });
                    }
                    else if (response.RstKey == 2) {
                        swal({
                            title: "oops!",
                            text: response.Message,
                            icon: "error",
                            button: "Aww yiss!"
                        });
                    }
                    else if (response.RstKey == 32) {
                        swal({
                            title: "oops!",
                            text: "Error!",
                            icon: "error",
                            button: "Aww yiss!"
                        });
                    }
                    else if (response.RstKey == 33) {
                        swal({
                            title: "oops!",
                            text: "Not authorized please login again and try",
                            icon: "error",
                            button: "Aww yiss!"
                        });
                    }
                }
            });
        })
    })

    $("#btnRejectFundTransfer").click(function () {

        swal({
            title: "Are you sure you want to reject this request?",
            // text: "Warnning",
            icon: "warnning",
            buttons: false,
            showConfirmButton: true,
            showCancelButton: true,
            dangerMode: true,
        }, function succes(isDone) {
            $("#loader1").show();
            var formData = new FormData();
            formData.append("UserGuid", employeeGuid);
            formData.append("AdminGuid", UserGuid);
            formData.append("AccessAmountId", accessRequestId);
            $.ajax({
                type: "POST",
                cache: false,
                url: "/Withdrawals/RejectSystemSpecsTransfer/",
                contentType: false,
                processData: false,
                data: formData,
                success: function (response) {

                    $("#loader1").hide();
                    if (response.RstKey == 1) {
                        swal({
                            title: "Good job!",
                            text: response.Message,
                            icon: "success",
                            button: "Aww yiss!"
                        });

                        window.location.reload();
                    } else if (response.RstKey == 2) {
                        swal({
                            title: "oops",
                            text: response.Message,
                            icon: "error",
                            button: "Aww yiss!"
                        });
                    } 
                }
            });
        })
    })
});

function getEwaEmployeeRequestDetail(employeeGuid, accessRequestId) {
    var formData = new FormData();
    formData.append("UserGuid", employeeGuid);
    formData.append("AccessAmountId", accessRequestId);

    $.ajax({
        type: "POST",
        cache: false,
        url: "/Withdrawals/GetEmployeeEarningDetailByUserGuid/",
        contentType: false,
        processData: false,
        data: formData,
        success: function (response) {
            
            if (response.accessAmountViewDetail.StatusId == 0) {
                $('#btnTransfer').prop('disabled', true);
            }
            if (response.accessAmountViewDetail.AdminStatus == 1) {
                $('#btnFundTransfer').prop('disabled', true);
            }
            

            if (response.RstKey == 1) {
                debugger
                if (response.accessAmountViewDetail.ProfileImage == "") {
                    $('.avtar').attr('src', '/Content/images/avatar.jpg');

                } else {
                    $('.avtar').attr('src', response.accessAmountViewDetail.ProfileImage);

                }
                // imgProfilePic
                $("#fullName").text(response.accessAmountViewDetail.FirstName + ' ' + response.accessAmountViewDetail.LastName);
                $("#email").text(response.accessAmountViewDetail.Email);
                $("#mobile").text(response.accessAmountViewDetail.CountryCode + ' ' + response.accessAmountViewDetail.PhoneNumber);
                $("#employerName").text(response.accessAmountViewDetail.EmployerName);
                $("#Hours").text(response.accessAmountViewDetail.Hours);
                $("#EarnedAmount").text('₦' + response.accessAmountViewDetail.EarnedAmount);
                $("#staffid").text(response.accessAmountViewDetail.StaffId);
                $("#AvailableAmount").text('₦' + response.accessAmountViewDetail.AvailableAmount);
                $("#AccessAmount").text('₦' + response.accessAmountViewDetail.AccessAmount);
                if (response.accessAmountViewDetail.StatusId == 1) {
                    $("#employerStatus").text(response.accessAmountViewDetail.Status);
                    $("#employerStatus").css('color', '#0CAF38');
                }
                else if (response.accessAmountViewDetail.StatusId == 2 || response.accessAmountViewDetail.StatusId == 0) {
                    $("#employerStatus").text(response.accessAmountViewDetail.Status);
                    $("#employerStatus").css('color', '#32A4FF');
                }
                else if (response.accessAmountViewDetail.StatusId == 3) {
                    $("#employerStatus").text(response.accessAmountViewDetail.Status);
                    $("#employerStatus").css('color', 'red');
                }
                else if (response.accessAmountViewDetail.StatusId == 6) {
                    $("#employerStatus").text(response.accessAmountViewDetail.Status);
                    $("#employerStatus").css('color', 'orange');
                }

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

function getUrlVars() {
    var vars = [], hash;
    var hashes = window.location.href.slice(window.location.href.indexOf('?') + 1).split('&');
    for (var i = 0; i < hashes.length; i++) {
        hash = hashes[i].split('=');
        vars.push(hash[0]);
        vars[hash[0]] = hash[1];
    }
    var data = hash[1];
    employeeGuid = data.split(',')[0];
    accessRequestId = data.split(',')[1];
    getEwaEmployeeRequestDetail(employeeGuid, accessRequestId)
    return vars;
}