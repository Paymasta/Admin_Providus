var UserGuid = "";
var pageNumber = 1;
var PageSize = 10;
var totalPages = 1;
var searchText = '';
var AdminUserGuid = "";
var AccessAmountId = ""

$(document).ready(function () {

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
        getEmployeeProfile(UserGuid, AdminUserGuid)
        return vars;
    }

    $("#employeeWithdrawals").on('click', function (e) {
        window.location.href = "/Employers/GetEmployeeWithdrawals/id=" + UserGuid;
    });
    $("#user_salary_structure").on('click', function (e) {
        window.location.href = "/Employers/GetEmployeeSalaryStructure/id=" + UserGuid;
    });
    $(document).on('click', '#backBtn', function () {
        window.href = window.history.back();
    });
});
function getEmployeeProfile(guid, AdminUserGuid) {

    var formData = new FormData();
    formData.append("id", guid);
    formData.append("AdminGuid", AdminUserGuid);
    $.ajax({
        type: "POST",
        cache: false,
        url: "/Employers/GetEmployeDetailByGuid",
        contentType: false,
        processData: false,
        data: formData,
        success: function (response) {
            
            $("#first_name").empty();
            $("#middle_name ").empty();
            $("#last_name ").empty();
            $("#email ").empty();
            $("#phone_number").empty();
            $("#dob").empty();
            $("#gender").empty();
            $("#staff_id").empty();
            $("#net_pay").empty();
            $("#gross_pay").empty();
            $("#profileImg").empty();
            if (response.RstKey == 1) {
                
                var value = response.employeeDetail;
                let date_of_birth = new Date(value.DateOfBirth);
                let Dob = moment(date_of_birth).format('MMMM/DD/YYYY');

                if (value.ProfileImage === null || value.ProfileImage == "") {
                    $("#profileImg").attr('src', "/Content/img/images/man-placeholder.png");   
                }
                else {
                    $("#profileImg").attr('src', value.ProfileImage);
                }
                $("#first_name").text(value.FirstName);
                $("#middle_name ").text();
                $("#last_name ").text(value.LastName);
                $("#email ").text(value.Email);
                $("#phone_number").text(value.CountryCode + ' ' + value.PhoneNumber);
                $("#dob").text(Dob);
                $("#gender").text(value.Gender);
                $("#staff_id").text(value.StaffId);
                $("#net_pay").text(value.NetPay);
                $("#gross_pay").text(value.GrossPay);

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
