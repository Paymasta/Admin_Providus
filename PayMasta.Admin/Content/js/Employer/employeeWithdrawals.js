var UserGuid = "";
var pageNumber = 1;
var PageSize = 10;
var totalPages = 1;
var searchText = '';
var AdminUserGuid = "";
var TotalRowCount = "";
var Month = "";
var AccessAmountId = '';
$("#noRecordFound").hide();

$(document).ready(function () {
    AdminUserGuid = sessionStorage.getItem("User1");
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
        getEmployeeWithdrawalProfile(UserGuid, AccessAmountId);
        getEmployeeWithdrawalsList(UserGuid, Month, pageNumber, PageSize);
        return vars;
    }
    $("#cars").on('change', function () {
        Month = $(this).val();
        TotalRowCount = '';
        getEmployeeWithdrawalsList(UserGuid, Month, pageNumber, PageSize);
    });
    $(document).on('click', '#backBtn', function () {
        window.href = window.history.back();
    });
});
function getEmployeeWithdrawalProfile(guid, AccesAmountId) {

    var formData = new FormData();
    formData.append("UserGuid", guid);
    
    $.ajax({
        type: "POST",
        cache: false,
        url: "/Employers/GetEmployeeEarningDetailByUserGuid",
        contentType: false,
        processData: false,
        data: formData,
        success: function (response) {

            $("#with_user_name").empty();
            $("#with_email ").empty();
            $("#with_working_days ").empty();
            $("#with_current_amount ").empty();
            $("#with_phone_number").empty();
            $("#with_staff_id").empty();
            $("#with_earned_wages").empty();
            if (response.RstKey == 1) {

                var value = response.employeeEwaDetail;
                $("#with_user_name").text(value.FirstName + ' '+ value.LastName);
                $("#with_email ").text(value.Email);
                $("#with_working_days ").text(value.WorkiingDays);
                $("#with_current_amount ").text('₦'+value.AvailableAmount);
                $("#with_phone_number").text(value.CountryCode + ' ' + value.PhoneNumber);
                $("#with_staff_id").text(value.StaffId);
                $("#with_earned_wages").text('₦'+value.EarnedAmount);

            } else {
                console.log('empty data');
                /*swal({
                    title: "oops!",
                    text: response.Message,
                    icon: "error",
                    button: "ohh no!"
                });*/
            }
        }
    });

}
function getEmployeeWithdrawalsList(UserGuid, Month, pageNumber, PageSize) {

    var formData = new FormData();
    formData.append("UserGuid", UserGuid);
    formData.append("Month", Month);
    formData.append("pageNumber", pageNumber);
    formData.append("PageSize", PageSize);

    $.ajax({
        type: "POST",
        cache: false,
        url: "/Employers/GetEmployeesWithdrwals/",
        contentType: false,
        processData: false,
        data: formData,
        success: function (response) {
            $('#employeeWithdrawals_List').empty();
            let employerList = '';

            if (response.RstKey == 1) {
                $("#noRecordFound").hide();
                $(response.employeesWithdrawls).each(function (index, value) {
                    TotalRowCount = value.TotalCount;
                    let created_date = new Date(value.CreatedAt);
                    let dateFormat = moment(created_date).format('MMMM/DD/YYYY');
                    employerList += '<tr>';
                    employerList += '<td>' + value.RowNumber + '</td>';
                    employerList += '<td>' + '₦' + value.AccessAmount + '</td>';
                    
                    if (value.Status == 'Hold') { employerList += '<td class="hold"  style="color:orange">' + value.Status + '</td>'; }
                    else if (value.Status == 'Approved') { employerList += '<td class="closed"  style="color:green">' + value.Status + '</td>'; }
                    else if (value.Status == 'Pending') { employerList += '<td class="pending" style="color:blue">' + value.Status +'</td>'; }
                    else if (value.Status == 'Rejected') { employerList += '<td class="reject"  style="color:red">' + value.Status + '</td>'; }
                    else { employerList += '<td class="">' + value.Status + '</td>'; }

                    if (value.AdminStatus == 'Hold') { employerList += '<td class="hold" style="color:orange">' + value.AdminStatus + '</td>'; }
                    else if (value.AdminStatus == 'Approved') { employerList += '<td class="closed" style="color:green">' + value.AdminStatus + '</td>'; }
                    else if (value.AdminStatus == 'Pending') { employerList += '<td class="pending" style="color:blue">' + value.AdminStatus +'</td>'; }
                    else if (value.AdminStatus == 'Rejected') { employerList += '<td class="reject" style="color:red">' + value.AdminStatus + '</td>'; }
                    else { employerList += '<td class="">' + value.AdminStatus + '</td>'; }

                    employerList += '<td>' + dateFormat + '</td>';
                    employerList += '</tr>';
                });
                $('#employeeWithdrawals_List').append(employerList);
                page_number(TotalRowCount, pageNumber);

            } else {
                $("#noRecordFound").show();
            }
        }
    });

}
function page_number(count, currentPage) {
    $('#employeeWithdrawal_pagination').pagination({
        total: count,
        current: currentPage,
        length: 10,
        size: 2,
        /**
         * [click description]
         * @param  {[object]} options = {
         *      current: options.current,
         *      length: options.length,
         *      total: options.total
         *  }
         * @param  {[object]} $target [description]
         * @return {[type]}         [description]
         */
        click: function (options, $target) {
            console.log(options);
            pageNumber = options.current;
            getEmployeeWithdrawalsList(UserGuid, Month, pageNumber, PageSize);
            
        }
    });
}