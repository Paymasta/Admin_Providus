$(document).ready(function () {

    var UserGuid = "";
    var pageNumber = 1;
    var PageSize = 10;
    var totalPages = 1;
    var searchText = '';
    var AdminUserGuid = "";

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

    $('body').on('click', '.deactivate-btn', function (e) {
        e.preventDefault;
        var msg = $(this).attr('data-msg'); 
        swal({
            title: "Are you sure you want to "+msg+" this user?",
        
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
    $('body').on('click', '#remove_btn', function (e) {
        e.preventDefault;

        swal({
            title: "Are you sure you want to remove this employee?",
       
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
                    if (response.RstKey == 1) {

                        swal({
                            title: "Good job!",
                            text: response.Message,
                            icon: "success",
                            button: "Aww yiss!"
                        }, function succes(isDone) { window.location.href = "/User"; });
                    } else {
                        swal({
                            title: "oops!",
                            text: "User not deleted !",
                            icon: "error",
                            button: "ohh no!"
                        });
                    }
                }
            });
        })

    });
    $('body').on('click', '#withdrawals_btn', function (e) {
        e.preventDefault;
        window.location.href = "/User/GetEmployeeWithdrawals/id=" + UserGuid;
    });
});

function getEmployeeProfile(guid) {
    var formData = new FormData();
    /*formData.append("EmployerGuid", guid);*/
    formData.append("id", guid);
    // formData.append("EmployeerUserGuid", UserGuid);

    $.ajax({
        type: "POST",
        cache: false,
        url: "/User/GetEmployeDetailByGuid/",
        contentType: false,
        processData: false,
        data: formData,
        success: function (response) {
            
            if (response.RstKey == 1) {

                var value = JSON.stringify(response.employeeDetail.FirstName);
                console.log("First name " + response.employeeDetail.FirstName);
                $("#firstName").text(response.employeeDetail.FirstName);
                $("#lastName").text(response.employeeDetail.LastName);
                $("#email").text(response.employeeDetail.Email);
                $("#countryCode").text(response.employeeDetail.CountryCode);
                $("#phoneNumber").text(response.employeeDetail.PhoneNumber);

                let date_of_birth = new Date(response.employeeDetail.DateOfBirth);
                let dob = moment(date_of_birth).format('MMMM/DD/YYYY');

                $("#DateOfBirth").text(dob);
                $("#Gender").text(response.employeeDetail.Gender);
                $("#Address").text(response.employeeDetail.Address);
                $("#CountryName").text(response.employeeDetail.CountryName);
                $("#State").text(response.employeeDetail.State);
                $("#City").text(response.employeeDetail.City);
                $("#PostalCode").text(response.employeeDetail.PostalCode);
                $("#EmployerName").text(response.employeeDetail.EmployerName);
                $("#StaffId").text(response.employeeDetail.StaffId);
                $("#NetPay").text(response.employeeDetail.NetPay);
                $("#GrossPay").text(response.employeeDetail.GrossPay);
                $("#BankName").text(response.employeeDetail.BankName);
                $("#AccountNumber").text(response.employeeDetail.AccountNumber);
                $("#BVN").text(response.employeeDetail.BVN);
                $("#BankAccountHolderName").text(response.employeeDetail.BankAccountHolderName);
                $("#Status").text(response.employeeDetail.Status);
                if (response.employeeDetail.Status == 'Active') { $("#unblock_btn").hide(); $("#block_btn").show(); $("#Status").addClass('active'); }
                else if (response.employeeDetail.Status == 'Inactive') { $("#block_btn").hide(); $("#unblock_btn").show(); $("#Status").addClass('inActive');}

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

